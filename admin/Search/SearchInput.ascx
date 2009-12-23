<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.SearchInput.SearchInput" CodeFile="SearchInput.ascx.vb" %>
<table cellSpacing="0" cellPadding="4" summary="Search Input Table" border="0">
	<tr>
	
		<td nowrap><dnn:label id="plSearch" runat="server" controlname="cboModule" suffix=":"></dnn:label><asp:image id="imgSearch" runat="server"></asp:image></td>
		<td><asp:textbox id="txtSearch" runat="server" Wrap="False" Width="150px" columns="35" maxlength="200"
				cssclass="NormalTextBox"></asp:textbox></td>
		<td><asp:DropDownList ID="ddSearchSection" runat="server">
		        <asp:ListItem Value="">Whole site</asp:ListItem>
                <asp:ListItem Value="shop">Shop</asp:ListItem>
                <asp:ListItem Value="news">News</asp:ListItem>
                <asp:ListItem Value="links">Links</asp:ListItem>
            </asp:DropDownList>
	    </td>
		<td><asp:imagebutton id="imgGo" runat="server"></asp:imagebutton><asp:Button id="cmdGo" runat="server" Text="Go"></asp:Button></td>
	</tr>
</table>
<table id="tableAdvanced" runat="server" visible="false" cellSpacing="0" cellPadding="4" summary="Advanced Search Input Table" border="0">
	<tr>
	    <td colspan="4">Advanced Options</td>
	</tr>
	<tr>
	    <td>Find Results</td>
	    <td>With <b>any</b> of the words</td>
	    <td><asp:TextBox ID="tbAny" runat="server"></asp:TextBox></td>
	    <td></td>
	</tr>
	<tr>
	    <td></td>
	    <td>With <b>all</b> of the words</td>
	    <td><asp:TextBox ID="tbAll" runat="server"></asp:TextBox></td>
	    <td></td>
	</tr>
	<tr>
	    <td></td>
	    <td>With <b>the exact phrase</b></td>
	    <td><asp:TextBox ID="tbExact" runat="server"></asp:TextBox></td>
	    <td></td>
	</tr>
	<tr>
	    <td></td>
	    <td>search within</td>
	    <td>
            <asp:DropDownList ID="ddAdvancedSearchSection" runat="server">
		        <asp:ListItem Value="">Whole site</asp:ListItem>
                <asp:ListItem Value="shop">Shop</asp:ListItem>
                <asp:ListItem Value="news">News</asp:ListItem>
                <asp:ListItem Value="links">Links</asp:ListItem>
            </asp:DropDownList>	        
	    </td>
	    <td><asp:imagebutton id="imgGoAdvanced" runat="server"></asp:imagebutton></td>
	</tr>
</table>
        