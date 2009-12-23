<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CategoryMenu.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CategoryMenu" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<div class="Store-CategoryMenu-Entity">
<div class="Store-CategoryMenu-Header"></div>
  <asp:datalist id="MyList" runat="server" cellpadding="3" cellspacing="0" Width="100%" datakeyfield="CategoryID"
	RepeatColumns="1">
    <SelectedItemStyle ></SelectedItemStyle>
    <SelectedItemTemplate>
      <asp:linkbutton id="linkEdit1" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CategoryID") %>' runat="server">
        <asp:Image ID="imageEdit1" Runat=server ImageUrl="~/images/edit.gif" AlternateText="Edit" Visible="<%# IsEditable %>" resourcekey="Edit" />
      </asp:linkbutton>
	  <span class="Store-CategoryMenu-ItemSelected">
      <asp:HyperLink id="linkCategory" CssClass="NormalBold" Runat="server" Font-Bold="true"> <%# DataBinder.Eval(Container.DataItem, "CategoryName") %> </asp:HyperLink></span>
    </SelectedItemTemplate>
    <ItemTemplate>
      <asp:linkbutton id="linkEdit2" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CategoryID") %>' runat="server">
        <asp:Image ID=imageEdit2 Runat=server ImageUrl="~/images/edit.gif" AlternateText="Edit" Visible="<%# IsEditable %>" resourcekey="Edit" />
      </asp:linkbutton>
	    <span class="Store-CategoryMenu-Item">
      <asp:HyperLink id="linkCategory" CssClass="NormalBold" Runat="server" Font-Bold="true"> <%# DataBinder.Eval(Container.DataItem, "CategoryName") %> </asp:HyperLink></span>
    </ItemTemplate>
  </asp:datalist>
  <div class="Store-CategoryMenu-Footer"></div>
</div>
