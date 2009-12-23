<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="c#" CodeBehind="HelpAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.Help" AutoEventWireup="True" %>

<table cellspacing="0" cellpadding="0" border="0" width="100%">
  <tr>
    <td valign="top" nowrap="nowrap"  class="ListContainer-Title"><dnn:label id="lblParentTitle" runat="server" visible="False" controlname="lblParentTitle"></dnn:label>
      Please Finish Me </td>
  </tr>
  <tr>
    <td align="center" colspan="2"><br />
      <asp:linkbutton id="btnSave" runat="server" cssclass="Normal">Update</asp:linkbutton>
      <br />
      <br />
    </td>
  </tr>
</table>
