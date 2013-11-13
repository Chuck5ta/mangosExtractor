<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WizardScreen1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WizardScreen1))
        Me.btnNext = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnQuit = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnSelectBaseFolder = New System.Windows.Forms.Button()
        Me.btnSelectOutputFolder = New System.Windows.Forms.Button()
        Me.txtBaseFolder = New System.Windows.Forms.TextBox()
        Me.txtOutputFolder = New System.Windows.Forms.TextBox()
        Me.MainTitle = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnNext
        '
        Me.btnNext.BackColor = System.Drawing.Color.LightGray
        Me.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.btnNext.Location = New System.Drawing.Point(522, 270)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(79, 32)
        Me.btnNext.TabIndex = 8
        Me.btnNext.Text = "&Next"
        Me.btnNext.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(6, 149)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Wow Root Folder"
        '
        'BtnQuit
        '
        Me.BtnQuit.BackColor = System.Drawing.Color.LightGray
        Me.BtnQuit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnQuit.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.BtnQuit.Location = New System.Drawing.Point(9, 270)
        Me.BtnQuit.Name = "BtnQuit"
        Me.BtnQuit.Size = New System.Drawing.Size(79, 32)
        Me.BtnQuit.TabIndex = 9
        Me.BtnQuit.Text = "E&xit"
        Me.BtnQuit.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.Label3.Location = New System.Drawing.Point(6, 199)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 15)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Output Folder"
        '
        'btnSelectBaseFolder
        '
        Me.btnSelectBaseFolder.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectBaseFolder.Location = New System.Drawing.Point(575, 162)
        Me.btnSelectBaseFolder.Margin = New System.Windows.Forms.Padding(0)
        Me.btnSelectBaseFolder.Name = "btnSelectBaseFolder"
        Me.btnSelectBaseFolder.Size = New System.Drawing.Size(26, 26)
        Me.btnSelectBaseFolder.TabIndex = 2
        Me.btnSelectBaseFolder.Text = "…"
        Me.btnSelectBaseFolder.UseVisualStyleBackColor = True
        '
        'btnSelectOutputFolder
        '
        Me.btnSelectOutputFolder.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectOutputFolder.Location = New System.Drawing.Point(575, 212)
        Me.btnSelectOutputFolder.Margin = New System.Windows.Forms.Padding(0)
        Me.btnSelectOutputFolder.Name = "btnSelectOutputFolder"
        Me.btnSelectOutputFolder.Size = New System.Drawing.Size(26, 26)
        Me.btnSelectOutputFolder.TabIndex = 4
        Me.btnSelectOutputFolder.Text = "…"
        Me.btnSelectOutputFolder.UseVisualStyleBackColor = True
        '
        'txtBaseFolder
        '
        Me.txtBaseFolder.BackColor = System.Drawing.Color.LightGray
        Me.txtBaseFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBaseFolder.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.txtBaseFolder.Location = New System.Drawing.Point(9, 165)
        Me.txtBaseFolder.Name = "txtBaseFolder"
        Me.txtBaseFolder.Size = New System.Drawing.Size(560, 23)
        Me.txtBaseFolder.TabIndex = 1
        Me.txtBaseFolder.Text = "W:\World of Warcraft"
        '
        'txtOutputFolder
        '
        Me.txtOutputFolder.BackColor = System.Drawing.Color.LightGray
        Me.txtOutputFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtOutputFolder.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.txtOutputFolder.Location = New System.Drawing.Point(9, 215)
        Me.txtOutputFolder.Name = "txtOutputFolder"
        Me.txtOutputFolder.Size = New System.Drawing.Size(560, 23)
        Me.txtOutputFolder.TabIndex = 3
        Me.txtOutputFolder.Text = "W:\World of Warcraft\Extracted"
        '
        'MainTitle
        '
        Me.MainTitle.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainTitle.Location = New System.Drawing.Point(9, 9)
        Me.MainTitle.Name = "MainTitle"
        Me.MainTitle.Size = New System.Drawing.Size(589, 140)
        Me.MainTitle.TabIndex = 10
        Me.MainTitle.Text = "Welcome to MaNGOS Extractor, a multi-purpose tool for the Mangos Family of Emulat" & _
    "ors"
        '
        'WizardScreen1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(612, 305)
        Me.Controls.Add(Me.MainTitle)
        Me.Controls.Add(Me.txtOutputFolder)
        Me.Controls.Add(Me.txtBaseFolder)
        Me.Controls.Add(Me.btnSelectOutputFolder)
        Me.Controls.Add(Me.btnSelectBaseFolder)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnQuit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnNext)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WizardScreen1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MaNGOSExtractor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnQuit As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnSelectBaseFolder As System.Windows.Forms.Button
    Friend WithEvents btnSelectOutputFolder As System.Windows.Forms.Button
    Public WithEvents txtBaseFolder As System.Windows.Forms.TextBox
    Public WithEvents txtOutputFolder As System.Windows.Forms.TextBox
    Friend WithEvents MainTitle As System.Windows.Forms.Label

End Class
