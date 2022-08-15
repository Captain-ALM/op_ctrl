Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

Public Class main
    Dim convertr As Boolean = False
    Dim sfile As String = ""
    Dim dfile As String = ""
    Dim toser As Boolean = False

    Private Sub butcls_Click(sender As Object, e As EventArgs) Handles butcls.Click
        txtbx.Clear()
    End Sub

    Private Sub butopen_Click(sender As Object, e As EventArgs) Handles butopen.Click
        Try
            Dim chk As DialogResult = OpenFileDialog1.ShowDialog()
            If chk = Windows.Forms.DialogResult.OK Then
                Dim bytes As Byte() = File.ReadAllBytes(OpenFileDialog1.FileName)
                OpenFileDialog1.FileName = ""
                txtbx.Text = convertobjecttostring(bytes)
            End If
        Catch ex As Exception
            MsgBox("Open File Error: " & ex.GetType.ToString & " ; " & ex.Message & " .", MsgBoxStyle.Exclamation, "Binary Ser!")
        End Try
    End Sub

    Public Function convertobjecttostring(obj As Object) As String
        Try
            Dim memorysteam As New MemoryStream
            Dim formatter As New BinaryFormatter()
            formatter.Serialize(memorysteam, obj)
            Dim toreturn As String = convert.ToBase64String(memorysteam.ToArray)
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

    Private Sub butsave_Click(sender As Object, e As EventArgs) Handles butsave.Click
        Try
            Dim chk As DialogResult = SaveFileDialog1.ShowDialog()
            If chk = Windows.Forms.DialogResult.OK Then
                Dim bytes As Byte() = convertstringtoobject(txtbx.Text)
                File.WriteAllBytes(SaveFileDialog1.FileName, bytes)
                SaveFileDialog1.FileName = ""
            End If
        Catch ex As Exception
            MsgBox("Save File Error: " & ex.GetType.ToString & " ; " & ex.Message & " .", MsgBoxStyle.Exclamation, "Binary Ser!")
        End Try
    End Sub

    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Environment.GetCommandLineArgs.Length = 4 Then
            Try
                butcls.Enabled = False
                butopen.Enabled = False
                butsave.Enabled = False
                txtbx.ReadOnly = True
                txtbx.Text = "Converting File..."
                sfile = Environment.GetCommandLineArgs(1)
                dfile = Environment.GetCommandLineArgs(2)
                toser = Environment.GetCommandLineArgs(3)
                convertr = True
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub main_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If convertr Then
            Try
            If toser Then
                Dim bytes As Byte() = File.ReadAllBytes(sfile)
                File.WriteAllText(dfile, convertobjecttostring(bytes))
            Else
                Dim bytes As Byte() = convertstringtoobject(File.ReadAllText(sfile))
                File.WriteAllBytes(dfile, bytes)
                End If
            Catch ex As Exception
            End Try
            Me.Close()
        End If
    End Sub
End Class
