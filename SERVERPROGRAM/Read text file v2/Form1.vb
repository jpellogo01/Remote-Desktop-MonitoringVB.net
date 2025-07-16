Imports System.IO

Public Class Form1
    Public IP As String '??
    Public ID As Integer '??
    Public IsShown As Boolean = False

    Public Online As Boolean
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' PictureBox1.Dock = DockStyle.Fill
        PictureBox1.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Images", "disconnected.jpg"))
    End Sub

    Private Sub PictureBox1_Click_1(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class