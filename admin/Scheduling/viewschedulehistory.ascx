<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Admin.Scheduling.ViewScheduleHistory"
    CodeFile="ViewScheduleHistory.ascx.vb" %>
<asp:DataGrid ID="dgScheduleHistory" runat="server" AutoGenerateColumns="false" CellPadding="4"
    CellSpacing="2" DataKeyField="ScheduleID" EnableViewState="false" border="1"
    summary="This table shows the schedule of events for the portal." BorderColor="gray"
    BorderStyle="Solid" BorderWidth="1px" GridLines="Both">
    <Columns>
        <asp:TemplateColumn HeaderText="Description">
            <HeaderStyle CssClass="NormalBold" />
            <ItemStyle CssClass="Normal" VerticalAlign="Top" />
            <ItemTemplate>
                <table border="0" width="100%">
                    <tr>
                        <td nowrap class="Normal">
                            <i>
                                <%# DataBinder.Eval(Container.DataItem,"TypeFullName")%>
                            </i>
                        </td>
                    </tr>
                </table>
                <asp:Label runat="server" Visible='<%# DataBinder.Eval(Container.DataItem,"LogNotes")<>""%>' Text='<%# DataBinder.Eval(Container.DataItem,"LogNotes")%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="ElapsedTime" HeaderText="Duration">
            <HeaderStyle CssClass="NormalBold"></HeaderStyle>
            <ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
        </asp:BoundColumn>
        <asp:BoundColumn DataField="Succeeded" HeaderText="Succeeded">
            <HeaderStyle CssClass="NormalBold"></HeaderStyle>
            <ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
        </asp:BoundColumn>
        <asp:TemplateColumn HeaderText="Start/End/Next Start">
            <HeaderStyle CssClass="NormalBold"></HeaderStyle>
            <ItemStyle Wrap="False" CssClass="Normal" VerticalAlign="Top"></ItemStyle>
            <ItemTemplate>
                S:&nbsp;<%# DataBinder.Eval(Container.DataItem,"StartDate")%>
                <br>
                E:&nbsp;<%# DataBinder.Eval(Container.DataItem,"EndDate")%>
                <br>
                N:&nbsp;<%# DataBinder.Eval(Container.DataItem,"NextStart")%>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<p>
    <asp:LinkButton ID="cmdCancel" runat="server" resourcekey="cmdCancel" CssClass="CommandButton">Cancel</asp:LinkButton></p>
