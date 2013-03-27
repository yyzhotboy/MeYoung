<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCoupon.aspx.cs" Inherits="MeYoung.UserCenter.AddCoupon" %>
<%@ Register Src="~/UserControl/Header.ascx" TagName="Head" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagName="Foot" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加打折信息</title>
      <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/usercenter.css" rel="stylesheet" type="text/css" />
    <script src="../../js/My97/WdatePicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <uc2:Head ID="head" runat="server" />
    <div id="main">
    <div class="umain">

    <div class="user_right_div">&nbsp;&nbsp;&nbsp;商&nbsp;铺&nbsp;名&nbsp;称：<asp:DropDownList ID="ddl_Shop" runat="server" CssClass="user_right_from"></asp:DropDownList></div>
     <div class="user_right_div">&nbsp;&nbsp;&nbsp;打&nbsp;折&nbsp;标&nbsp;题：<asp:TextBox ID="tb_Titile" runat="server" CssClass="user_right_from"></asp:TextBox></div>
     <div class="user_right_div">活动开始时间：<asp:TextBox ID="tb_Start" runat="server" onclick="WdatePicker();" CssClass="user_right_from"></asp:TextBox></div>
     <div class="user_right_div">活动结束时间：<asp:TextBox ID="tb_End" runat="server" onclick="WdatePicker();" CssClass="user_right_from"></asp:TextBox></div>
     <div class="user_right_detail">打折详细信息：<asp:TextBox ID="tb_Detail" runat="server" 
             CssClass="user_detail" TextMode="MultiLine" Height="76px" 
             Width="317px"></asp:TextBox></div>
    </div>
    <div class="clear"></div>
     	<div><asp:Button ID="Button1" runat="server" Text="发布活动信息" ></asp:Button>
    <asp:Button ID="Button2" runat="server" Text="在浏览器中查看" onclick="Button2_Click" ></asp:Button></div>
        </div>
     <uc3:Foot ID="foot" runat="server" />
    </form>
</body>
</html>
