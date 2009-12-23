<%@ Control Language="c#" CodeBehind="ReviewList.ascx.cs" AutoEventWireup="True" Inherits="DotNetNuke.Modules.Store.WebControls.ReviewList" targetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%--

    This user control display a list of review for a specific product.

--%>

<table cellspacing="0" cellpadding="0" width="100%" border="0">
  <tr>
    <td align="right"><asp:linkbutton id="btnAddReview" CssClass="CommandButton" Text="Add Review" resourcekey="AddReview" runat="server" onclick="btnAddReview_Click"></asp:linkbutton>
      <br />
    </td>
  </tr>
  <tr>
    <td><asp:datalist id="lstReviews" runat="server" cellspacing="0" cellpadding="0" Width="100%">
        <ItemTemplate>
          <table cellpadding="0" cellspacing="0" width="100%" border="0">
            <tr>
              <td nowrap="nowrap"><asp:Label class="NormalBold" Text='<%# DataBinder.Eval(Container.DataItem, "Username") %>' runat="server" ID="Label1" NAME="Label1" />
                <asp:Label ID="lblSays" Runat=server resourcekey="lblSays" class="Normal">says... </asp:Label>
              </td>
              <td width="100%"><asp:PlaceHolder ID="plhRating" Runat="server"></asp:PlaceHolder>
              </td>
            </tr>
            <tr>
              <td colspan="2"><p><asp:Label class="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "Comments") %>' runat="server" ID="Label2" NAME="Label2" /></p>
              </td>
            </tr>
          </table>
        </ItemTemplate>
        <SeparatorTemplate> <hr  />
        </SeparatorTemplate>
      </asp:datalist>
    </td>
  </tr>
</table>
