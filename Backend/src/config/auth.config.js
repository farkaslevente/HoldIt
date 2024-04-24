const { config } = require("dotenv")
config();
module.exports = {
    secret: process.env.SECRET,
    cookie_keys: ["Bunsen burner", "fact", "flask", "fossil", "funnel", "astronomy", "astrophysics", "atom"]
}