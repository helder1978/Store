<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Admin.SQL.SQL"
    CodeFile="SQL.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table width="400" cellspacing="2" cellpadding="2" summary="Load Script Design Table" border="0">
    <tr>
        <td class="SubHead" vAlign="top">
            <dnn:label id="plSqlScript" runat="server" controlname="uplSqlScript" suffix=""></dnn:label>
        </td>
        <td vAlign="top">
            <asp:FileUpload ID="uplSqlScript" runat="server" />
        </td>
        <td vAlign="top">
            <asp:LinkButton ID="cmdUpload" resourcekey="cmdUpload" EnableViewState="False"
    CssClass="CommandButton" runat="server" ToolTip="Load the selected file.">Load</asp:LinkButton>
        </td>
    </tr>
</table>
<asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" Columns="50" Rows="10"
    EnableViewState="False"></asp:TextBox>
<br>
<asp:LinkButton ID="cmdExecute" resourcekey="cmdExecute" EnableViewState="False"
    CssClass="CommandButton" runat="server" ToolTip="can include {directives} and /*comments*/"
    Width="120px">Execute</asp:LinkButton>
<asp:CheckBox ID="chkRunAsScript" resourcekey="chkRunAsScript" CssClass="SubHead"
    runat="server" Text="Run as Script" TextAlign="Left" ToolTip="include 'GO' directives; for testing &amp; update scripts">
</asp:CheckBox>
<br>
<br>
<asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" EnableViewState="False"></asp:Label>
<asp:DataGrid ID="grdResults" runat="server" AutoGenerateColumns="True" HeaderStyle-CssClass="SubHead"
    ItemStyle-CssClass="Normal" summary="SQL Design Table" EnableViewState="False">
    <ItemStyle CssClass="Normal"></ItemStyle>
    <HeaderStyle CssClass="SubHead"></HeaderStyle>
</asp:DataGrid>
