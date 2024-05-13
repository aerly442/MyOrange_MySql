using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Caching;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;


namespace MyOrange
{

	/// <summary>
	/// 一个通用的web类
	/// </summary>
    public class Web
    {


        public Web()
        {

        }


        public static string AppPath = "";
        static Web()
        {
            AppPath = ApplicationPath();
        }
        private static string CookieDomain = "xxx.com";

        /// <summary>
        /// 设置一个cookie域名
        /// </summary>
        public static void setCookieDomain(string domain)
        {
            CookieDomain = domain;
        }


        /// <summary>
        /// 输出一个提示框
        /// </summary>
        public static void Alert(string info)
        {
            string script = "<script language='javascript'>"
                + "alert('" + info + "');</script>";
            HttpContext.Current.Response.Write(script);

        }

        /// <summary>
        /// 输出一个提示框
        /// </summary>
        public static void Alert(string info, System.Web.UI.Page page)
        {
            string script = "<script language='javascript'>"
                + "alert('" + info + "');</script>";
            page.RegisterClientScriptBlock(Web.getGUIDString(), script);
        }


        /// <summary>
        /// 输出一个提示框
        /// </summary>
        public static void Alert(System.Web.UI.Page page, string info, string defer, string newurl)
        {
            string script = "<script language='javascript' {0}>"
                + "alert('" + info + "');{1}</script>";
            if (defer.ToLower() != "true")
                defer = "";
            else
                defer = "defer=true";
            if (newurl != "")
            {
                newurl = "location.href='" + newurl + "'";
            }
            script = string.Format(script, defer, newurl);
            page.RegisterClientScriptBlock(Web.getGUIDString(), script);


        }


        /// <summary>
        /// 输出一个提示框
        /// </summary>
        public static void Alert(string info, string defer, string newurl)
        {
            string script = "<script language='javascript' {0}>"
                + "alert('" + info + "');{1}</script>";
            if (defer.ToLower() != "true")
                defer = "";
            else
                defer = "defer=true";
            if (newurl != "")
            {
                newurl = "location.href='" + newurl + "'";
            }
            script = string.Format(script, defer, newurl);
            HttpContext.Current.Response.Write(script);


        }


        /// <summary>
        /// 添加一个脚本
        /// </summary>
        public static void AddScript(WebControl ctrl, string key, string script)
        {
            ctrl.Attributes.Add(key, script);

        }

        /// <summary>
        /// 是否是一个中文
        /// </summary>
        public static bool isAChineseLetter(string strvalue)
        {
            string strreg = "[^\u4E00-\u9FA0]";
            bool result = System.Text.RegularExpressions.Regex.IsMatch(strvalue, strreg, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return !result;
            //if 

        }


        /// <summary>
        /// 左边截断
        /// </summary>
        public static string LeftString(string strvalue, int length)
        {

            if (strvalue.Length * 2 < length)
            {
                return strvalue;
            }
            else
            {
                int maxLength = length * 2;
                int currLength = 0;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < strvalue.Length; i++)
                {
                    string str = strvalue.Substring(i, 1);
                    if (isAChineseLetter(str))
                    {
                        currLength += 2;
                    }
                    else
                    {
                        currLength += 1;
                    }
                    sb.Append(str);
                    if (currLength >= maxLength)
                    {
                        return sb.ToString();
                    }

                }
                return strvalue;

            }


        }

        /// <summary>
        /// 左边截断
        /// </summary>
        public static string Left(string info, int length)
        {
            if (info == null)
                return "";
            else if (info.Length > length)
            {
                return info.Substring(0, length);
            }
            else
                return info;
        }

        /// <summary>
        /// 右截断
        /// </summary>
        public static string Right(string info, int length)
        {
            if (info == null)
                return "";
            else if (info.Length > length)
            {
                return info.Substring(info.Length - length, length);
            }
            else
                return info;
        }


        /// <summary>
        /// 是否本地环境
        /// </summary>
        public static bool IsLocal()
        {
            try
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                if (ip == "127.0.0.1" || ip.IndexOf("192.168.") >= 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }


        }


