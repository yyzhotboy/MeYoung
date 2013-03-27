<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddShop.aspx.cs" Inherits="MeYoung.UserCenter.AddShop" %>
<%@ Register Src="~/UserControl/Header.ascx" TagName="Head" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagName="Foot" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加沿街商铺</title>
        <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/usercenter.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/jquery1.7.1.min.js"></script>
    <script type="text/javascript" language="javascript">
        function LoadPoint(point) {
            document.getElementById('hd_id').value = point;
            var msg = document.getElementById('hd_id').value;
            alert(msg);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hd_id" runat="server" />
       <uc2:Head ID="head" runat="server" />
    <div id="main">
    <div class="umain">

    <div class="user_right_div">商铺名称：<asp:TextBox ID="tb_name" runat="server" CssClass="user_right_from"></asp:TextBox></div> 
    <div class="user_right_div">商铺类型：<asp:DropDownList ID="ddl_Type" runat="server" CssClass="user_right_from"> </asp:DropDownList></div>  
    <div class="user_right_div">商铺地址：<asp:TextBox ID="tb_Address" runat="server" CssClass="user_right_from"></asp:TextBox></div> 
    <div class="user_right_div">上传图片：<asp:FileUpload ID="fu_Img" runat="server" 
            CssClass="user_upfile"/></div> 
    <div class="user_right_div">店铺所属：<asp:DropDownList ID="ddl_Mail" runat="server" CssClass="user_right_from"> </asp:DropDownList></div>     
   
    <iframe src="../../Map/MapPoint/" frameborder=0 height="380" width="340"></iframe> 
     </div>
        <div><asp:Button ID="btn_Publish" runat="server" Text="添加" CssClass="btn2_4"
            onclick="btn_Publish_Click" /></div>
    </div>
     <uc3:Foot ID="foot" runat="server" />
    </form>
</body>
</html>
