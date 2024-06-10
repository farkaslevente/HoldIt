const query = require("../database/db");
const { dbFunctions } = require("../database/dbFunc");

const postController = {
  getUploads: async function (res) {
    try {
      res = await query(`
            SELECT * FROM uploads`);
      return res;
    } catch (err) {
      console.error("Error getting!", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },

  postUpload: async function (req, res, id) {
    try {
      const { imgUrl, comment, ownerId,isPrivate, targetId } = req.body;
      const d = new Date();
      await query(`INSERT INTO uploads (id, imgUrl, time, comment, ownerId, isPrivate, targetId) VALUES 
            (null, '${imgUrl}', '${d}', '${comment}','${ownerId}','${isPrivate}','${targetId}')`);
      res.status(200).json({ message: "Upload successfully uploaded!" });
    } catch (err) {
      console.error("Error posting ads!", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },

  editUpload: async function (req, res, userId, id) {
    try {
      const { imgUrl, comment } = req.body;

      const rows =
        (await dbFunctions.execQueryWithReturn(`
            SELECT * FROM uploads WHERE id = '${id}'`)) || [];
      const post = rows[0];
      if (post === undefined) {
        return res.status(404).json({ message: "Ad not found" });
      }
      if (ad.tulajId == userId) {
        const d = new Date();
        await query(
          `UPDATE uploads SET imgUrl = '${imgUrl}', time = '${d}', comment = '${comment}' WHERE id = '${id}'`
        );
        res.status(200).json({ message: "Upload successfully updated!" });
      } else {
        return res.status(403).json({ message: "Unathorized action" });
      }
    } catch (err) {
      console.error("Error posting ads!", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },

  deleteUpload: async function (req, res, userId, id) {
    console.log("Incoming delete on ads...");
    try {
      const rows =
        (await dbFunctions.execQueryWithReturn(`
            SELECT * FROM uploads WHERE id = '${id}'`)) || [];
      const ad = rows[0];
      console.log(ad)
      if (ad === undefined) {
        return res.status(404).json({ message: "Ad not found" });
      }
      if (ad.ownerId == userId) {
        await query(`
                DELETE FROM uploads WHERE id = ${id}`);
        return res
          .status(200)
          .json({ message: `Upload with id: ${id} was deleted succesfully` });
      } else {
        return res.status(403).json({ message: "Unathorized action" });
      }
    } catch (err) {
      console.error("Error deleting!", err.message);
    }
  },

  removeUpload: async function (req, res, id) {
    console.log("Incoming remove on ads...");
    try {
      await query(`
            DELETE FROM uploads WHERE id = ${id}`);
      return res
        .status(200)
        .json({ message: `Upload with id: ${id} was deleted succesfully` });
    } catch (err) {
      console.error("Error deleting!", err.message);
    }
  },

  modifyAd: async function (req, res, id) {
    try {
      const { comment } = req.body;
      const d = new Date();
      await query(
        `UPDATE uploads SET comment = '${comment}', datum = '${d}' WHERE id = '${id}'`
      );
      res.status(200).json({ message: "Ad successfully updated!" });
    } catch (err) {
      console.error("Error posting ads!", err.message);
      res.status(500).json({ error: "Internal server error!" });
    }
  },
};

module.exports = {
  postController,
};
