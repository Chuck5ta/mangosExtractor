<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WizardScreen3
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WizardScreen3))
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkExportH = New System.Windows.Forms.CheckBox()
        Me.chkExportMD = New System.Windows.Forms.CheckBox()
        Me.chkExportXML = New System.Windows.Forms.CheckBox()
        Me.chkCSV = New System.Windows.Forms.CheckBox()
        Me.chkSQL = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chkExtractMMAPS = New System.Windows.Forms.CheckBox()
        Me.chkExtractVmap = New System.Windows.Forms.CheckBox()
        Me.chkExtractMaps = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkXMLDefs = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
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
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.chkXMLDefs)
        Me.Panel1.Controls.Add(Me.chkExportH)
        Me.Panel1.Controls.Add(Me.chkExportMD)
        Me.Panel1.Controls.Add(Me.chkExportXML)
        Me.Panel1.Controls.Add(Me.chkCSV)
        Me.Panel1.Controls.Add(Me.chkSQL)
        Me.Panel1.Location = New System.Drawing.Point(9, 76)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(290, 188)
        Me.Panel1.TabIndex = 18
        '
        'chkExportH
        '
        Me.chkExportH.AutoSize = True
        Me.chkExportH.BackColor = System.Drawing.Color.Transparent
        Me.chkExportH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExportH.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExportH.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExportH.Location = New System.Drawing.Point(21, 110)
        Me.chkExportH.Name = "chkExportH"
        Me.chkExportH.Size = New System.Drawing.Size(227, 20)
        Me.chkExportH.TabIndex = 25
        Me.chkExportH.Text = "Create .H stubs from DBC files"
        Me.chkExportH.UseVisualStyleBackColor = False
        '
        'chkExportMD
        '
        Me.chkExportMD.AutoSize = True
        Me.chkExportMD.BackColor = System.Drawing.Color.Transparent
        Me.chkExportMD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExportMD.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExportMD.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExportMD.Location = New System.Drawing.Point(21, 86)
        Me.chkExportMD.Name = "chkExportMD"
        Me.chkExportMD.Size = New System.Drawing.Size(274, 20)
        Me.chkExportMD.TabIndex = 20
        Me.chkExportMD.Text = "Create MarkDown files from DBC files"
        Me.chkExportMD.UseVisualStyleBackColor = False
        '
        'chkExportXML
        '
        Me.chkExportXML.AutoSize = True
        Me.chkExportXML.BackColor = System.Drawing.Color.Transparent
        Me.chkExportXML.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExportXML.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExportXML.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExportXML.Location = New System.Drawing.Point(21, 62)
        Me.chkExportXML.Name = "chkExportXML"
        Me.chkExportXML.Size = New System.Drawing.Size(258, 20)
        Me.chkExportXML.TabIndex = 19
        Me.chkExportXML.Text = "Save XML column defs for DBC files"
        Me.chkExportXML.UseVisualStyleBackColor = False
        '
        'chkCSV
        '
        Me.chkCSV.AutoSize = True
        Me.chkCSV.BackColor = System.Drawing.Color.Transparent
        Me.chkCSV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkCSV.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkCSV.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkCSV.Location = New System.Drawing.Point(21, 14)
        Me.chkCSV.Name = "chkCSV"
        Me.chkCSV.Size = New System.Drawing.Size(233, 20)
        Me.chkCSV.TabIndex = 18
        Me.chkCSV.Text = "Create CSV files from DBC files"
        Me.chkCSV.UseVisualStyleBackColor = False
        '
        'chkSQL
        '
        Me.chkSQL.AutoSize = True
        Me.chkSQL.BackColor = System.Drawing.Color.Transparent
        Me.chkSQL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkSQL.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkSQL.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkSQL.Location = New System.Drawing.Point(21, 38)
        Me.chkSQL.Name = "chkSQL"
        Me.chkSQL.Size = New System.Drawing.Size(232, 20)
        Me.chkSQL.TabIndex = 17
        Me.chkSQL.Text = "Create SQL files from DBC files"
        Me.chkSQL.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.Label4.Location = New System.Drawing.Point(12, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(103, 15)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Export Options"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.chkExtractMMAPS)
        Me.Panel2.Controls.Add(Me.chkExtractVmap)
        Me.Panel2.Controls.Add(Me.chkExtractMaps)
        Me.Panel2.Location = New System.Drawing.Point(305, 76)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(295, 188)
        Me.Panel2.TabIndex = 23
        '
        'chkExtractMMAPS
        '
        Me.chkExtractMMAPS.AutoSize = True
        Me.chkExtractMMAPS.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractMMAPS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractMMAPS.Enabled = False
        Me.chkExtractMMAPS.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractMMAPS.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExtractMMAPS.Location = New System.Drawing.Point(21, 60)
        Me.chkExtractMMAPS.Name = "chkExtractMMAPS"
        Me.chkExtractMMAPS.Size = New System.Drawing.Size(266, 20)
        Me.chkExtractMMAPS.TabIndex = 27
        Me.chkExtractMMAPS.Text = "Create .MMap (Movement Map) Files"
        Me.chkExtractMMAPS.UseVisualStyleBackColor = False
        '
        'chkExtractVmap
        '
        Me.chkExtractVmap.AutoSize = True
        Me.chkExtractVmap.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractVmap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractVmap.Enabled = False
        Me.chkExtractVmap.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractVmap.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExtractVmap.Location = New System.Drawing.Point(21, 36)
        Me.chkExtractVmap.Name = "chkExtractVmap"
        Me.chkExtractVmap.Size = New System.Drawing.Size(240, 20)
        Me.chkExtractVmap.TabIndex = 26
        Me.chkExtractVmap.Text = "Create .VMap (Vertex Map) Files"
        Me.chkExtractVmap.UseVisualStyleBackColor = False
        '
        'chkExtractMaps
        '
        Me.chkExtractMaps.AutoSize = True
        Me.chkExtractMaps.BackColor = System.Drawing.Color.Transparent
        Me.chkExtractMaps.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkExtractMaps.Enabled = False
        Me.chkExtractMaps.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkExtractMaps.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkExtractMaps.Location = New System.Drawing.Point(21, 14)
        Me.chkExtractMaps.Margin = New System.Windows.Forms.Padding(1)
        Me.chkExtractMaps.Name = "chkExtractMaps"
        Me.chkExtractMaps.Size = New System.Drawing.Size(236, 20)
        Me.chkExtractMaps.TabIndex = 25
        Me.chkExtractMaps.Text = "Create .Map (Terrain Map) Files"
        Me.chkExtractMaps.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(307, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 15)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Creation Options"
        '
        'chkXMLDefs
        '
        Me.chkXMLDefs.AutoSize = True
        Me.chkXMLDefs.BackColor = System.Drawing.Color.Transparent
        Me.chkXMLDefs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.chkXMLDefs.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkXMLDefs.Font = New System.Drawing.Font("Arial Rounded MT Bold", 9.75!)
        Me.chkXMLDefs.Location = New System.Drawing.Point(21, 136)
        Me.chkXMLDefs.Name = "chkXMLDefs"
        Me.chkXMLDefs.Size = New System.Drawing.Size(228, 20)
        Me.chkXMLDefs.TabIndex = 26
        Me.chkXMLDefs.Text = "Save Default XML column defs "
        Me.chkXMLDefs.UseVisualStyleBackColor = False
        '
        'WizardScreen3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(612, 305)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnBack)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WizardScreen3"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MaNGOSExtractor"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents chkExportXML As System.Windows.Forms.CheckBox
    Public WithEvents chkCSV As System.Windows.Forms.CheckBox
    Public WithEvents chkSQL As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents chkExportMD As System.Windows.Forms.CheckBox
    Public WithEvents chkExportH As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents chkExtractVmap As System.Windows.Forms.CheckBox
    Public WithEvents chkExtractMaps As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents chkExtractMMAPS As System.Windows.Forms.CheckBox
    Public WithEvents chkXMLDefs As System.Windows.Forms.CheckBox

End Class
