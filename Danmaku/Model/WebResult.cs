using System.Collections.Generic;
using Newtonsoft.Json;

namespace Danmaku.Model
{
    public class WebResult
    {
        public WebResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss">数据list</param>
        public WebResult(List<string> ss)
        {
            var lo = new List<object[]>();
            foreach (var s in ss)
            {
                lo.Add(JsonConvert.DeserializeObject<object[]>(s));
            }

            Data = lo;
        }


        /// <summary>
        /// 代码，0正常 1错误
        /// </summary>
        public int Code { get; set; } = 1;

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }
}