<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMail.aspx.cs" Inherits="MeYoung.UserCenter.AddMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加商城</title>
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
    <div>
    <div class="umain">
    <div>商城名称：<asp:TextBox ID="tb_name" runat="server"></asp:TextBox></div>     
    <div>商铺地址：<asp:TextBox ID="tb_Address" runat="server"></asp:TextBox></div> 
    <div>上传图片：<asp:FileUpload ID="fu_Img" runat="server" /></div> 
    </div>
     <iframe src="../../Map/MapPoint/" height="500" width="500"></iframe> 
        <asp:Button ID="btn_Publish" runat="server" Text="添加" 
            onclick="btn_Publish_Click" />
    </div>
    </form>
</body>
</html>
