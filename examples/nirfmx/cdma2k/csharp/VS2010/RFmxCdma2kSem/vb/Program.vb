
NotInheritable Class Program
	Private Sub New()
	End Sub
	Friend Shared Sub Main()
		Dim cdma2kSem As New RFmxCdma2kSem()
		cdma2kSem.Run()
	End Sub
End Class
