<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageVersionDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.PageVersionDialog" MasterPageFile="Dialog.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainArea" runat="server">
    <table class="table table-striped">
        <thead>
            <tr>
                <th>Version</th>
                <th>Modified</th>
                <th>Modified by</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <asp:Literal ID="VersionRows" runat="server" />
        </tbody>
    </table>
</asp:Content>