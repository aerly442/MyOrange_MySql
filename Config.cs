 using System;
using System.Data;
using System.IO;

namespace MyOrange
{
	/// <summary>
	/// 读取配置文件信息，仅仅适用于DataSet 方式保存
	/// 为xml文件的信息读取
	/// 类似于
	/// 2007.1.15 之前增加
	/// <table>
	///    <name>abc</name>
	///    <value>fdsf</value>
	/// </table>
	/// </summary>
	public class Config
	{
		/// <summary>
		/// 构造函数请传入完全路径
		/// </summary>
		/// <param name="configfilenname"></param>
		/// 
		private DataSet ds=null;
		public Config(string configfilenname)
		{
			ds=new DataSet();
			if (File.Exists(configfilenname))
			{
				ds.ReadXml(configfilenname);
			}
		}
		/// <summary>
		/// 获取配置中的每个表（节点）数据
		/// 如果无则返回null
		/// </summary>
		/// <param name="tablename">表(节点)名称</param>
		/// <returns></returns>
		public DataTable getConfigDataTable(string tablename)
		{
			if (ds.Tables.Contains(tablename))
				return ds.Tables[tablename];
			else
				return null;
		
		}
		/// <summary>
		/// 返回特定表的值，值经过条件筛选,如果有多个记录只
		/// 返回第一行;如果没有表或条件不符合则返回""
		/// </summary>
		/// <param name="tablename">表（节点)</param>
		/// <param name="filter">格式等同于DataTable.Select()</param>
		/// <param name="fieldname">字段（节点）名</param>
		/// <returns></returns>
		public string getConfigNodeValue(string tablename,string filter,string fieldname)
		{

			DataRow[] dr=this.getConfigDataRow(tablename,filter);
			if (dr==null || dr.Length==0)
				return "";
			if (ds.Tables[tablename].Columns.Contains(fieldname))
					return dr[0][fieldname].ToString();
				else
					return "";
		}
		/// <summary>
		/// 返回特定表的值，值经过条件筛选
		/// 如果没有表或条件不符合则返回null
		/// </summary>
		/// <param name="tablename">表（节点)</param>
		/// <param name="filter">格式等同于DataTable.Select()</param>
		/// <returns></returns>
		public DataRow[] getConfigDataRow(string tablename,string filter)
		{
			DataTable dt=this.getConfigDataTable(tablename);
			if (dt!=null)
			{
				return dt.Select(filter);
			}
			return null;
		
		}
		/// <summary>
		/// 直接返回对应表（节点）第一行的值
		/// </summary>
		/// <param name="tablename">表（节点）</param>
		/// <param name="fieldname">字段名称</param>
		/// <returns></returns>
		public string getConfigFirstNodeValue(string tablename,string fieldname)
		{
			DataTable dt=this.getConfigDataTable(tablename);
			if (dt!=null && dt.Rows.Count>0 && dt.Columns.Contains(fieldname))
				return dt.Rows[0][fieldname].ToString();
			else
				return "";
		
		}
		/// <summary>
		/// 直接返回对应表（节点）第一行的值
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <param name="tablename">表（节点）</param>
		/// <param name="fieldname">字段名称</param>
		/// <returns></returns>
		public static string getConfigFirstValue(string filename,string tablename,string fieldname)
		{
			Config config=new Config(filename);
			return config.getConfigFirstNodeValue(tablename,fieldname);
			
		}
		/// <summary>
		/// 直接返回符合条件对应表（节点）第一行的值
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <param name="tablename">表（节点）</param>
		/// <param name="fieldname">字段名称</param>
		/// <param name="filter">条件</param>
		/// <returns></returns>
		public static string getConfigValue(string filename,string tablename,string fieldname,string filter)
		{
			Config config=new Config(filename);
			return config.getConfigNodeValue(tablename,filter,fieldname);	
		
		}
	}
}