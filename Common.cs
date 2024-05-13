using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using System.Collections;

namespace MyOrange
{
    public class Common
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Common()
        {

            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 去掉盘符获取文件路径和文件名
        /// </summary>
        /// <param name="filename">绝对路径</param>
        /// <returns></returns>
        public static string GetDisplayImportFileName(string filename)
        {
            if (filename != "")
            {
                int pos = filename.LastIndexOf("\\");
                if (pos > 0)
                {
                    string strvalue = MyOrange.Web.Right(filename, filename.Length - pos - 1);
                    return strvalue;
                }
                else
                {
                    return filename;
                }
            }
            else
                return "";

        }
        /// <summary>
        /// 过去正确的SQL串
        /// </summary>
        /// <param name="param">SQL串</param>
        /// <returns></returns>
        public static string GetRightSQLParam(string param)
        {
            param = param.Replace("'", "");
            param = param.Replace("insert", "");
            param = param.Replace("update", "");
            param = param.Replace("--", "");
            param = param.Replace("<!", "");
             
            return param;


        }
        /// <summary>
        /// 根据其实标识，形成对应SQL语句：limit start,end
        /// </summary>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns></returns>
        public static string GetLimitString(int start, int end)
        {
            string strsql = "";
            if (start <= end && start > 0)
            {
                if (start > 0)
                {
                    end = end - start + 1;
                }
                start = start - 1;
                strsql += " limit " + start.ToString() + "," + end.ToString();
            }
            else
                strsql += " limit 30";
            return strsql;

        }
        /// <summary>
        /// 根据当前页形成SQL语句:limit start,end
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pagesize">每页记录数</param>
        /// <returns></returns>
        public static string GetLimit(int page, int pagesize)
        {
            int start = 0;
            int end = 0;
            if (page <= 1)
            {
                start = 0;
            }
            else
            {
                start = (page - 1) * pagesize;

            }
            int s = start - 1;
            string strsql = " limit " + start.ToString() + "," + pagesize;
            return strsql;

        }
        /// <summary>
        /// 获取导出的限制集合
        /// </summary>
        /// <param name="totalcount">总页数</param>
        /// <param name="pagesize">每页记录数</param>
        /// <returns></returns>
        public static ArrayList GetExportLimit(int totalcount, int pagesize)
        {
            ArrayList arylst = new ArrayList();
            if (totalcount != 0 && pagesize != 0 && totalcount > pagesize)
            {
                int pagecount = 0;
                if (totalcount % pagesize == 0)
                {
                    pagecount = totalcount / pagesize;
                }
                else
                    pagecount = totalcount / pagesize + 1;
                string limit = "";
                for (int i = 0; i < pagecount; i++)
                {
                    if (i == 0)
                    {
                        limit = "limit 0," + pagesize;


                    }
                    else
                    {
                        limit = "limit " + (pagesize * i - 1).ToString() + "," + pagesize;


                    }
                    arylst.Add(limit);


                }



            }
            else if (totalcount != 0 && pagesize != 0 && totalcount <= pagesize)
            {
                arylst.Add(" limit 0," + pagesize.ToString());
            }
            return arylst;


        }

        /// <summary>
        /// 设置页面旧值
        /// </summary>
        /// <param name="e"></param>
        /// <param name="page"></param>
        /// <param name="dg"></param>
        public static void SetPageControlValueFromDataGrid(
            System.Web.UI.WebControls.DataGridCommandEventArgs e, System.Web.UI.Page page,
            System.Web.UI.WebControls.DataGrid dg

            )
        {
            #region
            for (int i = 0; i < dg.Columns.Count; i++)
            {

                System.Web.UI.WebControls.BoundColumn b = dg.Columns[i] as System.Web.UI.WebControls.BoundColumn;
                if (b != null)
                {
                    string ctrlid = "tb" + b.DataField;
                    //this.Master.FindControl("MainContent").FindControl("TextBox1")

                    System.Web.UI.Control ctrl = page.Master.FindControl("cphMain").FindControl(ctrlid);
                    if (ctrl is System.Web.UI.WebControls.TextBox)
                    {
                        (ctrl as System.Web.UI.WebControls.TextBox).Text = e.Item.Cells[i].Text.Replace("&nbsp;", "");
                        (ctrl as System.Web.UI.WebControls.TextBox).Focus();
                    }

                    if (ctrl is System.Web.UI.WebControls.DropDownList)
                    {
                        System.Web.UI.WebControls.DropDownList ddl = ctrl as System.Web.UI.WebControls.DropDownList;
                        string strvalue = e.Item.Cells[i].Text;
                        if (ddl.Items.FindByValue(strvalue) != null)
                        {
                            ddl.SelectedValue = strvalue;
                        }
                    }
                }

            }

            #endregion

        }
        /// <summary>
        /// 设置页面旧值
        /// </summary>
        /// <param name="e"></param>
        /// <param name="page"></param>
        /// <param name="dg"></param>
        /// <param name="subname"></param>
        public static void SetPageControlValueFromDataGrid(
                 System.Web.UI.WebControls.DataGridCommandEventArgs e, System.Web.UI.Page page,
                 System.Web.UI.WebControls.DataGrid dg,
                 string subname

    )
        {
            #region
            for (int i = 0; i < dg.Columns.Count; i++)
            {

                System.Web.UI.WebControls.BoundColumn b = dg.Columns[i] as System.Web.UI.WebControls.BoundColumn;
                if (b != null)
                {
                    string ctrlid = "tb" + subname + b.DataField;
                    //this.Master.FindControl("MainContent").FindControl("TextBox1")

                    System.Web.UI.Control ctrl = page.Master.FindControl("cphMain").FindControl(ctrlid);
                    if (ctrl is System.Web.UI.WebControls.TextBox)
                    {
                        (ctrl as System.Web.UI.WebControls.TextBox).Text = e.Item.Cells[i].Text.Replace("&nbsp;", "");
                        (ctrl as System.Web.UI.WebControls.TextBox).Focus();
                    }

                    if (ctrl is System.Web.UI.WebControls.DropDownList)
                    {
                        System.Web.UI.WebControls.DropDownList ddl = ctrl as System.Web.UI.WebControls.DropDownList;
                        string strvalue = e.Item.Cells[i].Text;
                        if (ddl.Items.FindByValue(strvalue) != null)
                        {
                            ddl.SelectedValue = strvalue;
                        }
                    }
                }

            }

            #endregion

        }



        /// <summary>
        /// 根据DataTable 设定页面的值哦！
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dt"></param>
        public static void SetPageControlValueFromDataTable(System.Web.UI.Page page, DataTable dt)
        {

            #region

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string fieldname = dt.Columns[i].ColumnName;
                    string fieldvalue = dt.Rows[0][fieldname].ToString();
                    string ctrlid = "tb" + fieldname;
                    System.Web.UI.Control ctrl = page.Master.FindControl("cphMain").FindControl(ctrlid);
                    if (ctrl is System.Web.UI.WebControls.TextBox)
                    {
                        (ctrl as System.Web.UI.WebControls.TextBox).Text = fieldvalue.Replace("&nbsp;", "");
                        //(ctrl as System.Web.UI.WebControls.TextBox).Focus();
                    }

                    if (ctrl is System.Web.UI.WebControls.DropDownList)
                    {
                        System.Web.UI.WebControls.DropDownList ddl = ctrl as System.Web.UI.WebControls.DropDownList;
                        string strvalue = fieldvalue;
                        if (ddl.Items.FindByValue(strvalue) != null)
                        {
                            ddl.SelectedValue = strvalue;
                        }
                    }



                }


            }





