<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WizardScreen5
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WizardScreen5))
        Me.BtnQuit = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.lstMainLog = New System.Windows.Forms.ListBox()
        Me.lstHeader = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'BtnQuit
        '
        Me.BtnQuit.BackColor = System.Drawing.Color.LightGray
        Me.BtnQuit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnQuit.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnQuit.Location = New System.Drawing.Point(359, 424)
        Me.BtnQuit.Name = "BtnQuit"
        Me.BtnQuit.Size = New System.Drawing.Size(79, 33)
        Me.BtnQuit.TabIndex = 9
        Me.BtnQuit.Text = "E&xit"
        Me.BtnQuit.UseVisualStyleBackColor = False
        '
        'btnStart
        '
        Me.btnStart.BackColor = System.Drawing.Color.LightGray
        Me.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnStart.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStart.Location = New System.Drawing.Point(709, 424)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(79, 33)
        Me.btnStart.TabIndex = 10
        Me.btnStart.Text = "&Start"
        Me.btnStart.UseVisualStyleBackColor = False
        '
        'btnBack
        '
        Me.btnBack.BackColor = System.Drawing.Color.LightGray
        Me.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBack.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBack.Location = New System.Drawing.Point(9, 424)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(79, 33)
        Me.btnBack.TabIndex = 11
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = False
        '
        'lstMainLog
        '
        Me.lstMainLog.BackColor = System.Drawing.Color.LightBlue
        Me.lstMainLog.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstMainLog.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstMainLog.ForeColor = System.Drawing.Color.Black
        Me.lstMainLog.FormattingEnabled = True
        Me.lstMainLog.Location = New System.Drawing.Point(5, 22)
        Me.lstMainLog.Name = "lstMainLog"
        Me.lstMainLog.Size = New System.Drawing.Size(783, 390)
        Me.lstMainLog.TabIndex = 13
        '
        'lstHeader
        '
        Me.lstHeader.BackColor = System.Drawing.Color.LightBlue
        Me.lstHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstHeader.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstHeader.ForeColor = System.Drawing.Color.Black
        Me.lstHeader.FormattingEnabled = True
        Me.lstHeader.Location = New System.Drawing.Point(5, 5)
        Me.lstHeader.Name = "lstHeader"
        Me.lstHeader.Size = New System.Drawing.Size(783, 13)
        Me.lstHeader.TabIndex = 14
        '
        'WizardScreen5
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(794, 458)
        Me.Controls.Add(Me.lstHeader)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.BtnQuit)
        Me.Controls.Add(Me.lstMainLog)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WizardScreen5"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MaNGOSExtractor"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BtnQuit As System.Windows.Forms.Button
    Public WithEvents lstMainLog As System.Windows.Forms.ListBox
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Public WithEvents lstHeader As System.Windows.Forms.ListBox

End Class
