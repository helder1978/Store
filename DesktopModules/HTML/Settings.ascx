<%@ Control Inherits="DotNetNuke.Modules.HTML.Settings" CodeFile="Settings.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="4" border="0" width=100%>
	<tr>
		<td class="SubHead"><dnn:label id="plReplaceTokens" controlname="rblstReplaceTokens" runat="server" /></td>
		<td valign="top">
			<asp:RadioButtonList id="rblstReplaceTokens" CssClass="NormalTextBox" runat="server" repeatdirection="Vertical">
				<asp:ListItem resourcekey="noReplace" Value="0" Selected="true" />
				<asp:ListItem resourcekey="oldReplace" Value="1" />
				<asp:ListItem resourcekey="EnhancedReplace" Value="2" />
			</asp:RadioButtonList>
		</td>
	</tr>
</table>

