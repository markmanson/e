Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing


Public Class Handler1
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest


        Dim objConn As New SqlConnection(ConfigurationManager.AppSettings("PathDb").ToString()) ' ConnectionStrings("PathDB").ConnectionString.ToString())
        If objConn.State = ConnectionState.Open Then
            objConn.Close()
        End If
        objConn.Open()

        Dim msatream As New MemoryStream()
        Dim query As String = "select user_photo from m_user_info where userid='" & context.Request.QueryString("id") & "'"
        Dim cmd As New SqlCommand(query, objConn)
        'Dim imgbyte() As Byte = DirectCast(cmd.ExecuteScalar(), Byte())
        Dim imgbyte As Byte()
        Try
            imgbyte = DirectCast(cmd.ExecuteScalar(), Byte())
        Catch ex As Exception

            'Dim bs As New MemoryStream()
            'bs.ReadByte()

            imgbyte = File.ReadAllBytes("Unirecruite/images/NoImage.jpg")
        End Try
        context.Response.ContentType = "Image/Jpeg"
        context.Response.BinaryWrite(imgbyte)
        objConn.Close()

        'context.Response.ContentType = "text/plain"
        'context.Response.Write("Hello World!")

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class