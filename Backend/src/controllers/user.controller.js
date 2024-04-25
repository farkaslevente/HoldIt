const query = require("../database/db");
const { dbFunctions } = require("../database/dbFunc");
const bcrypt = require("bcryptjs");

const userController = {
  putUsers: async function (req, res, id) {
    console.log("Patching incoming...", req);
    try {
      const { name, email, pPic } = req;

      const rows =
        (await dbFunctions.execQueryWithReturn(
          `SELECT * FROM users WHERE id = '${id}'`
        )) || [];
      const hashed = rows[0].pwd;
      const followed = rows[0].followed;
      const role = rows[0].role;

      await query(`UPDATE felhasznalok SET name= '${name}', email= '${email}', pPic= '${pPic}', pwd= '${hashed}',
            followed= '${followed}', role= '${role}' WHERE id=${id}`);

      res.status(200).json({ message: "User patched succesfully!" });
    } catch (err) {
      console.error("Error posting!", err.message);
    }
  },

  deleteUsers: async function (req, res, id) {
    console.log("Incoming delete on users...", req);
    try {
      await query(`
            DELETE FROM users WHERE id = ${id}`);
      await query(`
            DELETE from uploads WHERE ownerId = ${id}`);
      // delete from upload
      return res
        .status(200)
        .json({ message: `User with id: ${id} was deleted succesfully` });
    } catch (err) {
      console.error("Error deleting!", err.message);
    }
  },

  changePicture: async function (req, res, id) {
    console.log("Incoming PP change...", req.body);
    try {
      const { pPic } = req.body;
      const rows =
        (await dbFunctions.execQueryWithReturn(
          `SELECT * FROM users WHERE id = ${id}`
        )) || [];
      user = rows[0];
      await query(`UPDATE users SET name= '${user.name}', email= '${user.email}', pPic= '${pPic}', pwd= '${user.pwd}',
            followed= '${user.followed}', role= '${user.role}' WHERE id=${id}`);
      res.status(200).json({ message: "Updating profile was successful" });
    } catch (err) {
      console.error("Error changing pic!", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },

  updateFollowed: async function (req, res, id) {
    console.log("Adding favourite...", req.body);
    try {
      const { userId } = req.body;

      const rows =
        (await dbFunctions.execQueryWithReturn(
          `SELECT * FROM users WHERE id = ${id}`
        )) || [];
      user = rows[0];
      const newFollowed = user.followed + " " + userId + " +";

      await query(`
            UPDATE users SET name= '${user.name}', email= '${user.email}', pPic= '${user.pPic}', pwd= '${user.pwd}',
            followed= '${newFollowed}', role= '${user.role}' WHERE id=${id}`);
      res.status(200).json({ message: "Ad marked as saved" });
    } catch (err) {
      console.error("Error updating favourites...", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },

  removeFollow: async function (req, res, id) {
    console.log("Removing favourite...", req.body);
    try {
      const { userId } = req.body;
      const rows =
        (await dbFunctions.execQueryWithReturn(
          `SELECT * FROM users WHERE id = ${id}`
        )) || [];
      user = rows[0];
      const newFollowed = (user.followed + "").replace(" " + userId + " +", "");

      await query(`
            UPDATE users SET name= '${user.name}', email= '${user.email}', pPic= '${user.pPic}', pwd= '${user.pwd}',
            followed= '${newFollowed}', role= '${user.role}' WHERE id=${id}`);
      res.status(200).json({ message: "Ad removed from saved" });
    } catch (err) {
      console.error("Error updating favourites...", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },

  newPassword: async function (req, res, id) {
    console.log("Updating password...", req.body);
    try {
      const { password } = req.body;
      const rows =
        (await dbFunctions.execQueryWithReturn(
          `SELECT * FROM users WHERE id = ${id}`
        )) || [];
      user = rows[0];

      const hashedPassword = await bcrypt.hash(password, 10);
      await query(`
            UPDATE users SET name= '${user.name}', email= '${user.email}', pPic= '${user.pPic}', pwd= '${hashedPassword}',
            followed= '${user.followed}', role= '${user.role}' WHERE id=${id}`);
      res.status(200).json({ message: "Password changed successfully" });
    } catch (err) {
      console.error("Error updating password...", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },

  support: async function (req, res, id) {
    console.log("Incoming question...", req.body);
    try {
      const { title, question } = req.body;
      await query(`
            INSERT INTO support (id, title, comment, userId) VALUES (null, '${title}', '${question}', '${id}')`);
      res.status(200).json({
        message: "We received your message, we'll repsond as fast as possible.",
      });
    } catch (err) {
      console.error("Error contacting support...", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },

  editUsers: async function (req, res) {
    console.log("Editing user incoming...", req.body);
    try {
      const { id, name, email, pPic, followed, role } = req.body;

      const rows =
        (await dbFunctions.execQueryWithReturn(
          `SELECT * FROM users WHERE id = '${id}'`
        )) || [];
      const hashed = rows[0].pwd;

      await query(`UPDATE users SET name= '${name}', email= '${email}', pPic= '${pPic}', pwd= '${hashed}',
                   followed= '${followed}', role= '${role}' WHERE id=${id}`);

      res.status(200).json({ message: "User edited succesfully!" });
    } catch (err) {
      console.error("Error posting!", err.message);
    }
  },

  removeUsers: async function (req, res, id) {
    console.log("Incoming delete on users...", req);
    try {
      await query(`
            DELETE FROM users WHERE id = ${id}`);
      await query(`
            DELETE from uploads WHERE ownerId = ${id}`);
      // delete from upload
      return res
        .status(200)
        .json({ message: `User with id: ${id} was deleted succesfully` });
    } catch (err) {
      console.error("Error deleting!", err.message);
    }
  },
};

module.exports = {
  userController,
};
