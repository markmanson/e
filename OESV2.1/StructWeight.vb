Public Class StructWeight
    Dim test_type As Integer

    Dim TFB As Integer
    Dim TFI As Integer

    Dim MCB As Integer
    Dim MCI As Integer

    Dim BLB As Integer
    Dim BLI As Integer

    Dim NofTFB As Integer
    Dim NofTFI As Integer
    Dim NofMCB As Integer
    Dim NofMCI As Integer
    Dim NofBLB As Integer
    Dim NofBLI As Integer



    Public Property TestType() As Integer
        Get
            Return test_type
        End Get
        Set(ByVal value As Integer)
            test_type = value
        End Set
    End Property

    Public Property TFBasic() As Integer
        Get
            Return TFB
        End Get
        Set(ByVal value As Integer)
            TFB = value
        End Set
    End Property

    Public Property TFIMed() As Integer
        Get
            Return TFI
        End Get
        Set(ByVal value As Integer)
            TFI = value
        End Set
    End Property

    Public Property MCBasic() As Integer
        Get
            Return MCB
        End Get
        Set(ByVal value As Integer)
            MCB = value
        End Set
    End Property

    Public Property MCIMed() As Integer
        Get
            Return MCI
        End Get
        Set(ByVal value As Integer)
            MCI = value
        End Set
    End Property

    Public Property BLBasic() As Integer
        Get
            Return BLB
        End Get
        Set(ByVal value As Integer)
            BLB = value
        End Set
    End Property

    Public Property BLIMed() As Integer
        Get
            Return BLI
        End Get
        Set(ByVal value As Integer)
            BLI = value
        End Set
    End Property

    Public Property NumberTFB() As Integer
        Get
            Return NofTFB
        End Get
        Set(ByVal value As Integer)
            NofTFB = value
        End Set
    End Property

    Public Property NumberTFI() As Integer
        Get
            Return NofTFI
        End Get
        Set(ByVal value As Integer)
            NofTFI = value
        End Set
    End Property

    Public Property NumberMCB() As Integer
        Get
            Return NofMCB
        End Get
        Set(ByVal value As Integer)
            NofMCB = value
        End Set
    End Property

    Public Property NumberMCI() As Integer
        Get
            Return NofMCI
        End Get
        Set(ByVal value As Integer)
            NofMCI = value
        End Set
    End Property

    Public Property NumberBLB() As Integer
        Get
            Return NofBLB
        End Get
        Set(ByVal value As Integer)
            NofBLB = value
        End Set
    End Property

    Public Property NumberBLI() As Integer
        Get
            Return NofBLI
        End Get
        Set(ByVal value As Integer)
            NofBLI = value
        End Set
    End Property





End Class
