<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Admin.Portals.Portals" CodeFile="Portals.ascx.vb" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<asp:panel id=plLetterSearch Runat="server" HorizontalAlign="Center">
    <asp:Repeater id=rptLetterSearch Runat="server">
		<itemtemplate>
			<asp:HyperLink ID="HyperLink1" runat="server" CssClass="CommandButton" NavigateUrl='<%# FilterURL(Container.DataItem,"1") %>' Text='<%# Container.DataItem %>'>
			</asp:HyperLink>&nbsp;&nbsp;
		</ItemTemplate>
	</asp:Repeater>
</asp:panel>

<asp:DataGrid ID="grdPortals" runat="server" Width="100%" 
    AutoGenerateColumns="false" CellPadding="2" GridLines="None" cssclass="DataGrid_Container">
	<headerstyle cssclass="NormalBold" verticalalign="Top" horizontalalign="Center"/>
	<itemstyle cssclass="Normal" horizontalalign="Center" />
	<alternatingitemstyle cssclass="Normal" />
	<edititemstyle cssclass="NormalTextBox" />
	<selecteditemstyle cssclass="NormalRed" />
	<footerstyle cssclass="DataGrid_Footer" />
	<pagerstyle cssclass="DataGrid_Pager" />
    <Columns>
		<dnn:imagecommandcolumn CommandName="Edit" ImageUrl="~/images/edit.gif" EditMode="URL" KeyField="PortalID" />
		<dnn:imagecommandcolumn commandname="Delete" imageurl="~/images/delete.gif" keyfield="PortalID" />
        <asp:TemplateColumn HeaderText="Title">
            <ItemStyle HorizontalAlign="Left" />
            <ItemTemplate>
                <asp:Label ID="lblPortal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PortalName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Portal Aliases">
            <ItemStyle HorizontalAlign="Left" />
            <ItemTemplate>
                <asp:Label ID="lblPortalAliases" runat="server" Text='<%# FormatPortalAliases(Convert.toInt32(DataBinder.Eval(Container.DataItem, "PortalID"))) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateColumn>
        <dnn:textcolumn DataField="Users" HeaderText="Users" />
        <dnn:textcolumn DataField="Pages" HeaderText="Pages" />
        <dnn:textcolumn DataField="HostSpace" HeaderText="DiskSpace" />
        <asp:BoundColumn DataField="HostFee" HeaderText="HostingFee" DataFormatString="{0:0.00}" />
        <asp:TemplateColumn HeaderText="Expires">
            <HeaderStyle CssClass="NormalBold"></HeaderStyle>
            <ItemTemplate>
                <asp:Label runat="server" Text='<%#FormatExpiryDate(DataBinder.Eval(Container.DataItem, "ExpiryDate")) %>'
                    CssClass="Normal" ID="Label1" />
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<br><br>
<dnn:pagingcontrol id=ctlPagingControl runat="server"></dnn:pagingcontrol>
