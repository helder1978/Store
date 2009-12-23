<%@ Control Language="VB" AutoEventWireup="false" CodeFile="nsolutions4uMenu.ascx.vb" Inherits="DesktopModules_nsolutions4uMenu_nsolutions4uMenu" %>
<%@ Register Assembly="DotNetNuke.WebControls" Namespace="DotNetNuke.UI.WebControls"
    TagPrefix="DNN" %>

<script type="text/javascript">
   
    function setupMenuIds(id, prefix)
    {
       // alert(id);
       //var menu = dnn.controls.controls[id + '_ctldnnNAV'];
       var menu = dnn.controls.controls[id];
       // alert(dnn.controls);
       // alert(menu);
        
       assignMenuIds(menu, menu.rootNode, prefix, '');
    }

    function assignMenuIds(menu, parent, prefix, id)
    {
	    var menuNode = new dnn.controls.DNNMenuNode(parent);
	    var menuCtr = menu.getChildControl(menuNode.id, 'ctr');
	    var newId = prefix + '_' + id;
        //alert(newId);
	    if (menuCtr)
		    menuCtr.id = newId;

        for (var i=0; i<parent.childNodeCount(); i++)
        {
	        assignMenuIds(menu, parent.childNodes(i), newId, i);
        }
    }
</script>

<DNN:DNNMenu ID="MyDNNMenu" runat="server" ChildArrowImage="../../../Images/Fwd.gif" Orientation="Horizontal">
</DNN:DNNMenu>

<script runat="server">
Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        DotNetNuke.UI.Utilities.DNNClientAPI.AddBodyOnloadEventHandler(Me.Page, "setupMenuIds('" & MyDNNMenu.ClientID & "', 'jcompany')")
End Sub
</script>
