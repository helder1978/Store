<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Control language="c#" CodeBehind="CatalogSettings.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CatalogSettings" AutoEventWireup="True" %>

<br />
<dnn:sectionhead id="dshGenSettings" runat="server" cssclass="Head" text="General Settings" section="tblGenSettings"
	includerule="false" isexpanded="true" resourcekey="dshGenSettings"></dnn:sectionhead>
<table id="tblGenSettings" runat="server" width="100%" cellspacing="5" cellpadding="0"
	border="0">
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblCatTemplate" Runat="server" resourcekey="lblCatTemplate.Text">Catalog Template:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstTemplate" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblUseDefaultCategory" Runat="server" resourcekey="lblUseDefaultCategory.Text">Use Default Category:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkUseDefaultCategory" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblDefaultCategory" Runat="server" resourcekey="lblDefaultCategory.Text">Default Category:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstDefaultCategory" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblShowCategoryMsg" Runat="server" resourcekey="lblShowCategoryMsg.Text">Show Category Message:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkShowMessage" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblShowNewProducts" Runat="server" resourcekey="lblShowNewProducts.Text">Show New Products:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkShowNew" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblShowFeaturedProducts" Runat="server" resourcekey="lblShowFeaturedProducts.Text">Show Featured Products:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkShowFeatured" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblShowPopularProducts" Runat="server" resourcekey="lblShowPopularProducts.Text">Show Popular Products:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkShowPopular" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblShowCategoryProducts" Runat="server" resourcekey="lblShowCategoryProducts.Text">Show Category Products:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkShowCategory" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblShowProductDetail" Runat="server" resourcekey="lblShowProductDetail.Text">Show Product Detail:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkShowDetail" runat="server"></asp:checkbox>
    </td>
  </tr>
</table>
<br />
<dnn:sectionhead id="dshNewProductList" runat="server" cssclass="Head" text="New Product Settings"
	section="tblNewProductList" includerule="false" isexpanded="false" resourcekey="dshNewProductList"></dnn:sectionhead>
<table id="tblNewProductList" runat="server" width="100%" cellspacing="5" cellpadding="0"
	border="0">
  <tr>
    <td align="right" class="SubHead" nowrap="nowrap" valign="top"><asp:Label ID="lblNPSContainerTemplate" runat="server" resourcekey="lblNPSContainerTemplate.Text">Container Template:</asp:Label></td>
    <td></td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstNPLContainerTemplate" runat="server" enableviewstate="True" autopostback="False"> </asp:DropDownList></td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblNPSListTemplate" Runat="server" resourcekey="lblNPSListTemplate.Text">List Template:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%">
        <asp:dropdownlist id="lstNPLTemplate" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblNPSRows" Runat="server" resourcekey="lblNPSRows.Text">Rows:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtNPLRowCount" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblNPSColumns" Runat="server" resourcekey="lblNPSColumns.Text">Columns:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtNPLColumnCount" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblNPSColumnWidth" Runat="server" resourcekey="lblNPSColumnWidth.Text">Column Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtNPLColumnWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
    <tr>
        <td align="right" class="SubHead" nowrap="nowrap" valign="top">
            <asp:Label ID="lblNPSRepeatDirection" runat="server" resourcekey="lblNPSRepeatDirection.Text">Repeat Direction:</asp:Label></td>
        <td>
        </td>
        <td class="Normal" valign="top" width="60%">
            <asp:dropdownlist id="lstNPLRepeatDirection" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
        </td>
    </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblNPSShowThumbnail" Runat="server" resourcekey="lblNPSShowThumbnail.Text">Show Thumbnail:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkNPLShowThumbnail" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblNPSThumbnailWidth" Runat="server" resourcekey="lblNPSThumbnailWidth.Text">Thumbnail Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtNPLThumbnailWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblNPSDetailPage" Runat="server" resourcekey="lblNPSDetailPage.Text">Detail Page:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstNPLDetailPage" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
</table>
<br />
<dnn:sectionhead id="dshFeaturedProductList" runat="server" cssclass="Head" text="Featured Product Settings"
	section="tblFeaturedProductList" includerule="false" isexpanded="false" resourcekey="dshFeaturedProductList"></dnn:sectionhead>
