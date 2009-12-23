<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Controls.Language" CodeFile="Language.ascx.vb" %>
<asp:Literal ID="litCommonHeaderTemplate" runat="server" EnableViewState="true"></asp:Literal><asp:DropDownList ID="selectCulture" runat="server" AutoPostBack="true" CssClass="NormalTextBox"></asp:DropDownList><asp:Repeater ID="rptLanguages" runat="server">
<ItemTemplate><asp:Literal ID="litItemTemplate" runat="server" EnableViewState="true"></asp:Literal> </ItemTemplate>
<AlternatingItemTemplate><asp:Literal ID="litItemtemplate" runat="server" EnableViewState="true"></asp:Literal></AlternatingItemTemplate>
<HeaderTemplate><asp:Literal ID="litItemtemplate" runat="server" EnableViewState="true"></asp:Literal></HeaderTemplate>
<SeparatorTemplate><asp:Literal ID="litItemtemplate" runat="server" EnableViewState="true"></asp:Literal></SeparatorTemplate>
<FooterTemplate><asp:Literal ID="litItemtemplate" runat="server" EnableViewState="true"></asp:Literal></FooterTemplate></asp:Repeater><asp:Literal ID="litCommonFooterTemplate" runat="server" EnableViewState="true"></asp:Literal>