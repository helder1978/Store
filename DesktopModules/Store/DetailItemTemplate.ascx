<%@ Control language="c#" CodeBehind="DetailItemTemplate.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.DetailItemTemplate" AutoEventWireup="True" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<itemtemplate>
	<TABLE id="ItemTable" cellSpacing="0" cellPadding="0" width="100%" border="0">
		<TR valign="top">
			<TD rowspan="5" style="PADDING-RIGHT: 5px; PADDING-LEFT: 5px; PADDING-BOTTOM: 5px; PADDING-TOP: 5px">
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
				<asp:Label id="labelSummary" CssClass="Normal" runat="server"></asp:Label>
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:Label id="labelPrice" CssClass="Normal" runat="server"></asp:Label>
			</TD>
		</TR>
		<TR>
			<TD>
				<asp:LinkButton id="linkAddToCart" CssClass="NormalBold" runat="server">Add To Cart</asp:LinkButton>
			</TD>
		</TR>
		<TR>
			<TD height="100%">
				&nbsp;
			</TD>
		</TR>
		<TR>
			<TD colspan="2">
				<asp:Literal id="literalDescription" runat="server"></asp:Literal>
			</TD>
		</TR>
	</TABLE>
</itemtemplate>
