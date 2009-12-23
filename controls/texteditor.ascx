<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.UserControls.TextEditor" %>
<table cellSpacing="2" cellPadding="2" summary="Edit HTML Design Table" border="0" id="tblTextEditor" Runat="server">
	<tr vAlign="top">
		<td align="center">
		    <asp:panel id="pnlOption" Visible="True" Runat="server">
				<asp:RadioButtonList id="optView" Runat="server" AutoPostBack="True" RepeatDirection="Horizontal" CssClass="NormalTextBox"></asp:RadioButtonList>
			</asp:panel>
		</td>
	</tr>
	<tr vAlign="top">
		<td id="celTextEditor" Runat="Server">
		    <asp:panel id="pnlBasicTextBox" Visible="False" Runat="server" Width="100%">
				<asp:TextBox id="txtDesktopHTML" CssClass="NormalTextBox" runat="server" textmode="multiline" rows="12" width="100%" columns="75"></asp:TextBox>
				<br/>
				<asp:Panel id="pnlBasicRender" Runat="server" Visible="True">
					<asp:RadioButtonList id="optRender" Runat="server" AutoPostBack="True" RepeatDirection="Horizontal" CssClass="NormalTextBox"></asp:RadioButtonList>
				</asp:Panel>
			</asp:panel><asp:panel id="pnlRichTextBox" Visible="False" Runat="server">
				<asp:PlaceHolder id="plcEditor" runat="server"></asp:PlaceHolder>
			</asp:panel>
		</td>
	</tr>
</table>