        /// <summary>
        /// 添加一个cookie
        /// </summary>
        public static void AddCookie(string strname, string strvalue, int expiresminute)
        {
            strvalue = System.Web.HttpContext.Current.Server.UrlEncode(strvalue);
            HttpCookie cookie = new HttpCookie(strname, strvalue);
            setCookieInfo(cookie, expiresminute);
            HttpContext.Current.Response.SetCookie(cookie);

        }

        /// <summary>
        /// 设置一个cookie
        /// </summary>
        private static void setCookieInfo(HttpCookie cookie, int expiresminute)
        {
            if (CookieDomain != "")
            {
                cookie.Domain = "xxxx.com";
            }
            if (expiresminute != 0)
            {
                cookie.Expires = DateTime.Now.Add(new TimeSpan(0, 0, expiresminute, 0, 0));
            }

        }


        /// <summary>
        /// 设置一个cookie
        /// </summary>
        public static void AddCookieNoUrlEncode(string strname, string strvalue, int expiresminute)
        {
            HttpCookie cookie = new HttpCookie(strname, strvalue);
            setCookieInfo(cookie, expiresminute);
            HttpContext.Current.Response.SetCookie(cookie);

        }

        /// <summary>
        /// 设置一个Cache
        /// </summary>
        public static void AddCache(string strname, string strvalue, int expiresminute)
        {
            HttpContext.Current.Cache.Add(strname, strvalue, null, DateTime.Now.AddMinutes(expiresminute),
                TimeSpan.Zero, CacheItemPriority.NotRemovable, null);

        }


        /// <summary>
        /// 设置一个Cache
        /// </summary>
        public static void AddCache(string strname, object ovalue, int expiresminute, object o)
        {
            HttpContext.Current.Cache.Add(strname, ovalue, null, DateTime.Now.AddMinutes(expiresminute),
                TimeSpan.Zero, CacheItemPriority.NotRemovable, null);

        }

        /// <summary>
        /// 设置一个Cache
        /// </summary>
        public static void AddCache(string name, object ovalue, string filename)
        {
            if (filename.IndexOf(":") < 0)
                filename = HttpContext.Current.Server.MapPath(filename);
            HttpContext.Current.Cache.Insert(name, ovalue, new CacheDependency(filename));
        }


        /// <summary>
        /// 设置一个cookie
        /// </summary>
        public static void AddCookie(string strname, string strvalue, int expirestime, string timetype)
        {
            strvalue = System.Web.HttpContext.Current.Server.UrlEncode(strvalue);
            HttpCookie cookie = new HttpCookie(strname, strvalue);
            cookie.Domain = "oeeee.com";
            string t = timetype.ToLower();
            if (t == "d")
            {
                cookie.Expires = DateTime.Now.Add(new TimeSpan(expirestime, 0, 0, 0, 0));
            }
            else if (t == "h")
            {
                cookie.Expires = DateTime.Now.Add(new TimeSpan(0, expirestime, 0, 0, 0));
            }
            else
            {
                cookie.Expires = DateTime.Now.Add(new TimeSpan(0, 0, expirestime, 0, 0));
            }
            HttpContext.Current.Response.SetCookie(cookie);

        }


        /// <summary>
        /// 设置一个cookie
        /// </summary>
        public static void AddCookieNoUrlEncode(string strname, string strvalue, int expirestime, string timetype)
        {
            HttpCookie cookie = new HttpCookie(strname, strvalue);
            if (CookieDomain != "")
                cookie.Domain = "oeeee.com";
            string t = timetype.ToLower();
            if (t == "d")
            {
                cookie.Expires = DateTime.Now.Add(new TimeSpan(expirestime, 0, 0, 0, 0));
            }
            else if (t == "h")
            {
                cookie.Expires = DateTime.Now.Add(new TimeSpan(0, expirestime, 0, 0, 0));
            }
            else
            {
                cookie.Expires = DateTime.Now.Add(new TimeSpan(0, 0, expirestime, 0, 0));
            }
            HttpContext.Current.Response.SetCookie(cookie);

        }


        /// <summary>
        /// 获取cookie
        /// </summary>
        public static string GetCookie(string strname)
        {

            if (HttpContext.Current.Request.Cookies.Get(strname) != null)
                return System.Web.HttpContext.Current.Server.UrlDecode(
                    HttpContext.Current.Request.Cookies.Get(strname).Value);
            else
                return "";
        }


