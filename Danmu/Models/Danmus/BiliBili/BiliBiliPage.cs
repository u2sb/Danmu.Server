namespace Danmu.Models.Danmus.BiliBili
{
    public class BiliBiliPage
    {
        public int Code { get; set; }

        public DataObj[] Data { get; set; }


        public class DataObj
        {
            /// <summary>
            ///     分P
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            ///     标题
            /// </summary>
            public string Part { get; set; }

            /// <summary>
            ///     cid
            /// </summary>
            public int Cid { get; set; }

            /// <summary>
            ///     时长
            /// </summary>
            public int Duration { get; set; }

            public DimensionObj Dimension { get; set; }

            public class DimensionObj
            {
                public int Width { get; set; }

                public int Height { get; set; }

                public int Rotate { get; set; }
            }
        }
    }
}
