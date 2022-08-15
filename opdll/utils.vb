Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Friend Module utils
    Public Function convertobjectargstostringargs(args As Object) As String()
        Dim toret(0) As String
        If IsNothing(args) Then
            Return Nothing
        End If
        Try
            Dim numofi As Integer = numberofindexes(args)
            ReDim toret(numofi - 1)
            For i As Integer = 0 To numofi - 1 Step 1
                toret(i) = args(i).ToString
            Next
        Catch ex As Exception
            Return Nothing
        End Try
        Return toret
    End Function

    Public Function numberofindexes(args As Object) As Integer
        If IsNothing(args) Then
            Return 0
        End If
        Try
            Dim i As Integer = 0
            Try
                While "True"
                    Dim tmp As Object = args(i)
                    tmp = Nothing
                    i = i + 1
                End While
                Return 0
            Catch ex As Exception
                Return i
            End Try
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function convertobjecttostring(obj As Object) As String
        Try
            Dim memorysteam As New MemoryStream
            Dim formatter As New BinaryFormatter()
            formatter.Serialize(memorysteam, obj)
            Dim toreturn As String = Convert.ToBase64String(memorysteam.ToArray)
            formatter = Nothing
            memorysteam.Dispose()
            memorysteam = Nothing
            Return toreturn
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function convertstringtoobject(str As String) As Object
        Try
            Dim memorysteam As MemoryStream = New MemoryStream(Convert.FromBase64String(str))
            Dim formatter As BinaryFormatter = New BinaryFormatter()
            Dim retobj As Object = formatter.Deserialize(memorysteam)
            formatter = Nothing
            memorysteam.Dispose()
            memorysteam = Nothing
            Return retobj
        Catch ex As Exception
            Return New Object
        End Try
    End Function
End Module
