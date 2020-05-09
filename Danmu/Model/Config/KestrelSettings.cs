namespace Danmu.Model.Config
{
    public class KestrelSettings
    {
        /// <summary>
        ///     服务运行端口
        /// </summary>
        public int[] Port { get; set; }

        /// <summary>
        ///     UnixSocketPath
        /// </summary>
        public string[] UnixSocketPath { get; set; }
    }
}
