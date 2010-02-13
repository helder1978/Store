<%@ Control language="c#" CodeBehind="Catalog.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.Catalog" AutoEventWireup="True" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:PlaceHolder ID="phSearch" runat="server">
    <script language="javascript">
        function redirectSearchPage()
        {
            var searchTerm = document.getElementById('txtCatalogSearch');
            //var categoryId = document.getElementById('ddCatalogSearchCategory');
            //alert(searchTerm.value);
            //alert(categoryId.value);
            //window.location = "http://www.canadean.com/Shop/Search/tabid/95/Default.aspx?Search=" + searchTerm.value + "&SearchCategoryId=" + categoryIdValue;
            //window.location = "http://www.canadean.com/Shop/Search/tabid/95/Default.aspx?Search=" + searchTerm.value + "&SearchCategoryId=";
            window.location = "/Shop/Search/tabid/95/Default.aspx?Search=" + searchTerm.value + "&SearchCategoryId=-1";
            return false;
        }
    </script>
    <asp:imagebutton id="imgGo" ImageUrl="/Portals/0/topmenu/arrow.gif" visible="false" runat="server"></asp:imagebutton>
    <table cellspacing="3" cellpadding="0" summary="Search Input Table" border="0" width="100%" style="border-bottom:1px dotted #9EA4BA;">
        <tr>
            <td width="45%"><img alt="shop" border="0" src="/images/canadean/shop/shop_title.gif" /></td>
            <td align="right" width="45%"></td>
            <td width="10%"></td>
        </tr>
        <tr>
            <td colspan="3"><img height="2" alt="" border="0" src="/images/spacer.gif" /></td>
        </tr>
        <tr>
            <td class="subtitleShop">search&nbsp; <input class="NormalTextBox" id="txtCatalogSearch" style="width: 180px" maxlength="200" size="35" name="txtCatalogSearch" type="text" onkeydown="return __dnn_KeyDown('13', 'javascript:redirectSearchPage()', event);" /></td>
            <td><a onclick="javascript:redirectSearchPage();" href="#"><img alt="" border="0" src="/Portals/0/topmenu/arrow.gif" /></a> <!--<input type="image" name="dnn$ctr621$Catalog$imgGo" id="dnn_ctr621_Catalog_imgGo" src="/Portals/0/topmenu/arrow.gif" 
                	onclick="javascript:redirectPage();" 
                	style="border-width:0px;" />--></td>
            <td></td>
        </tr>
	    <tr><td colspan="3">&nbsp;</td></tr>
    </table>
</asp:PlaceHolder>

<asp:PlaceHolder ID="phSubcategoryDD" visible="false" runat="server">
    <div class="CatalogDDWrapperTemplate">
        <div class="demoHeading">you selected <asp:Label ID="labelAreaName" runat="server"></asp:Label></div>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td width="50%" class="subtitleShop">now select continent</td>
                <td width="50%" class="subtitleShop">and country or region</td>
            </tr>
            <tr><td><img height="10" src="/images/spacer.gif" border="0" /></td></tr>
            <tr>
                <td><asp:DropDownList ID="DropDownList1" runat="server" Width="170" /></td>
                <td><asp:DropDownList ID="DropDownList2" runat="server" Width="170" AutoPostBack="true" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged"/></td>
            </tr>
        </table>
        <br />
        <asp:DropDownList ID="ddHidden" runat="server" style="display:none"/>
        
        <ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" runat="server" TargetControlID="DropDownList1"
            Category="continent"  PromptText="Please select "  LoadingText="[Loading ...]"
            ServicePath="/DesktopModules/Store/CarsService.asmx" ServiceMethod="GetDropDownContents" ParentControlID="ddHidden"/>
        <ajaxToolkit:CascadingDropDown ID="CascadingDropDown2" runat="server" TargetControlID="DropDownList2"
            Category="country" PromptText="Please select " LoadingText="[Loading ...]"
            ServicePath="/DesktopModules/Store/CarsService.asmx" ServiceMethod="GetDropDownContents" ParentControlID="DropDownList1" />
      
    </div>
</asp:PlaceHolder>    

