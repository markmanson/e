Imports System.Data.SqlClient
Imports System.Data

Module connect
    Dim strPathDb As String
    Dim sqlconn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))

End Module
