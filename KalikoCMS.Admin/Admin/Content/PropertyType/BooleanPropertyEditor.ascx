<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BooleanPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.BooleanPropertyEditor" %>
<div class="form-group">
  <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" Text="Boolean" CssClass="control-label col-xs-2" />
  <div class="controls col-xs-10">
    <div class="checkbox">
      <asp:CheckBox runat="server" ID="ValueField" />
      <asp:Label AssociatedControlID="ValueField" runat="server" />
    </div>
    <asp:Literal ID="ErrorText" runat="server" />
  </div>
</div>