<asp:PlaceHolder ID="phSubSubcategory" runat="server" Visible="false">
    <div class="CatalogSubSubWrapper">
        <div class="demoHeading">you selected <u><a href="/Shop/Reports/tabid/109/Default.aspx" class="demoHeadingLink">Reports</a></u> from <asp:Label ID="labelCategoryName" runat="server"></asp:Label></div>
        <table id="tableSubSubCategory" runat="server" cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr><td><img height="5" src="/images/spacer.gif" border="0" /></td></tr>
            <tr>
                <td width="100%" class="subtitleShop"><asp:Label ID="labelSubSubcategoryNow" runat="server" Text="" CssClass="subtitleShop" /></td>
            </tr>
            <tr>
                <td><asp:Label ID="labelSubSubCategory" runat="server" Text="" /></td>
            </tr>
        </table>
    </div>
</asp:PlaceHolder>

<asp:PlaceHolder ID="phSearchResults" Visible="false" runat="server">
    <br /><asp:Label ID="Label1" runat="server" Text="your results" CssClass="subtitleShop" Visible="false"/><br /><br />
    <asp:GridView ID="gvResults" AllowPaging="True" PageSize="20" AllowSorting="True" AutoGenerateColumns="False" runat="server" 
            OnPageIndexChanging="gvResults_PageIndexChanging" OnSorting="gvResults_Sorting" Width="100%" OnRowDataBound="gvResults_RowDataBound"
            HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="headerTableReports" RowStyle-CssClass="rowTableReports" CellPadding="5" GridLines="None" 
            PagerStyle-HorizontalAlign="Center" PagerStyle-CssClass="pagerTableReports">
        <PagerSettings 
            Position="Bottom" 
            Mode="NumericFirstLast" 
            FirstPageText="<<" 
            LastPageText=">>" 
            NextPageText="next" 
            PreviousPageText="prev" />
        <Columns>
            <asp:TemplateField HeaderText="Title" SortExpression="ModelName" ItemStyle-Font-Bold="true" HeaderStyle-Width="50%">
             <ItemTemplate>
                <asp:HyperLink ID="hlTitle" Runat="Server" NavigateUrl='<%# FixHyperLink(Eval("ProductID").ToString(), Eval("CategoryID1").ToString(), Eval("CategoryID2").ToString(), Eval("CategoryID3").ToString(), Eval("ModelName").ToString())  %>' Text='<%# Eval("ModelName").ToString()  %>'/>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PublishDate" HeaderText="Published Date" HeaderStyle-Wrap="false" SortExpression="PublishDate" Visible="True" HtmlEncode="False" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="UnitCost" HeaderText="Price" SortExpression="UnitCost" Visible="True" HtmlEncode="False" DataFormatString="&pound;{0:F0}" ItemStyle-HorizontalAlign="Right" />
            <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Font-Bold="true">
              <ItemTemplate>view:</ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Font-Bold="true">
             <ItemTemplate>
                <asp:HyperLink ID="imgContent" Runat="Server" NavigateUrl='<%# "~/Portals/0/documents/" + Eval("ProductPreview").ToString()  %>' Target="_blank"
                    Text="pdf"  />
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" SortExpression="" ItemStyle-Font-Bold="true">
             <ItemTemplate>
                <asp:HyperLink ID="hlHTML" Runat="Server" NavigateUrl='<%# FixHyperLinkTOC(Eval("ProductID").ToString())  %>' 
                    Text="html" Visible='<%# HasTOC_Html(Eval("ProductID").ToString())  %>' />
                <asp:Label id="lblHTML" Text="html" runat="server" Visible='<%# !HasTOC_Html(Eval("ProductID").ToString())  %>'></asp:Label>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" SortExpression="">
             <ItemTemplate>
                <asp:HyperLink ID="imgMore" Runat="Server" NavigateUrl='<%# FixHyperLink(Eval("ProductID").ToString(), Eval("CategoryID1").ToString(), Eval("CategoryID2").ToString(), Eval("CategoryID3").ToString(), Eval("ModelName").ToString()) %>' 
                    ImageUrl='<%# "~/images/canadean/shop/shop_moreinfo.gif" %>'/><br/>
              </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView><br /><br /><br /><br />

</asp:PlaceHolder>

