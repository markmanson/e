#Region "Namespaces"
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports log4net
#End Region

Namespace unirecruite
    Partial Public Class ClientRegistration
        Inherits System.Web.UI.Page
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("ClientRegistration")
        Dim objconnect As New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
        Dim strPathDb As String = ConfigurationManager.AppSettings("PathDb")
        Dim clientId As String
        Dim objconn As New ConnectDb
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            txtClientId.Focus()
            If Not IsPostBack Then
                clientId = Session.Item("ClientIDValue")
                Edit()
            End If
        End Sub
#Region "Page_Unload"
        Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles Me.Unload
            Try
                If Not strPathDb = Nothing Then
                    If objconn.connect(strPathDb) Then
                        objconn.disconnect()
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx")
            End Try
        End Sub
#End Region
        Sub Edit()
            Dim strSB As StringBuilder
            Dim objCommand As SqlCommand
            Dim objDataReader As SqlDataReader
            Try
                If Session.Item("ClientIDValue").ToString <> "" Then
                    strSB = New StringBuilder
                    strSB.Append("select * from M_Client_Master")
                    strSB.Append(" where client_id='")
                    strSB.Append(Session.Item("ClientIDValue").ToString)
                    strSB.Append("'")
                    If objconn.connect(strPathDb) Then
                        objCommand = New SqlCommand(strSB.ToString, objconn.MyConnection)
                        objDataReader = objCommand.ExecuteReader()
                        While objDataReader.Read()
                            txtClientId.ReadOnly = True
                            txtClientId.Text = objDataReader.Item("client_id")
                            txtClientName.Text = objDataReader.Item("client_name")
                            txtEmailAddress.Text = objDataReader.Item("email")
                            txtAddress.Text = objDataReader.Item("client_address")
                            txtMobileNo.Text = objDataReader.Item("mob_number")
                            btnSubmit.Visible = False
                            btnClear.Visible = False
                            btnReset.Visible = True
                            btnUpdate.Visible = True

                        End While
                    End If
                End If
            Catch ex As Exception

            End Try

        End Sub
        

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
            If Not PageValidation() Then
                Exit Sub
            End If
            InsertInformation()
        End Sub
        Sub InsertInformation()
            Dim cmd As SqlCommand
            Dim strSb As StringBuilder
            Try
                strSb = New StringBuilder()
                objconnect.Open()
                strSb.Append("INSERT INTO M_Client_Master(client_id,client_name,client_address,email,mob_number) VALUES('")
                strSb.Append(txtClientId.Text)
                strSb.Append("','")
                strSb.Append(txtClientName.Text)
                strSb.Append("','")
                strSb.Append(txtAddress.Text)
                strSb.Append("','")
                strSb.Append(txtEmailAddress.Text)
                strSb.Append("','")
                strSb.Append(txtMobileNo.Text)
                strSb.Append("')")
                cmd = New SqlCommand(strSb.ToString(), objconnect)
                cmd.ExecuteNonQuery()
                lblMsg.Text = "Register Succesfully."
                lblMsg.Visible = True
                lblMsg.ForeColor = Color.Green
                Clear()
            Catch ex As Exception
                objconnect.Close()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                objconnect.Close()
                cmd = Nothing
                strSb = Nothing
            End Try
        End Sub
        Function PageValidation() As Boolean

            '*************Client Name********************
            If txtClientId.Text.Trim() = "" Then
                lblMsg.Text = "Please enter client Id."
                lblMsg.Visible = True
                txtClientId.Focus()
                Return False
                Exit Function
            End If

            '*************Client Name********************
            If txtClientName.Text.Trim() = "" Then
                lblMsg.Text = "Please enter client name."
                lblMsg.Visible = True
                txtClientName.Focus()
                Return False
                Exit Function
            End If

            '*************Email Address********************
            If Not EmailAddressCheck(txtEmailAddress.Text.Trim()) Then
                If txtEmailAddress.Text.Trim() = "" Then
                    lblMsg.Text = "Please enter email address."
                    lblMsg.Visible = True
                    txtEmailAddress.Focus()
                    Return False
                    Exit Function
                Else
                    lblMsg.Text = "Please enter valid email address"
                    lblMsg.Visible = True
                    txtEmailAddress.Focus()
                    Return False
                    Exit Function
                End If
            End If

            '*********Mobile Number Validation************
            If txtMobileNo.Text.Trim() = "" Then
                lblMsg.Text = "Please enter mobile number."
                lblMsg.Visible = True
                txtMobileNo.Focus()
                Return False
                Exit Function
            ElseIf Not NumberFieldCheck(txtMobileNo.Text.Trim()) Then
                lblMsg.Text = "Please enter digits only."
                lblMsg.Visible = True
                txtMobileNo.Focus()
                Return False
                Exit Function
            ElseIf txtMobileNo.Text.Trim().Length <> 10 Then
                lblMsg.Text = "Please enter 10 digit mobile number."
                lblMsg.Visible = True
                txtMobileNo.Focus()
                txtMobileNo.Text = ""
                Return False
                Exit Function
            End If

            '*********Address Details Validation**********
            If txtAddress.Text.Trim = "" Then
                lblMsg.Text = "Please enter Address details."
                lblMsg.Visible = True
                txtAddress.Focus()
                Return False
                Exit Function
            ElseIf txtAddress.Text.Trim().Length > 500 Then
                lblMsg.Text = "Please enter Address details in less then 500 characters."
                lblMsg.Visible = True
                txtAddress.Focus()
                Return False
                Exit Function
            End If

            lblMsg.Visible = False
            Return True

        End Function
