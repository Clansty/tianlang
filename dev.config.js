global.dev = true;
const groups = require('./groups.js');
module.exports = {
  type: "ws",
  server: "ws://101.132.178.136:5700",
  secret: "asdfasdf",
  token: "asdfasdf",
  plugins: [
    '.'
  ],
  database: {
    mysql: {
      host: 'cdb-pi7fvpvu.cd.tencentcdb.com',
      port: 10058,
      user: 'root',
      password: 't00rrooT',
      database: 'tianlang_dev',
    },
  },
}