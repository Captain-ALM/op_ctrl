Public Class main
    Function playfile(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                My.Computer.Audio.Play(args(0), AudioPlayMode.WaitToComplete)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function playbytes(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim byt As Byte() = CType(convertstringtoobject(args(0)), Byte())
                My.Computer.Audio.Play(byt, AudioPlayMode.WaitToComplete)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function playfilebackground(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                My.Computer.Audio.Play(args(0), AudioPlayMode.Background)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function playbytesbackground(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim byt As Byte() = CType(convertstringtoobject(args(0)), Byte())
                My.Computer.Audio.Play(byt, AudioPlayMode.Background)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function playfilebackgroundloop(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                My.Computer.Audio.Play(args(0), AudioPlayMode.BackgroundLoop)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function playbytesbackgroundloop(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim byt As Byte() = CType(convertstringtoobject(args(0)), Byte())
                My.Computer.Audio.Play(byt, AudioPlayMode.BackgroundLoop)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function stopaudio(obj As Object) As Object
        Try
            My.Computer.Audio.Stop()
            Return "DONE"
        Catch ex As Exception
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
        Return "FAILED"
    End Function

    Function version(args As Object) As Object
        Return Me.GetType.Assembly.GetName.Version.ToString
    End Function

    Function functionlist(args As Object) As Object
        Return "main.functionlist" & ControlChars.CrLf & "main.version" & ControlChars.CrLf & "main.stopaudio" _
            & ControlChars.CrLf & "main.playfile" & ControlChars.CrLf & "main.playbytes" _
            & ControlChars.CrLf & "main.playfilebackground" & ControlChars.CrLf & "main.playbytesbackground" _
            & ControlChars.CrLf & "main.playfilebackgroundloop" & ControlChars.CrLf & "main.playbytesbackgroundloop"
    End Function
End Class
