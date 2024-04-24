const conf = require('../config/db.config');
const { createConnection } = require("mysql2/promise");

async function query(sql, params) {
    const con = await createConnection(conf.db);
    const [results] = await con.execute(sql, params);

    return results;
}

module.exports = query