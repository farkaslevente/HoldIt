const express = require("express");
const { dbFunctions } = require("../database/dbFunc");
const { authController } = require("../controllers/auth.controller");
const { userController } = require("../controllers/user.controller");
const { isAdmin } = require("../controllers/role.controller");
const { verifyToken } = require("../middlewares/jwtMiddleware");
const { emailController } = require("../controllers/email.controller");
const { uploadController } = require("../controllers/upload.controller");
const { postController } = require("../controllers/post.controller");
const router = express.Router();
const { supportController } = require("../controllers/support.controller");

//• ▌ ▄ ·. ▄• ▄▌▄▄▌  ▄▄▄▄▄▄▄▄ .▄▄▄
//·██ ▐███▪█▪██▌██•  •██  ▀▄.▀·▀▄ █·
//▐█ ▌▐▌▐█·█▌▐█▌██▪   ▐█.▪▐▀▀▪▄▐▀▀▄
//██ ██▌▐█▌▐█▄█▌▐█▌▐▌ ▐█▌·▐█▄▄▌▐█•█▌
//▀▀  █▪▀▀▀ ▀▀▀ .▀▀▀  ▀▀▀  ▀▀▀ .▀  ▀

const multer = require("multer");
const storage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, "uploads");
  },
  filename: function (req, file, cb) {
    cb(null, file.originalname);
  },
});
const upload = multer({ storage: storage }).array("file", 6);

//▄▄▄        ▄• ▄▌▄▄▄▄▄▄▄▄ ..▄▄ ·
//▀▄ █·▪     █▪██▌•██  ▀▄.▀·▐█ ▀.
//▐▀▀▄  ▄█▀▄ █▌▐█▌ ▐█.▪▐▀▀▪▄▄▀▀▀█▄
//▐█•█▌▐█▌.▐▌▐█▄█▌ ▐█▌·▐█▄▄▌▐█▄▪▐█
//.▀  ▀ ▀█▄▀▪ ▀▀▀  ▀▀▀  ▀▀▀  ▀▀▀▀

router.get("/", async function (_req, res, next) {
  try {
    res.render("index.html");
  } catch (err) {
    console.error("Error while loading in the main page!", err.message);
    next(err);
  }
});

router.get("/pictures", [verifyToken], async function (_req, res, next) {
  try {
    res.json(await dbFunctions.getPictures());
  } catch (err) {
    console.error("Error while getting pictures!", err.message);
    next(err);
  }
});

//            ▄• ▄▌.▄▄ · ▄▄▄ .▄▄▄  .▄▄ ·
//            █▪██▌▐█ ▀. ▀▄.▀·▀▄ █·▐█ ▀.
//            █▌▐█▌▄▀▀▀█▄▐▀▀▪▄▐▀▀▄ ▄▀▀▀█▄
//            ▐█▄█▌▐█▄▪▐█▐█▄▄▌▐█•█▌▐█▄▪▐█
//            ▀▀▀  ▀▀▀▀  ▀▀▀ .▀  ▀ ▀▀▀▀

router.get(
  "/users",  
  async function (_req, res, next) {
    try {
      res.json(await dbFunctions.getUsers());
    } catch (err) {
      console.error("Error while getting users!", err.message);
      next(err);
    }
  }
);

router.get("/users/:id", async function (req, res) {
  try {
    res.json(await dbFunctions.getUserById(req, res));
  } catch (err) {
    console.error("Error getting user by id!", err.message);
  }
});

router.put("/users/put", [verifyToken], async function (req, res) {
  try {
    res.json(await userController.putUsers(req.body, res, req.user.id));
  } catch (err) {
    console.error("Error updating!", err.message);
  }
}),
  router.delete("/users/delete", [verifyToken], async function (req, res) {
    try {
      res.json(await userController.deleteUsers(req.body, res, req.user.id));
    } catch (err) {
      console.error("Error deleting!", err.message);
    }
  }),
  router.put(
    "/users/edit",
    [verifyToken],
    [isAdmin],
    async function (req, res) {
      try {
        res.json(await userController.editUsers(req, res));
      } catch (err) {
        console.error("Error updating!", err.message);
      }
    }
  ),
  router.delete(
    "/users/remove/:id",
    [verifyToken],
    [isAdmin],
    async function (req, res) {
      try {
        res.json(await userController.removeUsers(req, res, req.params.id));
      } catch (err) {
        console.error("Error updating!", err.message);
      }
    }
  ),
  router.post("/users/changepicture", [verifyToken], async function (req, res) {
    try {
      res.json(await userController.changePicture(req, res, req.user.id));
    } catch {
      console.error("Error while posting pictures!", err.message);
    }
  });

