namespace Danmu.Utils.Global
{
    /// <summary>
    ///     变量字典
    /// </summary>
    public class VariableDictionary
    {
        #region EdnPoints

        /// <summary>
        ///     后台管理面板Route
        /// </summary>
        public const string AdminRoute = "AdminRoute";

        #endregion

        #region 跨域标记

        /// <summary>
        ///     普通弹幕允许跨域
        /// </summary>
        public const string DanmuAllowSpecificOrigins = "DanmuAllowSpecificOrigins";

        /// <summary>
        ///     直播弹幕允许跨域
        /// </summary>
        public const string LiveAllowSpecificOrigins = "LiveAllowSpecificOrigins";

        /// <summary>
        ///     管理后台api允许跨域
        /// </summary>
        public const string AdminAllowSpecificOrigins = "AdminAllowSpecificOrigins";

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

        #region 角色 Role Policy

        /// <summary>
        ///     管理员
        /// </summary>
        public const string AdminRolePolicy = "adminRolePolicy";

        /// <summary>
        ///     普通用户
        /// </summary>
        public const string GeneralUserRolePolicy = "generalUserRolePolicy";

        #endregion
    }
}