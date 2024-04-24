const jwt = require('jsonwebtoken')
const { secret } = require('../config/auth.config');
const bcrypt = require('bcryptjs')


  function verifyToken (req, res, next) {

    const reqToken = req.headers['authorization'];

    if (!reqToken) {
      return res.status(403).send({ message: "Invalid token" });
    } else if (!reqToken.startsWith('Bearer ')) {
    return res.status(403).send({ message: "Not a Bearer token" });
    }
    const token = reqToken.slice(7, reqToken.length);
    jwt.verify(token,
      secret,
      (err, decoded) => {
        if (err) {
          if (err.name === 'TokenExpiredError') {
            return res.status(401).send({ message: "Token expired" });
          } else {
            return res.status(401).send({ message: "Unauthorized" });
          }
        }

        decoded = jwt.verify(token, secret)
        req.user = decoded.payload
        next()
        
      });
}

function accessToken (payload) {
const token = jwt.sign(
    payload,
    secret,
    { expiresIn: '30d'});
return token;
} 

function refreshToken (payload) {
const token = jwt.sign(
payload,
secret,
{expiresIn: '5m'});
return token;
}

function compareToken (res, inputToken, dbToken, user) {
console.log(user)
jwt.verify(dbToken,
secret,
(err, decoded) => {
  if (err) {
    if (err.name === 'TokenExpiredError') {
      return res.status(401).send({ message: "Token expired" });
    } else {
      return res.status(401).send({ message: "Unauthorized" });
    }
  }

  decoded = jwt.verify(dbToken, secret)
  let hashedToken = decoded.token
  let tokenMatch =  bcrypt.compareSync(inputToken, hashedToken)

  if (tokenMatch) {
    const payload = {
      id: user.id,
      name: user.nev,
      email: user.email,
      location: user.hely,
      pPic: user.pPic,
      role: user.szerep,
      favourites: user.kedvencek,
      phone: user.telefonszam
  }
    let token = accessToken({payload})
    return res.status(200).json({token: `${token}`}) 
  } else {
    return res.status(401).send({message: "Token does not match with the one in database"})
  }
  
});
} 



 

module.exports = {verifyToken, accessToken, refreshToken, compareToken}