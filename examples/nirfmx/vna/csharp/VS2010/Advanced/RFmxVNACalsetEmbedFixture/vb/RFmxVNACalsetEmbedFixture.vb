'Steps:
'1. Open a new RFmx session.
'2. Load Calset data from a file.
'3. Embed reverse of the input Fixture network from s2p file into new Calset.
'4. Save new Calset with Embedding applied.
'5. Read Frequency Grid for each calset.
'6. Fetch Error Terms for each calset - Directivity, Source Match, Reflection Tracking
'7. Close RFmx Session.

Imports System.IO
Imports System.Reflection
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVNACalsetEmbedFixture
    Public Class RFmxVNACalsetEmbedFixture
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private vnaPort As String

        Private s2pFixtureFilePath As String
        Private sParameterOrientation As RFmxVnaMXSParameterOrientation

        Private calsetFilePath As String
        Private outputCalsetFilePath As String
        Private calsetNames As String()

        Private frequencyGrid As Double()

        Private currentDirectoryPath As String

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureVna()
                RetrieveResults()
            Catch ex As Exception
                DisplayError(ex)
            Finally
                'Close session
                CloseSession()
                Console.WriteLine("Press any key to exit")
                Console.ReadKey()
            End Try
        End Sub

        Private Sub InitializeVariables()
            resourceName = "VNA"
            vnaPort = "port1"

            s2pFixtureFilePath = "1dB_Attenuation.s2p"
            sParameterOrientation = RFmxVnaMXSParameterOrientation.Port1TowardsVna

            calsetFilePath = "Calset_Embed_Fixture.ncst"
            outputCalsetFilePath = "Calset_Embed_Fixture_(Embedding_Applied).ncst"
            calsetNames = New String() {"Original Calset", "New Calset (Embedding Applied)"}
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()                                                      'Create a new RFmx Session

            currentDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\"

            vna.CalsetLoadFromFile("", calsetNames(0), currentDirectoryPath + calsetFilePath)
            vna.CalsetEmbedFixtureS2p("", calsetNames(0), currentDirectoryPath + s2pFixtureFilePath, vnaPort, sParameterOrientation, calsetNames(1))
            vna.CalsetSaveToFile("", calsetNames(1), currentDirectoryPath + outputCalsetFilePath)
        End Sub

        Private Sub RetrieveResults()
            Dim errorTermIdentifier() As RFmxVnaMXCalErrorTerm = New RFmxVnaMXCalErrorTerm() {RFmxVnaMXCalErrorTerm.Directivity, RFmxVnaMXCalErrorTerm.SourceMatch, RFmxVnaMXCalErrorTerm.ReflectionTracking}
            For Each calsetName As String In calsetNames
                vna.CalsetGetFrequencyGrid("", calsetName, RFmxVnaMXCalFrequencyGrid.Directivity, frequencyGrid)
                Dim errorTerms(frequencyGrid.Length) As ComplexSingle
                For Each errorTermType As RFmxVnaMXCalErrorTerm In CType(errorTermIdentifier, Collections.Generic.IEnumerable(Of RFmxVnaMXCalErrorTerm))
                    vna.CalsetGetErrorTerm("", calsetName, errorTermType, vnaPort, vnaPort, errorTerms)
                    Dim errorTermMagnitude() As Single = CalculateMagnitude(errorTerms)
                Next
            Next
        End Sub

        Private Function CalculateMagnitude(ByRef complexArray() As ComplexSingle) As Single()
            Dim magnitude(complexArray.Length - 1) As Single
            For i As Integer = 0 To complexArray.Length - 1
                Dim absValue As Double = Math.Sqrt(complexArray(i).Real * complexArray(i).Real + complexArray(i).Imaginary * complexArray(i).Imaginary)
                magnitude(i) = CSng(20 * Math.Log10(absValue))
            Next
            Return magnitude
        End Function

        Private Sub CloseSession()
            If vna IsNot Nothing Then
                vna.Dispose()
                vna = Nothing
            End If
            If instrSession IsNot Nothing Then
                instrSession.Close()
                instrSession = Nothing
            End If
        End Sub

        Private Shared Sub DisplayError(ex As Exception)
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " + ex.Message)
        End Sub
    End Class
End Namespace
