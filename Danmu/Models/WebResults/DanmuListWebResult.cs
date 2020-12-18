namespace Danmu.Models.WebResults
{
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

    public class DanmuList<T>
    {
        public int Total { get; set; }
        public T[] List { get; set; }
    }
}
