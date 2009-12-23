<%@ Control CodeFile="ModuleDefinitions.ascx.vb" Language="vb" AutoEventWireup="false"
    Explicit="True" Inherits="DotNetNuke.Modules.Admin.ModuleDefinitions.ModuleDefinitions" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<table cellspacing="2" cellpadding="2" summary="Module Definitions Design Table" border="0">
    <tr>
        <td>
            <br />
            <dnn:SectionHead ID="dshInstalled" CssClass="Head" runat="server" Text="Installed Modules"
                Section="divInstalled" ResourceKey="Installed" IncludeRule="True" />
            <div id="divInstalled" runat="Server">
                <asp:Label ID="lblUpdate" runat="server" CssClass="Normal" resourceKey="lblUpdate" />
                <br />
                <asp:DataGrid ID="grdDefinitions" BorderWidth="0" BorderStyle="None" CellPadding="4"
                    CellSpacing="4" AutoGenerateColumns="false" EnableViewState="false" runat="server"
                    summary="Module Defs Design Table" GridLines="None">
                    <HeaderStyle Wrap="False" CssClass="NormalBold" />
                    <ItemStyle CssClass="Normal" />
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemStyle Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <asp:HyperLink NavigateUrl='<%# EditURL("desktopmoduleid",DataBinder.Eval(Container.DataItem,"DesktopModuleId")) %>'
                                    Visible="<%# IsEditable %>" runat="server" ID="Hyperlink1">
                                    <asp:Image ImageUrl="~/images/edit.gif" AlternateText="Edit" Visible="<%# IsEditable %>"
                                        runat="server" ID="Hyperlink1Image" resourcekey="Edit" />
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="FriendlyName" HeaderText="ModuleName">
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Description" HeaderText="Description">
                            <ItemStyle Wrap="True"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Version" HeaderText="Version">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Upgrade">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink2" NavigateUrl='<%# UpgradeRedirect(DataBinder.Eval(Container.DataItem,"ModuleName")) %>'
                                    ImageUrl='<%# UpgradeIndicator(DataBinder.Eval(Container.DataItem,"Version"),DataBinder.Eval(Container.DataItem,"ModuleName")) %>'
                                    Target="_new" ToolTip="Click Here To Get The Upgrade" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <dnn:SectionHead ID="dshAvailable" CssClass="Head" runat="server" Text="Available Modules"
                Section="divAvailable" ResourceKey="Available" IncludeRule="True" />
            <div id="divAvailable" runat="server">
                <asp:Label ID="lblAvailable" runat="server" CssClass="Normal" resourceKey="lblAvailable" />
                <br />
                <br />
                <asp:CheckBoxList ID="lstModules" runat="server" CssClass="Normal" RepeatColumns="3" RepeatDirection="Horizontal" />
                <br />
                <dnn:CommandButton id="cmdInstall" runat="server" imageurl="~/images/register.gif" resourcekey="cmdInstall" causesvalidation="False" />
                <br />
                <asp:PlaceHolder ID="phPaLogs" runat="server"></asp:PlaceHolder>
                <br />
                <dnn:CommandButton ID="cmdRefresh" runat="server" Visible="false" imageurl="~/images/refresh.gif" resourcekey="cmdRefresh" causesvalidation="False" />
            </div>
        </td>
    </tr>
</table>
