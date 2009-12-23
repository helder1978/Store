<%@ Control language="c#" CodeBehind="ListItemTemplate.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ListItemTemplate" AutoEventWireup="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<itemtemplate>
	<TABLE id="ItemTable" cellSpacing="0" cellPadding="0" width="100%" border="0">
		<TR valign="top">
			<TD rowspan="4" style="PADDING-RIGHT: 5px; PADDING-LEFT: 5px; PADDING-BOTTOM: 5px; PADDING-TOP: 5px">
				<asp:Hyperlink id="linkImage" NavigateUrl="" runat="server">
					<asp:Image id="thumbImage" runat="server" ImageUrl=""></asp:Image>
				</asp:Hyperlink>
			</TD>
			<TD width="100%" nowrap>
				<asp:LinkButton id="linkEdit" CommandName="Edit" CommandArgument="" Visible="False" runat="server">
					<asp:Image ID="imageEdit" ImageUrl="~/images/edit.gif" AlternateText="Edit" Visible="False"
						resourcekey="Edit" runat="server" />
				</asp:LinkButton>
				<asp:HyperLink id="linkProductTitle" NavigateUrl="" CssClass="NormalBold" runat="server"></asp:HyperLink>
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="labelPrice" CssClass="Normal" runat="server"></asp:Label>
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:LinkButton id="linkAddToCart" CssClass="NormalBold" runat="server" resourcekey="linkAddToCart">Add To Cart</asp:LinkButton>
			</TD>
		</TR>
		<TR>
			<TD height="100%">
				&nbsp;
			</TD>
		</TR>
	</TABLE>
</itemtemplate>
