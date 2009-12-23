<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Inherits="DotNetNuke.Modules.Admin.Users.BulkEmail" CodeFile="BulkEmail.ascx.vb"
    Language="vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" TagName="URLControl" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<table class="Settings" cellspacing="2" cellpadding="2" summary="Edit Roles Design Table"
    border="0">
    <tr>
        <td width="560" valign="top">
            <asp:Panel ID="pnlSettings" runat="server" CssClass="WorkPanel" Visible="True">
                <dnn:SectionHead ID="dshBasic" CssClass="Head" runat="server" IncludeRule="True"
                    ResourceKey="BasicSettings" Section="tblBasic" />
                <table id="tblBasic" cellspacing="0" cellpadding="2" width="525" summary="Basic Settings Design Table"
                    border="0" runat="server">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblBasicSettingsHelp" CssClass="Normal" runat="server" resourcekey="BasicSettingsDescription"
								enableviewstate="False" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150"><br />
                            <dnn:Label ID="plRoles" runat="server" ControlName="chkRoles" Suffix=":"></dnn:Label>
                        </td>
                        <td align="left" width="325">
                            <dnn:RolesSelectionGrid runat="server" ID="dgSelectedRoles" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
							<dnn:label id="plEmail" runat="server" controlname="txtEmail" suffix=":" /></td>
                        <td width="325">
                            <asp:TextBox ID="txtEmail" CssClass="NormalTextBox" runat="server" Width="325" TextMode="MultiLine"
								rows="3" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
							<dnn:label id="plFrom" runat="server" controlname="txtFrom" suffix=":" /></td>
                        <td width="325">
                            <asp:TextBox ID="txtFrom" CssClass="NormalTextBox" runat="server" Width="325" Columns="40" MaxLength="100" />
                            <asp:RegularExpressionValidator ID="revEmailAddress" runat="server" resourcekey="revEmailAddress.ErrorMessage"
                                ErrorMessage="RegularExpressionValidator" CssClass="NormalRed" ControlToValidate="txtFrom"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label ID="plSubject" runat="server" ControlName="txtSubject" Suffix=":" /></td>
                        <td width="325">
                            <asp:TextBox ID="txtSubject" CssClass="NormalTextBox" runat="server" Width="325" Columns="40" MaxLength="100" /></td>
                    </tr>
                </table>
                <br />
                <dnn:SectionHead ID="dshMessage" CssClass="Head" runat="server" IncludeRule="True"
                    ResourceKey="Message" Section="tblMessage" Text="Message" />
                <table id="tblMessage" cellspacing="0" cellpadding="2" width="525" summary="Message Design Table"
                    border="0" runat="server">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessageHelp" CssClass="Normal" runat="server" resourcekey="MessageDescription"
								enableviewstate="False" /></td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2">
                            <dnn:TextEditor ID="teMessage" runat="server" Width="550" TextRenderMode="Raw" HtmlEncode="False"
								defaultmode="Rich" height="350" choosemode="True" chooserender="False" /></td>
                    </tr>
                </table>
                <br />
				<dnn:sectionhead id="dshAdvanced" cssclass="Head" runat="server" includerule="True" resourcekey="AdvancedSettings"
					section="tblAdvanced" text="Advanced Settings" isexpanded="False" />
                <table id="tblAdvanced" cellspacing="0" cellpadding="2" width="525" summary="Message Design Table"
                    border="0" runat="server">
                    <tr>
                        <td colspan="2">
                            <asp:Label id="lblAdvancedSettingsHelp" CssClass="Normal" runat="server" resourcekey="AdvancedSettingsHelp"
                                EnableViewState="False" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label id="plAttachment" runat="server" ControlName="ctlAttachment" Suffix=":" /></td>
                        <td width="325">
                            <dnn:URLControl id="ctlAttachment" runat="server" Required="False" ShowUpLoad="true"
                                ShowTrack="False" ShowLog="False" ShowTabs="False" ShowUrls="False" /></td>
                    </tr>
						  <tr valign="top">
							   <td class="SubHead" width="150">
								   <dnn:label id="plReplaceTokens" runat="server" controlname="chkReplaceTokens" suffix=":" /></td>
							   <td width="325">
								   <asp:CheckBox id="chkReplaceTokens" cssclass="Normal" runat="server" Checked="true" /></td>
						  </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label id="plPriority" runat="server" ControlName="cboPriority" Suffix=":" /></td>
                        <td width="325">
                            <asp:DropDownList id="cboPriority" CssClass="NormalTextBox" runat="server" Width="100">
                                <asp:ListItem resourcekey="High" Value="1" />
                                <asp:ListItem resourcekey="Normal" Value="2" Selected="True" />
                                <asp:ListItem resourcekey="Low" Value="3" />
                            </asp:DropDownList></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label id="plSendMethod" runat="server" ControlName="cboSendMethod" Suffix=":" /></td>
                        <td width="325">
                            <asp:DropDownList id="cboSendMethod" CssClass="NormalTextBox" runat="server" Width="325px">
                                <asp:ListItem resourcekey="SendTo" Value="TO" Selected="True" />
                                <asp:ListItem resourcekey="SendBCC" Value="BCC" />
                            </asp:DropDownList></td>
                    </tr>
                    <tr valign="top">
                        <td class="SubHead" width="150">
                            <dnn:Label id="plSendAction" runat="server" ControlName="optSendAction" Suffix=":" /></td>
                        <td width="325">
                            <asp:RadioButtonList id="optSendAction" CssClass="Normal" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem resourcekey="Synchronous" Value="S" />
                                <asp:ListItem resourcekey="Asynchronous" Value="A" Selected="True" />
                            </asp:RadioButtonList></td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<p>
    <asp:LinkButton id="cmdSend" resourcekey="cmdSend" Text="Send Email" runat="server" CssClass="CommandButton" />
    <asp:LinkButton id="cmdPreview" resourcekey="cmdPreview" runat="server" CssClass="CommandButton" />
</p>
<p>
    <asp:Panel ID="pnlPreview" runat="server" EnableViewState="false" Visible="false">
        <table>
            <tr>
                <td style="width:100px; vertical-align:top"><asp:Label ID="label3" runat="server" CssClass="Head" resourcekey="Preview" >Preview</asp:Label></td>
                <td style="width:460px;"></td>
            </tr>
            <tr>
                <td style="width:100px; vertical-align:top"><asp:Label ID="label1" runat="server" CssClass="SubHead" resourcekey="plSubject" ></asp:Label></td>
                <td style="width:460px;"><asp:Label id="lblPreviewSubject" runat="server" CssClass="Normal"></asp:Label></td>
            </tr>
            <tr>
                <td style="width:100px; vertical-align:top"><asp:Label ID="label2" runat="server" CssClass="SubHead" resourcekey="Message" ></asp:Label></td>
                <td style="width:460px;"><asp:Label id="lblPreviewBody" runat="server" CssClass="Normal"></asp:Label></td>
            </tr>
        </table>
    </asp:Panel>
</p>