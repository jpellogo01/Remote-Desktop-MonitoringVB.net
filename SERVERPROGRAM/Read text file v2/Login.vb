Imports System.Text.RegularExpressions

Public Class Login

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Panel1.BackColor = Color.FromArgb(10, Color.AntiqueWhite) ' 50% transparent black
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        ' Check if username and password fields are empty
        If String.IsNullOrWhiteSpace(txtUsername.Text) Or String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Please fill in both username and password!")
        End If  'ENDS HERE

        ' REGEX STARTS HERE
        Dim pattern As String = "^[a-zA-Z0-9]*$"
            Dim usernameRegex As New Regex(pattern)
            Dim passwordRegex As New Regex(pattern)

        If Not usernameRegex.IsMatch(txtUsername.Text) Then
            MessageBox.Show("Username contains invalid characters. Only alphanumeric and numbers characters are allowed!")
            Return

        ElseIf Not passwordRegex.IsMatch(txtPassword.Text) Then
            MessageBox.Show("Password contains invalid characters. Only alphanumeric and numbers characters are allowed!")
            Return

        End If ' REGEX STARTS HERE


        If txtUsername.Text = "user" And txtPassword.Text = "pass" Then
            MDIParent1.Show()

        ElseIf txtUsername.Text <> "user" Then
            MessageBox.Show("Incorrect username!")

        ElseIf txtPassword.Text <> "pass" Then
            MessageBox.Show("Incorrect password!")

        Else
            MessageBox.Show("username and password are incorrect!")

        End If
    End Sub


End Class