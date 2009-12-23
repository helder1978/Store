<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.SearchResults.SearchResults" CodeFile="SearchResults.ascx.vb" %>
<asp:Label ID="lblSearch" runat="server"></asp:Label>
<asp:Label ID="lblSearchResult" runat="server"></asp:Label>
<asp:Datagrid id="dgResults" runat="server" AutoGenerateColumns="False" AllowPaging="True" BorderStyle="None"
	PagerStyle-CssClass="NormalBold" ShowHeader="False" CellPadding="4" GridLines="None">
	<Columns>
		<asp:TemplateColumn>
			<ItemTemplate>
				<asp:Label id=lblNo runat="server" Text='<%# DataBinder.Eval(Container, "ItemIndex") + 1 %>' CssClass="SubHead">
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
			<ItemTemplate>
				<div style="float:left" >
				<asp:HyperLink id="lnkTitle" runat="server" CssClass="SubHead" NavigateUrl='<%# FormatURL(DataBinder.Eval(Container.DataItem,"TabId"),DataBinder.Eval(Container.DataItem,"Guid")) %>' Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
				</asp:HyperLink>&nbsp;-
				<asp:Label id="lblRelevance" runat="server" CssClass="Normal" Text='<%# FormatRelevance(DataBinder.Eval(Container.DataItem, "Relevance")) %>' >
				</asp:Label></div>
				<div style="float:right" align="right"><asp:Label id="lblPubDate" runat="server" CssClass="Normal" Text='<%# FormatDate(DataBinder.Eval(Container.DataItem, "PubDate")) %>'>
				</asp:Label>
				</div><BR>
				<asp:Label id="lblSummary" runat="server" CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "Description") + "<br>" %>' Visible="<%# ShowDescription() %>">
				</asp:Label>
				<asp:HyperLink id="lnkLink" runat="server" CssClass="CommandButton" NavigateUrl='<%# FormatURL(DataBinder.Eval(Container.DataItem,"TabId"),DataBinder.Eval(Container.DataItem,"Guid")) %>' Text='<%# FormatURL(DataBinder.Eval(Container.DataItem,"TabId"),DataBinder.Eval(Container.DataItem,"Guid")) %>'>
				</asp:HyperLink>&nbsp;-
				<asp:Label id="Label1" runat="server" CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "TabId")  %>' Visible="<%# ShowDescription() %>">
				</asp:Label>
				<asp:Label id="Label2" runat="server" CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "Guid") %>' Visible="<%# ShowDescription() %>">
				</asp:Label>
				
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
	<PagerStyle CssClass="NormalBold" Mode="NumericPages"></PagerStyle>
</asp:Datagrid>
