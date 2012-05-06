<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.DateTimePropertyEditor" %>
<div class="control-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <div class="input-append">
            <asp:TextBox runat="server" ID="ValueField" CssClass="span2" /><a class="btn" href="#"><i class="icon-calendar"></i></a>
        </div>
        <asp:Label ID="ErrorText" CssClass="help-inline" runat="server" />
    </div>
    <script>
        $(function () {
            $("#<%=ValueField.ClientID %>").datepicker({ constrainInput: false, showOn: "none", onSelect: function (dateStr, inst) {
                var d2 = $("#<%=ValueField.ClientID %>").datetimeEntry('getDatetime');
                d2.setDate(inst.selectedDay);
                d2.setMonth(inst.selectedMonth);
                d2.setYear(inst.selectedYear);
                $("#<%=ValueField.ClientID %>").datetimeEntry('setDatetime', d2);
            }
            });
            $("#<%=ValueField.ClientID %> + a").click(function () { $("#<%=ValueField.ClientID %>").datepicker("show"); });
            $("#<%=ValueField.ClientID %>").datetimeEntry({ datetimeFormat: 'O/D/Y H:M:S', spinnerImage: '' });
        });
    </script>
</div>
