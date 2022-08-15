'System.dll|System.Core.dll|System.Data.dll|System.Data.DataSetExtensions.dll|System.Windows.Forms.dll|System.Xml.dll|System.Xml.Linq.dll|microsoft.visualbasic.dll
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class main
    Function msgbx(args As Object) As Object
        Dim shown As Boolean = False
        If Not Microsoft.VisualBasic.Information.IsNothing(args) Then
            Try
                Dim arrargs As System.Array = args
                If arrargs.Length >= 2 Then
                    Microsoft.VisualBasic.Interaction.MsgBox(arrargs(0), MessageBoxButtons.OK, arrargs(1))
                    shown = True
                Else
                    Throw New System.InvalidOperationException("Not Enough Params")
                End If
            Catch ex As System.Exception
                Microsoft.VisualBasic.Interaction.MsgBox("", MessageBoxButtons.OK, "")
            End Try
        Else
            Microsoft.VisualBasic.Interaction.MsgBox("Hello World", MessageBoxButtons.OK, "msgbx dll : " & version(Nothing).ToString)
            shown = True
        End If
        If shown Then
            Return "SHOWN"
        End If
        Return "FAIL"
    End Function

    Function prmpt(args As Object) As Object
        Dim shown As Boolean = False
        Dim ret As String = ""
        If Not Microsoft.VisualBasic.Information.IsNothing(args) Then
            Try
                Dim arrargs As System.Array = args
                If arrargs.Length = 2 Then
                    ret = Microsoft.VisualBasic.Interaction.InputBox(arrargs(0), arrargs(1), "")
                    shown = True
                ElseIf arrargs.Length >= 3 Then
                    ret = Microsoft.VisualBasic.Interaction.InputBox(arrargs(0), arrargs(1), arrargs(2))
                    shown = True
                Else
                    Throw New System.InvalidOperationException("Not Enough Params")
                End If
            Catch ex As System.Exception
                Microsoft.VisualBasic.Interaction.MsgBox("", MessageBoxButtons.OK, "")
            End Try
        Else
            Microsoft.VisualBasic.Interaction.MsgBox("Hello World", MessageBoxButtons.OK, "msgbx dll : " & version(Nothing).ToString)
            ret = "msgbx dll : " & version(Nothing).ToString
            shown = True
        End If
        If shown Then
            Return "SHOWN : " & ret
        End If
        Return "FAIL"
    End Function

    Function version(args As Object) As Object
        Return Me.GetType.Assembly.GetName.Version.ToString
    End Function

    Function functionlist(args As Object) As Object
        Return "main.msgbx" & ControlChars.CrLf & "main.prmpt" & ControlChars.CrLf & "main.version" & ControlChars.CrLf & "main.functionlist"
    End Function
End Class
