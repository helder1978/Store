<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="c#" CodeBehind="CustomerProfile.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CustomerProfile" AutoEventWireup="True" %>

<span class="ListContainer-Title">
<dnn:label id="lblParentTitle" runat="server" visible="False" controlname="lblParentTitle"></dnn:label>
</span>
<asp:Panel ID="pnlLoginMessage" Runat="server">
  <TABLE align="center">
    <TR>
      <TD><asp:Label id="lblLoginMessage" Runat="server" resourcekey="lblLoginMessage" CssClass="NormalRed">Please login to view profile settings.</asp:Label>
      </TD>
    </TR>
  </TABLE>
  </asp:Panel>
<asp:Panel ID="pnlAddressProvider" Runat="server" Visible="False">
  <asp:placeholder id="plhAddressProvider" runat="server"></asp:placeholder>
  </asp:Panel>
