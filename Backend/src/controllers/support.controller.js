const query = require('../database/db')

const supportController = {
    
    getSupport: async function (res) {
        try {
            res = await query(`
            SELECT * FROM support`)
            return res
        } catch (err) {
            console.error("Error getting!", err.message);
            res.status(500).json({error: "Internal server error!"})
        }
    },

    deleteSupport: async function (req,res,id) {
        try {
            await query(`
            DELETE FROM support WHERE id = '${id}'`)
            return res.status(200).json({message: `Support id:${id} was deleted succesfully`})
        } catch (err) {
            console.error("Error getting!", err.message);
            res.status(500).json({error: "Internal server error!"})
        }
    },
}

module.exports = {
    supportController
}