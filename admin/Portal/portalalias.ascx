<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Admin.Portals.PortalAlias" CodeFile="PortalAlias.ascx.vb" %>
<asp:DataGrid BorderWidth="0px" Width="500" AutoGenerateColumns="false" ID="dgPortalAlias" Runat="server"
	EnableViewState="False" GridLines="None">
	<Columns>
		<asp:TemplateColumn>
			<ItemStyle Width="15px"></ItemStyle>
			<ItemTemplate>
				<asp:HyperLink NavigateUrl='<%# EditURL("paid",DataBinder.Eval(Container.DataItem,"PortalAliasID")) %>' runat="server" Visible='<% # IsNotCurrent(DataBinder.Eval(Container.DataItem,"PortalAliasID")) %>' ID="Hyperlink1">
					<asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" runat="server" ID="Hyperlink1Image"
						resourcekey="Edit" />
				</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn DataField="HTTPAlias" HeaderText="HTTP Alias">
			<HeaderStyle CssClass="NormalBold"></HeaderStyle>
			<ItemStyle CssClass="Normal"></ItemStyle>
		</asp:BoundColumn>
	</Columns>
</asp:DataGrid>
