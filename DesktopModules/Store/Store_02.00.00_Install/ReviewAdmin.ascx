<%@ Control language="c#" CodeBehind="ReviewAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ReviewAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<span><dnn:label id=lblParentTitle runat="server" controlname="lblParentTitle" visible="False"></dnn:label></span>
<table cellspacing="0" cellpadding="0" border="0" width="100%">
  <tbody>
    <asp:Panel id="panelList" Visible="true" runat="server">
    <tr>
      <td vAlign=top noWrap align=center><table cellSpacing=3 cellPadding=0 border=0>
          <tr>
            <td width="100" class="NormalBold" nowrap><dnn:label id=lblStatus runat="server" controlname="lblStatus" suffix=":" CssClass="NormalBold"></dnn:label>
            </td>
            <td><asp:DropDownList id=cmbStatus runat="server" Width="200" AutoPostBack="true" DataTextField="StatusName" DataValueField="StatusID" onselectedindexchanged="cmbStatus_SelectedIndexChanged"></asp:DropDownList>
            </td>
          </tr>
          <tr>
            <td width="100" class="NormalBold" nowrap><dnn:label id=lblCategory runat="server" controlname="lblCategory" suffix=":" CssClass="NormalBold"></dnn:label>
            </td>
            <td><asp:DropDownList id=cmbCategory runat="server" Width="200" AutoPostBack="true" DataTextField="CategoryName" DataValueField="CategoryID" onselectedindexchanged="cmbCategory_SelectedIndexChanged"></asp:DropDownList>
            </td>
          </tr>
          <tr>
            <td width="100" class="NormalBold" nowrap><dnn:label id=lblProduct runat="server" controlname="lblProduct" suffix=":" CssClass="NormalBold"></dnn:label>
            </td>
            <td><asp:DropDownList id=cmbProduct runat="server" Width="200" AutoPostBack="true" DataTextField="ProductTitle" DataValueField="ProductID" onselectedindexchanged="cmbProduct_SelectedIndexChanged"></asp:DropDownList>
            </td>
          </tr>
        </table>
        <br />
        <asp:datagrid id=grdReviews runat="server" showheader="true" showfooter="false" autogeneratecolumns="false" width="500" AllowPaging="True">
          <columns>
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblSubmitter" Runat=server resourcekey="lblSubmitter" cssclass="NormalBold">Submitter</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="labelUserName" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "UserName") %> </asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblProduct" Runat=server resourcekey="lblProduct" cssclass="NormalBold">Product</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="lblProduct2" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "ModelName") %> </asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblRating" Runat=server resourcekey="lblRating" cssclass="NormalBold">Rating</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:PlaceHolder id="phRating" runat="server" />
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblComments" Runat=server resourcekey="lblComments" cssclass="NormalBold">Comments</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="labelComments" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "Comments") %> </asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <ItemTemplate>
              <asp:HyperLink id="linkEdit" Text="Edit" runat="server" cssclass="Normal" resourcekey="linkEdit"></asp:HyperLink>
            </ItemTemplate>
          </asp:TemplateColumn>
          </columns>
          <PagerStyle Mode="NumericPages" HorizontalAlign="Center" CssClass="NormalBold"></PagerStyle>
        </asp:datagrid>
      </td>
    </tr>
    <tr>
      <td>&nbsp;</td>
    </tr>
    <tr>
      <td align=center><asp:linkbutton id=linkAddImage runat="server" cssclass="Normal" Visible="False">
          <asp:Image ID="imageAdd" Runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit" resourcekey="Edit" />
        </asp:linkbutton>
        <asp:linkbutton id=linkAddNew runat="server" cssclass="Normal" Visible="False" resourcekey="linkAddNew">Add Review</asp:linkbutton>
      </td>
    </tr>
    </asp:Panel>
  <asp:Panel id="panelEdit" Visible="false" runat="server">
    <tr>
      <td><asp:PlaceHolder id=editControl runat="server"></asp:PlaceHolder>
      </td>
    </tr>
    </asp:Panel>
  </tbody>
  
</table>
