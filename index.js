const groups = require("./groups")

module.exports = (app) => {
    //var logger = app.logger()
    //logger.info("app starting in " + (global.dev ? "dev" : "production") + " mode...");
    //app.groups.middleware(require('./middlewares/test'))
    // app.receiver.on('group-increase', require('./on-group-increase'))
    // app.receiver.on('ready', aaa)


    // 手动添加要获取的字段，下面会介绍

    app.command('echo <message>')
        .action(({ meta }, message) => meta.$send(message))
}