        /// <summary>
        /// 获取cache
        /// </summary>
        public static string GetCache(string strname)
        {
            if (HttpContext.Current.Cache.Get(strname) != null)
                return HttpContext.Current.Cache[strname].ToString();
            else
                return "";
        }

        /// <summary>
        /// 获取cache
        /// </summary>
        public static object GetCache(string strname, object o)
        {
            if (HttpContext.Current.Cache.Get(strname) != null)
                return HttpContext.Current.Cache[strname];
            else
                return null;
        }


        /// <summary>
        /// 获取int
        /// </summary>
        public static int getInt(object o)
        {
            try
            {
                return Convert.ToInt32(o.ToString());
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// 是否是一个数字
        /// </summary>
        public static bool IsNumberic(string strvalue)
        {
            string strReg = @"^\d+$";
            Regex reg = new Regex(strReg);
            return reg.IsMatch(strvalue);
        }

        /// <summary>
        /// 替换特殊字符
        /// </summary>
        public static string ReplaceSpecialChar(string strvalue, string strspecial)
        {
            if (strspecial.IndexOf("|") < 0)
                return strvalue;
            else
            {
                string[] arys = strspecial.Split('|');
                foreach (string s in arys)
                {
                    string strs = s.Trim();
                    if (strs == "'")
                        strvalue = strvalue.Replace("'", "''");
                    else if (strs != "")
                        strvalue = strvalue.Replace(strs, "");

                }
                return strvalue;

            }

        }


        /// <summary>
        /// MD5编码
        /// </summary>
        public static string EncodeByMD5(string strvalue)
        {
            string key = "*()RYTGJK";
            strvalue = key + strvalue;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(strvalue, "md5");


        }


        /// <summary>
        /// 过去GUID
        /// </summary>
        public static string getGUIDString()
        {
            return System.Guid.NewGuid().ToString();

        }


        /// <summary>
        /// 获取配置不同的文件
        /// </summary>
        public static string GetWEBString(string webname)
        {
            if (Web.IsLocal())
                return ConfigurationSettings.AppSettings.Get("Local" + webname);
            else
                return ConfigurationSettings.AppSettings.Get(webname);
        }



        /// <summary>
        /// 获取配置字符串
        /// </summary>
        public static string GetConfigString(string key)
        {
            return ConfigurationSettings.AppSettings.Get(key);

        }

        /// <summary>
        /// 替换回车符
        /// </summary>
        public static string GetBRString(string strvalue)
        {
            return GetBRString(strvalue, "");

        }


        /// <summary>
        /// 替换回车符
        /// </summary>
        public static string GetBRString(string strvalue, string addHtml)
        {
            strvalue = strvalue.Replace("\r\n", addHtml + "<br>");
            return strvalue;
        }


        /// <summary>
        /// 获取随机字符串
        /// </summary>

        public static int GetRndNumber(int min, int max)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return r.Next(min, max);
        }



        /// <summary>
        /// 过去日期字符串
        /// </summary>
        public static string getDateTimeFormatString()
        {
            //string str=DateTime.Now.ToShortDateString("")+DateTime.Now.ToShortTimeString()
            string str = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0')
                + DateTime.Now.Day.ToString().PadLeft(2, '0')
                + DateTime.Now.Hour.ToString().PadLeft(2, '0')
                + DateTime.Now.Minute.ToString().PadLeft(2, '0')
                //+DateTime.Now.ToShortTimeString("HHMM")
                + GetRndNumber(1000, 9999).ToString();
            str = str.Replace(" ", "");
            str = str.Replace(":", "");
            str = str.Replace("-", "");
            return str;

        }


        /// <summary>
        /// 返回路径
        /// </summary>
        public static string ApplicationPath()
        {
            if (HttpContext.Current == null)
                return "";
            if (HttpContext.Current.Request.ApplicationPath == "/")
                return "";
            else
                return HttpContext.Current.Request.ApplicationPath;

        }

        /// <summary>
        /// 替换html代码
        /// </summary>
        public static string ClearHTMLCode(string strvalue)
        {

            string regEx = "<.+?>";
            Regex reg = new Regex(regEx);
            string result = reg.Replace(strvalue, "");
            return result;
        }
    }
}
