<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DemoControl.ascx.vb" Inherits="DesktopModules_DemoApp_DemoControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel ID="pnlAjaxUpdate" runat="server">
    Current Time:<asp:Label ID="lblCurrentTime" runat="server"></asp:Label><br /><br />
    <asp:Button ID="btnAjaxTimeUpdate" runat="server" text="Update (AJAX)" /> &nbsp; 
    <asp:Button ID="btnPostbackTimeUpdate" runat="server" Text="Update (Postback" />
</asp:Panel>



<h4>Collapsible Panel Demonstration</h4>
<cc1:CollapsiblePanelExtender 
    ID="panelExtender" 
    runat="server" 
    TargetControlID="pnlCollapse" 
    CollapsedText="Click to Expand" 
    ExpandedText="Click to Collapse"  
    TextLabelID="lblAction"
    CollapseControlID="pnlHeader" 
    ExpandControlID="pnlHeader"/>

<!-- This is the panel used as the "Action Header" to handle clicks -->
<asp:panel ID="pnlHeader" runat="server">
    <asp:Label ID="lblAction" runat="server"></asp:Label>
</asp:panel>

<!-- This is the actual content we will be collapsing -->
<asp:Panel ID="pnlCollapse" runat="server">
    <p>The content inside this panel can be expanded or collapsed by using the ASP.NET AJAX Control Toolkit's
    CollapsiblePanelExtender.</p>
    
    <p>Additional text can be included as well as ANY other desired HTML or ASP.NET server side controls</p>
    
    <p>This is the last paragraph of example text</p>
</asp:Panel>
