const fs = require('fs');

let uploadController = {
    uploadedPictures: async function (directory) {
        console.log("Getting uploaded pictures...")
        let fileNames = fs.readdirSync(directory, {withFileTypes: true})
        .filter(item => !item.isDirectory())
        .map(item => item.name)
        return fileNames
    }
}


module.exports = {
    uploadController
}