const query = require('./db')

const dbFunctions = {
    getUsers: async function (res) {
        res = await query(
        `SELECT *
        FROM felhasznalok`
        );
        return res;
    },

    getUserById: async function (req) {
        const id = req.params.id
        res = await query(`
        SELECT * FROM felhasznalok WHERE id = '${id}'`)

        const payload = {
            id: res[0].id,
            name: res[0].nev,
            email: res[0].email,
            location: res[0].hely,
            pPic: res[0].pPic,
            favourites: res[0].kedvencek,
            phone: res[0].telefonszam,
            role: res[0].szerep
        }
        return payload
    },

    getPictures: async function(res) {
        res = await query(`
        SELECT * FROM kepek`)
        return res
    },

    getSettlements: async function(res) {
        res = await query(`
        SELECT * FROM telepulesek`)
        return res
    },

    getCounties: async function(res) {
        res = await query(`
        SELECT * FROM varmegyek`)
        return res
    },   
        execQueryWithReturn: async function (req) {
            try {
                const result = await query(req);
                return Array.from(result);
            } catch (error) {
                console.error("Error executing query:", error);
                throw error;
            }
        },
    
        execQueryRegister: async function (req) {
            try {
                query(req);
            } catch (error) {
                console.error("Error executing query:", error);
                throw error;
            }
        },

    deleteToken: async function (req, res, id) {
        console.log("Delete incoming...")
        try {
            await query(`
            DELETE FROM tokenek WHERE id = ${id}`)
            return res.status(200).json({message: `Token id:${id} was deleted succesfully`})
        } catch (err) {
            console.error("Error deleting!", err.message);
            res.status(500).json({error: "Internal server error!"})
        }

    },

    getTokens: async function (res) {
        try {
            res = await query(`
            SELECT * FROM tokenek`)
            return res
        } catch (err) {
            console.error("Error getting!", err.message);
            res.status(500).json({error: "Internal server error!"})
        }
    },

}
module.exports = {
    dbFunctions
}
