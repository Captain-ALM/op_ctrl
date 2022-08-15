<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(main))
        Me.butopen = New System.Windows.Forms.Button()
        Me.butsave = New System.Windows.Forms.Button()
        Me.butcls = New System.Windows.Forms.Button()
        Me.txtbx = New System.Windows.Forms.TextBox()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'butopen
        '
        Me.butopen.Location = New System.Drawing.Point(13, 13)
        Me.butopen.Name = "butopen"
        Me.butopen.Size = New System.Drawing.Size(75, 23)
        Me.butopen.TabIndex = 0
        Me.butopen.Text = "Open"
        Me.butopen.UseVisualStyleBackColor = True
        '
        'butsave
        '
        Me.butsave.Location = New System.Drawing.Point(94, 13)
        Me.butsave.Name = "butsave"
        Me.butsave.Size = New System.Drawing.Size(75, 23)
        Me.butsave.TabIndex = 1
        Me.butsave.Text = "Save"
        Me.butsave.UseVisualStyleBackColor = True
        '
        'butcls
        '
        Me.butcls.Location = New System.Drawing.Point(175, 13)
        Me.butcls.Name = "butcls"
        Me.butcls.Size = New System.Drawing.Size(75, 23)
        Me.butcls.TabIndex = 2
        Me.butcls.Text = "Clear"
        Me.butcls.UseVisualStyleBackColor = True
        '
        'txtbx
        '
        Me.txtbx.Location = New System.Drawing.Point(13, 43)
        Me.txtbx.MaxLength = 1000000000
        Me.txtbx.Multiline = True
        Me.txtbx.Name = "txtbx"
        Me.txtbx.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtbx.Size = New System.Drawing.Size(559, 306)
        Me.txtbx.TabIndex = 3
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "All Files|*.*"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "All Files|*.*"
        '
        'main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 361)
        Me.Controls.Add(Me.txtbx)
        Me.Controls.Add(Me.butcls)
        Me.Controls.Add(Me.butsave)
        Me.Controls.Add(Me.butopen)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(600, 400)
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "main"
        Me.Text = "File Binary Ser"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents butopen As System.Windows.Forms.Button
    Friend WithEvents butsave As System.Windows.Forms.Button
    Friend WithEvents butcls As System.Windows.Forms.Button
    Friend WithEvents txtbx As System.Windows.Forms.TextBox
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog

End Class
