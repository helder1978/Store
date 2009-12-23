<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="c#" CodeBehind="AccountSettings.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.AccountSettings" AutoEventWireup="True" %>
<table cellSpacing="5" width="80%" align="center" border="0">
	<tr vAlign="top">
		<td class="NormalBold" width="33%">
			<dnn:label id="lblRequireSSL" suffix=":" controlname="chkRequireSSL" runat="server"></dnn:label>
		</td>
		<td class="Normal" width="67%">
			<asp:CheckBox id="chkRequireSSL" runat="server"></asp:CheckBox>
		</td>
	</tr>
	<tr>
		<td class="NormalBold" width="33%">
			<dnn:label id="lblSSLNote" suffix=":" controlname="lblSSLNote" runat="server"></dnn:label>
		</td>
		<td class="Normal" width="67%">
			<asp:Label id="lblSSLMessage" runat="server" resourcekey="SSLMessage"></asp:Label>
		</td>
	</tr>
</table>
