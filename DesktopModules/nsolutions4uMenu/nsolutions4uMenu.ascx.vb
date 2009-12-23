Imports DotNetNuke.UI.WebControls

Partial Class DesktopModules_nsolutions4uMenu_nsolutions4uMenu
    Inherits Entities.Modules.PortalModuleBase


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            myDNNMenu.ImageList.Add("../../../Images/folder.gif")
            myDNNMenu.ImageList.Add("../../../Images/file.gif")
            PopulateTree()
        End If
    End Sub

    Private Sub PopulateTree()
        myDNNMenu.MenuNodes.Clear()
        Dim index As Integer = 0

        Dim objNode As MenuNode = New MenuNode("Tutorials")
        objNode.NavigateUrl = "http://www.adefwebserver.com/DotNetNukeHELP/"
        objNode.ToolTip = "DotNetNuke Tutorial Series"
        objNode.ImageIndex = eImageType.Folder
        objNode.ClickAction = eClickAction.Navigate
        objNode.HasNodes = True
        myDNNMenu.MenuNodes.Add(objNode)
        index = objNode.MenuNodes.Add()
        objNode = objNode.MenuNodes(index)
        objNode.Text = "DotNetNuke 4"
        objNode.ToolTip = "DotNetNuke 4 Tutorials"
        objNode.ImageIndex = eImageType.Folder
        objNode.ClickAction = eClickAction.Expand
        PopulateChildrenMenuNodes(objNode)

        objNode = New MenuNode("Link2")
        objNode.NavigateUrl = "http://www.adefwebserver.com/DotNetNukeHELP/"
        objNode.ToolTip = "DotNetNuke Tutorial Series"
        objNode.ImageIndex = eImageType.Page
        objNode.ClickAction = eClickAction.Navigate
        objNode.HasNodes = True
        MyDNNMenu.MenuNodes.Add(objNode)
        index = objNode.MenuNodes.Add()

        objNode = objNode.MenuNodes(index)
        objNode.Text = "DotNetNuke 4"
        objNode.ToolTip = "DotNetNuke 4 Tutorials"
        objNode.ImageIndex = eImageType.Folder
        objNode.ClickAction = eClickAction.Expand

        PopulateChildrenMenuNodes(objNode)

    End Sub

    Private Sub PopulateChildrenMenuNodes(ByVal objParent As MenuNode)
        Dim index As Integer = 0
        Dim objMenuNode As MenuNode
        index = objParent.MenuNodes.Add()
        objMenuNode = objParent.MenuNodes(index)
        objMenuNode.Text = "Super-Simple Module (DAL+)"
        objMenuNode.NavigateURL = "http://www.adefwebserver.com/DotNetNukeHELP/DNN_ShowMeThePages/"
        objMenuNode.ImageIndex = eImageType.Page
        objMenuNode.ClickAction = eClickAction.Navigate
        index = objParent.MenuNodes.Add()
        objMenuNode = objParent.MenuNodes(index)
        index += 1
        objMenuNode.Text = "Super-Fast Super-Easy Module (DAL+)"
        objMenuNode.NavigateURL = "http://www.adefwebserver.com/DotNetNukeHELP/DNN_Things4Sale/"
        objMenuNode.ImageIndex = eImageType.Page
        objMenuNode.ClickAction = eClickAction.Navigate
        index = objParent.MenuNodes.Add()
        objMenuNode = objParent.MenuNodes(index)
        index += 1
        objMenuNode.Text = "Create a full complete Module"
        objMenuNode.NavigateURL = "http://www.adefwebserver.com/DotNetNukeHELP/DNN_Module4/"
        objMenuNode.ImageIndex = eImageType.Page
        objMenuNode.ClickAction = eClickAction.Navigate
    End Sub

    Public Enum eImageType
        Folder
        Page
    End Enum
End Class
