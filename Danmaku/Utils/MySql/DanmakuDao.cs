using System.Collections.Generic;
using Danmaku.Model;
using Dapper;

namespace Danmaku.Utils.MySql
{
    public class DanmakuDao : IDanmakuDao
    {
        private readonly IMySqlCon _con;

        public DanmakuDao(IMySqlCon con)
        {
            _con = con;
        }


        public List<DanmakuGet> Query(string id)
        {
            return _con.Connection()
                .QueryAsync<DanmakuGet>(
                    "SELECT Time, Type, Color, Author, Text FROM Danmaku WHERE Id = @id", new {id}).Result.AsList();
        }

        public int Insert(DanmakuModel danmaku)
        {
            return _con.Connection()
                .ExecuteAsync(
                    "INSERT INTO Danmaku (Id, Author, Time, Type, Color, Text, Ip, Referer, Date) VALUES (@Id, @Author, @Time, @Type, @Color, @Text, @Ip, @Referer, @Date)",
                    danmaku)
                .Result;
        }
    }
}