<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KalikoCMS.Admin.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Login</title>
    <meta name="robots" content="noindex, nofollow" />
    <style type="text/css">

body {
    background-color: #FFFFFF;
    color: #333333;
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
    font-size: 14px;
    line-height: 1.42857;
}
.container:before, .container:after {
    content: " ";
    display: table;
}
*, *:before, *:after {
    -moz-box-sizing: border-box;
}
.container:after {
    clear: both;
}

.container {
    max-width: 1170px;
}
.container {
    margin-left: auto;
    margin-right: auto;
    padding-left: 15px;
    padding-right: 15px;
}
.row {
    margin-left: -15px;
    margin-right: -15px;
}
.row:after {
    clear: both;
}
.row:before, .row:after {
    content: " ";
    display: table;
}

.col-md-offset-4 {
    margin-left: 33.3333%;
}
.col-md-4 {
    width: 33.3333%;
}
.col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11 {
    float: left;
}
.col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12 {
    min-height: 1px;
    padding-left: 15px;
    padding-right: 15px;
    position: relative;
}

.login-title {
    color: #555555;
    display: block;
    font-size: 18px;
    font-weight: 400;
}
.text-center {
    text-align: center;
}
h1, h2, h3 {
    margin-bottom: 10px;
    margin-top: 20px;
}
h1, h2, h3, h4, h5, h6, .h1, .h2, .h3, .h4, .h5, .h6 {
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
    line-height: 1.1;
}
.account-wall {
    background-color: #F7F7F7;
    box-shadow: 0 2px 2px rgba(0, 0, 0, 0.3);
    margin-top: 20px;
    padding: 40px 0 20px;
}
.form-signin {
    margin: 0 auto;
    max-width: 330px;
    padding: 15px;
}

.form-signin input[type="text"] {
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
    margin-bottom: -1px;
}
.form-signin .form-control {
    -moz-box-sizing: border-box;
    font-size: 16px;
    height: auto;
    padding: 10px;
    position: relative;
}
.form-control {
    background-color: #FFFFFF;
    border: 1px solid #CCCCCC;
    border-radius: 4px 4px 4px 4px;
    box-shadow: 0 1px 1px rgba(0, 0, 0, 0.075) inset;
    color: #555555;
    display: block;
    font-size: 14px;
    height: 34px;
    line-height: 1.42857;
    padding: 6px 12px;
    transition: border-color 0.15s ease-in-out 0s, box-shadow 0.15s ease-in-out 0s;
    vertical-align: middle;
    width: 100%;
}
button, input, select[multiple], textarea {
    background-image: none;
}
input, button, select, textarea {
    font-family: inherit;
    font-size: inherit;
    line-height: inherit;
}
button, input {
    line-height: normal;
}
button, input, select, textarea {
    font-family: inherit;
    font-size: 100%;
    margin: 0;
}
*, *:before, *:after {
    -moz-box-sizing: border-box;
}
.form-control::-moz-placeholder {
    color: #999999;
}
.form-signin input[type="password"] {
    border-top-left-radius: 0;
    border-top-right-radius: 0;
    margin-bottom: 10px;
}.btn-block {
    display: block;
    padding-left: 0;
    padding-right: 0;
    width: 100%;
}
.btn-lg {
    border-radius: 6px 6px 6px 6px;
    font-size: 18px;
    line-height: 1.33;
    padding: 10px 16px;
}
.btn-primary {
    background-color: #428BCA;
    border-color: #357EBD;
    color: #FFFFFF;
}
.btn {
    -moz-user-select: none;
    border: 1px solid rgba(0, 0, 0, 0);
    border-radius: 4px 4px 4px 4px;
    cursor: pointer;
    display: inline-block;
    font-size: 14px;
    font-weight: normal;
    line-height: 1.42857;
    margin-bottom: 0;
    padding: 6px 12px;
    text-align: center;
    vertical-align: middle;
    white-space: nowrap;
}
button, input, select[multiple], textarea {
    background-image: none;
}
input, button, select, textarea {
    font-family: inherit;
    font-size: inherit;
    line-height: inherit;
}
button, html input[type="button"], input[type="reset"], input[type="submit"] {
    cursor: pointer;
}
button, select {
    text-transform: none;
}
button, input {
    line-height: normal;
}
button, input, select, textarea {
    font-family: inherit;
    font-size: 100%;
    margin: 0;
}
    </style>
</head>
<body>
  <div class="container">
    <div class="row">
      <div class="col-sm-6 col-md-4 col-md-offset-4">
        <h1 class="text-center login-title">
          Sign in to continue to Bootsnipp</h1>
        <div class="account-wall">
          <form id="Form1" method="post" class="form-signin" runat="server">
          <asp:Login ID="Login1" runat="server" FailureText="Wrong username or password">
            <LayoutTemplate>
              <asp:TextBox ID="UserName" CssClass="form-control" runat="server"></asp:TextBox>
              <asp:TextBox ID="Password" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
              <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" CssClass="btn btn-lg btn-primary btn-block" />
              <label class="checkbox pull-left">
                <asp:CheckBox ID="RememberMe" runat="server" CssClass="checkbox" Text="Remember me next time." />&nbsp;
              </label>
              <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Login1" />
              <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
            </LayoutTemplate>
          </asp:Login>
          <asp:LinkButton ID="lnkPassword" runat="server" PostBackUrl="#" OnClick="lnkPassword_Click">Remind me of my password</asp:LinkButton>
          <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" Visible="false">
            <UserNameTemplate>
              <fieldset>
                <legend>Password recovery</legend>
                <div class="fieldset_inner">
                  <p>
                    Enter your User Name to receive your password.</p>
                  <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                  <asp:TextBox ID="UserName" runat="server" CssClass="email"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                  <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                  <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" ValidationGroup="PasswordRecovery1" CssClass="button" />
                </div>
              </fieldset>
            </UserNameTemplate>
          </asp:PasswordRecovery>
          </form>
        </div>
      </div>
    </div>
  </div>
  <script type="text/javascript">
    window.onload = setFocus;
    function setFocus() { document.getElementById('Login1_UserName').focus(); }
  </script>
</body>
</html>
