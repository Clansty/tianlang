var groups = {
    test: 828390342
}
if (global.dev) {
    groups.major = 670526569;
    groups.console = 960701873;
}
else {
    groups.major = 646751705;
    groups.console = 960696283;
}
module.exports = groups;