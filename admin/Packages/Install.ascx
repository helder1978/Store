<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Admin.Packages.Install" CodeFile="Install.ascx.vb" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>

<asp:Wizard ID="wizInstall" runat="server"  DisplaySideBar="false" ActiveStepIndex="0"
    CellPadding="5" CellSpacing="5" 
    DisplayCancelButton="True"
    CancelButtonType="Link"
    StartNextButtonType="Link"
    StepNextButtonType="Link" 
    FinishCompleteButtonType="Link"
    >
    <StepStyle VerticalAlign="Top" />
    <NavigationButtonStyle CssClass="CommandButton" BorderStyle="None" BackColor="Transparent" />
    <HeaderTemplate>
        <asp:Label ID="lblTitle" CssClass="Head" runat="server"><% =GetText("Title") %></asp:Label><br /><br />
        <asp:Label ID="lblHelp" CssClass="WizardText" runat="server"><% =GetText("Help") %></asp:Label>
    </HeaderTemplate>
    <WizardSteps>
        <asp:WizardStep ID="Step0" runat="Server" Title="Introduction" StepType="Start" AllowReturn="false">
            <table class="Settings" id="tblUpload" cellspacing="2" cellpadding="2" summary="Packages Install Design Table" runat="server" Width="500px" >
                <tr>
                    <td align="center">
                        <label style="display: none" for="<%=cmdBrowse.ClientID%>">Browse Files</label>
                        <input id="cmdBrowse" type="file" size="50" name="cmdBrowse" runat="server" />&nbsp;&nbsp;
                    </td>
                </tr>
                <tr><td align="left" colspan="2"><asp:Label ID="lblLoadMessage" runat="server" EnableViewState="False" CssClass="NormalRed" /></td></tr>
                <tr><td><asp:PlaceHolder ID="phLoadLogs" runat="server" /></td></tr>
            </table>
        </asp:WizardStep>
        <asp:WizardStep ID="Step1" runat="Server" Title="PackageInfo" StepType="Step" AllowReturn="false">
            <table class="Settings" cellspacing="2" cellpadding="2" summary="Packages Install Design Table">
                <tr>
                    <td>
                        <dnn:propertyeditorcontrol id="ctlPackage" runat="Server"
                            SortMode="SortOrderAttribute"
                            ErrorStyle-cssclass="NormalRed"
                            labelstyle-cssclass="SubHead" 
                            helpstyle-cssclass="Help" 
                            editcontrolstyle-cssclass="NormalTextBox" 
                            labelwidth="150px" 
                            editcontrolwidth="450px" 
                            width="600px"/>
                    </td>
                </tr>
                <tr><td><asp:CheckBox ID="chkAcceptLicense" runat="server" resourcekey="AcceptLicense" CssClass="SubHead" TextAlign="Left" /></td></tr>
                <tr><td align="left" colspan="2"><asp:Label ID="lblAcceptMessage" runat="server" EnableViewState="False" CssClass="NormalRed" /></td></tr>
                <tr><td><asp:PlaceHolder ID="phAcceptLogs" runat="server" /></td></tr>
            </table>
        </asp:WizardStep>
        <asp:WizardStep ID="Step2" runat="Server" Title="InstallResults" StepType="Finish">
            <table class="Settings" cellspacing="2" cellpadding="2" summary="Packages Install Design Table">
                <tr><td align="left" colspan="2"><asp:Label ID="lblInstallMessage" runat="server" EnableViewState="False" CssClass="NormalRed" /></td></tr>
                <tr><td><asp:PlaceHolder ID="phInstallLogs" runat="server" /></td></tr>
            </table>
        </asp:WizardStep>
    </WizardSteps>
</asp:Wizard>
