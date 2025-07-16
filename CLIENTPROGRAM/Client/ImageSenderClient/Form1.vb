Imports System.Net.Sockets
Imports System.Net
Imports System.Threading
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net.Dns
Imports System.Runtime.InteropServices
Imports System.Management
Imports System.Drawing.Imaging
Imports Encoder = System.Drawing.Imaging.Encoder
Imports System.Drawing



Public Class Form1
    Const ConnectionPort As Int16 = 9876 'Connection Port Number
    Const RequestPort As Int16 = 6789 'Request Port Number
    Const RequestPort2 As Int16 = 6800

    Dim ServerIp As String
    Dim ClientIp As String
    Dim NetStream As NetworkStream
    Dim NetStream2 As NetworkStream
    Dim myReader As BinaryReader
    Dim myWriter As BinaryWriter

    Dim Look4Request As Thread = Nothing
    Dim Look4Request2 As Thread = Nothing
    Dim Look4MsgRequest As Thread = Nothing
    Dim Look4MouseMvmt As Thread = Nothing
    Dim Look4KeyPress As Thread = Nothing

    Dim infiniteCounter As Integer
    Dim readData As String
    Dim IpAddr2() As Net.IPAddress = GetHostEntry(GetHostName()).AddressList
    Dim IpAddr As Net.IPAddress
    Dim controlling As Boolean = True

    'current dir for file managing
    Dim currentDir As String
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.NotifyIcon1.Visible = False
        Me.NotifyIcon1.Dispose()
        System.Environment.Exit(System.Environment.ExitCode) ' Informs to the Server When it is Closed
    End Sub
    Sub StartReceivingServerScreen()
        Dim serverListener As New TcpListener(IPAddress.Any, 6790)
        serverListener.Start()

        While True
            Dim serverSender As TcpClient = serverListener.AcceptTcpClient()
            Dim stream As NetworkStream = serverSender.GetStream()
            Dim ms As New MemoryStream()

            Dim buffer(1023) As Byte
            Dim bytesRead As Integer
            Do
                bytesRead = stream.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then Exit Do
                ms.Write(buffer, 0, bytesRead)
            Loop

            Dim img As Image = Image.FromStream(ms)
            ' Show the image in a PictureBox or Form
            PictureBox1.Image = img

            stream.Close()
            serverSender.Close()
        End While
    End Sub

    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim receiveScreenThread As New Thread(AddressOf StartReceivingServerScreen)
        receiveScreenThread.IsBackground = True
        receiveScreenThread.Start()

        'Stores the ImageReceiver Address To Connect
        ServerIp = InputBox("Enter the Server IPAddress", "Server IPAddress", "192.168.1.10").Trim()
        If ServerIp = "" Then
            MsgBox("Server IP address can't be empty", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical)
            End
        End If
        ClientIp = InputBox("Enter the Client IPAddress", "Client IPAddress", IpAddr2(1).ToString()).Trim()
        If ClientIp = "" Then
            MsgBox("Client IP address can't be empty", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical)
            End
        Else
            Try
                IpAddr = Dns.GetHostAddresses(ClientIp)(0)
            Catch ex As Exception
                MsgBox("Invalid Client IP address", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical)
                End
            End Try
        End If

        'Informs the Server That it is Connected
        Try
            Dim myClient As New TcpClient
            myClient.Connect(ServerIp, ConnectionPort)
            myClient.Close()
        Catch ex As SocketException
            MsgBox("Error connecting to server: " & ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)
            Me.NotifyIcon1.Dispose()
            End
        Catch ex As Exception
            MsgBox("Unexpected error: " & ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)
            Me.NotifyIcon1.Dispose()
            End
        End Try

        ' Creates a Thread To Listen for Image Request
        StartThread(AddressOf StartWaitForRequest, "WaitForRequestThread")
        ' Creates a Thread To Listen for Process Requests
        StartThread(AddressOf StartWaitForRequest2, "WaitForRequest2Thread")

        ' Creates a Thread To Receive Messages
        Look4MsgRequest = New Thread(AddressOf ReceiveMsg)
        Look4MsgRequest.Priority = ThreadPriority.BelowNormal
        Look4MsgRequest.Start()

        ' Creates a Thread To Listen for Mouse Movements
        Look4MouseMvmt = New Thread(AddressOf ListenToMouseMovement)
        Look4MouseMvmt.Start()

        ' Creates a Thread To Listen for Keypress Events
        Look4KeyPress = New Thread(AddressOf ListenToKeyPresses)
        Look4KeyPress.Start()
    End Sub

    Private Sub StartThread(ByVal threadAction As ThreadStart, ByVal threadName As String)
        Dim thread As Thread = New Thread(threadAction)
        thread.Name = threadName
        thread.Start()
    End Sub


    Private Sub StartWaitForRequest()
        Dim retryInterval As Integer = 1000 ' Initial retry interval (1 second)
        While True
            Try
                Console.WriteLine("Started WaitForRequest")
                WaitForRequest()
                retryInterval = 1000 ' Reset retry interval on successful operation
            Catch ex As SocketException
                Console.WriteLine("SocketException in WaitForRequest: " & ex.Message)
                Thread.Sleep(retryInterval)
                retryInterval *= 2 ' Exponential backoff: double the interval for next retry
            Catch ex As IOException
                Console.WriteLine("IOException in WaitForRequest: " & ex.Message)
                Thread.Sleep(retryInterval)
                retryInterval *= 2 ' Exponential backoff: double the interval for next retry
            Catch ex As Exception
                Console.WriteLine("Error in WaitForRequest: " & ex.Message)
                Thread.Sleep(retryInterval)
                retryInterval *= 2 ' Exponential backoff: double the interval for next retry
            Finally
                If NetStream IsNot Nothing Then
                    NetStream.Close()
                End If
            End Try
        End While
    End Sub

    Sub WaitForRequest() 'Images
        Try
            Dim ServerAddress As IPAddress = Dns.GetHostAddresses(ServerIp)(0)
            Dim myListener As New TcpListener(IPAddress.Any, RequestPort)
            myListener.Start()
            While True
                Try
                    Dim myClient As TcpClient = myListener.AcceptTcpClient()
                    NetStream = myClient.GetStream()
                    Send_Screen_Shot() ' Send Screen Image to Server
                    myClient.Close()
                Catch ex As Exception

                    Exit While
                End Try
            End While
        Catch ex As Exception
            Console.WriteLine("Error in WaitForRequest: " & ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)
        Finally
            If NetStream IsNot Nothing Then
                NetStream.Close()
            End If
        End Try
    End Sub

    Sub Send_Screen_Shot()
        Dim FileName As String = Environment.CurrentDirectory & "\Mohiudeen_Screen2.bmp"
        Dim screenWidth As Integer = Screen.FromControl(Me).Bounds.Width
        Dim screenHeight As Integer = Screen.FromControl(Me).Bounds.Height

        ' Lower resolution settings
        Dim lowerWidth As Integer = screenWidth * 0.45 ' Example lower width
        Dim lowerHeight As Integer = screenHeight * 0.45 ' Example lower height

        Try
            ' Capture the full-resolution screenshot
            Using originalBitmap As New Bitmap(screenWidth, screenHeight)
                Using g As Graphics = Graphics.FromImage(originalBitmap)
                    g.CopyFromScreen(0, 0, 0, 0, New Size(screenWidth, screenHeight))
                End Using

                ' Create a bitmap with the lower resolution
                Using lowerResolutionBitmap As New Bitmap(lowerWidth, lowerHeight)
                    Using g As Graphics = Graphics.FromImage(lowerResolutionBitmap)
                        ' Resize the original screenshot to the lower resolution
                        g.DrawImage(originalBitmap, 0, 0, lowerWidth, lowerHeight)
                    End Using

                    ' Ensure any existing file is deleted
                    If File.Exists(FileName) Then
                        File.Delete(FileName)
                    End If

                    ' Save the lower resolution screenshot as a compressed JPEG file
                    Dim jpegEncoder As ImageCodecInfo = GetEncoder(ImageFormat.Jpeg)
                    Dim encoderParams As New EncoderParameters(1)
                    encoderParams.Param(0) = New EncoderParameter(Encoder.Quality, 70L) ' Set quality level (0-100)

                    lowerResolutionBitmap.Save(FileName, jpegEncoder, encoderParams)
                End Using
                originalBitmap.Dispose()
            End Using

            ' Send the file contents over the network stream
            Using FStreams As New FileStream(FileName, FileMode.Open)
                Dim buffer(1024) As Byte
                Dim bytesRead As Integer

                Do
                    bytesRead = FStreams.Read(buffer, 0, buffer.Length)
                    If bytesRead > 0 Then
                        Me.NetStream.Write(buffer, 0, bytesRead)
                        Me.NetStream.Flush()
                    End If
                Loop While bytesRead > 0

                ' Close the file stream
                FStreams.Close()
            End Using

            ' Optional: Delete the file after sending
            If File.Exists(FileName) Then
                File.Delete(FileName)
            End If
        Catch ex As Exception
            Console.WriteLine("Error in Send_Screen_Shot: " & ex.Message)
            ' Handle or log the exception as needed
        Finally
            ' Close the network stream if it's open
            If NetStream IsNot Nothing Then
                NetStream.Close()
            End If
        End Try
    End Sub
    Private Function GetEncoder(ByVal format As ImageFormat) As ImageCodecInfo
        Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageDecoders()
        For Each codec As ImageCodecInfo In codecs
            If codec.FormatID = format.Guid Then
                Return codec
            End If
        Next
        Return Nothing
    End Function



    Private Sub StartWaitForRequest2()
        Dim retryInterval As Integer = 1000 ' Initial retry interval (1 second)
        While True
            Try
                Console.WriteLine("Started WaitForRequest2")
                WaitForRequest2()
                retryInterval = 1000 ' Reset retry interval on successful operation
            Catch ex As SocketException
                Console.WriteLine("SocketException in WaitForRequest2: " & ex.Message)
                Thread.Sleep(retryInterval)
                retryInterval *= 2 ' Exponential backoff: double the interval for next retry
            Catch ex As IOException
                Console.WriteLine("IOException in WaitForRequest2: " & ex.Message)
                Thread.Sleep(retryInterval)
                retryInterval *= 2 ' Exponential backoff: double the interval for next retry
            Catch ex As Exception
                Console.WriteLine("Error in WaitForRequest2: " & ex.Message)
                Thread.Sleep(retryInterval)
                retryInterval *= 2 ' Exponential backoff: double the interval for next retry
            Finally
                If NetStream2 IsNot Nothing Then
                    NetStream2.Close()
                End If
            End Try
        End While
    End Sub
    Sub WaitForRequest2() 'Sending Process
        Try
            Dim ServerAddress As IPAddress = Dns.GetHostAddresses(ServerIp)(0)
            Dim myListener As New TcpListener(IPAddress.Any, RequestPort2)
            myListener.Start()
            While True
                Try
                    Dim myClient2 As TcpClient = myListener.AcceptTcpClient()
                    NetStream2 = myClient2.GetStream()
                    Send_Running_Apps() ' Send List of Running Applications to Server
                    myClient2.Close()
                Catch ex As Exception

                    Exit While
                End Try
            End While
        Catch ex As Exception
            Console.WriteLine("Error in WaitForRequest2: " & ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)
        Finally
            If NetStream2 IsNot Nothing Then
                NetStream2.Close()
            End If
        End Try
    End Sub
    'Sends the List of Running Applications to Client
    Sub Send_Running_Apps()

        ' Retrieve the list of running processes
        Dim processList As Process() = Process.GetProcesses()
        ' StringBuilder to accumulate the process details
        Dim processInfo As New StringBuilder()
        processInfo.AppendLine("Task Name␟ Application Name␟ PID␟ Class Name␟ CommandLine")
        For Each proc As Process In processList
            Try
                ' Get process details
                Dim taskName As String = proc.MainWindowTitle
                ' Append details to StringBuilder
                If String.IsNullOrEmpty(taskName) Then
                Else
                    Dim processName As String = proc.ProcessName
                    Dim pid As Integer = proc.Id
                    Dim className As String = proc.MainWindowHandle.ToString()
                    Dim commandLine As String = GetCommandLine(proc.Id)
                    processInfo.AppendLine($"{taskName}␟ {processName}␟ {pid}␟ {className}␟ {commandLine}")
                End If

            Catch ex As Exception
                ' Handle any exceptions, such as access denied
                Continue For
            End Try
        Next
        Dim message As String = processInfo.ToString()
        Dim buffer() As Byte = Encoding.UTF8.GetBytes(message)
        If NetStream2 IsNot Nothing AndAlso NetStream2.CanWrite Then
            NetStream2.Write(buffer, 0, buffer.Length)
            NetStream2.Flush()
            NetStream2.Close()
        End If
    End Sub

    Private Function GetCommandLine(pId As Integer) As String 'gets the command line
        Dim commandLine As String = String.Empty
        Try
            Dim query As String = "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " & pId.ToString
            Dim searcher As New ManagementObjectSearcher(query)

            For Each obj As ManagementObject In searcher.Get()
                ' Dim pisd As Integer = Convert.ToInt32(obj("ProcessId"))
                commandLine = If(obj("CommandLine")?.ToString(), "N/A")
                Console.WriteLine("Command Line: " & commandLine)

                Dim text As String = "some text here ###MONTH-3### some text here ###MONTH-2### ..."
                Dim regex = New System.Text.RegularExpressions.Regex("^\""(.*)\""")

                Dim m As Match = regex.Match(commandLine)

                If String.IsNullOrEmpty(m.Groups(1).Value) Then
                    Return commandLine
                End If
                Return m.Groups(1).Value
                Exit For
            Next
        Catch ex As Exception
            Console.WriteLine("An error occurred: " & ex.Message)
        End Try
        Console.ReadLine()
        Return commandLine
    End Function

    Sub ReceiveMsg()
        Dim clsEndpoint As IPEndPoint = New IPEndPoint(IpAddr, 1977)
        Dim clsServerSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Try
            clsServerSocket.Bind(clsEndpoint)

            clsServerSocket.Listen(SocketOptionName.MaxConnections)
            Do
                ' Poll for incoming connections every 10 seconds
                If clsServerSocket.Poll(10000, SelectMode.SelectRead) Then
                    ' Accept the incoming connection
                    Dim clsSocket As Socket = clsServerSocket.Accept()
                    ' Handle the connection asynchronously to avoid blocking the main thread
                    Dim clientAddress As String = DirectCast(clsSocket.RemoteEndPoint, IPEndPoint).Address.ToString()
                    Console.WriteLine($"Client connected: {clientAddress}")
                    ' Process the client's message
                    Try
                        Dim bReadBuffer As Byte() = New Byte(255) {}
                        Dim bytesRead As Integer = clsSocket.Receive(bReadBuffer, bReadBuffer.Length, SocketFlags.None)
                        If bytesRead > 0 Then
                            Dim sMessage As String = System.Text.Encoding.ASCII.GetString(bReadBuffer, 0, bytesRead).Trim()
                            ' Process the message based on the command code
                            ProcessMessage(sMessage)
                        End If
                    Catch ex As Exception
                        Console.WriteLine($"Error processing message from {clientAddress}: {ex.Message}")
                    Finally
                        ' Close the client socket
                        clsSocket.Close()
                        Console.WriteLine($"Connection with {clientAddress} closed")
                    End Try
                End If
            Loop
        Catch ex As Exception
            Console.WriteLine($"Server error: {ex.Message}")
        Finally
            ' Close the server socket when done
            clsServerSocket.Close()
            Console.WriteLine("Server socket closed")
        End Try
    End Sub

    Sub ProcessMessage(ByVal message As String)
        ' Extract command code and message content
        Dim mCode As String = Regex.Match(message, "^(\([A-Z]{3}.*?\))").Value
        Dim sMessage As String = Regex.Replace(message, "^(\([A-Z]{3}.*?\))", "")

        ' Process based on command code
        Select Case True
            Case mCode.StartsWith("(ACT") 'Action
                ProcessActionCommand(sMessage)
            Case mCode.StartsWith("(SPR") ' Start Process
                ProcessStartProcessCommand(sMessage)
            Case mCode.StartsWith("(LNK")   'Open Link
                ProcessOpenLinkCommand(sMessage)
            Case mCode.StartsWith("(CLS")   'Kill Process
                ProcessKillProcessCommand(sMessage)
            Case mCode.StartsWith("(DIR")

            Case mCode.StartsWith("(MSG")   'Show Message
                ShowMessageBoxOnTop(sMessage.Trim(), "Incoming Message")
            Case Else
                ShowMessageBoxOnTop(sMessage.Trim(), "Incoming Message")
                '               ShowMessageBoxOnTop("Unknown command code: " & sMessage.Trim(), "Incoming Message")
        End Select
    End Sub

    Sub ShowMessageBoxOnTop(ByVal message As String, ByVal title As String, Optional ByVal icon As MessageBoxIcon = MessageBoxIcon.Information)
        MessageBox.Show(message, title, MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
    End Sub

    Sub ProcessActionCommand(ByVal action As String)
        ' Process PC action commands
        Select Case action
            Case "shutdownF"
                ShutdownComputer(True)
            Case "shutdown"
                ShutdownComputer()
            Case "restartF"
                RestartComputer(True)
            Case "restart"
                RestartComputer()
            Case "logoffF"
                LogOffComputer(True)
            Case "logoff"
                LogOffComputer()
            Case "sleepF"
                SleepComputer(True)
            Case "sleep"
                SleepComputer()
            Case "hibernateF"
                HibernateComputer(True)
            Case "hibernate"
                HibernateComputer()
            Case "standbyF"
                StandbyComputer(True)
            Case "standby"
                StandbyComputer()
            Case "lockF"
                StandbyComputer(True)
            Case "lock"
                StandbyComputer()
            Case "ctrlon"
                TurnOnControls()
            Case "ctrloff"
                TurnOffControls()
            Case Else
                Console.WriteLine($"Unknown action command received: {action}")
        End Select
    End Sub

    Sub ProcessStartProcessCommand(ByVal processPath As String)
        ' Start a process based on the provided path
        Try
            System.Diagnostics.Process.Start(processPath)
        Catch ex As Exception
            Console.WriteLine($"Error starting process: {ex.Message}")
        End Try
    End Sub

    Sub ProcessOpenLinkCommand(ByVal link As String)
        ' Open a link in the default browser
        Try
            System.Diagnostics.Process.Start("http://" & link)
        Catch ex As Exception
            Console.WriteLine($"Error opening link: {ex.Message}")
        End Try
    End Sub

    Sub ProcessKillProcessCommand(ByVal processID As String)
        ' Kill a process with the specified id
        Try
            KillProcess(processID)
        Catch ex As Exception
            Console.WriteLine($"Error killing process: {ex.Message}")
        End Try
    End Sub

    Sub KillProcess(ByVal PID As String)
        Try
            Dim aProcess As System.Diagnostics.Process

            Console.WriteLine("Killing Process: " & PID)
            aProcess = System.Diagnostics.Process.GetProcessById(PID)
            aProcess.Kill()
            aProcess.WaitForExit()

            Console.WriteLine("Process killed successfully.")
        Catch ex As ArgumentException
            Console.WriteLine("Process with the specified PID does not exist.")
        Catch ex As InvalidOperationException
            Console.WriteLine("The process has already exited.")
        Catch ex As System.ComponentModel.Win32Exception
            Console.WriteLine("An error occurred while accessing the process.")
        Catch ex As Exception
            Console.WriteLine("An unexpected error occurred: " & ex.Message)
        End Try
    End Sub


    ''Mouse and Keyboard Control
    'Listen to updcoming mouse coordinates
    Public Sub ListenToMouseMovement()
        Dim clsEndpoint As New IPEndPoint(IpAddr, 1111)
        Dim clsServerSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Try
            clsServerSocket.Bind(clsEndpoint)
            ' Listen for incoming connections
            clsServerSocket.Listen(SocketOptionName.MaxConnections)
            Console.WriteLine("Waiting for data...")
            Do
                If clsServerSocket.Poll(10000, SelectMode.SelectRead) Then
                    Dim clsSocket As Socket = clsServerSocket.Accept()

                    Dim bReadBuffer As Byte() = New Byte(255) {}
                    Dim bytesRead As Integer = clsSocket.Receive(bReadBuffer, bReadBuffer.Length, SocketFlags.None)

                    Dim sMessage As String = System.Text.Encoding.ASCII.GetString(bReadBuffer, 0, bytesRead).Trim()
                    Console.WriteLine("Mouse Coordinates: " & sMessage)

                    ' process mouse coordinates
                    processMouseCoordinate(sMessage)

                    clsSocket.Close()
                End If
            Loop
        Catch ex As Exception
            Console.WriteLine("Error in ListenToMouseMovement: " & ex.Message)
        Finally
            ' Close the server socket if needed
            If clsServerSocket IsNot Nothing Then
                clsServerSocket.Close()
            End If
        End Try
    End Sub

    'Listen to updcoming keyboard and mouse presses
    Public Sub ListenToKeyPresses()
        Dim clsEndpoint As New IPEndPoint(IpAddr, 1122)
        Dim clsServerSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Try
            clsServerSocket.Bind(clsEndpoint)
            clsServerSocket.Listen(SocketOptionName.MaxConnections)
            Console.WriteLine("Waiting for data...")
            Do
                If clsServerSocket.Poll(10000, SelectMode.SelectRead) Then
                    Dim clsSocket As Socket = clsServerSocket.Accept()

                    Dim bReadBuffer As Byte() = New Byte(255) {}
                    Dim bytesRead As Integer = clsSocket.Receive(bReadBuffer, bReadBuffer.Length, SocketFlags.None)

                    Dim sMessage As String = System.Text.Encoding.ASCII.GetString(bReadBuffer, 0, bytesRead).Trim()
                    Console.WriteLine("Received message: " & sMessage)

                    SimulateKeyPress(sMessage)

                    clsSocket.Close()
                End If
            Loop
        Catch ex As Exception
            Console.WriteLine("Error in ListenToKeyPresses: " & ex.Message)
        Finally
            If clsServerSocket IsNot Nothing Then
                clsServerSocket.Close()
            End If
        End Try
    End Sub

    Private Sub SimulateKeyPress(key As String)
        If controlling Then       'Mouse Buttons
            If key.StartsWith("lpress") Then
                Console.WriteLine("LButton Press")
                mouse_event(&H2, 0, 0, 0, 0)
            ElseIf key.StartsWith("lrelease") Then
                Console.WriteLine("LButton Release")
                mouse_event(&H4, 0, 0, 0, 0)
            ElseIf key.StartsWith("rpress") Then
                Console.WriteLine("RButton Press")
                mouse_event(&H8, 0, 0, 0, 0)
            ElseIf key.StartsWith("rrelease") Then
                Console.WriteLine("RButton Release")
                mouse_event(&H10, 0, 0, 0, 0)
            ElseIf key.StartsWith("mpress") Then
                Console.WriteLine("MButton Press")
                mouse_event(&H20, 0, 0, 0, 0)
            ElseIf key.StartsWith("mrelease") Then
                Console.WriteLine("MButton Release")
                mouse_event(&H40, 0, 0, 0, 0)
            ElseIf key.StartsWith("scrollup") Then          'Doesnt Work (probably)
                Console.WriteLine("Scroll Up")
                mouse_event(&H800, 0, 0, -120, IntPtr.Zero)
            ElseIf key.StartsWith("scrolldown") Then            'Doesnt Work (probably)
                Console.WriteLine("Scroll Down")
                mouse_event(&H800, 0, 0, 120, IntPtr.Zero)
            Else            'KeyBoard Keys
                Console.WriteLine("KeyBoard Button")
                keybd_event(key, 0, 0, IntPtr.Zero)
                Thread.Sleep(100)
                keybd_event(key, 0, KEYEVENTF_KEYUP, IntPtr.Zero)
            End If
        End If
    End Sub

    'Contains x and y; not actually listboxes
    Dim ListBox1 As String = 0 'x
    Dim ListBox2 As String = 0 'y

    Private Sub processMouseCoordinate(ByVal mycoordinat As String)
        Dim mycoordinate As String = mycoordinat.Trim
        Try
            Dim lenx As String = Microsoft.VisualBasic.Mid(mycoordinate, 1, 1)
            Dim leny As String = Microsoft.VisualBasic.Mid(mycoordinate, 2, 1)
            Dim x As String = Microsoft.VisualBasic.Mid(mycoordinate, 3, CType(lenx, Integer))
            Dim y As String = Microsoft.VisualBasic.Mid(mycoordinate, 3 + CType(lenx, Integer), CType(leny, Integer))

            ' Convert coordinates from 1080p to client screen resolution
            Dim PPoint As Point = ConvertPointSize(1920, 1080, x, y, Screen.FromControl(Me).Bounds.Width, Screen.FromControl(Me).Bounds.Height)
            ListBox1 = PPoint.X
            ListBox2 = PPoint.Y

            If controlling Then  ' Move cursor
                Console.WriteLine(Convert.ToString(New Point(CType(ListBox1, Integer), CType(ListBox2, Integer))))
                Cursor.Position = New Point(CType(ListBox1, Integer), CType(ListBox2, Integer))
            End If
        Catch ex As Exception
            Console.WriteLine("Error processing mouse coordinates: " & ex.Message)
        End Try
    End Sub
    Function ConvertPointSize(ByVal swidth As Integer, ByVal sheight As Integer, ByVal x As Integer, ByVal y As Integer, ByVal twidth As Integer, ByVal theight As Integer) As Point
        If x = 0 Then
            x = 1
        End If
        If y = 0 Then
            y = 1
        End If
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
        Return New Point(x * xdiff, y * ydiff)
    End Function





    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Ensure user input is unblocked when the form is closing
        BlockUserInput(False)
    End Sub
    Sub ShutdownComputer(Optional ByVal forced As Boolean = False)
        Try
            Process.Start("shutdown", "/s /t 0")
        Catch ex As Exception
            Console.WriteLine("Shutdown failed: " & ex.Message)
        End Try
    End Sub
    Sub RestartComputer(Optional ByVal forced As Boolean = False)
        Try
            Process.Start("shutdown", "/r /t 0")
        Catch ex As Exception
            Console.WriteLine("Restart failed: " & ex.Message)
        End Try
    End Sub
    Sub HibernateComputer(Optional ByVal forced As Boolean = False)
        If Not SetSuspendState(True, forced, False) Then
            Console.WriteLine("Hibernate failed: " & Marshal.GetLastWin32Error().ToString())
        End If
    End Sub
    Sub SleepComputer(Optional ByVal forced As Boolean = False)
        If Not SetSuspendState(False, forced, False) Then
            Console.WriteLine("Sleep failed: " & Marshal.GetLastWin32Error().ToString())
        End If
    End Sub
    Sub LogOffComputer(Optional ByVal forced As Boolean = False)
        Dim flags As Integer = EWX_LOGOFF
        If forced Then
            flags = flags Or EWX_FORCE Or EWX_FORCEIFHUNG
        End If
        If Not ExitWindowsEx(flags, 0) Then
            Console.WriteLine("Logoff failed: " & Marshal.GetLastWin32Error().ToString())
        End If
    End Sub
    Sub StandbyComputer(Optional ByVal forced As Boolean = False)
        If Not Application.SetSuspendState(PowerState.Suspend, forced, False) Then
            Console.WriteLine("Standby failed: " & Marshal.GetLastWin32Error().ToString())
        End If
    End Sub
    Sub LockComputer(Optional ByVal forced As Boolean = False)
        If Not LockWorkStation() Then
            Console.WriteLine("Locking the workstation failed.")
        End If
    End Sub

    Declare Auto Function ExitWindowsEx Lib "user32.dll" (ByVal uFlags As Integer, ByVal dwReason As Integer) As Boolean
    Declare Auto Function SetSuspendState Lib "powrprof.dll" (ByVal Hibernate As Boolean, ByVal ForceCritical As Boolean, ByVal DisableWakeEvent As Boolean) As Boolean
    Declare Auto Function LockWorkStation Lib "user32.dll" () As Boolean

    Const EWX_LOGOFF As Integer = &H0
    Const EWX_SHUTDOWN As Integer = &H1
    Const EWX_REBOOT As Integer = &H2
    Const EWX_FORCE As Integer = &H4
    Const EWX_POWEROFF As Integer = &H8
    Const EWX_FORCEIFHUNG As Integer = &H10


    Declare Function mouse_event Lib "user32" (ByVal dwFlags As Short, ByVal dx As Short, ByVal dy As Short, ByVal dwData As Short, ByVal dwExtraInfo As IntPtr) As Short
    Public Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Long, ByVal dwExtraInfo As Long)

    Const KEYEVENTF_EXTENDEDKEY As UInteger = &H1
    Const KEYEVENTF_KEYUP As UInteger = &H2


    Sub TurnOnControls()
        Console.WriteLine("on")
        BlockUserInput(False)
        'controlling = True
        'Timer1.Enabled = True
        'Timer1.Start()
        'Timer1.Interval = 200
    End Sub
    Sub TurnOffControls()
        Console.WriteLine("off")
        BlockUserInput(True)
        Threading.Thread.Sleep(5000)
        BlockUserInput(False)
        'Timer1.Enabled = False
        'Timer1.Stop()
        'controlling = False
    End Sub

    ' Import the BlockInput function from user32.dll
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function BlockInput(fBlock As Boolean) As Boolean
    End Function

    ' Method to block user input
    Public Shared Function BlockUserInput(block As Boolean) As Boolean
        Try
            ' Call BlockInput with the specified block flag
            Return BlockInput(block)
        Catch ex As Exception
            Console.WriteLine("Failed to block user input: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class
