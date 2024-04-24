const { config } = require("dotenv")
config();
module.exports = {
    hosts: {
        HOST: process.env.HOST
    }
}