<table id="tblFeaturedProductList" runat="server" width="100%" cellspacing="5" cellpadding="0"
	border="0">
  <tr>
    <td align="right" class="SubHead" nowrap="nowrap" valign="top"><asp:Label ID="lblFPSContainerTemplate" runat="server" resourcekey="lblFPSContainerTemplate.Text">Container Template:</asp:Label></td>
    <td></td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstFPLContainerTemplate" runat="server" enableviewstate="True" autopostback="False"> </asp:DropDownList></td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblFPSListTemplate" Runat="server" resourcekey="lblFPSListTemplate.Text">List Template:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstFPLTemplate" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblFPSRows" Runat="server" resourcekey="lblFPSRows.Text">Rows:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtFPLRowCount" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblFPSColumns" Runat="server" resourcekey="lblFPSColumns.Text">Columns:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtFPLColumnCount" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblFPSColumnWidth" Runat="server" resourcekey="lblFPSColumnWidth.Text">Column Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtFPLColumnWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
    <tr>
        <td align="right" class="SubHead" nowrap="nowrap" valign="top">
            <asp:Label ID="lblFPSRepeatDirection" runat="server" resourcekey="lblFPSRepeatDirection.Text">Repeat Direction:</asp:Label></td>
        <td>
        </td>
        <td class="Normal" valign="top" width="60%">
            <asp:dropdownlist id="lstFPLRepeatDirection" runat="server" enableviewstate="True" autopostback="False">
            </asp:DropDownList></td>
    </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblFPSShowThumbnail" Runat="server" resourcekey="lblFPSShowThumbnail.Text">Show Thumbnail:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkFPLShowThumbnail" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblFPSThumbnailWidth" Runat="server" resourcekey="lblFPSThumbnailWidth.Text">Thumbnail Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtFPLThumbnailWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblFPSDetailPage" Runat="server" resourcekey="lblFPSDetailPage.Text">Detail Page:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstFPLDetailPage" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
</table>
<br />
<dnn:sectionhead id="dshPopularProductList" runat="server" cssclass="Head" text="Popular Product Settings"
	section="tblPopularProductList" includerule="false" isexpanded="false" resourcekey="dshPopularProductList"></dnn:sectionhead>
<table id="tblPopularProductList" runat="server" width="100%" cellspacing="5" cellpadding="0"
	border="0">
  <tr>
    <td align="right" class="SubHead" nowrap="nowrap" valign="top"><asp:Label ID="lblPPSContainerTemplate" runat="server" resourcekey="lblPPSContainerTemplate.Text">Container Template:</asp:Label></td>
    <td></td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstPPLContainerTemplate" runat="server" enableviewstate="True" autopostback="False"> </asp:DropDownList></td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPPSListTemplate" Runat="server" resourcekey="lblPPSListTemplate.Text">List Template:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstPPLTemplate" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPPSRows" Runat="server" resourcekey="lblPPSRows.Text">Rows:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtPPLRowCount" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPPSColumns" Runat="server" resourcekey="lblPPSColumns.Text">Columns:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtPPLColumnCount" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPPSColumnWidth" Runat="server" resourcekey="lblPPSColumnWidth.Text">Column Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtPPLColumnWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
    <tr>
        <td align="right" class="SubHead" nowrap="nowrap" valign="top">
            <asp:Label ID="lblPPSRepeatDirection" runat="server" resourcekey="lblPPSRepeatDirection.Text">Repeat Direction:</asp:Label></td>
        <td>
        </td>
        <td class="Normal" valign="top" width="60%">
            <asp:dropdownlist id="lstPPLRepeatDirection" runat="server" enableviewstate="True" autopostback="False">
            </asp:DropDownList></td>
    </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPPSShowThumbnail" Runat="server" resourcekey="lblPPSShowThumbnail.Text">Show Thumbnail:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkPPLShowThumbnail" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPPSThumbnailWidth" Runat="server" resourcekey="lblPPSThumbnailWidth.Text">Thumbnail Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtPPLThumbnailWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPPSDetailPage" Runat="server" resourcekey="lblPPSDetailPage.Text">Detail Page:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstPPLDetailPage" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
