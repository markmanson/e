Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Web.Mail
Imports System.Configuration


Namespace unirecruite

    Public Class mailcom
        'Dim CONS As New Constant()
        Private Function getEmailId(ByVal userid As String) As String
            Dim objconn As New ConnectDb()
            Dim sqlstr As String
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader

            sqlstr = "SELECT email FROM m_user_info"
            sqlstr = sqlstr & " WHERE userid=" & CInt(userid)

            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader()
                While myDataReader.Read
                    getEmailId = myDataReader.Item("email")
                End While
            End If
            myCommand.Dispose()
            myDataReader.Close()
            objconn.disconnect()
        End Function

        'this will send the mail for examination
        Public Function MailForExam(ByVal userid As Integer, ByVal mailsub As String, ByVal filename As String, ByVal testid As String)
            sendMail(getEmailId(CInt(userid)), mailsub, getMsg(CInt(userid), filename, testid))
        End Function

        Public Function getMsg(ByVal userid As Integer, ByVal filename As String, ByVal testid As String) As String
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim msg As String
            Dim objStreamReader As StreamReader

            'Code commented by Pratik
            'If objconn.connect(objconn.PATHDB) Then
            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                sqlstr = ""
                sqlstr = sqlstr & "SELECT b.name, "
                sqlstr = sqlstr & "b.email, "
                sqlstr = sqlstr & "a.written_test_date, "
                sqlstr = sqlstr & "a.written_test_appear_date, "
                'sqlstr = sqlstr & "a.loginname, "
                'sqlstr = sqlstr & "a.pwd, "
                sqlstr = sqlstr & "c.test_name "
                sqlstr = sqlstr & "FROM t_candidate_status as a,"
                sqlstr = sqlstr & "m_user_info as b, "
                sqlstr = sqlstr & "m_testinfo as c "
                sqlstr = sqlstr & "WHERE(a.userid = b.userid) "
                sqlstr = sqlstr & "AND (a.test_type = c.test_type) "
                sqlstr = sqlstr & "AND b.userid=" & userid & " "
                sqlstr = sqlstr & "AND c.test_type='" & testid & "'"

                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader()
                'filename = ConfigurationSettings.AppSettings("FailureMail")
                objStreamReader = File.OpenText(filename)
                While myDataReader.Read
                    msg = objStreamReader.ReadToEnd
                    msg = Replace(msg, "&#Name#&", myDataReader.Item("name"))
                    msg = Replace(msg, "&#sub#&", myDataReader.Item("test_name"))
                    msg = Replace(msg, "&#testdate#&", Left(myDataReader.Item("written_test_date"), 10))
                    msg = Replace(msg, "&#appeardate#&", Left(myDataReader.Item("written_test_appear_date"), 10))
                    'msg = Replace(msg, "&#links#&", h)
                    'msg = Replace(msg, "&#pass#&", myDataReader.Item("pwd"))
                    getMsg = msg
                    'sendMail(myDataReader.Item("email_address"), CInt(userid))
                End While
                'Kamal on 24/01/06
                objStreamReader.Close()
                myCommand.Dispose()
                myDataReader.Close()
                objconn.disconnect()
            End If
        End Function

        'Send mail to the person
        Public Function sendMail(ByVal StrTo As String, ByVal mailsub As String, ByVal messageBody As String, Optional ByVal StrFrom As String = "")
            If StrTo <> "" Then
                Dim mail As New MailMessage

                Dim strMailFrom As String
                strMailFrom = ConfigurationSettings.AppSettings("MailFrom")
                If StrFrom = "" Then StrFrom = strMailFrom
                mail.From = StrFrom
                mail.To = StrTo
                mail.Subject = mailsub
                mail.Body = messageBody
                mail.BodyFormat = MailFormat.Text
                Dim strSmtpServer As String
                strSmtpServer = ConfigurationSettings.AppSettings("SmtpServer")

                '********** Added this code for Server Authentication ***************/

                mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = "uks.mail"
                mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25
                mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                'mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1
                'mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusername") = "abdur"
                'mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "abdur"

                SmtpMail.SmtpServer = strSmtpServer
                SmtpMail.Send(mail)
            End If
        End Function
    End Class

End Namespace