<script language="C#" runat="server">
    /*
    void imgAddDECart_OnClick2(object sender, ImageClickEventArgs e) 
    {
        //labelLog.Text = labelLog.Text + "<br>selected location: " + ddListLocation.SelectedValue + " category: " + ddListCategory.SelectedValue;
        String addedDE = ddListLocation.SelectedValue + ":" + ddListCategory.SelectedValue;
        String selDE = (String) Session["selectedDE"];
        if (selDE == null || selDE == "")
            selDE = ";";
        if (selDE.IndexOf(";" + addedDE + ";") == -1)
        {
            Session["selectedDE"] = selDE + addedDE + ";";
            //Label2.Text = Label2.Text + "<br>selected location: " + ddListLocation.SelectedValue + " category: " + ddListCategory.SelectedValue;
            System.Collections.ArrayList prods = PopulateDEGridView(true);
            int numProds = prods.Count;
            Label2.Text = "Price of the selected Category Volumes: " + numProds * 50 + " &pound;";
            Session["selectedDECost"] = new Decimal(numProds * 50);
        }
        else
        {
            labelError.Text = "Category Volume already selected";
            //String js = "<script language=javascript> alert('Category Volume already selected');<" +  "/script>";
            //this.Page.RegisterClientScriptBlock("alert_already_selected", js);
        }
        phAddToCart.Visible = true;
    }

    void imgResetDECart_OnClick2(object sender, ImageClickEventArgs e)
    {
        ResetDEGridView();
    }

    void imgAddCart_OnClick2(object sender, ImageClickEventArgs e)
    {
        if (Session["selectedDE"] != null && Session["selectedDE"] != "")
        {
            string modelNumber = (String)Session["selectedDE"];
            string modelName = tbDEReference.Text;
            decimal cost = 0;
            if (Session["selectedDECost"] != null)
                cost = (Decimal)Session["selectedDECost"];
            DotNetNuke.Modules.Store.Cart.CurrentCart.AddItem(PortalId, -1, 1, -1, modelNumber, modelName, cost);
            ResetDEGridView();
        }
        else
        {
            String js = "<script language=javascript> alert('You must select a Category Volume first.');<" + "/script>";
            this.Page.RegisterClientScriptBlock("alert_already_selected", js);
        }            
            
    }
    */
</script>

