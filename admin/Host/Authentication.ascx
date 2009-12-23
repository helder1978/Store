<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Authentication.ascx.vb" Inherits="DotNetNuke.Modules.Admin.Host.Authentication" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<asp:DataGrid ID="grdAuthentication" AutoGenerateColumns="false" width="475px" 
    CellPadding="2" GridLines="None" cssclass="DataGrid_Container" Runat="server">
    <headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="Center"/>
    <itemstyle cssclass="Normal" horizontalalign="Left" />
    <alternatingitemstyle cssclass="Normal" />
    <edititemstyle cssclass="NormalTextBox" />
    <selecteditemstyle cssclass="NormalRed" />
    <footerstyle cssclass="DataGrid_Footer" />
    <Columns>
		<dnn:imagecommandcolumn commandname="Delete" imageurl="~/images/delete.gif" keyfield="PackageID" />
		<dnn:textcolumn DataField="AuthenticationType" HeaderText="Type" />
		<dnn:checkboxcolumn DataField="IsEnabled" HeaderText="Enabled" HeaderCheckBox="false" AutoPostBack="True" />
		<dnn:textcolumn DataField="SettingsControlSrc" HeaderText="SettingsControl" />
		<dnn:textcolumn DataField="LoginControlSrc" HeaderText="LoginControl" />
    </Columns>
</asp:DataGrid>
<br />
<dnn:CommandButton ID="cmdAdd" runat="server" ResourceKey="cmdAdd" ImageUrl="~/images/add.gif" />
