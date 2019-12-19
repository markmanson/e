Imports Microsoft.VisualBasic

Public Class StructureQuePaper

    Dim intQueId As Integer
    Dim strQueDesc As String
    Dim intCorrectOpt As Integer
    Dim strTest_type As String
    Dim strQuesTypeID As String
    Dim strQuesAudio As String
    Dim htOpions As Hashtable

    'Dim arrOptIds As ArrayList
    'Dim arrOptTxt As ArrayList



    Public Sub New()
        htOpions = New Hashtable()
    End Sub
    'Dim intOpt1Id As Integer
    'Dim intOpt2Id As Integer
    'Dim intOpt3Id As Integer
    'Dim intOpt4Id As Integer

    ' Put Array over here

    'Dim strOpt1Txt As String
    'Dim strOpt2Txt As String
    'Dim strOpt3Txt As String
    'Dim strOpt4Txt As String


  
    Public Property Options() As Hashtable
        Get
            Return htOpions
        End Get

        Set(ByVal value As Hashtable)
            htOpions = value
        End Set
    End Property


    Public Property QuesTypeID() As String
        Get
            Return strQuesTypeID
        End Get

        Set(ByVal value As String)
            strQuesTypeID = value
        End Set
    End Property

    '----------Added by pragnesha----------
    Public Property QueAudio() As String
        Get
            Return strQuesAudio
        End Get

        Set(ByVal value As String)
            strQuesAudio = value
        End Set
    End Property
    '-----------Ended by pragnesha------------
    Public Property QueId() As Integer
        Get
            Return intQueId
        End Get

        Set(ByVal value As Integer)
            intQueId = value
        End Set
    End Property

    Public Property QueDesc() As String
        Get
            Return strQueDesc
        End Get

        Set(ByVal value As String)
            strQueDesc = value
        End Set
    End Property

    Public Property CorrectOpt() As String
        Get
            Return intCorrectOpt
        End Get

        Set(ByVal value As String)
            intCorrectOpt = value
        End Set
    End Property

    Public Property Test_type() As String
        Get
            Return strTest_type
        End Get

        Set(ByVal value As String)
            strTest_type = value
        End Set
    End Property


    'Public Property Opt1Id() As Integer
    '    Get
    '        Return intOpt1Id
    '    End Get

    '    Set(ByVal value As Integer)
    '        intOpt1Id = value
    '    End Set
    'End Property

    'Public Property Opt2Id() As Integer
    '    Get
    '        Return intOpt2Id
    '    End Get

    '    Set(ByVal value As Integer)
    '        intOpt2Id = value
    '    End Set
    'End Property

    'Public Property Opt3Id() As Integer
    '    Get
    '        Return intOpt3Id
    '    End Get

    '    Set(ByVal value As Integer)
    '        intOpt3Id = value
    '    End Set
    'End Property

    'Public Property Opt4Id() As Integer
    '    Get
    '        Return intOpt4Id
    '    End Get

    '    Set(ByVal value As Integer)
    '        intOpt4Id = value
    '    End Set
    'End Property

    'Public Property Opt1Txt() As String
    '    Get
    '        Return strOpt1Txt
    '    End Get

    '    Set(ByVal value As String)
    '        strOpt1Txt = value
    '    End Set
    'End Property

    'Public Property Opt2Txt() As String
    '    Get
    '        Return strOpt2Txt
    '    End Get

    '    Set(ByVal value As String)
    '        strOpt2Txt = value
    '    End Set
    'End Property

    'Public Property Opt3Txt() As String
    '    Get
    '        Return strOpt3Txt
    '    End Get

    '    Set(ByVal value As String)
    '        strOpt3Txt = value
    '    End Set
    'End Property

    'Public Property Opt4Txt() As String
    '    Get
    '        Return strOpt4Txt
    '    End Get

    '    Set(ByVal value As String)
    '        strOpt4Txt = value
    '    End Set
    'End Property




End Class
