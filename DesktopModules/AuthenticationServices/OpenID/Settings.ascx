<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="DotNetNuke.Authentication.OpenID.Settings, DotNetNuke.Authentication.OpenID" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<dnn:propertyeditorcontrol id="SettingsEditor" runat="Server" 
    editcontrolwidth="200px" 
    labelwidth="250px" 
    width="450px" 
    editcontrolstyle-cssclass="NormalTextBox" 
    helpstyle-cssclass="Help" 
    labelstyle-cssclass="SubHead" 
    editmode="Edit"
    SortMode="SortOrderAttribute"
    />
<asp:Panel ID="pnlFullTrustRequired" runat="server" Visible="false">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/red-error.gif" ImageAlign="Left"  /><asp:Label ID="lblFullTrustRequired" runat="server" CssClass="Normal" resourcekey="FullTrustRequired" />
</asp:Panel>
