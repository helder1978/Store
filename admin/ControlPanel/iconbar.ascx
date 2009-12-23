<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.ControlPanels.IconBar" CodeFile="IconBar.ascx.vb" %>
<table class="ControlPanel" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td>
			<table cellspacing="1" cellpadding="1" width="100%">
				<tr>
					<td align="left" valign="middle" width="25%" nowrap>
					    &nbsp;<asp:Label ID="lblMode" Runat="server" CssClass="SubHead" resourcekey="Mode" enableviewstate="False">Mode:</asp:Label>
						<asp:radiobuttonlist id="optMode" cssclass="SubHead" runat="server" repeatdirection="Horizontal" repeatlayout="Flow" autopostback="True">
							<asp:listitem value="VIEW" resourcekey="ModeView">View</asp:listitem>
							<asp:listitem value="EDIT" resourcekey="ModeEdit">Edit</asp:listitem>
						</asp:radiobuttonlist>
					</td>
					<td align="center" valign="middle" width="50%"><asp:HyperLink ID="hypUpgrade" runat="server" Target="_new" Visible="False" /></td>
					<td align="right" valign="middle" width="25%" nowrap>
					    <asp:Label ID="lblVisibility" Runat="server" CssClass="SubHead" resourcekey="Visibility" />
                        <asp:LinkButton ID="cmdVisibility" Runat="server" CausesValidation="False"><asp:Image ID="imgVisibility" Runat="server"></asp:Image></asp:LinkButton>&nbsp;
					</td>
				</tr>
				<tr id="rowControlPanel" runat="server">
					<td align="center" valign="middle" style="border-top:1px #CCCCCC dotted;">
                        <asp:Label ID="lblPageFunctions" Runat="server" CssClass="SubHead" enableviewstate="False"><font size="1">Page Functions</font></asp:Label>
						<table cellspacing="0" cellpadding="2" border="0">
							<tr valign="bottom" height="24">
								<td width="35" align="center"><asp:LinkButton ID="cmdAddTabIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgAddTabIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_addtab.gif"></asp:Image>
									</asp:LinkButton></td>
								<td width="35" align="center"><asp:LinkButton ID="cmdEditTabIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgEditTabIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_edittab.gif"></asp:Image>
									</asp:LinkButton></td>
								<td width="35" align="center"><asp:LinkButton ID="cmdDeleteTabIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgDeleteTabIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_deletetab.gif"></asp:Image>
									</asp:LinkButton></td>
								<td width="35" align="center"><asp:LinkButton ID="cmdCopyTabIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgCopyTabIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_copytab.gif"></asp:Image>
									</asp:LinkButton></td>
								<td width="35" align="center"><asp:LinkButton ID="cmdDesignTabIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgDesignTabIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_designtab.gif"></asp:Image>
									</asp:LinkButton></td>
							</tr>
							<tr valign="bottom">
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdAddTab" Runat="server" CssClass="CommandButton" CausesValidation="False">Add</asp:LinkButton></td>
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdEditTab" Runat="server" CssClass="CommandButton" CausesValidation="False">Settings</asp:LinkButton></td>
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdDeleteTab" Runat="server" CssClass="CommandButton" CausesValidation="False">Delete</asp:LinkButton></td>
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdCopyTab" Runat="server" CssClass="CommandButton" CausesValidation="False">Copy</asp:LinkButton></td>
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdDesignTab" Runat="server" CssClass="CommandButton" CausesValidation="False">Preview</asp:LinkButton></td>
							</tr>
						</table>
					</td>
					<td align="center" valign="top" style="border-left:1px #CCCCCC dotted; border-top:1px #CCCCCC dotted; border-right:1px #CCCCCC dotted;">
						<asp:radiobuttonlist id="optModuleType" cssclass="SubHead" runat="server" repeatdirection="Horizontal"
							repeatlayout="Flow" autopostback="True">
							<asp:listitem value="0" resourcekey="optModuleTypeNew">Add New Module</asp:listitem>
							<asp:listitem value="1" resourcekey="optModuleTypeExisting">Add Existing Module</asp:listitem>
						</asp:radiobuttonlist>
						<table cellspacing="1" cellpadding="0" border="0">
							<tr>
								<td align="center">
									<table cellspacing="1" cellpadding="0" border="0">
										<tr valign="bottom">
											<td class="SubHead" align="right" nowrap><asp:Label ID="lblModule" Runat="server" CssClass="SubHead" enableviewstate="False">Module:</asp:Label>&nbsp;</td>
											<td nowrap><asp:dropdownlist id="cboTabs" runat="server" cssclass="NormalTextBox" Width="140" datavaluefield="TabID"
													datatextfield="TabName" visible="False" autopostback="True"></asp:dropdownlist><asp:dropdownlist id="cboDesktopModules" runat="server" cssclass="NormalTextBox" Width="140" datavaluefield="DesktopModuleID"
													datatextfield="FriendlyName"></asp:dropdownlist>&nbsp;&nbsp;</td>
											<td class="SubHead" align="right" nowrap><asp:Label ID="lblPane" Runat="server" CssClass="SubHead" enableviewstate="False">Pane:</asp:Label>&nbsp;</td>
											<td nowrap><asp:dropdownlist id="cboPanes" runat="server" cssclass="NormalTextBox" Width="110"></asp:dropdownlist>&nbsp;&nbsp;</td>
											<td align="center" nowrap><asp:LinkButton id="cmdAddModuleIcon" runat="server" cssclass="CommandButton" CausesValidation="False">
													<asp:Image runat="server" EnableViewState="False" ID="imgAddModuleIcon" ImageUrl="~/admin/ControlPanel/images/iconbar_addmodule.gif"></asp:Image>
												</asp:LinkButton></td>
										</tr>
										<tr valign="bottom">
											<td class="SubHead" align="right" nowrap><asp:Label ID="lblTitle" Runat="server" CssClass="SubHead" enableviewstate="False">Title:</asp:Label>&nbsp;</td>
											<td nowrap><asp:dropdownlist id="cboModules" runat="server" cssclass="NormalTextBox" Width="140" datavaluefield="ModuleID"
													datatextfield="ModuleTitle" visible="False"></asp:dropdownlist><asp:TextBox ID="txtTitle" Runat="server" CssClass="NormalTextBox" Width="140"></asp:TextBox>&nbsp;&nbsp;</td>
											<td class="SubHead" align="right" nowrap><asp:Label ID="lblPosition" Runat="server" CssClass="SubHead" resourcekey="Position" enableviewstate="False">Insert:</asp:Label>&nbsp;</td>
											<td nowrap>
												<asp:dropdownlist id="cboPosition" runat="server" CssClass="NormalTextBox" Width="110">
													<asp:ListItem Value="0" resourcekey="Top">Top</asp:ListItem>
													<asp:ListItem Value="-1" resourcekey="Bottom">Bottom</asp:ListItem>
												</asp:dropdownlist>&nbsp;&nbsp;
											</td>
											<td align="center" class="Normal" nowrap><asp:linkbutton id="cmdAddModule" runat="server" cssclass="CommandButton" CausesValidation="False">Add</asp:linkbutton></td>
										</tr>
										<tr valign="bottom">
											<td class="SubHead" align="right" nowrap><asp:Label ID="lblPermission" Runat="server" CssClass="SubHead" resourcekey="Permission" enableviewstate="False">Visibility:</asp:Label>&nbsp;</td>
											<td nowrap>
												<asp:dropdownlist id="cboPermission" runat="server" CssClass="NormalTextBox" Width="140">
													<asp:ListItem Value="0" resourcekey="PermissionView">Same As Page</asp:ListItem>
													<asp:ListItem Value="1" resourcekey="PermissionEdit">Page Editors Only</asp:ListItem>
												</asp:dropdownlist>&nbsp;&nbsp;
											</td>
											<td class="SubHead" align="right" nowrap><asp:Label ID="lblAlign" Runat="server" CssClass="SubHead" enableviewstate="False">Align:</asp:Label>&nbsp;</td>
											<td nowrap>
												<asp:dropdownlist id="cboAlign" runat="server" CssClass="NormalTextBox" Width="110">
													<asp:ListItem Value="left" resourcekey="Left">Left</asp:ListItem>
													<asp:ListItem Value="center" resourcekey="Center">Center</asp:ListItem>
													<asp:ListItem Value="right" resourcekey="Right">Right</asp:ListItem>
										            <asp:listitem value="" resourcekey="Not_Specified" >Not Specified</asp:listitem>
												</asp:dropdownlist>&nbsp;&nbsp;
											</td>
											<td align="center" nowrap>&nbsp;</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
						<asp:linkbutton id="cmdInstallModules" runat="server" cssclass="CommandButton" CausesValidation="False" Visible="False">Install Additional Modules</asp:linkbutton>
					</td>
					<td align="center" valign="middle" style="border-top:1px #CCCCCC dotted;">
                        <asp:Label ID="lblCommonTasks" Runat="server" CssClass="SubHead" enableviewstate="False"><font size="1">Common Tasks</font></asp:Label>
						<table cellspacing="0" cellpadding="2" border="0">
							<tr valign="bottom" height="24">
								<td width="35" align="center"><asp:LinkButton ID="cmdSiteIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgSiteIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_site.gif"></asp:Image>
									</asp:LinkButton></td>
								<td width="35" align="center"><asp:LinkButton ID="cmdUsersIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgUsersIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_users.gif"></asp:Image>
									</asp:LinkButton></td>
								<td width="35" align="center"><asp:LinkButton ID="cmdRolesIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgRolesIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_roles.gif"></asp:Image>
									</asp:LinkButton></td>
								<td width="35" align="center"><asp:LinkButton ID="cmdFilesIcon" Runat="server" CssClass="CommandButton" CausesValidation="False">
										<asp:Image ID="imgFilesIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_files.gif"></asp:Image>
									</asp:LinkButton></td>
								<td width="35" align="center"><asp:Hyperlink ID="cmdHelpIcon" Runat="server" CssClass="CommandButton" CausesValidation="False"
										Target="_new">
										<asp:Image ID="imgHelpIcon" Runat="server" ImageUrl="~/admin/ControlPanel/images/iconbar_help.gif"></asp:Image>
									</asp:Hyperlink></td>
							</tr>
							<tr valign="bottom">
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdSite" Runat="server" CssClass="CommandButton" CausesValidation="False">Site</asp:LinkButton></td>
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdUsers" Runat="server" CssClass="CommandButton" CausesValidation="False">Users</asp:LinkButton></td>
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdRoles" Runat="server" CssClass="CommandButton" CausesValidation="False">Roles</asp:LinkButton></td>
								<td width="35" align="center" class="Normal"><asp:LinkButton ID="cmdFiles" Runat="server" CssClass="CommandButton" CausesValidation="False">Files</asp:LinkButton></td>
								<td width="35" align="center" class="Normal"><asp:Hyperlink ID="cmdHelp" Runat="server" CssClass="CommandButton" CausesValidation="False" Target="_new">Help</asp:Hyperlink></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
