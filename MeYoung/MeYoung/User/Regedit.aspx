<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Regedit.aspx.cs" Inherits="MeYoung.User.Regedit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户注册</title>
    <link href="../css/login.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jquery1.7.1.min.js" type="text/javascript"></script>
     <script type="text/javascript">
         var ok8 = true;
         var ok1 = true;      
         var ok9 = true;
         var ok10 = true;
         function BtnCheck() {
             var ok3, ok4, ok5,  ok66;
             var name = document.getElementById('tb_name').value;           
             var pwd = document.getElementById('tb_pwd').value;
             var email = document.getElementById('tb_email').value;
             var phone = document.getElementById('tb_phone').value;
             if (name == null || name == '') {
                 ok3 = false;
                 $("#Em1").html("<span class='red'>该项不能为空！</span>");
             }
             else {
                 ok3 = true;
                 $("#Em1").html("");
             }
             if (pwd == null || pwd == '') {
                 ok4 = false;
                 $("#Em2").html("<span class='red'>该项不能为空！</span>");
             }
             else {
                 ok4 = true;
                 $("#Em2").html("");
             }
             if (email == null || email == '') {
                 ok5 = false;
                 $("#mail").html("<span class='red'>该项不能为空！</span>");
             }
             else {
                 ok5 = true;
                 $("#mail").html("");
             }
             if (phone == null || phone == '') {
                 ok6 = false;
                 $("#phone").html("<span class='red'>该项不能为空！</span>");
             }
             else {
                 ok6 = true;
                 $("#phone").html("");
             }        
             var ch66 = document.getElementById('checkbox');
             if (ch66.checked) {
                 ok66 = true;
             }
             else {
                 ok66 = false;
                 alert('请阅读相关协议！');
             }
             checkEmail();
             checkPhone();           
             return ok1 && ok3 && ok4 && ok5 && ok8 && ok9 && ok10 && ok66;


         }
         function checkpwd() {
             var pwd = document.getElementById('tb_pwd').value;
             var pwd1 = document.getElementById('tb_ok').value;
             if (pwd != pwd1) {
                 $("#ok").html("<span class='red'>密码不一致！</span>");
                 ok8 = false;
             }
             else {
                 ok8 = true;
                 $("#ok").html("");
             }
         }
         function checkcname() {
             var name = document.getElementById('tb_name').value;
             if (name == null || name == '') {
                 ok1 = false;
                 $("#Em1").html("<span class='red'>该项不能为空！</span>");

             }
             else {

                 if (name.length < 4 || name.length > 10) {
                     ok1 = false;
                     $("#Em1").html("<span class='red'>用户名长度请在6到10位之间！</span>");


                 }
                 else {
                     ok1 = true;
                     $("#Em1").html("");
                 }

             }
             if (ok1) {
                 var i =MeYoung.User.Regedit.ExitName(name).value;
                 if (i > 0) {
                     ok1 = false;
                     $("#Em1").html("<span class='red'>该用户已存在！</span>");
                     return;
                 }
                 else {
                     ok1 = true;
                     $("#Em1").html("");
                 }
             }
         }
     
         function checkEmail() {
             var email = document.getElementById('tb_email').value;
             var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
             if (reg.test(email)) {
                 var i = MeYoung.User.Regedit.ExitEmail(email).value;
                 if (i > 0) {
                     ok9 = false;
                     $("#mail").html("该邮箱已被占用！");
                 }
                 else {
                     ok9 = true;
                     $("#mail").html("");
                 }

             } else {
                 ok9 = false;
                 $("#mail").html("<span class='red'>格式不正确！</span>");
             }

         }
         function checkPhone() {
             var email = document.getElementById('tb_phone').value;
             var reg = /(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;
             if (reg.test(email)) {
                 ok10 = true;
                 $("#phone").html("");
             } else {
                 ok10 = false;
                 $("#phone").html("<span class='red'>格式不正确！</span>");
             }

         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="register">
        <h1>
            用户注册</h1>
        <!--登录-->
        <div class="register_left">
            <ul>
                <li><span class="register_left_text">用户名：</span>                   
                    <asp:TextBox ID="tb_name" runat="server" CssClass="textfield3" onblur=" checkcname()"></asp:TextBox>
                    <em id="Em1"></em> 
                </li>
              
                <li><span class="register_left_text">密码：</span>
                     <asp:TextBox ID="tb_pwd" runat="server" CssClass="textfield3" 
                        TextMode="Password"  ></asp:TextBox>
                        <em id="Em2"></em>
                </li>
                <li><span class="register_left_text">确认密码：</span>
                      <asp:TextBox ID="tb_ok" runat="server" CssClass="textfield3" 
                        TextMode="Password" onblur=" checkpwd()"></asp:TextBox>
                        <em id="ok"></em>
                </li>
                <li><span class="register_left_text">邮箱：</span>
                    <asp:TextBox ID="tb_email" runat="server" CssClass="textfield3" onblur=" checkEmail()"></asp:TextBox>
                    <em id="mail"></em>
                </li>
                <li><span class="register_left_text">手机号码：</span>
                   <asp:TextBox ID="tb_phone" runat="server" CssClass="textfield3" onblur=" checkPhone()"></asp:TextBox>
                   <em id="phone"></em>
                </li>
                <li class="register_text">               
                    <asp:CheckBox ID="checkbox" runat="server" Text="" /> 我已阅读并接受《<a href='Agreement.aspx'>米痒协议</a>》
                    </li>
            </ul>         
            <asp:Button ID="button2" runat="server" Text="注册"  
                OnClientClick="return BtnCheck();" CssClass="register_btn" 
                onclick="button2_Click"/>
        </div>
        <div class="register_line_login">
        </div>
        <!--注册-->
        <div class="register_right">
            <h3>
                已有堵车不寂寞账号？</h3>
            <h4>
                使用账号和密码 <a href="../Login/">立即登录</a></h4>
            <h2>
                使用合作网站账号登录：</h2>
            <div class="register_right_btn">
                <asp:Button ID="btn_sina" CssClass="mgs_sina" runat="server" Text="" 
           UseSubmitBehavior="False" ClientIDMode="Static"/>
          <asp:Button ID="btn_qq" CssClass="mgs_qq" 
          runat="server" Text=""   UseSubmitBehavior="False" ClientIDMode="Static"/></div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </form>
</body>
</html>
