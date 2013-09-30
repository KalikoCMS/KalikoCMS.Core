<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.DateTimePropertyEditor" %>
<div class="control-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label" />
    <asp:Panel ID="DateControl" runat="server" CssClass="controls">
        <div class="input-append date">
            <asp:TextBox runat="server" ID="ValueField" CssClass="narrow-input" /><span class="add-on"><i data-time-icon="icon-time" data-date-icon="icon-calendar"></i></span>
        </div>
        <asp:Label ID="ErrorText" CssClass="help-inline" runat="server" />
    </asp:Panel>
    <script type="text/javascript">
      $(function () {
        $("#<%=DateControl.ClientID %>").datetimepicker();
      });
    </script>
</div>
