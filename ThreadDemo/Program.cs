using Common.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ThreadDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int pageSize = 10;
            int minval = Convert.ToInt32(ConfigurationManager.AppSettings["MinValue"]);
            int maxval = Convert.ToInt32(ConfigurationManager.AppSettings["MaxValue"]);
            string Creation_Time = ConfigurationManager.AppSettings["Time"];
            string fileName = DateTime.Now.ToString("yyyMMddHHmmss")+"log.txt";
            string where = " and QS.creation_time<'" + Creation_Time + "'";
            

            #region
            for (int i = minval; i < maxval; i++)
            {
                var sql = ReadXmlHelper.GetXmlValue(Environment.CurrentDirectory + "/../../SQLXml.xml", "GetExcutePage");
                sql = string.Format(sql, where, (i - 1) * pageSize + 1, i * pageSize);
                //DataTable dt = SqlHelper.ExecuteTable(SqlHelper.GetConnection("GoalConStr"), CommandType.Text, sql);
                SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.GetConnection("GoalConStr"), CommandType.Text, sql);
                //生成语句
                //GenerateSqlByRender(reader);

                //sql方法
                DataBulkCopy(reader);
                Console.WriteLine(i);
               
                 WriteLog(fileName, ConvertLog(i, where, pageSize));
            }
            #endregion
            //Console.WriteLine(json);
            //string aa = Console.ReadLine();
            //Console.WriteLine(aa);
            //new Plinq().AddList();
            //new Plinq().ConcurrentBag();
            //new ThreadDemo().ParallelInvokeMethod();
            Console.ReadLine();
        }
        private static void DataBulkCopy(SqlDataReader reader)
        {
            try
            {
                SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlHelper.GetConnSting(), SqlBulkCopyOptions.UseInternalTransaction);//在事务中运行
                bulkCopy.BatchSize = 1000;
                bulkCopy.DestinationTableName = "System_SqlRecord";
                bulkCopy.BulkCopyTimeout = 600;

                bulkCopy.WriteToServer(reader);
                bulkCopy.Close();
            }
            catch (Exception ex)
            {
                WriteLog("error.txt",DateTime.Now.ToString()+" "+ex.Message+ex.StackTrace);
                throw;
            }
            
        }
        private static void GenerateSqlByRender(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                //在While循环外先根据列名取得索引
 
                List<String> list = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    list.Add(reader.GetName(i));
                }
                //int c2 = reader.GetOrdinal("lie2");
                //如果查询出了数据则开始循环获取每一条数据
                while (reader.Read())
                {
                    string str = "";
                    StringBuilder sql = new StringBuilder();
                    sql.Append("INSERT INTO [dbo].[System_SqlRecord]([RecordID],[Execution_Count],[Total_Elapsed_Time],[Total_Logical_Reads],[Total_Logical_Writes],[Total_Physical_Reads],[Creation_Time],[DatabaseId],[ExecQueryText])VALUES(");
                    
                    for (int i = 0; i < list.Count-1; i++)
                    {
                        
                        sql.Append("'"+reader[list[i]]+"',");
                       
                    }
                    sql.Append('"');
                    sql.Append(reader[list[list.Count - 1]]);
                    sql.Append('"');
                    //str = sql.ToString().Substring(0, sql.Length - 1);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetConnSting(),CommandType.Text, sql.ToString());
                    WriteLog("GenerateSql.txt",str+");");
                }
                reader.Close();
             }
        }
        private static void WriteLog(string fileName,string Text)
        {
            string path = Environment.CurrentDirectory + "/../../" + fileName;

            if (!File.Exists(path))
            {
                FileStream fs1 = File.Create(path);
                fs1.Close();
            }
            FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            
            sw.WriteLine(Text);
            sw.Close();
        }
        private static string ConvertLog(int  i,string where,int pageSize)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("添加数据" + i.ToString());
            sb.Append("  " + i * pageSize + "条");
            sb.Append("  条件" + where);
            sb.Append("  执行时间" + DateTime.Now.ToString());
            return sb.ToString();
        }
    }
}
