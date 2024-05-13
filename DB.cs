/*
 *修改历史:
 *时    间:2006.6.9 
 *内    容：增加CommandTimeout属性
 * 
 *时    间:2006.6.29 
 *内    容：修正类型为NText类型时候的插入和更新错误
 * 
 *         2006.7.20
 *        :Search(string strsql)增加
 * 
 *        sqlda.SelectCommand.CommandTimeout=this.CommandTimeout;
 * 
 * 
 *         2006.12.20
 *         从VSS绑定中断开
 *         增加读取DataSet 保存 为xml文件的读取
 *         
 * 
 *        直接连接mysql数据 2009-7-16
 */

using System;
using System.Data;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
//using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace MyOrange
{
    /// <summary>
    /// DB 的摘要说明。
    /// </summary>
    public class DB
    {
        private string _ErrorInfo = "";
        /// <summary>
        /// 错误信息
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        public string ErrorInfo
        {
            get { return this._ErrorInfo; }
        }
        private string _StrCon = "";
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string StrCon
        {
            get { return this._StrCon; }
            set { value = this._StrCon; }

        }
        private MySqlConnection _SqlCon = new MySqlConnection();
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public MySqlConnection SqlCon
        {
            get { return this._SqlCon; }
            set { value = this._SqlCon; }

        }
        private int _CommandTimeout = 30;
        /// <summary>
        /// SqlCommand 执行时间
        /// </summary>
        public int CommandTimeout
        {
            get { return this._CommandTimeout; }
            set { this._CommandTimeout = value; }
        }
        private int TextLength = 4000;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strcon">连接字符串</param>
        public DB(string strcon)
        {
            this._SqlCon.ConnectionString = strcon;
            this._StrCon = strcon;
        }

        /// <summary>
        /// 提供一个执行SQL语句的方法
        /// 如果返0可以通过查看属性ErrorInfo
        /// 了解错误信息
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string strsql)
        {
            try
            {
                if (this.SqlCon.State == ConnectionState.Closed)
                    this.SqlCon.Open();

                MySqlCommand sqlcmd = new MySqlCommand(strsql, this.SqlCon);
                sqlcmd.CommandTimeout = this.CommandTimeout;
                //sqlcmd.ex
                sqlcmd.CommandType = CommandType.Text;
                int nresult = sqlcmd.ExecuteNonQuery();
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return nresult;
            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                this._ErrorInfo = e.Message;
                //				OEException.Save(e);
                //				OEException.Save("DB.ExecuteSql:"+strsql);
                return 0;
            }

        }
        private MySqlParameter getSqlParameter(string fieldname, string fieldvalue)
        {
            if (fieldvalue.Length >= TextLength)
            {
                MySqlParameter sqlparam = new MySqlParameter(fieldname,

                    MySqlDbType.Text);
                sqlparam.Value = fieldvalue;
                return sqlparam;
            }
            else
                return new MySqlParameter(fieldname, fieldvalue);
        }
        //private SqlCommand getSqlCmd(NameValueCollection nvcUser,out string ,
        //public virtual int
        /// <summary>
        /// 新增一条数据
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="nvcUser">保存了字段名和值的NameValueCollection</param>
        /// <returns></returns>
        public virtual bool Insert(string tablename, NameValueCollection nvcUser)
        {
            try
            {

                if (tablename == "" || nvcUser.Count == 0)
                    return false;

                string strsql = "Insert Into " + tablename;
                string subsql1 = "", subsql2 = "";
                if (this.SqlCon.State == ConnectionState.Closed)
                    this.SqlCon.Open();
                MySqlCommand cmd = new MySqlCommand(strsql, this.SqlCon);
                cmd.CommandTimeout = this.CommandTimeout;
                #region 构造参数
                for (int i = 0; i < nvcUser.Count; i++)
                {
                    string fieldname = nvcUser.GetKey(i);
                    string fieldvalue = nvcUser.Get(i);
                    subsql1 += fieldname + ",";
                    subsql2 += "@" + fieldname + ",";
                    //SqlParameter sqlpar=getSqlParameter("@"+fieldname,fieldvalue);
                    MySqlParameter sqlpar = getSqlParameter("@" + fieldname, fieldvalue);
                    cmd.Parameters.Add(sqlpar);
                }
                #endregion

                subsql1 = subsql1.Substring(0, subsql1.Length - 1);
                subsql2 = subsql2.Substring(0, subsql2.Length - 1);
                strsql = strsql + "(" + subsql1 + ") values (" + subsql2 + ")";
                cmd.CommandText = strsql;
                cmd.CommandType = CommandType.Text;
                int nresult = cmd.ExecuteNonQuery();
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return true;
            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                //nihaokexue
                this._ErrorInfo = e.Message;
                //				OEException.Save(e);
                //				OEException.Save("DB.Insert:"+this._ErrorInfo);
                return false;
            }

            //return true;

        }
        public virtual string GetInsertSqlNew(string tablename, NameValueCollection nvcUser)
        {
            if (tablename == "" || nvcUser.Count == 0)
                return "";

            string strsql = "Insert Into " + tablename;
            string subsql1 = "", subsql2 = "";
            #region 构造参数
            for (int i = 0; i < nvcUser.Count; i++)
            {
                string fieldname = nvcUser.GetKey(i);
                string fieldvalue = nvcUser.Get(i);
                subsql1 += fieldname + ",";
                subsql2 += "'"+fieldvalue + "',";
                //SqlParameter sqlpar=getSqlParameter("@"+fieldname,fieldvalue);
               // MySqlParameter sqlpar = getSqlParameter("@" + fieldname, fieldvalue);
                //cmd.Parameters.Add(sqlpar);
            }
            #endregion

            subsql1 = subsql1.Substring(0, subsql1.Length - 1);
            subsql2 = subsql2.Substring(0, subsql2.Length - 1);
            strsql = strsql + "(" + subsql1 + ") values (" + subsql2 + ")";
            return strsql;
        }
        
        public virtual int InsertList(string ComndText,string tablename,List<NameValueCollection> nvcList)
        {
            int count = 0;
            MySqlCommand command = new MySqlCommand();
            if (this.SqlCon.State == ConnectionState.Closed)
            {
                this.SqlCon.Open();
            }
            
            command.CommandType = CommandType.Text;
            command.Connection = this.SqlCon;
            command.CommandTimeout = this.CommandTimeout;
            command.CommandText = ComndText;
            
            MySqlTransaction transaction = this.SqlCon.BeginTransaction();
            command.Transaction = transaction;
            if (nvcList != null && nvcList.Count > 0)
            {
                foreach (NameValueCollection nvc in nvcList)
                {
                    if (nvc != null && nvc.Count > 0)
                    {
                        for (int i = 0; i < nvc.Count; i++)
                        {
                           
                            string key = nvc.GetKey(i);
                            string value = nvc.Get(i);
                            string fieldname = key;
                            string fieldvalue = value;
                            Console.WriteLine("");
                            Console.WriteLine(key+":"+value);
                            command.Parameters.AddWithValue(key, value);
                        }
                        try
                        {
                            count += command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        catch (Exception ex)
                        {
                            command.Parameters.Clear();
                            Console.WriteLine(ex.Message);
                        }
                       

                    }
                   
                }
                transaction.Commit();
            }
           
            transaction.Dispose();
            this.SqlCon.Close();
            this.SqlCon.Dispose();
            
            return count;

        }
        public static int DeleteList(string cmdText, List<int> IdList)
        {
            int count = 0;
            //SQLiteCommand command = new SQLiteCommand();
            //using (SQLiteConnection connection = GetSQLiteConnection())
            //{
            //    command.CommandText = cmdText;
            //    connection.Open();
            //    command.Connection = connection;
            //    command.CommandType = CommandType.Text;
            //    SQLiteTransaction transaction = connection.BeginTransaction();
            //    if (IdList != null && IdList.Count > 0)
            //    {
            //        for (int i = 0; i < IdList.Count; i++)
            //        {
            //            command.Parameters.AddWithValue("Id", IdList[i].ToString());
            //            count += command.ExecuteNonQuery();
            //        }
            //        transaction.Commit();
            //    }
            //    connection.Close();
            //}

            return count;
        }

        /// <summary>
        /// 根据SQL语句取得数据
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public virtual DataSet Search(string strsql)
        {
            try
            {
                MySqlDataAdapter sqlda = new MySqlDataAdapter(strsql, this.SqlCon);
                DataSet ds = new DataSet();
                sqlda.SelectCommand.CommandTimeout = this.CommandTimeout;
                sqlda.Fill(ds);
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return ds;
            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                this._ErrorInfo = e.Message;
                return null;
            }


        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="nvcUser"></param>
        /// <returns></returns>
        public virtual int Delete(string strsql, NameValueCollection nvcUser)
        {
            try
            {
                if (strsql.ToLower().IndexOf("delete") < 0)
                    return 0;
                if (strsql.IndexOf("@") < 0)
                    return 0;
                if (nvcUser.Count < 0)
                    return 0;

                MySqlCommand cmd = new MySqlCommand(strsql, this.SqlCon);
                cmd.CommandTimeout = this.CommandTimeout;
                #region 构造参数
                this.AddCommandParameters(cmd, nvcUser);
                #endregion
                cmd.CommandText = strsql;
                cmd.CommandType = CommandType.Text;
                if (this.SqlCon.State == ConnectionState.Closed)
                    this.SqlCon.Open();
                int result = cmd.ExecuteNonQuery();
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return result;

            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                this._ErrorInfo = e.Message;
                return 0;
            }

        }
        /// <summary>
        /// 返回一个DataSet
        /// </summary>
        /// <param name="strsql">带参数的SQL语句</param>
        /// <param name="nvcUser"></param>
        /// <returns></returns>
        public virtual DataSet Search(string strsql, NameValueCollection nvcUser)
        {
            try
            {
                if (strsql.ToLower().IndexOf("select") < 0)
                    return null;
                if (strsql.IndexOf("@") < 0)
                    return null;
                if (nvcUser.Count < 0)
                    return null;

                MySqlCommand cmd = new MySqlCommand(strsql, this.SqlCon);
                cmd.CommandTimeout = this.CommandTimeout;
                #region 构造参数
                this.AddCommandParameters(cmd, nvcUser);
                #endregion
                cmd.CommandText = strsql;
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter sqlda = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlda.Fill(ds);
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return ds;
            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                this._ErrorInfo = e.Message;
                return null;
            }



        }
        /// <summary>
        /// 执行存储过程，返回记录集合,注意out参数必须是整数
        /// </summary>
        /// <param name="procname">存储工程名</param>
        /// <param name="nvc">输入参数列表</param>
        /// <param name="outnvc">输出参数</param>
        /// <returns></returns>
        public virtual DataSet Search(string procname, NameValueCollection nvc, NameValueCollection outnvc)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = this.SqlCon;
            #region 构造参数
            this.AddCommandParameters(cmd, nvc);
            for (int i = 0; i < outnvc.Count; i++)
            {
                string fieldname = outnvc.GetKey(i);
                MySqlParameter sqlpar = new MySqlParameter("@" + fieldname, SqlDbType.Int);
                cmd.Parameters.Add(sqlpar);
                sqlpar.Direction = System.Data.ParameterDirection.Output;
            }

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procname;
            MySqlDataAdapter sqlda = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            for (int i = 0; i < outnvc.Count; i++)
            {
                string parname = outnvc.GetKey(i);
                int n = cmd.Parameters.IndexOf("@" + parname);
                if (n > -1)
                    outnvc.Set(parname, cmd.Parameters[n].Value.ToString());

            }
            this.SqlCon.Close();
            this.SqlCon.Dispose();
            return ds;

            #endregion
        }
        /// <summary>
        /// 执行存储过程，,注意out参数的类型在outnvc的值
        /// 里面定义int 就是 int ,string 就是string 默认是
        /// string
        /// </summary>
        /// <param name="procname">存储工程名</param>
        /// <param name="nvc">输入参数列表</param>
        /// <param name="outnvc">输出参数</param>
        /// <returns></returns>
        public virtual DataSet ExecuteProc(string procname, NameValueCollection nvc, NameValueCollection outnvc)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Connection = this.SqlCon;
            #region 构造参数
            this.AddCommandParameters(cmd, nvc);
            //for (int i = 0; i < outnvc.Count; i++)
            //{
            //    string fieldname = outnvc.GetKey(i);
            //    string fieldvalue = outnvc[i].ToLower();
            //    MySqlParameter sqlpar = null;
            //    if (fieldvalue == "int")
            //    {
            //        sqlpar = new MySqlParameter("@" + fieldname, SqlDbType.Int);
            //    }
            //    else
            //    {
            //        sqlpar = new MySqlParameter("@" + fieldname, MySqlDbType.String, 4000);
            //    }
            //    cmd.Parameters.Add(sqlpar);
            //    sqlpar.Direction = System.Data.ParameterDirection.Output;
            //}

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procname;
            MySqlDataAdapter sqlda = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            //for (int i = 0; i < outnvc.Count; i++)
            //{
            //    string parname = outnvc.GetKey(i);
            //    int n = cmd.Parameters.IndexOf("@" + parname);
            //    if (n > -1)
            //        outnvc.Set(parname, cmd.Parameters[n].Value.ToString());

            //}
            this.SqlCon.Close();
            this.SqlCon.Dispose();
            return ds;

            #endregion



        }
        private void AddCommandParameters(MySqlCommand cmd, NameValueCollection nvcUser)
        {
            #region 构造参数
            for (int i = 0; i < nvcUser.Count; i++)
            {
                string fieldname = nvcUser.GetKey(i);
                string fieldvalue = nvcUser.Get(i);
                MySqlParameter sqlpar = this.getSqlParameter("@" + fieldname, fieldvalue);
                cmd.Parameters.Add(sqlpar);
            }
            #endregion

        }
        /// <summary>
        /// 更新记录,strsql语句必须是参数类型
        /// 如:update mytable set a=@a1,b=@b1 where c=@c1
        /// </summary>
        /// <param name="strsql">sql语句必须是update mytable set a=@a1,b=@b1 where c=@c1</param>
        /// <param name="nvcUser">参数和值对应关系</param>
        /// <returns></returns>
        public virtual bool Update(string strsql, NameValueCollection nvcUser)
        {
            try
            {
                if (strsql.ToLower().IndexOf("update") < 0)
                    return false;
                if (strsql.IndexOf("@") < 0)
                    return false;
                if (nvcUser.Count < 0)
                    return false;
                if (this.SqlCon.State == ConnectionState.Closed)
                    this.SqlCon.Open();
                MySqlCommand cmd = new MySqlCommand(strsql, this.SqlCon);
                cmd.CommandTimeout = this.CommandTimeout;
                this.AddCommandParameters(cmd, nvcUser);

                cmd.CommandText = strsql;
                cmd.CommandType = CommandType.Text;
                int nresult = cmd.ExecuteNonQuery();
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return nresult > 0;
            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                this._ErrorInfo = e.Message + strsql;
                return false;
            }



        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tablename">表明</param>
        /// <param name="nvcWhere">条件集合</param>
        /// <param name="nvcUser">更新字段集合</param>
        /// <returns></returns>
        public virtual bool Update(string tablename, NameValueCollection nvcWhere, NameValueCollection nvcUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(tablename);
            sb.Append(" set ");

            try
            {

                if (tablename == "" || nvcUser.Count == 0)
                    return false;


                StringBuilder sbWhereSql = new StringBuilder();
                if (this.SqlCon.State == ConnectionState.Closed)
                    this.SqlCon.Open();
                MySqlCommand cmd = new MySqlCommand("", this.SqlCon);
                cmd.CommandTimeout = this.CommandTimeout;

                #region SQL语句
                for (int i = 0; i < nvcUser.Count; i++)
                {
                    string fieldname = nvcUser.GetKey(i);
                    string fieldvalue = nvcUser.Get(i);

                    sb.Append(fieldname);
                    sb.Append("=@");
                    sb.Append(fieldname);
                    if (i != (nvcUser.Count - 1))
                        sb.Append(",");

                    MySqlParameter sqlpar = this.getSqlParameter("@" + fieldname, fieldvalue);
                    cmd.Parameters.Add(sqlpar);
                }
                sb.Append(" where ");
                for (int i = 0; i < nvcWhere.Count; i++)
                {
                    string fieldname = nvcWhere.GetKey(i);
                    string pfieldname = fieldname;

                    #region 如果有相同的参数名称则重命名
                    if (nvcUser[fieldname] != null)
                    {
                        pfieldname = fieldname + i.ToString();
                    }
                    #endregion
                    string fieldvalue = nvcWhere.Get(i);

                    sb.Append(fieldname);
                    sb.Append("=@");
                    sb.Append(pfieldname);
                    sb.Append(" and ");

                    MySqlParameter sqlpar = this.getSqlParameter("@" + pfieldname, fieldvalue);
                    cmd.Parameters.Add(sqlpar);
                }
                sb.Append(" 2>1 ");
                #endregion

                cmd.CommandText = sb.ToString();
                cmd.CommandType = CommandType.Text;
                int nresult = cmd.ExecuteNonQuery();
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return nresult > 0;
            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                this._ErrorInfo = e.Message + sb.ToString();
                return false;



            }

        }
        public virtual string GetUpdateSql(string tablename, NameValueCollection nvcWhere, NameValueCollection nvcUser)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(tablename);
            sb.Append(" set ");

            if (tablename == "" || nvcUser.Count == 0)
                return "";


            StringBuilder sbWhereSql = new StringBuilder();

            #region SQL语句
            for (int i = 0; i < nvcUser.Count; i++)
            {
                string fieldname = nvcUser.GetKey(i);
                string fieldvalue = nvcUser.Get(i);

                sb.Append(fieldname);
                sb.Append("=");
                sb.Append("'"+fieldvalue+"'");
                if (i != (nvcUser.Count - 1))
                    sb.Append(",");
            }
            sb.Append(" where ");
            for (int i = 0; i < nvcWhere.Count; i++)
            {
                string fieldname = nvcWhere.GetKey(i);
                string pfieldname = fieldname;

                //#region 如果有相同的参数名称则重命名
                //if (nvcUser[fieldname] != null)
                //{
                //    pfieldname = fieldname + i.ToString();
                //}
                //#endregion
                string fieldvalue = nvcWhere.Get(i);

                sb.Append(fieldname);
                sb.Append("=");
                sb.Append(fieldvalue);
                sb.Append(" and ");

                //MySqlParameter sqlpar = this.getSqlParameter("@" + pfieldname, fieldvalue);
                //cmd.Parameters.Add(sqlpar);
            }
            sb.Append(" 2>1 ");
            return sb.ToString();
            #endregion
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="where">更新条件</param>
        /// <param name="nvcUser">保存了字段名和值的NameValueCollection</param>
        /// <returns></returns>
        public virtual bool Update(string tablename, string where, NameValueCollection nvcUser)
        {
            string strsql = "Update " + tablename + " set  ";
            try
            {

                if (tablename == "" || where == "" || nvcUser.Count == 0)
                    return false;

                if (where.ToLower().IndexOf("where") < 0)
                    where = " where " + where;
                //string strsql="Update "+tablename+" set  ";
                string subsql = "";
                if (this.SqlCon.State == ConnectionState.Closed)
                    this.SqlCon.Open();
                MySqlCommand cmd = new MySqlCommand(strsql, this.SqlCon);
                cmd.CommandTimeout = this.CommandTimeout;

                #region 构造参数
                for (int i = 0; i < nvcUser.Count; i++)
                {
                    string fieldname = nvcUser.GetKey(i);
                    string fieldvalue = nvcUser.Get(i);
                    subsql += fieldname + "=@" + fieldname + ",";
                    MySqlParameter sqlpar = this.getSqlParameter("@" + fieldname, fieldvalue);
                    cmd.Parameters.Add(sqlpar);
                }
                #endregion

                subsql = subsql.Substring(0, subsql.Length - 1);
                strsql = strsql + subsql + " " + where;
                cmd.CommandText = strsql;
                cmd.CommandType = CommandType.Text;
                int nresult = cmd.ExecuteNonQuery();
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return true;
            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                this._ErrorInfo = e.Message + strsql;
                return false;



            }

        }
        /// <summary>
        /// 执行一个存储过程
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="nvc">参数和值列表</param>
        /// <returns></returns>
        public virtual int ExecuteProc(string procname, NameValueCollection nvcUser)
        {
            try
            {


                if (this.SqlCon.State == ConnectionState.Closed)
                    this.SqlCon.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandTimeout = this.CommandTimeout;
                cmd.Connection = this.SqlCon;

                this.AddCommandParameters(cmd, nvcUser);

                //				#region 构造参数
                //				for(int i=0;i<nvcUser.Count;i++)
                //				{
                //					string fieldname=nvcUser.GetKey(i);
                //					string fieldvalue=nvcUser.Get(i);
                //					SqlParameter sqlpar=new SqlParameter("@"+fieldname,fieldvalue);
                //					cmd.Parameters.Add(sqlpar);
                //				}
                //				#endregion
                cmd.CommandText = procname;
                cmd.CommandType = CommandType.StoredProcedure;
                int nresult = cmd.ExecuteNonQuery();
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return nresult;
            }
            catch (Exception e)
            {
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                this._ErrorInfo = e.Message;
                //				OEException.Save("DB.Update:"+procname+";"+this._ErrorInfo);
                return 0;



            }

        }


        /// <summary>
        /// 根据那么valuecollection赋值
        /// </summary>
        /// <param name="assamblyname"></param>
        /// <param name="className"></param>
        /// <param name="o"></param>
        /// <param name="nvcData"></param>
        public static void setClassPropertyValueFromNameValueCollection(string assamblyname,
            string className, object o, NameValueCollection nvcData)
        {
            if (nvcData.Count > 0 && o != null)
            {
                PropertyInfo[] aryP = getPropertyInfo(assamblyname, className);
                #region 循环属性
                foreach (PropertyInfo p in aryP)
                {
                    string fieldname = p.Name;
                    if (nvcData[fieldname]!=null)
                    {
                        p.SetValue(o,nvcData[fieldname],null);
                    }
 
                }
                #endregion
            }

        }
        

        /// <summary>
        /// 根据反射把DataTable的值赋值到类实例
        /// </summary>
        /// <param name="assamblyname"></param>
        /// <param name="className"></param>
        /// <param name="o">实例对象</param>
        /// <param name="dt">保存了数据的DataTable</param>
        public static void setClassPropertyValue(string assamblyname, string className, object o, DataTable dt)
        {
            if (dt.Rows.Count > 0 && o != null)
            {
                PropertyInfo[] aryP = getPropertyInfo(assamblyname, className);
                #region 循环属性
                foreach (PropertyInfo p in aryP)
                {
                    string fieldname = p.Name;
                    if (dt.Columns.Contains(fieldname))
                    {
                        if (dt.Rows[0][fieldname] != System.DBNull.Value)
                        {

                            if (dt.Rows[0][fieldname].GetType().ToString() == "System.DateTime"
                             
                                )
                            {
                                p.SetValue(o, dt.Rows[0][fieldname].ToString(), null);
                            }
                            else if (dt.Rows[0][fieldname].GetType().ToString() == "System.Int32")
                            {
                                int a = 0;
                                Int32.TryParse(dt.Rows[0][fieldname].ToString(), out a);
                                p.SetValue(o, a, null);
                            }
                            else
                            {
                                try
                                {
                                    p.SetValue(o, dt.Rows[0][fieldname], null);
                                }
                                catch
                                {
                                    //

                                }
                            }

                        }
                    }
                }
                #endregion
            }

        }
        /// <summary>
        /// 使用反射把类的属性保存到NameValueCollection中
        /// 				foreach (DBAttribute attr in DBAttribute.GetCustomAttributes(aryP[i])) 
        //	{
        //		if (attr.GetType() == typeof(DBAttribute))
        //	{
        //		MessageBox.Show(attr.PKField.ToString());
        //		MessageBox.Show(attr.IgnoreBlankAndZero.ToString());
        //		MessageBox.Show(attr.FieldType.ToString());
        //
        //	}
        //}
        /// 
        /// </summary>
        /// <param name="assamblyname">程序集合名称</param>
        /// <param name="className">类名称</param>
        /// <param name="o">实例对象</param>
        /// <returns></returns>
        public static NameValueCollection getNameValueCollection(string assamblyname, string className, object o)
        {
            NameValueCollection nvc = new NameValueCollection();
            PropertyInfo[] aryP = getPropertyInfo(assamblyname, className);
            foreach (PropertyInfo p in aryP)
            {
                string fieldvalue = p.GetValue(o, null).ToString();
                string fieldname = p.Name;
                if (IsNeedAddToCollection(p, fieldvalue) == true)
                    //if (fieldvalue!="0" && fieldvalue!="")
                    nvc.Add(fieldname, fieldvalue);
            }
            return nvc;
        }
        public static string getInsertSql(string assamblyname, string className, object o)
        {
            if (className == "" )
                return "";
            string strsql = "Insert Into " + className;
            string subsql1 = "", subsql2 = "";
            PropertyInfo[] aryP = getPropertyInfo(assamblyname, className);
            foreach (PropertyInfo p in aryP)
            {
               
                //string fieldvalue = p.GetValue(o, null).ToString();
                string fieldname = p.Name;
                if (fieldname != "Id")
                {
                    subsql1 += fieldname + ",";
                    subsql2 += "@" + fieldname + ",";
                }
            }
            subsql1 = subsql1.Substring(0, subsql1.Length - 1);
            subsql2 = subsql2.Substring(0, subsql2.Length - 1);
            strsql = strsql + "(" + subsql1 + ") values (" + subsql2 + ")";
            return strsql;
        }
        /// <summary>
        /// 判断是否需要插入集合中
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fieldvalue"></param>
        /// <returns></returns>
        public static bool IsNeedAddToCollection(PropertyInfo p, string fieldvalue)
        {
            DBAttribute a = DBAttribute.GetCustomAttribute(p, typeof(DBAttribute)) as DBAttribute;
            if (a == null)
            {
                if (fieldvalue != "-1" && fieldvalue != "")
                    return true;
                else
                    return false;
            }
            else
            {
                if (a.PKField == true || a.IgnoreBlankAndZero)
                    return false;
                else
                    return true;
            }


        }
        /// <summary>
        /// 使用反射把类的属性保存到NameValueCollection中
        /// </summary>
        /// <param name="assamblyname">程序集合名称</param>
        /// <param name="className">类名称</param>
        /// <param name="o">实例对象</param>
        /// <param name="ignorevalue">如果属性值为此值时候不添加到集合中,如果为null则添加所有属性名和值到集合中</param>
        /// <returns></returns>
        public static NameValueCollection getNameValueCollection(string assamblyname,
            string className, object o, string ignorevalue)
        {
            NameValueCollection nvc = new NameValueCollection();
            PropertyInfo[] aryP = getPropertyInfo(assamblyname, className);
            foreach (PropertyInfo p in aryP)
            {
                string fieldvalue = p.GetValue(o, null).ToString();
                string fieldname = p.Name;
                if (ignorevalue == null)
                    nvc.Add(fieldname, fieldvalue);
                else
                {
                    if (fieldvalue != ignorevalue)
                        nvc.Add(fieldname, fieldvalue);
                }
            }
            return nvc;
        }
        /// <summary>
        /// 取得类的属性集合
        /// </summary>
        /// <param name="assamblyname">程序集合的名称</param>
        /// <param name="className">类的名称</param>
        /// <returns></returns>
        public static PropertyInfo[] getPropertyInfo(string assamblyname, string className)
        {
            string key = assamblyname + "." + className;
            Assembly a = Assembly.Load(assamblyname);
            NameValueCollection nvc = new NameValueCollection();
            Type[] t = a.GetTypes();
            foreach (Type t1 in t)
            {
                string classname = t1.FullName;
               
                int npos = classname.LastIndexOf(".") + 1;
                classname = classname.Substring(npos, classname.Length - npos);
                if (classname == className)
                {
                    PropertyInfo[] aryP = t1.GetProperties();
                    return aryP;
                }
            }
            return null;

        }

        /// <summary>
        /// 通过反射取得类名列表并用,分割
        /// </summary>
        /// <param name="assamblyname"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static string GetClassPropertyNameList(string assamblyname, string className)
        {
            PropertyInfo[] aryP = getPropertyInfo(assamblyname, className);
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo p in aryP)
            {
                sb.Append(p.Name);
                sb.Append(",");
            }
            string result = sb.ToString();
            if (result.EndsWith(",") && result.Length > 1)
                return result.Substring(0, result.Length - 1);
            return result;

        }
        public virtual bool ExecuteTransaction(string ComndText)
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                if (this.SqlCon.State == ConnectionState.Closed)
                {
                    this.SqlCon.Open();
                }

                command.CommandType = CommandType.Text;
                command.Connection = this.SqlCon;
                command.CommandTimeout = this.CommandTimeout;
                command.CommandText = ComndText;

                MySqlTransaction transaction = this.SqlCon.BeginTransaction();
                command.Transaction = transaction;
                int result = command.ExecuteNonQuery();
                Console.WriteLine("tansaction:"+result);
                if (result>0)
                {

                }
                else
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    this.SqlCon.Close();
                    this.SqlCon.Dispose();
                    return false;
                }
                transaction.Commit();
                transaction.Dispose();
                this.SqlCon.Close();
                this.SqlCon.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



    }
}
