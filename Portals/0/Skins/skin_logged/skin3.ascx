<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SOLPARTMENU" Src="~/Admin/Skins/SolPartMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BANNER" Src="~/Admin/Skins/Banner.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="HELP" Src="~/Admin/Skins/Help.ascx" %>

	<div style="background-color:White;">
	 <table class="pagemaster" border="0" cellspacing="0" cellpadding="0" align=center width="100%">
		<tr>
		    <td height="1%" align="center">
			    <table border="0" align="center" cellpadding="0" cellspacing="0">
				    <tr>
						<td id="ControlPanel" runat="server" class="contentpane" valign="top" align="center"></td>
					</tr>
				</table>
			</td>
	    </tr>
    </table>
    </div>
	<div id="topBackground">
	    <table class="topTable" cellspacing = "0px" cellpadding = "0px" align="center">
		    <tr>
			    <td colspan="3">
				    <table cellspacing="0" cellpadding="0" border="0" width="100%">
				        <tr style="height:10px;">
				            &nbsp;
				        </tr>
					    <tr>
						    <td style="width:50px">&nbsp;	</td>
						    <td align="left"><dnn:LOGO runat="server" id="dnnLOGO" />			</td>
						    <td class="header"> 
							   &nbsp;
						    </td>
						    <td style="width:50px">&nbsp;	</td>
					    </tr>
				    </table>
			    </td>
		    </tr>
		    <tr>
			    <td colspan="3">
				    <table class="menu" cellspacing="0" cellpadding="0" border="0" style="width:989px">
					    <tr>
						    <td class="top2background">&nbsp;	</td>
							    <!--
							    <td align="center" valign="top" runat="server" id="TOPPANE"><dnn:SOLPARTMENU runat="server" id="dnnSOLPARTMENU" /> <dnn:SEARCH runat="server" id="dnnSEARCH" /></td>
							    -->
						   
						    <td>
							    <table class="menu" cellspacing="0px" cellpadding="0px" style="width:893px">
							    <tr>
							    
  								    <td width="39px"> 
  								        <a href="http://wisdev.canadean.com/HomePage/tabid/54/Default.aspx">
  								            <img style="border: 0px;"  src="<%= SkinPath %>images/menu/new-menu/home-menu.gif" />
  								        </a>
  								    </td>
  								    <td class="itemspace"></td>
  								    <td width="62px">
  								        <a href="http://wisdev.canadean.com/AboutUs/tabid/55/Default.aspx">
  								            <img style="border: 0px;"  src="<%= SkinPath %>images/menu/new-menu/aboutus-menu.gif" />
  								        </a>
  								    </td>
  								    <td class="itemspace"></td>
								    <td width="136px">
  								        <a href="http://wisdev.canadean.com/ProductsServices/tabid/56/Default.aspx">
  								            <img style="border: 0px;"  src="<%= SkinPath %>images/menu/new-menu/productsservices-menu.gif" />
  								        </a>
  								    </td>
								    <td class="itemspace"></td>
								    <td width="33px">
  								        <a href="http://wisdev.canadean.com/Shop/tabid/57/Default.aspx">
  								            <img style="border: 0px;"  src="<%= SkinPath %>images/menu/new-menu/shop-menu.gif" />
  								        </a>
  								    </td>
								    <td class="itemspace"></td>
								    <td width="37px">
  								        <a href="http://wisdev.canadean.com/News/tabid/58/Default.aspx">
  								            <img style="border: 0px;"  src="<%= SkinPath %>images/menu/new-menu/news-menu.gif" />
  								        </a>
  								    </td>
								    <td class="itemspace"></td>
								    <td width="52px">
  								        <a href="http://wisdev.canadean.com/Contact/tabid/59/Default.aspx">
  								            <img style="border: 0px;"  src="<%= SkinPath %>images/menu/new-menu/contact-menu.gif" />
  								        </a>
  								    </td>
								    <td class="itemspace"></td>
								    <td width="113px">
  								        <a href="http://wisdev.canadean.com/ShoppingBasket/tabid/60/Default.aspx">
  								            <img style="border: 0px;"  src="<%= SkinPath %>images/menu/new-menu/shoppingbasket-menu.gif" />
  								        </a>
  								    </td>
								    <td class="itemtwospace"></td>
								    <td class="item8"></td>
 		 						    <td style="padding-left: 10px;">
    								    <input width="123px" type="TEXT" name="Search">
  								    </td>
  								    <td class="itemarrow"></td>
							    </tr>
						    </table>
					    </td>
					    <td class="top2background">&nbsp; 	</td>
				    </tr>
			    </table>
		    </td>
	    </tr>
	    
	    <tr style="width:989px">	
		    <td class="topbackgroundLeft">&nbsp;	</td>
		    <td class="container"> <dnn:BANNER runat="server" id="dnnBANNER" />		</td>
		    <td class="topbackgroundRight">&nbsp; 	</td>
	    </tr>
	    </table>
	</div>
	<div id="bottomColor">
	<div id="bottomBackground">
	<table cellspacing="0" cellpadding="0" border="0" align="center">
		<tr>
		    <td colspan=3 class="canadean3_2_05"></td>
		</tr>
		<tr>
		    <td style="width:46px;">&nbsp;</td>
			<td>
			    <table width="893px" cellspacing="0" cellpadding="0" border="0" align="center" class="PaneBG">
				    <tr>
					    <td width="156px" valign="top" class="LeftPane">
					        <div id="LeftPane" class="LeftPaneInner" runat="server"></div>
					     </td>
						<td width="736px" valign="top" class="ContentPane">
			                <table class="ContentPaneInner" width="100%" cellspacing="0" cellpadding="0" border="0" >
			                    <tr>
			                        <td><img style="border: 0px;" src="<%= SkinPath %>images/li_top.jpg" /></td>
			                    </tr>
					    <tr><td>
