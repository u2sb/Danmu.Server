namespace Danmu.Models.Danmus.BiliBili
{
    public class BvidInfo
    {
        public int Code { get; set; } = 1;

        public DataObj Data { get; set; }


        public class DataObj
        {
            public int Aid { get; set; }

            public string Bvid { get; set; }
        }
    }
}
