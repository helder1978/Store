Imports System
Imports System.Collections.Generic
Imports System.Data

Namespace YourCompany.Modules.ThingsForSale

    Public Class ThingsForSaleController

        <DataObjectMethod(DataObjectMethodType.Insert)> _
        Public Shared Sub ThingsForSale_Insert(ByVal ThingsForSaleInfo As ThingsForSaleInfo)
            DataProvider.Instance().ExecuteNonQuery("ThingsForSale_Insert", ThingsForSaleInfo.ModuleId, GetNull(ThingsForSaleInfo.UserID), GetNull(ThingsForSaleInfo.Category.ToString), GetNull(ThingsForSaleInfo.Description.ToString), GetNull(ThingsForSaleInfo.Price))
        End Sub

        <DataObjectMethod(DataObjectMethodType.Delete)> _
        Public Shared Sub ThingsForSale_Delete(ByVal ThingsForSaleInfo As ThingsForSaleInfo)
            DataProvider.Instance().ExecuteNonQuery("ThingsForSale_Delete", ThingsForSaleInfo.ID)
        End Sub

        <DataObjectMethod(DataObjectMethodType.Update)> _
        Public Shared Sub ThingsForSale_Update(ByVal ThingsForSaleInfo As ThingsForSaleInfo)
            DataProvider.Instance().ExecuteNonQuery("ThingsForSale_Update", ThingsForSaleInfo.ID, ThingsForSaleInfo.ModuleId, GetNull(ThingsForSaleInfo.UserID), GetNull(ThingsForSaleInfo.Category.ToString), GetNull(ThingsForSaleInfo.Description.ToString), GetNull(ThingsForSaleInfo.Price))
        End Sub

        <DataObjectMethod(DataObjectMethodType.Select)> _
        Public Shared Function ThingsForSale_SelectAll(ByVal ModuleId As Integer) As List(Of ThingsForSaleInfo)
            Return CBO.FillCollection(Of ThingsForSaleInfo)(CType(DataProvider.Instance().ExecuteReader("ThingsForSale_SelectAll", ModuleId), IDataReader))
        End Function

        Private Shared Function GetNull(ByVal Field As Object) As Object
            Return Null.GetNull(Field, DBNull.Value)
        End Function

    End Class

End Namespace