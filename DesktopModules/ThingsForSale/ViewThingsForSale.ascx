<%@ Control  language="vb" Inherits="YourCompany.Modules.ThingsForSale.ViewThingsForSale"  CodeFile="ViewThingsForSale.ascx.vb"  AutoEventWireup="false"  Explicit="True" %>
<%@ Register Assembly="DotNetNuke.WebUtility" Namespace="DotNetNuke.UI.Utilities" TagPrefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %> <br />

<asp:ObjectDataSource ID="ObjectDataSource_ThingsForSale" runat="server" DataObjectTypeName="YourCompany.Modules.ThingsForSale.ThingsForSaleInfo"
DeleteMethod="ThingsForSale_Delete" InsertMethod="ThingsForSale_Insert" OldValuesParameterFormatString="original_{0}"
OnInit="Page_Load" SelectMethod="ThingsForSale_SelectAll" TypeName="YourCompany.Modules.ThingsForSale.ThingsForSaleController"
UpdateMethod="ThingsForSale_Update">
<SelectParameters>
<asp:Parameter DefaultValue="00" Name="ModuleId" Type="Int32" />
</SelectParameters>
</asp:ObjectDataSource>


<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
DataSourceID="ObjectDataSource_ThingsForSale" CellPadding="4" CellSpacing="1" EnableViewState="False">
<Columns>
    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
    <asp:BoundField DataField="UserID" HeaderText="UserID" SortExpression="UserID" />
    <asp:BoundField DataField="ModuleId" HeaderText="ModuleId" SortExpression="ModuleId" />
    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
    <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
</Columns>
<EmptyDataTemplate>
There are no Things 4 Sale
</EmptyDataTemplate>
</asp:GridView>


<br />
<asp:LinkButton ID="Add_My_Listing_LinkButton" runat="server" EnableViewState="False">Add My Listing</asp:LinkButton><br />
<br />


<asp:FormView ID="FormView1" runat="server" DataSourceID="ObjectDataSource_ThingsForSale"
DefaultMode="Insert" BorderColor="DarkGray" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" Visible="False">
<EditItemTemplate>
    Category:
    <asp:TextBox ID="CategoryTextBox" runat="server" Text='<%# Bind("Category") %>'>
    </asp:TextBox><br />
    UserID:
    <asp:TextBox ID="UserIDTextBox" runat="server" Text='<%# Bind("UserID") %>'>
    </asp:TextBox><br />
    ModuleId:
    <asp:TextBox ID="ModuleIdTextBox" runat="server" Text='<%# Bind("ModuleId") %>'>
    </asp:TextBox><br />
    ID:
    <asp:TextBox ID="IDTextBox" runat="server" Text='<%# Bind("ID") %>'>
    </asp:TextBox><br />
    Price:
    <asp:TextBox ID="PriceTextBox" runat="server" Text='<%# Bind("Price") %>'>
    </asp:TextBox><br />
    Description:
    <asp:TextBox ID="DescriptionTextBox" runat="server" Text='<%# Bind("Description") %>'>
    </asp:TextBox><br />
    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
        Text="Update">
    </asp:LinkButton>
    <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
        Text="Cancel">
    </asp:LinkButton>
</EditItemTemplate>
<InsertItemTemplate>
Category:
    <asp:TextBox ID="CategoryTextBox" runat="server" Text='<%# Bind("Category") %>'>
    </asp:TextBox><br />
    UserID:
    <asp:TextBox ID="UserIDTextBox" runat="server" Text='<%# Bind("UserID") %>'>
    </asp:TextBox><br />
    ModuleId:
    <asp:TextBox ID="ModuleIdTextBox" runat="server" Text='<%# Bind("ModuleId") %>'>
    </asp:TextBox><br />
    ID:
    <asp:TextBox ID="IDTextBox" runat="server" Text='<%# Bind("ID") %>'>
    </asp:TextBox><br />
    Price:
    <asp:TextBox ID="PriceTextBox" runat="server" Text='<%# Bind("Price") %>'>
    </asp:TextBox><br />
    Description:
    <asp:TextBox ID="DescriptionTextBox" runat="server" Text='<%# Bind("Description") %>'>
    </asp:TextBox><br />
    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
        Text="Insert">
    </asp:LinkButton>
    <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
        Text="Cancel">
    </asp:LinkButton>
</InsertItemTemplate>
<ItemTemplate>
    Category:
    <asp:Label ID="CategoryLabel" runat="server" Text='<%# Bind("Category") %>'></asp:Label><br />
    UserID:
    <asp:Label ID="UserIDLabel" runat="server" Text='<%# Bind("UserID") %>'></asp:Label><br />
    ModuleId:
    <asp:Label ID="ModuleIdLabel" runat="server" Text='<%# Bind("ModuleId") %>'></asp:Label><br />
    ID:
    <asp:Label ID="IDLabel" runat="server" Text='<%# Bind("ID") %>'></asp:Label><br />
    Price:
    <asp:Label ID="PriceLabel" runat="server" Text='<%# Bind("Price") %>'></asp:Label><br />
    Description:
    <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Bind("Description") %>'>
    </asp:Label><br />
    <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit"
        Text="Edit">
    </asp:LinkButton>
    <asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete"
        Text="Delete">
    </asp:LinkButton>
    <asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New"
        Text="New">
    </asp:LinkButton>
</ItemTemplate>
</asp:FormView> 