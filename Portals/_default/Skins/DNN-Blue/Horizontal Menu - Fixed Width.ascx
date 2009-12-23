<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BANNER" Src="~/Admin/Skins/Banner.ascx" %>
<%@ Register TagPrefix="dnn" TagName="NAV" Src="~/Admin/Skins/Nav.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="CURRENTDATE" Src="~/Admin/Skins/CurrentDate.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="DOTNETNUKE" Src="~/Admin/Skins/DotNetNuke.ascx" %>
<TABLE class="pagemaster" border="0" cellspacing="0" cellpadding="0">
<TR>
<TD valign="top">
<TABLE class="skinmaster" width="770" border="0" align="center" cellspacing="0" cellpadding="0">
<TR>
<TD id="ControlPanel" runat="server" class="contentpane" valign="top" align="center"></TD>
</TR>
<TR>
<TD valign="top">
<TABLE class="skinheader" cellSpacing="0" cellPadding="3" width="100%" border="0">
  <TR>
    <TD vAlign="middle" align="left"><dnn:LOGO runat="server" id="dnnLOGO" /></TD>
    <TD vAlign="middle" align="right"><dnn:BANNER runat="server" id="dnnBANNER" /></TD>
  </TR>
</TABLE>
<TABLE class="skingradient" cellSpacing="0" cellPadding="3" width="100%" border="0">
  <TR>
    <TD width="100%" vAlign="middle" align="left" nowrap><dnn:NAV runat="server" id="dnnNAV" ProviderName="DNNMenuNavigationProvider" CSSControl="main_dnnmenu_bar" CSSContainerRoot="main_dnnmenu_container" CSSNode="main_dnnmenu_item" CSSNodeRoot="main_dnnmenu_rootitem" CSSIcon="main_dnnmenu_icon" CSSContainerSub="main_dnnmenu_submenu" CSSBreak="main_dnnmenu_break" CSSNodeHover="main_dnnmenu_itemhover" NodeLeftHTMLBreadCrumbRoot="<img alt=&quot;*&quot; BORDER=&quot;0&quot; src=&quot;breadcrumb.gif&quot;/>" /></TD>
    <TD class="skingradient" vAlign="middle" align="right" nowrap><dnn:SEARCH runat="server" id="dnnSEARCH" showWeb="True" showSite="True" /><dnn:LANGUAGE runat="server" id="dnnLANGUAGE" showMenu="False" showLinks="True" /></TD>
  </TR>
</TABLE>
<TABLE cellSpacing="0" cellPadding="3" width="100%" border="0">
  <TR>
    <TD width="200" vAlign="top" align="left" nowrap><dnn:CURRENTDATE runat="server" id="dnnCURRENTDATE" /></TD>
    <TD width="100%" vAlign="top" align="center"><B>..::</B>&nbsp;<dnn:BREADCRUMB runat="server" id="dnnBREADCRUMB" Separator="&nbsp;&raquo;&nbsp;" RootLevel="0" /><B>::..</B></TD>
    <TD width="200" vAlign="top" align="right" nowrap><dnn:USER runat="server" id="dnnUSER" />&nbsp;&nbsp;<dnn:LOGIN runat="server" id="dnnLOGIN" /></TD>
  </TR>
</TABLE>
</TD>
</TR>
<TR>
<TD valign="top" height="100%">
<TABLE cellspacing="3" cellpadding="3" width="100%" border="0">
  <TR>
    <TD class="toppane" colspan="3" id="TopPane" runat="server" valign="top" align="center"></TD>
  </TR>
  <TR valign="top">
    <TD class="leftpane" id="LeftPane" runat="server" valign="top" align="center"></TD>
    <TD class="contentpane" id="ContentPane" runat="server" valign="top" align="center"></TD>
    <TD class="rightpane" id="RightPane" runat="server" valign="top" align="center"></TD>
  </TR>
  <TR>
    <TD class="bottompane" colspan="3" id="BottomPane" runat="server" valign="top" align="center"></TD>
  </TR>
</TABLE>
</TD>
</TR>
<TR>
<TD valign="top">
<TABLE class="skingradient" cellSpacing="0" cellPadding="0" width="100%" border="0">
  <TR>
    <TD valign="middle" align="center"><dnn:COPYRIGHT runat="server" id="dnnCOPYRIGHT" />&nbsp;&nbsp;<dnn:TERMS runat="server" id="dnnTERMS" />&nbsp;&nbsp;<dnn:PRIVACY runat="server" id="dnnPRIVACY" /></TD>
  </TR>
</TABLE>
</TD>
</TR>
<TR>
<TD valign="top" align="center"><dnn:DOTNETNUKE runat="server" id="dnnDOTNETNUKE" /></TD>
</TR>
</TABLE>
</TD>
</TR>
</TABLE>
