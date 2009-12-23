<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CategoryEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CategoryEdit" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>

<table width="500" border="0" align="center" cellSpacing="5">
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelCategoryName" runat="server" controlname="labelCategoryName" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtCategoryName" Runat="server" Width="200" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox>
      <asp:RequiredFieldValidator ID="valReqCategoryName" runat="server" ControlToValidate="txtCategoryName"
                Display="Dynamic" ErrorMessage="* Category name is required." resourcekey="valReqCategoryName"></asp:RequiredFieldValidator>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelParentCategory" runat="server" controlname="labelParentCategory" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:DropDownList ID="ddlParentCategory" runat="server"></asp:DropDownList>
      <br />
      <asp:Label ID="lblRecursionWarning" runat="server" Font-Bold="true" ForeColor="red"
                resourcekey="lblRecursionWarning" Text="Recursive category relationship detected.  Please specify a <br/>different parent category."
                Visible="false"></asp:Label>
    </td>
  </tr>
  <tr>
    <td colspan="2" align="center" nowrap="nowrap"><asp:linkbutton id="cmdUpdate" CssClass="CommandButton" runat="server" 
				BorderStyle="None" resourcekey="cmdUpdate" onclick="cmdUpdate_Click">Update</asp:linkbutton>
      <asp:linkbutton id="cmdCancel" CssClass="CommandButton" runat="server" CausesValidation="False"
				BorderStyle="None" resourcekey="cmdCancel" onclick="cmdCancel_Click">Cancel</asp:linkbutton>
      <asp:linkbutton id="cmdDelete" CssClass="CommandButton" runat="server" CausesValidation="False"
				BorderStyle="None" Visible="False" resourcekey="cmdDelete" onclick="cmdDelete_Click">Delete</asp:linkbutton>
    </td>
  </tr>
</table>
