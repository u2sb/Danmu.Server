using Npgsql;

namespace Danmaku.Utils.PostgreSQL
{
    public interface IPsCon
    {
        NpgsqlConnection Connection();
    }
}