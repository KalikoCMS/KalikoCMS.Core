<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KalikoCMS.Admin.Login" %>
<!doctype html>
<html>
<head>
  <title>Login</title>
  <meta name="robots" content="noindex, nofollow" />
  <link href='http://fonts.googleapis.com/css?family=Open+Sans:300' rel='stylesheet' type='text/css' />
  <style type="text/css">
    body { background: #222222; color: #ffffff; }
    body, input { font-family: 'Open Sans' , sans-serif; }
    .login { background: #10537B; width: 400px; margin: 100px auto 0; box-shadow: 0 0px 10px rgba(0,0,0,0.18) }
    ul { padding: 0; margin: 0; }
    li { display: inline-block; list-style: none outside none; }
    li a { color: #ffffff; padding: 8px 24px; display: inline-block; text-decoration: none; }
    li.active { background: #1570A6; }
    .login-form { background: #1570A6; padding: 10px 20px; }
    .login-form h1 { margin:0;font-size:40px;font-weight:100;}
    .login-form p { margin-top:0; }
    .control-group { width: 100%; }
    .control-group input[type=text], .control-group input[type=password] { width: 340px; padding: 10px; z-index: 9; position: relative; font-size: 18px; margin-bottom:10px; }
    .control-group .controls { display: inline-block; }
    .btn { background: #10537B;  color:#ffffff; border: medium none; padding: 8px 20px; font-size:16px;font-weight:100; cursor:pointer; margin:0 auto;width:100px; }
    .checkbox { float:right; }
  </style>
</head>
<body>
  <form id="Form1" method="post" runat="server">
  <div class="login">
    <ul>
      <li class="active"><a href="#">Login</a></li>
      <!-- TODO: Recover password -->
    </ul>
    <% ((TextBox)LoginForm.FindControl("UserName")).Attributes.Add("placeholder", "Username"); %>
    <% ((TextBox)LoginForm.FindControl("Password")).Attributes.Add("placeholder", "Password"); %>
    <div class="login-form">
      <asp:Login ID="LoginForm" runat="server" FailureText="Wrong username or password" RenderOuterTable="false">
        <LayoutTemplate>
          <h1>
            Login to website</h1>
          <p>
            Enter your username and password.</p>
          <div class="control-group">
            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
          </div>
          <div class="control-group">
            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
          </div>
          <div class="control-group">
            <asp:CheckBox ID="RememberMe" runat="server" CssClass="checkbox" Text=" Remember me next time." />
          </div>
          <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" CssClass="btn" />
        </LayoutTemplate>
      </asp:Login>
    </div>
  </div>
  <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
  <script type="text/javascript">
    window.onload = function () { document.getElementById('Login1_UserName').focus(); }
  </script>
  </form>
</body>
</html>
