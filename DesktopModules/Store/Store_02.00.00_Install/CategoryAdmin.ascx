<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="c#" CodeBehind="CategoryAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CategoryAdmin" AutoEventWireup="True" %>

<span><dnn:label id="lblParentTitle" runat="server" controlname="lblParentTitle" visible="False"></dnn:label></span>
<table cellspacing="0" cellpadding="0" border="0" width="100%">
  <tbody>
    <asp:placeholder id="plhGrid" runat="server">
      <tr>
        <td valign="top" nowrap="nowrap" align="center"><asp:datagrid id="grdCategories" runat="server" showheader="true" showfooter="false" autogeneratecolumns="false"
						width="500" AllowPaging="True" CellPadding="5">
            <columns>
            <asp:TemplateColumn>
              <HeaderTemplate>
                <asp:Label ID="lblPathNameHeader" Runat="server" cssclass="NormalBold" resourcekey="lblPathNameHeader">Path</asp:Label>
              </HeaderTemplate>
              <ItemTemplate>
                <asp:label id="lblPathName" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "CategoryPathName") %> </asp:label>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
              <HeaderTemplate>
                <asp:Label ID="lblNameHeader" Runat="server" cssclass="NormalBold" resourcekey="lblNameHeader">Name</asp:Label>
              </HeaderTemplate>
              <ItemTemplate>
                <asp:label id="lblName" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "CategoryName") %> </asp:label>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
              <HeaderTemplate>
                <asp:Label ID="lblParentCategory" runat="server" CssClass="NormalBold" resourcekey="lblParentCategory">Parent</asp:Label>
              </HeaderTemplate>
              <ItemTemplate>
                <asp:label id="lblParentCategory" runat="server" cssclass="Normal">Parent Category Name</asp:label>
              </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
              <HeaderTemplate> <span class="NormalBold">&nbsp;</span> </HeaderTemplate>
              <ItemTemplate>
                <asp:hyperlink id="linkEdit" runat="server" cssclass="Normal" resourcekey="linkEdit">Edit</asp:hyperlink>
              </ItemTemplate>
            </asp:TemplateColumn>
            </columns>
            <PagerStyle Mode="NumericPages" HorizontalAlign="Center" CssClass="NormalBold"></PagerStyle>
          </asp:datagrid></td>
      </tr>
      <tr>
        <td>&nbsp;</td>
      </tr>
      <tr>
        <td align="center"><asp:linkbutton id="linkAddImage" runat="server" cssclass="Normal">
            <asp:Image ID="imageAdd" Runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit" resourcekey="Edit" />
          </asp:linkbutton>
          <asp:linkbutton id="linkAddNew" runat="server" resourcekey="linkAddNew" cssclass="Normal">Add Category</asp:linkbutton>
          <br />
        <br />        </td>
      </tr>
    </asp:placeholder>
    <asp:placeholder id="plhForm" runat="server" visible="false">
      <tr>
        <td align="center"><asp:label id="lblEditTitle" runat="server" cssclass="SubHead"></asp:label></td>
      </tr>
      <tr>
        <td>&nbsp;</td>
      </tr>
      <tr>
        <td><asp:placeholder id="plhEditControl" runat="server"></asp:placeholder></td>
      </tr>
      <tr>
        <td>&nbsp;</td>
      </tr>
    </asp:placeholder>
  </tbody>
</table>
