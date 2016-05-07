using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyUniversity
{
    /// <summary>
    /// data 的摘要说明
    /// </summary>
    public class data : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //设置文件类型和编码类型
            context.Response.ContentType = "text/html";
            context.Response.Charset = "utf-8";

            string assitUrl = "http://localhost:4987/wangEditor/wangEditor_uploadImg_assist.html";

            //取得文件对象
            HttpPostedFile file = context.Request.Files["wangEditor_uploadImg"];

            if (file == null)
            {
                string iframeSrc = assitUrl + "#" + "未成功获取文件，上传失败";
                string result = "<iframe src=\"" + iframeSrc + "\"></iframe>";

                context.Response.Write(result);
                context.Response.End();
                return;
            }
            else
            {
                //验证通过了，最后保存文件
                string path = context.Server.MapPath("~/articleFiles/");
                string originalFileName = file.FileName;
                string fileExtension = originalFileName.Substring(originalFileName.LastIndexOf('.'));
                string currentFileName = (new Random()).Next() + fileExtension; //文件名中不要带中文，否则出错
                //生成文件路径
                string imagePath = path + currentFileName;
                //保存文件
                file.SaveAs(imagePath);

                //保存文件之后，要告诉web前端上传已经成功
                //获取图片的url
                string imgUrl = context.Request.Url.GetLeftPart(UriPartial.Authority) + "/articleFiles/" + currentFileName;
                string iframeSrc = assitUrl + "#" + "ok|" + imgUrl;
                string result = "<iframe src=\"" + iframeSrc + "\"></iframe>";

                context.Response.Write(result);
                context.Response.End();
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}