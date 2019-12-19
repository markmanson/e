Imports System.Data.OleDb
Imports System.Data.SqlClient

Module Module1

    Public Sub ReadCentreFromXL1()
        Dim adapOleDb As OleDbDataAdapter
        Dim dsOledb As New DataSet
        Dim connStr As String
        Dim strTemp As String
        Dim intTempRow, intFirstNameRow As Integer
        Dim intTempCol, intFirstNameCol As Integer
        ' Access the file from sever folder

        ' By Nisha on 2018/05/17
        ' Modify folder name from Excel Import to ExcelImport 
        strTemp = "ExcelImport\Bulk user data.XLT"
        connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        "Data Source=" + strTemp + ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=1;"""
        adapOleDb = New OleDbDataAdapter("SELECT * FROM [Center Details$]", connStr)
        adapOleDb.TableMappings.Add("Table", "Excel")
        adapOleDb.Fill(dsOledb)
        While intTempRow <= dsOledb.Tables(0).Rows.Count - 1
            For intTempCol = 0 To dsOledb.Tables(0).Columns.Count - 1
                strTemp = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                'If strTemp.ToUpper = "CAMPUS ID (COLGNAME+YEAR)" Then
                If RemoveSpace1(strTemp.ToString()) = "FirstName" Then
                    intFirstNameRow = intTempRow
                    intFirstNameCol = intTempCol
                    Exit While
                End If
            Next
            intTempRow = intTempRow + 1
        End While
    End Sub
    Public Function RemoveSpace1(ByVal StrScr As String)
        Dim strTemp As String = ""
        Try
            For Each ch As Char In StrScr
                If ch <> " " Then
                    strTemp += ch
                End If
            Next
            Return strTemp
        Catch ex As Exception
            'If log.IsDebugEnabled Then
            '    log.Debug("Error :" & ex.ToString)
            'End If
        Finally
            strTemp = Nothing
        End Try
    End Function

    Public Function ConvertDate(ByVal strdate As String) As String
        Dim str As String = ""
        If strdate = "" Then
            Return ""
        End If
        If strdate.Contains("-") Then
            Dim val() As String = strdate.Split("-")
            str = val(2) & "/" & val(1) & "/" & val(0)
        Else
            Dim val() As String = strdate.Split("/")
            str = val(2) & "/" & val(1) & "/" & val(0)
        End If

        Return str
    End Function

    'Created by Pranit on 2019/12/12
    Public Function ConvertDate2(ByVal strdate As String) As String
        Dim str As String = ""
        If strdate = "" Then
            Return ""
        End If
        If strdate.Contains("-") Then
            Dim val() As String = strdate.Split("-")
            str = val(0) & "/" & val(1) & "/" & val(2)
        Else
            Dim val() As String = strdate.Split("/")
            str = val(0) & "/" & val(1) & "/" & val(2)
        End If

        Return str
    End Function

    'Created by Pranit on 2019/12/18
    Public Function ConvertDateForDOB(ByVal strdate As String) As String
        Dim str As String = ""
        If strdate = "" Then
            Return ""
        End If
        If strdate.Contains("/") Then
            Dim val() As String = strdate.Split("/")
            str = val(2) & "-" & val(0) & "-" & val(1)
        End If

        Return str
    End Function



End Module
