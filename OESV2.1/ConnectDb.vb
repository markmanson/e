Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Configuration

Public Class ConnectDb
    Public MyConnection As System.Data.SqlClient.SqlConnection

    Public Function connect() As Boolean
        Try
            'Dim csb As New SqlConnectionStringBuilder(ConfigurationSettings.AppSettings("PathDb"))
            'csb.AsynchronousProcessing = True
            'Dim _connectionString As String = csb.ConnectionString
            'MyConnection = New SqlConnection(_connectionString)
            MyConnection = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            If MyConnection.State = ConnectionState.Closed Then
                MyConnection.Open()
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return True
    End Function

    Public Function disconnect() As Boolean
        If MyConnection IsNot Nothing Then
            If MyConnection.State = ConnectionState.Open Then
                MyConnection.Close()
            End If
        End If
    End Function

End Class
