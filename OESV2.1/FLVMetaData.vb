
Imports System.Data
Imports System.Configuration
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Xml.Linq
Imports System.IO
Public Class FLVMetaData
    ' <summary>
    ' Reads the meta information (if present) in an FLV
    ' </summary>
    ' <param name="path">The path to the FLV file</returns>
    Public Function GetFlvMetaInfo(ByVal path As String) As FlvMetaInfo
        'if (!File.Exists(path))
        '{
        '    throw new Exception(String.Format("File '{0}' doesn't exist for FlvMetaDataReader.GetFlvMetaInfo(path)", path));
        '}
        Dim hasMetaData As Boolean = False
        Dim duration As Double = 0
        Dim width As Double = 0
        Dim height As Double = 0
        Dim videoDataRate As Double = 0
        Dim audioDataRate As Double = 0
        Dim frameRate As [Double] = 0
        Dim creationDate As DateTime = DateTime.MinValue
        ' open file 
        Dim fileStream As New FileStream(path, FileMode.Open)
        Try
            ' read where "onMetaData"
            Dim bytes As Byte() = New Byte(9) {}
            fileStream.Seek(27, SeekOrigin.Begin)
            Dim result As Integer = fileStream.Read(bytes, 0, 10)
            ' if "onMetaData" exists then proceed to read the attributes
            Dim onMetaData As String = ByteArrayToString(bytes)
            If onMetaData = "onMetaData" Then
                hasMetaData = True
                ' 16 bytes past "onMetaData" is the data for "duration" 
                duration = GetNextDouble(fileStream, 16, 8)
                ' 8 bytes past "duration" is the data for "width"
                width = GetNextDouble(fileStream, 8, 8)
                ' 9 bytes past "width" is the data for "height"
                height = GetNextDouble(fileStream, 9, 8)
                ' 16 bytes past "height" is the data for "videoDataRate"
                videoDataRate = GetNextDouble(fileStream, 16, 8)
                ' 16 bytes past "videoDataRate" is the data for "audioDataRate"
                audioDataRate = GetNextDouble(fileStream, 16, 8)
                ' 12 bytes past "audioDataRate" is the data for "frameRate"
                frameRate = GetNextDouble(fileStream, 12, 8)
                ' read in bytes for creationDate manually
                fileStream.Seek(17, SeekOrigin.Current)
                Dim seekBytes As Byte() = New Byte(23) {}
                result = fileStream.Read(seekBytes, 0, 24)
                Dim dateString As String = ByteArrayToString(seekBytes)
                ' create .NET readable date string
                ' cut off Day of Week
                dateString = dateString.Substring(4)
                ' grab 1) month and day, 2) year, 3) time
                dateString = dateString.Substring(0, 6) & " " & dateString.Substring(16, 4) & " " & dateString.Substring(7, 8)
                ' .NET 2.0 has DateTime.TryParse
                Try




                    creationDate = Convert.ToDateTime(dateString)
                Catch

                End Try
            End If
            ' no error handling
        Catch e As Exception
        Finally
            fileStream.Close()
        End Try

        ' FlvMetaInfo ss();
        'ss.Duration =duration; 
        Return New FlvMetaInfo(hasMetaData, duration, width, height, videoDataRate, audioDataRate, _
         frameRate, creationDate)
    End Function
    Private Shared Function GetNextDouble(ByVal fileStream As FileStream, ByVal offset As Integer, ByVal length As Integer) As [Double]
        ' move the desired number of places in the array
        fileStream.Seek(offset, SeekOrigin.Current)
        ' create byte array
        Dim bytes As Byte() = New Byte(length - 1) {}
        ' read bytes
        Dim result As Integer = fileStream.Read(bytes, 0, length)
        ' convert to double (all flass values are written in reverse order)
        Return ByteArrayToDouble(bytes, True)
    End Function
    Private Shared Function ByteArrayToString(ByVal bytes As Byte()) As String
        Dim byteString As String = String.Empty
        For Each b As Byte In bytes
            byteString += Convert.ToChar(b).ToString()
        Next
        Return byteString
    End Function
    Private Shared Function ByteArrayToDouble(ByVal bytes As Byte(), ByVal readInReverse As Boolean) As [Double]
        If bytes.Length <> 8 Then
            Throw New Exception("bytes must be exactly 8 in Length")
        End If
        If readInReverse Then
            Array.Reverse(bytes)
        End If
        Return BitConverter.ToDouble(bytes, 0)
    End Function
End Class
''' <summary>
''' Read only container holding meta data embedded in FLV files
''' </summary>
Public Class FlvMetaInfo
    Public _duration As [Double]
    Public _width As [Double]
    Public _height As [Double]
    Public _frameRate As [Double]
    Public _videoDataRate As [Double]
    Public _audioDataRate As [Double]
    Public _creationDate As DateTime
    Public _hasMetaData As Boolean
    ''' <summary>
    ''' The duration in seconds of the video
    ''' </summary>
    ''' 
    Public Sub New()
    End Sub
    Public Property Duration() As [Double]
        Get
            Return _duration
        End Get
        Set(ByVal value As [Double])
            _duration = value
        End Set
    End Property
    ''' <summary>
    ''' The width in pixels of the video
    ''' </summary>
    Public ReadOnly Property Width() As [Double]
        Get
            Return _width
        End Get
    End Property
    'set { _width = value; }
    ''' <summary>
    ''' The height in pixels of the video
    ''' </summary>
    Public ReadOnly Property Height() As [Double]
        Get
            Return _height
        End Get
    End Property
    'set { _height = value; }
    ''' <summary>
    ''' The data rate in KB/sec of the video 
    ''' </summary>
    Public ReadOnly Property VideoDataRate() As [Double]
        Get
            Return _videoDataRate
        End Get
    End Property
    'set { _videoDataRate = value; }
    ''' <summary>
    ''' The data rate in KB/sec of the video's audio track
    ''' </summary>
    Public ReadOnly Property AudioDataRate() As [Double]
        Get
            Return _audioDataRate
        End Get
    End Property
    'set { _audioDataRate = value; }
    ''' <summary>
    ''' The frame rate of the video
    ''' </summary>
    Public ReadOnly Property FrameRate() As [Double]
        Get
            Return _frameRate
        End Get
    End Property
    'set { _frameRate = value; }
    ''' <summary>
    ''' The creation date of the video
    ''' </summary>
    Public ReadOnly Property CreationDate() As DateTime
        Get
            Return _creationDate
        End Get
    End Property
    'set { _creationDate = value; }
    ''' <summary>
    ''' Whether or not the FLV has meta data
    ''' </summary>
    Public ReadOnly Property HasMetaData() As Boolean
        Get
            Return _hasMetaData
        End Get
    End Property
    'set { _hasMetaData = value; }
    Friend Sub New(ByVal hasMetaData As Boolean, ByVal duration As [Double], ByVal width As [Double], ByVal height As [Double], ByVal videoDataRate As [Double], ByVal audioDataRate As [Double], _
     ByVal frameRate As [Double], ByVal creationDate As DateTime)
        Dim ss As New FlvMetaInfo()
        _hasMetaData = hasMetaData
        '          _duration = duration;
        ss.Duration = duration
        'i tried like this to set the property value from here. 
        _duration = duration
        _width = width
        _height = height
        _videoDataRate = videoDataRate
        _audioDataRate = audioDataRate
        _frameRate = frameRate
        _creationDate = creationDate
    End Sub
End Class

