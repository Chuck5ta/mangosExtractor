
Public Class WizardScreen2
    Private Sub BtnStartDbcClick(sender As Object, e As EventArgs) Handles btnBack.Click
        WizardScreen1.Show()
        Hide()
    End Sub

    Private Sub BtnQuit_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        WizardScreen3.Show()
        Hide()
    End Sub

    Private Sub WizardScreen2_Load(sender As Object, e As EventArgs) Handles Me.Load
        Text = Text & " " & Core.MaNgosExtractorVersion & " - File Extraction Options"
    End Sub
End Class