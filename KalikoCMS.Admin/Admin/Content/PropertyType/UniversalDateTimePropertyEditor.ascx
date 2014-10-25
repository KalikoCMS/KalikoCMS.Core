<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UniversalDateTimePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.UniversalDateTimePropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" Text="Datetime" CssClass="control-label col-xs-2" />
    <asp:Panel ID="DateControl" runat="server" CssClass="controls col-xs-4">
        <div class="input-group date">
            <asp:TextBox runat="server" ID="ValueField" CssClass="form-control narrow-input" /><span class="input-group-btn add-on"><button class="btn btn-default" type="button"><i data-time-icon="icon icon-clock-o" data-date-icon="icon icon-calendar" class="icon icon-clock-o"></i></button></span>
        </div>
        <asp:Label ID="ErrorText" CssClass="help-inline" runat="server" />
    </asp:Panel>
    <asp:HiddenField runat="server" ID="UniversalDateField" />
    <script type="text/javascript">
        $(function() {
            var element = $('#<%=DateControl.ClientID %>');
            element.datetimepicker();
            var picker = element.data('datetimepicker');
            var date = picker.parseDate($('#<%=UniversalDateField.ClientID %>').val());
            if (date != null && !isNaN(date.getTime())) {
                picker.setLocalDate(date);
            }

            element.on('changeDate', function(e) {
                var newDate = "";
                if (e.date != null && !isNaN(e.date.getTime())) {
                    newDate = moment.utc(e.localDate).format('<%=KalikoCMS.Configuration.SiteSettings.Instance.DateFormat.Replace("yyyy", "YYYY").Replace("dd", "DD")%>');
                }
                $('#<%=UniversalDateField.ClientID %>').val(newDate);
            });
        });
    </script>
</div>
