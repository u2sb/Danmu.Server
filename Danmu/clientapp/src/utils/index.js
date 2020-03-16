export function isEmpty(...str) {
    return str.some(i => i === undefined || i === null || i === '')
}

//简单重置对象属性
export function resetObj(obj, options) {
    if (isEmpty(obj)) return
    Object.keys(obj).forEach(key => {
        if (obj[key] !== null && typeof obj[key] === 'object') {
            resetObj(obj[key], options)
        }
        else obj[key] = resetVar(obj[key], options)
    })
}

//根据类型置空变量
export function resetVar(v, { emptyNumber = 0, emptyString = '', emptyBool = false } = {}) {
    if (Array.isArray(v)) return []
    switch (typeof v) {
        case 'boolean':
            return emptyBool
        case 'number':
            return emptyNumber
        case 'string':
            return emptyString
        default:
            return null
    }
}

//简单合并对象
export function mergeObj(target, source) {
    Object.keys(target).forEach(key => {
        if (key in source) {
            if (Array.isArray(target[key])) {
                target[key] = source[key] || []
            }
            else if (target[key] !== null && typeof target[key] === 'object') {
                mergeObj(target[key], source[key])
            }
            else target[key] = source[key]
        }
    })
}

//store中根据state批量生成对应的mutation
export function createMutations(state, all = true, keyFunction = key => 'set' + firstLetter2UpperCase(key)) {
    const arr = Object.keys(state)
    const obj = {}
    arr.forEach(key => obj[keyFunction(key)] = (s, v) => s[key] = v)
    if (all) obj['setAll'] = (s, v) => {
        if (!v) return resetObj(s)
        arr.forEach(key => {
            s[key] = isEmpty(v[key]) ? resetVar(s[key]) : v[key]
        })
    }
    return obj
}

//字符串首字母大写
export function firstLetter2UpperCase(str) {
    if (isEmpty(str)) return ''
    const firstLetter = str.charAt(0).toUpperCase()
    return str.length === 1 ? firstLetter : firstLetter + str.slice(1)
}

export function deepClone(data) {
    if (data === null || data === undefined) {
        return undefined
    }

    const dataType = getDataType(data)

    if (dataType === 'Date') {
        let clonedDate = new Date()
        clonedDate.setTime(data.getTime())
        return clonedDate
    }

    if (dataType === 'Object') {
        if (isCyclic(data) === true) {
            return data
        }

        let copiedObject = {}

        Object.keys(data).forEach(key => copiedObject[key] = deepClone(data[key]))

        return copiedObject
    }

    if (dataType === 'Array') {
        let copiedArray = []

        for (let i = 0; i < data.length; i++) {
            copiedArray.push(deepClone(data[i]))
        }

        return copiedArray
    }

    else return data
}

function getDataType(data) {
    return Object.prototype.toString.call(data).slice(8, -1)
}

function isCyclic(data) {
    let seenObjects = []

    function detect(data) {
        if (data && getDataType(data) === 'Object') {

            if (seenObjects.indexOf(data) !== -1) {
                return true
            }

            seenObjects.push(data)

            for (let key in data) {
                if (data.hasOwnProperty(key) && detect(data[key])) {
                    return true
                }
            }
        }
        return false
    }

    return detect(data)
}



