<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="wc" Namespace="DotNetNuke.UI.WebControls" Assembly="CountryListBox" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AddressEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.AddressEdit" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<table border="0" align="center" cellspacing="0" cellpadding="0" class="AddressEditMasterTABLE">
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblDescription" runat="server" controlname="lblDescription" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:TextBox ID="txtDescription" Runat="server" Width="300" MaxLength="100" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblName" runat="server" controlname="lblName" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:TextBox ID="txtName" Runat="server" Width="300" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblAddress1" runat="server" controlname="lblAddress1" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:TextBox ID="txtAddress1" Runat="server" Width="300" MaxLength="100" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblAddress2" runat="server" controlname="lblAddress2" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:TextBox ID="txtAddress2" Runat="server" Width="300" MaxLength="100" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblCity" runat="server" controlname="lblCity" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:TextBox ID="txtCity" Runat="server" Width="300" MaxLength="100" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblCountry" runat="server" controlname="lblCountry" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <wc:CountryListBox TestIP="" LocalhostCountryCode="US" id="cboCountry" CssClass="NormalTextBox" Width="250px" DataValueField="Value" DataTextField="Text" AutoPostBack="True" runat="server"></wc:CountryListBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblRegion" runat="server" controlname="lblRegion" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:DropDownList id="cboRegion" runat="server" CssClass="NormalTextBox" Width="250px" DataValueField="Value" DataTextField="Text"></asp:DropDownList>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblPostalCode" runat="server" controlname="lblPostalCode" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:TextBox ID="txtPostalCode" Runat="server" Width="100" MaxLength="10" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblPhone1" runat="server" controlname="lblPhone1" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:TextBox ID="txtPhone1" Runat="server" Width="100" MaxLength="20" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblPhone2" runat="server" controlname="lblPhone2" suffix=" :"></dnn:label>
    </td>
    <td class="Normal" class="AddressEditMasterTABLERLeftCell">
    <asp:TextBox ID="txtPhone2" Runat="server" Width="100" MaxLength="20" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="AddressEditMasterTABLERightCell"><dnn:label id="lblPrimary" runat="server" controlname="lblPrimary" suffix=" :"></dnn:label>
    </td>
    <td class="AddressEditMasterTABLERLeftCell"><asp:Checkbox ID="chkPrimary" Runat="server"></asp:Checkbox>
    </td>
  </tr>
  <tr>
    <td colspan="2" align="center"><asp:linkbutton id="cmdUpdate" CssClass="CommandButton" runat="server" BorderStyle="None" resourcekey="cmdUpdate" onclick="cmdUpdate_Click">Update</asp:linkbutton>
      <asp:linkbutton id="cmdCancel" CssClass="CommandButton" runat="server" CausesValidation="False" BorderStyle="None" resourcekey="cmdCancel" onclick="cmdCancel_Click">Cancel</asp:linkbutton>
      <asp:linkbutton id="cmdDelete" CssClass="CommandButton" runat="server" CausesValidation="False" BorderStyle="None" Visible="False" resourcekey="cmdDelete" onclick="cmdDelete_Click">Delete</asp:linkbutton>
    </td>
  </tr>
</table>
