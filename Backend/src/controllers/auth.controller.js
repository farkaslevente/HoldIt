const query = require('../database/db')
const { dbFunctions } = require('../database/dbFunc')
const { verifyToken, accessToken, refreshToken, compareToken } = require('../middlewares/jwtMiddleware')
const bcrypt = require('bcryptjs')
const crypto = require('crypto')
const { emailController } = require('../controllers/email.controller')


const authController = {
    register: async function (req, res) {
        console.log('Register incoming...', req.body)
        const { name, email, location, password, phone } = req.body;

        if (name, email, location, password, phone) {
            try {
                const rows = await dbFunctions.execQueryWithReturn(
                    `SELECT * FROM felhasznalok WHERE email = '${email}'`) || []
                if (!rows || rows.length === 0) {

                    const hashedPassword = await bcrypt.hash(password, 10)

                    dbFunctions.execQueryRegister(`INSERT INTO felhasznalok (id, nev, email, hely, pPic, jelszo, szerep, kedvencek, telefonszam) VALUES
                    (null, '${name}', '${email}', '${location}', "https://www.svgrepo.com/show/442075/avatar-default-symbolic.svg", '${hashedPassword}', 0, '0 +', '${phone}')`)

                    res.status(200).json({
                        message: "Successful registration!"
                    })
                    
                }
                else {
                    return res.status(403).json({message: "User already exists!"})
                }
            } catch (err) {
                console.log(err.message);
                return res.status(500).send("Error in register!");
            }

        }
        else {
            return res.status(400).json({error: "Bad request"})
        }
    },


    login: async function (req, res) {
        console.log("Incoming login:", req.body)
        try {
        const {email, password} = req.body;
        if (!email || !password) return res.status(400).json({ message: 'Username and password are required.' });

        const rows = await dbFunctions.execQueryWithReturn(
        `SELECT * FROM felhasznalok WHERE email = '${email}'`) || [];

        if (rows == [] || !rows || rows.length === 0) {
            return res.status(401).json({error: "Invalid email or password"})
        }
        
        let isPasswordValid = false
        const user = rows[0]
        if (password) { 
            isPasswordValid = await bcrypt.compare(password, user.jelszo)
            if (!isPasswordValid) {
            return res.status(401).json({"message": "Invalid email or password"})
            }
            else {
                const payload = {
                    id: user.id,
                    name: user.nev,
                    email: user.email,
                    location: user.hely,
                    pPic: user.pPic,
                    role: user.szerep,
                    favourites: user.kedvencek,
                    phone: user.telefonszam
                }
                const token = accessToken({payload})

                const d = new Date()
                dbFunctions.execQueryRegister(`INSERT INTO tokenek (id, token, datum, tulajEmail) VALUES 
                (null, '${token}', '${d}', '${user.email}')`)

                req.session.token = token;
                res.status(200).json({token})
            }
        }
        else {
            return res.status(400).json({"message": "Bad request"})
        }
        
        } catch (err) {
            console.error("Error logging in!", err.message);
        }
    },

    acceptToken: async function (req,res) {
        console.log("Incoming tokenverification...", req.body)
            return verifyToken(req, res)
    },

    resetPassword: async function (req, res) {
        console.log("Password reset incoming...")
        const {email} = req.body

        const rows = await dbFunctions.execQueryWithReturn(
            `SELECT * FROM felhasznalok WHERE email = '${email}'`) || [];
        if (rows == [] || !rows || rows.length === 0) {
            return res.status(404).json({error: "This email is not registered to an account"})
        }
            

        let resetToken = crypto.randomBytes(4).toString('hex').toUpperCase()
        emailController.sendResetPassword(req,res,email,resetToken)

        let dbToken =  await bcrypt.hash(resetToken, 10)
        let token = refreshToken({token: dbToken})

        await query(`
        INSERT INTO tokenek (token, datum, id, tulajEmail) VALUES ('${token}', '${Date.now()}', null, '${email}')`)
    },

    authorizeReset: async function (req,res) {
        console.log("Authorizing reset...")
        const {token, email} = req.body

        const rows = await dbFunctions.execQueryWithReturn(`
        SELECT * from tokenek WHERE tulajEmail = '${email}'`)
        const dbToken = rows[rows.length - 1]
        
        const user = await dbFunctions.execQueryWithReturn(
            `SELECT * FROM felhasznalok WHERE email = '${email}'`) || [];

            console.log(user[0])

        compareToken(res, token, dbToken.token, user[0])
        
    }


}

module.exports = {
    authController
}