#Region "Email Address Validation"
        Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
            Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
            Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
            If emailAddressMatch.Success Then
                EmailAddressCheck = True
            Else
                EmailAddressCheck = False
            End If
        End Function
#End Region
#Region "Number Field Validation"
        Function NumberFieldCheck(ByVal Field As String) As Boolean
            Dim pattern As String = "^[0-9]*$"
            Dim numberFieldMatch As Match = Regex.Match(Field, pattern)
            If numberFieldMatch.Success Then
                NumberFieldCheck = True
            Else
                NumberFieldCheck = False
            End If
        End Function
#End Region
        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click

            If Session("ClientRegistration") = "true" Then
                Response.Redirect("ClientInformation.aspx", False)
                Session.Remove("ClientRegistration")
            Else
                Session.Add("ClientRegistration", "true")
                Session.Remove("ClientIDValue")
                Dim intpi As Integer = CInt(Request.QueryString("pi").ToString())
                Response.Redirect("ClientInformation.aspx?pi=" & intpi, False)
            End If


        End Sub

        Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnReset.Click
            Edit()
            lblMsg.Visible = False
        End Sub

        Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnClear.Click
            Clear()
        End Sub
        Sub Clear()
            txtClientId.Text = String.Empty
            txtAddress.Text = String.Empty
            txtClientName.Text = String.Empty
            txtEmailAddress.Text = String.Empty
            txtMobileNo.Text = String.Empty
            lblhead.Visible = False
        End Sub

        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnUpdate.Click
            If Not PageValidation() Then
                Exit Sub
            End If
            UpdateData()
        End Sub
        Public Sub UpdateData()
            Dim strSb As StringBuilder
            Dim update_cmd As SqlCommand
            Try

                If objconn.connect(strPathDb) = True Then
                    strSb = New StringBuilder()
                    strSb.Append("Update M_Client_Master Set client_name='")
                    strSb.Append(txtClientName.Text)
                    strSb.Append("',client_address='")
                    strSb.Append(txtAddress.Text)
                    strSb.Append("',email='")
                    strSb.Append(txtEmailAddress.Text)
                    strSb.Append("', mob_number='")
                    strSb.Append(txtMobileNo.Text)
                    strSb.Append("' where client_id='")
                    strSb.Append(Session.Item("ClientIDValue").ToString)
                    strSb.Append("'")
                    update_cmd = New SqlCommand(strSb.ToString, objconn.MyConnection)
                    update_cmd.ExecuteNonQuery()
                    objconn.disconnect()
                    lblMsg.Text = "Data Updated Successfully."
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Color.Green
                End If
            Catch ex As Exception
                objconn.disconnect()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn.disconnect()
                update_cmd = Nothing
                strSb = Nothing
            End Try
        End Sub
    End Class
    
End Namespace