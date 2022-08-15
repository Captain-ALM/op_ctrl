Imports captainalm.calmclientandserver
Imports System.Threading
Imports System.Reflection
Imports System.IO
Imports System.Security.Principal

Module runner
    Friend to_exit As Boolean = False
    Friend to_restart As Boolean = False
    Friend restart_args As String = ""
    Friend this_assembly As Assembly = Assembly.GetEntryAssembly()
    Friend program_path As String = this_assembly.Location
    Friend program_dir As String = Path.GetDirectoryName(program_path)
    Friend program_args As String()
    Friend ip_address As String = "127.0.0.1"
    Friend port As Integer = 9786
    Friend dll_path As String = ""
    Friend dlls_loaded As New Dictionary(Of String, Assembly)
    Friend connected As Boolean = False
    Friend pass As String = "tor stinks"
    Friend disconnected_so_timeout As Boolean = False
    Friend nodelay As Boolean = False
    Friend embedded As Boolean = False
    Friend running_c As Boolean = False
    Friend stop_the_service As Boolean = False

    Sub main()
        running_c = True
        init()
        While Not to_exit
            runtime()
            Thread.Sleep(1500)
        End While
        shutdown()
        running_c = False
        to_exit = False
        to_restart = False
    End Sub

    Sub init()
        'read program args
        Try
            For i As Integer = 1 To program_args.Length - 1 Step 1
                Dim carg As String = ""
                Dim cargarg As String = ""
                If program_args(i).Contains("=") Then
                    carg = program_args(i).Substring(0, program_args(i).IndexOf("="))
                    cargarg = program_args(i).Substring(program_args(i).IndexOf("=") + 1)
                Else
                    carg = program_args(i)
                    cargarg = ""
                End If
                If carg.ToLower = "a" Or carg.ToLower = "address" Or carg.ToLower = "ip" Then
                    If cargarg <> "" Then
                        If check_if_ip_address(cargarg) Then
                            ip_address = cargarg
                        ElseIf cargarg.ToLower.StartsWith("loop") And cargarg.ToLower.EndsWith("back") Then
                            ip_address = "127.0.0.1"
                        End If
                    Else
                        ip_address = "127.0.0.1"
                    End If
                ElseIf carg.ToLower = "p" Or carg.ToLower = "port" Then
                    If cargarg <> "" Then
                        If convert_string_to_integer(cargarg) > 0 Then
                            port = convert_string_to_integer(cargarg)
                            If port > 65535 Then port = 65535
                            If port < 1 Then port = 1
                        Else
                            port = 9786
                        End If
                    Else
                        port = 9786
                    End If
                ElseIf carg.ToLower = "d" Or (carg.ToLower.StartsWith("dll") And carg.ToLower.EndsWith("path")) Or carg.ToLower = "path" Then
                    If cargarg <> "" Then
                        If check_if_dir_exists(cargarg) Then
                            dll_path = cargarg
                        Else
                            dll_path = ""
                        End If
                    Else
                        dll_path = ""
                    End If
                ElseIf carg.ToLower = "e" Or carg.ToLower = "embedded" Then
                    embedded = True
                ElseIf carg.ToLower = "n" Or carg.ToLower = "nd" Then
                    nodelay = True
                End If
            Next
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try

        'load dep dll's (if path not blank)
        If dll_path <> "" And Not embedded Then
            Try
                Dim dlls_to_load As String() = Directory.GetFiles(dll_path)

                If Not IsNothing(dlls_to_load) Then
                    For i As Integer = 0 To dlls_to_load.Length - 1 Step 1
                        Try
                            Dim cfile As String = dlls_to_load(i)
                            If Path.GetExtension(cfile) = ".dll" Then
                                Dim ass As Assembly = Assembly.LoadFile(cfile)
                                Dim ass_nom As String = Path.GetFileNameWithoutExtension(cfile)
                                If Not dlls_loaded.ContainsKey(ass_nom) Then
                                    dlls_loaded.Add(ass_nom, ass)
                                End If
                            End If
                        Catch ex As ThreadAbortException
                            Thread.CurrentThread.Abort()
                        Catch ex As Exception
                        End Try
                    Next
                End If
            Catch ex As ThreadAbortException
                Thread.CurrentThread.Abort()
            Catch ex As Exception
            End Try
        End If
    End Sub

    Sub runtime()
        If disconnected_so_timeout Then
            disconnected_so_timeout = False
            Thread.Sleep(15000)
            client_name = getNetworkAdapterIP().ToString & "-" & WindowsIdentity.GetCurrent.User.ToString
        End If
        If Not connected And Not disconnected_so_timeout Then
            If check_server_up(ip_address, port) Then
                If connect() Then
                    Dim timeout_time As Integer = 100
                    Do While Not connected And timeout_time > 0
                        Thread.Sleep(100)
                        timeout_time -= 1
                    Loop

                    If Not connected Then
                        disconnect()
                    End If

                    Thread.Sleep(350)
                Else
                    Thread.Sleep(5000)
                End If
            Else
                Thread.Sleep(5000)
            End If
        Else
            exec()
        End If
    End Sub

    Sub shutdown()
        If connected Then
            disconnect()
        End If

        If to_restart Then
            Try
                program_args = convertstringtoargs(restart_args)
            Catch ex As ThreadAbortException
                Thread.CurrentThread.Abort()
            Catch ex As Exception
            End Try
        End If
    End Sub
End Module
