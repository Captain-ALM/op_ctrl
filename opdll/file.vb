Imports System.IO
Imports Microsoft.VisualBasic

Public Class file
    Public Function savefile(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                System.IO.File.WriteAllText(args(0), args(1))
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function savefilebin(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim arr As Byte() = convertstringtoobject(args(1))
                System.IO.File.WriteAllBytes(args(0), arr)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function openfile(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Return System.IO.File.ReadAllText(args(0))
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function openfilebin(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim arr As Byte() = System.IO.File.ReadAllBytes(args(0))
                Return convertobjecttostring(arr)
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function createdir(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                IO.Directory.CreateDirectory(args(0))
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function deletedir(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                IO.Directory.Delete(args(0), True)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function fileexists(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Return IO.File.Exists(obj(0))
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function direxists(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Return IO.Directory.Exists(obj(0))
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function list(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim toret As String = ""
                For Each current As String In IO.Directory.GetFileSystemEntries(obj(0))
                    toret = toret & current & ControlChars.CrLf
                Next
                Return toret
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function deletefile(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                IO.File.Delete(args(0))
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function drivelist(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
            Try
                Dim toret As String = ""
                For Each current As String In IO.Directory.GetLogicalDrives()
                    toret = toret & current & ControlChars.CrLf
                Next
                Return toret
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        Return "FAILED"
    End Function

    Public Function copyfile(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                IO.File.Copy(args(0), args(1))
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function copyfileow(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                IO.File.Copy(args(0), args(1), True)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function
End Class

Public Class file_extended
    Public Function savefile(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                FileIO.FileSystem.WriteAllText(args(0), args(1), False)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function savefilebin(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim arr As Byte() = convertstringtoobject(args(1))
                FileIO.FileSystem.WriteAllBytes(args(0), arr, False)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function openfile(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Return FileIO.FileSystem.ReadAllText(args(0))
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function openfilebin(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim arr As Byte() = FileIO.FileSystem.ReadAllBytes(args(0))
                Return convertobjecttostring(arr)
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function createdir(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                FileIO.FileSystem.CreateDirectory(args(0))
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function deletedir(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                FileIO.FileSystem.DeleteDirectory(args(0), FileIO.DeleteDirectoryOption.DeleteAllContents)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function fileexists(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Return FileIO.FileSystem.FileExists(obj(0))
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function direxists(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Return FileIO.FileSystem.DirectoryExists(obj(0))
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function list(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim toret As String = ""
                For Each current As String In FileIO.FileSystem.GetDirectories(args(0))
                    toret = toret & current & ControlChars.CrLf
                Next
                For Each current As String In FileIO.FileSystem.GetFiles(args(0))
                    toret = toret & current & ControlChars.CrLf
                Next
                Return toret
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function listallsub(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim toret As String = ""
                For Each current As String In FileIO.FileSystem.GetDirectories(args(0), FileIO.SearchOption.SearchAllSubDirectories)
                    toret = toret & current & ControlChars.CrLf
                Next
                For Each current As String In FileIO.FileSystem.GetFiles(args(0), FileIO.SearchOption.SearchAllSubDirectories)
                    toret = toret & current & ControlChars.CrLf
                Next
                Return toret
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function deletefile(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                FileIO.FileSystem.DeleteFile(args(0))
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function drivelist(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        Try
            Dim toret As String = ""
            For Each current As DriveInfo In FileIO.FileSystem.Drives
                toret = toret & current.VolumeLabel & " ; " & current.Name & ControlChars.CrLf
            Next
            Return toret
        Catch ex As Exception
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
        Return "FAILED"
    End Function

    Public Function copyfile(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                FileIO.FileSystem.CopyFile(args(0), args(1), False)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function copyfileow(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                FileIO.FileSystem.CopyFile(args(0), args(1), True)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function copydir(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                FileIO.FileSystem.CopyDirectory(args(0), args(1), False)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Public Function copydirow(ByVal obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                FileIO.FileSystem.CopyDirectory(args(0), args(1), True)
                Return "DONE"
            Catch ex As Exception
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function
End Class
