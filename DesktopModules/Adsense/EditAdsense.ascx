<%@ Control language="vb" CodeFile="EditAdsense.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Adsense.EditAdsense" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellSpacing="2" cellPadding="0" width="560" summary="AdClient Design Table">
	<tr valign="bottom">
		<td class="SubHead" width="125" vAlign="bottom"><dnn:label id="plAdClient" runat="server" controlname="txtAdClient" suffix=":"></dnn:label></td>
		<td vAlign="top">
			<asp:LinkButton ID="cmdAdClient" runat="server" CssClass="CommandButton" CausesValidation="False" resourcekey="cmdAdClient" BorderStyle="none" Visible="False"></asp:LinkButton><br />
			<asp:TextBox id="txtAdClient" Runat="server" CssClass="NormalTextBox" Columns="30" Width="250px"></asp:TextBox><br />
			<asp:RequiredFieldValidator id="valAdClient" ControlToValidate="txtAdClient" Display="Dynamic" resourcekey="valAdClient.Message" runat="server" CssClass="NormalRed" />
		</td>
	</tr>
</table>
<table id="tblAdsense" runat="server" cellSpacing="2" cellPadding="0" width="560" summary="Adsense Design Table">
	<tr>
	    <td colspan="2">&nbsp;</td>
	</tr>
	<tr valign="bottom">
		<td class="SubHead" width="125" vAlign="top"><dnn:label id="plFormat" runat="server" controlname="cboFormat" suffix=":"></dnn:label></td>
		<td vAlign="top">
		    <asp:DropDownList ID="cboFormat" runat="server" CssClass="NormalTextBox" Width="250px" AutoPostBack="True">
		        <asp:ListItem Value="">Horizontal</asp:ListItem>  
		        <asp:ListItem Value="728x90">-- 728 x 90 Leaderboard</asp:ListItem>  
		        <asp:ListItem Value="468x60">-- 468 x 60 Banner</asp:ListItem>  
		        <asp:ListItem Value="234x60">-- 234 x 60 Half Banner</asp:ListItem>
		        <asp:ListItem Value="">Vertical</asp:ListItem>  
		        <asp:ListItem Value="120x600">-- 120 x 600 Skyscraper</asp:ListItem>  
		        <asp:ListItem Value="160x600">-- 160 x 600 Wide Skyscraper</asp:ListItem>  
		        <asp:ListItem Value="120x240">-- 120 x 240 Vertical Banner</asp:ListItem>
		        <asp:ListItem Value="">Square</asp:ListItem>  
		        <asp:ListItem Value="336x280">-- 336 x 280 Large Rectangle</asp:ListItem>  
		        <asp:ListItem Value="300x250">-- 300 x 250 Medium Rectangle</asp:ListItem>  
		        <asp:ListItem Value="250x250">-- 250 x 250 Square</asp:ListItem>  
		        <asp:ListItem Value="200x200">-- 200 x 200 Small Square</asp:ListItem>  
		        <asp:ListItem Value="180x150">-- 180 x 150 Small Rectangle</asp:ListItem>  
		        <asp:ListItem Value="125x125">-- 125 x 125 Button</asp:ListItem>
		    </asp:DropDownList>
		</td>
	</tr>
	<tr>
	    <td colspan="2">&nbsp;</td>
	</tr>
	<tr valign="bottom">
		<td class="SubHead" width="125" vAlign="top"><dnn:label id="plChannel" runat="server" controlname="txtChannel" suffix=":"></dnn:label></td>
		<td vAlign="top">
		    <asp:TextBox ID="txtChannel" runat="server" CssClass="NormalTextBox" Width="250px"></asp:TextBox>
		</td>
	</tr>
	<tr>
	    <td colspan="2">&nbsp;</td>
	</tr>
	<tr valign="bottom">
		<td class="SubHead" width="125" vAlign="top"><dnn:label id="plBorder" runat="server" controlname="cboBorder" suffix=":"></dnn:label></td>
		<td vAlign="top">
		    <asp:DropDownList ID="cboBorder" runat="server" CssClass="NormalTextBox" Width="100px" AutoPostBack="True">
		        <asp:ListItem Value="" resourcekey="Not_Specified"></asp:ListItem>
		        <asp:ListItem Value="008000" resourcekey="Green">Green</asp:ListItem>
		        <asp:ListItem Value="00FF00" resourcekey="Lime">Lime</asp:ListItem>
		        <asp:ListItem Value="00FFFF" resourcekey="Aqua">Aqua</asp:ListItem>
		        <asp:ListItem Value="000080" resourcekey="Navy">Navy</asp:ListItem>
		        <asp:ListItem Value="0000FF" resourcekey="Blue">Blue</asp:ListItem>
		        <asp:ListItem Value="800080" resourcekey="Purple">Purple</asp:ListItem>
		        <asp:ListItem Value="FF00FF" resourcekey="Pink">Pink</asp:ListItem>
		        <asp:ListItem Value="FF0000" resourcekey="Red">Red</asp:ListItem>
		        <asp:ListItem Value="FFA500" resourcekey="Orange">Orange</asp:ListItem>
		        <asp:ListItem Value="FFFF00" resourcekey="Yellow">Yellow</asp:ListItem>
		        <asp:ListItem Value="FFFFFF" resourcekey="White">White</asp:ListItem>
		        <asp:ListItem Value="C0C0C0" resourcekey="Silver">Silver</asp:ListItem>
		        <asp:ListItem Value="808080" resourcekey="Gray">Gray</asp:ListItem>
		        <asp:ListItem Value="000000" resourcekey="Black">Black</asp:ListItem>
		    </asp:DropDownList>
		    <asp:Label ID="lblBorder" runat="server" CssClass="Normal" resourcekey="lblOr" /><asp:TextBox ID="txtBorder" runat="server" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
		</td>
	</tr>
	<tr valign="bottom">
		<td class="SubHead" width="125" vAlign="top"><dnn:label id="plTitle" runat="server" controlname="cboTitle" suffix=":"></dnn:label></td>
		<td vAlign="top">
		    <asp:DropDownList ID="cboTitle" runat="server" CssClass="NormalTextBox" Width="100px" AutoPostBack="True">
		        <asp:ListItem Value="" resourcekey="Not_Specified"></asp:ListItem>
		        <asp:ListItem Value="008000" resourcekey="Green">Green</asp:ListItem>
		        <asp:ListItem Value="00FF00" resourcekey="Lime">Lime</asp:ListItem>
		        <asp:ListItem Value="00FFFF" resourcekey="Aqua">Aqua</asp:ListItem>
		        <asp:ListItem Value="000080" resourcekey="Navy">Navy</asp:ListItem>
		        <asp:ListItem Value="0000FF" resourcekey="Blue">Blue</asp:ListItem>
		        <asp:ListItem Value="800080" resourcekey="Purple">Purple</asp:ListItem>
		        <asp:ListItem Value="FF00FF" resourcekey="Pink">Pink</asp:ListItem>
		        <asp:ListItem Value="FF0000" resourcekey="Red">Red</asp:ListItem>
		        <asp:ListItem Value="FFA500" resourcekey="Orange">Orange</asp:ListItem>
		        <asp:ListItem Value="FFFF00" resourcekey="Yellow">Yellow</asp:ListItem>
		        <asp:ListItem Value="FFFFFF" resourcekey="White">White</asp:ListItem>
		        <asp:ListItem Value="C0C0C0" resourcekey="Silver">Silver</asp:ListItem>
		        <asp:ListItem Value="808080" resourcekey="Gray">Gray</asp:ListItem>
		        <asp:ListItem Value="000000" resourcekey="Black">Black</asp:ListItem>
		    </asp:DropDownList>
		    <asp:Label ID="lblTitle" runat="server" CssClass="Normal" resourcekey="lblOr" /><asp:TextBox ID="txtTitle" runat="server" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
		</td>
	</tr>
	<tr valign="bottom">
		<td class="SubHead" width="125" vAlign="top"><dnn:label id="plBackground" runat="server" controlname="cboBackground" suffix=":"></dnn:label></td>
		<td vAlign="top">
		    <asp:DropDownList ID="cboBackground" runat="server" CssClass="NormalTextBox" Width="100px" AutoPostBack="True">
		        <asp:ListItem Value="" resourcekey="Not_Specified"></asp:ListItem>
		        <asp:ListItem Value="008000" resourcekey="Green">Green</asp:ListItem>
		        <asp:ListItem Value="00FF00" resourcekey="Lime">Lime</asp:ListItem>
		        <asp:ListItem Value="00FFFF" resourcekey="Aqua">Aqua</asp:ListItem>
		        <asp:ListItem Value="000080" resourcekey="Navy">Navy</asp:ListItem>
		        <asp:ListItem Value="0000FF" resourcekey="Blue">Blue</asp:ListItem>
		        <asp:ListItem Value="800080" resourcekey="Purple">Purple</asp:ListItem>
		        <asp:ListItem Value="FF00FF" resourcekey="Pink">Pink</asp:ListItem>
		        <asp:ListItem Value="FF0000" resourcekey="Red">Red</asp:ListItem>
		        <asp:ListItem Value="FFA500" resourcekey="Orange">Orange</asp:ListItem>
		        <asp:ListItem Value="FFFF00" resourcekey="Yellow">Yellow</asp:ListItem>
		        <asp:ListItem Value="FFFFFF" resourcekey="White">White</asp:ListItem>
		        <asp:ListItem Value="C0C0C0" resourcekey="Silver">Silver</asp:ListItem>
		        <asp:ListItem Value="808080" resourcekey="Gray">Gray</asp:ListItem>
		        <asp:ListItem Value="000000" resourcekey="Black">Black</asp:ListItem>
		    </asp:DropDownList>
		    <asp:Label ID="lblBackground" runat="server" CssClass="Normal" resourcekey="lblOr" /><asp:TextBox ID="txtBackground" runat="server" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
		</td>
	</tr>
	<tr valign="bottom">
		<td class="SubHead" width="125" vAlign="top"><dnn:label id="plText" runat="server" controlname="cboText" suffix=":"></dnn:label></td>
		<td vAlign="top">
		    <asp:DropDownList ID="cboText" runat="server" CssClass="NormalTextBox" Width="100px" AutoPostBack="True">
		        <asp:ListItem Value="" resourcekey="Not_Specified"></asp:ListItem>
		        <asp:ListItem Value="008000" resourcekey="Green">Green</asp:ListItem>
		        <asp:ListItem Value="00FF00" resourcekey="Lime">Lime</asp:ListItem>
		        <asp:ListItem Value="00FFFF" resourcekey="Aqua">Aqua</asp:ListItem>
		        <asp:ListItem Value="000080" resourcekey="Navy">Navy</asp:ListItem>
		        <asp:ListItem Value="0000FF" resourcekey="Blue">Blue</asp:ListItem>
		        <asp:ListItem Value="800080" resourcekey="Purple">Purple</asp:ListItem>
		        <asp:ListItem Value="FF00FF" resourcekey="Pink">Pink</asp:ListItem>
		        <asp:ListItem Value="FF0000" resourcekey="Red">Red</asp:ListItem>
		        <asp:ListItem Value="FFA500" resourcekey="Orange">Orange</asp:ListItem>
		        <asp:ListItem Value="FFFF00" resourcekey="Yellow">Yellow</asp:ListItem>
		        <asp:ListItem Value="FFFFFF" resourcekey="White">White</asp:ListItem>
		        <asp:ListItem Value="C0C0C0" resourcekey="Silver">Silver</asp:ListItem>
		        <asp:ListItem Value="808080" resourcekey="Gray">Gray</asp:ListItem>
		        <asp:ListItem Value="000000" resourcekey="Black">Black</asp:ListItem>
		    </asp:DropDownList>
		    <asp:Label ID="lblText" runat="server" CssClass="Normal" resourcekey="lblOr" /><asp:TextBox ID="txtText" runat="server" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
		</td>
	</tr>
	<tr valign="bottom">
		<td class="SubHead" width="125" vAlign="top"><dnn:label id="plURL" runat="server" controlname="cboURL" suffix=":"></dnn:label></td>
		<td vAlign="top">
		    <asp:DropDownList ID="cboURL" runat="server" CssClass="NormalTextBox" Width="100px" AutoPostBack="True">
		        <asp:ListItem Value="" resourcekey="Not_Specified"></asp:ListItem>
		        <asp:ListItem Value="008000" resourcekey="Green">Green</asp:ListItem>
		        <asp:ListItem Value="00FF00" resourcekey="Lime">Lime</asp:ListItem>
		        <asp:ListItem Value="00FFFF" resourcekey="Aqua">Aqua</asp:ListItem>
		        <asp:ListItem Value="000080" resourcekey="Navy">Navy</asp:ListItem>
		        <asp:ListItem Value="0000FF" resourcekey="Blue">Blue</asp:ListItem>
		        <asp:ListItem Value="800080" resourcekey="Purple">Purple</asp:ListItem>
		        <asp:ListItem Value="FF00FF" resourcekey="Pink">Pink</asp:ListItem>
		        <asp:ListItem Value="FF0000" resourcekey="Red">Red</asp:ListItem>
		        <asp:ListItem Value="FFA500" resourcekey="Orange">Orange</asp:ListItem>
		        <asp:ListItem Value="FFFF00" resourcekey="Yellow">Yellow</asp:ListItem>
		        <asp:ListItem Value="FFFFFF" resourcekey="White">White</asp:ListItem>
		        <asp:ListItem Value="C0C0C0" resourcekey="Silver">Silver</asp:ListItem>
		        <asp:ListItem Value="808080" resourcekey="Gray">Gray</asp:ListItem>
		        <asp:ListItem Value="000000" resourcekey="Black">Black</asp:ListItem>
		    </asp:DropDownList>
		    <asp:Label ID="lblURL" runat="server" CssClass="Normal" resourcekey="lblOr" /><asp:TextBox ID="txtURL" runat="server" CssClass="NormalTextBox" Width="100px"></asp:TextBox>
		</td>
	</tr>
</table>
<p>
	<asp:LinkButton id="cmdUpdate" Text="Update" resourcekey="cmdUpdate" runat="server" class="CommandButton" BorderStyle="none" />&nbsp;&nbsp;
	<asp:LinkButton id="cmdCancel" Text="Cancel" resourcekey="cmdCancel" runat="server" class="CommandButton" BorderStyle="none" CausesValidation="false" />&nbsp;&nbsp;
	<asp:LinkButton id="cmdPreview" Text="Preview" resourcekey="cmdPreview" runat="server" class="CommandButton" BorderStyle="none" CausesValidation="false" />
</p>
<asp:Label ID="lblPreview" runat="server"></asp:Label> 
