<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.DateTimePropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label col-xs-2" />
    <asp:Panel ID="DateControl" runat="server" CssClass="controls col-xs-4">
        <div class="input-group date">
            <asp:TextBox runat="server" ID="ValueField" CssClass="form-control narrow-input" /><span class="input-group-btn add-on"><i data-time-icon="icon-time" data-date-icon="icon-calendar" class="btn btn-default icon-time"></i></span>
        </div>
        <asp:Label ID="ErrorText" CssClass="help-inline" runat="server" />
    </asp:Panel>
    <script type="text/javascript">
      $(function () {
        $("#<%=DateControl.ClientID %>").datetimepicker();
      });
    </script>
</div>
