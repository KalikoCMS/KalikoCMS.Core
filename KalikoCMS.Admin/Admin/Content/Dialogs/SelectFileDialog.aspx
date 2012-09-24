<%@ Page Title="Select file" Language="C#" MasterPageFile="~/Admin/Content/Dialogs/Dialog.Master" AutoEventWireup="true" CodeBehind="SelectFileDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.SelectFileDialog" %>
<%@ Register TagPrefix="cms" Namespace="KalikoCMS.WebControls" Assembly="KalikoCMS.Engine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainArea" runat="server">
    <style>
        .file-list { width: 100%; }
        .file-list tr { cursor: pointer;padding: 0 5px; }
        .table-striped.file-list tbody tr.selected td { background: #ccddff; }
    </style>

    <table id="file-list" class="table-striped file-list">
<%--        <cms:FileList BaseUrl="/Files/Images/" AutoBind="True" runat="server">
            <FileTemplate>
                <tr data-path="<%#Container.FileItem.Name %>" class="file">
                    <td><%#Container.FileItem.Name %></td>
                    <td style="text-align: right;"><%#Container.FileItem.Size %></td>
                </tr>
            </FileTemplate>
            <FolderTemplate>
                <tr data-path="<%#Container.FileItem.Name %>" class="folder">
                    <td colspan="2"><%#Container.FileItem.Name %></td>
                </tr>
            </FolderTemplate>
        </cms:FileList>--%>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ButtonArea" runat="server">
    <a id="select-button"class="btn btn-primary disabled" href="javascript:selectFile();">Select file</a>
    <a id="deselect-button" href="javascript:noPage();" class="btn">No file</a>
    <a data-dismiss="modal" class="btn" href="javascript:close();">Cancel</a>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptArea" runat="server">
    <script>
        var path = "/Files/";
        var selectedFile = "";
        var callback;

        $(document).ready(function () {
            loadFileList();
        });

        function loadFileList() {
            $.ajax({
                type: 'POST',
                url: '/Admin/Content/Dialogs/SelectFileDialog.aspx/GetFileList',
                data: '{ path: "' + path + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d.status == 200) {
                        var fileList = $('.file-list');
                        fileList.html('');
                        
                        var folders = msg.d.data.folders;
                        for (var i in folders) {
                            var folder = folders[i];
                            fileList.append('<tr data-path="' + folder.name + '" class="folder"><td colspan="2">' + folder.name + '</td></tr>');
                        }

                        var files = msg.d.data.files;
                        for (i in files) {
                            var file = files[i];
                            fileList.append('<tr data-path="' + file.name + '" class="file"><td colspan="2">' + file.name + '</td></tr>');
                        }

                        $('.file-list tr.file').click(function () {
                            $('.file-list tr').removeClass('selected');
                            $(this).addClass('selected');

                            selectedFile = $(this).attr('data-path');
                            $('#select-button').removeClass('disabled');
                        });
                        $('.file-list tr.folder').click(function () {
                            path += $(this).attr('data-path') + '/';
                            loadFileList();
                        });

                    } else {
                        alert(msg.d.data);
                    }
                }
            });
        }


        function selectFile() {
            callback(path + selectedFile);
            close();

        }

        function noPage() {
            callback('');
            close();
        }
    </script>
</asp:Content>
