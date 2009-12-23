
Partial Class DesktopModules_DemoApp_DemoControl
    Inherits DotNetNuke.Entities.Modules.PortalModuleBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Register AJAX components if available
            If DotNetNuke.Framework.AJAX.IsInstalled Then
                'Register the script manager
                DotNetNuke.Framework.AJAX.RegisterScriptManager()

                'Wrap the pnlAjaxUpdate within an update panel with progress control
                DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(Me.pnlAjaxUpdate, True)

                'Register the btnPostbackTimeUpdate button as a postback control
                DotNetNuke.Framework.AJAX.RegisterPostBackControl(Me.btnPostbackTimeUpdate)
            End If
        End If
    End Sub

    Protected Sub ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles btnAjaxTimeUpdate.Click, btnPostbackTimeUpdate.Click
        'Sleep for 5 seconds to simulate a long running process
        Threading.Thread.Sleep(5000)

        'Update the label
        lblCurrentTime.Text = System.DateTime.Now.ToString()
    End Sub

End Class
