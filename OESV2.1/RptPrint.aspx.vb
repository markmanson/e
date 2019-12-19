#Region "Nmespaces"
Imports System.Data
Imports log4net
#End Region

Namespace unirecruite

    Partial Class RptPrint
        Inherits System.Web.UI.Page
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("RptPrint")
#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region "Page Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim Ds As DataSet = Session("DS")
            Try

                If Session("UserName") = Nothing Then
                    Response.Redirect("~\login.aspx", False)
                End If
                If Session("UniUserType").ToString <> "1" Then
                    Response.Redirect("~\register.aspx", False)
                End If

                'Put user code to initialize the page here
                DgPrnReport.DataSource = Ds
                DgPrnReport.DataMember = "TblResults"
                DgPrnReport.DataBind()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                Ds = Nothing
            End Try
        End Sub
#End Region

#Region "Page Unload Event"
        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            Try
                Session.Remove("DS")
                'If Session("gridvisible") = "true" Then
                'Else
                '    Session.Remove("DS")
                'End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try

        End Sub
#End Region

#Region "Button Back Click Event"
        Private Sub BtnBack_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBack.ServerClick
            Try
                Response.Redirect("CandStatus.aspx", False)
                'Session.Add("fromprint", "true")
                'If Request.QueryString("pi") <> Nothing Then
                '    Dim intpi As Integer = Request.QueryString("pi")
                '    Dim dt As DataSet = Session("DS")
                '    Session.Remove("DS")
                '    Session.Add("data", dt)
                '    Response.Redirect("CandStatus.aspx?pi=" & intpi, False)
                'End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region
    End Class

End Namespace
