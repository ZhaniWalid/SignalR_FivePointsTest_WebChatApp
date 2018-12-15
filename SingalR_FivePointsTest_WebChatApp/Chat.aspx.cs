using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SD = System.Drawing;

namespace SingalR_FivePointsTest_WebChatApp
{
    public partial class Chat : System.Web.UI.Page
    {
        public string UserName = "admin";
        public string UserImage = "/images/Storage/userProfilePicture.png";
        ConnClass ConnC = new ConnClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                UserName = Session["UserName"].ToString();
                GetUserImage(UserName);
            }
            else
                Response.Redirect("Login.aspx");
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        public void GetUserImage(string Username)
        {
            if (Username != null)
            {
                string query = "select Photo from tbl_Users where UserName='" + Username + "'";

                string ImageName = ConnC.GetColumnVal(query, "Photo");
                if (ImageName != "")
                    UserImage = "images/Storage/" + ImageName;
            }


        }

        protected void btnChangePicModel_Click(object sender, EventArgs e)
        {

            string serverPath = HttpContext.Current.Server.MapPath("~/");
            //path = serverPath + path;
            if (FileUpload1.HasFile)
            {
                string FileWithPat = serverPath + @"images/Storage/" + UserName + FileUpload1.FileName;

                FileUpload1.SaveAs(FileWithPat);
                SD.Image img = SD.Image.FromFile(FileWithPat);
                SD.Image img1 = RezizeImage(img, 151, 150);
                img1.Save(FileWithPat);
                if (File.Exists(FileWithPat))
                {
                    FileInfo fi = new FileInfo(FileWithPat);
                    string ImageName = fi.Name;
                    string query = "update tbl_Users set Photo='" + ImageName + "' where UserName='" + UserName + "'";
                    if (ConnC.ExecuteQuery(query))
                        UserImage = "images/Storage/" + ImageName;
                }
            }
        }


        #region Resize Image With Best Qaulity

        private SD.Image RezizeImage(SD.Image img, int maxWidth, int maxHeight)
        {
            if (img.Height < maxHeight && img.Width < maxWidth) return img;
            using (img)
            {
                Double xRatio = (double)img.Width / maxWidth;
                Double yRatio = (double)img.Height / maxHeight;
                Double ratio = Math.Max(xRatio, yRatio);
                int nnx = (int)Math.Floor(img.Width / ratio);
                int nny = (int)Math.Floor(img.Height / ratio);
                SD.Bitmap cpy = new SD.Bitmap(nnx, nny, SD.Imaging.PixelFormat.Format32bppArgb);
                using (SD.Graphics gr = SD.Graphics.FromImage(cpy))
                {
                    gr.Clear(SD.Color.Transparent);

                    // This is said to give best quality when resizing images
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    gr.DrawImage(img,
                        new SD.Rectangle(0, 0, nnx, nny),
                        new SD.Rectangle(0, 0, img.Width, img.Height),
                        SD.GraphicsUnit.Pixel);
                }
                return cpy;
            }

        }

        private MemoryStream BytearrayToStream(byte[] arr)
        {
            return new MemoryStream(arr, 0, arr.Length);
        }

        #endregion

    }
}