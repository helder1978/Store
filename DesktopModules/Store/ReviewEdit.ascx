<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ReviewEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ReviewEdit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="5" width="80%" align="center" border="0">
  <tr valign="top">
    <td class="NormalBold" width="33%" nowrap><dnn:label id=labelUserName suffix=":" controlname="labelUserName" runat="server"></dnn:label>
    </td>
    <td class="Normal" width="67%"><asp:textbox id="txtUserName" MaxLength="50" Width="200" Runat="server" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" width="33%" nowrap><dnn:label id="labelRating" suffix=":" controlname="labelRating" runat="server"></dnn:label>
    </td>
    <td class="Normal" width="67%" nowrap><table cellpadding="0" cellspacing="0" border="0">
        <tr>
          <td width="75"><asp:dropdownlist id="cmbRating" Runat="server" Width="50px" AutoPostBack="true" onselectedindexchanged="cmbRating_SelectedIndexChanged">
              <asp:ListItem Value="5" Selected="True">5</asp:ListItem>
              <asp:ListItem Value="4">4</asp:ListItem>
              <asp:ListItem Value="3">3</asp:ListItem>
              <asp:ListItem Value="2">2</asp:ListItem>
              <asp:ListItem Value="1">1</asp:ListItem>
            </asp:dropdownlist>
          </td>
          <td><asp:PlaceHolder ID="plhRating" Runat="server"></asp:PlaceHolder>
          </td>
        </tr>
      </table></td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" width="33%" nowrap><dnn:label id="labelComments" suffix=":" controlname="labelComments" runat="server"></dnn:label>
    </td>
    <td class="Normal" width="67%"><asp:textbox id="txtComments" MaxLength="500" Width="250" Runat="server" TextMode="MultiLine" Rows="5" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" width="33%" nowrap><dnn:label id="labelAuthorized" suffix=":" controlname="labelAuthorized" runat="server" Visible="False"></dnn:label>
    </td>
    <td class="Normal" width="67%"><asp:checkbox id="chkAuthorized" Runat="server" Visible="False"></asp:checkbox>
    </td>
  </tr>
  <tr valign="top">
    <td colspan="2" align="center"><asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" BorderStyle="None" CssClass="CommandButton">Update</asp:linkbutton>
      &nbsp;
      <asp:linkbutton id="cmdCancel" runat="server" resourcekey="cmdCancel" BorderStyle="None" CssClass="CommandButton" CausesValidation="False">Cancel</asp:linkbutton>
      &nbsp;
      <asp:linkbutton id="cmdDelete" runat="server" resourcekey="cmdDelete" BorderStyle="None" CssClass="CommandButton" CausesValidation="False" Visible="False">Delete</asp:linkbutton>
    </td>
  </tr>
</table>
