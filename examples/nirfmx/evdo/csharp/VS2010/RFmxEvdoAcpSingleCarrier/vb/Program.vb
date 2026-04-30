
NotInheritable Class Program
	Private Sub New()
	End Sub
	Friend Shared Sub Main()
		Dim AcpSingleCarrier As New RFmxEvdoAcpSingleCarrier()
		AcpSingleCarrier.Run()
	End Sub
End Class
