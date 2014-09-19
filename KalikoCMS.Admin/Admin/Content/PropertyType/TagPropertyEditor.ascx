<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.TagPropertyEditor" %>
<div class="form-group">
  <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" Text="Tag" CssClass="control-label col-xs-2" />
  <div class="controls col-xs-10">
    <asp:HiddenField runat="server" ID="TagContext" />
    <asp:TextBox runat="server" CssClass="form-control" ID="ValueField" />
    <asp:Literal ID="ErrorText" runat="server" />
  </div>
  <asp:Literal ID="Script" runat="server" />
</div>
