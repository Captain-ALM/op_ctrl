'system,system.core,system.data,System.Data.DataSetExtensions,System.Windows.Forms,System.Xml,System.Xml.Linq,microsoft.visualbasic
Imports System.Windows.Forms

Public Class main
    Function msgbx(args As Object) As Object
        If Not IsNothing(args) Then
            Try
                Dim arrargs As Array = args
                If arrargs.Length = 1 Then
                    MessageBox.Show(arrargs(0), "msgbx dll", MessageBoxButtons.OK)
                ElseIf arrargs.Length >= 2 Then
                    MessageBox.Show(arrargs(0), arrargs(1), MessageBoxButtons.OK)
                Else
                    MessageBox.Show("Hello World", "msgbx dll", MessageBoxButtons.OK)
                End If
            Catch ex As Exception
                MessageBox.Show("Hello World", "msgbx dll", MessageBoxButtons.OK)
            End Try
        Else
            MessageBox.Show("Hello World", "msgbx dll", MessageBoxButtons.OK)
        End If
        Return ""
    End Function
End Class
