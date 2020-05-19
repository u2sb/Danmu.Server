using MessagePack;

namespace Danmu.Model.WebResult
{
    [MessagePackObject]
    public class DanmuListWebResult<T> : WebResult<DanmuList<T>>
    {
        public DanmuListWebResult() { }
        public DanmuListWebResult(int code) : base(code) { }

        public DanmuListWebResult(int total, T[] list) : this(0)
        {
            Data = new DanmuList<T>
            {
                Total = total,
                List = list
            };
        }
    }

    [MessagePackObject]
    public class DanmuList<T>
    {
        [Key(0)]
        public int Total { get; set; }
        [Key(1)]
        public T[] List { get; set; }
    }
}
