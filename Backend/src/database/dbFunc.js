const query = require("./db");

const dbFunctions = {
  getUsers: async function (res) {
    res = await query(
      `SELECT *
        FROM users`
    );
    return res;
  },

  getUserById: async function (req) {
    const id = req.params.id;
    res = await query(`
        SELECT * FROM users WHERE id = '${id}'`);

    const payload = {
      id: res[0].id,
      name: res[0].name,
      email: res[0].email,
      pPic: res[0].pPic,
      followed: res[0].followed,
    };
    return payload;
  },

  getPictures: async function (res) {
    res = await query(`
        SELECT * FROM pictures`);
    return res;
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
};
module.exports = {
  dbFunctions,
};