<!-- Start sub-menu -->
<table class="menu" cellspacing="0px" cellpadding="0px" width="100%">
    <tr><td colspan="11"><img height="5" alt="" style="border: 0px none ;" src="../../../images/canadean/1x1.gif" /></td></tr>
    <tr>
	    <td> 
	        <a href="http://wisdev.canadean.com/tabid/78/Default.aspx">
	            <img style="border: 0px;"  src="../../../images/canadean/li_submenu_library_on.jpg" />
	        </a>
	    </td>
	    <td><img style="border: 0px;"  src="../../../images/canadean/li_submenu_separator.jpg" /></td>
	    <td> 
	        <a href="#">
	            <img style="border: 0px;"  src="../../../images/canadean/li_submenu_wisdom_off.jpg" />
	        </a>
	    </td>
	    <td><img style="border: 0px;"  src="../../../images/canadean/li_submenu_separator.jpg" /></td>
	    <td> 
	        <a href="http://wisdev.canadean.com/DataAvailability/tabid/63/Default.aspx">
	            <img style="border: 0px;"  src="../../../images/canadean/li_submenu_wisdomavailability_off.jpg" />
	        </a>
	    </td>
	    <td><img style="border: 0px;"  src="../../../images/canadean/li_submenu_separator.jpg" /></td>
	    <td> 
	        <a href="http://wisdev.canadean.com/CanadeanTrial1/Wisdom%20Reporting.aspx">
	            <img style="border: 0px;"  src="../../../images/canadean/li_submenu_wisdomreporting_off.jpg" />
	        </a>
	    </td>
	    <td><img style="border: 0px;"  src="../../../images/canadean/li_submenu_separator.jpg" /></td>
	    <td> 
	        <a href="#">
	            <img style="border: 0px;"  src="../../../images/canadean/li_submenu_newsarchive_off.jpg" />
	        </a>
	    </td>
	    <td><img style="border: 0px;"  src="../../../images/canadean/li_submenu_separator.jpg" /></td>
	    <td> 
	        <a href="http://wisdev.canadean.com/Links/tabid/62/Default.aspx">
	            <img style="border: 0px;"  src="../../../images/canadean/li_submenu_linksdirectory_off.jpg" />
	        </a>
	    </td>
    </tr>
    <tr><td colspan="11"><img height="10" alt="" style="border: 0px none ;" src="../../../images/canadean/1x1.gif" /></td></tr>
</table>
<!-- End sub-menu -->

					    </td></tr>
			                    <tr>
			                        <td><div id="ContentPane" runat="server"></div></td>
			                    </tr>
			                </table>
					    </td>
					</tr>
				    <tr>
					    <td class="LeftPaneBottom"><img style="border: 0px;" height="12" src="<%= SkinPath %>images/1x1.gif" /></td>
					    <td class="ContentPaneBottom"><img style="border: 0px;" height="12" src="<%= SkinPath %>images/1x1.gif" /></td>
					</tr>
				</table>
			</td>
			<td class="bottom2background">&nbsp;	</td>
		</tr>
		<tr>
			<td style="width:46px;">	</td>
			<td style="width:888px;">
			    <table width="100%" cellspacing="0" cellpadding="0">
				    <tr>
					    <td class="footer" width="50%" align="left">
						    <h3><dnn:COPYRIGHT runat="server" id="dnnCOPYRIGHT" /> &nbsp;</h3>
						</td>
						<td class="footer" width="40%" align="right">
						    <h1>
							    <dnn:TERMS runat="server" id="dnnTERMS" />&nbsp;|&nbsp;
            				    <dnn:PRIVACY runat="server" id="dnnPRIVACY" />&nbsp;|&nbsp;
            				    <dnn:HELP runat="server" id="dnnHELP" />
							</h1>
						</td>
					</tr>
				</table>
			</td>
			<td style="width:46px;">	</td>
		</tr>
	</table>
</div>
</div>

