<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Infocenter.aspx.cs" Inherits="MeYoung.UserCenter.Infocenter" %>
<%@ Register Src="~/UserControl/Header.ascx" TagName="Head" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagName="Foot" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>欢迎来到用户中心</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/usercenter.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <uc2:Head ID="head" runat="server" />
        <div id="main"> <a href="../AddShop/">添加商铺</a>
        <a href="../AddMail/">添加商城</a>
        <a href="../AddCoupon/">添加打折信息</a>
        </div>
        <div class="clear">
       
        </div>
         <uc3:Foot ID="foot" runat="server" />
    </form>
</body>
</html>
