<%@ Control language="vb" CodeBehind="~/admin/Containers/container.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="ACTIONS" Src="~/Admin/Containers/SolPartActions.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ICON" Src="~/Admin/Containers/Icon.ascx" %>
<link rel="stylesheet" type="text/css" href="<%= SkinPath %>style.css" />
<table border="0px" cellspacing="0px" cellpadding="0px">
	<tr>
    		<td colspan="3">
         		<TABLE width="100%" border="0" cellpadding="0" cellspacing="0">
            			<TR>
                			<TD nowrap><dnn:ACTIONS runat="server" id="dnnACTIONS" /> </TD>
                			<TD nowrap><dnn:ICON runat="server" id="dnnICON" /></TD>
            			</TR>
        		</TABLE>
   	 	</td>
	</tr>
	<TR>
		<TD COLSPAN="3">
			<TABLE WIDTH="100%" BORDER="0" CELLPADDING="0" CELLSPACING="0"  bgcolor="#FFFFFF">
			<tr>
        			<td class="top_border_left">  		</td>
				<td class="top_border">       		</td>
				<td class="top_border_right"> 		</td>
			</tr>
			<tr>
				<td class="left_side_border"> 		</td>
				<td runat=server id="ContentPane">	</td>
				<td class="right_side_border">		</td>
        		</tr>	
			<tr>
				<td class="bottom_border_left">		</td>
				<td class="bottom_border">		</td>
				<td class="bottom_border_right">	</td>
			</tr>
			<tr>
	        		<td class="bottom_shadow" colspan="3">	</td>
			</tr>
			</TABLE>
		</TD>
	</TR>
</table>

