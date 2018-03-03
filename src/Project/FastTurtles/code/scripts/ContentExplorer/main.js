function init(data) {
    var $ = go.GraphObject.make;  // for conciseness in defining templates

    myDiagram =
      $(go.Diagram, "myDiagramDiv", // must be the ID or reference to div
        {
            initialContentAlignment: go.Spot.Center,
            maxSelectionCount: 1,
            layout:
              $(go.TreeLayout,
                {
                    treeStyle: go.TreeLayout.StyleLastParents,
                    arrangement: go.TreeLayout.ArrangementHorizontal,
                    // properties for most of the tree:
                    angle: 90,
                    layerSpacing: 35,
                    // properties for the "last parents":
                    alternateAngle: 90,
                    alternateLayerSpacing: 35,
                    alternateAlignment: go.TreeLayout.AlignmentBus,
                    alternateNodeSpacing: 20
                }),
            "undoManager.isEnabled": true // enable undo & redo
        });

    // when the document is modified, add a "*" to the title and enable the "Save" button
    myDiagram.addDiagramListener("Modified", function (e) {
        var button = document.getElementById("SaveButton");
        if (button) button.disabled = !myDiagram.isModified;
        var idx = document.title.indexOf("*");
        if (myDiagram.isModified) {
            if (idx < 0) document.title += "*";
        } else {
            if (idx >= 0) document.title = document.title.substr(0, idx);
        }
    });

    // manage boss info manually when a node or link is deleted from the diagram
    myDiagram.addDiagramListener("SelectionDeleting", function (e) {
        var part = e.subject.first(); // e.subject is the myDiagram.selection collection,
        // so we'll get the first since we know we only have one selection
        myDiagram.startTransaction("clear boss");
        if (part instanceof go.Node) {
            var it = part.findTreeChildrenNodes(); // find all child nodes
            while (it.next()) { // now iterate through them and clear out the boss information
                var child = it.value;
                var bossText = child.findObject("boss"); // since the boss TextBlock is named, we can access it by name
                if (bossText === null) return;
                bossText.text = "";
            }
        } else if (part instanceof go.Link) {
            var child = part.toNode;
            var bossText = child.findObject("boss"); // since the boss TextBlock is named, we can access it by name
            if (bossText === null) return;
            bossText.text = "";
        }
        myDiagram.commitTransaction("clear boss");
    });

    var levelColors = ["#AC193D", "#2672EC", "#8C0095", "#5133AB",
                       "#008299", "#D24726", "#008A00", "#094AB2"];

    // override TreeLayout.commitNodes to also modify the background brush based on the tree depth level
    myDiagram.layout.commitNodes = function () {
        go.TreeLayout.prototype.commitNodes.call(myDiagram.layout);  // do the standard behavior
        // then go through all of the vertexes and set their corresponding node's Shape.fill
        // to a brush dependent on the TreeVertex.level value
        myDiagram.layout.network.vertexes.each(function (v) {
            if (v.node) {
                var level = v.level % (levelColors.length);
                var color = levelColors[level];
                var shape = v.node.findObject("SHAPE");
                if (shape) shape.fill = $(go.Brush, "Linear", { 0: color, 1: go.Brush.lightenBy(color, 0.05), start: go.Spot.Left, end: go.Spot.Right });
            }
        });
    };

    // This function is used to find a suitable ID when modifying/creating nodes.
    // We used the counter combined with findNodeDataForKey to ensure uniqueness.
    function getNextKey() {
        var key = nodeIdCounter;
        while (myDiagram.model.findNodeDataForKey(key) !== null) {
            key = nodeIdCounter--;
        }
        return key;
    }

    myDiagram.toolManager.panningTool.isEnabled = false;
    var nodeIdCounter = -1; // use a sequence to guarantee key uniqueness as we add/remove/modify nodes

    // This function provides a common style for most of the TextBlocks.
    // Some of these values may be overridden in a particular TextBlock.
    function textStyle() {
        return { font: "9pt  Segoe UI,sans-serif", stroke: "white" };
    }

    // This converter is used by the Picture.
    function findHeadShot(icon) {
        return icon;
    }

    function nodeClick(e, obj) {
        var source,
            template;

        jQuery.ajax({
            url: '/sitecore/api/layout/render/jss?item=' + (obj.data.path ? obj.data.path : '/') + '&sc_lang=en&sc_apikey={1EEAEFC8-A093-492F-BAB6-00F55B1DAEF5}',
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                source = jQuery('[data-template="node-item"]').html();
                template = Handlebars.compile(source);
                // Register the list partial that "main" uses.
                Handlebars.registerPartial("placeholdersBlock", jQuery("[data-template=\"placeholdersBlock\"]").html());
                console.log(data.sitecore);
                jQuery('#myModal').find('.modal-body').html(template(data.sitecore));
                jQuery('#myModal').modal('show');

            }
        });
    }

    // define the Node template
    myDiagram.nodeTemplate =
      $(go.Node, "Auto",
        {
            doubleClick: nodeClick,
            deletable: false
        },
        // for sorting, have the Node.text be the data.name
        new go.Binding("text", "name"),
        // bind the Part.layerName to control the Node's layer depending on whether it isSelected
        new go.Binding("layerName", "isSelected", function (sel) { return sel ? "Foreground" : ""; }).ofObject(),
        // define the node's outer shape
        $(go.Shape, "Rectangle",
          {
              name: "SHAPE", fill: "white", stroke: null,
              // set the port properties:
              portId: "", fromLinkable: false, toLinkable: false, cursor: "pointer"
          }),
        $(go.Panel, "Horizontal",
          $(go.Picture,
            {
                name: "Picture",
                desiredSize: new go.Size(32, 32),
                margin: new go.Margin(6, 8, 6, 10),
            },
            new go.Binding("source", "icon", findHeadShot)),
          // define the panel where the text will appear
          $(go.Panel, "Table",
            {
                maxSize: new go.Size(150, 999),
                margin: new go.Margin(6, 10, 0, 3),
                defaultAlignment: go.Spot.Left
            },
            $(go.RowColumnDefinition, { column: 2, width: 4 }),
            $(go.TextBlock, textStyle(),  // the name
              {
                  row: 0, column: 0, columnSpan: 5,
                  font: "12pt Segoe UI,sans-serif",
                  editable: false, isMultiline: false,
                  minSize: new go.Size(10, 16)
              },
              new go.Binding("text", "name").makeTwoWay()),
            $(go.TextBlock, "Title: ", textStyle(),
              { row: 1, column: 0 }),
            $(go.TextBlock, textStyle(),
              {
                  row: 1, column: 1, columnSpan: 4,
                  editable: false, isMultiline: false,
                  minSize: new go.Size(10, 14),
                  margin: new go.Margin(0, 0, 0, 3)
              },
              new go.Binding("text", "title").makeTwoWay()),
            $(go.TextBlock, textStyle(),  // the comments
              {
                  row: 3, column: 0, columnSpan: 5,
                  font: "italic 9pt sans-serif",
                  wrap: go.TextBlock.WrapFit,
                  editable: false,
                  minSize: new go.Size(10, 14)
              },
              new go.Binding("text", "comments").makeTwoWay())
          )  // end Table Panel
        ) // end Horizontal Panel
      );  // end Node

    // the context menu allows users to make a position vacant,
    // remove a role and reassign the subtree, or remove a department
    myDiagram.nodeTemplate.contextMenu =
      $(go.Adornment, "Vertical",
        $("ContextMenuButton",
          $(go.TextBlock, "Vacate Position"),
          {
              click: function (e, obj) {
                  var node = obj.part.adornedPart;
                  if (node !== null) {
                      var thisemp = node.data;
                      myDiagram.startTransaction("vacate");
                      // update the key, name, and comments
                      myDiagram.model.setKeyForNodeData(thisemp, getNextKey());
                      myDiagram.model.setDataProperty(thisemp, "name", "(Vacant)");
                      myDiagram.model.setDataProperty(thisemp, "comments", "");
                      myDiagram.commitTransaction("vacate");
                  }
              }
          }
        ),
        $("ContextMenuButton",
          $(go.TextBlock, "Remove Role"),
          {
              click: function (e, obj) {
                  // reparent the subtree to this node's boss, then remove the node
                  var node = obj.part.adornedPart;
                  if (node !== null) {
                      myDiagram.startTransaction("reparent remove");
                      var chl = node.findTreeChildrenNodes();
                      // iterate through the children and set their parent key to our selected node's parent key
                      while (chl.next()) {
                          var emp = chl.value;
                          myDiagram.model.setParentKeyForNodeData(emp.data, node.findTreeParentNode().data.key);
                      }
                      // and now remove the selected node itself
                      myDiagram.model.removeNodeData(node.data);
                      myDiagram.commitTransaction("reparent remove");
                  }
              }
          }
        ),
        $("ContextMenuButton",
          $(go.TextBlock, "Remove Department"),
          {
              click: function (e, obj) {
                  // remove the whole subtree, including the node itself
                  var node = obj.part.adornedPart;
                  if (node !== null) {
                      myDiagram.startTransaction("remove dept");
                      myDiagram.removeParts(node.findTreeParts());
                      myDiagram.commitTransaction("remove dept");
                  }
              }
          }
        )
      );

    // define the Link template
    myDiagram.linkTemplate =
      $(go.Link, go.Link.Orthogonal,
        { corner: 5, relinkableFrom: true, relinkableTo: true },
        $(go.Shape, { strokeWidth: 4, stroke: "#00a4a4" }));  // the link shape

    load(data);


    // support editing the properties of the selected person in HTML
    if (window.Inspector) myInspector = new Inspector("myInspector", myDiagram,
      {
          properties: {
              "key": { readOnly: true },
              "comments": {}
          }
      });
}

// Show the diagram's model in JSON format
function save() {
    document.getElementById("mySavedModel").value = myDiagram.model.toJson();
    myDiagram.isModified = false;
}
function load(data) {
    myDiagram.model = go.Model.fromJson(data);
}

$(document).ready(function () {
    var obj = {};

    obj.class = "go.TreeModel";

    $.ajax({
        url: '/sitecore/api/layout/render/jss?item=/&sc_lang=en&sc_apikey={1EEAEFC8-A093-492F-BAB6-00F55B1DAEF5}',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {

            obj.nodeDataArray = data.sitecore.context.contentTree;
            console.log(obj);
            init(obj);
        }
    });
});