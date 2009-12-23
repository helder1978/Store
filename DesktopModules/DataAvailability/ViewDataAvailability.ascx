<%@ Control  language="vb"  Inherits="nsolutions4u.Modules.DataAvailability.ViewDataAvailability"  CodeFile="ViewDataAvailability.ascx.vb"  AutoEventWireup="false"  Explicit="True" %>
<%@ Register Assembly="DotNetNuke.WebUtility" Namespace="DotNetNuke.UI.Utilities" TagPrefix="cc1" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %> <br />

<asp:ObjectDataSource ID="ObjectDataSource_DataAvailability" runat="server" DataObjectTypeName="nsolutions4u.Modules.DataAvailability.DataAvailabilityInfo"
DeleteMethod="DataAvailability_Delete" InsertMethod="DataAvailability_Insert" OldValuesParameterFormatString="original_{0}"
OnInit="Page_Load" SelectMethod="DataAvailability_SelectAll" TypeName="nsolutions4u.Modules.DataAvailability.DataAvailabilityController"
UpdateMethod="DataAvailability_Update">
<SelectParameters>
<asp:Parameter DefaultValue="00" Name="ModuleId" Type="Int32" />
</SelectParameters>
</asp:ObjectDataSource>

<asp:SqlDataSource ID="_serviceDataSource" runat="server"
    SelectCommand="SELECT [Service_ID], [Service_Descr] FROM [DataAvailability_Services]"
    EnableCaching="True" CacheDuration="200"
    ConnectionString="<%$ ConnectionStrings:SiteSqlServer %>" />
                            
<asp:SqlDataSource ID="_countryDataSource" runat="server"
    SelectCommand="SELECT [Country_ID], [CountryDesc] FROM [DataAvailability_Country]"
    EnableCaching="True" CacheDuration="200"
    ConnectionString="<%$ ConnectionStrings:SiteSqlServer %>" />

<asp:SqlDataSource ID="_cycleDataSource" runat="server"
    SelectCommand="SELECT [Cycle_ID], [Cycle_Descr] FROM [DataAvailability_Cycle]"
    EnableCaching="True" CacheDuration="200"
    ConnectionString="<%$ ConnectionStrings:SiteSqlServer %>" />

