<%@ Control language="c#" CodeBehind="DefaultAddressProfile.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider.DefaultAddressProfile" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnnstore" TagName="address" Src="~/DesktopModules/Store/Providers/AddressProviders/DefaultAddressProvider/StoreAddress.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="wc" Namespace="DotNetNuke.UI.WebControls" Assembly="CountryListBox" %>
<table cellspacing="0" cellpadding="0" width="100%" border="0">
	<tr>
		<td class="SubHead" align="center">
			<dnn:label id="lblGridTitle" controlname="lblGridTitle" runat="server" cssclass="SubHead"></dnn:label>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<asp:placeholder id="plhGrid" runat="server">
		<TR>
			<TD vAlign="top" noWrap>
				<asp:datagrid id="grdAddresses" runat="server" width="100%" autogeneratecolumns="false" showfooter="true"
					showheader="true">
					<columns>
						<asp:templatecolumn>
							<headertemplate>
								<asp:label id="Label1" runat="server" cssclass="NormalBold" resourcekey="lblDescription">Description</asp:label>
							</headertemplate>
							<itemtemplate>
								<asp:label id="Label2" runat="server" cssclass="Normal">
									<%# DataBinder.Eval(Container.DataItem, "Description") %>
								</asp:label>
							</itemtemplate>
						</asp:templatecolumn>
						<asp:templatecolumn headerstyle-width="50" headerstyle-horizontalalign="center" itemstyle-horizontalalign="center">
							<headertemplate>
								<asp:label id="Label3" runat="server" cssclass="NormalBold" resourcekey="lblPrimary">Primary</asp:label>
							</headertemplate>
							<itemtemplate>
								<asp:image id="imgPrimary" runat="server" cssclass="Normal"></asp:image>
							</itemtemplate>
						</asp:templatecolumn>
						<asp:templatecolumn headerstyle-width="50" headerstyle-horizontalalign="center" itemstyle-horizontalalign="center">
							<headertemplate>
								<span class="NormalBold">&nbsp;</span>
							</headertemplate>
							<itemtemplate>
								<asp:hyperlink id="lnkEdit" runat="server" cssclass="Normal" resourcekey="lnkEdit">Edit</asp:hyperlink>
							</itemtemplate>
						</asp:templatecolumn>
					</columns>
				</asp:datagrid></TD>
		</TR>
		<TR>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center">
				<asp:linkbutton id="btnAdd" tabIndex="1" cssclass="Normal" runat="server" resourcekey="btnAdd" onclick="btnAdd_Click">Add Address</asp:linkbutton><BR>
			</TD>
		</TR>
	</asp:placeholder>
	<asp:placeholder id="plhEditAddress" runat="server" visible="False">
		<TR>
			<TD align="center">
				<asp:label class="SubHead" id="lblEditTitle" runat="server"></asp:label></TD>
		</TR>
		<TR>
			<TD>&nbsp;</TD>
		</TR>
		<TR>
			<TD align="center">
				<dnnstore:address id="addressEdit" runat="server" ControlColumnWidth="300" StartTabIndex="2"></dnnstore:address>
				<TABLE cellSpacing="0" cellPadding="1" summary="Address Edit Controls" border="0">
					<TR id="rowPrimaryOption" runat="server" vAlign="top">
						<TD class="SubHead" vAlign="top" width="120">
							<dnn:label id="lblPrimary" runat="server" controlname="lblPrimary" suffix=":"></dnn:label></TD>
						<TD vAlign="top" width="300" noWrap>
							<asp:checkbox id="chkPrimary" tabIndex="20" runat="server"></asp:checkbox></TD>
					</TR>
					<TR>
						<TD align="center" colSpan="2">
							<asp:linkbutton id="cmdUpdate" tabIndex="21" cssclass="CommandButton" runat="server" resourcekey="cmdUpdate"
								borderstyle="None" onclick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;
							<asp:linkbutton id="cmdCancel" tabIndex="22" cssclass="CommandButton" runat="server" resourcekey="cmdCancel"
								borderstyle="None" causesvalidation="False" onclick="cmdCancel_Click">Cancel</asp:linkbutton>&nbsp;
							<asp:linkbutton id="cmdDelete" tabIndex="23" cssclass="CommandButton" runat="server" resourcekey="cmdDelete"
								visible="False" borderstyle="None" causesvalidation="False" onclick="cmdDelete_Click">Delete</asp:linkbutton></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
	</asp:placeholder>
</table>
