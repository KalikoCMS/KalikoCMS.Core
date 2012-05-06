<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageTreeControl.ascx.cs" Inherits="KalikoCMS.Admin.Content.PageTree.PageTreeControl" %>
    
    <div class="btn-group" style="margin-left:4px;margin-top:-4px;margin-bottom:4px;">
        <a href="#" id="new-page-button" class="btn btn-small" rel="tooltip" data-original-title="Add a new page under<br/>the selected page below."><i class="icon-plus"></i> Add page</a>        
        <a href="#" id="remove-page-button" class="btn btn-small" rel="tooltip" data-original-title="Remove the selected page."><i class="icon-trash"></i> Remove page</a>
    </div>

    <div id="pagetree">
    </div>



    <script type="text/javascript">
        var currentPageId = "";

        $(document).ready(function () {
            initTreeView();
            $("#new-page-button").click(clickCreateNewPage);
            $("#remove-page-button").click(clickDeletePage);

            $("#new-page-button").tooltip({ placement: "bottom" });
            $("#remove-page-button").tooltip({ placement: "bottom" });
        });

        function clickCreateNewPage() {
            openModal("/Admin/Content/Dialogs/SelectPagetypeDialog.aspx?pageId=" + currentPageId, 500, 400);
            return false;
        }

        function clickDeletePage() {
            if (confirm("Really delete page?")) {
                deletePage(currentPageId);
            }
            return false;
        }

        function deletePage(pageId) {
            $.ajax({
                async: false,
                type: 'POST',
                url: "/Admin/Content/PageTree/JQueryTreeContent.ashx",
                data: {
                    "operation": "remove_node",
                    "id": currentPageId
                },
                success: function(r) {
                    if (!r.status) {
                        refreshTreeNode();
                    }
                }
            });
        }

        function createNewPage(pageTypeId) {
            closeModal();

            $("#maincontent").attr("src", "/Admin/Content/EditPage.aspx?id=&parentId=" + currentPageId + "&pageTypeId=" + pageTypeId);
        }

        function refreshTreeNode(node) {
            //$("#pagetree").jstree("refresh", $(".jstree-clicked"))
            $("#pagetree").jstree("refresh", "#node_" + node);
        }

        function initTreeView() {
            var plugins = ["themes", "json_data", "ui", "crrm", "cookies", "dnd", "search", "types", "hotkeys"];

            $("#pagetree").jstree({
                "plugins": plugins,
                "themes": { "theme": "classic", "url": "/Admin/Assets/Styles/classic/style.css" },
                "json_data": {
                    "ajax": {
                        "url": "/Admin/Content/PageTree/JQueryTreeContent.ashx",
                        "type": 'POST',
                        "data": function (n) {
                            return {
                                "operation": "get_children",
                                "id": n.attr ? n.attr("id").replace("node_", "") : "<%=Guid.Empty %>"
                            };
                        }
                    },
                    "data": [
                        {
                            "data": "Root",
                            "attr": { "id": "<%=Guid.Empty %>", "rel": "root" },
                            "state": "closed"
                        }
                    ]
                },
                "types": {
                    "max_depth": -2,
                    "max_children": -2,
                    "types": {
                        "default": {
                            "icon": { "image": "../file.png" }
                        },
                        "folder": {
                            "valid_children": ["default", "folder"],
                            "icon": { "image": "../folder.png" }
                        },
                        "root": {
                            "valid_children": ["default", "folder"],
                            "icon": { "image": "../root.png" },
                            "start_drag": false,
                            "move_node": false,
                            "delete_node": false,
                            "remove": false
                        }
                    }
                },
                "ui": { "disable_selecting_children": true },
                "core": {
                    "initially_open": ["<%=Guid.Empty %>"]
                }
            }).bind("select_node.jstree", function (event, data) {
                // `data.rslt.obj` is the jquery extended node that was clicked
                var _url = data.rslt.obj.attr("url");
                var pageId = data.rslt.obj.attr("id").replace("node_", "");

                currentPageId = pageId;

                $("#maincontent").attr("src", "/Admin/Content/EditPage.aspx?id=" + pageId);
                //$("#maincontent").attr("src", _url);
            })
            //	.bind("create.jstree", function (e, data) {
            //	    $.post(
            //			"./server.php",
            //			{
            //			    "operation": "create_node",
            //			    "id": data.rslt.parent.attr("id").replace("node_", ""),
            //			    "position": data.rslt.position,
            //			    "title": data.rslt.name,
            //			    "type": data.rslt.obj.attr("rel")
            //			},
            //			function (r) {
            //			    if (r.status) {
            //			        $(data.rslt.obj).attr("id", "node_" + r.id);
            //			    }
            //			    else {
            //			        $.jstree.rollback(data.rlbk);
            //			    }
            //			}
            //		);
            //	})
            //	.bind("remove.jstree", function (e, data) {
            //	    data.rslt.obj.each(function () {
            //	        $.ajax({
            //	            async: false,
            //	            type: 'POST',
            //	            url: "./server.php",
            //	            data: {
            //	                "operation": "remove_node",
            //	                "id": this.id.replace("node_", "")
            //	            },
            //	            success: function (r) {
            //	                if (!r.status) {
            //	                    data.inst.refresh();
            //	                }
            //	            }
            //	        });
            //	    });
            //	})
            //	.bind("rename.jstree", function (e, data) {
            //	    $.post(
            //			"./server.php",
            //			{
            //			    "operation": "rename_node",
            //			    "id": data.rslt.obj.attr("id").replace("node_", ""),
            //			    "title": data.rslt.new_name
            //			},
            //			function (r) {
            //			    if (!r.status) {
            //			        $.jstree.rollback(data.rlbk);
            //			    }
            //			}
            //		);
            //	})
                .bind("move_node.jstree", function (e, data) {
                    data.rslt.o.each(function (i) {
                        $.ajax({
                            async: false,
                            type: 'POST',
                            url: "/Admin/Content/PageTree/JQueryTreeContent.ashx",
                            data: {
                                "operation": "move_node",
                                "id": $(this).attr("id").replace("node_", ""),
                                "ref": data.rslt.cr === -1 ? 1 : data.rslt.np.attr("id").replace("node_", ""),
                                "position": data.rslt.cp + i,
                                "title": data.rslt.name,
                                "copy": data.rslt.cy ? 1 : 0
                            },
                            success: function (r) {
                                if (!r.success) {
                                    $.jstree.rollback(data.rlbk);
                                    alert("Could not move " + data.rslt.name);
                                } else {
                                    $(data.rslt.oc).attr("id", "node_" + r.id);
                                    if (data.rslt.cy && $(data.rslt.oc).children("UL").length) {
                                        data.inst.refresh(data.inst._get_parent(data.rslt.oc));
                                    }
                                }
                            }
                        });
                    });
                });

        }

    </script>
