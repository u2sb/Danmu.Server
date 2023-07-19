/*eslint-disable block-scoped-var, id-length, no-control-regex, no-magic-numbers, no-prototype-builtins, no-redeclare, no-shadow, no-var, sort-vars*/
import $protobuf from "./protobuf.js";

const $Reader = $protobuf.Reader, $Writer = $protobuf.Writer, $util = $protobuf.util;

const $root = $protobuf.roots["default"] || ($protobuf.roots["default"] = {});

export const DanMu = $root.DanMu = (() => {

    const DanMu = {};

    DanMu.Models = (function() {

        const Models = {};

        Models.Protos = (function() {

            const Protos = {};

            Protos.BiliBili = (function() {

                const BiliBili = {};

                BiliBili.Dm = (function() {

                    const Dm = {};

                    Dm.DanmakuAIFlag = (function() {

                        function DanmakuAIFlag(properties) {
                            this.dmFlags = [];
                            if (properties)
                                for (let keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                                    if (properties[keys[i]] != null)
                                        this[keys[i]] = properties[keys[i]];
                        }

                        DanmakuAIFlag.prototype.dmFlags = $util.emptyArray;

                        DanmakuAIFlag.encode = function encode(message, writer) {
                            if (!writer)
                                writer = $Writer.create();
                            if (message.dmFlags != null && message.dmFlags.length)
                                for (let i = 0; i < message.dmFlags.length; ++i)
                                    $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuFlag.encode(message.dmFlags[i], writer.uint32(10).fork()).ldelim();
                            return writer;
                        };

                        DanmakuAIFlag.encodeDelimited = function encodeDelimited(message, writer) {
                            return this.encode(message, writer).ldelim();
                        };

                        DanmakuAIFlag.decode = function decode(reader, length) {
                            if (!(reader instanceof $Reader))
                                reader = $Reader.create(reader);
                            let end = length === undefined ? reader.len : reader.pos + length, message = new $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag();
                            while (reader.pos < end) {
                                let tag = reader.uint32();
                                switch (tag >>> 3) {
                                case 1: {
                                        if (!(message.dmFlags && message.dmFlags.length))
                                            message.dmFlags = [];
                                        message.dmFlags.push($root.DanMu.Models.Protos.BiliBili.Dm.DanmakuFlag.decode(reader, reader.uint32()));
                                        break;
                                    }
                                default:
                                    reader.skipType(tag & 7);
                                    break;
                                }
                            }
                            return message;
                        };

                        DanmakuAIFlag.decodeDelimited = function decodeDelimited(reader) {
                            if (!(reader instanceof $Reader))
                                reader = new $Reader(reader);
                            return this.decode(reader, reader.uint32());
                        };

                        DanmakuAIFlag.fromObject = function fromObject(object) {
                            if (object instanceof $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag)
                                return object;
                            let message = new $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag();
                            if (object.dmFlags) {
                                if (!Array.isArray(object.dmFlags))
                                    throw TypeError(".DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag.dmFlags: array expected");
                                message.dmFlags = [];
                                for (let i = 0; i < object.dmFlags.length; ++i) {
                                    if (typeof object.dmFlags[i] !== "object")
                                        throw TypeError(".DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag.dmFlags: object expected");
                                    message.dmFlags[i] = $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuFlag.fromObject(object.dmFlags[i]);
                                }
                            }
                            return message;
                        };

                        DanmakuAIFlag.toObject = function toObject(message, options) {
                            if (!options)
                                options = {};
                            let object = {};
                            if (options.arrays || options.defaults)
                                object.dmFlags = [];
                            if (message.dmFlags && message.dmFlags.length) {
                                object.dmFlags = [];
                                for (let j = 0; j < message.dmFlags.length; ++j)
                                    object.dmFlags[j] = $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuFlag.toObject(message.dmFlags[j], options);
                            }
                            return object;
                        };

                        DanmakuAIFlag.prototype.toJSON = function toJSON() {
                            return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
                        };

                        DanmakuAIFlag.getTypeUrl = function getTypeUrl(typeUrlPrefix) {
                            if (typeUrlPrefix === undefined) {
                                typeUrlPrefix = "type.googleapis.com";
                            }
                            return typeUrlPrefix + "/DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag";
                        };

                        return DanmakuAIFlag;
                    })();

                    Dm.DanmakuElem = (function() {

                        function DanmakuElem(properties) {
                            if (properties)
                                for (let keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                                    if (properties[keys[i]] != null)
                                        this[keys[i]] = properties[keys[i]];
                        }

                        DanmakuElem.prototype.id = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
                        DanmakuElem.prototype.progress = 0;
                        DanmakuElem.prototype.mode = 0;
                        DanmakuElem.prototype.fontsize = 0;
                        DanmakuElem.prototype.color = 0;
                        DanmakuElem.prototype.midHash = "";
                        DanmakuElem.prototype.content = "";
                        DanmakuElem.prototype.ctime = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
                        DanmakuElem.prototype.weight = 0;
                        DanmakuElem.prototype.action = "";
                        DanmakuElem.prototype.pool = 0;
                        DanmakuElem.prototype.idStr = "";
                        DanmakuElem.prototype.attr = 0;
                        DanmakuElem.prototype.animation = "";

                        DanmakuElem.encode = function encode(message, writer) {
                            if (!writer)
                                writer = $Writer.create();
                            if (message.id != null && Object.hasOwnProperty.call(message, "id"))
                                writer.uint32(8).int64(message.id);
                            if (message.progress != null && Object.hasOwnProperty.call(message, "progress"))
                                writer.uint32(16).int32(message.progress);
                            if (message.mode != null && Object.hasOwnProperty.call(message, "mode"))
                                writer.uint32(24).int32(message.mode);
                            if (message.fontsize != null && Object.hasOwnProperty.call(message, "fontsize"))
                                writer.uint32(32).int32(message.fontsize);
                            if (message.color != null && Object.hasOwnProperty.call(message, "color"))
                                writer.uint32(40).uint32(message.color);
                            if (message.midHash != null && Object.hasOwnProperty.call(message, "midHash"))
                                writer.uint32(50).string(message.midHash);
                            if (message.content != null && Object.hasOwnProperty.call(message, "content"))
                                writer.uint32(58).string(message.content);
                            if (message.ctime != null && Object.hasOwnProperty.call(message, "ctime"))
                                writer.uint32(64).int64(message.ctime);
                            if (message.weight != null && Object.hasOwnProperty.call(message, "weight"))
                                writer.uint32(72).int32(message.weight);
                            if (message.action != null && Object.hasOwnProperty.call(message, "action"))
                                writer.uint32(82).string(message.action);
                            if (message.pool != null && Object.hasOwnProperty.call(message, "pool"))
                                writer.uint32(88).int32(message.pool);
                            if (message.idStr != null && Object.hasOwnProperty.call(message, "idStr"))
                                writer.uint32(98).string(message.idStr);
                            if (message.attr != null && Object.hasOwnProperty.call(message, "attr"))
                                writer.uint32(104).int32(message.attr);
                            if (message.animation != null && Object.hasOwnProperty.call(message, "animation"))
                                writer.uint32(178).string(message.animation);
                            return writer;
                        };

                        DanmakuElem.encodeDelimited = function encodeDelimited(message, writer) {
                            return this.encode(message, writer).ldelim();
                        };

                        DanmakuElem.decode = function decode(reader, length) {
                            if (!(reader instanceof $Reader))
                                reader = $Reader.create(reader);
                            let end = length === undefined ? reader.len : reader.pos + length, message = new $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuElem();
                            while (reader.pos < end) {
                                let tag = reader.uint32();
                                switch (tag >>> 3) {
                                case 1: {
                                        message.id = reader.int64();
                                        break;
                                    }
                                case 2: {
                                        message.progress = reader.int32();
                                        break;
                                    }
                                case 3: {
                                        message.mode = reader.int32();
                                        break;
                                    }
                                case 4: {
                                        message.fontsize = reader.int32();
                                        break;
                                    }
                                case 5: {
                                        message.color = reader.uint32();
                                        break;
                                    }
                                case 6: {
                                        message.midHash = reader.string();
                                        break;
                                    }
                                case 7: {
                                        message.content = reader.string();
                                        break;
                                    }
                                case 8: {
                                        message.ctime = reader.int64();
                                        break;
                                    }
                                case 9: {
                                        message.weight = reader.int32();
                                        break;
                                    }
                                case 10: {
                                        message.action = reader.string();
                                        break;
                                    }
                                case 11: {
                                        message.pool = reader.int32();
                                        break;
                                    }
                                case 12: {
                                        message.idStr = reader.string();
                                        break;
                                    }
                                case 13: {
                                        message.attr = reader.int32();
                                        break;
                                    }
                                case 22: {
                                        message.animation = reader.string();
                                        break;
                                    }
                                default:
                                    reader.skipType(tag & 7);
                                    break;
                                }
                            }
                            return message;
                        };

                        DanmakuElem.decodeDelimited = function decodeDelimited(reader) {
                            if (!(reader instanceof $Reader))
                                reader = new $Reader(reader);
                            return this.decode(reader, reader.uint32());
                        };

                        DanmakuElem.fromObject = function fromObject(object) {
                            if (object instanceof $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuElem)
                                return object;
                            let message = new $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuElem();
                            if (object.id != null)
                                if ($util.Long)
                                    (message.id = $util.Long.fromValue(object.id)).unsigned = false;
                                else if (typeof object.id === "string")
                                    message.id = parseInt(object.id, 10);
                                else if (typeof object.id === "number")
                                    message.id = object.id;
                                else if (typeof object.id === "object")
                                    message.id = new $util.LongBits(object.id.low >>> 0, object.id.high >>> 0).toNumber();
                            if (object.progress != null)
                                message.progress = object.progress | 0;
                            if (object.mode != null)
                                message.mode = object.mode | 0;
                            if (object.fontsize != null)
                                message.fontsize = object.fontsize | 0;
                            if (object.color != null)
                                message.color = object.color >>> 0;
                            if (object.midHash != null)
                                message.midHash = String(object.midHash);
                            if (object.content != null)
                                message.content = String(object.content);
                            if (object.ctime != null)
                                if ($util.Long)
                                    (message.ctime = $util.Long.fromValue(object.ctime)).unsigned = false;
                                else if (typeof object.ctime === "string")
                                    message.ctime = parseInt(object.ctime, 10);
                                else if (typeof object.ctime === "number")
                                    message.ctime = object.ctime;
                                else if (typeof object.ctime === "object")
                                    message.ctime = new $util.LongBits(object.ctime.low >>> 0, object.ctime.high >>> 0).toNumber();
                            if (object.weight != null)
                                message.weight = object.weight | 0;
                            if (object.action != null)
                                message.action = String(object.action);
                            if (object.pool != null)
                                message.pool = object.pool | 0;
                            if (object.idStr != null)
                                message.idStr = String(object.idStr);
                            if (object.attr != null)
                                message.attr = object.attr | 0;
                            if (object.animation != null)
                                message.animation = String(object.animation);
                            return message;
                        };

                        DanmakuElem.toObject = function toObject(message, options) {
                            if (!options)
                                options = {};
                            let object = {};
                            if (options.defaults) {
                                if ($util.Long) {
                                    let long = new $util.Long(0, 0, false);
                                    object.id = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                                } else
                                    object.id = options.longs === String ? "0" : 0;
                                object.progress = 0;
                                object.mode = 0;
                                object.fontsize = 0;
                                object.color = 0;
                                object.midHash = "";
                                object.content = "";
                                if ($util.Long) {
                                    let long = new $util.Long(0, 0, false);
                                    object.ctime = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                                } else
                                    object.ctime = options.longs === String ? "0" : 0;
                                object.weight = 0;
                                object.action = "";
                                object.pool = 0;
                                object.idStr = "";
                                object.attr = 0;
                                object.animation = "";
                            }
                            if (message.id != null && message.hasOwnProperty("id"))
                                if (typeof message.id === "number")
                                    object.id = options.longs === String ? String(message.id) : message.id;
                                else
                                    object.id = options.longs === String ? $util.Long.prototype.toString.call(message.id) : options.longs === Number ? new $util.LongBits(message.id.low >>> 0, message.id.high >>> 0).toNumber() : message.id;
                            if (message.progress != null && message.hasOwnProperty("progress"))
                                object.progress = message.progress;
                            if (message.mode != null && message.hasOwnProperty("mode"))
                                object.mode = message.mode;
                            if (message.fontsize != null && message.hasOwnProperty("fontsize"))
                                object.fontsize = message.fontsize;
                            if (message.color != null && message.hasOwnProperty("color"))
                                object.color = message.color;
                            if (message.midHash != null && message.hasOwnProperty("midHash"))
                                object.midHash = message.midHash;
                            if (message.content != null && message.hasOwnProperty("content"))
                                object.content = message.content;
                            if (message.ctime != null && message.hasOwnProperty("ctime"))
                                if (typeof message.ctime === "number")
                                    object.ctime = options.longs === String ? String(message.ctime) : message.ctime;
                                else
                                    object.ctime = options.longs === String ? $util.Long.prototype.toString.call(message.ctime) : options.longs === Number ? new $util.LongBits(message.ctime.low >>> 0, message.ctime.high >>> 0).toNumber() : message.ctime;
                            if (message.weight != null && message.hasOwnProperty("weight"))
                                object.weight = message.weight;
                            if (message.action != null && message.hasOwnProperty("action"))
                                object.action = message.action;
                            if (message.pool != null && message.hasOwnProperty("pool"))
                                object.pool = message.pool;
                            if (message.idStr != null && message.hasOwnProperty("idStr"))
                                object.idStr = message.idStr;
                            if (message.attr != null && message.hasOwnProperty("attr"))
                                object.attr = message.attr;
                            if (message.animation != null && message.hasOwnProperty("animation"))
                                object.animation = message.animation;
                            return object;
                        };

                        DanmakuElem.prototype.toJSON = function toJSON() {
                            return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
                        };

                        DanmakuElem.getTypeUrl = function getTypeUrl(typeUrlPrefix) {
                            if (typeUrlPrefix === undefined) {
                                typeUrlPrefix = "type.googleapis.com";
                            }
                            return typeUrlPrefix + "/DanMu.Models.Protos.BiliBili.Dm.DanmakuElem";
                        };

                        return DanmakuElem;
                    })();

                    Dm.DanmakuFlag = (function() {

                        function DanmakuFlag(properties) {
                            if (properties)
                                for (let keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                                    if (properties[keys[i]] != null)
                                        this[keys[i]] = properties[keys[i]];
                        }

                        DanmakuFlag.prototype.dmid = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
                        DanmakuFlag.prototype.flag = 0;

                        DanmakuFlag.encode = function encode(message, writer) {
                            if (!writer)
                                writer = $Writer.create();
                            if (message.dmid != null && Object.hasOwnProperty.call(message, "dmid"))
                                writer.uint32(8).int64(message.dmid);
                            if (message.flag != null && Object.hasOwnProperty.call(message, "flag"))
                                writer.uint32(16).uint32(message.flag);
                            return writer;
                        };

                        DanmakuFlag.encodeDelimited = function encodeDelimited(message, writer) {
                            return this.encode(message, writer).ldelim();
                        };

                        DanmakuFlag.decode = function decode(reader, length) {
                            if (!(reader instanceof $Reader))
                                reader = $Reader.create(reader);
                            let end = length === undefined ? reader.len : reader.pos + length, message = new $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuFlag();
                            while (reader.pos < end) {
                                let tag = reader.uint32();
                                switch (tag >>> 3) {
                                case 1: {
                                        message.dmid = reader.int64();
                                        break;
                                    }
                                case 2: {
                                        message.flag = reader.uint32();
                                        break;
                                    }
                                default:
                                    reader.skipType(tag & 7);
                                    break;
                                }
                            }
                            return message;
                        };

                        DanmakuFlag.decodeDelimited = function decodeDelimited(reader) {
                            if (!(reader instanceof $Reader))
                                reader = new $Reader(reader);
                            return this.decode(reader, reader.uint32());
                        };

                        DanmakuFlag.fromObject = function fromObject(object) {
                            if (object instanceof $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuFlag)
                                return object;
                            let message = new $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuFlag();
                            if (object.dmid != null)
                                if ($util.Long)
                                    (message.dmid = $util.Long.fromValue(object.dmid)).unsigned = false;
                                else if (typeof object.dmid === "string")
                                    message.dmid = parseInt(object.dmid, 10);
                                else if (typeof object.dmid === "number")
                                    message.dmid = object.dmid;
                                else if (typeof object.dmid === "object")
                                    message.dmid = new $util.LongBits(object.dmid.low >>> 0, object.dmid.high >>> 0).toNumber();
                            if (object.flag != null)
                                message.flag = object.flag >>> 0;
                            return message;
                        };

                        DanmakuFlag.toObject = function toObject(message, options) {
                            if (!options)
                                options = {};
                            let object = {};
                            if (options.defaults) {
                                if ($util.Long) {
                                    let long = new $util.Long(0, 0, false);
                                    object.dmid = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                                } else
                                    object.dmid = options.longs === String ? "0" : 0;
                                object.flag = 0;
                            }
                            if (message.dmid != null && message.hasOwnProperty("dmid"))
                                if (typeof message.dmid === "number")
                                    object.dmid = options.longs === String ? String(message.dmid) : message.dmid;
                                else
                                    object.dmid = options.longs === String ? $util.Long.prototype.toString.call(message.dmid) : options.longs === Number ? new $util.LongBits(message.dmid.low >>> 0, message.dmid.high >>> 0).toNumber() : message.dmid;
                            if (message.flag != null && message.hasOwnProperty("flag"))
                                object.flag = message.flag;
                            return object;
                        };

                        DanmakuFlag.prototype.toJSON = function toJSON() {
                            return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
                        };

                        DanmakuFlag.getTypeUrl = function getTypeUrl(typeUrlPrefix) {
                            if (typeUrlPrefix === undefined) {
                                typeUrlPrefix = "type.googleapis.com";
                            }
                            return typeUrlPrefix + "/DanMu.Models.Protos.BiliBili.Dm.DanmakuFlag";
                        };

                        return DanmakuFlag;
                    })();

                    Dm.DmSegMobileReply = (function() {

                        function DmSegMobileReply(properties) {
                            this.elems = [];
                            if (properties)
                                for (let keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                                    if (properties[keys[i]] != null)
                                        this[keys[i]] = properties[keys[i]];
                        }

                        DmSegMobileReply.prototype.elems = $util.emptyArray;
                        DmSegMobileReply.prototype.state = 0;
                        DmSegMobileReply.prototype.aiFlag = null;

                        DmSegMobileReply.encode = function encode(message, writer) {
                            if (!writer)
                                writer = $Writer.create();
                            if (message.elems != null && message.elems.length)
                                for (let i = 0; i < message.elems.length; ++i)
                                    $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuElem.encode(message.elems[i], writer.uint32(10).fork()).ldelim();
                            if (message.state != null && Object.hasOwnProperty.call(message, "state"))
                                writer.uint32(16).int32(message.state);
                            if (message.aiFlag != null && Object.hasOwnProperty.call(message, "aiFlag"))
                                $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag.encode(message.aiFlag, writer.uint32(26).fork()).ldelim();
                            return writer;
                        };

                        DmSegMobileReply.encodeDelimited = function encodeDelimited(message, writer) {
                            return this.encode(message, writer).ldelim();
                        };

                        DmSegMobileReply.decode = function decode(reader, length) {
                            if (!(reader instanceof $Reader))
                                reader = $Reader.create(reader);
                            let end = length === undefined ? reader.len : reader.pos + length, message = new $root.DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReply();
                            while (reader.pos < end) {
                                let tag = reader.uint32();
                                switch (tag >>> 3) {
                                case 1: {
                                        if (!(message.elems && message.elems.length))
                                            message.elems = [];
                                        message.elems.push($root.DanMu.Models.Protos.BiliBili.Dm.DanmakuElem.decode(reader, reader.uint32()));
                                        break;
                                    }
                                case 2: {
                                        message.state = reader.int32();
                                        break;
                                    }
                                case 3: {
                                        message.aiFlag = $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag.decode(reader, reader.uint32());
                                        break;
                                    }
                                default:
                                    reader.skipType(tag & 7);
                                    break;
                                }
                            }
                            return message;
                        };

                        DmSegMobileReply.decodeDelimited = function decodeDelimited(reader) {
                            if (!(reader instanceof $Reader))
                                reader = new $Reader(reader);
                            return this.decode(reader, reader.uint32());
                        };

                        DmSegMobileReply.fromObject = function fromObject(object) {
                            if (object instanceof $root.DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReply)
                                return object;
                            let message = new $root.DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReply();
                            if (object.elems) {
                                if (!Array.isArray(object.elems))
                                    throw TypeError(".DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReply.elems: array expected");
                                message.elems = [];
                                for (let i = 0; i < object.elems.length; ++i) {
                                    if (typeof object.elems[i] !== "object")
                                        throw TypeError(".DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReply.elems: object expected");
                                    message.elems[i] = $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuElem.fromObject(object.elems[i]);
                                }
                            }
                            if (object.state != null)
                                message.state = object.state | 0;
                            if (object.aiFlag != null) {
                                if (typeof object.aiFlag !== "object")
                                    throw TypeError(".DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReply.aiFlag: object expected");
                                message.aiFlag = $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag.fromObject(object.aiFlag);
                            }
                            return message;
                        };

                        DmSegMobileReply.toObject = function toObject(message, options) {
                            if (!options)
                                options = {};
                            let object = {};
                            if (options.arrays || options.defaults)
                                object.elems = [];
                            if (options.defaults) {
                                object.state = 0;
                                object.aiFlag = null;
                            }
                            if (message.elems && message.elems.length) {
                                object.elems = [];
                                for (let j = 0; j < message.elems.length; ++j)
                                    object.elems[j] = $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuElem.toObject(message.elems[j], options);
                            }
                            if (message.state != null && message.hasOwnProperty("state"))
                                object.state = message.state;
                            if (message.aiFlag != null && message.hasOwnProperty("aiFlag"))
                                object.aiFlag = $root.DanMu.Models.Protos.BiliBili.Dm.DanmakuAIFlag.toObject(message.aiFlag, options);
                            return object;
                        };

                        DmSegMobileReply.prototype.toJSON = function toJSON() {
                            return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
                        };

                        DmSegMobileReply.getTypeUrl = function getTypeUrl(typeUrlPrefix) {
                            if (typeUrlPrefix === undefined) {
                                typeUrlPrefix = "type.googleapis.com";
                            }
                            return typeUrlPrefix + "/DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReply";
                        };

                        return DmSegMobileReply;
                    })();

                    Dm.DmSegMobileReq = (function() {

                        function DmSegMobileReq(properties) {
                            if (properties)
                                for (let keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                                    if (properties[keys[i]] != null)
                                        this[keys[i]] = properties[keys[i]];
                        }

                        DmSegMobileReq.prototype.pid = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
                        DmSegMobileReq.prototype.oid = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
                        DmSegMobileReq.prototype.type = 0;
                        DmSegMobileReq.prototype.segmentIndex = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
                        DmSegMobileReq.prototype.teenagersMode = 0;
                        DmSegMobileReq.prototype.ps = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
                        DmSegMobileReq.prototype.pe = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
                        DmSegMobileReq.prototype.pullMode = 0;
                        DmSegMobileReq.prototype.fromScene = 0;

                        DmSegMobileReq.encode = function encode(message, writer) {
                            if (!writer)
                                writer = $Writer.create();
                            if (message.pid != null && Object.hasOwnProperty.call(message, "pid"))
                                writer.uint32(8).int64(message.pid);
                            if (message.oid != null && Object.hasOwnProperty.call(message, "oid"))
                                writer.uint32(16).int64(message.oid);
                            if (message.type != null && Object.hasOwnProperty.call(message, "type"))
                                writer.uint32(24).int32(message.type);
                            if (message.segmentIndex != null && Object.hasOwnProperty.call(message, "segmentIndex"))
                                writer.uint32(32).int64(message.segmentIndex);
                            if (message.teenagersMode != null && Object.hasOwnProperty.call(message, "teenagersMode"))
                                writer.uint32(40).int32(message.teenagersMode);
                            if (message.ps != null && Object.hasOwnProperty.call(message, "ps"))
                                writer.uint32(48).int64(message.ps);
                            if (message.pe != null && Object.hasOwnProperty.call(message, "pe"))
                                writer.uint32(56).int64(message.pe);
                            if (message.pullMode != null && Object.hasOwnProperty.call(message, "pullMode"))
                                writer.uint32(64).int32(message.pullMode);
                            if (message.fromScene != null && Object.hasOwnProperty.call(message, "fromScene"))
                                writer.uint32(72).int32(message.fromScene);
                            return writer;
                        };

                        DmSegMobileReq.encodeDelimited = function encodeDelimited(message, writer) {
                            return this.encode(message, writer).ldelim();
                        };

                        DmSegMobileReq.decode = function decode(reader, length) {
                            if (!(reader instanceof $Reader))
                                reader = $Reader.create(reader);
                            let end = length === undefined ? reader.len : reader.pos + length, message = new $root.DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReq();
                            while (reader.pos < end) {
                                let tag = reader.uint32();
                                switch (tag >>> 3) {
                                case 1: {
                                        message.pid = reader.int64();
                                        break;
                                    }
                                case 2: {
                                        message.oid = reader.int64();
                                        break;
                                    }
                                case 3: {
                                        message.type = reader.int32();
                                        break;
                                    }
                                case 4: {
                                        message.segmentIndex = reader.int64();
                                        break;
                                    }
                                case 5: {
                                        message.teenagersMode = reader.int32();
                                        break;
                                    }
                                case 6: {
                                        message.ps = reader.int64();
                                        break;
                                    }
                                case 7: {
                                        message.pe = reader.int64();
                                        break;
                                    }
                                case 8: {
                                        message.pullMode = reader.int32();
                                        break;
                                    }
                                case 9: {
                                        message.fromScene = reader.int32();
                                        break;
                                    }
                                default:
                                    reader.skipType(tag & 7);
                                    break;
                                }
                            }
                            return message;
                        };

                        DmSegMobileReq.decodeDelimited = function decodeDelimited(reader) {
                            if (!(reader instanceof $Reader))
                                reader = new $Reader(reader);
                            return this.decode(reader, reader.uint32());
                        };

                        DmSegMobileReq.fromObject = function fromObject(object) {
                            if (object instanceof $root.DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReq)
                                return object;
                            let message = new $root.DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReq();
                            if (object.pid != null)
                                if ($util.Long)
                                    (message.pid = $util.Long.fromValue(object.pid)).unsigned = false;
                                else if (typeof object.pid === "string")
                                    message.pid = parseInt(object.pid, 10);
                                else if (typeof object.pid === "number")
                                    message.pid = object.pid;
                                else if (typeof object.pid === "object")
                                    message.pid = new $util.LongBits(object.pid.low >>> 0, object.pid.high >>> 0).toNumber();
                            if (object.oid != null)
                                if ($util.Long)
                                    (message.oid = $util.Long.fromValue(object.oid)).unsigned = false;
                                else if (typeof object.oid === "string")
                                    message.oid = parseInt(object.oid, 10);
                                else if (typeof object.oid === "number")
                                    message.oid = object.oid;
                                else if (typeof object.oid === "object")
                                    message.oid = new $util.LongBits(object.oid.low >>> 0, object.oid.high >>> 0).toNumber();
                            if (object.type != null)
                                message.type = object.type | 0;
                            if (object.segmentIndex != null)
                                if ($util.Long)
                                    (message.segmentIndex = $util.Long.fromValue(object.segmentIndex)).unsigned = false;
                                else if (typeof object.segmentIndex === "string")
                                    message.segmentIndex = parseInt(object.segmentIndex, 10);
                                else if (typeof object.segmentIndex === "number")
                                    message.segmentIndex = object.segmentIndex;
                                else if (typeof object.segmentIndex === "object")
                                    message.segmentIndex = new $util.LongBits(object.segmentIndex.low >>> 0, object.segmentIndex.high >>> 0).toNumber();
                            if (object.teenagersMode != null)
                                message.teenagersMode = object.teenagersMode | 0;
                            if (object.ps != null)
                                if ($util.Long)
                                    (message.ps = $util.Long.fromValue(object.ps)).unsigned = false;
                                else if (typeof object.ps === "string")
                                    message.ps = parseInt(object.ps, 10);
                                else if (typeof object.ps === "number")
                                    message.ps = object.ps;
                                else if (typeof object.ps === "object")
                                    message.ps = new $util.LongBits(object.ps.low >>> 0, object.ps.high >>> 0).toNumber();
                            if (object.pe != null)
                                if ($util.Long)
                                    (message.pe = $util.Long.fromValue(object.pe)).unsigned = false;
                                else if (typeof object.pe === "string")
                                    message.pe = parseInt(object.pe, 10);
                                else if (typeof object.pe === "number")
                                    message.pe = object.pe;
                                else if (typeof object.pe === "object")
                                    message.pe = new $util.LongBits(object.pe.low >>> 0, object.pe.high >>> 0).toNumber();
                            if (object.pullMode != null)
                                message.pullMode = object.pullMode | 0;
                            if (object.fromScene != null)
                                message.fromScene = object.fromScene | 0;
                            return message;
                        };

                        DmSegMobileReq.toObject = function toObject(message, options) {
                            if (!options)
                                options = {};
                            let object = {};
                            if (options.defaults) {
                                if ($util.Long) {
                                    let long = new $util.Long(0, 0, false);
                                    object.pid = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                                } else
                                    object.pid = options.longs === String ? "0" : 0;
                                if ($util.Long) {
                                    let long = new $util.Long(0, 0, false);
                                    object.oid = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                                } else
                                    object.oid = options.longs === String ? "0" : 0;
                                object.type = 0;
                                if ($util.Long) {
                                    let long = new $util.Long(0, 0, false);
                                    object.segmentIndex = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                                } else
                                    object.segmentIndex = options.longs === String ? "0" : 0;
                                object.teenagersMode = 0;
                                if ($util.Long) {
                                    let long = new $util.Long(0, 0, false);
                                    object.ps = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                                } else
                                    object.ps = options.longs === String ? "0" : 0;
                                if ($util.Long) {
                                    let long = new $util.Long(0, 0, false);
                                    object.pe = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                                } else
                                    object.pe = options.longs === String ? "0" : 0;
                                object.pullMode = 0;
                                object.fromScene = 0;
                            }
                            if (message.pid != null && message.hasOwnProperty("pid"))
                                if (typeof message.pid === "number")
                                    object.pid = options.longs === String ? String(message.pid) : message.pid;
                                else
                                    object.pid = options.longs === String ? $util.Long.prototype.toString.call(message.pid) : options.longs === Number ? new $util.LongBits(message.pid.low >>> 0, message.pid.high >>> 0).toNumber() : message.pid;
                            if (message.oid != null && message.hasOwnProperty("oid"))
                                if (typeof message.oid === "number")
                                    object.oid = options.longs === String ? String(message.oid) : message.oid;
                                else
                                    object.oid = options.longs === String ? $util.Long.prototype.toString.call(message.oid) : options.longs === Number ? new $util.LongBits(message.oid.low >>> 0, message.oid.high >>> 0).toNumber() : message.oid;
                            if (message.type != null && message.hasOwnProperty("type"))
                                object.type = message.type;
                            if (message.segmentIndex != null && message.hasOwnProperty("segmentIndex"))
                                if (typeof message.segmentIndex === "number")
                                    object.segmentIndex = options.longs === String ? String(message.segmentIndex) : message.segmentIndex;
                                else
                                    object.segmentIndex = options.longs === String ? $util.Long.prototype.toString.call(message.segmentIndex) : options.longs === Number ? new $util.LongBits(message.segmentIndex.low >>> 0, message.segmentIndex.high >>> 0).toNumber() : message.segmentIndex;
                            if (message.teenagersMode != null && message.hasOwnProperty("teenagersMode"))
                                object.teenagersMode = message.teenagersMode;
                            if (message.ps != null && message.hasOwnProperty("ps"))
                                if (typeof message.ps === "number")
                                    object.ps = options.longs === String ? String(message.ps) : message.ps;
                                else
                                    object.ps = options.longs === String ? $util.Long.prototype.toString.call(message.ps) : options.longs === Number ? new $util.LongBits(message.ps.low >>> 0, message.ps.high >>> 0).toNumber() : message.ps;
                            if (message.pe != null && message.hasOwnProperty("pe"))
                                if (typeof message.pe === "number")
                                    object.pe = options.longs === String ? String(message.pe) : message.pe;
                                else
                                    object.pe = options.longs === String ? $util.Long.prototype.toString.call(message.pe) : options.longs === Number ? new $util.LongBits(message.pe.low >>> 0, message.pe.high >>> 0).toNumber() : message.pe;
                            if (message.pullMode != null && message.hasOwnProperty("pullMode"))
                                object.pullMode = message.pullMode;
                            if (message.fromScene != null && message.hasOwnProperty("fromScene"))
                                object.fromScene = message.fromScene;
                            return object;
                        };

                        DmSegMobileReq.prototype.toJSON = function toJSON() {
                            return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
                        };

                        DmSegMobileReq.getTypeUrl = function getTypeUrl(typeUrlPrefix) {
                            if (typeUrlPrefix === undefined) {
                                typeUrlPrefix = "type.googleapis.com";
                            }
                            return typeUrlPrefix + "/DanMu.Models.Protos.BiliBili.Dm.DmSegMobileReq";
                        };

                        return DmSegMobileReq;
                    })();

                    return Dm;
                })();

                return BiliBili;
            })();

            return Protos;
        })();

        return Models;
    })();

    return DanMu;
})();

export { $root as default };
