Imports System.Threading
Imports System.Reflection
Imports System.IO

Public Class main
    Private runtime_thread As Thread = Nothing
    Private running As Boolean = False
    Private started As Boolean = False

    Protected Overrides Sub OnStart(ByVal args() As String)
        running = True
        started = True
        runtime_thread = New Thread(New ThreadStart(AddressOf runtime_sub))
        runtime_thread.IsBackground = True
        runtime_thread.Start()
    End Sub

    Protected Overrides Sub OnStop()
        stop_service()
    End Sub

    Protected Overrides Sub OnShutdown()
        stop_service()
    End Sub

    Private Sub stop_service()
        started = False
        If Not IsNothing(runtime_thread) Then
            If running_c Then
                to_exit = True
            End If
            If runtime_thread.IsAlive Then
                runtime_thread.Abort()
            End If
            running = False
            While running
                Thread.Sleep(100)
            End While
        End If
    End Sub

    Friend Sub runtime_sub()
        While started
            Try
                'DEF
                runner.main()
                If stop_the_service Then
                    started = False
                    End
                End If
            Catch ex As ThreadAbortException
                running = False
                Thread.CurrentThread.Abort()
            Catch ex As Exception
            End Try
            Thread.Sleep(1500)
        End While
        running = False
        Thread.CurrentThread.Abort()
    End Sub
End Class
