namespace Danmu.Models.Danmus.BiliBili
{
    public class BiliBiliQuery
    {
        public int Cid { get; set; }
        public int Aid { get; set; }
        public string Bvid { get; set; }
        public int P { get; set; }
        public string[] Date { get; set; } = new string[0];
    }
}
