Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices

Friend Module utils
    Public Function convertobjectargstostringargs(args As Object, Optional limit As Integer = 0) As String()
        Dim toret(0) As String
        If IsNothing(args) Then
            Return Nothing
        End If
        Try
            Dim numofi As Integer = numberofindexes(args, limit)
            ReDim toret(numofi - 1)
            For i As Integer = 0 To numofi - 1 Step 1
                toret(i) = args(i).ToString
            Next
        Catch ex As Exception
            Return Nothing
        End Try
        Return toret
    End Function

    Public Function numberofindexes(args As Object, Optional limit As Integer = 0) As Integer
        If IsNothing(args) Then
            Return 0
        End If
        Try
            Dim i As Integer = 0
            Try
                If limit = 0 Then
                    While True
                        Dim tmp As Object = args(i)
                        tmp = Nothing
                        i = i + 1
                    End While
                Else
                    While i <= limit
                        Dim tmp As Object = args(i)
                        tmp = Nothing
                        i = i + 1
                    End While
                End If
                Return i
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

    <DllImport("dbghelp.dll")> _
    Public Function MiniDumpWriteDump(ByVal hProcess As IntPtr, _
                                             ByVal ProcessId As Int32, _
                                             ByVal hFile As IntPtr, _
                                             ByVal DumpType As MINIDUMP_TYPE, _
                                             ByVal ExceptionParam As IntPtr, _
                                             ByVal UserStreamParam As IntPtr, _
                                             ByVal CallackParam As IntPtr) As Boolean
    End Function

    Public Enum MINIDUMP_TYPE
        DumpNormal = 0
        DumpWithDataSegs = 1
        DumpWithFullMemory = 2
        DumpWithHandleData = 4
        DumpFilterMemory = 8
        DumpScanMemory = 10
        DumpWithUnloadedModules = 20
        DumpWithIndirectlyReferencedMemory = 40
        DumpFilterModulePaths = 80
        DumpWithProcessThreadData = 100
        DumpWithPrivateReadWriteMemory = 200
        DumpWithoutOptionalData = 400
        DumpWithFullMemoryInfo = 800
        DumpWithThreadInfo = 1000
        DumpWithCodeSegs = 2000
    End Enum
End Module
