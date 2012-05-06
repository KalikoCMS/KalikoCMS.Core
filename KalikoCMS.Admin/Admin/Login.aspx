<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KalikoCMS.Admin.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Login</title>
    <meta name="robots" content="noindex, nofollow" />
    <link href="/Admin/Assets/Styles/bootstrap.css" rel="stylesheet" />
    <style type="text/css">
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div class="login">
        <asp:Login ID="Login1" runat="server" FailureText="Wrong username or password">
            <LayoutTemplate>
                <fieldset class="form-horizontal">
                    <legend>Log In</legend>
                    <div class="control-group">
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="control-label">User Name:</asp:Label>
                        <div class="controls">
                            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="control-group">
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="control-label">Password:</asp:Label>
                        <div class="controls">
                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="control-group">
                        <div class="controls">
                            <asp:CheckBox ID="RememberMe" runat="server" CssClass="checkbox" Text="Remember me next time." />&nbsp;
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1"
                                CssClass="button" />
                        </div>
                    </div>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Login1" />
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                    </div>
                </fieldset>
            </LayoutTemplate>
        </asp:Login>
        <br class="clearfloat" />
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
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" ValidationGroup="PasswordRecovery1"
                            CssClass="button" />
                    </div>
                </fieldset>
            </UserNameTemplate>
        </asp:PasswordRecovery>
    </div>

    <script type="text/javascript">
        window.onload = setFocus;
        function setFocus() { document.getElementById('Login1_UserName').focus(); }
    </script>

    </form>
</body>
</html>
