Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MSTSCLib
Imports System.Net.Dns
Imports System.Runtime.InteropServices
Public Class DesktopControl
    Public Sub New(ByVal MDIParent1 As MDIParent1)
        InitializeComponent()
        Me.Owner = MDIParent1
        ResetScreen()
    End Sub

    Sub ResetScreen() 'resets when first open and when selected ip is changed
        DisableControl()
        ViewDesktop.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Images", "disconnected.jpg"))
    End Sub

    Dim IsMouseDown As Boolean 'Is mouse pressed
    Dim Controlling As Boolean = False 'Mouse and KeyBoard Controls On or Off

    Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Short) As Short

    Declare Function GetKeyboardState Lib "user32" (ByRef lpKeyState As Byte) As Boolean

    Public Function MouseIsOverControl(ByVal c As Control) As Boolean  'Detects if Mouse is Over Control
        Return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition))
    End Function

    ''Detect Mouse Movement
    Private Sub MoveMove(sender As Object, e As MouseEventArgs) Handles ViewDesktop.MouseMove
        Dim PPoint As Point = New Point(e.X, e.Y)  'get mouse coordinates

        'convert coordinates from picturebox size to to 1080p
        Dim PPoint2 As Point = ConvertPointSize(ViewDesktop.Width, ViewDesktop.Height, PPoint.X, PPoint.Y, 1920, 1080)
        Dim x As String = PPoint2.X.ToString
        Dim y As String = PPoint2.Y.ToString

        'if mouse is Over Control picturebox/screen
        If MouseIsOverControl(ViewDesktop) Then
            Dim content As String
            content = Len(x) & Len(y) & x & y

            SendCoordinatesToClient(content)  'send the mouse coords to the client
        End If
    End Sub

    'Handles Mouse Clicks
    Private Sub ViewDesktop_MouseDown(sender As Object, e As MouseEventArgs) Handles ViewDesktop.MouseDown
        If IsMouseDown = False Then PerformMouseClick(e)
    End Sub
    Private Sub ViewDesktop_MouseUp(sender As Object, e As MouseEventArgs) Handles ViewDesktop.MouseUp
        If IsMouseDown = True Then PerformMouseRelease(e)
    End Sub

    Sub PerformMouseClick(e As MouseEventArgs)
        IsMouseDown = True
        Select Case e.Button
            Case MouseButtons.Left
                SimulateMousePress(e.Button)
                Console.WriteLine("Left mouse button pressed")
            Case MouseButtons.Right
                SimulateMousePress(e.Button)
                Console.WriteLine("Right mouse button pressed")
            Case MouseButtons.Middle
                SimulateMousePress(e.Button)
                Console.WriteLine("Middle mouse button pressed")
            Case MouseButtons.XButton1
                SimulateMousePress(e.Button)
                Console.WriteLine("XButton1 pressed")
            Case MouseButtons.XButton2
                SimulateMousePress(e.Button)
                Console.WriteLine("XButton2 pressed")
        End Select
    End Sub
    Sub PerformMouseRelease(e As MouseEventArgs)
        IsMouseDown = False
        Select Case e.Button
            Case MouseButtons.Left
                SimulateMouseRelease(e.Button)
                Console.WriteLine("Left mouse button released")
            Case MouseButtons.Right
                SimulateMouseRelease(e.Button)
                Console.WriteLine("Right mouse button released")
            Case MouseButtons.Middle
                SimulateMouseRelease(e.Button)
                Console.WriteLine("Middle mouse button released")
            Case MouseButtons.XButton1
                SimulateMouseRelease(e.Button)
                Console.WriteLine("XButton1 released")
            Case MouseButtons.XButton2
                SimulateMouseRelease(e.Button)
                Console.WriteLine("XButton2 released")
        End Select
    End Sub
    Public Sub SimulateMousePress(button As MouseButtons)
        Select Case button
            Case MouseButtons.Left
                SendKeyPressToClient("lpress")
            Case MouseButtons.Right
                SendKeyPressToClient("rpress")
            Case MouseButtons.Middle
                SendKeyPressToClient("mpress")
        End Select
    End Sub
    Public Sub SimulateMouseRelease(button As MouseButtons)
        Select Case button
            Case MouseButtons.Left
                SendKeyPressToClient("lrelease")
            Case MouseButtons.Right
                SendKeyPressToClient("rrelease")
            Case MouseButtons.Middle
                SendKeyPressToClient("mrelease")
        End Select
    End Sub

    Function ConvertPointSize(ByVal swidth As Integer, ByVal sheight As Integer, ByVal x As Integer, ByVal y As Integer, ByVal twidth As Integer, ByVal theight As Integer) As Point
        Dim soheight As Decimal = sheight
        Dim sowidth As Decimal = swidth
        Dim taheight As Decimal = theight
        Dim tawidth As Decimal = twidth
        Dim ydiff As Decimal
        If soheight = taheight Then
            ydiff = 1
        Else
            ydiff = taheight / soheight
        End If

        Dim xdiff As Decimal
        If sowidth = twidth Then
            xdiff = 1
        Else
            xdiff = tawidth / sowidth
        End If

        If x = 0 Then
            x = 1
        End If
        If y = 0 Then
            y = 1
        End If

        Return New Point(x * xdiff, y * ydiff)
    End Function

    ''Detects keyboard presses
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'Check if the window where the picturebox is located is focued
        If MDIParent1.IsApplicationFocused() Then
            For i As Integer = 1 To 255
                If GetAsyncKeyState(i) = -32767 Then
                    If CType(i, Keys).ToString() = "LButton" Then
                        Exit Sub
                    ElseIf CType(i, Keys).ToString() = "RButton" Then
                        Exit Sub
                    ElseIf CType(i, Keys).ToString() = "MButton" Then
                        Exit Sub
                    Else
                        SendKeyPressToClient(i)
                    End If
                End If
            Next
        End If
    End Sub
    'Sends Mouse Position
    Sub SendCoordinatesToClient(ByVal Message As String)
        Dim ReceivingIP As String
        ReceivingIP = MDIParent1.getSelectedIP 'ip to control
        If Controlling Then
            If MDIParent1.isConnected(ReceivingIP) Then 'check if connected
                Try
                    Dim clsError As System.Net.Sockets.SocketError
                    Dim msg As String = Message
                    Console.WriteLine("Sent coordinates: (" & msg & ")")
                    If String.IsNullOrWhiteSpace(ReceivingIP) Then
                        Exit Sub
                    End If
                    Try
                        Dim bMessage As Byte() = System.Text.Encoding.ASCII.GetBytes(msg)
                        Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

                        clsSocket.Connect(ReceivingIP, 1111)
                        clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                        If clsError = SocketError.Success Then
                            'MessageBox.Show(Me, "Message sent!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show(Me, "The message was not sent successfully, the message was: " & clsError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                        clsSocket.Close()
                        clsSocket = Nothing
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                Catch ex As Exception
                    MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                DisableControl()
            End If
        End If
    End Sub

    'Sends Key Press and Mouse Clicks
    Sub SendKeyPressToClient(ByVal Message As String)
        Dim ReceivingIP As String
        ReceivingIP = MDIParent1.getSelectedIP 'ip to control
        If Controlling Then
            If MDIParent1.isConnected(ReceivingIP) Then 'check if connected
                If String.IsNullOrWhiteSpace(ReceivingIP) Then
                    Exit Sub
                End If
                Try
                    Dim clsError As System.Net.Sockets.SocketError
                    Dim msg As String = Message
                    Console.WriteLine("Sent keypress: (" & msg & ")")
                    Try
                        'Dim bMessage As Byte() = BitConverter.GetBytes(x).Concat(BitConverter.GetBytes(y)).ToArray()
                        Dim bMessage As Byte() = System.Text.Encoding.ASCII.GetBytes(msg)
                        Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)

                        clsSocket.Connect(ReceivingIP, 1122)
                        clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                        If clsError = SocketError.Success Then
                            'MessageBox.Show(Me, "Message sent!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show(Me, "The message was not sent successfully, the message was: " & clsError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                        clsSocket.Close()
                        clsSocket = Nothing
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                Catch ex As Exception
                    MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                DisableControl()
            End If

        End If
    End Sub

    ''Enabling and Disable Control
    Private Sub btnOn_Click(sender As Object, e As EventArgs) 'On Button
        EnableControl()
    End Sub

    Private Sub btnOff_Click(sender As Object, e As EventArgs) 'Off Button
        DisableControl()
    End Sub
    Sub ControlButton_Click() 'On/Off Toggle Button
        If Controlling Then
            DisableControl()
        Else
            EnableControl()
        End If
    End Sub

    Sub DisableControl()
        If Controlling Then
            Controlling = False
            Label1.ForeColor = Color.Firebrick
            Label1.Text = "Disabled"
            Controlling = False
            Timer1.Enabled = False
            Timer1.Stop()
            MDIParent1.ControlButton.Text = "Control Disabled"
            MDIParent1.ControlButton.ForeColor = Color.FromArgb(&H212121)
        End If


    End Sub
    Sub EnableControl()
        If Not Controlling Then
            ViewDesktop.Select()
            Controlling = True
            Label1.ForeColor = Color.RoyalBlue
            Label1.Text = "Enabled"
            Controlling = True
            Timer1.Enabled = True
            Timer1.Start()
            MDIParent1.ControlButton.Text = "Control Enabled"
            MDIParent1.ControlButton.ForeColor = Color.FromArgb(&H1F801F)
        End If


    End Sub
    'auto resizes the picturebox
    Function fixAR()
        Dim oldheight As Decimal = Me.Height - 100
        Dim oldwidth As Decimal = Me.Width - 30

        Dim newheight As Decimal = oldheight
        Dim newwidth As Decimal = newheight * (16 / 9)

        If newwidth + 30 > Me.Width Then
            newwidth = oldwidth
            newheight = newwidth * (9 / 16)
        End If

        ViewDesktop.Height = newheight
        ViewDesktop.Width = newwidth
        ViewDesktop.Location = New Point((Me.Width - ViewDesktop.Width) / 2, ((Me.Height) - ViewDesktop.Height) / 2)
    End Function
    Private Sub ViewDesktop_SizeChanged(sender As Object, e As EventArgs) Handles ViewDesktop.SizeChanged
        fixAR()
    End Sub
End Class

