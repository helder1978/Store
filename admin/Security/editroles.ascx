<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Admin.Security.EditRoles"
    CodeFile="EditRoles.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/UrlControl.ascx" %>
<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Roles Design Table"
    border="0">
    <tr>
        <td width="560" valign="top">
            <asp:Panel ID="pnlBasic" runat="server" CssClass="WorkPanel" Visible="True">
                <dnn:SectionHead ID="dshBasic" CssClass="Head" runat="server" Text="Basic Settings"
                    Section="tblBasic" ResourceKey="BasicSettings" IncludeRule="True"></dnn:SectionHead>
                <table id="tblBasic" cellspacing="0" cellpadding="2" width="525" summary="Basic Settings Design Table"
                    border="0" runat="server">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblBasicSettingsHelp" CssClass="Normal" runat="server" resourcekey="BasicSettingsDescription"
                                EnableViewState="False"></asp:Label></td>
                    </tr>
                    <tr><td colspan="2" height="10"></td></tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plRoleName" runat="server" ResourceKey="RoleName" Suffix=":" ControlName="txtRoleName">
                            </dnn:Label>
                        </td>
                        <td align="left" width="325">
                            <asp:TextBox ID="txtRoleName" CssClass="NormalTextBox" runat="server" MaxLength="50"
                                Columns="30" Width="325"></asp:TextBox><asp:Label ID="lblRoleName" Visible="False"
                                    runat="server" CssClass="Normal"></asp:Label><asp:RequiredFieldValidator ID="valRoleName"
                                        CssClass="NormalRed" runat="server" resourcekey="valRoleName" ControlToValidate="txtRoleName"
                                        ErrorMessage="<br>You Must Enter a Valid Name" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plDescription" runat="server" ResourceKey="Description" Suffix=":"
                                ControlName="txtDescription"></dnn:Label>
                        </td>
                        <td width="325">
                            <asp:TextBox ID="txtDescription" CssClass="NormalTextBox" runat="server" MaxLength="1000"
                                Columns="30" Width="325" TextMode="MultiLine" Height="84px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plRoleGroups" runat="server" Suffix="" ControlName="cboRoleGroups"></dnn:Label>
                        </td>
                        <td width="325">
                            <asp:DropDownList ID="cboRoleGroups" CssClass="NormalTextBox" runat="server">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plIsPublic" runat="server" ResourceKey="PublicRole" ControlName="chkIsPublic">
                            </dnn:Label>
                        </td>
                        <td width="325">
                            <asp:CheckBox ID="chkIsPublic" runat="server"></asp:CheckBox></td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plAutoAssignment" runat="server" ResourceKey="AutoAssignment" ControlName="chkAutoAssignment">
                            </dnn:Label>
                        </td>
                        <td width="325">
                            <asp:CheckBox ID="chkAutoAssignment" runat="server"></asp:CheckBox></td>
                    </tr>
                </table>
                <br/>
                <dnn:SectionHead ID="dshAdvanced" CssClass="Head" runat="server" Text="Advanced Settings"
                    Section="tblAdvanced" ResourceKey="AdvancedSettings" IncludeRule="True" IsExpanded="False">
                </dnn:SectionHead>
                <table id="tblAdvanced" cellspacing="0" cellpadding="2" width="525" summary="Advanced Settings Design Table"
                    border="0" runat="server">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblAdvancedSettingsHelp" CssClass="Normal" runat="server" resourcekey="AdvancedSettingsHelp"
                                EnableViewState="False"></asp:Label></td>
                    </tr>
                    <tr height="10"><td colspan="2"></td></tr>
                    <tr>
                        <td colspan="2"><asp:Label ID="lblProcessorWarning" visible="false" CssClass="Normal" runat="server" resourcekey="ProcessorWarning" EnableViewState="False"></asp:Label></td>
                    </tr>
                    <tr height="10"><td colspan="2"></td></tr>
                    <tr id="trServiceFee" runat="server" valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plServiceFee" runat="server" ResourceKey="ServiceFee" Suffix=":" ControlName="txtServiceFee">
                            </dnn:Label>
                        </td>
                        <td width="325">
                            <asp:TextBox ID="txtServiceFee" CssClass="NormalTextBox" runat="server" MaxLength="50"
                                Columns="30" Width="100"></asp:TextBox><asp:CompareValidator ID="valServiceFee1"
                                    CssClass="NormalRed" runat="server" resourcekey="valServiceFee1" ControlToValidate="txtServiceFee"
                                    ErrorMessage="<br>Service Fee Value Entered Is Not Valid" Display="Dynamic" Type="Currency"
                                    Operator="DataTypeCheck"></asp:CompareValidator><asp:CompareValidator ID="valServiceFee2"
                                        CssClass="NormalRed" runat="server" resourcekey="valServiceFee2" ControlToValidate="txtServiceFee"
                                        ErrorMessage="<br>Service Fee Must Be Greater Than or Equal to Zero" Display="Dynamic"
                                        Operator="GreaterThanEqual" ValueToCompare="0"></asp:CompareValidator></td>
                    </tr>
                    <tr id="trBillingPeriod" valign="top" runat="server">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plBillingPeriod" runat="server" ResourceKey="BillingPeriod" Suffix=":"
                                ControlName="txtBillingPeriod"></dnn:Label>
                        </td>
                        <td width="325">
                            <asp:TextBox ID="txtBillingPeriod" CssClass="NormalTextBox" runat="server" MaxLength="50"
                                Columns="30" Width="100"></asp:TextBox>&nbsp;&nbsp;
                            <asp:DropDownList ID="cboBillingFrequency" CssClass="NormalTextBox" runat="server"
                                Width="100px" DataValueField="value" DataTextField="text">
                            </asp:DropDownList><asp:CompareValidator ID="valBillingPeriod1" CssClass="NormalRed"
                                runat="server" resourcekey="valBillingPeriod1" ControlToValidate="txtBillingPeriod"
                                ErrorMessage="<br>Billing Period Value Entered Is Not Valid" Display="Dynamic"
                                Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator><asp:CompareValidator
                                    ID="valBillingPeriod2" CssClass="NormalRed" runat="server" resourcekey="valBillingPeriod2"
                                    ControlToValidate="txtBillingPeriod" ErrorMessage="<br>Billing Period Must Be Greater Than or Equal to Zero"
                                    Display="Dynamic" Operator="GreaterThan" ValueToCompare="0"></asp:CompareValidator></td>
                    </tr>
                    <tr id="trTrialFee" valign="top" runat="server">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plTrialFee" runat="server" ResourceKey="TrialFee" Suffix=":" ControlName="txtTrialFee">
                            </dnn:Label>
                        </td>
                        <td width="325">
                            <asp:TextBox ID="txtTrialFee" CssClass="NormalTextBox" runat="server" MaxLength="50"
                                Columns="30" Width="100"></asp:TextBox><asp:CompareValidator ID="valTrialFee1" CssClass="NormalRed"
                                    runat="server" resourcekey="valTrialFee1" ControlToValidate="txtTrialFee" ErrorMessage="<br>Trial Fee Value Entered Is Not Valid"
                                    Display="Dynamic" Type="Currency" Operator="DataTypeCheck"></asp:CompareValidator><asp:CompareValidator
                                        ID="valTrialFee2" CssClass="NormalRed" runat="server" resourcekey="valTrialFee2"
                                        ControlToValidate="txtTrialFee" ErrorMessage="<br>Trial Fee Must Be Greater Than Zero"
                                        Display="Dynamic" Operator="GreaterThanEqual" ValueToCompare="0"></asp:CompareValidator></td>
                    </tr>
                    <tr id="trTrialPeriod" valign="top" runat="server">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plTrialPeriod" runat="server" ResourceKey="TrialPeriod" Suffix=":"
                                ControlName="txtTrialPeriod"></dnn:Label>
                        </td>
                        <td width="325">
                            <asp:TextBox ID="txtTrialPeriod" CssClass="NormalTextBox" runat="server" MaxLength="50"
                                Columns="30" Width="100"></asp:TextBox>&nbsp;&nbsp;
                            <asp:DropDownList ID="cboTrialFrequency" CssClass="NormalTextBox" runat="server"
                                Width="100px" DataValueField="value" DataTextField="text">
                            </asp:DropDownList><asp:CompareValidator ID="valTrialPeriod1" CssClass="NormalRed"
                                runat="server" resourcekey="valTrialPeriod1" ControlToValidate="txtTrialPeriod"
                                ErrorMessage="<br>Trial Period Value Entered Is Not Valid" Display="Dynamic"
                                Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator><asp:CompareValidator
                                    ID="valTrialPeriod2" CssClass="NormalRed" runat="server" resourcekey="valTrialPeriod2"
                                    ControlToValidate="txtTrialPeriod" ErrorMessage="<br>Trial Period Must Be Greater Than Zero"
                                    Display="Dynamic" Operator="GreaterThan" ValueToCompare="0"></asp:CompareValidator></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plRSVPCode" runat="server" ControlName="txtRSVPCode"></dnn:Label>
                        </td>
                        <td width="325">
                            <asp:TextBox ID="txtRSVPCode" CssClass="NormalTextBox" runat="server" MaxLength="50" Columns="30" Width="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plRSVPLink" runat="server" ControlName="txtRSVPLink"></dnn:Label>
                        </td>
                        <td width="325">
                            <asp:TextBox ID="txtRSVPLink" CssClass="NormalTextBox" runat="server" Width="325" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="SubHead" width="150" valign="top">
                            <dnn:Label ID="plIcon" Text="Icon:" runat="server" ControlName="ctlIcon"></dnn:Label>
                        </td>
                        <td width="325">
                            <dnn:Url ID="ctlIcon" runat="server" Width="325" ShowUrls="False" ShowTabs="False"
                                ShowLog="False" ShowTrack="False" Required="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<p>
    <asp:LinkButton ID="cmdUpdate" resourcekey="cmdUpdate" runat="server" CssClass="CommandButton"
        Text="Update" BorderStyle="none" />
    &nbsp;
    <asp:LinkButton ID="cmdCancel" resourcekey="cmdCancel" runat="server" CssClass="CommandButton"
        Text="Cancel" CausesValidation="False" BorderStyle="none" />
    &nbsp;
    <asp:LinkButton ID="cmdDelete" resourcekey="cmdDelete" runat="server" CssClass="CommandButton"
        Text="Delete" CausesValidation="False" BorderStyle="none" />
    &nbsp;
    <asp:LinkButton ID="cmdManage" resourcekey="cmdManage" runat="server" CssClass="CommandButton"
        Text="Manage Users" CausesValidation="False" />
</p>
