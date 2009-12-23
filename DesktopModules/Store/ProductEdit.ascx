<%@ Control Language="c#" AutoEventWireup="False" Codebehind="ProductEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ProductEdit" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<table width="500" border="0" align="center" cellspacing="5">
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelModelName" runat="server" controlname="labelModelName" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtModelName" Runat="server" Width="300" MaxLength="250" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelModelNumber" runat="server" controlname="labelModelNumber" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtModelNumber" Runat="server" Width="300" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelCategory1" runat="server" controlname="labelCategory1" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:DropDownList ID="cmbCategory1" Runat="server" Width="200" DataTextField="CategoryPathName" DataValueField="CategoryID"></asp:DropDownList>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelCategory2" runat="server" controlname="labelCategory2" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:DropDownList ID="cmbCategory2" Runat="server" Width="200" DataTextField="CategoryPathName" DataValueField="CategoryID"></asp:DropDownList>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelCategory3" runat="server" controlname="labelCategory3" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:DropDownList ID="cmbCategory3" Runat="server" Width="200" DataTextField="CategoryPathName" DataValueField="CategoryID"></asp:DropDownList>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelNumPages" runat="server" controlname="txtNumPages" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtNumPages" Runat="server" Width="100" MaxLength="25" CssClass="NormalTextBox"></asp:TextBox>
      <asp:CompareValidator id="validatorNumPages" runat="server" ErrorMessage="Error! Please enter a valid number of pages."
				resourcekey="validatorNumPages" Type="Integer" ControlToValidate="txtNumPages" Operator="DataTypeCheck"
				Display="Dynamic"></asp:CompareValidator>
      <asp:RequiredFieldValidator id="validatorRequireNumPages" runat="server" ControlToValidate="txtNumPages" ErrorMessage="* Number of pages is required."
				resourcekey="validatorRequireNumPages" Display="Dynamic"></asp:RequiredFieldValidator>
    </td>
  </tr>
  <tr valign="top">
      <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelPublishDate" runat="server" controlname="labelPublishDate" suffix=":"></dnn:label>
      </td>
      <td class="Normal" nowrap="nowrap">
        <asp:Panel ID="mypanel" runat="server">
            <asp:TextBox runat="server" ID="tbPublishDate" />
            <asp:ImageButton runat="Server" ID="imgPublishDate" Visible="false" ImageUrl="~/images/Calendar_scheduleHS.png" AlternateText="Click to show calendar" /><br />
            <ajaxToolkit:CalendarExtender ID="calendarButtonExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="tbPublishDate" PopupButtonID="imgPublishDate" />
        </asp:Panel>
      </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelDeliveryMethod" runat="server" controlname="labelDeliveryMethod" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap">
        <asp:DropDownList ID="cmbDeliveryMethod" Runat="server" Width="200">
            <asp:ListItem Value="3" Text="Both"></asp:ListItem>
            <asp:ListItem Value="1" Text="File Download only"></asp:ListItem>
            <asp:ListItem Value="2" Text="Hardcopy only"></asp:ListItem>
        </asp:DropDownList>
    </td>
  </tr>
  
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelUnitPrice" runat="server" controlname="txtUnitPrice" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtUnitPrice" Runat="server" Width="100" MaxLength="25" CssClass="NormalTextBox"></asp:TextBox>
      <asp:CompareValidator id="validatorUnitPrice" runat="server" ErrorMessage="Error! Please enter a valid price."
				resourcekey="validatorUnitPrice" Type="Currency" ControlToValidate="txtUnitPrice" Operator="DataTypeCheck"
				Display="Dynamic"></asp:CompareValidator>
      <asp:RequiredFieldValidator id="validatorRequireUnitPrice" runat="server" ControlToValidate="txtUnitPrice" ErrorMessage="* Price is required."
				resourcekey="validatorRequireUnitPrice" Display="Dynamic"></asp:RequiredFieldValidator>
    </td>
  </tr>

  <tr valign="top">
    <td colspan="2" class="NormalBold" nowrap="nowrap">Price text (max. 10 chars, overrides unit price):
    </td></tr>
    <tr>
    <td colspan="2" class="Normal" nowrap="nowrap">
        <asp:TextBox id="tbPriceStr" runat="server" Width="100" MaxLength="10" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>

  <tr valign="top">
    <td colspan="2" class="NormalBold" nowrap="nowrap"><dnn:label id="labelSummary" runat="server" controlname="labelSummary" suffix=":"></dnn:label>
    </td></tr>
    <tr>
    <td colspan="2" class="Normal" nowrap="nowrap">
        <dnn:TextEditor id="txtSummary" runat="server" width="500" height="300"></dnn:TextEditor>
    </td>
  </tr>

  <tr valign="top">
    <td colspan="2" class="NormalBold" nowrap="nowrap"><dnn:label id="labelTOC_Html" runat="server" Text="TOC (html version):"></dnn:label>
    </td></tr>
    <tr>
    <td colspan="2" class="Normal" nowrap="nowrap">
        <dnn:TextEditor id="txtTOC_Html" runat="server" width="500" height="300"></dnn:TextEditor>
    </td>
  </tr>

  <tr valign="top">
    <td colspan="2" class="NormalBold" nowrap="nowrap"><dnn:label id="labelDescriptionTag" runat="server" Text="Description tag (SEO):"></dnn:label>
    </td></tr>
    <tr>
    <td colspan="2" class="Normal" nowrap="nowrap">
        <asp:TextBox ID="tbDescriptionTag" Runat="server" Width="400" MaxLength="250" CssClass="NormalTextBox"></asp:TextBox>
    </td>
  </tr>

  <tr valign="top">
    <td class="NormalBold" colspan="2" nowrap="nowrap"><hr />
      Description
    </td>
  </tr>
  <tr>
    <td class="Normal" colspan="2" nowrap="nowrap">
        <dnn:TextEditor id="txtDescription" runat="server" width="500" height="300"></dnn:TextEditor>
    </td>
  </tr>

  <tr valign="top">
    <td class="NormalBold" colspan="2" nowrap="nowrap"><hr />
      Why Buy This Report?
    </td>
  </tr>
  <tr>
    <td class="Normal" colspan="2" nowrap="nowrap">
        <dnn:TextEditor id="txtDescriptionTwo" runat="server" width="500" height="250"></dnn:TextEditor>
    </td>
  </tr>

  <tr valign="top">
    <td class="NormalBold" colspan="2" nowrap="nowrap"><hr />
      Key Information Provided in This Report
    </td>
  </tr>
  <tr>
    <td class="Normal" colspan="2" nowrap="nowrap">
        <dnn:TextEditor id="txtDescriptionThree" runat="server" width="500" height="250"></dnn:TextEditor>
    </td>
  </tr>

  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelAvailableOnline" runat="server" controlname="labelAvailableOnline" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:CheckBox ID="chkAvailableOnline" Runat="server"></asp:CheckBox>
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelArchived" runat="server" controlname="labelArchived" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap"><asp:CheckBox ID="chkArchived" Runat="server"></asp:CheckBox>
    </td>
  </tr>
  <tr>
    <td colspan="2"><hr />
    </td>
  </tr>
  <tr>
    <td colspan="2"><dnn:sectionhead id="dshSpecialOffer" runat="server" resourcekey="dshSpecialOffer" cssclass="NormalBold" text="Special Offer Pricing"
				section="tblSpecialOffer" includerule="false" isexpanded="false"></dnn:sectionhead>
      <table  width="500" border="0" align="center" cellspacing="5" id="tblSpecialOffer" runat="server" >
        <tr valign="top">
          <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelSalePrice" runat="server" controlname="labelSalePrice" suffix=":"></dnn:label>
          </td>
          <td class="Normal" nowrap="nowrap"><asp:TextBox ID="txtSalePrice" Runat="server" Width="100" MaxLength="25" OnTextChanged="txtSalePrice_TextChanged" CssClass="NormalTextBox"></asp:TextBox>
            <asp:CompareValidator id="validatorSalePrice" runat="server" ErrorMessage="Error! Please enter a valid price."
				            resourcekey="validatorSalePrice" Type="Currency" ControlToValidate="txtSalePrice" Operator="DataTypeCheck"
				            Display="Dynamic"></asp:CompareValidator>
          </td>
        </tr>
        <tr valign="top">
          <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelSaleStartDate" runat="server" controlname="labelSaleStartDate" suffix=":"></dnn:label>
          </td>
          <td class="Normal" nowrap="nowrap"><asp:Calendar ID="calSaleStartDate" runat="server" CssClass="Normal" SelectionMode="Day" OnSelectionChanged="calSaleStartDate_SelectionChanged" OnVisibleMonthChanged="calSaleStartDate_VisibleMonthChanged"></asp:Calendar>
            <asp:Button ID="btnClearStartDate" runat="server" resourcekey="btnClearStartDate" Text="Clear start date" OnClick="btnClearStartDate_Click" CssClass="Normal" />
          </td>
        </tr>
        <tr valign="top">
          <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelSaleEndDate" runat="server" controlname="labelSaleEndDate" suffix=":"></dnn:label>
          </td>
          <td class="Normal" nowrap="nowrap"><asp:Calendar ID="calSaleEndDate" runat="server" CssClass="Normal" SelectionMode="Day" OnSelectionChanged="calSaleEndDate_SelectionChanged" OnVisibleMonthChanged="calSaleEndDate_VisibleMonthChanged"></asp:Calendar>
            <asp:Button ID="btnClearEndDate" runat="server" resourcekey="btnClearEndDate" Text="Clear end date" OnClick="btnClearEndDate_Click" CssClass="Normal" />
          </td>
        </tr>
      </table></td>
  </tr>
  <tr>
    <td colspan="2"><hr />
    </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelFile" runat="server" controlname="labelFile" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap">&nbsp;
      <asp:TextBox ID="tbFile1" runat="server" Visible="false"></asp:TextBox>
      <portal:URL id="file1" ShowUpLoad="True" showtabs="False" showfiles="True" showUrls="True" showlog="False" shownone="true" shownewwindow="False" showtrack="False" runat="server" width="300" />
     </td>
  </tr>
  <tr valign="top">
    <td class="NormalBold" nowrap="nowrap"><dnn:label id="labelPreview" runat="server" controlname="labelPreview" suffix=":"></dnn:label>
    </td>
    <td class="Normal" nowrap="nowrap">&nbsp;
      <asp:TextBox ID="tbPreview1" runat="server" Visible="false"></asp:TextBox>
      <portal:URL id="preview1" runat="server" width="300" />
     </td>
  </tr>


  <tr>
    <td colspan="2" align="center" nowrap="nowrap"><asp:linkbutton id="cmdUpdate" CssClass="CommandButton" runat="server" BorderStyle="None" resourcekey="cmdUpdate">Update</asp:linkbutton>
      <asp:linkbutton id="cmdCancel" CssClass="CommandButton" runat="server" CausesValidation="False"
				BorderStyle="None" resourcekey="cmdCancel">Cancel</asp:linkbutton>
      <asp:linkbutton id="cmdDelete" CssClass="CommandButton" runat="server" CausesValidation="False"
				BorderStyle="None" Visible="False" resourcekey="cmdDelete">Delete</asp:linkbutton>
    </td>
  </tr>
</table>
