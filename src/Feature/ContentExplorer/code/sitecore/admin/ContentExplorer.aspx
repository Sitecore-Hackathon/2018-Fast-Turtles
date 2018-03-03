<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentExplorer.aspx.cs" Inherits="ContentExplorer.sitecore.admin.ContentExplorer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Content Explorer</title>
    <meta name="description" content="An organization chart editor -- edit details and change relationships." />

    <!-- Copyright 1998-2018 by Northwoods Software Corporation. -->
    <meta charset="UTF-8">
    
    <link rel="stylesheet" href="/styles/ContentExplorer/main.css" />

    <script src="/sitecore/shell/Controls/Lib/jQuery/jquery-1.10.2.min.js"></script>
    <script src="/scripts/go/go.js"></script>
    <script src="/scripts/ContentExplorer/main.js"></script>


    <!-- Bootstrap begin-->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/handlebars.js/4.0.11/handlebars.min.js" type="text/javascript"></script>
    <!-- Bootstrap end-->

    <!-- Bootstrap end-->

    <script data-template="placeholdersBlock" type="text/x-handlebars-template">

        {{#each this}}
        <div class="panel panel-default "> <%--placeholder-container--%>
            <div class="panel-heading">
                <h3 class="panel-title">Placeholder key: <b>{{@key}}</b></h3>
            </div>
            <div class="panel-body">
                {{#each this}}
				        {{#if this.fields}}				
                            <ul>
                                {{#each this.fields}}						
                                    <li>{{@key}} : {{this.value}}</li>
                                {{/each}}				
                            </ul>
                        {{/if}}
                        {{#if this.placeholders}}				
                            {{> placeholdersBlock this.placeholders}}
                        {{/if}}                    
			        {{/each}}
            </div>
        </div>	
            
		{{/each}}
		
</script>
<script data-template="node-item" type="text/x-handlebars-template">
	<div class="row">
		<div class="col-sm-5 col-xs-12">
			<table class="table">
				<tr>
					<td>Display Name</td>
					<td>{{route.displayName}}</td>
				</tr>
				<tr>
					<td>Item ID</td>
					<td>{{route.itemId}}</td>
				</tr>
				<tr>
					<td>Name</td>
					<td>{{route.name}}</td>
				</tr>
			</table>
		</div>
		<div class="col-sm-7 col-xs-12">
                <div class="presentation-container">
                    {{> placeholdersBlock route.placeholders}}	
		</div>
	</div>
        </div>
</script>
    
    

</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="IdentifierLabel" runat="server" Text="Identifier: "></asp:Label>
        <asp:TextBox ID="IdentifierText" runat="server" Width="224px"></asp:TextBox>
        <asp:Button ID="SubmitButton" runat="server" Text="Switch to Contact" OnClick="SwitchToContact" />
<div id="sample">
    <div id="myDiagramDiv" style="background-color: #fff; border: solid 1px black; height: 800px"></div>
</div>

<!-- Modal -->
<div id="myModal" class="modal fade" role="dialog">
    <div class="modal-dialog" style="width: 1200px;">

        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">Node details</h4>
            </div>
            <div class="modal-body">
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>

    </div>
</div>
<!-- Modal end -->

    </form>

</body>
</html>
