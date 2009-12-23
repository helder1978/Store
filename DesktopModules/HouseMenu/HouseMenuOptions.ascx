<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="HouseMenuOptions.ascx.vb" Inherits="TimRolands.DNN.Modules.HouseMenu.HouseMenuOptions" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<p>
	<table id="Table1" cellspacing="0" cellpadding="5" width="300" border="0">
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right"><dnn:label id="plScope" runat="server" suffix="" controlname="radTabs"></dnn:label></td>
			<td class="Normal" valign="baseline" nowrap><asp:radiobuttonlist id="radTabs" autopostback="True" repeatlayout="Flow" runat="server">
					<asp:listitem value="-1">All Tabs/Pages</asp:listitem>
					<asp:listitem value="0">Current Tab/Page Children</asp:listitem>
					<asp:listitem value="1">Selected Tab/Page Children:</asp:listitem>
				</asp:radiobuttonlist>&nbsp;
				<asp:dropdownlist id="cboTabs" runat="server" enabled="False"></asp:dropdownlist><asp:requiredfieldvalidator id="reqTabsDropdown" runat="server" enabled="False" controltovalidate="cboTabs"
					errormessage="Valid tab/page selection required"></asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right">
				<dnn:label id="plShow" runat="server" suffix="" controlname="chkShowHome"></dnn:label></td>
			<td class="Normal" valign="baseline" nowrap>
				<p>
					<asp:checkbox id="chkShowHome" runat="server" autopostback="True"></asp:checkbox>&nbsp;<asp:checkbox id="chkShowParent" runat="server"></asp:checkbox>&nbsp;
					<asp:checkbox id="chkShowHidden" runat="server" autopostback="True"></asp:checkbox>&nbsp;
					<asp:checkbox id="chkShowAdmin" runat="server" enabled="False"></asp:checkbox>&nbsp;
					<br>
					<asp:checkbox id="chkShowSearchResults" runat="server" enabled="False"></asp:checkbox>&nbsp;
					<asp:textbox id="txtSearchResultsName" runat="server" enabled="False">Search Results</asp:textbox><br>
					<asp:checkbox id="chkShowPageIcons" runat="server"></asp:checkbox>&nbsp;</p>
			</td>
		</tr>
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right"></td>
			<td class="Normal" valign="baseline">
				<asp:checkbox id="chkHidePageNames" runat="server"></asp:checkbox></td>
		</tr>
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right"><dnn:label id="plRecursive" runat="server" suffix="" controlname="chkIsRecursive"></dnn:label></td>
			<td class="Normal" valign="baseline"><asp:checkbox id="chkIsRecursive" runat="server"></asp:checkbox></td>
		</tr>
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right"><dnn:label id="plOrientation" runat="server" suffix="" controlname="radOrientation"></dnn:label></td>
			<td class="Normal" valign="baseline"><asp:radiobuttonlist id="radOrientation" repeatlayout="Flow" runat="server" repeatdirection="Horizontal">
					<asp:listitem value="V" selected="True">V</asp:listitem>
					<asp:listitem value="H">H</asp:listitem>
				</asp:radiobuttonlist></td>
		</tr>
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right"><dnn:label id="plMode" runat="server" suffix="" controlname="radMode"></dnn:label></td>
			<td class="Normal" valign="baseline">
				<asp:radiobuttonlist id="radMode" runat="server" repeatlayout="Flow" repeatdirection="Horizontal">
					<asp:listitem value="S" selected="True">Static</asp:listitem>
					<asp:listitem value="D">Dynamic</asp:listitem>
				</asp:radiobuttonlist></td>
		</tr>
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right"><dnn:label id="plCssElementId" runat="server" suffix="" controlname="txtCssElementId"></dnn:label></td>
			<td class="Normal" valign="baseline">
				<asp:textbox id="txtCssElementId" runat="server" cssclass="NormalTextbox"></asp:textbox></td>
		</tr>
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right">
				<dnn:label id="plStyleName" controlname="txtCssElementId" suffix="" runat="server"></dnn:label></td>
			<td class="Normal" valign="baseline">
				<table id="DefaultStylesTable" cellspacing="0" cellpadding="0" align="right" border="0"
					style="FLOAT:right">
					<tr>
						<td class="SubHead" nowrap>
							<dnn:label id="plDefaultStyles" controlname="txtCssElementId" suffix="" runat="server"></dnn:label></td>
					</tr>
					<tr>
						<td class="Normal" nowrap style="padding-right:16px;padding-left:16px;">
							Vertical: "ModuleV"<br>
							Horizontal: "ModuleH"<br>
							Vertical (Static): "ModuleVstatic"</td>
					</tr>
				</table>
				<asp:dropdownlist id="ddlStyleNames" runat="server"></asp:dropdownlist>
			</td>
		</tr>
		<tr>
			<td class="SubHead" valign="baseline" nowrap align="right">&nbsp;</td>
			<td class="Normal" valign="baseline">&nbsp;</td>
		</tr>
	</table>
</p>
