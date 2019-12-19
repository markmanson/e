#Region "Namespaces"
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
#End Region

Namespace unirecruite

    Partial Class changepass
        Inherits BasePage
        'Protected WithEvents textfield3 As System.Web.UI.HtmlControls.HtmlInputText
        Dim CONS As New unirecruite.Errconstants
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("changepassword")


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
        '*************On Error Go to Error Page****************

#Region "onErrors"
        Private Sub onErrors(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Error
            Dim Err As New CreateLog
            Err.ErrorLog(Server.MapPath("Logs/RMS"), Server.GetLastError().ToString().Trim, "changepass.aspx")
            Response.Redirect("error.aspx?err= Error On Page")
        End Sub
#End Region

#Region "Page_Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Session.Add("ChkforOld", "OK")
            If Session("UserName") = Nothing Then
                Response.Redirect("~\login.aspx", False)
            End If
            'If Session("UniUserType").ToString <> "1" Then
            '    Response.Redirect("~\register.aspx", False)
            'End If

            txt_oldpass.Focus()
            If Session("LoginGenuine") Is Nothing Then
                Response.Redirect("error.aspx?err=changepass Please Login to continue")
            End If
            lbl_passchange.Visible = False
        End Sub
#End Region

#Region "Page_Unload"
        Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim objconn As New ConnectDb
            'Dim strPathDb As String
            Try
                If objconn.connect() = True Then
                    objconn.disconnect()
                End If
            Catch ex As Exception
                objconn.disconnect()
            End Try
            'objconn.disconnect()

        End Sub
#End Region
        '#Region "validoldpass_ServerValidate"
        '        Private Sub validoldpass_ServerValidate(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles validoldpass.ServerValidate
        '            Dim myCommand As SqlCommand
        '            Dim myDataReader As SqlDataReader
        '            Dim objconn As New ConnectDb
        '            Dim sqlstr As String

        '            'Dim strPathDb As String
        '            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        '            If objconn.connect() Then
        '                sqlstr = ""
        '                sqlstr = sqlstr & "SELECT COUNT(*) FROM m_user_info WHERE "
        '                sqlstr = sqlstr & "userid = " & Session.Item("userid") & " AND "
        '                sqlstr = sqlstr & "pwd = '" & args.Value & "'"

        '                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
        '                myDataReader = myCommand.ExecuteReader
        '                myDataReader.Read()
        '                If myDataReader.Item(0) <> 0 Then
        '                    args.IsValid = True
        '                Else
        '                    args.IsValid = False
        '                End If
        '                myDataReader.Close()
        '                myCommand.Dispose()
        '                objconn.disconnect()
        '            Else
        '                Response.Redirect("error.aspx?err=changepass" & CONS.ERR_DBCON)
        '            End If

        '        End Sub
        '#End Region

        'Added by Pranit on 06/11/2019
#Region "Check Old Password"
        Private Function Check_Old_Pwd()
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim objconn As New ConnectDb
            Dim sqlstr As String

            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                sqlstr = ""
                sqlstr = sqlstr & "SELECT COUNT(*) FROM m_user_info WHERE "
                sqlstr = sqlstr & "userid = " & Session.Item("userid") & " AND "
                sqlstr = sqlstr & "pwd = '" & txt_oldpass.Text & "'"

                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader
                myDataReader.Read()

                If myDataReader.Item(0) = 0 Then
                    lbl_passchange.Text = Resources.Resource.ChangePass_OpwdWrng
                    lbl_passchange.Visible = True
                    lbl_passchange.ForeColor = Drawing.Color.Red
                    Session.Item("ChkforOld") = "AllWrong"
                Else
                    Session.Item("ChkforOld") = "AllRight"
                End If
                myDataReader.Close()
                myCommand.Dispose()
                objconn.disconnect()
            Else
                Response.Redirect("error.aspx?err=changepass" & CONS.ERR_DBCON)
            End If
        End Function
#End Region

        'Added by Pranit Chimurkar on 2019/10/25
#Region "imgbtn_cancel_Click"
        Private Sub imgbtn_cancel_Click(sender As Object, e As EventArgs) Handles imgbtn_cancel.Click
            Try
                ClientScript.RegisterClientScriptBlock(Me.GetType, "CloseScript", "self.close();", True)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try

        End Sub
#End Region

#Region "Page Validation"
        Function PageValidation() As Boolean
            '***********Added By Pranit Chimurkar ***************
            'Date : 2019/11/07
            'Desc. : Page Validation
            '******************************************************

            'Old Password Validation
            If txt_oldpass.Text.Trim() = "" Then
                lbl_passchange.Text = Resources.Resource.ChangePass_Pfillopwd
                lbl_passchange.Visible = True
                txt_oldpass.Focus()
                Return False
                Exit Function
            ElseIf txt_oldpass.Text.Trim() <> "" Then
                Check_Old_Pwd()
                If Session.Item("ChkforOld") = "AllWrong" Then
                    Exit Function
                End If
            End If

                'New Password Validation
                If txt_newpass.Text.Trim() = "" Then
                lbl_passchange.Text = Resources.Resource.ChangePass_Pfillnpwd
                lbl_passchange.Visible = True
                txt_newpass.Focus()
                Return False
                Exit Function
            ElseIf Not RequiredRegex(txt_newpass.Text.Trim()) Then
                lbl_passchange.Text = Resources.Resource.ChangePass_PwdValidate
                lbl_passchange.Visible = True
                txt_newpass.Focus()
                Return False
                Exit Function
            End If

            'Confirm Password Validation
            If txt_confpassword.Text.Trim() = "" Then
                lbl_passchange.Text = Resources.Resource.ChangePass_Pfillcpwd
                lbl_passchange.Visible = True
                txt_confpassword.Focus()
                Return False
                Exit Function
            ElseIf (txt_newpass.Text <> txt_confpassword.Text) Then
                lbl_passchange.Text = Resources.Resource.ChangePass_NewConNoMch
                lbl_passchange.Visible = True
                txt_confpassword.Focus()
                Exit Function
                Return False
            End If
            Return True
            lbl_passchange.Visible = False
        End Function
#End Region

        'Added by Pranit on 07/11/2019
#Region "Pattern Check"
        Function RequiredRegex(ByVal Field As String) As Boolean
            Dim pattern As String = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$"
            Dim reqTextFieldMatch As Match = Regex.Match(Field, pattern)
            If reqTextFieldMatch.Success Then
                RequiredRegex = True
            Else
                RequiredRegex = False
            End If
        End Function
#End Region

        'Added by Pranit Chimurkar on 2019/10/25
#Region "btn_update_Click"
        Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
            If Page.IsValid Then
                Dim myCommand As SqlCommand
                Dim objconn As New ConnectDb
                Dim sqlstr As String

                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                'Added by Pranit on 07/11/2019
                'Validation for password fields
                If Not PageValidation() Then
                    Exit Sub
                End If
                'Ended by Pranit

                If objconn.connect() Then
                    sqlstr = ""
                    sqlstr = sqlstr & "UPDATE m_user_info SET "
                    sqlstr = sqlstr & "pwd = '" & Replace(txt_newpass.Text, "'", "''", 1, -1, 1) & "' WHERE "
                    sqlstr = sqlstr & "userid = " & Session.Item("userid")
                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    myCommand.ExecuteNonQuery()
                    myCommand.Dispose()
                    objconn.disconnect()
                    lbl_passchange.Visible = True
                    'Added by Pranit on 06/11/2019
                    lbl_passchange.Text = Resources.Resource.ChangePass_PwdChng
                    lbl_passchange.ForeColor = Drawing.Color.Green
                    'imgbtn_cancel.ImageUrl = "images/BtnloginBack.jpg"
                Else
                    Response.Redirect("error.aspx?err=changepass" & CONS.ERR_DBCON)
                End If
            Else
                'Added by Pranit on 06/11/2019
                lbl_passchange.Visible = True
                lbl_passchange.Text = Resources.Resource.changepass_PlEnAllField
                lbl_passchange.ForeColor = Drawing.Color.Red
                txt_oldpass.Focus()
                'validoldpass.ErrorMessage = "The Old password seems to be wrong"
            End If
        End Sub
#End Region

    End Class

End Namespace
