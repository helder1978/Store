<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.SkinControl" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<asp:RadioButton ID="optHost" resourcekey="Host" Runat="server" Text="Host" CssClass="SubHead" Checked="True" AutoPostBack="True" GroupName="SkinControl"></asp:RadioButton>&nbsp;&nbsp;<asp:RadioButton ID="optSite" resourcekey="Site" Runat="server" Text="Site" CssClass="SubHead" Checked="False" AutoPostBack="True" GroupName="SkinControl"></asp:RadioButton>
<br/>
<asp:DropDownList id="cboSkin" runat="server" cssclass="NormalTextBox"></asp:DropDownList>&nbsp;<dnn:CommandButton ID="cmdPreview" Runat="server" CssClass="CommandButton" EnableViewState="False" resourcekey="Preview" ImageUrl="~/images/view.gif" />

