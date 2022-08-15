Imports System.Reflection
Imports System.Security.Principal

Public Class main
    Function version(args As Object) As Object
        Return Me.GetType.Assembly.GetName.Version.ToString
    End Function

    Function adminmodestart(args As Object) As Object
        Try
            Process.Start(New ProcessStartInfo(Assembly.GetEntryAssembly().Location) With {.Verb = "runas"})
            Return "DONE"
        Catch ex As Exception
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
        Return "FAIL"
    End Function

    Function clientversion(args As Object) As Object
        Return Assembly.GetEntryAssembly.GetName.Version.ToString
    End Function

    Function clientdata(args As Object) As Object
        Return "User:" & WindowsIdentity.GetCurrent.Name & ControlChars.CrLf & "Sid:" & WindowsIdentity.GetCurrent.User.Value & ControlChars.CrLf & "Admin:" & New WindowsPrincipal(WindowsIdentity.GetCurrent).IsInRole(WindowsBuiltInRole.Administrator).ToString
    End Function

    Function functionlist(args As Object) As Object
        Return "main.adminmodestart" & ControlChars.CrLf & "main.clientdata" & ControlChars.CrLf & "main.clientversion" & ControlChars.CrLf & "main.version" & ControlChars.CrLf & "main.functionlist" & ControlChars.CrLf & "app.startapp" & ControlChars.CrLf & "app.getapps" & ControlChars.CrLf & "app.stopapp" & ControlChars.CrLf & "app.killapp" & ControlChars.CrLf & "file.createdir" & ControlChars.CrLf & "file.deletedir" & ControlChars.CrLf & "file.deletefile" & ControlChars.CrLf & "file.direxists" & ControlChars.CrLf & "file.drivelist" & ControlChars.CrLf & "file.fileexists" & ControlChars.CrLf & "file.list" & ControlChars.CrLf & "file.openfile" & ControlChars.CrLf & "file.openfilebin" & ControlChars.CrLf & "file.savefile" & ControlChars.CrLf & "file.savefilebin" & ControlChars.CrLf & "app.startappnoargs" & ControlChars.CrLf & "app.startappadminnoargs" & ControlChars.CrLf & "app.startappadmin" & ControlChars.CrLf & "file.copyfile" & ControlChars.CrLf & "file.copyfileow" & ControlChars.CrLf & "file_extended.createdir" & ControlChars.CrLf & "file_extended.deletedir" & ControlChars.CrLf & "file_extended.deletefile" & ControlChars.CrLf & "file_extended.direxists" & ControlChars.CrLf & "file_extended.drivelist" & ControlChars.CrLf & "file_extended.fileexists" & ControlChars.CrLf & "file_extended.list" & ControlChars.CrLf & "file_extended.openfile" & ControlChars.CrLf & "file_extended.openfilebin" & ControlChars.CrLf & "file_extended.savefile" & ControlChars.CrLf & "file_extended.savefilebin" & ControlChars.CrLf & "file_extended.copyfile" & ControlChars.CrLf & "file_extended.copyfileow" & ControlChars.CrLf & "file_extended.copydir" & ControlChars.CrLf & "file_extended.copydirow" & ControlChars.CrLf & "file_extended.listallsub"
    End Function
End Class