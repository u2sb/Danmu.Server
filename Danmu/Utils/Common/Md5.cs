using System.Security.Cryptography;
using System.Text;

namespace Danmu.Utils.Common
{
    public static class Md5
    {
        /// <summary>
        ///     获取某个字符串的md5值
        /// </summary>
        /// <param name="raw">原始字符串</param>
        /// <param name="salt">盐</param>
        /// <returns>md5值，小写</returns>
        public static string GetMd5(string raw, string salt = null)
        {
            using var md5Hash = MD5.Create();
            if (!string.IsNullOrEmpty(salt)) raw = raw.Substring(2) + salt;
            var md5Data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(raw));
            var sBuilder = new StringBuilder();
            foreach (var t in md5Data)
                sBuilder.Append(t.ToString("x2"));
            return sBuilder.ToString().ToLower();
        }

        /// <summary>
        ///     验证md5
        /// </summary>
        /// <param name="md5">md5</param>
        /// <param name="raw">原始md5</param>
        /// <param name="salt">盐</param>
        /// <returns>是否相同</returns>
        public static bool IsThisMd5(string md5, string raw, string salt = null)
        {
            return GetMd5(raw, salt).Equals(md5.ToLower());
        }
    }
}