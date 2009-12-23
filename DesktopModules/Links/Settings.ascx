<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Settings.ascx.vb" Inherits="DotNetNuke.Modules.Links.Settings" TargetSchema="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<table cellspacing="0" cellpadding="2" border="0" summary="Edit Links Design Table">
    <tr>
        <td class="SubHead" width="150"><dnn:label id="plControl" runat="server" controlname="optControl" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:radiobuttonlist id="optControl" runat="server" cssclass="NormalTextBox" repeatdirection="Horizontal">
                <asp:listitem resourcekey="List" value="L">List</asp:listitem>
                <asp:listitem resourcekey="Dropdown" value="D">Dropdown</asp:listitem>
            </asp:radiobuttonlist>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150"><dnn:label id="ploptView" runat="server" controlname="optView" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:radiobuttonlist id="optView" runat="server" cssclass="NormalTextBox" repeatdirection="Horizontal">
                <asp:listitem resourcekey="Vertical" value="V">Vertical</asp:listitem>
                <asp:listitem resourcekey="Horizontal" value="H">Horizontal</asp:listitem>
            </asp:radiobuttonlist>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150"><dnn:label id="plInfo" runat="server" controlname="optInfo" suffix=":"></dnn:label></td>
        <td valign="bottom" >
            <asp:radiobuttonlist id="optInfo" runat="server" cssclass="NormalTextBox" repeatdirection="Horizontal">
                <asp:listitem resourcekey="Yes" value="Y">Yes</asp:listitem>
                <asp:listitem resourcekey="No" value="N">No</asp:listitem>
            </asp:radiobuttonlist>
        </td>
    </tr>
	<tr>
		<td class="SubHead" width="150"><dnn:label id="plNoWrap" runat="server" controlname="optNoWrap" suffix=":"></dnn:label></td>
		<td valign="bottom">
			<asp:radiobuttonlist id="optNoWrap" runat="server" cssclass="NormalTextBox" repeatdirection="Horizontal">
                <asp:listitem resourcekey="Wrap" value="W">Wrap</asp:listitem>
                <asp:listitem resourcekey="NoWrap" value="NW">No Wrap</asp:listitem>
            </asp:radiobuttonlist>
        </td>
    </tr>
    <tr>
        <td class="SubHead" width="150"><dnn:label id="plIcon" runat="server" controlname="ctlIcon" suffix=":"></dnn:label></td>
        <td width="365"><portal:url id="ctlIcon" runat="server" width="250" showurls="False" showtabs="False" showlog="False" showtrack="False" required="False" /></td>
    </tr>
</table>
