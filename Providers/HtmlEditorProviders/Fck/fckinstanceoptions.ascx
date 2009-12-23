<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Sectionhead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="fckinstanceoptions.ascx.vb" Inherits="DotNetNuke.HtmlEditor.FckHtmlEditorProvider.fckinstanceoptions" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<TABLE id="tblEditorOptions" cellSpacing="0" cellPadding="0" width="660" align="center"
	border="0" runat="server" visible="true">
	<TR>
		<TD class="SubHead" vAlign="top" colSpan="2">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD>
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD><asp:label id="lblModuleType" runat="server" resourcekey="lblModuleType" CssClass="subhead"></asp:label>&nbsp;<asp:label id="txtModuleType" runat="server" CssClass="normal"></asp:label></TD>
								<TD align="right"><asp:label id="txtUsername" runat="server" CssClass="normal"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD><asp:label id="lblModuleName" runat="server" resourcekey="lblModuleName" CssClass="subhead"></asp:label>&nbsp;<asp:label id="txtModuleName" runat="server" CssClass="normal"></asp:label></TD>
				</TR>
				<TR>
					<TD><asp:label id="lblModuleInstance" runat="server" resourcekey="lblModuleInstance" CssClass="subhead"></asp:label>&nbsp;<asp:label id="txtModuleInstance" runat="server" CssClass="normal"></asp:label></TD>
				</TR>
			</TABLE>
			<HR width="100%" SIZE="1">
		</TD>
	</TR>
	<TR>
		<TD class="SubHead" vAlign="top" width="239"><dnn:label id="plSettingsType" runat="server" suffix=":" controlname="rbSettingsType"></dnn:label></TD>
		<TD class="Normal" vAlign="top"><asp:radiobuttonlist id="rbSettingsType" runat="server" CssClass="Normal" AutoPostBack="True" RepeatDirection="Horizontal">
				<asp:ListItem Value="I" resourcekey="typeInstance.Text" Selected="True">Instance</asp:ListItem>
				<asp:ListItem Value="M" resourcekey="typeModule.Text">Module</asp:ListItem>
				<asp:ListItem Value="P" resourcekey="typePortal.Text">Portal</asp:ListItem>
			</asp:radiobuttonlist></TD>
	</TR>
	<TR>
		<TD width="100%" colSpan="2" height="100%">
			<div id="optionsarea" style="BORDER-RIGHT: #000000 1px dashed; PADDING-RIGHT: 10px; BORDER-TOP: #000000 1px dashed; PADDING-LEFT: 10px; PADDING-BOTTOM: 10px; OVERFLOW: auto; BORDER-LEFT: #000000 1px dashed; WIDTH: 100%; COLOR: #333333; PADDING-TOP: 10px; BORDER-BOTTOM: #000000 1px dashed; HEIGHT: 360px; BACKGROUND-COLOR: #ffffff">
				<table cellSpacing="0" cellPadding="0" border="0">
					<tr>
						<td><dnn:sectionhead id="dshThemes" runat="server" resourcekey="dshThemes" cssclass="Head" text="Editor skins"
								includerule="true" section="tblThemes"></dnn:sectionhead>
							<TABLE id="tblThemes" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
								<TR>
									<TD class="SubHead" width="230"><dnn:label id="plToolbarSkin" runat="server" suffix=":" controlname="ddlToolbarSkin"></dnn:label></TD>
									<TD class="Normal"><asp:dropdownlist id="ddlToolbarSkin" runat="server" CssClass="NormalTextBox" Width="296px"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="230"><dnn:label id="plImageBrowserTheme" runat="server" suffix=":" controlname="ddlImageBrowserTheme"></dnn:label></TD>
									<TD class="Normal"><asp:dropdownlist id="ddlImageBrowserTheme" runat="server" CssClass="NormalTextBox" Width="296px"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="230"><dnn:label id="plFlashBrowserTheme" runat="server" suffix=":" controlname="ddlFlashBrowserTheme"></dnn:label></TD>
									<TD class="Normal"><asp:dropdownlist id="ddlFlashBrowserTheme" runat="server" CssClass="NormalTextBox" Width="296px"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="230"><dnn:label id="plLinkBrowserTheme" runat="server" suffix=":" controlname="ddlLinkBrowserTheme"></dnn:label></TD>
									<TD class="Normal"><asp:dropdownlist id="ddlLinkBrowserTheme" runat="server" CssClass="NormalTextBox" Width="296px"></asp:dropdownlist></TD>
								</TR>
							</TABLE>
						</td>
					</tr>
					<TR>
						<TD><dnn:sectionhead id="dshAvailableStyles" runat="server" resourcekey="dshAvailableStyles" cssclass="Head"
								text="List of Available editor styles" includerule="true" section="tblAvailableStyles" isexpanded="false"></dnn:sectionhead>
							<TABLE id="tblAvailableStyles" cellSpacing="0" cellPadding="0" width="100%" border="0"
								runat="server">
								<TR>
									<TD class="SubHead" width="224"><dnn:label id="plStyleMode" runat="server" suffix=":" controlname="txtStyleFilter"></dnn:label></TD>
									<TD class="Normal"><asp:radiobuttonlist id="rbStyleMode" runat="server" CssClass="Normal" RepeatDirection="Horizontal" Width="301px">
											<asp:ListItem Value="static" Selected="True">Static</asp:ListItem>
											<asp:ListItem Value="dynamic">Dynamic</asp:ListItem>
											<asp:ListItem Value="url">URL</asp:ListItem>
										</asp:radiobuttonlist></TD>
								</TR>
								<TR>
									<TD class="SubHead" vAlign="top" width="224"><dnn:label id="plStyleFilter" runat="server" suffix=":" controlname="txtStyleFilter"></dnn:label></TD>
									<TD class="Normal" vAlign="top"><asp:textbox id="txtStyleFilter" runat="server" CssClass="NormalTextBox" Width="400px" TextMode="MultiLine"
											Height="56px"></asp:textbox><BR>
										<asp:linkbutton id="cmdCopyFilter" runat="server" CssClass="CommandButton" CausesValidation="False">Copy host default filter</asp:linkbutton></TD>
								</TR>
								<TR>
									<TD class="SubHead" vAlign="top" width="224"><dnn:label id="plPortalStyle" runat="server" suffix=":" controlname="ctlURL"></dnn:label></TD>
									<TD class="Normal" vAlign="top"><portal:url id="ctlURL" runat="server" width="250" ShowUrls="true" ShowTabs="False" ShowLog="False"
											ShowTrack="False" Required="False"></portal:url></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD><dnn:sectionhead id="dshEditorAreaCss" runat="server" resourcekey="dshEditorAreaCss" cssclass="Head"
								text="Editor Area Css File" includerule="true" section="tblEditorAreaCss" isexpanded="false"></dnn:sectionhead>
							<TABLE id="tblEditorAreaCss" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
								<TR>
									<TD class="SubHead" vAlign="top" width="224"><dnn:label id="plCssMode" runat="server" suffix=":" controlname="txtStyleFilter"></dnn:label></TD>
									<TD class="Normal" vAlign="top"><asp:radiobuttonlist id="rbCssMode" runat="server" CssClass="Normal" RepeatDirection="Horizontal" Width="301px">
											<asp:ListItem Value="static" Selected="True">Static</asp:ListItem>
											<asp:ListItem Value="dynamic">Dynamic</asp:ListItem>
											<asp:ListItem Value="url">URL</asp:ListItem>
										</asp:radiobuttonlist></TD>
								</TR>
								<TR>
									<TD class="SubHead" vAlign="top" width="224"><dnn:label id="plPortalCss" runat="server" suffix=":" controlname="txtStyleFilter"></dnn:label></TD>
									<TD class="Normal" vAlign="top"><portal:url id="ctlUrlCss" runat="server" width="250" ShowUrls="true" ShowTabs="False" ShowLog="False"
											ShowTrack="False" Required="False"></portal:url></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD><dnn:sectionhead id="dshOther" runat="server" resourcekey="dshOther" cssclass="Head" text="Other editor options"
								includerule="true" section="tblOther" isexpanded="false"></dnn:sectionhead>
							<TABLE id="tblOther" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plToolbarNotExpanded" runat="server" suffix=":" controlname="chkToolbarNotExpanded"></dnn:label></TD>
									<TD><asp:checkbox id="chkToolbarNotExpanded" runat="server" CssClass="NormalTextBox"></asp:checkbox></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plEnhancedSecurity" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:checkbox id="chkEnhancedSecurity" runat="server" CssClass="NormalTextBox"></asp:checkbox></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plFullImagePath" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:checkbox id="chkFullImagePath" runat="server" CssClass="NormalTextBox"></asp:checkbox></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plForceWidth" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:textbox id="txtForceWidth" runat="server" CssClass="NormalTextBox" Width="136px"></asp:textbox></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plForceHeight" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:textbox id="txtForceHeight" runat="server" CssClass="NormalTextBox" Width="136px"></asp:textbox></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plImageFolder" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:dropdownlist id="ddlImageFolder" runat="server" CssClass="NormalTextBox" Width="416px"></asp:dropdownlist></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plFontColors" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:textbox id="txtFontColors" runat="server" CssClass="NormalTextBox" Width="416px"></asp:textbox></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plFontNames" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:textbox id="txtFontNames" runat="server" CssClass="NormalTextBox" Width="416px"></asp:textbox></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plFontSizes" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:textbox id="txtFontSizes" runat="server" CssClass="NormalTextBox" Width="416px"></asp:textbox></TD>
								</TR>
								<TR>
									<TD class="SubHead" width="225"><dnn:label id="plFontFormats" runat="server" suffix=":" controlname="chkEnhancedSecurity"></dnn:label></TD>
									<TD class="Normal"><asp:textbox id="txtFontFormats" runat="server" CssClass="NormalTextBox" Width="416px"></asp:textbox></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR>
						<TD><dnn:sectionhead id="dshToolbarRoles" runat="server" resourcekey="dshToolbarRoles" cssclass="Head"
								text="Toolbar Roles" includerule="true" section="tblToolbarset"></dnn:sectionhead>
							<TABLE id="tblToolbarset" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
								<TR>
									<TD class="SubHead" vAlign="top" width="100%"><dnn:label id="plToolbarSet" runat="server" suffix=":" controlname="lstToolbars"></dnn:label><asp:datagrid id="grdToolbars" runat="server" CssClass="Normal" Width="620px" AutoGenerateColumns="False">
											<HeaderStyle CssClass="SubHead"></HeaderStyle>
											<Columns>
												<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit">
													<HeaderStyle Width="5px"></HeaderStyle>
													<ItemStyle VerticalAlign="Top"></ItemStyle>
												</asp:EditCommandColumn>
												<asp:TemplateColumn>
													<HeaderStyle Width="5px"></HeaderStyle>
													<ItemStyle VerticalAlign="Top"></ItemStyle>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Toolbar">
													<HeaderStyle Width="180px"></HeaderStyle>
													<ItemStyle VerticalAlign="Top"></ItemStyle>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Disabled">
													<HeaderStyle Width="5px"></HeaderStyle>
													<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="ViewRoles">
													<HeaderStyle Width="100%"></HeaderStyle>
													<ItemStyle VerticalAlign="Top"></ItemStyle>
												</asp:TemplateColumn>
											</Columns>
										</asp:datagrid><asp:linkbutton id="cmdMakeAllUsers" runat="server" resourcekey="cmdMakeAllUsers" CssClass="CommandButton"
											CausesValidation="False">Include all users to each toolbar</asp:linkbutton></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</table>
			</div>
		</TD>
	</TR>
	<TR>
		<TD vAlign="top" colSpan="2">
			<HR width="100%" SIZE="1">
			<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD class="SubHead"><dnn:label id="plApplyTo" runat="server" suffix=":" controlname="ddlSettingsType"></dnn:label></TD>
					<TD class="Normal"><asp:dropdownlist id="ddlSettingsType" runat="server" CssClass="NormalTextBox">
							<asp:ListItem Value="I" resourcekey="typeInstance.Text" Selected="True">Instance</asp:ListItem>
							<asp:ListItem Value="M" resourcekey="typeModule.Text">Module</asp:ListItem>
							<asp:ListItem Value="P" resourcekey="typePortal.Text">Portal</asp:ListItem>
						</asp:dropdownlist>&nbsp;&nbsp;
						<asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" CssClass="CommandButton" CausesValidation="False">Update</asp:linkbutton>&nbsp;
						<asp:linkbutton id="cmdClear" runat="server" resourcekey="cmdClear" CssClass="CommandButton" CausesValidation="False">Clear</asp:linkbutton></TD>
				</TR>
				<TR>
					<TD class="SubHead" colSpan="2"><asp:label id="lblResult" runat="server" CssClass="NormalRed"></asp:label></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
</TABLE>
