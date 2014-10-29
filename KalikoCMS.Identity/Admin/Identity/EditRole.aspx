<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditRole.aspx.cs" Inherits="KalikoCMS.Identity.Admin.Identity.EditRole" MasterPageFile="../Templates/MasterPages/Admin.Master" %>

<%@ Register Src="IdentityMenu.ascx" TagPrefix="admin" TagName="Menu" %>

<asp:Content ContentPlaceHolderID="LeftRegion" runat="server">
  <admin:menu activemenuitem="Roles" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
  <div class="main-area admin-page">
    <div class="page-head">
      <h1>Edit role</h1>
    </div>

    <div class="main-content">
      <div class="inner-container form-horizontal">
        <asp:Literal ID="Feedback" runat="server" />

        <fieldset>
          <asp:Panel ID="FormFields" runat="server">
            <legend>Information</legend>
            <asp:HiddenField ID="RoleId" runat="server" />
            <div class="form-group">
              <asp:Label AssociatedControlID="RoleName" CssClass="control-label col-xs-2" runat="server">Role name</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="RoleName" CssClass="form-control" runat="server" />
              </div>
            </div>
          </asp:Panel>
          <div class="form-buttons">
            <asp:LinkButton ID="SaveButton" CssClass="btn btn-primary" runat="server"><i class="icon-thumbs-up"></i> Save changes</asp:LinkButton>
            <a href="Identity/Roles.aspx" class="btn btn-default">Cancel</a>
            <a href="#" id="deleteButton" class="btn btn-danger"><i class="icon-remove"></i> Delete role</a>
          </div>
        </fieldset>


        <fieldset>
          <legend>Users in role</legend>

          <table class="table table-striped table-hover">
            <thead>
              <tr>
                <th style="width: 20%;">Username</th>
                <th style="width: 25%;">Real name</th>
                <th style="width: 25%;">Email</th>
                <th style="width: 15%;">Created</th>
                <th style="width: 15%;">Updated</th>
              </tr>
            </thead>
            <tbody>
              <asp:Literal ID="UserList" runat="server" />
            </tbody>
          </table>
        </fieldset>
      </div>
    </div>
  </div>
  <script>
    function deleteRoleConfirm(e) {
      e.preventDefault();
      bootbox.confirm("Do you really want to delete this role?<br/>This cannot be undone.", function (result) {
        if (result === true) {
          document.location = "Identity/DeleteRole.aspx?id=<%=Request.QueryString["id"]%>";
        }
      });
    }

    $(document).ready(function () {
      $('#deleteButton').click(deleteRoleConfirm);
    });
  </script>
</asp:Content>