</table>
<br />
<dnn:sectionhead id="dshCategoryProductList" runat="server" cssclass="Head" text="Category Settings"
	section="tblCategoryProductList" includerule="false" isexpanded="false" resourcekey="dshCategoryProductList"></dnn:sectionhead>
<table id="tblCategoryProductList" runat="server" width="100%" cellspacing="5" cellpadding="0"
	border="0">
  <tr>
    <td align="right" class="SubHead" nowrap="nowrap" valign="top"><asp:Label ID="lblCSContainerTemplate" runat="server" resourcekey="lblCSContainerTemplate.Text">Container Template:</asp:Label></td>
    <td></td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstCPLContainerTemplate" runat="server" enableviewstate="True" autopostback="False"> </asp:dropdownList></td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblCSListTemplate" Runat="server" resourcekey="lblCSListTemplate.Text">List Template:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstCPLTemplate" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblCSRows" Runat="server" resourcekey="lblCSRows.Text">Rows:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtCPLRowCount" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblCSColumns" Runat="server" resourcekey="lblCSColumns.Text">Columns:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtCPLColumnCount" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblCSColumnWidth" Runat="server" resourcekey="lblCSColumnWidth.Text">Column Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtCPLColumnWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
    <tr>
        <td align="right" class="SubHead" nowrap="nowrap" valign="top">
            <asp:Label ID="lblCSRepeatDirection" runat="server" resourcekey="lblCSRepeatDirection.Text">Repeat Direction:</asp:Label></td>
        <td>
        </td>
        <td class="Normal" valign="top" width="60%">
            <asp:dropdownlist id="lstCPLRepeatDirection" runat="server" enableviewstate="True" autopostback="False">
            </asp:DropDownList></td>
    </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblCSShowThumbnail" Runat="server" resourcekey="lblCSShowThumbnail.Text">Show Thumbnail:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkCPLShowThumbnail" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblCSThumbnailWidth" Runat="server" resourcekey="lblCSThumbnailWidth.Text">Thumbnail Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtCPLThumbnailWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblCSDetailPage" Runat="server" resourcekey="lblCSDetailPage.Text">Detail Page:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstCPLDetailPage" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
</table>
<br />
<dnn:sectionhead id="dshProductDetails" runat="server" cssclass="Head" text="Product Detail Settings"
	section="tblProductDetails" includerule="false" isexpanded="false" resourcekey="dshProductDetails"></dnn:sectionhead>
<table id="tblProductDetails" runat="server" width="100%" cellspacing="5" cellpadding="0"
	border="0">
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblDetailTemplate" Runat="server" resourcekey="lblDetailTemplate.Text">Detail Template:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstDetailTemplate" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPDSShowThumbnail" Runat="server" resourcekey="lblPDSShowThumbnail.Text">Show Thumbnail:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:checkbox id="chkDetailShowThumbnail" runat="server"></asp:checkbox>
    </td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label id="lblPDSThumbnailWidth" Runat="server" resourcekey="lblPDSThumbnailWidth.Text">Thumbnail Width:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:textbox id="txtDetailThumbnailWidth" runat="server" width="50" MaxLength="4" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr>
    <td align="right" class="SubHead" nowrap="nowrap" valign="top"><asp:Label ID="lblPDSShowReviews" runat="server" resourcekey="lblPDSShowReviews.Text">Show Reviews:</asp:Label></td>
    <td></td>
    <td class="Normal" valign="top" width="60%"><asp:CheckBox ID="chkDetailShowReviews" runat="server" /></td>
  </tr>
  <tr>
    <td class="SubHead" align="right" valign="top" nowrap><asp:Label ID="lblPDSReturnPage" runat="server" resourcekey="lblPDSReturnPage.Text">Return To:</asp:Label></td>
    <td>&nbsp;</td>
    <td class="Normal" valign="top" width="60%"><asp:dropdownlist id="lstPDSReturnPage" runat="server" enableviewstate="True" autopostback="False"> </asp:dropdownlist>
    </td>
  </tr>
</table>
