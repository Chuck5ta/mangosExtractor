
Public Class WizardScreen3
    Private Sub BtnStartDbcClick(sender As Object, e As EventArgs) Handles btnBack.Click
        WizardScreen2.Show()
        Hide()
    End Sub

                          
    Private Sub BtnQuit_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        'Dim tform As New WizardScreen5
        WizardScreen5.Show()
        WizardScreen5.lstMainLog.Items.Clear()
        WizardScreen5.LoadSummary()
        Hide()
    End Sub

    Private Sub WizardScreen3_Load(sender As Object, e As EventArgs) Handles Me.Load
        Text = Text & " " & Core.MaNgosExtractorVersion & " - File Export selection"
    End Sub
End Class