<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AutoGenerateEditButton="True" DataKeyNames="ModuleId, ID"
DataSourceID="ObjectDataSource_DataAvailability" CellPadding="4" EnableViewState="False" BackColor="White" BorderColor="#C5DFF4" BorderStyle="None" BorderWidth="0px" GridLines="Horizontal">
<Columns>
    <asp:TemplateField HeaderText="Service" SortExpression="Service_Descr">
     <EditItemTemplate>
      <asp:DropDownList runat="server" ID="_serviceDropDown" DataSourceID="_serviceDataSource"
            EnableViewState="false"
            AppendDataBoundItems="false" DataTextField="Service_Descr"
            DataValueField="Service_ID" SelectedValue='<%# Bind("ServiceID") %>'>
        <asp:ListItem Selected="true" Text="[Select a service]" Value="-1" />
      </asp:DropDownList>
     </EditItemTemplate>
     <ItemTemplate>
      <asp:Label ID="lblService" runat="server" Text='<%# Bind("Service_Descr") %>' />
     </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="CountryID" HeaderText="CountryID" SortExpression="CountryID" Visible="False" />
    <asp:TemplateField HeaderText="Country" SortExpression="CountryID">
     <EditItemTemplate>
      <asp:DropDownList runat="server" ID="_countryDropDown" DataSourceID="_countryDataSource"
            EnableViewState="false"
            AppendDataBoundItems="false" DataTextField="CountryDesc"
            DataValueField="Country_ID" SelectedValue='<%# Bind("CountryID") %>'>
        <asp:ListItem Selected="true" Text="[Select a country]" Value="-1" />
      </asp:DropDownList>
     </EditItemTemplate>
     <ItemTemplate>
      <asp:Label ID="lblCountry" runat="server" Text='<%# Bind("CountryDesc") %>' />
     </ItemTemplate>
    </asp:TemplateField>
  
    <asp:BoundField DataField="Cycle" HeaderText="Cycle" SortExpression="Cycle" Visible="False"/>
    <asp:TemplateField HeaderText="Cycle" SortExpression="Cycle">
     <EditItemTemplate>
      <asp:DropDownList runat="server" ID="_cycleDropDown" DataSourceID="_cycleDataSource"
            EnableViewState="false"
            AppendDataBoundItems="false" DataTextField="Cycle_Descr"
            DataValueField="Cycle_Descr" SelectedValue='<%# Bind("Cycle") %>'>
        <asp:ListItem Selected="true" Text="[Select a cycle]" Value="-1" />
      </asp:DropDownList>
     </EditItemTemplate>
     <ItemTemplate>
      <asp:Label ID="lblCycle" runat="server" Text='<%# Bind("Cycle") %>' />
     </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Status" SortExpression="Status">
     <EditItemTemplate>
      <asp:DropDownList runat="server" ID="_statusDropDown1" SelectedValue='<%# Bind("Status") %>'>
        <asp:ListItem Text="Published" Value="P" />
        <asp:ListItem Text="Scheduled" Value="S" />
      </asp:DropDownList>
     </EditItemTemplate>
     <ItemTemplate>
        <asp:Image ID="imgStatus" Runat="Server"
            AlternateText='<%# Eval("Status") %>'
            ImageUrl='<%# "~/images/canadean/li_status_" & Eval("Status") & ".gif" %>'/><br/>
      </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="PubDate" SortExpression="PubDate">
     <EditItemTemplate>
        <asp:TextBox ID="PubDateTextBox" runat="server" Text='<%# Bind("PubDateStr", "{0:dd/MM/yyyy}")  %>'></asp:TextBox>
     </EditItemTemplate>
     <ItemTemplate>
        <asp:Label ID="lblPubDate" runat="server" Text='<%# Bind("PubDate", "{0:dd/MM/yyyy}") %>' />
      </ItemTemplate>
    </asp:TemplateField>

    <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" Visible="False"/>
    <asp:BoundField DataField="UserID" HeaderText="UserID" SortExpression="UserID"  Visible="False"/>
    <asp:BoundField DataField="ModuleId" HeaderText="ModuleId" SortExpression="ModuleId" Visible="false"/>
    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"/>
</Columns>
<EmptyDataTemplate>
There are no items.
</EmptyDataTemplate>
    <FooterStyle BackColor="White" ForeColor="#333333" />
    <RowStyle BackColor="White" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
    <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
</asp:GridView>


<br />
<asp:LinkButton ID="Add_My_Listing_LinkButton" runat="server" EnableViewState="False">Add Data Availability</asp:LinkButton><br />
<br />


<asp:FormView ID="FormView1" runat="server" DataSourceID="ObjectDataSource_DataAvailability"
DefaultMode="Insert" BorderColor="DarkGray" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" Visible="False">
<EditItemTemplate>
</EditItemTemplate>
<InsertItemTemplate>
    ServiceID:
    <asp:TextBox ID="ServiceIDTextBox" runat="server" Text='<%# Bind("ServiceID") %>'>
    </asp:TextBox><br />
    CountryID:
    <asp:TextBox ID="CountryIDTextBox" runat="server" Text='<%# Bind("CountryID") %>'>
    </asp:TextBox><br />
    Cycle:
    <asp:TextBox ID="CycleTextBox" runat="server" Text='<%# Bind("Cycle") %>'>
    </asp:TextBox><br />
    Status:
    <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>'>
    </asp:TextBox><br />
    PubDate:
    <asp:TextBox ID="PubDateTextBox" runat="server" Text='<%# Bind("PubDate")  %>'>
    </asp:TextBox><br />
    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
        Text="Insert">
    </asp:LinkButton>
    <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
        Text="Cancel">
    </asp:LinkButton>
</InsertItemTemplate>
<ItemTemplate>
</ItemTemplate>
</asp:FormView> 
