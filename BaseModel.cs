using System;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace MyOrange
{
	/// <summary>
	/// BaseModel 的摘要说明。
	/// 所有数据库映射类的父亲
	/// </summary>
	public class BaseModel
	{
		/// <summary>
		/// 数据库组件
		/// </summary>
		protected DB db=null;
		private   string assemblyname="";
		private   string classname="";
		/// <summary>
		/// 错误信息
		/// </summary>
		public    string ErrorInfo="";
		/// <summary>
		/// 表名称
		/// </summary>
		protected string tablename="";   
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="webconfig_key">web.config 的key</param>
		public BaseModel(string webconfig_key)
		{
			setname();
			db=new DB(getConnectionString(webconfig_key));
		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="connectionString">连接字符串</param>
		/// <param name="isConnectionString">是否使用此字符串</param>
		public BaseModel(string connectionString,bool isConnectionString)
		{
			setname();
			db=new DB(connectionString);
		}
		/// <summary>
		/// 重写此类可以设置连接字符串
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public virtual string getConnectionString(string key)
		{
			return Web.GetConfigString("db");
		}
		/// <summary>
		/// 构造函数
		/// </summary>
		public BaseModel():this("db")
		{

		}
		/// <summary>
		/// 根据条件给模型赋值
		/// </summary>
		/// <param name="id">条件</param>
		/// <param name="idfieldname">对应的字段</param>
		public BaseModel(string id,string idfieldname):this("db")
		{
			NameValueCollection nvc=new NameValueCollection();
			nvc.Add(id,idfieldname);
			DataTable dt=this.Search(null,nvc,"");
			if (dt!=null && dt.Rows.Count>0)
			{
				DB.setClassPropertyValue(assemblyname,classname,this,dt);
			}	
		}
		protected void setname()
		{
			Type t			=this.GetType();
			string name		=t.FullName;
			string[] aryName=name.Split('.');
			if (aryName.Length==2)
			{
				this.assemblyname=aryName[0];
				this.classname	=aryName[1];
				tablename		=this.classname;
			}
			else
			{
				this.classname		=aryName[aryName.Length-1];
				tablename		=this.classname;
				this.assemblyname=name.Replace("."+this.classname,"");

			
			}
		}

        /// <summary>
        /// MySQL 不支持默认select 名称,* 的方式，通过反射取回所有字段名称
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetFieldsName()
        {
            return DB.GetClassPropertyNameList(this.assemblyname, this.classname);
        }

		/// <summary>
		/// 插入一条记录
		/// </summary>
		/// <returns></returns>
		public virtual bool Insert()
		{
			NameValueCollection nvc=DB.getNameValueCollection(assemblyname,classname,this);
			bool result=db.Insert(this.tablename,nvc);

			this.ErrorInfo +=db.ErrorInfo;
            if (result == false)
            {
                //OEException.AddToFileLog(ErrorInfo);
            }
			return result;
		}
        public virtual string GetInsertSqlNew()
        {
            NameValueCollection nvc = DB.getNameValueCollection(assemblyname, classname, this);
            string result = db.GetInsertSqlNew(this.tablename, nvc);
            return result;
        }
        public virtual int InsertList(List<NameValueCollection> nvcList)
        {
            return db.InsertList(DB.getInsertSql(assemblyname,classname,this),this.tablename,nvcList);
        }
		public virtual bool Delete(int id)
		{
			return this.Delete(getDefaultNVC(id));
		}
		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="nvc"></param>
		/// <returns></returns>
		public virtual bool Delete(NameValueCollection nvcWhere)
		{
			if (this.IsRightCondition(nvcWhere)==false)
			{
				return false;
			}
			string strsql="delete from "+this.tablename+"  where ";
			strsql=strsql+getWhere(nvcWhere);
			bool result=this.db.Delete(strsql,nvcWhere)>0;
			if (result==false)
				this.ErrorInfo +=db.ErrorInfo+";"+strsql;
            if (result == false)
            {
                //OEException.AddToFileLog( ErrorInfo);
            }
			return result;
		}
        public virtual int ClearDataAll()
        {
            string strsql = "TRUNCATE table "+this.tablename;
            int count = this.db.ExecuteSql(strsql);
            return count;
        }
		private string getWhere(NameValueCollection nvc)
		{
			StringBuilder sb=new StringBuilder();
			for(int i=0;i<nvc.Count;i++)
			{
				sb.Append(nvc.GetKey(i));
				sb.Append("=@");
				sb.Append(nvc.GetKey(i));
				sb.Append(" and ");
			}
			sb.Append(" 2>1");
			return sb.ToString();
		
		}
		private bool IsRightCondition(NameValueCollection nvc)
		{
			if (nvc==null || nvc.Count==0)
				return false;
			else
				return true;
		
		}
		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="nvcWhere"></param>
		/// <returns></returns>
		public virtual bool Update(NameValueCollection nvcWhere)
		{
			if (this.IsRightCondition(nvcWhere)==false)
			{
				return false;
			}
			NameValueCollection nvc=DB.getNameValueCollection(assemblyname,classname,this);
			bool result=this.db.Update(this.tablename,nvcWhere,nvc);
			this.ErrorInfo +=db.ErrorInfo;
            if (result == false)
            {
                //OEException.AddToFileLog( ErrorInfo);
            }
			return result;
		}
        public virtual string GetUpdateSql(NameValueCollection nvcWhere)
        {
            if (this.IsRightCondition(nvcWhere) == false)
            {
                return "";
            }
            
            NameValueCollection nvc = DB.getNameValueCollection(assemblyname, classname, this);
            string result = this.db.GetUpdateSql(this.tablename, nvcWhere, nvc);
            return result;
        }
		/// <summary>
		/// 默认更新id
		/// </summary>
		/// <returns></returns>
		public virtual bool Update(int id)
		{
			return this.Update(getDefaultNVC(id));
		}
        public virtual string GetUpdateSql(int id)
        {
            return this.GetUpdateSql(getDefaultNVC(id));
        }
		/// <summary>
		/// 查询
		/// </summary>
		/// <param name="fieldnamelist">要查询的字段如果为null 则 select * </param>
		/// <param name="nvcWhere">条件集合如果为null 则没有条件</param>
		/// <param name="orderby">排序字段</param>
		/// <returns></returns>
		public virtual DataTable Search(string fieldnamelist,NameValueCollection nvcWhere,string orderby)
		{
			DataSet ds=null;

			#region 构造SQL语句
			string strsql="select "+fieldnamelist;
			strsql +=" from "+this.tablename;
            
			if (nvcWhere!=null &&nvcWhere.Count>0)
			{
				strsql +=" where "+this.getWhere(nvcWhere);
			}
			if (orderby!="")
				strsql +=" order by "+orderby;
			#endregion 
            Console.WriteLine(strsql);
			try
			{
				if (nvcWhere==null || nvcWhere.Count==0)
					ds=db.Search(strsql);
				else
					ds=db.Search(strsql,nvcWhere);
				
				if (ds!=null && ds.Tables.Count>0)
					return ds.Tables[0];
				else
				{
					this.ErrorInfo +=db.ErrorInfo+strsql;
					return null;
				}
			}
			catch(Exception e)
			{
				this.ErrorInfo +=e.Message+";"+strsql;
				return null;
			}
		}
		/// <summary>
		/// 默认查询id
		/// </summary>
		/// <returns></returns>
		public virtual DataTable Search(int id)
		{
			return 	 this.Search("*",this.getDefaultNVC(id),"");
		}
		private NameValueCollection getDefaultNVC(int id)
		{
			NameValueCollection nvc=new NameValueCollection();
			nvc.Add("id",id.ToString());
			return nvc;
		}

        public virtual bool ExecuteTransaction(string cmdtext)
        {
            return db.ExecuteTransaction(cmdtext);
        }

        public virtual void DeleteBy(string where)
        {
            string sqlStr = "delete from " + this.tablename + " where " + where;
            this.ExecuteTransaction(sqlStr);
        }

        public DataTable SearchList(int page, int pagesize, out string totalcount, string where, string orderby)
        {
            if (where.Trim().ToLower().StartsWith("where") == false)
                where = " where " + where;
            if (orderby.Trim().ToLower().StartsWith("order") == false)
                orderby = " order by " + orderby;

            string strsql = "select 'allcount' as 'allcount', count(*) as totalcount from " + this.tablename;
            strsql += where;
            strsql += ";";
            strsql += "select 'all' as 'all', " + GetFieldsName() + " from " + this.tablename;
            strsql += where;
            strsql += orderby + Common.GetLimit(page, pagesize);

            return GetSpliterPageDataTable(strsql, out totalcount);


        }

        public DataTable GetSpliterPageDataTable(string strsql, out string totalcount)
        {
            DataSet ds = Common.RenameDataSet(this.db.Search(strsql));

            totalcount = "0";
            if (ds == null)
                return null;

            if (ds.Tables.Contains("allcount") && ds.Tables["allcount"] != null && ds.Tables["allcount"].Rows.Count > 0)
                totalcount = ds.Tables["allcount"].Rows[0]["totalcount"].ToString();
            if (ds.Tables.Contains("all"))
            {
                return ds.Tables["all"];
            }
            else
                return null;
        }


        public DataTable Search(int page, int pagesize, out string totalcount,
        string fieldname, string fieldvalue)
        {
            return Search(page, pagesize, out totalcount, fieldname, fieldvalue, "createtime");
        }

        public virtual DataTable Search(int page, int pagesize, out string totalcount, string fieldname, string fieldvalue,
    string orderbyfieldname)
        {

            string strsql = "select 'allcount' as 'allcount', count(*) as totalcount from " + this.tablename;
            strsql += Common.GetWhere(fieldname, fieldvalue);
            strsql += ";";
            strsql += "select 'all' as 'all', " + GetFieldsName() + " from " + this.tablename;
            strsql += Common.GetWhere(fieldname, fieldvalue);
            strsql += " order by " + orderbyfieldname + " desc " + Common.GetLimit(page, pagesize);
            return GetSpliterPageDataTable(strsql, out totalcount);


        }

        public virtual DataTable Search(int page, int pagesize, out string totalcount, string orderbyfieldname)
        {

            string strsql = "select 'allcount' as 'allcount', count(*) as totalcount from " + this.tablename;

            strsql += ";";
            strsql += "select 'all' as 'all', " + GetFieldsName() + " from " + this.tablename;

            strsql += " order by " + orderbyfieldname + " desc " + Common.GetLimit(page, pagesize);
            DataSet ds = Common.RenameDataSet(this.db.Search(strsql));

            return GetSpliterPageDataTable(strsql, out totalcount);


        }

    }
}