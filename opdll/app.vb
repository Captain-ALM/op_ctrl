Public Class app
    Public Function startapp(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Process.Start(args(0), args(1))
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function getapps(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim toret As String = ""
                For Each current As Process In Process.GetProcesses()
                    toret = toret & current.Id & " : " & current.ProcessName & ControlChars.CrLf
                Next
                Return toret
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function stopapp(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                For Each current As Process In Process.GetProcesses()
                    If args(0) = current.Id Or args(0) = current.ProcessName Then
                        current.CloseMainWindow()
                    End If
                Next
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function killapp(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                For Each current As Process In Process.GetProcesses()
                    If args(0) = current.Id Or args(0) = current.ProcessName Then
                        current.Kill()
                    End If
                Next
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function startappnoargs(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Process.Start(args(0))
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function startappadminnoargs(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Process.Start(New ProcessStartInfo(args(0)) With {.Verb = "runas"})
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function startappadmin(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Process.Start(New ProcessStartInfo(args(0), args(1)) With {.Verb = "runas"})
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function
End Class

Public Class app_extended
    Public Function startapphidden(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Process.Start(New ProcessStartInfo(args(0), args(1)) With {.CreateNoWindow = True})
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function startappnoargshidden(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Process.Start(New ProcessStartInfo(args(0)) With {.CreateNoWindow = True})
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function startappandwait(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim p As Process = Process.Start(args(0), args(1))
                p.WaitForExit()
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function startappnoargsandwait(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim p As Process = Process.Start(args(0))
                p.WaitForExit()
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function
End Class
