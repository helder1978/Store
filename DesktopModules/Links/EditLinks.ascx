<%@ Control language="vb" CodeBehind="EditLinks.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Links.EditLinks" %>
<%@ Register TagPrefix="Portal" TagName="Tracking" Src="~/controls/URLTrackingControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table width="560" cellspacing="0" cellpadding="0" border="0" summary="Edit Links Design Table">
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td class="SubHead" width="160"><dnn:label id="plTitle" runat="server" controlname="txtTitle" suffix=":"></dnn:label></td>
        <td width="365">
            <asp:textbox id="txtTitle" cssclass="NormalTextBox" width="300" columns="30" maxlength="160" runat="server" />
            <br>
            <asp:requiredfieldvalidator id="valTitle" resourcekey="valTitle.ErrorMessage" display="Dynamic" errormessage="You Must Enter a Title For The Link" controltovalidate="txtTitle" runat="server" cssclass="NormalRed" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="160"><dnn:label id="plURL" runat="server" controlname="ctlURL" suffix=":"></dnn:label></td>
        <td width="365"><portal:url id="ctlURL" runat="server" width="250" shownewwindow="True" showusers="True" /></td>
    </tr>
    <tr height="10"><td colspan="2"></td></tr>
    <tr>
        <td class="SubHead" width="160"><dnn:label id="plDescription" runat="server" controlname="txtDescription" suffix=":"></dnn:label></td>
        <td width="365"><asp:textbox id="txtDescription" cssclass="NormalTextBox" width="300" columns="30" rows="5" textmode="MultiLine" maxlength="2000" runat="server" /></td>
    </tr>
    <tr>
        <td class="SubHead" width="160"><dnn:label id="plViewOrder" runat="server" controlname="txtViewOrder" suffix=":"></dnn:label></td>
        <td width="365">
            <asp:textbox id="txtViewOrder" cssclass="NormalTextBox" width="300" columns="30" maxlength="3" runat="server" />
            <br>
            <asp:regularexpressionvalidator id="valViewOrder" resourcekey="valViewOrder.ErrorMessage" runat="server" errormessage="View Order must be a Number or an Empty String" display="Dynamic" controltovalidate="txtViewOrder" validationexpression="^\d*$" cssclass="NormalRed" />
        </td>
    </tr>
</table>
<p>
    <asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="CommandButton" text="Update" borderstyle="none"></asp:linkbutton>&nbsp;
    <asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="CommandButton" text="Cancel" borderstyle="none" causesvalidation="False"></asp:linkbutton>&nbsp;
    <asp:linkbutton id="cmdDelete" resourcekey="cmdDelete" runat="server" cssclass="CommandButton" text="Delete" borderstyle="none" causesvalidation="False"></asp:linkbutton>
</p>
<portal:audit id="ctlAudit" runat="server" />
<br><br>
<portal:tracking id="ctlTracking" runat="server" />
