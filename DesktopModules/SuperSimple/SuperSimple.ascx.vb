Imports DotNetNuke

Imports System.Web.UI

Imports System.Collections.Generic

Imports System.Reflection

Imports DotNetNuke.Security.PortalSecurity

Partial Class DesktopModules_SuperSimple_SuperSimple

    Inherits Entities.Modules.PortalModuleBase

    Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            ShowData("")

        End If



    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        ShowData(txtSearch.Text)

    End Sub

    Private Sub ShowData(ByVal SearchString As String)

        Dim mySqlString As New StringBuilder()

        mySqlString.Append("SELECT FriendlyName, Description ")

        mySqlString.Append("FROM {databaseOwner}{objectQualifier}DesktopModules ")

        mySqlString.Append("WHERE Description like '%' + @SearchString + '%' ")

        mySqlString.Append("ORDER BY FriendlyName")

        Dim myParam As SqlParameter = New SqlParameter("@SearchString", SqlDbType.VarChar, 150)

        myParam.Value = SearchString

        Me.GridView1.DataSource = CType(DataProvider.Instance().ExecuteSQL(mySqlString.ToString(), myParam), IDataReader)

        Me.GridView1.DataBind()

    End Sub

End Class