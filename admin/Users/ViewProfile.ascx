<%@ Control language="vb" CodeFile="ViewProfile.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Admin.Users.ViewProfile" %>
<%@ Register TagPrefix="dnn" TagName="Profile" Src="~/Admin/Users/Profile.ascx" %>
<dnn:Profile id="ctlProfile" runat="server" 
	EditorMode="View" 
	ShowUpdate="False"/>
