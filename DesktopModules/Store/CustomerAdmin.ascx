<%@ Control language="c#" CodeBehind="CustomerAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CustomerAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="3" border="0" align="center" width="100%">
  <tr>
    <td colspan="2" nowrap="nowrap"><dnn:label id="lblParentTitle" runat="server" visible="False" controlname="lblParentTitle"></dnn:label>
    </td>
  </tr>
  <tr>
    <td><table align="center">
        <tr>
          <td width="120" class="NormalBold" nowrap="nowrap" valign="top"><dnn:label id="lblOrderNumber" runat="server" controlname="lblOrderNumber" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:TextBox ID="tbOrderNumber" Width="100" runat="server" OnTextChanged="tbOrderNumber_TextChanged" CssClass="NormalTextBox"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td width="120" class="NormalBold" nowrap="nowrap" valign="top"><dnn:label id="lblCustomers" runat="server" controlname="lblCustomers" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:DropDownList ID="lstCustomers" Runat="server"></asp:DropDownList>
          </td>
        </tr>
        <tr>
          <td width="120" class="NormalBold" nowrap="nowrap" valign="top"><dnn:label id="lblOrderStatus" runat="server" controlname="lblOrderStatus" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:DropDownList ID="lstOrderStatus" Runat="server"></asp:DropDownList>
          </td>
        </tr>
        <tr>
          <td colspan="2" align="center"><asp:Label ID="noOrdersFound" resourcekey="noOrdersFound" Text="Order number not found" ForeColor="red" runat="server" Visible="false"></asp:Label>
            <br />
            <asp:Button ID="btnSearch" resourcekey="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />
          </td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td colspan="2">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="2"><asp:placeholder id="plhOrders" runat="server" Visible="False" />
    </td>
  </tr>
</table>
