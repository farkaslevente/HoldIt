const { config } = require("dotenv");
config();
const {DB_local_user, DB_local_host, DB_local_pwd,DB_name} = process.env

const conf = {
    db: {
        host: DB_local_host,
        user: DB_local_user,
        password: DB_local_pwd, 
        database: DB_name,
        connectTimeout: 60000
    }
};

module.exports = conf;