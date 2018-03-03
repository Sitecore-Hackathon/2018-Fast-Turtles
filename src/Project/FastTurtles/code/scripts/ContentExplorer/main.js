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

    var levelColors = ["#f0f0f0", "#e9e9e9", "#e0e0e0", "#d8d8d8", "#d4d4d4", "#d6d6d6", "#d1d1d1", "#cccccc", "#cccccc", "#cccccc", "#cccccc", "#cccccc", "#cccccc", "#cccccc", "#cccccc"];

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
                if (shape) shape.fill = $(go.Brush, "Linear", { 0.0: color, 1.0: go.Brush.lightenBy(color, 0.05), start: go.Spot.Left, end: go.Spot.Right });
            }
        });
    };

    myDiagram.toolManager.panningTool.isEnabled = false;

    // This function provides a common style for most of the TextBlocks.
    // Some of these values may be overridden in a particular TextBlock.
    function textStyle() {
        return { font: "8pt  Segoe UI,sans-serif", stroke: "black" };
    }

    // This converter is used by the Picture.
    function findHeadShot(icon) {
        return icon;
    }

    function nodeClick(e, obj) {
        var source,
            template;
        showLoading();
        jQuery.ajax({
            url: '/sitecore/api/layout/render/jss?item=' + (obj.data.path ? obj.data.path : '/') + '&sc_lang=en&sc_apikey={1EEAEFC8-A093-492F-BAB6-00F55B1DAEF5}&tracking=true&xdbcontactid=qwerty4@gmail.com',
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
                hideLoading();
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
            $(go.Shape, "RoundedRectangle",
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
                        margin: new go.Margin(6, 0, 6, 3),
                    },
                    new go.Binding("source", "icon", findHeadShot)),
                // define the panel where the text will appear
                $(go.Panel, "Table",
                    {
                        maxSize: new go.Size(180, 999),
                        margin: new go.Margin(6, 10, 0, 3),
                        defaultAlignment: go.Spot.Left
                    },
                    $(go.RowColumnDefinition, { column: 2, width: 4 }),
                    $(go.TextBlock, textStyle(),  // the name
                        {
                            row: 0, column: 0, columnSpan: 5,
                            font: "11pt Segoe UI,sans-serif",
                            editable: false, isMultiline: false,
                            minSize: new go.Size(10, 16)
                        },
                        new go.Binding("text", "title").makeTwoWay()),
                    $(go.TextBlock, "Workflow: ", textStyle(),
                        { row: 1, column: 0 }),
                    $(go.TextBlock, textStyle(),
                        {
                            row: 1, column: 1, columnSpan: 4,
                            editable: false,
                            isMultiline: false,
                            minSize: new go.Size(10, 14),
                            margin: new go.Margin(0, 0, 0, 3)
                        },
                        new go.Binding("text", "workflow").makeTwoWay()), 
                    $(go.TextBlock, "Template: ", textStyle(),
                        { row: 2, column: 0 }),
                    $(go.TextBlock, textStyle(),
                        {
                            row: 2, column: 1, columnSpan: 4,
                            editable: false,
                            isMultiline: false,
                            minSize: new go.Size(10, 14),
                            margin: new go.Margin(0, 0, 0, 3)
                        },
                        new go.Binding("text", "template").makeTwoWay())
                )  // end Table Panel
            ) // end Horizontal Panel
        );  // end Node

    myDiagram.linkTemplate =
        $(go.Link, go.Link.Orthogonal,
            { corner: 5, relinkableFrom: false, relinkableTo: false },
            $(go.Shape, { strokeWidth: 2, stroke: "#000" }));  // the link shape

    load(data);

}

function showLoading() {
    $('.disable-page').show();
    $('.loader-indicator').show();
}
function hideLoading() {
    $('.disable-page').hide();
    $('.loader-indicator').hide();
}

// Show the diagram's model in JSON format
function load(data) {
    myDiagram.model = go.Model.fromJson(data);
}

$(document).ready(function () {
    var obj = {};

    obj.class = "go.TreeModel";
    showLoading();
    $.ajax({
        url: '/sitecore/api/layout/render/jss?item=/&sc_lang=en&sc_apikey={1EEAEFC8-A093-492F-BAB6-00F55B1DAEF5}',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            obj.nodeDataArray = data.sitecore.context.contentTree;
            console.log(obj);
            init(obj);
            hideLoading();
        }
    });

    
});