router.post('/users/addfollow', [verifyToken], async function(req,res) {
    try {
        res.json(await userController.updateFollowed(req,res,req.user.id))
    } catch (err) {
        console.error(err.message)
    }
}),

router.post('/users/removefollow', [verifyToken], async function(req,res) {
    try {
        await userController.removeFollow(req,res,req.user.id)
    } catch (err) {
        console.error(err.message)
    }
}),

router.post("/users/resetpassword", async function (req, res) {
  try {
    await authController.resetPassword(req, res);
  } catch (err) {
    console.error(err.message);
  }
}),
  router.post("/users/authorizereset", async function (req, res) {
    try {
      await authController.authorizeReset(req, res);
    } catch (err) {
      console.error(err.message);
    }
  }),
  router.post("/users/newpassword", [verifyToken], async function (req, res) {
    try {
      await userController.newPassword(req, res, req.user.id);
    } catch (err) {
      console.error(err.message);
    }
  }),
  router.post("/users/support", [verifyToken], async function (req, res) {
    try {
      await userController.support(req, res, req.user.id);
    } catch (err) {
      console.error(err.message);
    }
  }),
  //                ▄▄▄▄▄      ▄ •▄ ▄▄▄ . ▐ ▄ .▄▄ ·
  //                •██  ▪     █▌▄▌▪▀▄.▀·•█▌▐█▐█ ▀.
  //                ▐█.▪ ▄█▀▄ ▐▀▀▄·▐▀▀▪▄▐█▐▐▌▄▀▀▀█▄
  //                ▐█▌·▐█▌.▐▌▐█.█▌▐█▄▄▌██▐█▌▐█▄▪▐█
  //                ▀▀▀  ▀█▄▀▪·▀  ▀ ▀▀▀ ▀▀ █▪ ▀▀▀▀

  router.get(
    "/tokens",
    [verifyToken],
    [isAdmin],
    async function (_req, res, next) {
      try {
        res.json(await dbFunctions.getTokens());
      } catch (err) {
        console.error("Error while getting tokens!", err.message);
        next(err);
      }
    }
  ),
  router.delete(
    "/tokens/:id",
    [verifyToken],
    [isAdmin],
    async function (req, res) {
      try {
        res.json(await dbFunctions.deleteToken(req, res, req.params.id));
      } catch (err) {
        console.error("Error deleting tokens!", err.message);
      }
    }
  ),
  router.post("/register", async function (req, res) {
    try {
      res.json(await authController.register(req, res));
    } catch (err) {
      console.error("Error registering!", err.message);
    }
  }),
  router.post("/login", async function (req, res) {
    try {
      res.json(await authController.login(req, res));
    } catch (err) {
      console.error("Error logging in!", err.message);
    }
  }),
  //            ▄▄▄· ·▄▄▄▄  .▄▄ ·
  //            ▐█ ▀█ ██▪ ██ ▐█ ▀.
  //            ▄█▀▀█ ▐█· ▐█▌▄▀▀▀█▄
  //            ▐█ ▪▐▌██. ██ ▐█▄▪▐█
  //            ▀  ▀ ▀▀▀▀▀•  ▀▀▀▀

  router.get("/uploads", async function (_req, res) {
    try {
      res.json(await postController.getUploads());
    } catch (err) {
      console.error("Error while getting ads!", err.message);
    }
  });

router.post("/uploads/post", [verifyToken], async function (req, res) {
  res.json(await postController.postUpload(req, res, req.user.id));
});

router.put("/uploads/:id", [verifyToken], async function (req, res) {
  try {
    res.json(
      await postController.editUpload(req, res, req.user.id, req.params.id)
    );
  } catch (err) {
    console.error("Error getting user by id!", err.message);
  }
});

