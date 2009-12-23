<%@ Register TagPrefix="Portal" TagName="DualList" Src="~/controls/DualListControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Skin" Src="~/controls/SkinControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Control Inherits="DotNetNuke.Modules.Admin.PortalManagement.SiteSettings" Language="vb"
    AutoEventWireup="false" Explicit="True" EnableViewState="True" CodeFile="SiteSettings.ascx.vb" %>
<!-- Settings Tables -->
<table class="Settings" cellspacing="2" cellpadding="2" width="560" summary="Site Settings Design Table"
    border="0">
    <tr>
        <td width="560" valign="top">
            <asp:Panel ID="pnlSettings" runat="server" CssClass="WorkPanel" Visible="True">
                <dnn:SectionHead ID="dshBasic" CssClass="Head" runat="server" Text="Basic Settings"
                    Section="tblBasic" ResourceKey="BasicSettings" IncludeRule="True"></dnn:SectionHead>
                <table id="tblBasic" cellspacing="0" cellpadding="2" width="525" summary="Basic Settings Design Table"
                    border="0" runat="server">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblBasicSettingsHelp" CssClass="Normal" runat="server" resourcekey="BasicSettingsHelp"
                                EnableViewState="False">In this section, you can set up the basic settings for your site.</asp:Label></td>
                    </tr>
                    <tr>
                        <td width="25">
                        </td>
                        <td valign="top" width="475">
                            <dnn:SectionHead ID="dshSite" CssClass="Head" runat="server" Text="Site Details"
                                Section="tblSite" ResourceKey="SiteDetails"></dnn:SectionHead>
                            <table id="tblSite" cellspacing="2" cellpadding="2" summary="Site Details Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plPortalName" runat="server" Text="Title:" ControlName="txtPortalName">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:TextBox ID="txtPortalName" CssClass="NormalTextBox" runat="server" Width="325"
                                            MaxLength="128"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plDescription" runat="server" Text="Description:" ControlName="txtDescription">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" width="325">
                                        <asp:TextBox ID="txtDescription" CssClass="NormalTextBox" runat="server" Width="325"
                                            MaxLength="475" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plKeyWords" runat="server" Text="Key Words:" ControlName="txtKeyWords">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" width="325">
                                        <asp:TextBox ID="txtKeyWords" CssClass="NormalTextBox" runat="server" Width="325"
                                            MaxLength="475" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plFooterText" runat="server" Text="Copyright:" ControlName="txtFooterText">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" width="325">
                                        <asp:TextBox ID="txtFooterText" CssClass="NormalTextBox" runat="server" Width="325"
                                            MaxLength="100"></asp:TextBox></td>
                                </tr>
							    <tr>
								    <td class="SubHead" width="150"><dnn:label id="plGUID" text="GUID:" controlname="lblGUID" runat="server" /></td>
								    <td><asp:Label ID="lblGUID" Runat="server" CssClass="NormalBold"></asp:Label></td>
							    </tr>
                            </table>
                            <br/>
                            <dnn:SectionHead ID="dshMarketing" CssClass="Head" runat="server" Text="Marketing"
                                Section="tblMarketing" ResourceKey="Marketing" IsExpanded="True"></dnn:SectionHead>
                            <table id="tblMarketing" cellspacing="2" cellpadding="2" summary="Marketing Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plSearchEngine" runat="server" ControlName="cboSearchEngine" Suffix="" Text="Search Engine:"></dnn:Label>
                                    </td>
                                    <td width="325">
                                        <asp:DropDownList ID="cboSearchEngine" runat="server" CssClass="NormalTextBox" Width="250">
                                            <asp:ListItem>Google</asp:ListItem>
                                            <asp:ListItem>Yahoo</asp:ListItem>
                                            <asp:ListItem>Microsoft</asp:ListItem>
                                        </asp:DropDownList>&nbsp;
                                        <asp:LinkButton CssClass="CommandButton" ID="cmdSearchEngine" resourcekey="cmdSearchEngine" runat="server" Text="Submit"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plSiteMap" runat="server" ControlName="txtSiteMap" Suffix="" Text="Site Map:"></dnn:Label>
                                    </td>
                                    <td width="325">
                                        <asp:TextBox ID="txtSiteMap" runat="server" CssClass="NormalTextBox" ReadOnly="true" Width="250"></asp:TextBox>&nbsp;
                                        <asp:LinkButton CssClass="CommandButton" ID="cmdSiteMap" resourcekey="cmdSiteMap" runat="server" Text="Submit"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plVerification" runat="server" ControlName="txtVerification" Suffix="" Text="Verification:"></dnn:Label>
                                    </td>
                                    <td width="325">
                                        <asp:TextBox ID="txtVerification" runat="server" CssClass="NormalTextBox" Width="250"></asp:TextBox>&nbsp;
                                        <asp:LinkButton CssClass="CommandButton" ID="cmdVerification" resourcekey="cmdVerification" runat="server" Text="Create"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plAdvertising" runat="server" ControlName="lblAdvertising" Suffix="" Text="Advertising:"></dnn:Label>
                                    </td>
                                    <td width="325">
                                        <asp:Label ID="lblAdvertising" runat="server" CssClass="CommandButton"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plBanners" runat="server" ControlName="optBanners" Text="Banners:"></dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" width="325" nowrap>
                                        <asp:RadioButtonList ID="optBanners" CssClass="Normal" runat="server" EnableViewState="False"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" resourcekey="None">None</asp:ListItem>
                                            <asp:ListItem Value="1" resourcekey="Site">Site</asp:ListItem>
                                            <asp:ListItem Value="2" resourcekey="Host">Host</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <dnn:SectionHead ID="dshAppearance" CssClass="Head" runat="server" Text="Appearance"
                                Section="tblAppearance" ResourceKey="Appearance" IsExpanded="False"></dnn:SectionHead>
                            <table id="tblAppearance" cellspacing="2" cellpadding="2" summary="Appearance Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plLogo" runat="server" ControlName="ctlLogo" Suffix=""></dnn:Label>
                                    </td>
                                    <td width="325">
                                        <Portal:URL ID="ctlLogo" runat="server" Width="325" ShowUrls="False" ShowTabs="False"
                                            ShowLog="False" ShowTrack="False" Required="False"></Portal:URL>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plBackground" runat="server" Text="Body Background:" ControlName="cboBackground">
                                        </dnn:Label>
                                    </td>
                                    <td width="325">
                                        <Portal:URL ID="ctlBackground" runat="server" Width="325" ShowUrls="False" ShowTabs="False"
                                            ShowLog="False" ShowTrack="False" Required="False"></Portal:URL>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plPortalSkin" runat="server" Text="Portal Skin:" ControlName="ctlPortalSkin">
                                        </dnn:Label>
                                    </td>
                                    <td valign="top" width="325">
                                        <dnn:Skin ID="ctlPortalSkin" runat="server"></dnn:Skin>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plPortalContainer" runat="server" Text="Portal Container:" ControlName="ctlPortalContainer">
                                        </dnn:Label>
                                    </td>
                                    <td valign="top" width="325">
                                        <dnn:Skin ID="ctlPortalContainer" runat="server"></dnn:Skin>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plAdminSkin" runat="server" Text="Admin Skin:" ControlName="ctlAdminSkin">
                                        </dnn:Label>
                                    </td>
                                    <td valign="top" width="325">
                                        <dnn:Skin ID="ctlAdminSkin" runat="server"></dnn:Skin>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plAdminContainer" runat="server" Text="Admin Container:" ControlName="ctlAdminContainer">
                                        </dnn:Label>
                                    </td>
                                    <td valign="top" width="325">
                                        <dnn:Skin ID="ctlAdminContainer" runat="server"></dnn:Skin>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <br/>
                                        <asp:HyperLink ID="lnkUploadSkin" runat="server" resourcekey="SkinUpload" CssClass="CommandButton">Upload 
                  Skin</asp:HyperLink>&nbsp;&nbsp;
                                        <asp:HyperLink ID="lnkUploadContainer" runat="server" resourcekey="ContainerUpload"
                                            CssClass="CommandButton">Upload 
                  Container</asp:HyperLink></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br>
                <dnn:SectionHead ID="dshAdvanced" CssClass="Head" runat="server" Text="Advanced Settings"
                    Section="tblAdvanced" ResourceKey="AdvancedSettings" IncludeRule="True" IsExpanded="False">
                </dnn:SectionHead>
                <table id="tblAdvanced" cellspacing="0" cellpadding="2" width="525" summary="Advanced Settings Design Table"
                    border="0" runat="server">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblAdvancedSettingsHelp" CssClass="Normal" runat="server" resourcekey="AdvancedSettingsHelp"
                                EnableViewState="False">In this section, you can set up more advanced settings for your site.</asp:Label></td>
                    </tr>
                    <tr>
                        <td width="25">
                        </td>
                        <td valign="top" width="475">
                            <dnn:SectionHead ID="dshSecurity" CssClass="Head" runat="server" Text="Security Settings"
                                Section="tblSecurity" ResourceKey="SecuritySettings"></dnn:SectionHead>
                            <table id="tblSecurity" cellspacing="2" cellpadding="2" summary="Security Settings Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plUserRegistration" runat="server" Text="User Registration:" ControlName="optUserRegistration">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:RadioButtonList ID="optUserRegistration" CssClass="Normal" runat="server" EnableViewState="False"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" resourcekey="None">None</asp:ListItem>
                                            <asp:ListItem Value="1" resourcekey="Private">Private</asp:ListItem>
                                            <asp:ListItem Value="2" resourcekey="Public">Public</asp:ListItem>
                                            <asp:ListItem Value="3" resourcekey="Verified">Verified</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <dnn:SectionHead ID="dshPages" CssClass="Head" runat="server" Text="Page Management"
                                Section="tblPages" ResourceKey="Pages"></dnn:SectionHead>
                            <table id="tblPages" cellspacing="2" cellpadding="2" summary="Page Management Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plSplashTabId" runat="server" Text="Splash Page:" ControlName="cboSplashTabId">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:DropDownList ID="cboSplashTabId" CssClass="NormalTextBox" runat="server" Width="325"
                                            DataTextField="TabName" DataValueField="TabId">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plHomeTabId" runat="server" Text="Home Page:" ControlName="cboHomeTabId">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:DropDownList ID="cboHomeTabId" CssClass="NormalTextBox" runat="server" Width="325"
                                            DataTextField="TabName" DataValueField="TabId">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plLoginTabId" runat="server" Text="Login Page:" ControlName="cboLoginTabId">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:DropDownList ID="cboLoginTabId" CssClass="NormalTextBox" runat="server" Width="325"
                                            DataTextField="TabName" DataValueField="TabId">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plUserTabId" runat="server" Text="User Page:" ControlName="cboUserTabId">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:DropDownList ID="cboUserTabId" CssClass="NormalTextBox" runat="server" Width="325"
                                            DataTextField="TabName" DataValueField="TabId">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plHomeDirectory" runat="server" Text="Home Directory:" ControlName="txtHomeDirectory">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" width="325">
                                        <asp:TextBox ID="txtHomeDirectory" CssClass="NormalTextBox" runat="server" Width="325"
                                            MaxLength="100" Enabled="False"></asp:TextBox></td>
                                </tr>
                            </table>
                            <br/>
                            <dnn:SectionHead ID="dshPayment" CssClass="Head" runat="server" Text="Payment Settings"
                                Section="tblPayment" ResourceKey="Payment" IsExpanded="False"></dnn:SectionHead>
                            <table id="tblPayment" cellspacing="2" cellpadding="2" summary="Payment Setttings Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plCurrency" runat="server" Text="Currency:" ControlName="cboCurrency">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:DropDownList ID="cboCurrency" CssClass="NormalTextBox" runat="server" Width="325"
                                            DataTextField="text" DataValueField="value">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <dnn:Label ID="plProcessor" runat="server" Text="Payment Processor:" ControlName="cboProcessor">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:DropDownList ID="cboProcessor" CssClass="NormalTextBox" runat="server" Width="325"
                                            DataTextField="value" DataValueField="text">
                                        </asp:DropDownList><br>
                                        <asp:LinkButton ID="cmdProcessor" CssClass="CommandButton" runat="server" resourcekey="ProcessorWebSite"
                                            EnableViewState="False">Go To Payment Processor Website</asp:LinkButton></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plUserId" runat="server" Text="Processor UserId:" ControlName="txtUserId">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:TextBox ID="txtUserId" CssClass="NormalTextBox" runat="server" Width="325" MaxLength="50"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plPassword" runat="server" Text="Processor Password:" ControlName="txtPassword">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:TextBox ID="txtPassword" CssClass="NormalTextBox" runat="server" Width="325"
                                            MaxLength="50" TextMode="Password"></asp:TextBox></td>
                                </tr>
                            </table>
                            <br/>
                            <dnn:SectionHead ID="dshUsability" CssClass="Head" runat="server" Text="Usability Settings"
                                Section="tblUsability" ResourceKey="Usability" IsExpanded="False"></dnn:SectionHead>
                            <table id="tblUsability" cellspacing="2" cellpadding="2" summary="Usability Setttings Design Table" border="0" runat="server">
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plInlineEditor" runat="server" Text="Inline Editor Enabled?" ControlName="chkInlineEditor"></dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
                                        <asp:CheckBox ID="chkInlineEditor" runat="server" CssClass="Normal" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plControlPanelMode" runat="server" Text="Control Panel Mode:" ControlName="optControlPanelMode"></dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
						                <asp:radiobuttonlist id="optControlPanelMode" cssclass="Normal" runat="server" repeatdirection="Horizontal" repeatlayout="Flow">
							                <asp:listitem value="VIEW" resourcekey="ControlPanelModeView">View</asp:listitem>
							                <asp:listitem value="EDIT" resourcekey="ControlPanelModeEdit">Edit</asp:listitem>
						                </asp:radiobuttonlist>
			                        </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plControlPanelVisibility" runat="server" Text="Control Panel Visibility:" ControlName="optControlPanelVisibility"></dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
						                <asp:radiobuttonlist id="optControlPanelVisibility" cssclass="Normal" runat="server" repeatdirection="Horizontal" repeatlayout="Flow">
							                <asp:listitem value="MIN" resourcekey="ControlPanelVisibilityMinimized">Minimized</asp:listitem>
							                <asp:listitem value="MAX" resourcekey="ControlPanelVisibilityMaximized">Maximized</asp:listitem>
						                </asp:radiobuttonlist>
			                        </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plControlPanelSecurity" runat="server" Text="Control Panel Security:" ControlName="optControlPanelSecurity"></dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top" width="325">
						                <asp:radiobuttonlist id="optControlPanelSecurity" cssclass="Normal" runat="server" repeatdirection="Horizontal" repeatlayout="Flow">
							                <asp:listitem value="TAB" resourcekey="ControlPanelSecurityTab">Page Editors</asp:listitem>
							                <asp:listitem value="MODULE" resourcekey="ControlPanelSecurityModule">Module Editors</asp:listitem>
						                </asp:radiobuttonlist>
			                        </td>
                                </tr>
                            </table>
                            <br/>
                            <dnn:SectionHead ID="dshOther" CssClass="Head" runat="server" Text="Other Settings"
                                Section="tblOther" ResourceKey="Other" IsExpanded="False"></dnn:SectionHead>
                            <table id="tblOther" cellspacing="2" cellpadding="2" summary="Other Setttings Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plAdministrator" runat="server" Text="Administrator:" ControlName="cboAdministratorId">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" width="325">
                                        <asp:DropDownList ID="cboAdministratorId" CssClass="NormalTextBox" runat="server"
                                            Width="300" DataTextField="FullName" DataValueField="UserId">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plDefaultLanguage" runat="server" Text="Default Language:" ControlName="cboDefaultLanguage">
                                        </dnn:Label>
                                    </td>
                                    <td width="325">
                                        <asp:DropDownList ID="cboDefaultLanguage" CssClass="NormalTextBox" runat="server"
                                            Width="300">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plTimeZone" runat="server" Text="Portal TimeZone:" ControlName="cboTimeZone">
                                        </dnn:Label>
                                    </td>
                                    <td width="325">
                                        <asp:DropDownList ID="cboTimeZone" CssClass="NormalTextBox" runat="server" Width="300">
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
                            <br/>
                            <dnn:SectionHead ID="dshHost" CssClass="Head" runat="server" Text="Host Settings"
                                Section="tblHost" ResourceKey="HostSettings" IsExpanded="False"></dnn:SectionHead>
                            <table id="tblHost" cellspacing="2" cellpadding="2" summary="Host Settings Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plExpiryDate" runat="server" Text="Expiry Date:" ControlName="txtExpiryDate">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" width="325">
                                        <asp:TextBox ID="txtExpiryDate" CssClass="NormalTextBox" runat="server" Width="150"
                                            MaxLength="15"></asp:TextBox>
                                        <asp:HyperLink ID="cmdExpiryCalendar" CssClass="CommandButton" runat="server" resourcekey="Calendar">Calendar</asp:HyperLink>
                                        <asp:CompareValidator ID="valExpiryDate" CssClass="NormalRed" runat="server" ControlToValidate="txtExpiryDate"
                                            ErrorMessage="<br>Invalid expiry date!" Operator="DataTypeCheck" Type="Date"
                                            Display="Dynamic"></asp:CompareValidator></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plHostFee" runat="server" Text="Hosting Fee:" ControlName="txtHostFee">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" width="325">
								        <asp:textbox id="txtHostFee" cssclass="NormalTextBox" runat="server" maxlength="10" width="100"></asp:textbox>
                                        <asp:CompareValidator ID="valHostFee" runat="server" ControlToValidate="txtHostFee"
                                            CssClass="NormalRed" Display="Dynamic" ErrorMessage="Invalid fee, needs to be a currency value!" 
                                            ResourceKey="valHostFee.Error" Operator="DataTypeCheck" Type="Currency" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plHostSpace" runat="server" Text="Disk Space:" ControlName="txtHostSpace"/>
                                    </td>
                                    <td class="NormalTextBox" width="100">
                                        <asp:TextBox ID="txtHostSpace" CssClass="NormalTextBox" runat="server" MaxLength="6"
                                            Width="300"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plPageQuota" runat="server" Text="Page Quota:" ControlName="txtPageQuota"/>
                                    </td>
                                    <td class="NormalTextBox" width="100">
                                        <asp:TextBox ID="txtPageQuota" CssClass="NormalTextBox" runat="server" MaxLength="6"
                                            Width="300"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plUserQuota" runat="server" Text="User Quota:" ControlName="txtUserQuota"/>
                                    </td>
                                    <td class="NormalTextBox" width="100">
                                        <asp:TextBox ID="txtUserQuota" CssClass="NormalTextBox" runat="server" MaxLength="6"
                                            Width="300"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plSiteLogHistory" runat="server" Text="Site Log History:" ControlName="txtSiteLogHistory"/>
                                    </td>
                                    <td class="NormalTextBox" width="325">
                                        <asp:TextBox ID="txtSiteLogHistory" CssClass="NormalTextBox" runat="server" Width="300"
                                            MaxLength="3"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="SubHead" valign="top" width="150">
                                        <br>
                                        <br>
                                        <dnn:Label ID="plDesktopModules" runat="server" Text="Premium Modules:" ControlName="ctlDesktopModules"/>
                                    </td>
                                    <td class="NormalTextBox" width="325">
                                        <Portal:DualList ID="ctlDesktopModules" runat="server" ListBoxWidth="130" ListBoxHeight="130"
                                            DataValueField="DesktopModuleID" DataTextField="FriendlyName"></Portal:DualList>
                                    </td>
                                </tr>
                            </table>
                            <br/>
                            <dnn:SectionHead ID="dshSSL" CssClass="Head" runat="server" Text="SSL Settings"
                                Section="tblSSL" ResourceKey="SSLSettings" IsExpanded="False"></dnn:SectionHead>
                            <table id="tblSSL" cellspacing="2" cellpadding="2" summary="SSL Settings Design Table"
                                border="0" runat="server">
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plSSLEnabled" runat="server" Text="SSL Enabled?" ControlName="chkSSLEnabled">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top">
                                        <asp:CheckBox ID="chkSSLEnabled" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plSSLEnforced" runat="server" Text="SSL Enforced?" ControlName="chkSSLEnforced">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top">
                                        <asp:CheckBox ID="chkSSLEnforced" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plSSLURL" runat="server" Text="SSL URL:" ControlName="txtSSLURL">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top">
                                        <asp:TextBox ID="txtSSLURL" CssClass="NormalTextBox" runat="server" Width="325"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SubHead" width="150">
                                        <dnn:Label ID="plSTDURL" runat="server" Text="Standard URL:" ControlName="txtSTDURL">
                                        </dnn:Label>
                                    </td>
                                    <td class="NormalTextBox" valign="top">
                                        <asp:TextBox ID="txtSTDURL" CssClass="NormalTextBox" runat="server" Width="325"></asp:TextBox>
                                    </td>
                                </tr>
                           </table>
                        </td>
                    </tr>
                </table>
                <br/>
                <dnn:SectionHead ID="dshStylesheet" CssClass="Head" runat="server" Text="Stylesheet Editor"
                    Section="tblStylesheet" ResourceKey="StylesheetEditor" IncludeRule="True" IsExpanded="False">
                </dnn:SectionHead>
                <table id="tblStylesheet" cellspacing="0" cellpadding="2" width="525" summary="Stylesheet Editor Design Table"
                    border="0" runat="server">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtStyleSheet" CssClass="NormalTextBox" runat="server" Rows="20"
                                TextMode="MultiLine" Wrap="False" Columns="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="cmdSave" CssClass="CommandButton" runat="server" resourcekey="SaveStyleSheet"
                                EnableViewState="False">Save Style Sheet</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="cmdRestore" CssClass="CommandButton" runat="server" resourcekey="RestoreDefaultStyleSheet"
                                EnableViewState="False">Restore Default Style Sheet</asp:LinkButton></td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<p>
    <asp:LinkButton CssClass="CommandButton" ID="cmdUpdate" resourcekey="cmdUpdate" runat="server"
        Text="Update"></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton CssClass="CommandButton" ID="cmdCancel" resourcekey="cmdCancel" runat="server"
        Text="Cancel" CausesValidation="False" BorderStyle="none"></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton CssClass="CommandButton" ID="cmdDelete" resourcekey="cmdDelete" runat="server"
        Text="Delete" CausesValidation="False" BorderStyle="none"></asp:LinkButton>
</p>
