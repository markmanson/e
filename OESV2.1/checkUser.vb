Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration


Namespace unirecruite



Public Class checkUser
        Dim CONS As New unirecruite.Errconstants
    Public Function retUser(ByVal userid As String) As Boolean
        Dim mycommand As SqlCommand
        Dim myDataReader As SqlDataReader
        Dim objconn As New ConnectDb()
        Dim sqlstr As String
        If IsNothing(userid) Then
            userid = "-1"
        End If
            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                sqlstr = ""
                sqlstr = sqlstr & "SELECT COUNT(*) FROM m_user_info WHERE "
                sqlstr = sqlstr & "userid = " & userid
                mycommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = mycommand.ExecuteReader
                If myDataReader.Read() Then
                    If myDataReader.Item(0) = 0 Then
                        retUser = False
                    Else
                        retUser = True
                    End If
                    myDataReader.Close()
                    mycommand.Dispose()
                    objconn.disconnect()
                Else
                    retUser = False
                End If
            End If
    End Function
    Public Function retAdmin(ByVal loginname As String) As Boolean
        Dim mycommand As SqlCommand
        Dim myDataReader As SqlDataReader
        Dim objconn As New ConnectDb()
        Dim sqlstr As String
        If IsNothing(loginname) Then
            loginname = "0"
        End If

            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                sqlstr = ""
                sqlstr = sqlstr & "SELECT userid FROM m_user_info WHERE "
                sqlstr = sqlstr & "loginname = '" & loginname & "'"
                mycommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = mycommand.ExecuteReader
                If myDataReader.Read() Then

                    If myDataReader.Item(0) = 1 Then
                        retAdmin = True
                    Else
                        retAdmin = False
                    End If
                Else
                    retAdmin = False
                End If
                myDataReader.Close()
                mycommand.Dispose()
                objconn.disconnect()
            End If
    End Function

End Class

End Namespace