// router.put("/uploads/edit/:id", [verifyToken], [isAdmin], async function(req,res) {
//     try {
//         res.json(await postController.modifyAd(req,res,req.params.id))
//     } catch (err) {
//         console.error("Error getting user by id!", err.message)
//     }
// })

router.delete("/uploads/:id", [verifyToken], async function (req, res) {
  try {
    res.json(
      await postController.deleteUpload(req, res, req.user.id, req.params.id)
    );
  } catch (err) {
    console.error("Error deleting ads", err.message);
  }
});

router.delete(
  "/uploads/remove/:id",
  [verifyToken],
  [isAdmin],
  async function (req, res) {
    try {
      res.json(await postController.removeUpload(req, res, req.params.id));
    } catch (err) {
      console.error("Error deleting", err.message);
    }
  }
);

//        ▄▄▄·▪   ▄▄· ▄▄▄▄▄▄• ▄▌▄▄▄  ▄▄▄ ..▄▄ ·
//        ▐█ ▄███ ▐█ ▌▪•██  █▪██▌▀▄ █·▀▄.▀·▐█ ▀.
//        ██▀·▐█·██ ▄▄ ▐█.▪█▌▐█▌▐▀▀▄ ▐▀▀▪▄▄▀▀▀█▄
//        ▐█▪·•▐█▌▐███▌ ▐█▌·▐█▄█▌▐█•█▌▐█▄▄▌▐█▄▪▐█
//        .▀   ▀▀▀·▀▀▀  ▀▀▀  ▀▀▀ .▀  ▀ ▀▀▀  ▀▀▀▀

router.post("/uploads/upload", [verifyToken], (req, res) => {
  upload(req, res, function (err) {
    if (err instanceof multer.MulterError)
      return res.status(500).json({ error: err.message });
    else if (err) return res.status(500).json({ error: "Unknown error" });

    res.status(200).json({ message: "Upload was successfull" });
  });
});

router.get("/uploads/upload", async (req, res) => {
  try {
    const filenames = await uploadController.uploadedPictures("./uploads");
    res.send(filenames);
  } catch (err) {
    console.error(err.message);
  }
});

//            ▄▄▄ .• ▌ ▄ ·.  ▄▄▄· ▪  ▄▄▌
//            ▀▄.▀··██ ▐███▪▐█ ▀█ ██ ██•
//            ▐▀▀▪▄▐█ ▌▐▌▐█·▄█▀▀█ ▐█·██▪
//            ▐█▄▄▌██ ██▌▐█▌▐█ ▪▐▌▐█▌▐█▌▐▌
//            ▀▀▀ ▀▀  █▪▀▀▀ ▀  ▀ ▀▀▀.▀▀▀

router.get("/email/subscribe", [verifyToken], async function (req, res) {
  try {
    await emailController.subscribe(req, res, req.user.email);
  } catch (err) {
    console.error(err.message);
  }
});

//                .▄▄ · ▄• ▄▌ ▄▄▄· ▄▄▄·      ▄▄▄  ▄▄▄▄▄
//                ▐█ ▀. █▪██▌▐█ ▄█▐█ ▄█▪     ▀▄ █·•██
//                ▄▀▀▀█▄█▌▐█▌ ██▀· ██▀· ▄█▀▄ ▐▀▀▄  ▐█.▪
//                ▐█▄▪▐█▐█▄█▌▐█▪·•▐█▪·•▐█▌.▐▌▐█•█▌ ▐█▌·
//                ▀▀▀▀  ▀▀▀ .▀   .▀    ▀█▄▀▪.▀  ▀ ▀▀▀

router.get("/support", [verifyToken], [isAdmin], async function (req, res) {
  try {
    res.json(await supportController.getSupport(req, res));
  } catch (err) {
    console.error(err.message);
  }
});

router.delete(
  "/support/:id",
  [verifyToken],
  [isAdmin],
  async function (req, res) {
    try {
      res.json(await supportController.deleteSupport(req, res, req.params.id));
    } catch (err) {
      console.error(err.message);
    }
  }
);

module.exports = {
  router,
};
