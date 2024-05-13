using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace MyOrange
{
	
	/// <summary>
	/// 一个图片处理类
	/// 2024.05.12 测试这个类
	/// </summary>		
	public class ImageClass
	{
		
		
		
		public ImageClass()
		{
			 
		}
		/// <summary>
		/// 错误信息
		/// </summary>
		public static string ErrorInfo="";


        /// <summary>
		///  保存上传的图片到服务器
		/// </summary>
		public static string SavePostedFile(System.Web.HttpPostedFile f,string path,int picsize)
		{
			try
			{
				string name		=Web.getDateTimeFormatString();
				path            =getRightFolder(path);
				string extend   =Path.GetExtension(f.FileName);
				string filename =path+name+extend;
				int    size     =picsize;
				if (size<=0)
					size=500;
				if (f.ContentLength>0 && f.ContentLength<1024*size)
				{
					if (path.StartsWith("/"))
						f.SaveAs(HttpContext.Current.Server.MapPath(filename));
					else
						f.SaveAs(HttpContext.Current.Server.MapPath("~/"+filename))	;
					return filename;
				}
				else
					return "";
			}
			catch(Exception e)
			{
				ErrorInfo +=e.Message;
				return "";
			}
			
		}
		
		/// <summary>
		///  保存上传的图片到服务器
		/// </summary>
		public static string SavePostedFile(System.Web.HttpPostedFile f)
		{
			string path		=Web.GetConfigString("PicPath");
			int    size     =Web.getInt(Web.GetConfigString("PicSize"));
			return SavePostedFile(f,path,size);

		}

        /// <summary>
		///  保存上传的图片到服务器
		/// </summary>
		public static string SaveImage(HtmlInputFile f,string path,int picsize)
		{
			try
			{
				string name		=Web.getDateTimeFormatString();
				path            =getRightFolder(path);
				string extend   =Path.GetExtension(f.PostedFile.FileName);
				string filename =path+name+extend;
				int    size     =picsize;
				if (size<=0)
					size=500;
				if (f.PostedFile.ContentLength>0 && f.PostedFile.ContentLength<1024*size)
				{
					if (path.StartsWith("/"))
						f.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(filename));
					else
						f.PostedFile.SaveAs(HttpContext.Current.Server.MapPath("~/"+filename))	;
					return filename;
				}
				else
					return "";
			}
			catch(Exception e)
			{
				ErrorInfo +=e.Message;
				return "";
			}
			
		}
		
			
		
		/// <summary>
		///  保存上传的图片到服务器
		/// </summary>
		public static string SaveImage(HtmlInputFile f)
		{
			string path		=Web.GetConfigString("PicPath");
			int    size     =Web.getInt(Web.GetConfigString("PicSize"));
			return SaveImage(f,path,size);

		}

		private static string getRightFolder(string path)
		{
			string folder=DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString()+DateTime.Now.Day.ToString()+"/";
			path   =path+folder;
			string apath="";
			if (path.StartsWith("/"))
			{
				apath=HttpContext.Current.Server.MapPath(path);
			}
			else
				apath=HttpContext.Current.Server.MapPath("~/"+path);
			if (Directory.Exists(apath)==false)
			{
				Directory.CreateDirectory(apath);
			}
			return path;
		
		}
	
        /// <summary>
		///  在图片上打水印
		/// </summary>			
		public static void DrawWaterMark(string imageMarkFileName,string imageFileName,
			
			MarkedImagePosition m,int LucencyPercent,string newimageFileName)
		{
			
			if (File.Exists(imageFileName)==false)
				return;
			if (File.Exists(imageMarkFileName)==false)
				return;

			if (LucencyPercent<0 || LucencyPercent>100)
				LucencyPercent=50;

			#region start
			System.Drawing.Image img = System.Drawing.Image.FromFile(imageFileName);
			int height = img.Height;
			int width = img.Width;
			img.Dispose();

			System.Drawing.Image mimg = System.Drawing.Image.FromFile(imageMarkFileName);
			int mheight = mimg.Height;
			int mwidth = mimg.Width;
			mimg.Dispose();
			#endregion 

			
			if (height>mheight && width>mwidth)
			{
				#region 水印
				ImageModification wm= new ImageModification();
				wm.DrawedImagePath= imageMarkFileName;
				wm.ModifyImagePath=imageFileName; 
				wm.RightSpace=wm.getRightSpace(width,m,mwidth);
				wm.BottoamSpace=wm.getBottoamSpace(height,m,mheight);
				wm.LucencyPercent=LucencyPercent;
				wm.OutPath=newimageFileName;
				wm.DrawImage();
				#endregion 
			}
	
		
		}
		
			
		 /// <summary>
		///  在图片上打水印
		/// </summary>		
		public static void DrawWaterMark(string imageMarkFileName,string imageFileName,
			
			MarkedImagePosition m)
		{
			DrawWaterMark(imageMarkFileName,imageFileName,m,50,	imageFileName);
		}


		#region 缩略图
		
		/// <summary>
		///  生产缩略图
		/// </summary>		
		public void ShowThumbnail(string oldfile, string newfile, int h, int w)
		{
   
			System.Drawing.Image img = System.Drawing.Image.FromFile(oldfile);
			System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

			int oldh = img.Height;
			int oldw = img.Width;
			
			if (h<=0) {h=oldh;}
			if (w<=0) {w=oldw;}


			int newh,neww;

			double h1 = oldh*1.0/h;
			double w1 = oldw*1.0/w;

			double f = (h1>w1)? h1:w1;

			if(f < 1.0)
			{
				newh = oldh;
				neww = oldw;
			}
			else
			{
				newh = (int)(oldh/f);
				neww = (int)(oldw/f);
			}

			System.Drawing.Image myThumbnail = img.GetThumbnailImage(neww, newh, myCallback, IntPtr.Zero);

			myThumbnail.Save(newfile, System.Drawing.Imaging.ImageFormat.Jpeg);

			img.Dispose();
			myThumbnail.Dispose();
		}
		private bool ThumbnailCallback()
		{
			return false;
		}
		#endregion
	}
}
