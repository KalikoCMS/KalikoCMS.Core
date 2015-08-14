<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageTreeControl.ascx.cs" Inherits="KalikoCMS.Admin.Content.PageTree.PageTreeControl" %>

<div class="btn-group pagetree-controls">
  <a href="#" id="new-page-button" class="btn btn-sm btn-default" rel="tooltip" data-html="true" data-original-title="Add a new page under<br/>the selected page." data-placement="bottom" data-container="body"><i class="icon icon-plus"></i>Add page</a>
  <a href="#" id="remove-page-button" class="btn btn-sm btn-default" rel="tooltip" data-html="true" data-original-title="Remove the selected page." data-placement="bottom" data-container="body"><i class="icon icon-trash"></i>Remove page</a>
</div>

<div id="pagetree"></div>



<script type="text/javascript">
  // TODO: Clean up code

  var currentPageId = "";

  $(document).ready(function () {
    initTreeView();
    $("#new-page-button").click(clickCreateNewPage);
    $("#remove-page-button").click(clickDeletePage);

    $("#new-page-button").tooltip();
    $("#remove-page-button").tooltip();
  });

  function clickCreateNewPage(e) {
    top.openModal("Content/Dialogs/SelectPagetypeDialog.aspx?pageId=" + currentPageId, 700, 500);
    e.preventDefault();
  }

  function clickDeletePage(e) {
    if (currentPageId == '00000000-0000-0000-0000-000000000000') {
      bootbox.alert("<i class=\"icon-ban\"></i> The root cannot be deleted!");
      e.preventDefault();
      return;
    }

    bootbox.confirm("<i class=\"icon-warning\"></i> Are you sure you want to delete the page?" , function (result) {
      if (result == true) {
        deletePage(currentPageId);
      }
    });
    e.preventDefault();
  }

  function deletePage() {
    $.ajax({
      async: false,
      type: 'POST',
      url: "Content/PageTree/JQueryTreeContent.ashx",
      data: {
        "operation": "remove_node",
        "id": currentPageId
      },
      success: function (r) {
        if (!r.status) {
          selectParentNode(currentPageId);
        }
      }
    });
  }

  function createNewPage(pageTypeId) {
    top.closeModal();

    $("#maincontent").attr("src", "Content/EditPage.aspx?id=&parentId=" + currentPageId + "&pageTypeId=" + pageTypeId);
  }

  function selectParentNode(node) {
      var instance = $.jstree.reference('#pagetree');
      var parent = instance.get_parent(node);
      instance.load_node(parent, function (n, s) {
          instance.deselect_all();
          instance.select_node(parent);
      });
  }

  function refreshNode(node) {
    var instance = $.jstree.reference('#pagetree');
    var parent = instance.get_parent(node);
    instance.load_node(parent, function (n, s) {
      instance.deselect_all();
      instance.select_node(node);
    });
  }

  function refreshParentNode(node) {
    var instance = $.jstree.reference('#pagetree');
    var parent = instance.get_parent(node);
    instance.load_node(parent);
  }

  function refreshTreeNode(parentNode, addedNode) {
    var instance = $.jstree.reference('#pagetree');
    instance.load_node(parentNode, function (n, s) {
      instance.deselect_all();
      instance.select_node(addedNode);
    });
  }

  function initTreeView() {
    $("#pagetree")
        .jstree({
          "plugins": ["state", "dnd"],
          "core": {
            'data': {
              'url': 'Content/PageTree/JQueryTreeContent.ashx',
              'type': 'POST',
              'dataType': 'JSON',
              'data': function (n) {
                return {
                  'operation': 'get_children',
                  'id': n.id
                };
              }
            },
            'check_callback': function (o, n, p, i, m) {
              if (m && m.dnd && p.id === '#') {
                return false;
              }
              return true;
            },
            'themes': {
              'dots': true
            },
            'multiple': false
          }
        })
        .bind("before.jstree", function (e, data) {
          if (data.func === "remove" && !confirm("Are you sure you want to delete the page?")) {
            e.stopImmediatePropagation();
            return false;
          }
        })
        .bind("select_node.jstree", function (event, data) {
          var pageId = data.node.id;
          data.instance.open_node(data.node);
          currentPageId = pageId;

          $("#maincontent").attr("src", "Content/EditPage.aspx?id=" + pageId);
        })
        .bind("move_node.jstree", function (e, data) {
        var moveNode = function(result) {
            if (result == true) {
                $.ajax({
                    async: false,
                    type: 'POST',
                    url: "Content/PageTree/JQueryTreeContent.ashx",
                    data: {
                        "operation": "move_node",
                        "id": data.node.id,
                        "ref": data.parent,
                        "old": data.old_parent,
                        "position": data.position
                    },
                    success: function(r) {
                        data.instance.load_node(data.parent);
                        if (data.parent != data.old_parent) {
                            data.instance.load_node(data.old_parent);
                        }

                        if (!r.success) {
                            if (r.message.length > 0) {
                                bootbox.alert(r.message);
                            }
                            else {
                                bootbox.alert("Could not move page!");
                            }
                            console.log(data.node);
                        }
                    }
                });
            }
            else {
                data.instance.load_node(data.parent);
                data.instance.load_node(data.old_parent);
            }
        };

        if (data.parent == data.old_parent) {
            moveNode(true);
        }
        else {
            bootbox.confirm("Move page?", moveNode);
        }
    });
  }

</script>
