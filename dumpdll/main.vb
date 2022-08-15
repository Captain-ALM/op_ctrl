Imports System.IO

Public Class main
    Function version(args As Object) As Object
        Return Me.GetType.Assembly.GetName.Version.ToString
    End Function

    Function functionlist(args As Object) As Object
        Return "main.functionlist" & ControlChars.CrLf & "main.version" _
            & ControlChars.CrLf & "main.dumpprogram" & ControlChars.CrLf & "main.dumpprogramfull" & ControlChars.CrLf & "main.dumpprogramcustom"
    End Function

    Function dumpprogram(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim ptd As Process = Process.GetProcessById(args(0))
                Dim fstream As FileStream = File.Open(args(1), FileMode.Create)
                MiniDumpWriteDump(ptd.Handle, ptd.Id, fstream.SafeFileHandle.DangerousGetHandle, MINIDUMP_TYPE.DumpNormal, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
                fstream.Close()
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function dumpprogramfull(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim ptd As Process = Process.GetProcessById(args(0))
                Dim fstream As FileStream = File.Open(args(1), FileMode.Create)
                MiniDumpWriteDump(ptd.Handle, ptd.Id, fstream.SafeFileHandle.DangerousGetHandle, MINIDUMP_TYPE.DumpWithFullMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
                fstream.Close()
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function dumpprogramcustom(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim ptd As Process = Process.GetProcessById(args(0))
                Dim fstream As FileStream = File.Open(args(1), FileMode.Create)
                MiniDumpWriteDump(ptd.Handle, ptd.Id, fstream.SafeFileHandle.DangerousGetHandle, args(2), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
                fstream.Close()
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function
End Class
