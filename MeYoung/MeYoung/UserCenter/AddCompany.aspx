<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCompany.aspx.cs" Inherits="MeYoung.UserCenter.AddCompany" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加商城店铺</title>
</head>
<body>
    <form id="form1" runat="server">
       <asp:HiddenField ID="hd_id" runat="server" />
    <div>
    <div class="umain">
    <div>店铺名称：<asp:TextBox ID="tb_name" runat="server"></asp:TextBox></div> 
    <div>店铺类型：<asp:DropDownList ID="ddl_Type" runat="server"> </asp:DropDownList></div>  
    <div>店铺所属：<asp:DropDownList ID="ddl_Mail" runat="server"> </asp:DropDownList></div>  
    <div>店铺地址：<asp:TextBox ID="tb_Address" runat="server"></asp:TextBox></div> 
    <div>上传图片：<asp:FileUpload ID="fu_Img" runat="server" /></div> 
        
    </div>
    
        <asp:Button ID="btn_Publish" runat="server" Text="添加" 
            onclick="btn_Publish_Click" />
    </div>
    </form>
</body>
</html>
