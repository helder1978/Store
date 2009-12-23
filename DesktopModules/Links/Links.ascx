<%@ Control language="vb" Inherits="DotNetNuke.Modules.Links.Links" CodeBehind="Links.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<asp:panel id="pnlList" runat="server">
    <asp:datalist id=lstLinks runat="server" summary="Links Design Table" itemstyle-verticalalign="Top" cellpadding="0">
        <itemtemplate>
            <table border="0" cellpadding="4" cellspacing="0">
                <tr>
                    <td <%# NoWrap %>>
                        <asp:HyperLink id="editLink" NavigateUrl='<%# EditURL("ItemID",DataBinder.Eval(Container.DataItem,"ItemID")) %>' Visible="<%# IsEditable %>" runat="server" ><asp:Image id="editLinkImage" ImageUrl="~/images/edit.gif" AlternateText="Edit" Visible="<%# IsEditable %>" Runat="server" /></asp:hyperlink>
                        <asp:Image ImageUrl="<%# FormatIcon() %>"  Visible="<%# DisplayIcon() %>" runat="server" />
                        <asp:HyperLink CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem,"Title") %>' NavigateUrl='<%# FormatURL(DataBinder.Eval(Container.DataItem,"Url"),DataBinder.Eval(Container.DataItem,"TrackClicks")) %>' ToolTip='<%# DisplayToolTip(DataBinder.Eval(Container.DataItem,"Description")) %>' Target= '<%# IIF(DataBinder.Eval(Container.DataItem,"NewWindow"),"_blank","_self") %>' runat="server" />
                        &nbsp;
                        <asp:linkbutton Runat="server" CssClass="CommandButton" Text='...' CommandName="Select" Visible='<%# DisplayInfo() %>'/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:panel id="pnlDescription" visible="false" runat="server">
                            <asp:Label runat="server" CssClass="Normal" Text='<%# HtmlDecode(DataBinder.Eval(Container.DataItem, "Description")) %>' ID="Label1"/>
                        </asp:panel>
                    </td>
                </tr>
            </table>
        </itemtemplate>
    </asp:datalist>
</asp:panel>
<asp:panel id="pnlDropdown" runat="server">
    <table cellspacing=0 cellpadding=4 summary="Links Design Table" border=0>
        <tr>
            <td nowrap>
                <asp:imagebutton id=cmdEdit runat="server" imageurl="~/images/edit.gif" alternatetext="Edit" resourcekey="Edit"></asp:imagebutton>
                <label style="DISPLAY: none" for="<%=cboLinks.ClientID%>">Link</label> 
                <asp:dropdownlist id=cboLinks runat="server" datatextfield="Title" datavaluefield="ItemID" cssclass="NormalTextBox"></asp:dropdownlist>&nbsp; 
                <asp:linkbutton id=cmdGo runat="server" cssclass="CommandButton" resourcekey="cmdGo"></asp:linkbutton>&nbsp; 
                <asp:linkbutton id=cmdInfo runat="server" cssclass="CommandButton" text="..."></asp:linkbutton>
            </td>
        </tr>
        <tr>
            <td><asp:label id=lblDescription runat="server" cssclass="Normal"></asp:label></td>
        </tr>
    </table>
</asp:panel>
