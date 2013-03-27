using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
namespace MeYoung.User
{
    public partial class imagesRead : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string checkCode = Common.CodeImage.GetCode(4);//生成随机数
            Session["CheckCode"] = checkCode;

            MemoryStream ms = Common.CodeImage.GetImage(checkCode);//创建图片
            Response.ClearContent();
            Response.ContentType = "image/Gif";
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}