            #endregion

        }
        /// <summary>
        /// 获取Where 参数
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fieldvalue"></param>
        /// <returns></returns>
        public static string GetWhere(string fieldname, string fieldvalue)
        {
            fieldvalue = Common.GetRightSQLParam(fieldvalue);
            if (fieldname != "" && fieldvalue != "")
            {
                return " where " + fieldname + " like '%" + fieldvalue + "%'";

            }
            else
                return "";


        }
        /// <summary>
        /// 获取Where 参数
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fieldvalue"></param>
        /// <returns></returns>
        public static string GetEqualWhere(string fieldname, string fieldvalue)
        {
            fieldvalue = Common.GetRightSQLParam(fieldvalue);
            if (fieldname != "" && fieldvalue != "")
            {
                return " where " + fieldname + " ='" + fieldvalue + "'";

            }
            else
                return "";


        }

        /// <summary>
        /// 16进制转为颜色
        /// </summary>
        /// <param name="strHxColor">#fff类似</param>
        /// <returns></returns>
        public static System.Drawing.Color colorHx16toRGB(string strHxColor)
        {
            try
            {
                if (strHxColor.Length == 0)
                {//如果为空
                    return System.Drawing.Color.FromArgb(0, 0, 0);//设为黑色
                }
                else
                {//转换颜色
                    return System.Drawing.Color.FromArgb(System.Int32.Parse(strHxColor.Substring(1, 2),
                        System.Globalization.NumberStyles.AllowHexSpecifier),
                        System.Int32.Parse(strHxColor.Substring(3, 2),
                        System.Globalization.NumberStyles.AllowHexSpecifier),
                        System.Int32.Parse(strHxColor.Substring(5, 2),
                        System.Globalization.NumberStyles.AllowHexSpecifier));
                }
            }
            catch
            {//设为黑色
                return System.Drawing.Color.FromArgb(0, 0, 0);
            }
        }
        /// <summary>
        /// 设置datagrid选中行的背景色
        /// </summary>
        /// <param name="e"></param>
        /// <param name="dg"></param>
        /// <param name="fieldslist"></param>
        public static void SetDataGridLineAndCellBackColor(System.Web.UI.WebControls.DataGridCommandEventArgs e,
            System.Web.UI.WebControls.DataGrid dg, string fieldslist)
        {
            int index = e.Item.ItemIndex;

            //if (e.Item.DataItem!=null)
            //HttpContext.Current.Response.Write(e.Item.ItemIndex.ToString());


            for (int i = 0; i < dg.Items.Count; i++)
            {
                //if (i % 2 == 0)
                //    dg.Items[i].BackColor = colorHx16toRGB("#FFFBD6");
                //else
                dg.Items[i].BackColor = colorHx16toRGB("#FFFFFF");
                dg.Items[i].CssClass = "trB";
                for (int n = 0; n < dg.Items[i].Cells.Count; n++)
                {
                    dg.Items[i].Cells[n].CssClass = "tdB";
                }

            }
            e.Item.BackColor = colorHx16toRGB("#FEFCE3");
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                string field = dg.Columns[i].HeaderText;
                if (e.CommandName == field)
                {
                    e.Item.Cells[i].CssClass = "tdA";
                }



            }
            //e.Item.Cells[]



        }
        /// <summary>
        /// 获取用户语言
        /// </summary>
        /// <returns></returns>
        public static string getUserLanguage()
        {
            return "cn";

        }
        /// <summary>
        /// 获取用户站点
        /// </summary>
        /// <returns></returns>
        public static string getUserWebSite()
        {
            return "localhost";

        }
        /// <summary>
        /// 用户通用上传地址
        /// </summary>
        /// <returns></returns>
        public static string getPicPath()
        {
            string path = Web.GetConfigString("PicPath");
            return path;


        }
        /// <summary>
        /// 最大尺寸
        /// </summary>
        /// <returns></returns>
        public static int getMaxPicSize()
        {
            int size = Web.getInt(Web.GetConfigString("MaxPicSize"));
            if (size == 0)
                return 500;
            else
                return size;


        }
        /// <summary>
        /// 根据第一列命名为表名
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataSet RenameDataSet(DataSet ds)
        {
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    ds.Tables[i].TableName = ds.Tables[i].Columns[0].ColumnName;

                }

                return ds;
            }
            else
                return null;


        }


        /// <summary>
        /// 获取配置文件绝对路径
        /// </summary>
        /// <param name="filename">配置文件名称</param>
        /// <returns></returns>
        public static string getConfigFile(string filename)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(
                Web.GetConfigString("Config")
                );
            filename = path + filename + ".config";
            if (System.IO.File.Exists(filename))
            {
                return filename;
            }
            else
                return "";

        }
        /// <summary>
        /// 播放地址
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string getPlayUrl(string guid)
        {
            string url = System.Web.HttpContext.Current.Request.Url.Host
                + System.Web.HttpContext.Current.Request.ApplicationPath
                + "/Play.aspx?guid=" + guid;
            return url;

        }
        /// <summary>
        /// 清除用户输入的值
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dg"></param>
        public static void ClearPageControlValueFromDataGrid(
            System.Web.UI.Page page,
            System.Web.UI.WebControls.DataGrid dg

            )
        {
            #region
            for (int i = 0; i < dg.Columns.Count; i++)
            {

                System.Web.UI.WebControls.BoundColumn b = dg.Columns[i] as System.Web.UI.WebControls.BoundColumn;
                if (b != null)
                {
                    string ctrlid = "tb" + b.DataField;
                    System.Web.UI.Control ctrl = page.Master.FindControl("cphMain").FindControl(ctrlid);
                    if (ctrl is System.Web.UI.WebControls.TextBox)
                    {
                        (ctrl as System.Web.UI.WebControls.TextBox).Text = "";
                    }
                    ctrlid = "ddl" + b.DataField;
                    if (ctrl is System.Web.UI.WebControls.DropDownList)
                    {
                        System.Web.UI.WebControls.DropDownList ddl = ctrl as System.Web.UI.WebControls.DropDownList;
                        ddl.SelectedIndex = 0;
                    }
                }

            }
            #endregion
        }
        /// <summary>
        ///  从xml绑定数据到datalist
        /// </summary>
        /// <param name="dl"></param>
        /// <param name="filename"></param>
        public void BindDataListFromXml(System.Web.UI.WebControls.DataList dl,
            string filename)
        {
            string f = getConfigFile(filename);
            DataSet ds = new DataSet();
            if (f != "")
            {
                ds.ReadXml(f);
                dl.DataSource = ds.Tables[0];
                dl.DataBind();
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static int asc(char mm)
        {
            byte[] aa = System.Text.Encoding.Default.GetBytes(mm.ToString());
            int pp = 0;
            if (aa.Length == 1)
            {
                pp = (int)aa[0];
            }
            else
            {
                for (int i = 0; i < aa.Length; i += 2)
                {
                    pp = (int)aa[i];
                    pp = pp * 256 + aa[i + 1] - 65536;
                }
            }
            return pp;
        }
        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns></returns>
        public static string getPath()
        {
            //System.Web.HttpContext.Current.Response.Write(System.Web.HttpContext.Current.Request.ApplicationPath);
            string result = "";
            if (System.Web.HttpContext.Current.Request.ApplicationPath == "" ||
                System.Web.HttpContext.Current.Request.ApplicationPath == "/" ||
                System.Web.HttpContext.Current.Request.ApplicationPath == "\\"
                )
                result = "/";
            else
                result = System.Web.HttpContext.Current.Request.ApplicationPath + "/";
            result = result.Replace("//", "/");
            return result;


        }
        /// <summary>
        /// 获取汉字或英文的首字母
        /// </summary>
        /// <param name="InputChar"></param>
        /// <returns></returns>
        public static char FirstChar(char InputChar)
        {
            int tmp = 65536 + asc(InputChar);
            string getpychar = "";
            if (tmp >= 45217 && tmp <= 45252)
            {
                getpychar = "A";
            }
            else if (tmp >= 45253 && tmp <= 45760)
            {
                getpychar = "B";
            }
            else if (tmp >= 45761 && tmp <= 46317)
            {
                getpychar = "C";
            }
            else if (tmp >= 46318 && tmp <= 46825)
            {
                getpychar = "D";
            }
            else if (tmp >= 46826 && tmp <= 47009)
            {
                getpychar = "E";
            }
            else if (tmp >= 47010 && tmp <= 47296)
            {
                getpychar = "F";
            }
            else if (tmp >= 47297 && tmp <= 47613)
            {
                getpychar = "G";
            }
            else if (tmp >= 47614 && tmp <= 48118)
            {
                getpychar = "H";
            }
            else if (tmp >= 48119 && tmp <= 49061)
            {
                getpychar = "J";
            }
            else if (tmp >= 49062 && tmp <= 49323)
            {
                getpychar = "K";
            }
            else if (tmp >= 49324 && tmp <= 49895)
            {
                getpychar = "L";
            }
            else if (tmp >= 49896 && tmp <= 50370)
            {
                getpychar = "M";
            }
            else if (tmp >= 50371 && tmp <= 50613)
            {
                getpychar = "N";
            }
            else if (tmp >= 50614 && tmp <= 50621)
            {
                getpychar = "O";
            }
            else if (tmp >= 50622 && tmp <= 50905)
            {
                getpychar = "P";
            }
            else if (tmp >= 50906 && tmp <= 51386)
            {
                getpychar = "Q";
            }
            else if (tmp >= 51387 && tmp <= 51445)
            {
                getpychar = "R";
            }
            else if (tmp >= 51446 && tmp <= 52217)
            {
                getpychar = "S";
            }
            else if (tmp >= 52218 && tmp <= 52697)
            {
                getpychar = "T";
            }
            else if (tmp >= 52698 && tmp <= 52979)
            {
                getpychar = "W";
            }
            else if (tmp >= 52980 && tmp <= 53640)
            {
                getpychar = "X";
            }
            else if (tmp >= 53689 && tmp <= 54480)
            {
                getpychar = "Y";
            }
            else if (tmp >= 54481 && tmp <= 62289)
            {

                getpychar = "Z";
            }
            else
            //如果不是中文，则不处理 
            {
                getpychar = InputChar.ToString();
            }
            return Convert.ToChar(getpychar);
        }
        /// <summary>
        /// 根据绝对路径返回文件名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFileName(string url)
        {
            if (url == "")
                return "";
            else
            {
                int pos = url.LastIndexOf("/");
                if (pos < 0)
                {
                    pos = url.LastIndexOf("\\");
                }
                if (pos > 0)
                {
                    return url.Substring(pos + 1);

                }
                else
                    return "";


            }



        }


        /// <summary>
        /// 根据集合创建一个数据表
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(NameValueCollection nvc)
        {
            if (nvc != null && nvc.Count > 0)
            {

                DataTable dt = new DataTable();

                for (int i = 0; i < nvc.Count; i++)
                {
                    string colname = nvc.GetKey(i);
                    if (dt.Columns.Contains(colname) == false)
                        dt.Columns.Add(colname);
                }

                DataRow dr = dt.NewRow();
                for (int i = 0; i < nvc.Count; i++)
                {
                    string colname = nvc.GetKey(i);
                    string fieldvalue = nvc[i];
                    dr[colname] = fieldvalue;
                }
                dt.Rows.Add(dr);

                return dt;

            }
            else
                return null;

        }


 
    }
}
