<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Admin.Packages.UnInstall" CodeFile="UnInstall.ascx.vb" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>
<table class="Settings" cellspacing="2" cellpadding="2" summary="Web Upload Design Table">
    <tr>
        <td valign="top">
            <asp:Panel ID="pnlUpload" Visible="True" CssClass="WorkPanel" runat="server">
                <table class="Settings" id="tblUpload" cellspacing="2" cellpadding="2" summary="Web Upload Design Table" runat="server">
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
                    <tr>
                        <td align="center"><dnn:CommandButton ID="cmdUninstall" runat="server" CssClass="CommandButton" imageurl="~/images/delete.gif" resourcekey="cmdUninstall"/></td>
                    </tr>
                    <tr>
                        <td align="left"><asp:Label ID="lblMessage" runat="server" CssClass="Normal" Width="500px" EnableViewState="False" /></td>
                    </tr>
                </table>
                <br />
                <dnn:CommandButton ID="cmdReturn1" runat="server" CssClass="CommandButton" imageurl="~/images/lt.gif" resourcekey="cmdReturn"/>
                <br />
                <table id="tblLogs" cellspacing="0" cellpadding="0" summary="Resource Upload Logs Table" runat="server" visible="False">
                    <tr><td><asp:Label ID="lblLogTitle" runat="server" resourcekey="LogTitle" CssClass="Head" /></td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr><td><asp:PlaceHolder ID="phPaLogs" runat="server" /></td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr><td><dnn:CommandButton ID="cmdReturn2" runat="server" CssClass="CommandButton" imageurl="~/images/lt.gif" resourcekey="cmdReturn"/></td></tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
