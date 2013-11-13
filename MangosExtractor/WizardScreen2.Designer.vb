<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WizardScreen2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WizardScreen2))
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chkExtractWDT = New System.Windows.Forms.CheckBox()
        Me.chkExtractWMO = New System.Windows.Forms.CheckBox()
        Me.chkExtractADT = New System.Windows.Forms.CheckBox()
        Me.chkDBC = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBack
        '
        Me.btnBack.BackColor = System.Drawing.Color.LightGray
        Me.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBack.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.btnBack.Location = New System.Drawing.Point(9, 270)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(79, 32)
        Me.btnBack.TabIndex = 8
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = False
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
        Me.btnNext.TabIndex = 9
        Me.btnNext.Text = "&Next"
        Me.btnNext.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.chkExtractWDT)
        Me.Panel2.Controls.Add(Me.chkExtractWMO)
        Me.Panel2.Controls.Add(Me.chkExtractADT)
        Me.Panel2.Controls.Add(Me.chkDBC)
        Me.Panel2.Location = New System.Drawing.Point(9, 76)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(591, 188)
        Me.Panel2.TabIndex = 20
        '
        'chkExtractWDT
        '
        Me.chkExtractWDT.AutoSize = True
        Me.chkExtractWDT.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractWDT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractWDT.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractWDT.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExtractWDT.Location = New System.Drawing.Point(21, 86)
        Me.chkExtractWDT.Name = "chkExtractWDT"
        Me.chkExtractWDT.Size = New System.Drawing.Size(323, 20)
        Me.chkExtractWDT.TabIndex = 22
        Me.chkExtractWDT.Text = "VMap WDT files (Required for VMAP creation)"
        Me.chkExtractWDT.UseVisualStyleBackColor = False
        '
        'chkExtractWMO
        '
        Me.chkExtractWMO.AutoSize = True
        Me.chkExtractWMO.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractWMO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractWMO.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractWMO.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExtractWMO.Location = New System.Drawing.Point(21, 62)
        Me.chkExtractWMO.Name = "chkExtractWMO"
        Me.chkExtractWMO.Size = New System.Drawing.Size(403, 20)
        Me.chkExtractWMO.TabIndex = 21
        Me.chkExtractWMO.Text = "VMap WMO/MDL files (Required for VMAP/MMAP creation)"
        Me.chkExtractWMO.UseVisualStyleBackColor = False
        '
        'chkExtractADT
        '
        Me.chkExtractADT.AutoSize = True
        Me.chkExtractADT.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractADT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractADT.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractADT.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExtractADT.Location = New System.Drawing.Point(21, 38)
        Me.chkExtractADT.Name = "chkExtractADT"
        Me.chkExtractADT.Size = New System.Drawing.Size(267, 20)
        Me.chkExtractADT.TabIndex = 20
        Me.chkExtractADT.Text = "ADT files (required for Map creation)"
        Me.chkExtractADT.UseVisualStyleBackColor = False
        '
        'chkDBC
        '
        Me.chkDBC.AutoSize = True
        Me.chkDBC.BackColor = System.Drawing.Color.Transparent
        Me.chkDBC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkDBC.Checked = True
        Me.chkDBC.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDBC.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkDBC.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkDBC.Location = New System.Drawing.Point(21, 14)
        Me.chkDBC.Name = "chkDBC"
        Me.chkDBC.Size = New System.Drawing.Size(380, 20)
        Me.chkDBC.TabIndex = 18
        Me.chkDBC.Text = "DBC files (required for Map, VMap and MMap creation)"
        Me.chkDBC.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.Label2.Location = New System.Drawing.Point(12, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 15)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Extraction Options"
        '
        'WizardScreen2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(612, 305)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnBack)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WizardScreen2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MaNGOSExtractor"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents chkDBC As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents chkExtractADT As System.Windows.Forms.CheckBox
    Public WithEvents chkExtractWMO As System.Windows.Forms.CheckBox
    Public WithEvents chkExtractWDT As System.Windows.Forms.CheckBox

End Class
