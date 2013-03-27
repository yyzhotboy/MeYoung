<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Allowance.aspx.cs" Inherits="MeYoung.Type.Allowance" %>

<%@ Register Src="~/UserControl/Header.ascx" TagName="Head" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Footer.ascx" TagName="Foot" TagPrefix="uc3" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>米痒优惠卡</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jquery1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Pager a").addClass('page_num');
            $("#Pager >a:first").removeClass('page_num');
            $("#Pager >a:last").removeClass('page_num');
            $("#Pager >a:first").addClass('previous_page');
            $("#Pager >a:last").addClass('next_page');
            $("#Pager >a:first").css("padding", "0px");
            $("#Pager >a:last").css("padding", "0px");

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc2:Head ID="head" runat="server" />
    <div class="allowancemain">
        <asp:Repeater runat="server" ID="R_Type">
            <ItemTemplate>
                <div class="typecontent">
                    <div class="allowanceimg">
                        <img src="" width="265px" height="160px"></img></div>
                    <div class="imgtitle">
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <cc1:AspNetPager runat="server" CssClass="pages" CurrentPageButtonClass="cpb" ID="Pager"
            FirstPageText="首页" LastPageText="尾页" NextPageText="" ShowFirstLast="false" PrevPageText=""
            OnPageChanged="Pager_PageChanged" ShowInputBox="Never" ShowPageIndexBox="Never"
            CurrentPageButtonPosition="Center" NumericButtonCount="4" PageSize="10">
        </cc1:AspNetPager>
    </div>
    <uc3:Foot ID="foot" runat="server" />
    </form>
</body>
</html>
