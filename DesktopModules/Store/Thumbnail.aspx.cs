/*
'  DotNetNuke -  http://www.dotnetnuke.com
'  Copyright (c) 2002-2007
'  by Shaun Walker ( sales@perpetualmotion.ca ) of Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
' 
'  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
'  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
'  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
'  to permit persons to whom the Software is furnished to do so, subject to the following conditions:
' 
'  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
'  of the Software.
' 
'  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
'  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
'  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
'  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
'  DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Security;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Modules.Store.Catalog;

namespace DotNetNuke.Modules.Store.WebControls
{
	/// <summary>
	/// Summary description for Thumbnail.
	/// </summary>
	public partial class Thumbnail : System.Web.UI.Page
	{
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

		#region Events
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Drawing.Image image;
			System.Drawing.Bitmap thumb;
			System.Drawing.Imaging.ImageFormat imageFormat;
			System.Drawing.Size thumbSize;

			string imagePath = Request.QueryString["IP"];
			string thumbWidth = Request.QueryString["IW"];

			try
			{
				if (imagePath != null)
				{
					if (thumbWidth == null)
					{
						thumbWidth = "175";
					}

					image = GetImage(imagePath);

					imageFormat = image.RawFormat;
					thumbSize = ThumbSize(image.Width, image.Height, Convert.ToInt16(thumbWidth));

                    thumb = new Bitmap(thumbSize.Width, thumbSize.Height);
					Graphics g = Graphics.FromImage(thumb);
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					g.DrawImage(image, 0, 0, thumbSize.Width, thumbSize.Height);

                    Response.ContentType = GetContentType(imageFormat);

                    MemoryStream memStream = new MemoryStream();
                    thumb.Save(memStream, imageFormat);
                    memStream.WriteTo(Response.OutputStream);

					image.Dispose();
					thumb.Dispose();
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
			}
		}
		#endregion

		#region Private Functions
		/// <summary>
		/// Retrieves and Image from a URI.  URI's external to the project
		/// are restricted because DNN's default trust level is Medium.  In 
		/// this case the thumnail.jpg shipped with DNN is displayed.
		/// 
		/// NOTE: 
		/// Changing the trust level to Full will allow external web requests.
		/// Ex. &lt;trust level="Full" originUrl=""&gt;
		/// 
		/// see http://msdn2.microsoft.com/en-US/library/tkscy493(VS.80).aspx
		/// </summary>
		/// <param name="sURL">URI of the image to be loaded</param>
		/// <returns>Image reference to the loaded image</returns>
		private System.Drawing.Image GetImage(string sURL)
		{
			if (sURL.IndexOf("http://") > -1)
			{
				try 
				{
					Stream str = null;

					HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(sURL);
				
					wReq.Credentials = CredentialCache.DefaultCredentials;

					HttpWebResponse wRes = (HttpWebResponse)wReq.GetResponse();
					str = wRes.GetResponseStream();

					return System.Drawing.Image.FromStream(str);
				} 
				catch
				{
					//TODO: Get n/a image for normal users or 
					//      an security exception image for administrators
					return System.Drawing.Image.FromFile( 
						Server.MapPath( "~/images/thumbnail.jpg" ) );
				}
			}
			else
			{
				return System.Drawing.Image.FromFile(Server.MapPath(sURL));
			}
		}
		
		private Size ThumbSize(int currentWidth, int currentHeight, int newWidth)
		{
			double iMultiplier;

			if (currentWidth > newWidth)
			{
				iMultiplier = Convert.ToDouble(newWidth) / currentWidth;
			}
			else
			{
				iMultiplier = 1;
			}

			return new Size(Convert.ToInt16(currentWidth * iMultiplier), Convert.ToInt16(currentHeight * iMultiplier));
		}

        private string GetContentType(ImageFormat imageFormat)
        {
            if (imageFormat.Equals(ImageFormat.Bmp))
            {
                return "image/bmp";
            }
            if (imageFormat.Equals(ImageFormat.Gif))
            {
                return "image/gif";
            }
            if (imageFormat.Equals(ImageFormat.Jpeg))
            {
                return "image/jpeg";
            }
            if (imageFormat.Equals(ImageFormat.Png))
            {
                return "image/png";
            }
            else
            {
                return "";
            }
        }
		#endregion
	}
}
