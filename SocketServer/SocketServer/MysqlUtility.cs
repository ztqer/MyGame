using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace SocketServer
{
    public class MysqlUtility
    {
        private static string connectString= "server=localhost;port=3306;database=mygamedb;user=root;password=zang19980226";

        public static void SqlTask(string statement)
        {
            MySqlConnection connection= new MySqlConnection(connectString);
            MySqlCommand command = new MySqlCommand(statement, connection);
            try
            {
                connection.Open();
                Console.WriteLine("连接mysql成功");
                MySqlDataReader reader = command.ExecuteReader();
                while(reader.Read())//读一行
                {
                    Console.WriteLine(reader.GetString("id"));
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("MySql连接失败"+e.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
