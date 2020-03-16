const { cookieRoleKey } = require('../../src/config')
module.exports = (req, res) => {
    res.cookie(cookieRoleKey, 'SuperAdmin', { maxAge: 900000 })
    return { uid: 1 }
}
