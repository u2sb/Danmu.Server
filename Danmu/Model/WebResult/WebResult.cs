using MessagePack;

namespace Danmu.Model.WebResult
{
    [MessagePackObject]
    public class WebResult<T>
    {
        public WebResult() { }

        public WebResult(int code)
        {
            Code = code;
        }

        public WebResult(T data) : this(0)
        {
            Data = data;
        }

        [Key(0)]
        public int Code { get; set; }

        [Key(1)]
        public T Data { get; set; }
    }

    public class WebResult : WebResult<dynamic>
    {
        public WebResult() { }
        public WebResult(int code) : base(code) { }
    }
}
