using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Danmu.Model.DataTable
{
    public class HttpClientCacheTable
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Data", TypeName = "jsonb")]
        [Required]
        public CacheData Data { get; set; }
    }

    public class CacheData
    {
        public string Key { get; set; }
        public byte[] Value { get; set; }
        public long TimeStamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
