namespace Danmu.Model.Config
{
    public class KestrelSettings
    {
        /// <summary>
        ///     服务运行端口
        /// </summary>
        public int[] Port { get; set; } = {5001};

        /// <summary>
        ///     UnixSocketPath
        /// </summary>
        public string[] UnixSocketPath { get; set; }
    }
}