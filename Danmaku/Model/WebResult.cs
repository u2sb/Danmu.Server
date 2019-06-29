using System.Collections.Generic;
using Newtonsoft.Json;

namespace Danmaku.Model
{
    public class WebResult
    {
        public WebResult()
        {
        }

        public WebResult(List<string> ss)
        {
            var lo = new List<object[]>();
            foreach (var s in ss)
            {
                lo.Add(JsonConvert.DeserializeObject<object[]>(s));
            }

            Data = lo;
        }

        public int Code { get; set; } = 1;
        public object Data { get; set; }
    }
}