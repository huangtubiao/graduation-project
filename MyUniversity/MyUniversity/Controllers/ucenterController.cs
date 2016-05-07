using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyUniversity.Controllers
{
    public class ucenterController : Controller
    {
        private loginUser loginUser { get; set; }
        private IUserService _userService { get; set; }

        public ucenterController(IUserService userService)
        {
            loginUser = Models.Help.loginUser.getLoginUser();
            _userService = userService;
        }

        [HttpPost]
        public ActionResult uploadHead(HttpPostedFileBase head)//命名和上传控件name 一样
        {
            try
            {
                if ((head == null))
                {
                    return Json(new { msg = 0 });
                }
                else
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "bmp" };
                    var fileExt = System.IO.Path.GetExtension(head.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        return Json(new { msg = -1 });
                    }

                    if (head.ContentLength > 1024 * 1000 * 10)
                    {
                        return Json(new { msg = -2 });
                    }

                    Random r = new Random();
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(10000) + "." + fileExt;
                    var filepath = Path.Combine(Server.MapPath("~/avatar/temp"), filename);
                    head.SaveAs(filepath);
                    return Json(new { msg = filename });
                }
            }
            catch (Exception)
            {
                return Json(new { msg = -3 });
            }
        }





        [HttpPost]
        [ValidateInput(false)]
        public ActionResult saveHead()
        {

            UploadImageModel model = new UploadImageModel();
            model.headFileName = Request.Form["headFileName"].ToString();
            model.x = Convert.ToInt32(Request.Form["x"]);
            model.y = Convert.ToInt32(Request.Form["y"]);
            model.width = Convert.ToInt32(Request.Form["width"]);
            model.height = Convert.ToInt32(Request.Form["height"]);

            if ((model == null))
            {
                return Json(new { msg = 0 });
            }
            else
            {

                var filepath = Path.Combine(Server.MapPath("~/avatar/temp"), model.headFileName);
                string fileExt = Path.GetExtension(filepath);
                Random r = new Random();
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(10000) + fileExt;
                var path180 = Path.Combine(Server.MapPath("~/avatar/180"), filename);
                cutAvatar(filepath, model.x, model.y, model.width, model.height, 75L, path180, 180);

                //同时更新数据库》个人头像
                string imgurl = "/avatar/180/" + filename;
                User user = _userService.getUserById(loginUser.userId);
                user.userImg = imgurl;
                _userService.updateUser(user);
                //更改用户登录的头像（cookies值）
                FormsAuthentication.SetAuthCookie(user.userImg, true);

                return Json(new { msg = 1, newImg = imgurl });
            }
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        public void cutAvatar(string imgSrc, int x, int y, int width, int height, long Quality, string SavePath, int t)
        {


            Image original = Image.FromFile(imgSrc);

            Bitmap img = new Bitmap(t, t, PixelFormat.Format24bppRgb);

            img.MakeTransparent(img.GetPixel(0, 0));
            img.SetResolution(72, 72);
            using (Graphics gr = Graphics.FromImage(img))
            {
                if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) || original.RawFormat.Equals(ImageFormat.Bmp))
                {
                    gr.Clear(Color.Transparent);
                }
                if (original.RawFormat.Equals(ImageFormat.Gif))
                {
                    gr.Clear(Color.White);
                }


                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                using (var attribute = new System.Drawing.Imaging.ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                }
            }
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
            if (original.RawFormat.Equals(ImageFormat.Jpeg))
            {
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
            }
            else
                if (original.RawFormat.Equals(ImageFormat.Png))
                {
                    myImageCodecInfo = GetEncoderInfo("image/png");
                }
                else
                    if (original.RawFormat.Equals(ImageFormat.Gif))
                    {
                        myImageCodecInfo = GetEncoderInfo("image/gif");
                    }
                    else
                        if (original.RawFormat.Equals(ImageFormat.Bmp))
                        {
                            myImageCodecInfo = GetEncoderInfo("image/bmp");
                        }

            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, Quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            img.Save(SavePath, myImageCodecInfo, myEncoderParameters);
        }

        //根据长宽自适应 按原图比例缩放 
        private static Size GetThumbnailSize(System.Drawing.Image original, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double)desiredWidth / original.Width;
            var heightScale = (double)desiredHeight / original.Height;
            var scale = widthScale < heightScale ? widthScale : heightScale;
            return new Size
            {
                Width = (int)(scale * original.Width),
                Height = (int)(scale * original.Height)
            };
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}
