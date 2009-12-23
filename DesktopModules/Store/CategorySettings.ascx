<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CategorySettings.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CategorySettings" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellSpacing=5 width="80%" align=center border=0>
  <tr vAlign=top>
    <td class=NormalBold width="40%"><dnn:label id=labelColumnCount suffix=":" controlname="labelColumnCount" runat="server"></dnn:label></TD>
    <td class=Normal width="60%"><asp:textbox id=txtColumnCount Width="100" Runat="server" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox></TD>
  </TR>
  <tr vAlign=top>
    <td class=NormalBold width="40%"><dnn:label id=labelCatalogPage suffix=":" controlname="labelCatalogPage" runat="server"></dnn:label></TD>
    <td class=Normal width="60%"><asp:dropdownlist id=cmbCatalogPage runat="server" Width="100" autopostback="False" enableviewstate="True"></asp:dropdownlist></TD>
  </TR>
</TABLE>
