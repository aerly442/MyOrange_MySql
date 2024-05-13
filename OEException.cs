/* 
* 一个异常处理类
*/  
using System;
using System.Diagnostics;
using System.IO;

namespace MyOrange
{
	/// <summary>
	/// 一个异常处理类
	/// </summary>
	public class OEException
	{
		public OEException()
		{
		}
		/// <summary>
		/// 保存异常信息
		/// </summary>
		/// <param name="e">异常</param>
		public static void  Save(Exception e)
		{
			if(e.GetType().ToString()!="System.Threading.ThreadAbortException")
			{
				AddToFileLog("源代码:"+e.Source+";错误信息:"+e.Message+";堆栈:"+e.StackTrace+";时间:"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\n\n");
			}
		}
		/// <summary>
		///  保存异常信息
		/// </summary>
		/// <param name="e"></param>
		/// <param name="info"></param>
		public static void Save(Exception e,string info)
		{
			
			if(e.GetType().ToString()!="System.Threading.ThreadAbortException")
			{
				string strvalue="源代码:"+e.Source+";\n错误信息:"+e.Message+";\n堆栈:"+e.StackTrace
					+";\n信息:"+info+";\n时间:"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+";";
				AddToFileLog(strvalue);	
			}
		}
		/// <summary>
		/// 在Web.Config 的key
		/// </summary>
		public static string WebConfigKey="FileLog";
		/// <summary>
		/// ֱ根据WebConfigKey保存错误信息
		/// </summary>
		/// <param name="info"></param>
		public static void AddToFileLog(string info)
		{
			string filename=Web.GetConfigString(WebConfigKey);
			if (filename==null || filename=="" )
				filename="/Images/log/BlogError.html";
            if (filename != null && filename.Length > 0 && System.Web.HttpContext.Current!=null)
			{
				
				string file=System.Web.HttpContext.Current.Server.MapPath(filename);
                if (File.Exists(file))
                {
                    using (StreamWriter sw = new StreamWriter(file, true, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        sw.WriteLine(info + ";时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        sw.Close();
                    }
                }
                else
                {
                    AddToFileLog(info, file);
                }
			
			}
		
		}
		/// <summary>
		/// 保存一个异常信息
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="path"></param>
		public static void AddToFileLog(string info,string path)
		{
			 
				if (System.IO.Directory.Exists(path))
				{
					string file =path +DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString()
						+DateTime.Now.Day.ToString()+".txt";
					using (StreamWriter sw=new StreamWriter(file,true,System.Text.Encoding.GetEncoding("gb2312")))
					{
						sw.WriteLine(info+";时间:"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
						sw.Close();
					}
				}
			
			 
		
		}
		/// <summary>
		/// 保存一个异常信息
		/// </summary>
		/// <param name="eventLogName">事件名称</param>
		/// <param name="errorInfo">错误信息</param>
		public static void Save(string eventLogName,string errorInfo)
		{
				AddToFileLog(errorInfo);
		}
		/// <summary>
		///  保存一个异常信息
		/// </summary>
		/// <param name="erroInfo">错误信息</param>
		public static void Save(string erroInfo)
		{
			AddToFileLog("错误信息:"+erroInfo);	
		}
		/// <summary>
		///  保存一个异常信息
		/// </summary>
		/// <param name="eventLogName">事件名称</param>
		/// <param name="erroInfo">错误信息</param>
		private static void AddToLog(string eventLogName,string erroInfo)
		{
			AddToFileLog("错误信息:"+erroInfo);
			
		}
	}
}
