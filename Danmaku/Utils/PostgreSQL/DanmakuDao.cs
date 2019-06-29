using System.Collections.Generic;
using Danmaku.Model;
using Dapper;
using Npgsql;

namespace Danmaku.Utils.PostgreSQL
{
    public class DanmakuDao : IDanmakuDao
    {
        private readonly NpgsqlConnection _con;

        public DanmakuDao(IPsCon con)
        {
            _con = con.Connection();
        }

        public List<string> Query(string id)
        {
            return _con.QueryAsync<string>(
                        $"select \"data\"::json->'data' from danmaku where \"data\" @> '{{\"id\": \"{id}\"}}'").Result
                    .AsList();
            }

        public int Insert(DanmakuInsert danmaku)
        {
            return _con.ExecuteAsync($"insert into danmaku (\"data\") values ('{danmaku.ToJson()}')").Result;
        }
    }
}