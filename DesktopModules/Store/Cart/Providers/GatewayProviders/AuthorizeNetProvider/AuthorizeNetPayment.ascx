<%@ Control language="c#" CodeBehind="AuthorizeNetPayment.ascx.cs" Inherits="DotNetNuke.Modules.Store.Cart.AuthorizeNetPayment" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" width="78%">
    <tr>
    <td>
    <fieldset>
				<legend>
					<dnn:label id="lblPaymentTitle" runat="server" control="lblPaymentTitle"></dnn:label>
				</legend>
	<TABLE cellSpacing="0" cellPadding="0" border="0">
	    <TR runat="server" id="tr1" visible="false">	    
		    <TD nowrap="nowrap">
			    <asp:Label id="lblCard" CssClass="SubHead" runat="server">Card Type:</asp:Label></TD>
		    <TD>
			    <TABLE id="Table3" cellSpacing="0" cellPadding="0" width="200" border="0">
				    <TR>
					    <TD>
						    <asp:RadioButtonList id="rbCard" CssClass="Normal" runat="server" Width="160px" RepeatDirection="Horizontal">
							    <asp:ListItem Value="Visa" Selected="true">Visa</asp:ListItem>
							    <asp:ListItem Value="MasterCard">MasterCard</asp:ListItem>
						    </asp:RadioButtonList></TD>
					    <TD>
						    <asp:RequiredFieldValidator id="rfvCard" runat="server" ControlToValidate="rbCard">*</asp:RequiredFieldValidator></TD>
				    </TR>
			    </TABLE>
		    </TD>
	    </TR>
	    <TR runat="server" visible="false" id="tr2">
		    <TD nowrap="nowrap">
			    <asp:Label id="lblName" CssClass="SubHead" runat="server">Name on Card:</asp:Label></TD>
		    <TD width="100%">
			    <asp:TextBox id="txtName" CssClass="Normal" runat="server" Width="200px" MaxLength="255"></asp:TextBox>
			    <asp:RequiredFieldValidator id="rfvName" runat="server" ControlToValidate="txtName">*</asp:RequiredFieldValidator></TD>
	    </TR>
	    <TR>
		    <TD nowrap="nowrap" class="SubHead" valign="top">
			   <dnn:label id="lblCardNumber" runat="server" control="txtNumber" suffix=":"></dnn:label></TD>
		    <TD width="100%" valign="top">
			    <asp:TextBox id="txtNumber" CssClass="Normal" runat="server" Width="130px" MaxLength="100"></asp:TextBox>
			    <asp:RequiredFieldValidator id="rfvNumber" runat="server" ControlToValidate="txtNumber">* Required</asp:RequiredFieldValidator></TD>
	    </TR>
	    <TR>
		    <TD nowrap="nowrap" class="SubHead" valign="top">
			    <dnn:label id="lblExpiryDate" runat="server" control="ddlMonth"  suffix=":"></dnn:label></TD>
		    <TD width="100%" valign="top">
			    <asp:DropDownList id="ddlMonth" CssClass="Normal" runat="server">
				    <asp:ListItem Value="01">01</asp:ListItem>
				    <asp:ListItem Value="02">02</asp:ListItem>
				    <asp:ListItem Value="03">03</asp:ListItem>
				    <asp:ListItem Value="04">04</asp:ListItem>
				    <asp:ListItem Value="05">05</asp:ListItem>
				    <asp:ListItem Value="06">06</asp:ListItem>
				    <asp:ListItem Value="07">07</asp:ListItem>
				    <asp:ListItem Value="08">08</asp:ListItem>
				    <asp:ListItem Value="09">09</asp:ListItem>
				    <asp:ListItem Value="10">10</asp:ListItem>
				    <asp:ListItem Value="11">11</asp:ListItem>
				    <asp:ListItem Value="12">12</asp:ListItem>
			    </asp:DropDownList>
			    <asp:Label id="lblSlash" CssClass="Normal" runat="server">&nbsp;/&nbsp;</asp:Label>
			    <asp:DropDownList id="ddlYear" CssClass="Normal" runat="server"></asp:DropDownList></TD>
	    </TR>
	    <TR>
		    <TD nowrap="nowrap" class="SubHead" valign="top">
			    <dnn:label id="lblCSC" runat="server" control="txtVer" suffix=":"></dnn:label></TD>
		    <TD vAlign="top"  width="100%">
			    <asp:TextBox id="txtVer" CssClass="Normal" runat="server" Width="60px" MaxLength="3"></asp:TextBox>
			    <asp:RequiredFieldValidator id="rfvVer" runat="server" ControlToValidate="txtVer">* Required</asp:RequiredFieldValidator></TD>
	    </TR>
	    <TR>
		    <TD colspan="2" align="center" class="NormalRed">
			    <asp:Literal id="litError" runat="Server"></asp:Literal>
		    </TD>
	    </TR>	    
	</TABLE>
	</fieldset>
	</td>
	</tr>
	<TR>
		<TD colspan="2" align="center">
		    <p align="left">
                <br />
                <asp:Label id="lblConfirmMessage" runat="server" CssClass="Normal"></asp:Label>        
            </p>
			<asp:button id="btnProcess" runat="server" resourcekey="btnProcess" cssclass="Normal" Text="Confirm Order" onclick="btnProcess_Click"></asp:button>
		</TD>
	</TR>
</TABLE>
