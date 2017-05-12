using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility
{
    //public class AsynSqlHelper
    //{
    //    public void aa()
    //    {
    //        string connectionString = @"Data Source=127.0.0.1;Integrated Security=SSPI;Database=Northwind;Asynchronous Processing=true";
    //                     mConnection = new SqlConnection(connectionString);
    //                     string sqlString = "select * from Orders";
    //                     SqlCommand command = new SqlCommand(sqlString, mConnection);
    //                     command
                       
    //                     AsyncCallback callBack = new AsyncCallback(HandleCallback);//注册回调方法
    //         //开始执行异步查询，将Command作为参数传递到回调函数以便执行End操作
    //          command.BeginExecuteReader(callBack, command);
    //    }
    //    private void HandleCallback(IAsyncResult MyResult)
    //     {
    //         try
    //         {
    //             SqlCommand command = (SqlCommand)MyResult.AsyncState;
    //             SqlDataReader reader = command.EndExecuteReader(MyResult);
    //             mWatch.Stop();
    //             string callBackTime = mWatch.ElapsedMilliseconds.ToString() + "毫秒";
    //             DataTable dataTable = new DataTable();
    //             dataTable.Load(reader);                
    //             this.Invoke(myTimeDelegate, callBackTime);                
    //             this.Invoke(myDataDelegate, dataTable);
    //         }
    //         catch (Exception MyEx)
    //         {
    //             this.Invoke(new DisplayInfoDelegate(DisplayTimeResults), String.Format(MyEx.Message));
    //         }
    //         finally
    //         {
    //             if (mConnection != null)
    //             {
    //                 mConnection.Close();
    //             }
    //         }
    //     }
    //}
}
