<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MeYoung.User.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户登录</title>
    <link href="../css/login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function resetcode() {
            document.getElementById("imagescode").src = "../imagesRead/" + Math.random();
        }

        function CheckNull() {
            var name = document.getElementById('tb_name').value;
            var pwd = document.getElementById('tb_pwd').value;
            var check = document.getElementById('tb_check').value;
            if (name == "") {
                alert("用户名不能为空!");
                return false;
            }
            if (pwd == "") {
                alert("密码不能为空!");
                return false;
            }
            if (check == "") {
                alert("验证码不能为空!");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="register">
  <h1>用户登录</h1>
  <!--登录-->
  <div class="register_left">
    <ul>
      <li><span class="register_left_text">用户名：</span>       
        <asp:TextBox ID="tb_name" runat="server" CssClass="textfield3"></asp:TextBox>
      </li>
      <li><span class="register_left_text">密码：</span>
         <asp:TextBox ID="tb_pwd" runat="server" CssClass="textfield3" 
              TextMode="Password"></asp:TextBox>
      </li>
      <li><span class="register_left_text">验证码：</span>
       
        <asp:TextBox ID="tb_check" runat="server" CssClass="textfield_checking" 
             ></asp:TextBox>
              <img id="imagescode" onclick="javascript:resetcode()" src="../imagesRead/'+Math.random()" />
              
      </li>
   
    </ul>
    <asp:Button  ID="button2" CssClass="register_btn" runat="server" Text="登录"  OnClientClick="CheckNull();"
          onclick="button2_Click" /> <span class=" register_btn_text">忘记密码？<a href="#">点击这里找回密码</a></span>
  </div>
  <div class="register_line"></div>
  <!--注册-->
  <div class="register_right"><h2>使用合作网站账号登录：</h2>
  <div class="register_right_btn"><asp:Button ID="btn_sina" CssClass="mgs_sina" runat="server" Text="" 
          UseSubmitBehavior="False"/>
          <asp:Button ID="btn_qq" CssClass="mgs_qq" 
          runat="server" Text=""   UseSubmitBehavior="False"/></div>
  <h3>还不是堵车不寂寞用户？</h3>
  <h4>一分钟轻松注册！ <a href="../Regedit/">免费注册</a></h4></div>
  <div class=" clear"></div>
        
</div>
    </form>
</body>
</html>
