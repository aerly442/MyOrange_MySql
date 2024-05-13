using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Web;

namespace MyOrange
{
    /// <summary>
    /// 用来处理xls
    /// </summary>
    public class Xls
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Xls()
        { }

        private string FileName = "";

        /// <summary>
        /// 包含绝对路径的
        /// </summary>
        /// <param name="filename"></param>
        public Xls(string filename)
        {
            this.FileName = filename;

        }
        private string GetConnectString()
        {
            if (MyOrange.Web.GetConfigString("DSN") == "")
                return "DRIVER={Microsoft Excel Driver (*.xlsx)};";
            else
                return "DSN=" + MyOrange.Web.GetConfigString("DSN") + ";";
        }
        /// <summary>
        /// 获取第一个工作表的数据
        /// </summary
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            OdbcConnection con = new OdbcConnection();
            string tablename = "[sheet1$]";
            string strcon = this.GetConnectString() + "DBQ=" + this.FileName + ";";
            con.ConnectionString = strcon;
            string strsql = "select * from " + tablename;
            System.Data.Odbc.OdbcDataAdapter apt = new OdbcDataAdapter(strsql, con);
            DataSet ds = new DataSet();
            apt.Fill(ds);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;


        }
        /// <summary>
        /// 把数据集写入 xls文件中
        /// </summary>
        /// <param name="ds">数据集</param>
        public void WriteToXls(DataSet ds)
        {
            if (ds != null)
            {
                OdbcConnection con = new OdbcConnection();
                string strcon = this.GetConnectString() + "DBQ=" + this.FileName + ";ReadOnly=0;";
                con.ConnectionString = strcon;
                OdbcCommand cmd = new OdbcCommand();
                cmd.Connection = con;
                con.Open();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    string sheetname = "[" + ds.Tables[i].TableName + "$]";
                    string insert = "";
                    for (int n = 0; n < ds.Tables[i].Rows.Count; n++)
                    {
                        insert = "insert into " + sheetname + " (";
                        System.Text.StringBuilder sbfield = new StringBuilder();
                        StringBuilder sbvalue = new StringBuilder();
                        for (int col = 1; col < ds.Tables[i].Columns.Count; col++)
                        {
                            sbfield.Append(ds.Tables[i].Columns[col].ColumnName);
                            string strvalue = ds.Tables[i].Rows[n][col].ToString().Replace("'", "");
                            sbvalue.Append("'");
                            if (ds.Tables[i].Columns[col].ColumnName == "SerialNo")
                            {
                                sbvalue.Append((n + 1).ToString());
                            }
                            else
                                sbvalue.Append(strvalue);
                            sbvalue.Append("'");
                            if (col < ds.Tables[i].Columns.Count - 1)
                            {

                                sbfield.Append(",");
                                sbvalue.Append(",");
                            }

                        }
                        insert += sbfield.ToString() + ") values (" + sbvalue.ToString() + ");";

                        cmd.CommandText = insert;
                        cmd.ExecuteNonQuery();
                        insert = "";

                    }



                }
                con.Close();


            }




        }
        /// <summary>
        /// 根据sheet名称获取数据
        /// </summary>
        /// <param name="sheetname">工作簿的名称</param>
        /// <returns></returns>
        public DataTable GetDataTable(string filename, string sheetname)
        {
           
            OdbcConnection con = new OdbcConnection();
            string tablename = "[" + sheetname + "$]";
            string strcon = this.GetConnectString() + "DBQ=" + filename + ";";
            con.ConnectionString = strcon;
            string strsql = "select * from " + tablename;
            System.Data.Odbc.OdbcDataAdapter apt = new OdbcDataAdapter(strsql, con);
            DataSet ds = new DataSet();
            apt.Fill(ds);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;


        }
        /// <summary>
        /// 获取行数
        /// </summary>
        /// <returns></returns>
        public string GetTotalCount()
        {
            
            OdbcConnection con = new OdbcConnection();
            string tablename = "[sheet1$]";
            string strcon = this.GetConnectString() + "DBQ=" + this.FileName + ";";
            con.ConnectionString = strcon;
            string strsql = "select count(*) from " + tablename;
            System.Data.Odbc.OdbcDataAdapter apt = new OdbcDataAdapter(strsql, con);
            DataSet ds = new DataSet();
            apt.Fill(ds);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0].Rows[0][0].ToString();
            else
                return "";

        }
        /// <summary>
        /// 根据文件名称获取数据
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns></returns>
        public DataTable GetDataTable(string filename)
        {
            this.FileName = filename;
            return this.GetDataTable();
        }
        /// <summary>
        /// 把数据写到一个字符串里面，含有了xls文件的信息
        /// </summary>
        /// <param name="dt">数据集合</param>
        /// <returns></returns>
        public static string GetExportXls(DataTable dt)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                #region 写入表头
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append(dt.Columns[i].ColumnName);
                    sb.Append("\t");

                }

                sb.Append("\r");
                #endregion

                #region 写入每行数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int n = 0; n < dt.Columns.Count; n++)
                    {
                        if (dt.Rows[i][n] != System.DBNull.Value)
                        {
                            sb.Append(dt.Rows[i][n]);
                        }
                        else
                            sb.Append("");
                        sb.Append("\t");
                        //sb.Append();

                    }
                    sb.Append("\r");


                }
                #endregion
                return sb.ToString();

            }
            else
                return "";

        }

        /// <summary>
        /// 下载一个xls文件
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="filename">文件名称</param>

        public static void DownLoadXls(DataTable dt, string filename)
        {
            if (filename == null || filename == "")
            {
                filename = System.Guid.NewGuid().ToString();
            }
            filename += ".xls";
            string reuslt = GetExportXls(dt);
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.Charset = "";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
                System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。 
                System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                System.Web.HttpContext.Current.Response.Write(reuslt);
                System.Web.HttpContext.Current.Response.End();
            }



        }
    }
}
