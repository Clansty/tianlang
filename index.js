const groups = require("./groups")

module.exports = (app) => {
    //var logger = app.logger()
    //logger.info("app starting in " + (global.dev ? "dev" : "production") + " mode...");
    //app.group(groups.major).prependMiddleware(require('./middlewares/test'))
    app.receiver.on('group-increase', require('./on-group-increase'))
}