namespace Danmu.Utils.Globals
{
    /// <summary>
    ///     变量字典
    /// </summary>
    public class VariableDictionary
    {
        #region 跨域标记

        /// <summary>
        ///     普通弹幕允许跨域
        /// </summary>
        public const string DanmuAllowSpecificOrigins = "DanmuAllowSpecificOrigins";

        /// <summary>
        ///     直播弹幕允许跨域
        /// </summary>
        public const string LiveAllowSpecificOrigins = "LiveAllowSpecificOrigins";

        #endregion

        #region http请求标记

        /// <summary>
        ///     Gzip压缩方式
        /// </summary>
        public const string Gzip = "gzip";

        /// <summary>
        ///     deflate压缩方式
        /// </summary>
        public const string Deflate = "deflate";

        #endregion

        #region EasyCaching缓存标记

        /// <summary>
        ///     内存缓存
        /// </summary>
        public const string InMemory = "InMemory";

        /// <summary>
        ///     LiteDb
        /// </summary>
        public const string LiteDb = "LiteDb";

        #endregion
    }
}
