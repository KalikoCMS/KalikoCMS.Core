<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageVersionDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.PageVersionDialog" MasterPageFile="Dialog.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainArea" runat="server">
    <div class="fill-area">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Version</th>
                    <th>Modified</th>
                    <th>Modified by</th>
                    <th>Status</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <asp:Literal ID="VersionRows" runat="server" />
            </tbody>
        </table>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptArea" runat="server">
    <script type="text/javascript">
        function editPage() {
            var pageId = $(this).data('pageid');
            var version = $(this).data('version');
            top.loadInMainContent('Content/EditPage.aspx?id=' + pageId + '&version=' + version);
            top.closeModal();
        }

        $(document).ready(function() {
            $('.edit-button').click(editPage);
        });
    </script>
</asp:Content>