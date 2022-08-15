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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.butreset = New System.Windows.Forms.Button()
        Me.butexit = New System.Windows.Forms.Button()
        Me.lblstats = New System.Windows.Forms.Label()
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel()
        Me.ProgressBarstats = New System.Windows.Forms.ProgressBar()
        Me.chkbxst = New System.Windows.Forms.CheckBox()
        Me.titlabel = New System.Windows.Forms.Label()
        Me.SplitContainerPanel = New System.Windows.Forms.SplitContainer()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me.butrfs = New System.Windows.Forms.Button()
        Me.butdiscon = New System.Windows.Forms.Button()
        Me.butsenddllmsg = New System.Windows.Forms.Button()
        Me.butsendintmsg = New System.Windows.Forms.Button()
        Me.butadddll = New System.Windows.Forms.Button()
        Me.ListViewClients = New System.Windows.Forms.ListView()
        Me.ColumnHeaderClientName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderUserName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderIP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderisadmin = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderSID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.butrsndmsg = New System.Windows.Forms.Button()
        Me.butmsgcls = New System.Windows.Forms.Button()
        Me.ListViewMessages = New System.Windows.Forms.ListView()
        Me.ColumnHeaderRefNumber = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderServerMessage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderClientMessage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeaderClientUUID = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.butopenmsg = New System.Windows.Forms.Button()
        Me.butabout = New System.Windows.Forms.Button()
        Me.OpenDLLDialog = New System.Windows.Forms.OpenFileDialog()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel6.SuspendLayout()
        CType(Me.SplitContainerPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerPanel.Panel1.SuspendLayout()
        Me.SplitContainerPanel.Panel2.SuspendLayout()
        Me.SplitContainerPanel.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.titlabel, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.SplitContainerPanel, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.butabout, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(784, 561)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel1.SetColumnSpan(Me.TableLayoutPanel2, 2)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.butreset, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.butexit, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblstats, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel6, 1, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 451)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(778, 107)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'butreset
        '
        Me.butreset.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butreset.Location = New System.Drawing.Point(3, 3)
        Me.butreset.Name = "butreset"
        Me.butreset.Size = New System.Drawing.Size(383, 68)
        Me.butreset.TabIndex = 0
        Me.butreset.Text = "Reset Server"
        Me.butreset.UseVisualStyleBackColor = True
        '
        'butexit
        '
        Me.butexit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butexit.Location = New System.Drawing.Point(392, 3)
        Me.butexit.Name = "butexit"
        Me.butexit.Size = New System.Drawing.Size(383, 68)
        Me.butexit.TabIndex = 1
        Me.butexit.Text = "Close Server"
        Me.butexit.UseVisualStyleBackColor = True
        '
        'lblstats
        '
        Me.lblstats.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblstats.Location = New System.Drawing.Point(3, 74)
        Me.lblstats.Name = "lblstats"
        Me.lblstats.Size = New System.Drawing.Size(383, 33)
        Me.lblstats.TabIndex = 2
        Me.lblstats.Text = "Ready."
        Me.lblstats.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.ColumnCount = 2
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel6.Controls.Add(Me.ProgressBarstats, 0, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.chkbxst, 1, 0)
        Me.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(392, 77)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 1
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(383, 27)
        Me.TableLayoutPanel6.TabIndex = 3
        '
        'ProgressBarstats
        '
        Me.ProgressBarstats.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressBarstats.Location = New System.Drawing.Point(3, 3)
        Me.ProgressBarstats.Name = "ProgressBarstats"
        Me.ProgressBarstats.Size = New System.Drawing.Size(185, 21)
        Me.ProgressBarstats.TabIndex = 4
        '
        'chkbxst
        '
        Me.chkbxst.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkbxst.Location = New System.Drawing.Point(194, 3)
        Me.chkbxst.Name = "chkbxst"
        Me.chkbxst.Size = New System.Drawing.Size(186, 21)
        Me.chkbxst.TabIndex = 5
        Me.chkbxst.Text = "Multi-Threaded Send"
        Me.chkbxst.UseVisualStyleBackColor = True
        '
        'titlabel
        '
        Me.titlabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.titlabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.titlabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.titlabel.Location = New System.Drawing.Point(3, 0)
        Me.titlabel.Name = "titlabel"
        Me.titlabel.Size = New System.Drawing.Size(699, 56)
        Me.titlabel.TabIndex = 2
        Me.titlabel.Text = "Operation Control Sever - HCALM"
        Me.titlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SplitContainerPanel
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.SplitContainerPanel, 2)
        Me.SplitContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerPanel.Location = New System.Drawing.Point(3, 59)
        Me.SplitContainerPanel.Name = "SplitContainerPanel"
        '
        'SplitContainerPanel.Panel1
        '
        Me.SplitContainerPanel.Panel1.Controls.Add(Me.GroupBox2)
        '
        'SplitContainerPanel.Panel2
        '
        Me.SplitContainerPanel.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainerPanel.Size = New System.Drawing.Size(778, 386)
        Me.SplitContainerPanel.SplitterDistance = 394
        Me.SplitContainerPanel.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TableLayoutPanel4)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(394, 386)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Client Management"
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.TableLayoutPanel5, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.ListViewClients, 0, 1)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 2
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(388, 367)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 5
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.butrfs, 4, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.butdiscon, 3, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.butsenddllmsg, 2, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.butsendintmsg, 1, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.butadddll, 0, 0)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 1
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(382, 67)
        Me.TableLayoutPanel5.TabIndex = 0
        '
        'butrfs
        '
        Me.butrfs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butrfs.Location = New System.Drawing.Point(307, 3)
        Me.butrfs.Name = "butrfs"
        Me.butrfs.Size = New System.Drawing.Size(72, 61)
        Me.butrfs.TabIndex = 4
        Me.butrfs.Text = "Refresh Clients"
        Me.butrfs.UseVisualStyleBackColor = True
        '
        'butdiscon
        '
        Me.butdiscon.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butdiscon.Location = New System.Drawing.Point(231, 3)
        Me.butdiscon.Name = "butdiscon"
        Me.butdiscon.Size = New System.Drawing.Size(70, 61)
        Me.butdiscon.TabIndex = 3
        Me.butdiscon.Text = "Disconnect Client(s)"
        Me.butdiscon.UseVisualStyleBackColor = True
        '
        'butsenddllmsg
        '
        Me.butsenddllmsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butsenddllmsg.Location = New System.Drawing.Point(155, 3)
        Me.butsenddllmsg.Name = "butsenddllmsg"
        Me.butsenddllmsg.Size = New System.Drawing.Size(70, 61)
        Me.butsenddllmsg.TabIndex = 2
        Me.butsenddllmsg.Text = "Send DLL Message"
        Me.butsenddllmsg.UseVisualStyleBackColor = True
        '
        'butsendintmsg
        '
        Me.butsendintmsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butsendintmsg.Location = New System.Drawing.Point(79, 3)
        Me.butsendintmsg.Name = "butsendintmsg"
        Me.butsendintmsg.Size = New System.Drawing.Size(70, 61)
        Me.butsendintmsg.TabIndex = 1
        Me.butsendintmsg.Text = "Send Internal Message"
        Me.butsendintmsg.UseVisualStyleBackColor = True
        '
        'butadddll
        '
        Me.butadddll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butadddll.Location = New System.Drawing.Point(3, 3)
        Me.butadddll.Name = "butadddll"
        Me.butadddll.Size = New System.Drawing.Size(70, 61)
        Me.butadddll.TabIndex = 0
        Me.butadddll.Text = "Add DLL"
        Me.butadddll.UseVisualStyleBackColor = True
        '
        'ListViewClients
        '
        Me.ListViewClients.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeaderClientName, Me.ColumnHeaderUserName, Me.ColumnHeaderIP, Me.ColumnHeaderisadmin, Me.ColumnHeaderSID})
        Me.ListViewClients.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewClients.FullRowSelect = True
        Me.ListViewClients.Location = New System.Drawing.Point(3, 76)
        Me.ListViewClients.Name = "ListViewClients"
        Me.ListViewClients.Size = New System.Drawing.Size(382, 288)
        Me.ListViewClients.TabIndex = 1
        Me.ListViewClients.UseCompatibleStateImageBehavior = False
        Me.ListViewClients.View = System.Windows.Forms.View.Details
        '
        'ColumnHeaderClientName
        '
        Me.ColumnHeaderClientName.Text = "Client Name"
        Me.ColumnHeaderClientName.Width = 93
        '
        'ColumnHeaderUserName
        '
        Me.ColumnHeaderUserName.Text = "User Name"
        Me.ColumnHeaderUserName.Width = 86
        '
        'ColumnHeaderIP
        '
        Me.ColumnHeaderIP.Text = "IP"
        Me.ColumnHeaderIP.Width = 79
        '
        'ColumnHeaderisadmin
        '
        Me.ColumnHeaderisadmin.Text = "Admin?"
        Me.ColumnHeaderisadmin.Width = 51
        '
        'ColumnHeaderSID
        '
        Me.ColumnHeaderSID.Text = "Security ID"
        Me.ColumnHeaderSID.Width = 69
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel3)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(380, 386)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Messages"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 3
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel3.Controls.Add(Me.butrsndmsg, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.butmsgcls, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.ListViewMessages, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.butopenmsg, 0, 1)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(374, 367)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'butrsndmsg
        '
        Me.butrsndmsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butrsndmsg.Location = New System.Drawing.Point(127, 314)
        Me.butrsndmsg.Name = "butrsndmsg"
        Me.butrsndmsg.Size = New System.Drawing.Size(118, 50)
        Me.butrsndmsg.TabIndex = 5
        Me.butrsndmsg.Text = "Re-Send Message"
        Me.butrsndmsg.UseVisualStyleBackColor = True
        '
        'butmsgcls
        '
        Me.butmsgcls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butmsgcls.Location = New System.Drawing.Point(251, 314)
        Me.butmsgcls.Name = "butmsgcls"
        Me.butmsgcls.Size = New System.Drawing.Size(120, 50)
        Me.butmsgcls.TabIndex = 4
        Me.butmsgcls.Text = "Clear Messages"
        Me.butmsgcls.UseVisualStyleBackColor = True
        '
        'ListViewMessages
        '
        Me.ListViewMessages.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeaderRefNumber, Me.ColumnHeaderServerMessage, Me.ColumnHeaderClientMessage, Me.ColumnHeaderClientUUID})
        Me.TableLayoutPanel3.SetColumnSpan(Me.ListViewMessages, 3)
        Me.ListViewMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewMessages.FullRowSelect = True
        Me.ListViewMessages.Location = New System.Drawing.Point(3, 3)
        Me.ListViewMessages.MultiSelect = False
        Me.ListViewMessages.Name = "ListViewMessages"
        Me.ListViewMessages.Size = New System.Drawing.Size(368, 305)
        Me.ListViewMessages.TabIndex = 1
        Me.ListViewMessages.UseCompatibleStateImageBehavior = False
        Me.ListViewMessages.View = System.Windows.Forms.View.Details
        '
        'ColumnHeaderRefNumber
        '
        Me.ColumnHeaderRefNumber.Text = "Reference Number"
        Me.ColumnHeaderRefNumber.Width = 109
        '
        'ColumnHeaderServerMessage
        '
        Me.ColumnHeaderServerMessage.Text = "Server Message"
        Me.ColumnHeaderServerMessage.Width = 96
        '
        'ColumnHeaderClientMessage
        '
        Me.ColumnHeaderClientMessage.Text = "Client Message"
        Me.ColumnHeaderClientMessage.Width = 91
        '
        'ColumnHeaderClientUUID
        '
        Me.ColumnHeaderClientUUID.Text = "Client UUID"
        Me.ColumnHeaderClientUUID.Width = 70
        '
        'butopenmsg
        '
        Me.butopenmsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butopenmsg.Location = New System.Drawing.Point(3, 314)
        Me.butopenmsg.Name = "butopenmsg"
        Me.butopenmsg.Size = New System.Drawing.Size(118, 50)
        Me.butopenmsg.TabIndex = 2
        Me.butopenmsg.Text = "Open Message"
        Me.butopenmsg.UseVisualStyleBackColor = True
        '
        'butabout
        '
        Me.butabout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butabout.Location = New System.Drawing.Point(708, 3)
        Me.butabout.Name = "butabout"
        Me.butabout.Size = New System.Drawing.Size(73, 50)
        Me.butabout.TabIndex = 4
        Me.butabout.Text = "About"
        Me.butabout.UseVisualStyleBackColor = True
        '
        'OpenDLLDialog
        '
        Me.OpenDLLDialog.Filter = "Loadable Files|*.dll;*.vb;*.cs|Source Code Files|*.vb;*.cs|VB Source Files|*.vb|C" & _
    "# Source Files|*.cs|Dynamic Link Libraries|*.dll|All Files|*.*"
        Me.OpenDLLDialog.Multiselect = True
        Me.OpenDLLDialog.Title = "Choose DLL/Source Code File to load:"
        '
        'main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "main"
        Me.Text = "Operation Control Sever"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel6.ResumeLayout(False)
        Me.SplitContainerPanel.Panel1.ResumeLayout(False)
        Me.SplitContainerPanel.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerPanel.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents titlabel As System.Windows.Forms.Label
    Friend WithEvents butreset As System.Windows.Forms.Button
    Friend WithEvents butexit As System.Windows.Forms.Button
    Friend WithEvents SplitContainerPanel As System.Windows.Forms.SplitContainer
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ListViewClients As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeaderUserName As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderIP As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderisadmin As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderSID As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderClientName As System.Windows.Forms.ColumnHeader
    Friend WithEvents ListViewMessages As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeaderRefNumber As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderServerMessage As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderClientMessage As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeaderClientUUID As System.Windows.Forms.ColumnHeader
    Friend WithEvents butrfs As System.Windows.Forms.Button
    Friend WithEvents butdiscon As System.Windows.Forms.Button
    Friend WithEvents butsenddllmsg As System.Windows.Forms.Button
    Friend WithEvents butsendintmsg As System.Windows.Forms.Button
    Friend WithEvents butadddll As System.Windows.Forms.Button
    Friend WithEvents butopenmsg As System.Windows.Forms.Button
    Friend WithEvents OpenDLLDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblstats As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel6 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ProgressBarstats As System.Windows.Forms.ProgressBar
    Friend WithEvents chkbxst As System.Windows.Forms.CheckBox
    Friend WithEvents butmsgcls As System.Windows.Forms.Button
    Friend WithEvents butrsndmsg As System.Windows.Forms.Button
    Friend WithEvents butabout As System.Windows.Forms.Button

End Class
