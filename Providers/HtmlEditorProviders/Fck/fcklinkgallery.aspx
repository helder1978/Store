<%@ Page Language="vb" ValidateRequest="false" Trace="false" CodeBehind="fcklinkgallery.aspx.vb" AutoEventWireup="false" Inherits="DotNetNuke.HtmlEditor.FckHtmlEditorProvider.FCKLinkGallery" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<HTML>
	<HEAD id="Head">
		<title>
			<%= Title %>
		</title>
		<style id="StylePlaceholder" runat="server"></style>
		<asp:placeholder id="CSS" runat="server"></asp:placeholder>
		<asp:placeholder id="phDNNHead" runat="server"></asp:placeholder>
		<base target="_self">
	</HEAD>
	<body>
		<script language="javascript">
	function OpenFile( fileUrl )
{
	if (!window.top.opener) {
		window.returnValue = fileUrl;
		window.close();
	}
	else {
		window.top.opener.SetUrl( fileUrl ) ;
		window.top.close() ;
		window.top.opener.focus() ;
	}
}
		</script>
		<form id="Form1" runat="server" enctype="multipart/form-data">
			<TABLE id="Table2" class="FCKLinkGalleryContainer">
				<TR>
					<TD class="FCKLinkGalleryTitleContainer">
						<asp:Label id="lblTitle" cssclass="Head" runat="server">Link Gallery</asp:Label></TD>
				</TR>
				<TR>
					<TD class="FCKLinkGalleryLinksContainer">
						<TABLE id="Table1" Class="FCKLinkGalleryLinksTable">
							<TR>
								<TD class="SubHead" width="75">
									<asp:label id="plURL" runat="server" Text="URL:"></asp:label></TD>
								<TD class="Normal">
									<portal:url id="ctlURL" runat="server" width="250" shownewwindow="True"></portal:url></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD><asp:LinkButton id="cmdSelect" resourcekey="cmdSelect" cssclass="CommandButton" runat="server">Select created link</asp:LinkButton></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
