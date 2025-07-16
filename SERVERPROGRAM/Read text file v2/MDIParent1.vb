Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.JavaScript.JSType
Imports System.Text
Imports System.Text.Json
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class MDIParent1
    Dim sPath As String = Application.StartupPath()
    Dim aPath As String = sPath & "\test.txt"
    Dim onPath As String = sPath & "\images\on.png"
    Dim offPath As String = sPath & "\images\off.png"
    Dim nothingPath As String = sPath & "\images\nothing.png"
    Dim isSharingScreen As Boolean = False

    Shared ActiveTab As Int16 = 0


    Public m_ChildFormNumber As Integer
    Dim al As ArrayList = New ArrayList() 'IP
    Dim al2 As ArrayList = New ArrayList() 'Shown or not shown
    Dim i As String
    Shared SelectedIP As String
    'Dim newIP As String 'IP of the client that was JUST added
    Dim byeIP As String

    'TreeStuff
    Sub treeAddNode(ByVal ip As String, ByVal chkstate As CheckState)
        Dim node As TreeNode = New TreeNode(ip)
        node.Expand()
        'treeview change checkbox icon
        If chkstate = CheckState.Checked Then
            node.ImageIndex = 1
            node.SelectedImageIndex = 1
        Else
            node.ImageIndex = 0
            node.SelectedImageIndex = 0
        End If

        TreeView1.Nodes(0).Nodes.Add(node)
        TreeView1.ExpandAll()
    End Sub
    Public Function treeCountChildNodes() As Integer
        Dim intCount As Integer = 0
        For Each objNode In TreeView1.Nodes
            If objNode.Parent Is Nothing = False Then
                intCount += 1
            End If
        Next
        Return intCount
    End Function
    Sub treeReset()
        TreeView1.Nodes.Clear()
        Dim root = New TreeNode("Computers")
        root.Expand()
        TreeView1.Nodes.Add(root)
    End Sub
    Private Sub treeNodeOn(node As TreeNode)
        node.ImageIndex = 1
        node.SelectedImageIndex = 1
        btnRefresh.PerformClick()
    End Sub
    Private Sub treeNodeOff(node As TreeNode)
        node.ImageIndex = 0
        node.SelectedImageIndex = 0
        btnRefresh.PerformClick()
    End Sub
    Function treeNodeIsOn(node As TreeNode) As Boolean
        If node.ImageIndex = 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub TreeView1_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterCollapse
        TreeView1.ExpandAll()
    End Sub
    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        If ActiveTab = 3 Then
            Tab3.ResetScreen()
        End If
        If e.Node.Parent Is Nothing = True Then
            SelectIP("")
        Else
            SelectIP(e.Node.Text)
        End If
    End Sub
    Private Sub TreeView1_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If e.Node.IsSelected = True Then
            If e.Node.Parent Is Nothing = True Then
                SelectIP("")
            Else
                SelectIP(e.Node.Text)
                If treeNodeIsOn(e.Node) = True Then
                    treeNodeOff(e.Node)
                ElseIf treeNodeIsOn(e.Node) = False Then
                    treeNodeOn(e.Node)
                End If
            End If
        End If
    End Sub
    Sub SortTreeView()
        Dim nodes As New List(Of TreeNode)
        ' Collect all child nodes
        For Each node As TreeNode In TreeView1.Nodes(0).Nodes
            nodes.Add(node)
        Next
        ' Sort nodes based on IP address
        nodes.Sort(Function(node1, node2)
                       Return CompareIPAddresses(node1.Text, node2.Text)
                   End Function)
        TreeView1.Nodes(0).Nodes.Clear()
        For Each node As TreeNode In nodes
            TreeView1.Nodes(0).Nodes.Add(node)
        Next
    End Sub
    Private Function CompareIPAddresses(ip1 As String, ip2 As String) As Integer
        Dim segments1 As String() = ip1.Split("."c)
        Dim segments2 As String() = ip2.Split("."c)
        For i As Integer = 0 To Math.Min(segments1.Length - 1, segments2.Length - 1)
            Dim segment1 As Integer = Integer.Parse(segments1(i))
            Dim segment2 As Integer = Integer.Parse(segments2(i))
            If segment1 < segment2 Then
                Return -1
            ElseIf segment1 > segment2 Then
                Return 1
            End If
        Next
        Return segments1.Length.CompareTo(segments2.Length)
    End Function
    Private Sub SelectIP(ByVal Text As String)
        SelectedIP = Text
        If String.IsNullOrEmpty(Text) Then
            lblSelectedIP.Text = "(None)"
        Else
            lblSelectedIP.Text = Text
        End If
    End Sub

    Sub ReloadIP()
        treeReset()
        Dim objStreamReader As StreamReader = Nothing
        Try
            objStreamReader = New StreamReader(aPath)
            Dim strLine As String = objStreamReader.ReadLine()
            Do While strLine IsNot Nothing
                If Not String.IsNullOrWhiteSpace(strLine) Then
                    treeAddNode(strLine, CheckState.Indeterminate)
                End If
                strLine = objStreamReader.ReadLine()
            Loop
            objStreamReader.Close()
        Catch ex As Exception
            If objStreamReader IsNot Nothing Then
                objStreamReader.Close()
            End If
            MsgBox("Error reading from file: " & ex.Message, MsgBoxStyle.Exclamation, "File Read Error")
            Exit Sub
        Finally
            If objStreamReader IsNot Nothing Then
                objStreamReader.Close()
            End If
        End Try
        SortTreeView()
        If treeCountChildNodes() <= 0 Then
            For Each parentNode As TreeNode In TreeView1.Nodes
                For Each Node As TreeNode In parentNode.Nodes
                    If treeNodeIsOn(Node) Then
                        al.Add(Node.Text)
                    End If
                Next
            Next
        End If
    End Sub
    Private Sub ApplyIP()
        al.Clear() 'ip
        al2.Clear() 'shown or not shown
        For Each parentNode In TreeView1.Nodes
            For Each Node In parentNode.Nodes
                If Node.Parent Is Nothing = False Then
                    al.Add(Node.Text)
                    If treeNodeIsOn(Node) Then
                        al2.Add(True)
                    Else
                        al2.Add(False)
                    End If
                End If
            Next
        Next
        Dim num As Int16 = 0
        If ActiveTab = 0 Or ActiveTab = 3 Then
            For Each ChildForm As Form1 In Me.MdiChildren.OfType(Of Form1)()
                If al.Count = num Then
                    ChildForm.IP = ""
                    ChildForm.IsShown = False
                Else
                    ChildForm.IP = al(num)
                    If al2(num) = True Then
                        ChildForm.IsShown = True
                    ElseIf al2(num) = False Then
                        ChildForm.IsShown = False
                    End If
                    num += 1
                End If
            Next
        End If
    End Sub
    Private Sub InitializeForms() 'Initialize the forms
        CloseAllReal()
        Dim a As Int16
        Dim y As Int16 = -250
        m_ChildFormNumber = 0
        ' form1
        Dim ChildForm1 As New Form1
        ChildForm1.MdiParent = Me
        m_ChildFormNumber += 1
        ChildForm1.ID = m_ChildFormNumber
        ChildForm1.PictureBox1.BackColor = Color.Empty
        Dim Label1 As String = "COM" + m_ChildFormNumber.ToString.PadLeft(3, "0")
        ChildForm1.lblIP.Text = ChildForm1.IP + "(" + ChildForm1.Name + ")"
        ChildForm1.Text = Label1

        ' form2
        Dim ChildForm2 As New Form1
        ChildForm2.MdiParent = Me
        m_ChildFormNumber += 1
        ChildForm2.ID = m_ChildFormNumber
        ChildForm2.PictureBox1.BackColor = Color.Empty
        Dim Label2 As String = "COM" + m_ChildFormNumber.ToString.PadLeft(3, "0")
        ChildForm2.lblIP.Text = ChildForm2.IP + "(" + ChildForm2.Name + ")"
        ChildForm2.Text = Label2

        ' form3
        Dim ChildForm3 As New Form1
        ChildForm3.MdiParent = Me
        m_ChildFormNumber += 1
        ChildForm3.ID = m_ChildFormNumber
        ChildForm3.PictureBox1.BackColor = Color.Empty
        Dim Label3 As String = "COM" + m_ChildFormNumber.ToString.PadLeft(3, "0")
        ChildForm3.lblIP.Text = ChildForm3.IP + "(" + ChildForm3.Name + ")"
        ChildForm3.Text = Label3

        ' form4
        Dim ChildForm4 As New Form1
        ChildForm4.MdiParent = Me
        m_ChildFormNumber += 1
        ChildForm4.ID = m_ChildFormNumber
        ChildForm4.PictureBox1.BackColor = Color.Empty
        Dim Label4 As String = "COM" + m_ChildFormNumber.ToString.PadLeft(3, "0")
        ChildForm4.lblIP.Text = ChildForm4.IP + "(" + ChildForm4.Name + ")"
        ChildForm4.Text = Label4

        ' form5
        Dim ChildForm5 As New Form1
        ChildForm5.MdiParent = Me
        m_ChildFormNumber += 1
        ChildForm5.ID = m_ChildFormNumber
        ChildForm5.PictureBox1.BackColor = Color.Empty
        Dim Label5 As String = "COM" + m_ChildFormNumber.ToString.PadLeft(3, "0")
        ChildForm5.lblIP.Text = ChildForm5.IP + "(" + ChildForm5.Name + ")"
        ChildForm5.Text = Label5
    End Sub

    Private Sub ShowAllForms()
        Dim enabledpath = Path.Combine(Application.StartupPath, "Images", "enable.jpg")
        Dim disabledpath = Path.Combine(Application.StartupPath, "Images", "disable.jpg")
        For Each ChildForm As Form1 In Me.MdiChildren.OfType(Of Form1)()
            ChildForm.PictureBox1.Image = Image.FromFile(enabledpath)
            If ChildForm.IP = "" Then
                ChildForm.lblIP.Text = "No IP (" + ChildForm.Name + ")"
            ElseIf ChildForm.IsShown = False Then
                ChildForm.lblIP.Text = ChildForm.IP + " (" + ChildForm.Name + ") (Disconnected)"
            Else
                ChildForm.PictureBox1.Image = Image.FromFile(disabledpath)
                ChildForm.lblIP.Text = ChildForm.IP + " (" + ChildForm.Name + ") (Disconnected)"
            End If
            ChildForm.Online = False
            ChildForm.Show()
        Next ChildForm
        SetPositions()
    End Sub

    Private Sub ShowConnectedForms()
        Dim enabledpath = Path.Combine(Application.StartupPath, "Images", "enable.jpg")
        Dim disabledpath = Path.Combine(Application.StartupPath, "Images", "disable.jpg")
        Dim num As Int16 = 0
        For Each ChildForm As Form1 In Me.MdiChildren.OfType(Of Form1)()
            ChildForm.PictureBox1.Image = Image.FromFile(enabledpath)

            ChildForm.lblIP.Text = ChildForm.IP + " (" + ChildForm.Name + ") (Disconnected)"
            If al.Count = num Then
                ChildForm.IP = ""
                ChildForm.Hide()
            Else
                ChildForm.IP = al(num)
                If al2(num) = True Then
                    ChildForm.Show()
                ElseIf al2(num) = False Then
                    ChildForm.Hide()
                End If
                num += 1
            End If
        Next
        SetPositions()
    End Sub
    Private Sub ShowAllButton_Click(sender As Object, e As EventArgs) Handles ShowAllFormButton.Click, NewToolStripMenuItem.Click
        If chkShowAll.Checked Then
            ApplyIP()
            ShowAllForms()
        Else
            chkShowAll.Checked = True
        End If
    End Sub
    Private Sub RefreshAction1()
        ApplyIP()
        If ActiveTab = 0 Then
        Else
            Exit Sub
        End If
        If chkShowAll.Checked Then
            ShowAllForms()
        Else
            ShowConnectedForms()
        End If
    End Sub
    Private Sub CloseAll1() 'Hides
        For Each ChildForm As Form1 In Me.MdiChildren.OfType(Of Form1)()
            ChildForm.Hide()
        Next ChildForm
    End Sub
    Private Sub MoveAll() 'Just moves it somewhere
        For Each ChildForm As Form1 In Me.MdiChildren.OfType(Of Form1)()
            ChildForm.Height = 2
            Dim myPoint As New Point(10, 20)
            ChildForm.Location = myPoint
        Next ChildForm
    End Sub
    Private Sub CloseAllReal() 'Disposes
        For Each ChildForm As Form1 In Me.MdiChildren.OfType(Of Form1)()
            ChildForm.Close()
            ChildForm.Dispose()
        Next ChildForm
    End Sub
    Private Sub SetPositions()
        If ActiveTab <> 0 Then Exit Sub
        ApplyIP()

        Dim parentWidth As Double = Me.Size.Width
        Dim rowCount As Int16 = 0
        Dim columns As Int16 = 0
        Dim sidePanelWidth As Double = LeftSidePanel.Size.Width
        Dim childWidth As Double = parentWidth / 6
        Dim childHeight As Double = Form1.Size.Height

        Dim xGap As Double = 0.03125 * childWidth
        Dim yGap As Double = xGap

        If ColumnCount = 0 Then
            columns = parentWidth / (400 + yGap)
        Else
            columns = ColumnCount
        End If
        yGap = 10
        xGap = 10

        Dim childAreaWidth As Double = parentWidth - sidePanelWidth - (9.6 * columns + 29.4)
        childWidth = childAreaWidth / columns
        childHeight = childWidth * (12 / 16)

        Dim xMargin As Double = 0
        Dim yMargin As Double = 0

        Dim sideDistance As Double = childWidth + xGap
        Dim bottomDistance As Double = childHeight + yGap

        Dim xPos As Double = xMargin
        Dim yPos As Double = yMargin - childHeight - yGap
        Dim index As Double = 0

        Try
            For Each childForm As Form1 In Me.MdiChildren.OfType(Of Form1)()
                childForm.Width = childWidth
                childForm.Height = childHeight

                If childForm.Visible Then
                    index += 1
                    If columns = 1 OrElse index Mod columns = 1 Then
                        ' Move to the next row
                        xPos = xMargin
                        yPos += bottomDistance
                        rowCount += 1
                    Else
                        xPos += sideDistance
                    End If
                    childForm.Location = New Point(xPos, yPos)
                End If
            Next
        Catch ex As Exception
        End Try

    End Sub
    Private ColumnCount As Int16 = 0
    Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRowAmount.SelectedIndexChanged
        If cboRowAmount.Focused = True Then
            If cboRowAmount.SelectedIndex = 0 Then 'Auto
                ColumnCount = 0
                SetPositions()
            Else
                ColumnCount = cboRowAmount.Text    'SetRow
                SetPositions()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        RefreshAction1()
    End Sub

    'Server--------------------------------------------------------------------------------
    Dim mReader As BinaryReader
    Dim mWriter As BinaryWriter = Nothing
    Const ListenPort As Int16 = 9876
    Const RequestPort As Int16 = 6789
    Const RequestPort2 As Int16 = 6800
    Shared NoofClients As Int16 = 0 'Stores the Number of Image Sender Connected
    Private Sub mServer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        System.Environment.Exit(System.Environment.ExitCode) 'Informs the Senders About the Departure
        End
    End Sub
    Private Sub mServer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cboRowAmount.SelectedIndex = 0
        btnStart.PerformClick()
        PrepareTabs()
        Dim myImageList = New ImageList()
        myImageList.Images.Add(Image.FromFile(offPath))
        myImageList.Images.Add(Image.FromFile(onPath))
        myImageList.Images.Add(Image.FromFile(nothingPath))
        TreeView1.ImageList = myImageList
        TreeView1.Nodes(0).ImageIndex = 1
        TreeView1.Nodes(0).SelectedImageIndex = 1
        TreeView1.ExpandAll()
    End Sub

    Private Sub btnStart_Click_1(sender As Object, e As EventArgs) Handles btnStart.Click
        InitializeForms()
        btnStart.Enabled = False
        Text = "LAN Monitoring System - Receiver Started.."

        ' Start the thread to listen for all incoming connections
        Dim ListenThread As New Thread(New ThreadStart(AddressOf ListenAlways))
        ListenThread.IsBackground = True ' Ensure the thread terminates when the main application closes
        ListenThread.Start()

        ReloadIP()
        ' Select the first IP in lstShowClients if available
        If lstShowClients.Items.Count > 0 Then
            SelectIP(lstShowClients.Items(0).ToString)
        End If
    End Sub

    ''' <summary>
    ''' Listens For All the Incomming Connections,And Store there IpAddress in the ListBox
    ''' </summary>
    ''' <remarks></remarks>
    Sub ListenAlways()
        ' Listen for incoming connections
        Dim MyIp As IPAddress = Dns.GetHostAddresses("192.168.1.10")(0) ' Server IP address
        Dim MyListener As New TcpListener(MyIp, ListenPort)
        MyListener.Start()
        Try
            While True
                Application.DoEvents()
                Dim TempClient As TcpClient = MyListener.AcceptTcpClient()
                ' Accept the client
                NoofClients += 1
                Dim newIP As String = CType(TempClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString()
                AddToListBox(newIP)
                ' Store its address in the list box with a delegate
                CheckForm(newIP)
                ' Activates the address in the checked listbox
                TempClient.Close()
            End While
        Catch ex As Exception
            MsgBox("Error when accepting client connection: " & ex.Message)
        Finally
            MyListener.Stop()
        End Try
    End Sub
    Public Delegate Sub AddItemDelegate(ByVal AddThis As String)

    Private Sub CheckForm(ByVal ipParam As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() CheckForm(ipParam))
        Else
            Dim i As String = 0
            For Each parentNode As TreeNode In TreeView1.Nodes
                For Each node As TreeNode In parentNode.Nodes
                    If node.Parent IsNot Nothing AndAlso node.Text = ipParam Then
                        treeNodeOn(node) ' Check that item
                        Exit For
                    End If
                Next
            Next
            btnRefresh.PerformClick()
        End If
    End Sub

    Sub disableForm(ByVal IP As String)  'Checks form at list box when connects
        byeIP = IP
        For Each parentNode As TreeNode In TreeView1.Nodes
            For Each node As TreeNode In parentNode.Nodes
                If node.Parent IsNot Nothing AndAlso node.Text = byeIP Then
                    treeNodeOff(node)
                    Exit For
                End If
            Next
        Next
    End Sub

    Sub AddToListBox(ByVal AddThis As String)
        ' A delegate to store the address of the incoming connection in a listbox
        If lstShowClients.InvokeRequired Then
            Dim NewAdd As New AddItemDelegate(AddressOf AddToListBox)
            lstShowClients.Invoke(NewAdd, New Object() {AddThis})
        Else
            lstShowClients.Items.Add(AddThis)
        End If
    End Sub

    Function IsClientConnected(ByVal client As TcpClient) As Boolean
        Try
            ' Poll the socket to see if it's still connected
            If client.Client.Poll(0, SelectMode.SelectRead) AndAlso client.Client.Available = 0 Then
                ' If both checks pass, we're disconnected
                Return False
            Else
                Return True
            End If
        Catch ex As SocketException
            ' An exception is thrown if the client is disconnected
            Return False
        End Try
    End Function

    Sub showImage()
        Dim active As Boolean = True
        Dim disable As Boolean = False
        Dim idx As Integer = 0
        Dim item As Object
        Dim term As String

        ' Remove duplicates from the list
        Dim uniqueClients = lstShowClients.Items.Cast(Of Object).Distinct().ToList()
        lstShowClients.Items.Clear()
        For Each uniqueItem In uniqueClients
            lstShowClients.Items.Add(uniqueItem)
        Next

        If lstShowClients.Items.Count <= 0 Then Exit Sub 'Quit if there nothing

        For Each item In lstShowClients.Items
            term = item.ToString
            Try
                Dim FilName As String = Path.Combine(Environment.CurrentDirectory, "Mohy_Screen_shot.jpeg")
                Using fStream As New FileStream(FilName, FileMode.Create)
                    ' Creates the File Where we are going to receive the File From the Sender
                    Dim ImageSenderAddress As IPAddress = Dns.GetHostAddresses(term)(0)
                    ' Gets the IPAddress Where I have to Connect

                    Dim ClientToSee As TcpClient
                    Try
                        ClientToSee = New TcpClient()
                        ClientToSee.Connect(ImageSenderAddress, RequestPort)
                        ' Connects to the Image Sender
                    Catch ex As Exception   'if fails to connect
                        lstShowClients.Items.RemoveAt(idx)
                        disableForm(term)       'remove from connected ips list
                        btnRefresh.PerformClick()
                        NoofClients -= 1        'this counter isnt used for anything
                        Exit Sub ' if crashed go back to exit sub
                    End Try
                    ' If ActiveTab = 0 Or ActiveTab = 3 Then
                    Using ClientToSee
                        Dim NStream As NetworkStream = ClientToSee.GetStream()
                        Using mReader As New BinaryReader(NStream)
                            Dim buffer(1024 - 1) As Byte
                            Do
                                Dim bytesRead As Integer = mReader.Read(buffer, 0, buffer.Length)
                                If bytesRead = 0 Then Exit Do
                                fStream.Write(buffer, 0, bytesRead)
                                fStream.Flush()
                            Loop
                        End Using
                    End Using

                    '  Else
                    '  active = False
                    '  End If
                    ' Gets the Screen Shot and Closes the Stream
                    fStream.Close()
                End Using

                ' Finally Showing the Screen Shot in the Picture Box
                If active Then
                    Using fs As New FileStream(FilName, FileMode.Open, FileAccess.Read)
                        Dim imgCurrentPhoto As Image = Image.FromStream(fs)

                        For Each ChildForm As Form1 In Me.MdiChildren.OfType(Of Form1)()

                            If ActiveTab = 3 AndAlso getSelectedIP() <> ChildForm.IP Then
                                Continue For
                            End If
                            If ChildForm.IsShown AndAlso ChildForm.IP = term Then
                                ChildForm.Text = "hello"
                                If disable Then
                                    ChildForm.PictureBox1.Image = Nothing
                                    ChildForm.PictureBox1.BackColor = Color.Empty
                                    ChildForm.lblIP.Text = "Loading"
                                Else

                                    ChildForm.PictureBox1.Image = imgCurrentPhoto
                                    If ActiveTab = 3 Then
                                        Tab3.ViewDesktop.Image = ChildForm.PictureBox1.Image
                                    End If
                                End If
                                ChildForm.lblIP.Text = $"{ChildForm.IP} ({ChildForm.Name})"
                            End If
                        Next

                    End Using
                End If
                idx += 1
            Catch ex As Exception
                Console.WriteLine($"Error processing Image of {term}: {ex.Message}")
            End Try
        Next
    End Sub

    Sub showProcess(ByVal isManual As Boolean)   'request proceses from client
        If ActiveTab = 4 Then
            Dim term As String = getSelectedIP()
            ' Check if there are items in the list
            If isConnected(getSelectedIP()) Then 'Verify that is a connected IP
                Try
                    Dim SenderAddress As IPAddress = Dns.GetHostAddresses(term)(0)
                    ' Gets the IPAddress Where I have to Connect

                    Using ClientToSee As New TcpClient()
                        Try
                            ClientToSee.Connect(SenderAddress, RequestPort2)
                            ' Connects to the App Data Sender
                        Catch ex As Exception
                            Console.WriteLine($"Error connecting to {term}: {ex.Message}")
                            Exit Sub ' Exit if connection fails
                        End Try

                        If Tab4.IsAutoRefresh Or isManual Then
                            Using NStream As NetworkStream = ClientToSee.GetStream()
                                ' Receives the application data
                                ReceiveApplicationData(NStream)
                            End Using
                        End If

                        ClientToSee.Close()
                    End Using
                Catch ex As Exception
                    ' Handle exception (optional)
                    Console.WriteLine($"Error processing Application Data of {term}: {ex.Message}")
                End Try
            Else
                Tab4.DisableAutoRefresh()
                Tab4.DGVClear()
            End If
        End If
    End Sub

    Private Sub ReceiveApplicationData(netStream As NetworkStream)
        ' Buffer to store application data
        Dim buffer(1024) As Byte
        Dim completeMessage As New StringBuilder()
        Dim bytesRead As Integer

        Try
            Do
                bytesRead = netStream.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then Exit Do
                completeMessage.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead))
            Loop While netStream.DataAvailable
        Catch ex As Exception
            ' Handle potential read exceptions
            Console.WriteLine("Error reading from network stream: " & ex.Message)
        End Try

        ' Display received data in DataGridView
        Dim data As String = completeMessage.ToString()
        Tab4.PopulateDataGridView(data)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        btnShowImage.PerformClick()
    End Sub
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Tab4.IsAutoRefresh Then
            Tab4.btnRefresh_Click()
        End If
    End Sub


    Function getSelectedIP() As String
        If Not String.IsNullOrEmpty(SelectedIP) Then
            Return SelectedIP
        Else
            Return ""
        End If
    End Function

    Private Sub btnShowImage_Click_1(sender As Object, e As EventArgs) Handles btnShowImage.Click
        showImage()
    End Sub

    Private Sub WindowResized(sender As Object, e As EventArgs) Handles MyBase.Resize
        SetPositions()

    End Sub

    Private Sub chkShowAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowAll.CheckedChanged
        RefreshAction1()
    End Sub


    Private Sub CloseAllToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If chkShowAll.Checked Then
            chkShowAll.Checked = False
        Else
            CloseAll1()
        End If
    End Sub

    'Show or Only Connected Computer
    Private Sub Connected_Click(sender As Object, e As EventArgs) Handles Connected.Click
        chkShowAll.Checked = False 'unchecks the Hidden Checkbox thing
        RefreshAction1()
    End Sub
    'Show or Only Connected Computer
    Private Sub All_Click(sender As Object, e As EventArgs) Handles All.Click
        chkShowAll.Checked = True 'Checks the Hidden Checkbox thing
        RefreshAction1()
    End Sub

    Dim IPListIsExtended As Boolean = False

    Dim Tab3 As DesktopControl = New DesktopControl(Me)
    Dim Tab4 As ManageProcess = New ManageProcess(Me)
    Private Sub PrepareTabs()
        '   Set MDI parent And dock style for each tab
        Dim tabs() As Form = {Tab3, Tab4}
        For Each tab In tabs
            tab.MdiParent = Me
            tab.Dock = DockStyle.Fill
        Next
    End Sub
    Private Sub DisableTab(ByVal num As Int16)
        ' Dim buttons() As System.Windows.Forms.Button = {Button1, Button2, Button3, Button4, Button5}
        Select Case num
            Case 0
                MoveAll()
                ToolStripforTab0(False)
                ' lblSelectedIPlbl.Visible = False
                'lblSelectedIP.Visible = False
            Case 1
            Case 2
            Case 3
                Tab3.DisableControl() 'disable receiving input
                ToolStripforTab3(False)
                Tab3.Hide()
            Case 4
                Tab4.DisableAutoRefresh() 'disable auto refresh of app data

                ToolStripforTab4(False)
                Tab4.Hide()
        End Select
        ' buttons(num).Enabled = True
    End Sub

    Sub ExtendSideBar(ByVal bool As Boolean)
        Dim shift As Integer = 260
        If bool Then
            If IPListIsExtended = True Then
                lblSelectedIPlbl.Location = New Point(lblSelectedIPlbl.Location.X, lblSelectedIPlbl.Location.Y - shift)
                lblSelectedIP.Location = New Point(lblSelectedIP.Location.X, lblSelectedIP.Location.Y - shift)
                TreeView1.Height -= shift
                IPListIsExtended = False
                Label2.Visible = True
                lstShowClients.Visible = True
            End If
        Else
            If IPListIsExtended = False Then
                lblSelectedIPlbl.Location = New Point(lblSelectedIPlbl.Location.X, lblSelectedIPlbl.Location.Y + shift)
                lblSelectedIP.Location = New Point(lblSelectedIP.Location.X, lblSelectedIP.Location.Y + shift)
                TreeView1.Height += shift
                IPListIsExtended = True
                Label2.Visible = False
                lstShowClients.Visible = False
            End If
        End If
    End Sub
    Sub EnabkeSelectedIPLabel(ByVal bool As Boolean)
        lblSelectedIPlbl.Visible = bool
        lblSelectedIP.Visible = bool
    End Sub

    Private Sub EnableTab(ByVal num As Int16)
        ' Dim buttons() As System.Windows.Forms.Button = {Button1, Button2, Button3, Button4, Button5}
        If ActiveTab = num Then Exit Sub
        ActiveTab = num
        Select Case num
            Case 0

                Timer1.Interval = 500   '2fps

                ToolStripforTab0(True)
                EnabkeSelectedIPLabel(True)

                ExtendSideBar(True)
                btnRefresh.PerformClick()
            Case 1
            Case 2
            Case 3
                ToolStripforTab3(True)
                Timer1.Interval = 71   '14 fps
                ExtendSideBar(True)
                Tab3.Show()
            Case 4
                ToolStripforTab4(True)
                ExtendSideBar(True)
                Timer1.Interval = 4000
                Tab4.Show()
        End Select
        'buttons(num).Enabled = False
        DisableAll(num)
    End Sub
    Private Sub DisableAll(ByVal num As Int16)
        For index As Integer = 0 To 4
            If Not index = num Then
                DisableTab(index)
            End If
        Next
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        EnableTab(0) 'View Desktop Screens
    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs)
        EnableTab(3) 'Control
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs)
        EnableTab(4) 'Manage Process
    End Sub

    Sub ToolStripforTab0(ByVal bool As Boolean)
        ShowAllFormButton.Visible = bool
        ToolStripSeparator2.Visible = bool
        View.Visible = bool
        cboRowAmount.Visible = bool
        ToolStripDropDownButton1.Visible = bool '
        ToolStripSeparator1.Visible = bool
    End Sub
    Sub ToolStripforTab4(ByVal bool As Boolean)
        ToolStripSeparator6.Visible = bool
        AutoRefreshButton.Visible = bool
        ToolStripSeparator5.Visible = bool
        RefreshButton.Visible = bool
        ToolStripSeparator7.Visible = bool
        ToolStripButton1.Visible = bool
    End Sub
    Sub ToolStripforTab3(ByVal bool As Boolean)
        ToolStripSeparator9.Visible = bool
        ControlButton.Visible = bool
    End Sub


    Sub SendMessage(ByVal IPList As ArrayList, ByVal Message As String)

        For Each IP As String In IPList
            SendMessage(IP, Message)
        Next
    End Sub

    Sub SendMessage(ByVal IP As String, ByVal Message As String)
        If String.IsNullOrWhiteSpace(IP) Then Exit Sub

        Try
            Dim msg As Byte() = System.Text.Encoding.ASCII.GetBytes(Message)
            Using clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(IP, 1977)

                Dim bytesSent As Integer = clsSocket.Send(msg, 0, msg.Length, SocketFlags.None)
                If bytesSent = msg.Length Then
                    ' MessageBox.Show(Me, "Message sent!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The message was not sent successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        Catch ex As SocketException
            MessageBox.Show(Me, $"Socket error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Function IsApplicationFocused() As Boolean
        Dim foregroundWindow As IntPtr = GetForegroundWindow()
        Dim foregroundProcessId As Integer = 0
        GetWindowThreadProcessId(foregroundWindow, foregroundProcessId)

        Dim currentProcessId As Integer = Process.GetCurrentProcess().Id
        Return foregroundProcessId = currentProcessId
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetWindowThreadProcessId(ByVal hWnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer
    End Function



    Private Sub AutoRefreshButton_Click(sender As Object, e As EventArgs) Handles AutoRefreshButton.Click
        Tab4.AutoRefreshButton_Click()
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        Tab4.btnRefresh_Click()
    End Sub
    Private Sub ControlButton_Click(sender As Object, e As EventArgs) Handles ControlButton.Click
        Tab3.ControlButton_Click()
    End Sub
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Tab4.btnKillTask_Click()
    End Sub

    Function isConnected(ByVal Ip As String) As Boolean
        If String.IsNullOrWhiteSpace(Ip) Then
            MessageBox.Show(Me, "No Computer Selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If lstShowClients.Items.Contains(Ip) Then
                Return True
            Else
                MessageBox.Show(Me, "Selected computer not connected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        End If
        Return False
    End Function

    Sub SendMsgByIP()
        Try
            Dim clsError As System.Net.Sockets.SocketError
            Dim Mensahe As String
            Mensahe = InputBox("Please type your message here.", "Message")
            If Mensahe = "" Then
                MsgBox("Please enter a message, then try again", 48, "Invalid")
                Exit Sub
            End If

            ' Send message to the IP address in the label lbgSelectedIP
            Dim selectedIP As String = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Dim bMessage As Byte() = System.Text.Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    MessageBox.Show(Me, "Message sent!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The message was not sent successfully, the message was: " & clsError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            ' Loop through the checked nodes in TreeView1
            For Each node As TreeNode In TreeView1.Nodes
                If node.Checked Then
                    Try
                        Dim bMessage As Byte() = System.Text.Encoding.ASCII.GetBytes(Mensahe)
                        Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        clsSocket.Connect(node.Text, 1977) ' Assuming node.Text holds the IP address
                        clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                        If clsError = SocketError.Success Then
                            MessageBox.Show(Me, "Message sent to " & node.Text & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show(Me, "The message was not sent successfully to " & node.Text & ", the message was: " & clsError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                        clsSocket.Close()
                    Catch ex As Exception
                        MsgBox("Error sending message to " & node.Text & ": " & ex.Message)
                    End Try
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        SendMsgByIP()
    End Sub



    Private Sub Button7_Click(sender As Object, e As EventArgs)
        Try
            Dim clsError As SocketError
            Dim Mensahe = "logoff"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    '    MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CALCULATORToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CALCULATORToolStripMenuItem.Click
        Try
            Dim clsError As SocketError
            Dim Mensahe = "calculator"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    '   MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs)
        Try
            Dim clsError As SocketError
            Dim Mensahe = "restart"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    '   MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs)
        Try
            Dim clsError As SocketError
            Dim Mensahe = "shutdown"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    ' MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DLSAUWebsiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DLSAUWebsiteToolStripMenuItem.Click
        Try
            Dim clsError As SocketError
            Dim Mensahe = "dlsauwebsite"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    '  MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    'MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub NotePadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NotePadToolStripMenuItem.Click
        Try
            Dim clsError As SocketError
            Dim Mensahe = "notepad"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    ' MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SENDTOSELECTEDIPToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SENDTOSELECTEDIPToolStripMenuItem.Click
        SendMsgByIP()
    End Sub

    Private Sub SENDTOALLToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Try
            Dim clsError As SocketError
            Dim Mensahe = InputBox("Please type your message here.", "Message")
            If String.IsNullOrEmpty(Mensahe) Then
                MsgBox("Please enter a message, then try again.", 48, "Invalid")
                Exit Sub
            End If

            ' Convert message to byte array
            Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)

            ' Loop through all nodes in the TreeView
            For Each node As TreeNode In TreeView1.Nodes(0).Nodes
                ' Check if the node is active based on its icon index
                If treeNodeIsOn(node) Then
                    Try
                        Dim nodeIP = node.Text ' Retrieve the IP address from the node's Text property
                        Using clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                            clsSocket.Connect(nodeIP, 1977)
                            clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                            If clsError = SocketError.Success Then
                                MessageBox.Show(Me, "Message sent to " & nodeIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show(Me, "The message was not sent successfully to " & nodeIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End Using
                    Catch ex As Exception
                        MsgBox("Error sending message to " & node.Text & ": " & ex.Message)
                    End Try
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub




    Private Sub RestartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartToolStripMenuItem.Click
        Try
            Dim clsError As SocketError
            Dim Mensahe = "restart"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    '   MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LogOffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogOffToolStripMenuItem.Click
        Try
            Dim clsError As SocketError
            Dim Mensahe = "logoff"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    '    MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ShutdownToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShutdownToolStripMenuItem.Click
        Try
            Dim clsError As SocketError
            Dim Mensahe = "shutdown"

            ' Get the selected IP address from lblSelectedIP
            Dim selectedIP = lblSelectedIP.Text
            If String.IsNullOrEmpty(selectedIP) Then
                MsgBox("Please select a valid IP address.", 48, "Invalid")
                Exit Sub
            End If

            Try
                Console.WriteLine("Logging off: " & selectedIP)
                Dim bMessage = Encoding.ASCII.GetBytes(Mensahe)
                Dim clsSocket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                clsSocket.Connect(selectedIP, 1977)
                clsSocket.Send(bMessage, 0, bMessage.Length, SocketFlags.None, clsError)
                If clsError = SocketError.Success Then
                    ' MessageBox.Show(Me, "Logoff command sent to " & selectedIP & "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(Me, "The logoff command was not sent successfully to " & selectedIP & ", error: " & clsError.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                clsSocket.Close()
            Catch ex As Exception
                MsgBox("Error sending logoff command to " & selectedIP & ": " & ex.Message)
            End Try

        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DISPLAYPCsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DISPLAYPCsToolStripMenuItem.Click
        EnableTab(0) 'View Desktop Screens
    End Sub

    Private Sub MOUSECONTROLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MOUSECONTROLToolStripMenuItem.Click
        EnableTab(3) 'Control
    End Sub

    Private Sub VIEWPROCESSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VIEWPROCESSToolStripMenuItem.Click
        EnableTab(4) 'Manage Process
    End Sub

    Sub StreamScreenContinuously()
        While isSharingScreen
            Try
                ' Capture screen
                Dim bmp As New Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
                Using g As Graphics = Graphics.FromImage(bmp)
                    g.CopyFromScreen(0, 0, 0, 0, bmp.Size)
                End Using

                ' Convert to byte array
                Dim ms As New MemoryStream()
                bmp.Save(ms, Imaging.ImageFormat.Jpeg)
                Dim bytes() As Byte = ms.ToArray()
                ms.Close()
                bmp.Dispose()

                ' Loop through all active IPs (those turned ON in the tree)
                For i As Integer = 0 To al.Count - 1
                    If al2(i) = True Then ' Only send to checked/active IPs
                        Dim ip As String = al(i)
                        Try
                            Dim client As New TcpClient(ip, 6790)
                            Dim stream As NetworkStream = client.GetStream()
                            stream.Write(bytes, 0, bytes.Length)
                            stream.Flush()
                            stream.Close()
                            client.Close()
                        Catch innerEx As Exception
                            ' Optional: log or ignore unreachable clients
                            Console.WriteLine($"Failed to send to {ip}: {innerEx.Message}")
                        End Try
                    End If
                Next

            Catch ex As Exception
                Console.WriteLine("Streaming error: " & ex.Message)
            End Try

            Thread.Sleep(1000) ' 1 second delay for streaming loop
        End While
    End Sub



    Private Sub ShareScreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShareScreenToolStripMenuItem.Click
        If Not isSharingScreen Then
            isSharingScreen = True
            Dim streamThread As New Thread(AddressOf StreamScreenContinuously)
            streamThread.IsBackground = True
            streamThread.Start()
            MessageBox.Show("Screen sharing started.")
        Else
            isSharingScreen = False
            MessageBox.Show("Screen sharing stopped.")
        End If
    End Sub
End Class









