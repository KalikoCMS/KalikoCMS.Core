<%@ Page Language="C#" AutoEventWireup="true" UnobtrusiveValidationMode="None" CodeBehind="Login.aspx.cs" Inherits="$rootnamespace$.Login" %>
<!doctype html>
<html>
<head>
  <title>Login</title>
  <meta name="robots" content="noindex, nofollow" />
  <style type="text/css">
    /*! normalize.css v3.0.2 | MIT License | git.io/normalize */html{font-family:sans-serif;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}body{margin:0}article,aside,details,figcaption,figure,footer,header,hgroup,main,menu,nav,section,summary{display:block}audio,canvas,progress,video{display:inline-block;vertical-align:baseline}audio:not([controls]){display:none;height:0}[hidden],template{display:none}a{background-color:transparent}a:active,a:hover{outline:0}abbr[title]{border-bottom:1px dotted}b,strong{font-weight:700}dfn{font-style:italic}h1{font-size:2em;margin:.67em 0}mark{background:#ff0;color:#000}small{font-size:80%}sub,sup{font-size:75%;line-height:0;position:relative;vertical-align:baseline}sup{top:-.5em}sub{bottom:-.25em}img{border:0}svg:not(:root){overflow:hidden}figure{margin:1em 40px}hr{-moz-box-sizing:content-box;box-sizing:content-box;height:0}pre{overflow:auto}code,kbd,pre,samp{font-family:monospace,monospace;font-size:1em}button,input,optgroup,select,textarea{color:inherit;font:inherit;margin:0}button{overflow:visible}button,select{text-transform:none}button,html input[type=button],input[type=reset],input[type=submit]{-webkit-appearance:button;cursor:pointer}button[disabled],html input[disabled]{cursor:default}button::-moz-focus-inner,input::-moz-focus-inner{border:0;padding:0}input{line-height:normal}input[type=checkbox],input[type=radio]{box-sizing:border-box;padding:0}input[type=number]::-webkit-inner-spin-button,input[type=number]::-webkit-outer-spin-button{height:auto}input[type=search]{-webkit-appearance:textfield;-moz-box-sizing:content-box;-webkit-box-sizing:content-box;box-sizing:content-box}input[type=search]::-webkit-search-cancel-button,input[type=search]::-webkit-search-decoration{-webkit-appearance:none}fieldset{border:1px solid silver;margin:0 2px;padding:.35em .625em .75em}legend{border:0;padding:0}textarea{overflow:auto}optgroup{font-weight:700}table{border-collapse:collapse;border-spacing:0}td,th{padding:0}  
    
    /* Custom styles */
    body { background: #1570A6; color: #222222; }
    body, input { font-family: Arial, sans-serif; font-size: 15px; }
    div { box-sizing: border-box; }
    .login { background: #fafafa; width: 420px; margin: 100px auto 0; border: 10px solid #12608C; }
    .head { background: none repeat scroll 0 0 #f0f0f0; border-bottom: 1px solid #d8d8d8; line-height: 50px; text-align: center; }
    .login-form { padding: 20px 20px; }
    .login-form p { margin-top: 0; }
    .form-group { width: 100%; }
    .control-label { font-size: 12px; font-weight: bold; }
    .form-group input[type=text],
    .form-group input[type=password] { width: 340px; padding: 10px; z-index: 9; position: relative; font-size: 15px; margin-top: 5px; margin-bottom: 5px; border: 1px solid #d8d8d8; border-radius: 3px; }
    .form-group .controls { display: inline-block; }
    .btn { background: -moz-linear-gradient(top, #d8d8d8 0%, #f8f8f8 100%); background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#d8d8d8), color-stop(100%,#f8f8f8)); background: -webkit-linear-gradient(top, #d8d8d8 0%,#f8f8f8 100%); background: -o-linear-gradient(top, #d8d8d8 0%,#f8f8f8 100%); background: -ms-linear-gradient(top, #d8d8d8 0%,#f8f8f8 100%); background: linear-gradient(to bottom, #d8d8d8 0%,#f8f8f8 100%); border: 1px solid #bbbbbb; border-radius: 3px; box-shadow: 1px 1px 0 #ffffff inset, 1px 3px 2px #d8d8d8; padding: 8px 20px; cursor: pointer; width: 100px; }
    .btn:active { box-shadow: 2px 2px 1px #cccccc inset; }
    .checkbox { float: right; line-height: 40px; }
  </style>
</head>
<body>
  <form method="post" id="LoginForm" runat="server">
    <% ((TextBox)LoginForm.FindControl("Email")).Attributes.Add("placeholder", "Email"); %>
    <% ((TextBox)LoginForm.FindControl("Password")).Attributes.Add("placeholder", "Password"); %>

    <div class="login">
      <div class="head">
        Login
      </div>
      <div class="login-form">
        <p>Use a local account to log in.</p>
        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
          <p class="text-danger">
            <asp:Literal runat="server" ID="FailureText" />
          </p>
        </asp:PlaceHolder>
        <div class="form-group">
          <asp:Label runat="server" AssociatedControlID="Email" CssClass="control-label">Email</asp:Label>
          <div>
            <asp:TextBox runat="server" ID="Email" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="The email field is required." />
          </div>
        </div>
        <div class="form-group">
          <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
          <div class="col-md-10">
            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
          </div>
        </div>
        <div class="form-group">
          <div class="checkbox">
            <asp:CheckBox runat="server" ID="RememberMe" />
            <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
          </div>
        </div>
        <div class="form-group">
          <div>
            <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn" />
          </div>
        </div>
      </div>
    </div>

    <script type="text/javascript">
      window.onload = function () { document.getElementById('<%=Email.ClientID%>').focus(); }
    </script>
  </form>
</body>
</html>
