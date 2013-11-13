Public Class WizardScreen1
    Private Sub BtnStartDbcClick(sender As Object, e As EventArgs) Handles btnNext.Click
        WizardScreen2.Show()
        Hide()
    End Sub


    ''' <summary>
    ''' Exits the Application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnQuit_Click(sender As Object, e As EventArgs) Handles BtnQuit.Click
        End
    End Sub

    Private Sub BtnSelectBaseFolderClick(sender As Object, e As EventArgs) Handles btnSelectBaseFolder.Click
        FolderBrowserDialog1.SelectedPath = txtBaseFolder.Text
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            txtBaseFolder.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub BtnSelectOutputFolderClick(sender As Object, e As EventArgs) Handles btnSelectOutputFolder.Click
        FolderBrowserDialog1.SelectedPath = txtOutputFolder.Text
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            txtOutputFolder.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    ''' <summary>
    ''' Handles the Load event of the WizardScreen1 control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    ''' <returns></returns>
    Private Sub WizardScreen1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Text = Text & " " & Core.MaNgosExtractorVersion & " - File location Selection"
        MainTitle.Text = "Welcome to MaNGOS Extractor - A multi-purpose extraction tool for the MaNGOS family of emulators"
        MainTitle.Text = MainTitle.Text & vbCrLf & ""
        MainTitle.Text = MainTitle.Text & vbCrLf & "Latest Changes v1.47"
        MainTitle.Text = MainTitle.Text & vbCrLf & "=================="
        MainTitle.Text = MainTitle.Text & vbCrLf & " * DBC Data detection & Extraction routines rewritten"
        MainTitle.Text = MainTitle.Text & vbCrLf & " * Now uses a wizard system to set settings"
        MainTitle.Text = MainTitle.Text & vbCrLf & " * Many other internal fixes"
    End Sub
End Class