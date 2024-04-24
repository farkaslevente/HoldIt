const { config } = require("dotenv");
config();
const {DB_user, DB_host, DB_pwd, DB_name} = process.env

const conf = {
    db: {
        host: DB_host,
        user: DB_user,
        password: DB_pwd, 
        database: DB_name,
        connectTimeout: 60000
    }
};

module.exports = conf;