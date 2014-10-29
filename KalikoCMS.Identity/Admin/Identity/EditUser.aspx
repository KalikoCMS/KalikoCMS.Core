<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="KalikoCMS.Identity.Admin.Identity.EditUser" MasterPageFile="../Templates/MasterPages/Admin.Master" %>

<%@ Register Src="IdentityMenu.ascx" TagPrefix="admin" TagName="Menu" %>

<asp:Content ContentPlaceHolderID="LeftRegion" runat="server">
  <admin:menu activemenuitem="Users" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
  <div class="main-area admin-page">
    <div class="page-head">
      <h1>Edit user</h1>
    </div>

    <div class="main-content">
      <div class="inner-container form-horizontal">
        <asp:Literal ID="Feedback" runat="server" />

        <fieldset>
          <asp:Panel ID="FormFields" runat="server">
            <legend>Information</legend>
            <asp:HiddenField ID="UserId" runat="server" />

            <div class="form-group">
              <asp:Label AssociatedControlID="UserName" CssClass="control-label col-xs-2" runat="server">Username</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="UserName" CssClass="form-control" ReadOnly="True" runat="server" />
              </div>
            </div>
            
            <div class="form-group">
              <asp:Label AssociatedControlID="Password" CssClass="control-label col-xs-2" runat="server">Password</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="Password" CssClass="form-control" TextMode="Password" runat="server" />
              </div>
            </div>
                        
            <div class="form-group">
              <asp:Label AssociatedControlID="ConfirmPassword" CssClass="control-label col-xs-2" runat="server">Confirm password</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="ConfirmPassword" CssClass="form-control" TextMode="Password" runat="server" />
              </div>
            </div>
            
            <div class="form-group">
              <asp:Label AssociatedControlID="FirstName" CssClass="control-label col-xs-2" runat="server">First name</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="FirstName" CssClass="form-control" runat="server" />
              </div>
            </div>
            
            <div class="form-group">
              <asp:Label AssociatedControlID="SurName" CssClass="control-label col-xs-2" runat="server">Surname</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="SurName" CssClass="form-control" runat="server" />
              </div>
            </div>
            
            <div class="form-group">
              <asp:Label AssociatedControlID="Email" CssClass="control-label col-xs-2" runat="server">Email</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="Email" CssClass="form-control" TextMode="Email" runat="server" />
              </div>
            </div>
            
            <div class="form-group">
              <asp:Label AssociatedControlID="PhoneNumber" CssClass="control-label col-xs-2" runat="server">Phone number</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="PhoneNumber" CssClass="form-control" runat="server" />
              </div>
            </div>
            
            <div class="form-group">
              <asp:Label CssClass="control-label col-xs-2" runat="server">Roles</asp:Label>
              <div class="col-xs-10">
                <ul class="list-unstyled">
                  <asp:Literal ID="Roles" runat="server" />
                </ul>
              </div>
            </div>

          </asp:Panel>
          <div class="form-buttons">
            <asp:LinkButton ID="SaveButton" CssClass="btn btn-primary" runat="server"><i class="icon-thumbs-up"></i> Save changes</asp:LinkButton>
            <a href="Identity/Users.aspx" class="btn btn-default">Cancel</a>
            <a href="#" id="deleteButton" class="btn btn-danger"><i class="icon-remove"></i> Delete user</a>
          </div>
        </fieldset>
      </div>
    </div>
  </div>
  <script>
    function deleteUserConfirm(e) {
      e.preventDefault();
      bootbox.confirm("Do you really want to delete this user?<br/>This cannot be undone.", function (result) {
        if (result === true) {
          document.location = "Identity/DeleteUser.aspx?id=<%=Request.QueryString["id"]%>";
        }
      });
    }

    $(document).ready(function () {
      $('#deleteButton').click(deleteUserConfirm);
    });
  </script>
</asp:Content>

