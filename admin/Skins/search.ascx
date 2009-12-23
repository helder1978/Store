<%@ Control language="vb" CodeFile="Search.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Controls.Search" %>
<asp:RadioButton ID="optWeb" runat="server" CssClass="SkinObject" GroupName="Search" Text="Web" />
<asp:RadioButton ID="optSite" runat="server" CssClass="SkinObject" GroupName="Search" Text="Site" />
<asp:TextBox id="txtSearch" runat="server" CssClass="NormalTextBox" Columns="20" maxlength="255" enableviewstate="False"></asp:TextBox>&nbsp;
<asp:LinkButton ID="cmdSearch" Runat="server" CausesValidation="False" CssClass="SkinObject"></asp:LinkButton>