'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports DotNetNuke.Services.Vendors

Namespace DotNetNuke.Modules.Admin.Vendors

    Partial Class BannerClickThrough

        Inherits Framework.PageBase

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If (Not Request.QueryString("vendorid") Is Nothing) And (Not Request.QueryString("bannerid") Is Nothing) Then
                    Dim intVendorId As Integer = Integer.Parse(Request.QueryString("vendorid"))
                    Dim intBannerId As Integer = Integer.Parse(Request.QueryString("bannerid"))

                    Dim strURL As String = "~/" & glbDefaultPage

                    Dim objBanners As New BannerController
                    Dim objBanner As BannerInfo = objBanners.GetBanner(intBannerId, intVendorId, PortalSettings.PortalId)
                    If objBanner Is Nothing Then
                        objBanner = objBanners.GetBanner(intBannerId, intVendorId, Null.NullInteger)
                    End If
                    If Not objBanner Is Nothing Then
                        If objBanners.IsBannerActive(objBanner) Then
                            If Not Null.IsNull(objBanner.URL) Then
                                strURL = Common.Globals.LinkClick(objBanner.URL, -1, -1, False)
                            Else
                                Dim objVendors As New VendorController
                                Dim objVendor As VendorInfo = objVendors.GetVendor(objBanner.VendorId, PortalSettings.PortalId)
                                If objVendor Is Nothing Then
                                    objVendor = objVendors.GetVendor(objBanner.VendorId, Null.NullInteger)
                                End If
                                If Not objVendor Is Nothing Then
                                    If objVendor.Website <> "" Then
                                        strURL = AddHTTP(objVendor.Website)
                                    End If
                                End If
                            End If

                            objBanners.UpdateBannerClickThrough(intBannerId, intVendorId)
                        End If
                    Else
                        If Not Request.UrlReferrer Is Nothing Then
                            strURL = Request.UrlReferrer.ToString
                        End If
                    End If

                    Response.Redirect(strURL, True)
                End If

            Catch exc As Exception    'Page failed to load
                ProcessPageLoadException(exc)
            End Try
        End Sub

    End Class

End Namespace
