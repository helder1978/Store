<%@ Control language="c#" CodeBehind="ProductDetail.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ProductDetail" AutoEventWireup="True" %>

<table cellspacing="0" cellpadding="0" width="100%" border="0">
  <tr>
    <td align="left" class="ShopBack"><asp:Panel ID="pnlReturn" runat="server">
        <asp:hyperlink id="lnkReturn" runat="server" cssclass="NormalBold" resourcekey="lnkReturn">« back</asp:hyperlink>
        </asp:Panel>
    </td>
  </tr>
  <tr>
    <td nowrap><asp:placeholder id="plhDetails" runat="server" />
      <asp:Panel ID="pnlReviews" visible="false" runat="server">
        <asp:Label ID="labelReviews" CssClass="ListContainer-Title" Runat="server" resourcekey="labelReviews">Reviews</asp:Label>
        <p><asp:PlaceHolder ID="plhReviews" Runat="server"></asp:PlaceHolder></p>
        </asp:Panel>
    </td>
  </tr>
  <tr><td>&nbsp;</td></tr>
  <tr><td>
        <asp:GridView ID="gvResults" AllowPaging="False" PageSize="1" AllowSorting="False" AutoGenerateColumns="False" runat="server" 
                Width="100%" ShowHeader="false" RowStyle-CssClass="rowTableReportsShop" CellPadding="5" GridLines="None" >
            <Columns>
                <asp:TemplateField HeaderText="Related Report">
                 <ItemTemplate>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                      <tr>
                        <td class="rowHeaderTableReports" width="48%"><img src="/images/canadean/shop/shop_canadean_difference.gif" alt="the canadean difference" border="0" /></td>
                        <td>&nbsp;&nbsp;&nbsp;</td>
                        <td colspan="2" class="rowHeaderTableReports" width="48%"><img src="/images/canadean/shop/shop_related_report.gif" alt="related report" border="0" /></td>
                      </tr>
                      <tr height="30px">
                        <td>
                            <p class="NormalBold">Our Methodology</p>
                        </td>
                        <td>&nbsp;</td>
                        <td width="30%">
                            <p>
                                <asp:HyperLink ID="hlTitle" Runat="Server" CssClass="NormalBold" NavigateUrl='<%# FixHyperLink(Eval("ProductID").ToString(), Eval("CategoryID1").ToString(), Eval("CategoryID2").ToString(), Eval("CategoryID3").ToString(), Eval("ModelName").ToString()) %>' Text='<%# Eval("ModelName").ToString()  %>'/>
                            </p>
                        </td>
                        <td width="18%" rowspan="2" valign="bottom">
                            <asp:HyperLink id="hlReportImage" runat="server" ToolTip="wisdom" NavigateUrl='<%# FixHyperLink(Eval("ProductID").ToString(), Eval("CategoryID1").ToString(), Eval("CategoryID2").ToString(), Eval("CategoryID3").ToString(), Eval("ModelName").ToString()) %>' >
                                <img width="100" src="/Portals/0/reports-images/img-reports-5.gif" border="0" alt="Wisdom" title="Wisdom" />
                            </asp:HyperLink>
                        </td>
                      </tr>
                      <tr>
                        <td class="Normal" valign="top">
                            <p>Canadean Ltd has over 30 years experience researching the internation beverage industry, covering
                            Soft Drinks, Beer, Wines & Spirits, and Beverage Packaging.</p>
                            <p>Our unique approach ensures that we offer a compreensive analysis of the global beverage market.</p>
                        </td>
                        <td></td>
                        <td class="Normal" valign="top">
                            <asp:Label ID="lblDescriptionTag" runat="server" Text='<%# Eval("DescriptionTag").ToString()  %>' CssClass="Normal"></asp:Label>
                        </td>
                        <td></td>
                      </tr>
                      <tr><td colspan="3">&nbsp;</td></tr>
                      <tr>
                        <td><a href="/About_Us/Research_Methodology.aspx"><img src="/images/canadean/shop/shop_find_out_more.gif" alt="find out more" border="0" /></a></td>
                        <td></td>
                        <td><asp:HyperLink id="hlFindOutMore" runat="server" ToolTip="find out more" NavigateUrl='<%# FixHyperLink(Eval("ProductID").ToString(), Eval("CategoryID1").ToString(), Eval("CategoryID2").ToString(), Eval("CategoryID3").ToString(), Eval("ModelName").ToString()) %>' ImageUrl="/images/canadean/shop/shop_find_out_more.gif" /></td>
                        <td></td>
                      </tr>
                    </table>
                  </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>    
  </td></tr>
  <tr><td>&nbsp;</td></tr>
</table>
<asp:Label ID="labelLog" Runat="server"></asp:Label>