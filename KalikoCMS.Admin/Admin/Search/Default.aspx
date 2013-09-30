<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KalikoCMS.Admin.Search.Default" MasterPageFile="../Templates/MasterPages/Admin.Master" %>


<asp:Content ContentPlaceHolderID="FullRegion" runat="server">
  <asp:Literal ID="ResultBox" runat="server" />
  <asp:Button ID="ReindexButton" runat="server" CssClass="btn" Text="Reindex website" />
</asp:Content>
