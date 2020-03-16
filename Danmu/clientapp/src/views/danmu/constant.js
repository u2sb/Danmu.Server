const danmuModes = [
    { value: 0, label: '' },
    { value: 1, label: '滚动弹幕' },
    { value: 2, label: '' },
    { value: 3, label: '' },
    { value: 4, label: '底部弹幕' },
    { value: 5, label: '顶部弹幕' },
    { value: 6, label: '' },
    { value: 7, label: '' },
    { value: 8, label: '高级弹幕' },
    { value: 9, label: '特殊弹幕' }
].filter(i => i.label)

const danmuDataDefine = {
    time: '出现时间：',
    mode: '弹幕类型：',
    size: '大小：',
    color: '颜色：',
    timeStamp: '时间戳：',
    pool: '弹幕池：',
    author: '用户名：',
    authorId: '用户ID：',
    text: '弹幕内容：'
}

const predefineColors = [
    '#000000',
    '#FFFFFF',
    '#E54256',
    '#FFE133',
    '#64DD17',
    '#39CCFF',
    '#D500F9'
]

export { danmuModes, danmuDataDefine, predefineColors }
