const jwt = require('jsonwebtoken')
const { dbFunctions } = require('../database/dbFunc')

    isAdmin = async (req, res, next) => {
        const token = req.headers['authorization'];

        if (!token) {
            return res.status(403).send({ message: "Invalid token" });
        } else if (!token.startsWith('Bearer ')) {
            return res.status(403).send({ message: "Not a Bearer token" });
        }
        const user = jwt.decode(token.slice(7, token.length))
        const compare = await dbFunctions.execQueryWithReturn(`SELECT * from felhasznalok WHERE id = ${user.payload.id}`) || []
        if (compare[0].szerep === 1) next()
        else if (compare[0].szerep === 0 || compare[0].szerep === null) return res.status(401).json({message: "Unathorized"})
        else {
            return res.status(404).json({message: "User not found"})
        }

    }


module.exports = {
    isAdmin
}