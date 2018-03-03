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

    <script data-template="placeholdersBlock" type="text/x-handlebars-template">

        {{#each this}}
        <div class="panel panel-success mb_10"> <%--placeholder-container--%>
            <div class="panel-heading">
                <h3 class="panel-title">Placeholder key: <b>{{@key}}</b></h3>
            </div>
            <div class="panel-body">
                {{#each this}}                
                        {{#if this.componentName}}
                            <div class="panel panel-info "> <%--placeholder-container--%>
                                <div class="panel-heading">
                                    <h3 class="panel-title">Component Name: <b>{{this.componentName}}</b></b></h3>
                                </div>
                                <div class="panel-body">
                                    {{#if this.fields}}				
                                        <ul>
                                            {{#each this.fields}}	
                                                {{#if this.value.src}}	
                                                <li>{{@key}} : <img src="{{{this.value.src}}}" alt="{{this.value.alt}}" style="width: 100%;" />></li>	
                                                {{else}}
                                                <li>{{@key}} : {{{this.value}}}</li>
                                                {{/if}}			
                                                
                                            {{/each}}				
                                        </ul>
                                        <hr />
                                    {{else}}
                                        <ul>
                                            <li>Component doesn't have any datasource fields</li>				
                                        </ul>
                                        <hr />
                                    {{/if}}
                                    {{#if this.placeholders}}				
                                        {{> placeholdersBlock this.placeholders}}
                                    {{/if}}  
                                </div>
                            </div>		
                            
				            
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
                        <td>Item ID</td>
                        <td>{{currentItem.itemId}}</td>
                    </tr>
                    <tr>
                        <td>Name</td>
                        <td>{{currentItem.name}}</td>
                    </tr>
                    <tr>
                        <td>Display Name</td>
                        <td>{{currentItem.displayName}}</td>
                    </tr>
                    <tr>
                        <td>Item path</td>
                        <td>{{currentItem.path}}</td>
                    </tr>
                    <tr>
                        <td>Template</td>
                        <td>{{currentItem.templateName}}</td>
                    </tr>
                    <tr>
                        <td>Created By</td>
                        <td>{{currentItem.createdBy}}</td>
                    </tr>
                    <tr>
                        <td>Updated By</td>
                        <td>{{currentItem.updatedBy}}</td>
                    </tr>
                    <tr>
                        <td>Count Of Versions</td>
                        <td>{{currentItem.countOfVersions}}</td>
                    </tr>
                    <tr>
                        <td>Current Version</td>
                        <td>{{currentItem.currentVersion}}</td>
                    </tr>
                    <tr>
                        <td>IsPublished</td>
                        <td>{{currentItem.isPublished}}</td>
                    </tr>
                    <tr>
                        <td>Workflow State</td>
                        <td>{{currentItem.workflowState}}</td>
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
<div class="disable-page" style="display: none"></div>
<div class="loader-indicator" style="display: none"></div>
    <form id="form1" runat="server">
        <asp:Label ID="IdentifierLabel" runat="server" Text="Identifier: "/>
        <asp:TextBox ID="IdentifierText" runat="server" Width="224px" TextMode="Email"/>
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
