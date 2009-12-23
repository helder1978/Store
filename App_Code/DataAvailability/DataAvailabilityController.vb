Imports Microsoft.VisualBasic

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization

Namespace nsolutions4u.Modules.DataAvailability

    Public Class DataAvailabilityController

        <DataObjectMethod(DataObjectMethodType.Insert)> _
        Public Shared Sub DataAvailability_Insert(ByVal DataAvailabilityInfo As DataAvailabilityInfo)
            DataProvider.Instance().ExecuteNonQuery("DataAvailability_Insert", DataAvailabilityInfo.ModuleId, GetNull(DataAvailabilityInfo.UserID), DataAvailabilityInfo.CountryID, DataAvailabilityInfo.ServiceID, GetNull(DataAvailabilityInfo.Cycle.ToString), GetNull(DataAvailabilityInfo.Status.ToString), GetNull(DataAvailabilityInfo.PubDate.ToString), GetNull(DataAvailabilityInfo.Price))
        End Sub

        <DataObjectMethod(DataObjectMethodType.Delete)> _
        Public Shared Sub DataAvailability_Delete(ByVal DataAvailabilityInfo As DataAvailabilityInfo)
            DataProvider.Instance().ExecuteNonQuery("DataAvailability_Delete", DataAvailabilityInfo.ID)
        End Sub

        <DataObjectMethod(DataObjectMethodType.Update)> _
        Public Shared Sub DataAvailability_Update(ByVal DataAvailabilityInfo As DataAvailabilityInfo)

            'DataProvider.Instance().ExecuteNonQuery("INSERT INTO DebugTable(debug) VALUES ('XXX')")

            ' fetch the en-GB culture
            Dim ukCulture As CultureInfo = New CultureInfo("en-GB")
            ' pass the DateTimeFormat information to DateTime.Parse
            'Dim myDateTime As DateTime = DateTime.Parse(DataAvailabilityInfo.PubDate.ToString, ukCulture.DateTimeFormat)
            'Dim myDateTime As DateTime = CType(DataAvailabilityInfo.PubDateStr, Date)
            Dim myDateTime As DateTime = DateTime.Parse(DataAvailabilityInfo.PubDateStr, ukCulture.DateTimeFormat)
            'Dim myDateTime As DateTime = DateTime.Parse(DataAvailabilityInfo.PubDateStr, "dd/MM/yyyy")
            Dim myDateTimeStr As String
            myDateTimeStr = Format(myDateTime, "MM/dd/yyyy")
            'myDateTimeStr = Format(CType(DataAvailabilityInfo.PubDateStr, Date), "MM/dd/yyyy")
            'Dim myDateTime2 As DateTime = CType(DataAvailabilityInfo.PubDateStr, Date)
            DataProvider.Instance().ExecuteNonQuery("DataAvailability_Update", DataAvailabilityInfo.ID, DataAvailabilityInfo.ModuleId, GetNull(DataAvailabilityInfo.UserID), DataAvailabilityInfo.CountryID, DataAvailabilityInfo.ServiceID, GetNull(DataAvailabilityInfo.Cycle.ToString), GetNull(DataAvailabilityInfo.Status.ToString), GetNull(myDateTimeStr), GetNull(DataAvailabilityInfo.Price))
        End Sub

        <DataObjectMethod(DataObjectMethodType.Select)> _
        Public Shared Function DataAvailability_SelectAll(ByVal ModuleId As Integer) As List(Of DataAvailabilityInfo)
            Return CBO.FillCollection(Of DataAvailabilityInfo)(CType(DataProvider.Instance().ExecuteReader("DataAvailability_SelectAll", ModuleId), IDataReader))
        End Function

        Private Shared Function GetNull(ByVal Field As Object) As Object
            Return Null.GetNull(Field, DBNull.Value)
        End Function

    End Class

End Namespace