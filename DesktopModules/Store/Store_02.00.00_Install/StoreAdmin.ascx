<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Control language="c#" CodeBehind="StoreAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.StoreAdmin" AutoEventWireup="True" %>

<dnn:label id="lblParentTitle" ResourceKey="lblParentTitle" runat="server" visible="False" controlname="lblParentTitle"></dnn:label>
<table cellspacing="0" cellpadding="0" border="0" width="100%">
  <tr>
    <td><table align="center">
        <tr>
          <td valign="top" class="NormalBold" width="150" nowrap="nowrap"><dnn:label id="lblStoreName" runat="server" controlname="txtStoreName" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:textbox id="txtStoreName" runat="server" width="300" CssClass="NormalTextBox"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td valign="top" class="NormalBold" width="150" nowrap="nowrap"><dnn:label id="lblDescription" runat="server" controlname="txtDescription" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:textbox id="txtDescription" runat="server" textmode="multiline" rows="4" width="300" CssClass="NormalTextBox"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td valign="top" class="NormalBold" width="150" nowrap="nowrap"><dnn:label id="lblKeywords" runat="server" controlname="txtKeywords" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:textbox id="txtKeywords" runat="server" textmode="multiline" rows="4" width="300" CssClass="NormalTextBox"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td valign="top" class="NormalBold" width="150" nowrap="nowrap"><dnn:label id="lblEmail" runat="server" controlname="txtEmail" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:textbox id="txtEmail" runat="server" width="300" CssClass="NormalTextBox"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td class="NormalBold" nowrap="nowrap" valign="top" width="150"><dnn:label id="lblCurrencySymbol" runat="server" controlname="txtCurrencySymbol" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:textbox id="txtCurrencySymbol" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td class="NormalBold" nowrap="nowrap" valign="top" width="150"><dnn:label id="lblUsePortalTemplates" runat="server" controlname="chkUsePortalTemplates" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:checkbox id="chkUsePortalTemplates" runat="server"></asp:checkbox>
          </td>
        </tr>
        <tr>
          <td valign="top" class="NormalBold" width="150" nowrap="nowrap"><dnn:label id="lblStorePageID" runat="server" controlname="lstStorePageID" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:dropdownlist id="lstStorePageID" runat="server" cssclass="Normal" width="200" autopostback="False"></asp:dropdownlist>
          </td>
        </tr>
        <tr>
          <td valign="top" class="NormalBold" width="150" nowrap="nowrap"><dnn:label id="lblShoppingCartPageID" runat="server" controlname="lstShoppingCartPageID" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:dropdownlist id="lstShoppingCartPageID" runat="server" cssclass="Normal" width="200" autopostback="False"></asp:dropdownlist>
          </td>
        </tr>
        <tr>
          <td class="NormalBold" nowrap="nowrap" valign="top" width="150"><dnn:label id="lblAuthorizeCancel" runat="server" controlname="chkAuthorizeCancel" suffix=":"></dnn:label>
          </td>
          <td valign="top"><asp:checkbox id="chkAuthorizeCancel" runat="server"></asp:checkbox>
          </td>
        </tr>
        <%--
                <tr>
                    <td colspan="2" height="20">
                    </td>
                </tr>
                <tr>
					<td valign="top" class="NormalBold" width="150" nowrap>
						<dnn:label id="lblAddressProvider" runat="server" suffix=":" controlname="lblAddressProvider"></dnn:label>
					</td>
					<td valign="top">
						<asp:dropdownlist id="lstAddressProviders" runat="server" cssclass="Normal" width="200" autopostback="True"></asp:dropdownlist>
					</td>
				</tr>
				--%>
        <tr>
          <td colspan="2" height="20"></td>
        </tr>
        <tr>
          <td valign="top" class="NormalBold" width="150" nowrap="nowrap"><dnn:label id="lblGateway" runat="server" suffix=":" controlname="lblGateway"></dnn:label>
          </td>
          <td valign="top"><asp:dropdownlist id="lstGateway" runat="server" cssclass="Normal" width="200" autopostback="True" onselectedindexchanged="lstGateway_SelectedIndexChanged"></asp:dropdownlist>
          </td>
        </tr>
        <tr>
          <td colspan="2"><asp:placeholder id="plhGateway" runat="server"></asp:placeholder>
          </td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td align="center"><br />
      <asp:linkbutton id="btnSave" runat="server" cssclass="Normal" resourcekey="btnSave" onclick="btnSave_Click">Update</asp:linkbutton>
      <br />
      <br />
    </td>
  </tr>
  <tr>
    <td><dnn:sectionhead id="dshTaxProvider" ResourceKey="dshTaxProvider" runat="server" cssclass="NormalBold" text="Tax Administration" section="tblTaxProvider" includerule="false" isexpanded="false"></dnn:sectionhead>
      <table id="tblTaxProvider" runat="server" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
          <td><asp:placeholder id="plhTaxProvider" runat="server" />
          </td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td><dnn:sectionhead id="dshShippingProvider" ResourceKey="dshShippingProvider" runat="server" cssclass="NormalBold" text="Shipping Administration" section="tblShippingProvider" includerule="false" isexpanded="false"></dnn:sectionhead>
      <table id="tblShippingProvider" runat="server" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
          <td><asp:placeholder id="plhShippingProvider" runat="server" />
          </td>
        </tr>
      </table></td>
  </tr>
</table>
