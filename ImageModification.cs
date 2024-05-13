using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace MyOrange
{
	
	
	
	public enum  MarkedImagePosition {Middle,left,right,BottomLeft,BottomRight};

	/// <summary>
	/// 打水印，别人写的
	/// </summary>		
	public class ImageModification
	{
		#region "member fields"
		private string modifyImagePath=null;
		private string drawedImagePath=null;
		private int rightSpace;
		private int bottoamSpace;
		private int lucencyPercent=70;
		private string outPath=null;
		#endregion
		public ImageModification()
		{
		}
		#region "propertys"
		
		
		
		public string ModifyImagePath
		{
			get{return this.modifyImagePath;}
			set{this.modifyImagePath=value;}
		}
		
		
		
		public string DrawedImagePath
		{
			get{return this.drawedImagePath;}
			set{this.drawedImagePath=value;}
		}
		
		
		
		public int RightSpace
		{
			get{return this.rightSpace;}
			set{this.rightSpace=value;}
		}
		// 
		public int BottoamSpace
		{
			get{return this.bottoamSpace;}
			set{this.bottoamSpace=value;}
		}
		
		
		
		public int LucencyPercent
		{
			get{return this.lucencyPercent;}
			set
			{
				if(value>=0&&value<=100)
					this.lucencyPercent=value;
			}
		}
		
		
		
		public string OutPath
		{
			get{return this.outPath;}
			set{this.outPath=value;}
		}
		#endregion
		#region "methods"
		
		
		
		public void DrawImage()
		{
			System.Drawing.Image modifyImage=null;
			System.Drawing.Image drawedImage=null;
			Graphics g=null;
			try
			{ 
				// 
				modifyImage= System.Drawing.Image.FromFile(this.ModifyImagePath);
				drawedImage= System.Drawing.Image.FromFile(this.DrawedImagePath);
				g=Graphics.FromImage(modifyImage);
				//
				int x=modifyImage.Width-this.rightSpace;
				int y=modifyImage.Height-this.BottoamSpace;
				//
				float[][] matrixItems ={
										   new float[] {1, 0, 0, 0, 0},
										   new float[] {0, 1, 0, 0, 0},
										   new float[] {0, 0, 1, 0, 0},
										   new float[] {0, 0, 0, (float)this.LucencyPercent/100f, 0},
										   new float[] {0, 0, 0, 0, 1}}; 

				ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
				ImageAttributes imgAttr=new ImageAttributes();
				imgAttr.SetColorMatrix(colorMatrix,ColorMatrixFlag.Default,ColorAdjustType.Bitmap);
				//
				g.DrawImage(
					drawedImage,
					new Rectangle(x,y,drawedImage.Width,drawedImage.Height),
					0,0,drawedImage.Width,drawedImage.Height,
					GraphicsUnit.Pixel,imgAttr);
				//
				string[] allowImageType={".jpg",".gif",".png",".bmp",".tiff",".wmf",".ico"};
				FileInfo file=new FileInfo(this.ModifyImagePath);
				ImageFormat imageType=ImageFormat.Gif;
				switch(file.Extension.ToLower())
				{
					case ".jpg":
						imageType=ImageFormat.Jpeg;
						break;
					case ".gif":
						imageType=ImageFormat.Gif;
						break;
					case ".png":
						imageType=ImageFormat.Png;
						break;
					case ".bmp":
						imageType=ImageFormat.Bmp;
						break;
					case ".tif":
						imageType=ImageFormat.Tiff;
						break;
					case ".wmf":
						imageType=ImageFormat.Wmf;
						break;
					case ".ico":
						imageType=ImageFormat.Icon;
						break;
					default:
						break;
				}
				MemoryStream ms=new MemoryStream();
				modifyImage.Save(ms,imageType);
				byte[] imgData=ms.ToArray();
				modifyImage.Dispose();
				drawedImage.Dispose();
				g.Dispose();
				FileStream fs=null;
				if(this.OutPath==null || this.OutPath=="")
				{
					File.Delete(this.ModifyImagePath);
					fs=new FileStream(this.ModifyImagePath,FileMode.Create,FileAccess.Write);
				}
				else
				{
					fs=new FileStream(this.OutPath,FileMode.Create,FileAccess.Write);
				}
				if(fs!=null)
				{
					fs.Write(imgData,0,imgData.Length);
					fs.Close();
				}
			}
			finally
			{
				try
				{
					drawedImage.Dispose();
					modifyImage.Dispose();
					g.Dispose();
				}
				catch{;}
			}
		}
		
		
		
		
		
		
		
		public int getRightSpace(int imgwidth,MarkedImagePosition m,int mwidth)
		{
			switch(m)
			{
				case MarkedImagePosition.left:
					 return imgwidth;
				case MarkedImagePosition.right:;
					return mwidth;
				case MarkedImagePosition.BottomLeft:
					return mwidth;
				case MarkedImagePosition.BottomRight:
					return imgwidth;
				case MarkedImagePosition.Middle:
					return imgwidth-(imgwidth/2-(mwidth/2));
				default:
					return imgwidth;
			}
		}
		
		
		
		
		
		
		
		public int getBottoamSpace(int imgheight,MarkedImagePosition m,int mheight)
		{
			switch(m)
			{
				case MarkedImagePosition.left:
					return imgheight;
				case MarkedImagePosition.right:;
					return imgheight;
				case MarkedImagePosition.BottomLeft:
					return mheight;
				case MarkedImagePosition.BottomRight:
					return mheight;
				case MarkedImagePosition.Middle:
					return imgheight-(imgheight/2-(mheight/2));
				default:
					return imgheight;
			}
		}
		#endregion
	}
}