<asp:PlaceHolder ID="phSubcategoryDEDD" visible="false" runat="server">

    <script language="javascript" type="text/javascript">
        function confirmSelection()
        {
            var ddLocation = document.getElementById("<%= ddListLocation.ClientID %>");
            var ddCategory = document.getElementById("<%= ddListCategory.ClientID %>");
            if(ddLocation.value == "" || ddCategory.value == "")
            {
                alert("You must choose a Category Volume location and category.");
                return false;
            }
            else
                return true;
        }
        
        function confirmReference()
        {
            var tb = document.getElementById("<%= tbDEReference.ClientID %>");
            if(tb.value == "")
            {
                alert("You must choose a reference.");
                return false;
            }
            else
                return true;
        }
    </script>
    <div class="CatalogDEDDWrapperTemplate">
        <div class="demoHeading">beverage category volumes by country</div>
        <table>
            <tr><td colspan="3" class="Normal">Category consumption volumes for all years between 1999 and 2014
			  can be purchased here for &pound;50 per category and country.
			  Data for years 1999-2008 are actual, 2009 is provisional, and
			  2010 to 2014 are forecasts.
            </td></tr>
            <tr><td colspan="3" class="Normal">Simply select from the countries and categories listed and click 'Add to Cart'.
                To place an order, give your selection a reference and click 'Add to your shopping basket'.
            </td></tr>
            <tr>
                <td class="subtitleShop">select country</td>
                <td class="subtitleShop">and category</td>
                <td></td>
            </tr>
            <tr>
                <td><asp:DropDownList ID="ddListLocation" runat="server" Width="170" /></td>
                <td><asp:DropDownList ID="ddListCategory" runat="server" Width="170" /></td>
                <td align="right">
                    <asp:ImageButton ID="imgAddDECart" Runat="Server" OnClientClick="return confirmSelection();" OnClick="imgAddDECart_OnClick" ImageUrl='~/images/canadean/shop/shop_addCart.gif'/>
                </td>
            </tr>
        </table>
        <br />
        <asp:DropDownList ID="ddDEHidden" runat="server" style="display:none"/>

        <ajaxToolkit:CascadingDropDown ID="CascadingDropDown3" runat="server" TargetControlID="ddListLocation"
            Category="continent"  PromptText="Please select "  LoadingText="[Loading ...]"
            ServicePath="/DesktopModules/Store/CarsService.asmx" ServiceMethod="GetDropDownContents" ParentControlID="ddDEHidden"/>
        <ajaxToolkit:CascadingDropDown ID="CascadingDropDown4" runat="server" TargetControlID="ddListCategory"
            Category="country" PromptText="Please select " LoadingText="[Loading ...]"
            ServicePath="/DesktopModules/Store/CarsService.asmx" ServiceMethod="GetDropDownContents" ParentControlID="ddListLocation" />
      
                
                <br />
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td width="300" class="subtitleShop">Category Volumes Cart</td>
                <td width="150" class="shopReferenceText" nowrap>Give this cart a reference</td>
                <td width="100"><asp:TextBox id="tbDEReference" runat="server"></asp:TextBox></td>
            </tr>
            <tr><td colspan=3"><img height="5" src="/images/spacer.gif" border="0" /></td></tr>
        </table>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="inline">
            <ContentTemplate>
                <div style="height:100%">
                <asp:GridView ID="gvDECart" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" runat="server" 
                    OnPageIndexChanging="gvDECart_PageIndexChanging" OnSorting="gvDECart_Sorting" 
                    HeaderStyle-CssClass="headerTableReports" Width="100%" 
                    HeaderStyle-HorizontalAlign="Left" RowStyle-CssClass="rowTableReports" CellPadding="5" GridLines="None" 
                    PagerStyle-HorizontalAlign="Center" PagerStyle-CssClass="pagerTableReports">
                    <Columns>
                        <asp:BoundField DataField="CategoryName1" HeaderText="Countries" SortExpression="CategoryName1" Visible="True" />
                        <asp:BoundField DataField="CategoryName2" HeaderText="Categories" SortExpression="CategoryName2" Visible="True" />
                    </Columns>
                </asp:GridView>
                </div>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td width="440" class="rowTableReports" align="right"><asp:Label ID="Label2" runat="server" Text="Total cart cost"  CssClass="shopTotalCartText"/></td>
                        <td width="110" class="rowTableReports">&nbsp;   </td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                </table>
                <asp:Label ID="labelError" runat="server" Text="" ForeColor="red"/>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="imgAddDECart" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="imgResetDECart" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:PlaceHolder id="phAddToCart" runat="server" Visible="true">
            <table cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td width="270"></td>
                    <td width="91"><asp:ImageButton ID="imgResetDECart" Runat="Server" OnClick="imgResetDECart_OnClick" ImageUrl='~/images/canadean/shop/shop_reset.gif'/></td>
                    <td width="188"><asp:ImageButton ID="imgAddCart" Runat="Server" OnClientClick="return confirmReference();" OnClick="imgAddCart_OnClick" ImageUrl='~/images/canadean/shop/shop_addBasket.gif'/></td>
                </tr>
            </table><br /><br /><br />
        </asp:PlaceHolder>     
    </div>
    <div class="demobottom"></div>
</asp:PlaceHolder>    

<asp:PlaceHolder ID="phSubcategory" runat="server" Visible="false">
    <br /><table cellspacing='3' cellpadding='0' border='0'><tr><td class='subtitleShop'>select report by service</td></tr><tr><td>
        <table cellspacing='0' cellpadding='0' border='0'>
            <tbody>
                <tr>
                    <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/5/Default.aspx'>Soft Drinks</a></td>
                    <td><a href='/Shop/Reports/tabid/109/CategoryID/5/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>
                    <td>&nbsp;</td>
                    <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/8/Default.aspx'>Beverage Packaging</a></td>
                    <td><a href='/Shop/Reports/tabid/109/CategoryID/8/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/6/Default.aspx'>Beer</a></td>
                    <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/6/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>
                    <td>&nbsp;</td>
                    <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/9/Default.aspx'>All Beverages</a></td>
                    <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/9/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/7/Default.aspx'>Dairy Drinks</a></td>
                    <td><a class='shopIntroText' href='/Shop/Reports/tabid/109/CategoryID/7/Default.aspx'><img alt='' border='0' src='/images/canadean/shop/shop_bullet.gif' /> </a></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </tbody>
        </table>
    </td></tr></table><p>&nbsp;</p>
</asp:PlaceHolder>

<asp:Label ID="labelLog2" runat="server" Text="" />
