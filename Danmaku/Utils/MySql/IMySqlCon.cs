using MySql.Data.MySqlClient;

namespace Danmaku.Utils.MySql
{
    public interface IMySqlCon
    {
        MySqlConnection Connection();
    }
}
