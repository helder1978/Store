<%@ Control Inherits="DotNetNuke.Modules.Admin.Host.HostSettingsModule" language="vb" AutoEventWireup="false" Explicit="True" CodeFile="HostSettings.ascx.vb" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="FriendlyUrls" Src="~/admin/Host/FriendlyUrls.ascx" %>
<%@ Register TagPrefix="dnn" TagName="RequestFilters" Src="~/admin/Host/RequestFilters.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Authentication" Src="~/admin/Host/Authentication.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Skin" Src="~/controls/SkinControl.ascx" %>
<!-- Settings Tables -->
<table class="Settings" cellspacing="2" cellpadding="2" width="560" summary="Host Settings Design Table" border="0">
	<tr>
		<td width="560" valign="top">
			<dnn:sectionhead id="dshBasic" runat="server" 
			    cssclass="Head" 
			    text="Basic Settings" 
			    section="tblBasic"
				resourcekey="BasicSettings" 
				includerule="True" 
				isexpanded="True" />
			<table id="tblBasic" cellspacing="0" cellpadding="2" width="525" summary="Basic Settings Design Table" border="0" runat="server">
				<tr>
					<td colspan="2"><asp:label id="lblBasicSettingsHelp" cssclass="Normal" runat="server" resourcekey="BasicSettingsHelp" enableviewstate="False" /></td>
				</tr>
				<tr>
					<td width="25"></td>
					<td valign="top" width="475">
						<dnn:sectionhead id="dshConfiguration" runat="server" 
						    cssclass="Head" 
						    text="Configuration" 
						    section="tblConfiguration"
							resourcekey="Configuration" />
						<table id="tblConfiguration" cellspacing="2" cellpadding="2" summary="Configuration Design Table" border="0" runat="server">
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plVersion" text="DotNetNuke Version:" controlname="lblVersion" runat="server" /></td>
								<td><asp:Label ID="lblVersion" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plUpgrade" text="Check For Upgrades?" controlname="chkUpgrade" runat="server" /></td>
								<td><asp:CheckBox ID="chkUpgrade" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plAvailable" text="Upgrade Available?" controlname="hypUpgrade" runat="server" /></td>
								<td><asp:HyperLink ID="hypUpgrade" Target="_new" Runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plDataProvider" text="Data Provider:" controlname="lblDataProvider" runat="server" /></td>
								<td><asp:Label ID="lblDataProvider" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plFramework" text=".NET Framework:" controlname="lblFramework" runat="server" /></td>
								<td><asp:Label ID="lblFramework" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plIdentity" text="ASP.NET Identity:" controlname="lblIdentity" runat="server" /></td>
								<td><asp:Label ID="lblIdentity" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostName" text="Host Name:" controlname="lblHostName" runat="server" /></td>
								<td><asp:Label ID="lblHostName" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plPermissions" text="Permissions:" controlname="lblPermissions" runat="server" /></td>
								<td><asp:Label ID="lblPermissions" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plApplicationPath" text="Relative Path:" controlname="lblApplicationPath" runat="server" /></td>
								<td><asp:Label ID="lblApplicationPath" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plApplicationMapPath" text="Physical Path:" controlname="lblApplicationMapPath" runat="server" /></td>
								<td><asp:Label ID="lblApplicationMapPath" Runat="server" CssClass="NormalBold" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plGUID" text="GUID:" controlname="lblGUID" runat="server" /></td>
								<td><asp:Label ID="lblGUID" Runat="server" CssClass="NormalBold" /></td>
							</tr>
						</table>
						<br/>
						<dnn:sectionhead id="dshHost" runat="server" 
						    cssclass="Head" 
						    text="Host Details" 
						    section="tblHost"
							resourcekey="HostDetails" />
						<table id="tblHost" cellspacing="2" cellpadding="2" summary="Site Details Design Table" border="0" runat="server">
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostPortal" text="Host Portal:" controlname="cboHostPortal" runat="server" /></td>
								<td><asp:dropdownlist id="cboHostPortal" cssclass="NormalTextBox" datatextfield="PortalName" datavaluefield="PortalID" width="300" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostTitle" text="Host Title:" controlname="txtHostTitle" runat="server" /></td>
								<td><asp:textbox id="txtHostTitle" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostURL" text="Host URL:" controlname="txtHostURL" runat="server" /></td>
								<td><asp:textbox id="txtHostURL" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostEmail" text="Host Email:" controlname="txtHostEmail" runat="server" /></td>
								<td><asp:textbox id="txtHostEmail" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
						</table>
						<br/>
						<dnn:sectionhead id="dshAppearance" runat="server" 
						    cssclass="Head" 
						    text="Appearance" 
						    section="tblAppearance"
							resourcekey="Appearance" />
						<table id="tblAppearance" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plCopyright" text="Show Copyright Credits?" controlname="chkCopyright" runat="server" /></td>
								<td valign="top"><asp:checkbox id="chkCopyright" cssclass="NormalTextBox" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plUseCustomErrorMessages" text="Use Custom Error Messages?" controlname="chkUseCustomErrorMessages" runat="server" /></td>
								<td valign="top"><asp:checkbox id="chkUseCustomErrorMessages" cssclass="NormalTextBox" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostSkin" text="Host Skin:" controlname="ctlHostSkin" runat="server" /></td>
								<td valign="top"><dnn:skin id="ctlHostSkin" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostContainer" text="Host Container:" controlname="ctlHostContainer" runat="server" /></td>
								<td valign="top"><dnn:skin id="ctlHostContainer" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plAdminSkin" text="Admin Skin:" controlname="ctlAdminSkin" runat="server" /></td>
								<td valign="top"><dnn:skin id="ctlAdminSkin" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plAdminContainer" text="Admin Container:" controlname="txtHostEmail" runat="server" /></td>
								<td valign="top"><dnn:skin id="ctlAdminContainer" runat="server" /></td>
							</tr>
							<tr>
								<td align="center" colspan="2">
								    <br/>
	                                <dnn:CommandButton id="cmdUploadSkin" resourcekey="SkinUpload" runat="server" cssClass="CommandButton" ImageUrl="~/images/up.gif" />&nbsp;&nbsp;
	                                <dnn:CommandButton id="cmdUploadContainer" resourcekey="ContainerUpload" runat="server" cssClass="CommandButton" ImageUrl="~/images/up.gif" />&nbsp;&nbsp;
								</td>
							</tr>
						</table>
						<br/>
						<dnn:sectionhead id="dshPayment" runat="server" 
						    cssclass="Head" 
						    text="Payment Settings" 
						    isExpanded="False"
							section="tblPayment" 
							resourcekey="Payment" />
						<table id="tblPayment" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0" runat="server">
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plProcessor" text="Payment Processor:" controlname="cboProcessor" runat="server" /></td>
								<td align="left">
									<asp:dropdownlist id="cboProcessor" cssclass="NormalTextBox" datatextfield="value" datavaluefield="text" width="325" runat="server" /><br/>
	                                <dnn:CommandButton id="cmdProcessor" resourcekey="ProcessorWebSite" runat="server" cssClass="CommandButton" ImageUrl="~/images/rt.gif" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plUserId" text="Payment UserId:" controlname="txtUserId" runat="server" /></td>
								<td><asp:textbox id="txtUserId" runat="server" width="300" maxlength="50" cssclass="NormalTextBox" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plPassword" text="Payment Password:" controlname="txtPassword" runat="server" /></td>
								<td><asp:textbox id="txtPassword" runat="server" width="300" maxlength="50" cssclass="NormalTextBox" textmode="Password" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostFee" text="Hosting Fee:" controlname="txtHostFee" runat="server" /></td>
								<td>
								    <asp:textbox id="txtHostFee" cssclass="NormalTextBox" runat="server" maxlength="10" width="100" />
                                    <asp:CompareValidator ID="valHostFee" runat="server" ControlToValidate="txtHostFee"
                                        CssClass="NormalRed" Display="Dynamic" ErrorMessage="Invalid fee, needs to be a currency value!" 
                                        ResourceKey="valHostFee.Error" Operator="DataTypeCheck" Type="Currency" />
                                </td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostCurrency" text="Hosting Currency:" controlname="cboHostCurrency" runat="server" /></td>
								<td><asp:dropdownlist id="cboHostCurrency" cssclass="NormalTextBox" datavaluefield="value" datatextfield="text" width="150" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHostSpace" text="Hosting Space (MB):" controlname="txtHostSpace" runat="server" /></td>
								<td><asp:textbox id="txtHostSpace" cssclass="NormalTextBox" runat="server" maxlength="6" width="100" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plPageQuota" text="Page Quota:" controlname="txtPageQuota" runat="server" /></td>
								<td><asp:textbox id="txtPageQuota" cssclass="NormalTextBox" runat="server" maxlength="6" width="100" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plUserQuota" text="User Quota:" controlname="txtUserQuota" runat="server" /></td>
								<td><asp:textbox id="txtUserQuota" cssclass="NormalTextBox" runat="server" maxlength="6" width="100" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plDemoPeriod" text="Demo Period (Days):" controlname="txtDemoPeriod" runat="server" /></td>
								<td><asp:textbox id="txtDemoPeriod" cssclass="NormalTextBox" runat="server" maxlength="3" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plDemoSignup" text="Anonymous Demo Signup:" controlname="chkDemoSignup" runat="server" /></td>
								<td><asp:checkbox id="chkDemoSignup" cssclass="NormalTextBox" runat="server" /></td>
							</tr>
						</table>
						<br/>
					</td>
				</tr>
			</table>
            <br/>
			<dnn:sectionhead id="dshAdvanced" runat="server" 
			    cssclass="Head" 
			    text="Advanced Settings" 
			    section="tblAdvanced"
				resourcekey="AdvancedSettings" 
				isexpanded="True" 
				includerule="False" />
			<table id="tblAdvanced" cellspacing="0" cellpadding="2" width="525" summary="Basic Settings Design Table" border="0" runat="server">
				<tr>
					<td colspan="2"><asp:label id="lblAdvancedSettingsHelp" cssclass="Normal" runat="server" resourcekey="AdvancedSettingsHelp" enableviewstate="False" /></td>
				</tr>
				<tr>
					<td width="25"></td>
					<td valign="top" width="475">
						<dnn:sectionhead id="dshAuthentication" runat="server" 
						    cssclass="Head" 
						    text="Authentication Settings" 
						    isexpanded="False"
							section="tblAuthentication" 
							resourcekey="Authentication" />
                        <table id="tblAuthentication" runat="server" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0">
	                        <tr>
	                            <td colspan="2" align="center">
	                                <dnn:Authentication id="authentication" runat="server" />
	                            </td>
	                        </tr>
	                    </table>
						<br />
						<dnn:sectionhead id="dshFriendlyUrl" runat="server" 
						    cssclass="Head" 
						    text="Friendly Url Settings" 
						    isexpanded="False"
							section="tblFriendlyUrl" 
							resourcekey="FriendlyUrl" />
                        <table id="tblFriendlyUrl" runat="server" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0">
	                        <tr>
		                        <td class="SubHead" width="150"><dnn:label id="plUseFriendlyUrls" text="Use Friendly Urls?" controlname="chkUseFriendlyUrls" runat="server" /></td>
		                        <td valign="top"><asp:checkbox id="chkUseFriendlyUrls" AutoPostBack="true" cssclass="NormalTextBox" runat="server" /></td>
	                        </tr>
	                        <tr id="rowFriendlyUrls" runat="server">
	                            <td colspan="2" align="center"><dnn:FriendlyUrls id="friendlyUrls" runat="server" /></td>
	                        </tr>
	                    </table>
						<br />
						<dnn:sectionhead id="dshRequestFilter" runat="server" 
						    cssclass="Head" 
						    text="Request Filter Settings" 
						    isexpanded="False"
							section="tblRequestFilter" 
							resourcekey="RequestFilter" />
                        <table id="tblRequestFilter" runat="server" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0">
	                        <tr>
		                        <td class="SubHead" width="150"><dnn:label id="plEnableRequestFilters" text="Enable Request Filters?" controlname="chkEnableRequestFilters" runat="server" /></td>
		                        <td valign="top"><asp:checkbox id="chkEnableRequestFilters" AutoPostBack="true" cssclass="NormalTextBox" runat="server" /></td>
	                        </tr>
	                        <tr id="rowRequestFilters" runat="server">
	                            <td colspan="2" align="center"><dnn:RequestFilters id="requestFilters" runat="server" /></td>
	                        </tr>
	                    </table>
						<br />
						<dnn:sectionhead id="dshProxy" runat="server" 
						    cssclass="Head" 
						    text="Proxy Settings" 
						    isexpanded="False"
							section="tblProxy" 
							resourcekey="Proxy" />
						<table id="tblProxy" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0" runat="server">
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plProxyServer" text="Proxy Server:" controlname="txtProxyServer" runat="server" /></td>
								<td><asp:textbox id="txtProxyServer" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plProxyPort" text="Proxy Port:" controlname="txtProxyPort" runat="server" /></td>
								<td><asp:textbox id="txtProxyPort" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plProxyUsername" text="Proxy Username:" controlname="txtProxyUsername" runat="server" /></td>
								<td><asp:textbox id="txtProxyUsername" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plProxyPassword" text="Proxy Password:" controlname="txtProxyPassword" runat="server" /></td>
								<td><asp:textbox id="txtProxyPassword" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" textmode="Password" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plWebRequestTimeout" text="Web Request Timeout:" controlname="txtWebRequestTimeout" runat="server" /></td>
								<td><asp:textbox id="txtWebRequestTimeout" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
						</table>
						<br />
						<dnn:sectionhead id="dshSMTP" runat="server" 
						    cssclass="Head" 
						    text="SMTP Server Settings" 
						    isexpanded="False"
							section="tblSMTP" 
							resourcekey="SMTP" />
						<table id="tblSMTP" cellspacing="2" cellpadding="2" summary="Appearance Design Table" border="0" runat="server">
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSMTPServer" text="SMTP Server:" controlname="txtSMTPServer" runat="server" /></td>
								<td align="left">
									<asp:textbox id="txtSMTPServer" cssclass="NormalTextBox" runat="server" maxlength="256" width="225" />
									&nbsp;
									<asp:linkbutton id="cmdEmail" resourcekey="EmailTest" runat="server" cssclass="CommandButton">Test</asp:linkbutton>
									<asp:label id="lblEmail" runat="server" cssclass="NormalRed" />
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSMTPAuthentication" text="SMTP Authentication:" controlname="optSMTPAuthentication" runat="server" /></td>
								<td>
									<asp:radiobuttonlist id="optSMTPAuthentication" cssclass="Normal" runat="server" repeatdirection="Horizontal">
										<asp:listitem value="0" resourcekey="SMTPAnonymous">Anonymous</asp:listitem>
										<asp:listitem value="1" resourcekey="SMTPBasic">Basic</asp:listitem>
										<asp:listitem value="2" resourcekey="SMTPNTLM">NTLM</asp:listitem>
									</asp:radiobuttonlist>
                                </td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSMTPEnableSSL" text="SMTP Enable SSL:" controlname="chkSMTPEnableSSL" runat="server" /></td>
								<td><asp:CheckBox ID="chkSMTPEnableSSL" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSMTPUsername" text="SMTP Username:" controlname="txtSMTPUsername" runat="server" /></td>
								<td><asp:textbox id="txtSMTPUsername" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSMTPPassword" text="SMTP Password:" controlname="txtSMTPPassword" runat="server" /></td>
								<td><asp:textbox id="txtSMTPPassword" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" textmode="Password" /></td>
							</tr>
						</table>
						<br/>
                        <dnn:sectionhead id="dshPerformance" cssclass="Head" runat="server" text="Performance Settings" isexpanded="False"
							section="tblPerformance" resourcekey="Performance" />
						<table id="tblPerformance" cellspacing="2" cellpadding="2" summary="Performance Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200" valign="top"><dnn:label id="plPageState" runat="server" controlname="cboPageState" text="Page State Persistence:"></dnn:label></td>
								<td>
									<asp:radiobuttonlist id="cboPageState" cssclass="Normal" runat="server" repeatdirection="Horizontal">
										<asp:listitem resourcekey="Page" value="P">Page</asp:listitem>
										<asp:listitem resourcekey="Memory" value="M">Memory</asp:listitem>
									</asp:radiobuttonlist>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200" valign="top"><dnn:label id="plCacheMethod" runat="server" controlname="cboCacheMethod" text="Cache Method:"></dnn:label></td>
								<td>
									<asp:radiobuttonlist id="cboCacheMethod" cssclass="Normal" runat="server" repeatdirection="Horizontal">
										<asp:listitem resourcekey="Memory" value="M">Memory</asp:listitem>
										<asp:listitem resourcekey="Disk" value="D">Disk</asp:listitem>
									</asp:radiobuttonlist>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plPerformance" text="Performance Setting:" controlname="cboPerformance" runat="server" /></td>
								<td>
									<asp:dropdownlist id="cboPerformance" runat="server" Width="150">
										<asp:listitem resourcekey="NoCaching" value="0">No Caching</asp:listitem>
										<asp:listitem resourcekey="LightCaching" value="1">Light Caching</asp:listitem>
										<asp:listitem resourcekey="ModerateCaching" value="3">Moderate Caching</asp:listitem>
										<asp:listitem resourcekey="HeavyCaching" value="6">Heavy Caching</asp:listitem>
									</asp:dropdownlist>
									&nbsp;
									<asp:linkbutton id="cmdCache" resourcekey="ClearCache" runat="server" cssclass="CommandButton">Clear Cache</asp:linkbutton>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCacheability" text="Authenticated Cacheability:" controlname="cboCacheability" runat="server" /></td>
								<td>
									<asp:dropdownlist id="cboCacheability" runat="server" Width="150">
										<asp:listitem resourcekey="NoCache" value="0">NoCache</asp:listitem>
										<asp:listitem resourcekey="Private" value="1">Private</asp:listitem>
										<asp:listitem resourcekey="Public" value="2">Public</asp:listitem>
										<asp:listitem resourcekey="Server" value="3">Server</asp:listitem>
										<asp:listitem resourcekey="ServerAndNoCache" value="4">ServerAndNoCache</asp:listitem>
										<asp:listitem resourcekey="ServerAndPrivate" value="5">ServerAndPrivate</asp:listitem>
									</asp:dropdownlist>
								</td>
							</tr>						
							<tr>
								<td class="SubHead" width="200"><dnn:label id="plCompression" text="Compression Setting:" controlname="cboCompression" runat="server" /></td>
								<td>
									<asp:dropdownlist id="cboCompression" runat="server" Width="150">
										<asp:listitem resourcekey="NoCompression" value="0">No Compression</asp:listitem>
										<asp:listitem resourcekey="Deflate" value="2">Deflate Compression</asp:listitem>
										<asp:listitem resourcekey="GZip" value="1">GZip Compression</asp:listitem>
									</asp:dropdownlist>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plWhitespace" text="Use Whitespace Filter:" controlname="chkWhitespace" runat="server" /></td>
								<td>
        							<asp:CheckBox ID="chkWhitespace" runat="server" />&nbsp;
                                </td>
							</tr>
						</table>
						<br/>
                        <dnn:sectionhead id="dshCompression" cssclass="Head" runat="server" text="Compression Settings" isexpanded="False"
							section="tblCompression" resourcekey="Compression" />
						<table id="tblCompression" cellspacing="2" cellpadding="2" summary="Compression Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="200" valign="top"><dnn:label id="plExcludedPaths" runat="server" controlname="txtExcludedPaths" text="Excluded Paths:"></dnn:label></td>
								<td><asp:textbox id="txtExcludedPaths" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" textmode="MultiLine" rows="3" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="200" valign="top"><dnn:label id="plWhitespaceFilter" runat="server" controlname="txtWhitespaceFilter" text="Whitespace Filter:"></dnn:label></td>
								<td><asp:textbox id="txtWhitespaceFilter" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" textmode="MultiLine" rows="3" /></td>
							</tr>
							<tr>
							    <td colspan="2">
    						        <asp:linkbutton class="CommandButton" id="cmdUpdateCompression" resourcekey="cmdUpdate" runat="server" text="Update"></asp:linkbutton>
							    </td>
							</tr>
						</table>
						<br/>
						<dnn:sectionhead id="dshOther" cssclass="Head" runat="server" text="Other Settings" isexpanded="False"
							section="tblOther" resourcekey="Other" />
						<table id="tblOther" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
							border="0" runat="server">
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plControlPanel" text="Control Panel:" controlname="cboControlPanel" runat="server" /></td>
								<td valign="top"><asp:dropdownlist id="cboControlPanel" cssclass="NormalTextBox" width="300" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSiteLogStorage" text="Site Log Storage:" controlname="optSiteLogStorage" runat="server" /></td>
								<td>
									<asp:radiobuttonlist id="optSiteLogStorage" cssclass="Normal" runat="server" repeatdirection="Horizontal">
										<asp:listitem value="D" resourcekey="Database">Database</asp:listitem>
										<asp:listitem value="F" resourcekey="FileSystem">File System</asp:listitem>
									</asp:radiobuttonlist>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSiteLogBuffer" text="Site Log Buffer (Items):" controlname="txtSiteLogBuffer"
										runat="server" /></td>
								<td><asp:textbox id="txtSiteLogBuffer" cssclass="NormalTextBox" runat="server" maxlength="4" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSiteLogHistory" text="Site Log History (Days):" controlname="txtSiteLogHistory"
										runat="server" /></td>
								<td><asp:textbox id="txtSiteLogHistory" cssclass="NormalTextBox" runat="server" maxlength="3" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plUsersOnline" text="Disable Users Online?" controlname="chkUsersOnline" runat="server" /></td>
								<td><asp:checkbox id="chkUsersOnline" cssclass="NormalTextBox" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plUsersOnlineTime" text="Users Online Time (Minutes):" controlname="txtUsersOnlineTime"
										runat="server" /></td>
								<td><asp:textbox id="txtUsersOnlineTime" cssclass="NormalTextBox" runat="server" maxlength="3" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plAutoAccountUnlock" text="Auto-Unlock Accounts After (Minutes):" controlname="txtAutoAccountUnlock"
										runat="server" /></td>
								<td><asp:textbox id="txtAutoAccountUnlock" cssclass="NormalTextBox" runat="server" maxlength="3"
										width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plFileExtensions" text="File Upload Extensions:" controlname="txtFileExtensions"
										runat="server" /></td>
								<td><asp:textbox id="txtFileExtensions" cssclass="NormalTextBox" runat="server" maxlength="256" width="300"
										textmode="MultiLine" rows="3" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSkinUpload" text="Skin Upload Permission:" controlname="optSkinUpload" runat="server" /></td>
								<td>
									<asp:radiobuttonlist id="optSkinUpload" cssclass="Normal" runat="server" repeatdirection="Horizontal">
										<asp:listitem resourcekey="Host" value="G">Host</asp:listitem>
										<asp:listitem resourcekey="Portal" value="L">Portal</asp:listitem>
									</asp:radiobuttonlist>
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plSchedulerMode" text="Scheduler Mode:" controlname="cboSchedulerMode" runat="server" /></td>
								<td>
									<asp:dropdownlist id="cboSchedulerMode" runat="server">
										<asp:listitem resourcekey="Disabled" value="0">Disabled</asp:listitem>
										<asp:listitem resourcekey="TimerMethod" value="1">Timer Method</asp:listitem>
										<asp:listitem resourcekey="RequestMethod" value="2">Request Method</asp:listitem>
									</asp:dropdownlist>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plLogBuffer" text="Enable Event Log Buffer?" controlname="chkUsersOnline" runat="server" /></td>
								<td><asp:checkbox id="chkLogBuffer" cssclass="NormalTextBox" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plHelpUrl" text="Help Url:" controlname="txtHelpURL" runat="server" /></td>
								<td><asp:textbox id="txtHelpURL" cssclass="NormalTextBox" runat="server" maxlength="256" width="300" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plEnableHelp" text="Enable Module Help?" controlname="chkEnableHelp" runat="server" /></td>
								<td valign="top"><asp:checkbox id="chkEnableHelp" cssclass="NormalTextBox" runat="server" /></td>
							</tr>
							<tr>
								<td class="SubHead" width="150"><dnn:label id="plAutoSync" text="Enable File System Auto-Sync?" controlname="chkAutoSync" runat="server" /></td>
								<td valign="top"><asp:checkbox id="chkAutoSync" cssclass="NormalTextBox" runat="server" /></td>
							</tr>
							<tr>
								<td colspan="2">
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<p>
	<dnn:CommandButton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssClass="CommandButton" ImageUrl="~/images/save.gif" />&nbsp;&nbsp;
	<dnn:CommandButton id="cmdRestart" resourcekey="cmdRestart" runat="server" cssClass="CommandButton" ImageUrl="~/images/reset.gif" />
</p>
<hr noshade="noshade" size="1" />
<br/>
<table class="Settings" cellspacing="2" cellpadding="2" summary="Host Settings Design Table" border="0">
	<tr>
		<td class="SubHead" valign="bottom"><dnn:label id="plLog" text="View Upgrade Log For Version:" controlname="cboUpgrade" runat="server" /></td>
		<td valign="bottom">
			<asp:dropdownlist id="cboVersion" runat="server" cssclass="NormalTextBox" />&nbsp;
			<dnn:CommandButton id="cmdUpgrade" resourcekey="cmdGo" runat="server" cssClass="CommandButton" ImageUrl="~/images/view.gif" />
		</td>
	</tr>
	<tr>
		<td colspan="2"><asp:label id="lblUpgrade" runat="server" cssclass="Normal" /></td>
	</tr>
</table>
