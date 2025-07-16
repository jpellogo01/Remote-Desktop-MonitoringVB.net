<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDIParent1
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
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDIParent1))
        MenuStrip = New MenuStrip()
        FileMenu = New ToolStripMenuItem()
        NewToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator3 = New ToolStripSeparator()
        ExitToolStripMenuItem = New ToolStripMenuItem()
        ViewMenu = New ToolStripMenuItem()
        ToolBarToolStripMenuItem = New ToolStripMenuItem()
        StatusBarToolStripMenuItem = New ToolStripMenuItem()
        HelpMenu = New ToolStripMenuItem()
        ToolStripSeparator8 = New ToolStripSeparator()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        OPENAPPToolStripMenuItem = New ToolStripMenuItem()
        CALCULATORToolStripMenuItem = New ToolStripMenuItem()
        DLSAUWebsiteToolStripMenuItem = New ToolStripMenuItem()
        NotePadToolStripMenuItem = New ToolStripMenuItem()
        MSTeamsToolStripMenuItem = New ToolStripMenuItem()
        MSWordToolStripMenuItem = New ToolStripMenuItem()
        MSPowerPointToolStripMenuItem = New ToolStripMenuItem()
        MSExcelToolStripMenuItem = New ToolStripMenuItem()
        SendMessageALLToolStripMenuItem = New ToolStripMenuItem()
        SENDTOSELECTEDIPToolStripMenuItem = New ToolStripMenuItem()
        POWERMENUToolStripMenuItem = New ToolStripMenuItem()
        ShutdownToolStripMenuItem = New ToolStripMenuItem()
        LogOffToolStripMenuItem = New ToolStripMenuItem()
        RestartToolStripMenuItem = New ToolStripMenuItem()
        CONTROLPCToolStripMenuItem = New ToolStripMenuItem()
        MOUSECONTROLToolStripMenuItem = New ToolStripMenuItem()
        VIEWPROCESSToolStripMenuItem = New ToolStripMenuItem()
        DISPLAYPCsToolStripMenuItem = New ToolStripMenuItem()
        StatusStrip = New StatusStrip()
        ToolStripStatusLabel1 = New ToolStripStatusLabel()
        ToolTip = New ToolTip(components)
        LeftSidePanel = New Panel()
        ToolStrip1 = New ToolStrip()
        lblSelectedIP = New Label()
        lblSelectedIPlbl = New Label()
        TreeView1 = New TreeView()
        chkShowAll = New CheckBox()
        Label2 = New Label()
        btnRefresh = New Button()
        btnShowImage = New Button()
        lstShowClients = New ListBox()
        btnStart = New Button()
        Label1 = New Label()
        ToolStrip = New ToolStrip()
        ShowAllFormButton = New ToolStripButton()
        ToolStripSeparator2 = New ToolStripSeparator()
        View = New ToolStripLabel()
        cboRowAmount = New ToolStripComboBox()
        ToolStripSeparator1 = New ToolStripSeparator()
        ToolStripDropDownButton1 = New ToolStripDropDownButton()
        Connected = New ToolStripMenuItem()
        All = New ToolStripMenuItem()
        ToolStripSeparator6 = New ToolStripSeparator()
        AutoRefreshButton = New ToolStripButton()
        ToolStripSeparator5 = New ToolStripSeparator()
        RefreshButton = New ToolStripButton()
        ToolStripSeparator7 = New ToolStripSeparator()
        ToolStripButton1 = New ToolStripButton()
        ToolStripSeparator9 = New ToolStripSeparator()
        ControlButton = New ToolStripButton()
        Timer1 = New Timer(components)
        BindingSource1 = New BindingSource(components)
        Timer2 = New Timer(components)
        Panel1 = New Panel()
        ToolTip1 = New ToolTip(components)
        ShareScreenToolStripMenuItem = New ToolStripMenuItem()
        MenuStrip.SuspendLayout()
        StatusStrip.SuspendLayout()
        LeftSidePanel.SuspendLayout()
        ToolStrip.SuspendLayout()
        CType(BindingSource1, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' MenuStrip
        ' 
        MenuStrip.ImageScalingSize = New Size(20, 20)
        MenuStrip.Items.AddRange(New ToolStripItem() {FileMenu, ViewMenu, HelpMenu, OPENAPPToolStripMenuItem, SendMessageALLToolStripMenuItem, POWERMENUToolStripMenuItem, CONTROLPCToolStripMenuItem})
        MenuStrip.Location = New Point(0, 0)
        MenuStrip.Name = "MenuStrip"
        MenuStrip.Padding = New Padding(8, 3, 0, 3)
        MenuStrip.Size = New Size(1132, 30)
        MenuStrip.TabIndex = 5
        MenuStrip.Text = "MenuStrip"
        ' 
        ' FileMenu
        ' 
        FileMenu.DropDownItems.AddRange(New ToolStripItem() {NewToolStripMenuItem, ToolStripSeparator3, ExitToolStripMenuItem})
        FileMenu.ImageTransparentColor = SystemColors.ActiveBorder
        FileMenu.Name = "FileMenu"
        FileMenu.Size = New Size(46, 24)
        FileMenu.Text = "&File"
        ' 
        ' NewToolStripMenuItem
        ' 
        NewToolStripMenuItem.Image = CType(resources.GetObject("NewToolStripMenuItem.Image"), Image)
        NewToolStripMenuItem.ImageTransparentColor = Color.Black
        NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        NewToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.N
        NewToolStripMenuItem.Size = New Size(175, 26)
        NewToolStripMenuItem.Text = "&New"
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(172, 6)
        ' 
        ' ExitToolStripMenuItem
        ' 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        ExitToolStripMenuItem.Size = New Size(175, 26)
        ExitToolStripMenuItem.Text = "E&xit"
        ' 
        ' ViewMenu
        ' 
        ViewMenu.DropDownItems.AddRange(New ToolStripItem() {ToolBarToolStripMenuItem, StatusBarToolStripMenuItem})
        ViewMenu.Name = "ViewMenu"
        ViewMenu.Size = New Size(55, 24)
        ViewMenu.Text = "&View"
        ' 
        ' ToolBarToolStripMenuItem
        ' 
        ToolBarToolStripMenuItem.Checked = True
        ToolBarToolStripMenuItem.CheckOnClick = True
        ToolBarToolStripMenuItem.CheckState = CheckState.Checked
        ToolBarToolStripMenuItem.Name = "ToolBarToolStripMenuItem"
        ToolBarToolStripMenuItem.Size = New Size(158, 26)
        ToolBarToolStripMenuItem.Text = "&Toolbar"
        ' 
        ' StatusBarToolStripMenuItem
        ' 
        StatusBarToolStripMenuItem.Checked = True
        StatusBarToolStripMenuItem.CheckOnClick = True
        StatusBarToolStripMenuItem.CheckState = CheckState.Checked
        StatusBarToolStripMenuItem.Name = "StatusBarToolStripMenuItem"
        StatusBarToolStripMenuItem.Size = New Size(158, 26)
        StatusBarToolStripMenuItem.Text = "&Status Bar"
        ' 
        ' HelpMenu
        ' 
        HelpMenu.DropDownItems.AddRange(New ToolStripItem() {ToolStripSeparator8, AboutToolStripMenuItem})
        HelpMenu.Name = "HelpMenu"
        HelpMenu.Size = New Size(55, 24)
        HelpMenu.Text = "&Help"
        ' 
        ' ToolStripSeparator8
        ' 
        ToolStripSeparator8.Name = "ToolStripSeparator8"
        ToolStripSeparator8.Size = New Size(143, 6)
        ' 
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(146, 26)
        AboutToolStripMenuItem.Text = "&About ..."
        ' 
        ' OPENAPPToolStripMenuItem
        ' 
        OPENAPPToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {CALCULATORToolStripMenuItem, DLSAUWebsiteToolStripMenuItem, NotePadToolStripMenuItem, MSTeamsToolStripMenuItem, MSWordToolStripMenuItem, MSPowerPointToolStripMenuItem, MSExcelToolStripMenuItem})
        OPENAPPToolStripMenuItem.Name = "OPENAPPToolStripMenuItem"
        OPENAPPToolStripMenuItem.Size = New Size(91, 24)
        OPENAPPToolStripMenuItem.Text = "OPEN APP"
        ' 
        ' CALCULATORToolStripMenuItem
        ' 
        CALCULATORToolStripMenuItem.Name = "CALCULATORToolStripMenuItem"
        CALCULATORToolStripMenuItem.Size = New Size(190, 26)
        CALCULATORToolStripMenuItem.Text = "Calculator"
        ' 
        ' DLSAUWebsiteToolStripMenuItem
        ' 
        DLSAUWebsiteToolStripMenuItem.Name = "DLSAUWebsiteToolStripMenuItem"
        DLSAUWebsiteToolStripMenuItem.Size = New Size(190, 26)
        DLSAUWebsiteToolStripMenuItem.Text = "YOUTUBE"
        ' 
        ' NotePadToolStripMenuItem
        ' 
        NotePadToolStripMenuItem.Name = "NotePadToolStripMenuItem"
        NotePadToolStripMenuItem.Size = New Size(190, 26)
        NotePadToolStripMenuItem.Text = "NotePad"
        ' 
        ' MSTeamsToolStripMenuItem
        ' 
        MSTeamsToolStripMenuItem.Name = "MSTeamsToolStripMenuItem"
        MSTeamsToolStripMenuItem.Size = New Size(190, 26)
        MSTeamsToolStripMenuItem.Text = "MS Teams"
        ' 
        ' MSWordToolStripMenuItem
        ' 
        MSWordToolStripMenuItem.Name = "MSWordToolStripMenuItem"
        MSWordToolStripMenuItem.Size = New Size(190, 26)
        MSWordToolStripMenuItem.Text = "MS Word"
        ' 
        ' MSPowerPointToolStripMenuItem
        ' 
        MSPowerPointToolStripMenuItem.Name = "MSPowerPointToolStripMenuItem"
        MSPowerPointToolStripMenuItem.Size = New Size(190, 26)
        MSPowerPointToolStripMenuItem.Text = "MS PowerPoint"
        ' 
        ' MSExcelToolStripMenuItem
        ' 
        MSExcelToolStripMenuItem.Name = "MSExcelToolStripMenuItem"
        MSExcelToolStripMenuItem.Size = New Size(190, 26)
        MSExcelToolStripMenuItem.Text = "MS Excel"
        ' 
        ' SendMessageALLToolStripMenuItem
        ' 
        SendMessageALLToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SENDTOSELECTEDIPToolStripMenuItem})
        SendMessageALLToolStripMenuItem.Name = "SendMessageALLToolStripMenuItem"
        SendMessageALLToolStripMenuItem.Size = New Size(130, 24)
        SendMessageALLToolStripMenuItem.Text = "SEND MESSAGE"
        ' 
        ' SENDTOSELECTEDIPToolStripMenuItem
        ' 
        SENDTOSELECTEDIPToolStripMenuItem.Name = "SENDTOSELECTEDIPToolStripMenuItem"
        SENDTOSELECTEDIPToolStripMenuItem.Size = New Size(239, 26)
        SENDTOSELECTEDIPToolStripMenuItem.Text = "SEND TO SELECTED IP"
        ' 
        ' POWERMENUToolStripMenuItem
        ' 
        POWERMENUToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {ShutdownToolStripMenuItem, LogOffToolStripMenuItem, RestartToolStripMenuItem})
        POWERMENUToolStripMenuItem.Name = "POWERMENUToolStripMenuItem"
        POWERMENUToolStripMenuItem.Size = New Size(119, 24)
        POWERMENUToolStripMenuItem.Text = "POWER MENU"
        ' 
        ' ShutdownToolStripMenuItem
        ' 
        ShutdownToolStripMenuItem.Name = "ShutdownToolStripMenuItem"
        ShutdownToolStripMenuItem.Size = New Size(158, 26)
        ShutdownToolStripMenuItem.Text = "Shutdown"
        ' 
        ' LogOffToolStripMenuItem
        ' 
        LogOffToolStripMenuItem.Name = "LogOffToolStripMenuItem"
        LogOffToolStripMenuItem.Size = New Size(158, 26)
        LogOffToolStripMenuItem.Text = "Log Off"
        ' 
        ' RestartToolStripMenuItem
        ' 
        RestartToolStripMenuItem.Name = "RestartToolStripMenuItem"
        RestartToolStripMenuItem.Size = New Size(158, 26)
        RestartToolStripMenuItem.Text = "Restart"
        ' 
        ' CONTROLPCToolStripMenuItem
        ' 
        CONTROLPCToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {MOUSECONTROLToolStripMenuItem, VIEWPROCESSToolStripMenuItem, DISPLAYPCsToolStripMenuItem, ShareScreenToolStripMenuItem})
        CONTROLPCToolStripMenuItem.Name = "CONTROLPCToolStripMenuItem"
        CONTROLPCToolStripMenuItem.Size = New Size(110, 24)
        CONTROLPCToolStripMenuItem.Text = "CONTROL PC"
        ' 
        ' MOUSECONTROLToolStripMenuItem
        ' 
        MOUSECONTROLToolStripMenuItem.Name = "MOUSECONTROLToolStripMenuItem"
        MOUSECONTROLToolStripMenuItem.Size = New Size(224, 26)
        MOUSECONTROLToolStripMenuItem.Text = "MOUSE CONTROL"
        ' 
        ' VIEWPROCESSToolStripMenuItem
        ' 
        VIEWPROCESSToolStripMenuItem.Name = "VIEWPROCESSToolStripMenuItem"
        VIEWPROCESSToolStripMenuItem.Size = New Size(224, 26)
        VIEWPROCESSToolStripMenuItem.Text = "VIEW PROCESS"
        ' 
        ' DISPLAYPCsToolStripMenuItem
        ' 
        DISPLAYPCsToolStripMenuItem.Name = "DISPLAYPCsToolStripMenuItem"
        DISPLAYPCsToolStripMenuItem.Size = New Size(224, 26)
        DISPLAYPCsToolStripMenuItem.Text = "DISPLAY PCs"
        ' 
        ' StatusStrip
        ' 
        StatusStrip.ImageScalingSize = New Size(20, 20)
        StatusStrip.Items.AddRange(New ToolStripItem() {ToolStripStatusLabel1})
        StatusStrip.Location = New Point(0, 826)
        StatusStrip.Name = "StatusStrip"
        StatusStrip.Padding = New Padding(1, 0, 19, 0)
        StatusStrip.RightToLeft = RightToLeft.Yes
        StatusStrip.Size = New Size(1132, 26)
        StatusStrip.TabIndex = 7
        StatusStrip.Text = "StatusStrip"
        ' 
        ' ToolStripStatusLabel1
        ' 
        ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        ToolStripStatusLabel1.Size = New Size(101, 20)
        ToolStripStatusLabel1.Text = "                       "
        ' 
        ' LeftSidePanel
        ' 
        LeftSidePanel.BackColor = SystemColors.Control
        LeftSidePanel.Controls.Add(ToolStrip1)
        LeftSidePanel.Controls.Add(lblSelectedIP)
        LeftSidePanel.Controls.Add(lblSelectedIPlbl)
        LeftSidePanel.Controls.Add(TreeView1)
        LeftSidePanel.Controls.Add(chkShowAll)
        LeftSidePanel.Controls.Add(Label2)
        LeftSidePanel.Controls.Add(btnRefresh)
        LeftSidePanel.Controls.Add(btnShowImage)
        LeftSidePanel.Controls.Add(lstShowClients)
        LeftSidePanel.Controls.Add(btnStart)
        LeftSidePanel.Controls.Add(Label1)
        LeftSidePanel.Dock = DockStyle.Left
        LeftSidePanel.Location = New Point(0, 67)
        LeftSidePanel.Name = "LeftSidePanel"
        LeftSidePanel.Size = New Size(176, 759)
        LeftSidePanel.TabIndex = 9
        ' 
        ' ToolStrip1
        ' 
        ToolStrip1.ImageScalingSize = New Size(20, 20)
        ToolStrip1.Location = New Point(0, 0)
        ToolStrip1.Name = "ToolStrip1"
        ToolStrip1.Size = New Size(176, 25)
        ToolStrip1.TabIndex = 11
        ToolStrip1.Text = "ToolStrip1"
        ' 
        ' lblSelectedIP
        ' 
        lblSelectedIP.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblSelectedIP.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold)
        lblSelectedIP.Location = New Point(3, 245)
        lblSelectedIP.Name = "lblSelectedIP"
        lblSelectedIP.Size = New Size(149, 24)
        lblSelectedIP.TabIndex = 10
        lblSelectedIP.Text = "(None)"
        ' 
        ' lblSelectedIPlbl
        ' 
        lblSelectedIPlbl.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lblSelectedIPlbl.AutoSize = True
        lblSelectedIPlbl.Location = New Point(3, 222)
        lblSelectedIPlbl.Name = "lblSelectedIPlbl"
        lblSelectedIPlbl.Size = New Size(85, 20)
        lblSelectedIPlbl.TabIndex = 9
        lblSelectedIPlbl.Text = "Selected IP:"
        ' 
        ' TreeView1
        ' 
        TreeView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        TreeView1.Location = New Point(0, 37)
        TreeView1.Name = "TreeView1"
        TreeView1.Size = New Size(176, 182)
        TreeView1.TabIndex = 8
        ' 
        ' chkShowAll
        ' 
        chkShowAll.AutoSize = True
        chkShowAll.Location = New Point(35, 249)
        chkShowAll.Margin = New Padding(3, 4, 3, 4)
        chkShowAll.Name = "chkShowAll"
        chkShowAll.Size = New Size(85, 24)
        chkShowAll.TabIndex = 7
        chkShowAll.Text = "ShowAll"
        chkShowAll.UseVisualStyleBackColor = True
        chkShowAll.Visible = False
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.ImageAlign = ContentAlignment.MiddleLeft
        Label2.Location = New Point(3, 274)
        Label2.Name = "Label2"
        Label2.Size = New Size(125, 25)
        Label2.TabIndex = 6
        Label2.Text = "Show Clients"
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnRefresh.Location = New Point(3, 676)
        btnRefresh.Margin = New Padding(3, 4, 3, 4)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(10, 10)
        btnRefresh.TabIndex = 2
        btnRefresh.Text = "Refresh"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' btnShowImage
        ' 
        btnShowImage.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnShowImage.ForeColor = Color.Red
        btnShowImage.Location = New Point(3, 706)
        btnShowImage.Margin = New Padding(3, 4, 3, 4)
        btnShowImage.Name = "btnShowImage"
        btnShowImage.Size = New Size(10, 10)
        btnShowImage.TabIndex = 5
        btnShowImage.Text = "Show Desktop"
        btnShowImage.UseVisualStyleBackColor = True
        ' 
        ' lstShowClients
        ' 
        lstShowClients.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        lstShowClients.FormattingEnabled = True
        lstShowClients.Location = New Point(0, 298)
        lstShowClients.Margin = New Padding(3, 4, 3, 4)
        lstShowClients.Name = "lstShowClients"
        lstShowClients.SelectionMode = SelectionMode.None
        lstShowClients.Size = New Size(176, 244)
        lstShowClients.TabIndex = 4
        ' 
        ' btnStart
        ' 
        btnStart.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnStart.ForeColor = Color.Red
        btnStart.Location = New Point(3, 745)
        btnStart.Margin = New Padding(3, 4, 3, 4)
        btnStart.Name = "btnStart"
        btnStart.Size = New Size(10, 10)
        btnStart.TabIndex = 3
        btnStart.Text = "Start Receiver"
        btnStart.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.ImageAlign = ContentAlignment.MiddleLeft
        Label1.Location = New Point(6, 14)
        Label1.Name = "Label1"
        Label1.Size = New Size(110, 25)
        Label1.TabIndex = 0
        Label1.Text = "Computers"
        ' 
        ' ToolStrip
        ' 
        ToolStrip.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ToolStrip.AutoSize = False
        ToolStrip.BackColor = SystemColors.HighlightText
        ToolStrip.Dock = DockStyle.None
        ToolStrip.ImageScalingSize = New Size(20, 20)
        ToolStrip.Items.AddRange(New ToolStripItem() {ShowAllFormButton, ToolStripSeparator2, View, cboRowAmount, ToolStripSeparator1, ToolStripDropDownButton1, ToolStripSeparator6, AutoRefreshButton, ToolStripSeparator5, RefreshButton, ToolStripSeparator7, ToolStripButton1, ToolStripSeparator9, ControlButton})
        ToolStrip.Location = New Point(0, 0)
        ToolStrip.Name = "ToolStrip"
        ToolStrip.Padding = New Padding(0)
        ToolStrip.Size = New Size(1132, 37)
        ToolStrip.TabIndex = 6
        ToolStrip.Text = "ToolStrip"
        ' 
        ' ShowAllFormButton
        ' 
        ShowAllFormButton.DisplayStyle = ToolStripItemDisplayStyle.Text
        ShowAllFormButton.ImageTransparentColor = Color.Magenta
        ShowAllFormButton.Name = "ShowAllFormButton"
        ShowAllFormButton.Padding = New Padding(5, 0, 5, 0)
        ShowAllFormButton.Size = New Size(85, 34)
        ShowAllFormButton.Text = "All Forms"
        ' 
        ' ToolStripSeparator2
        ' 
        ToolStripSeparator2.Name = "ToolStripSeparator2"
        ToolStripSeparator2.Size = New Size(6, 37)
        ' 
        ' View
        ' 
        View.Name = "View"
        View.Size = New Size(41, 34)
        View.Text = "View"
        ' 
        ' cboRowAmount
        ' 
        cboRowAmount.AutoSize = False
        cboRowAmount.DropDownStyle = ComboBoxStyle.DropDownList
        cboRowAmount.Items.AddRange(New Object() {"Auto", "1", "2", "3", "4", "5"})
        cboRowAmount.Name = "cboRowAmount"
        cboRowAmount.Size = New Size(57, 28)
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(6, 37)
        ' 
        ' ToolStripDropDownButton1
        ' 
        ToolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text
        ToolStripDropDownButton1.DropDownItems.AddRange(New ToolStripItem() {Connected, All})
        ToolStripDropDownButton1.ImageTransparentColor = Color.Magenta
        ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        ToolStripDropDownButton1.Size = New Size(59, 34)
        ToolStripDropDownButton1.Text = "Show"
        ' 
        ' Connected
        ' 
        Connected.Name = "Connected"
        Connected.Size = New Size(163, 26)
        Connected.Text = "Connected"
        ' 
        ' All
        ' 
        All.Name = "All"
        All.Size = New Size(163, 26)
        All.Text = "All"
        ' 
        ' ToolStripSeparator6
        ' 
        ToolStripSeparator6.Name = "ToolStripSeparator6"
        ToolStripSeparator6.Size = New Size(6, 37)
        ' 
        ' AutoRefreshButton
        ' 
        AutoRefreshButton.ImageTransparentColor = Color.Magenta
        AutoRefreshButton.Name = "AutoRefreshButton"
        AutoRefreshButton.Size = New Size(45, 34)
        AutoRefreshButton.Text = "Auto"
        AutoRefreshButton.Visible = False
        ' 
        ' ToolStripSeparator5
        ' 
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New Size(6, 37)
        ToolStripSeparator5.Visible = False
        ' 
        ' RefreshButton
        ' 
        RefreshButton.ImageTransparentColor = Color.Magenta
        RefreshButton.Name = "RefreshButton"
        RefreshButton.Size = New Size(62, 34)
        RefreshButton.Text = "Refresh"
        RefreshButton.Visible = False
        ' 
        ' ToolStripSeparator7
        ' 
        ToolStripSeparator7.Name = "ToolStripSeparator7"
        ToolStripSeparator7.Size = New Size(6, 37)
        ToolStripSeparator7.Visible = False
        ' 
        ' ToolStripButton1
        ' 
        ToolStripButton1.ImageTransparentColor = Color.Magenta
        ToolStripButton1.Name = "ToolStripButton1"
        ToolStripButton1.Size = New Size(79, 34)
        ToolStripButton1.Text = "Close APP"
        ToolStripButton1.Visible = False
        ' 
        ' ToolStripSeparator9
        ' 
        ToolStripSeparator9.Name = "ToolStripSeparator9"
        ToolStripSeparator9.Size = New Size(6, 37)
        ToolStripSeparator9.Visible = False
        ' 
        ' ControlButton
        ' 
        ControlButton.ImageTransparentColor = Color.Magenta
        ControlButton.Name = "ControlButton"
        ControlButton.Size = New Size(44, 34)
        ControlButton.Text = "Stop"
        ControlButton.Visible = False
        ' 
        ' Timer1
        ' 
        Timer1.Enabled = True
        Timer1.Interval = 500
        ' 
        ' Timer2
        ' 
        Timer2.Enabled = True
        Timer2.Interval = 10000
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = SystemColors.HighlightText
        Panel1.Controls.Add(ToolStrip)
        Panel1.Dock = DockStyle.Top
        Panel1.Location = New Point(0, 30)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1132, 37)
        Panel1.TabIndex = 14
        ' 
        ' ShareScreenToolStripMenuItem
        ' 
        ShareScreenToolStripMenuItem.Name = "ShareScreenToolStripMenuItem"
        ShareScreenToolStripMenuItem.Size = New Size(224, 26)
        ShareScreenToolStripMenuItem.Text = "Share Screen"
        ' 
        ' MDIParent1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.Control
        ClientSize = New Size(1132, 852)
        Controls.Add(LeftSidePanel)
        Controls.Add(Panel1)
        Controls.Add(MenuStrip)
        Controls.Add(StatusStrip)
        IsMdiContainer = True
        MainMenuStrip = MenuStrip
        Margin = New Padding(5, 4, 5, 4)
        MinimumSize = New Size(960, 840)
        Name = "MDIParent1"
        Text = "LAN Monitoring System - Offline"
        MenuStrip.ResumeLayout(False)
        MenuStrip.PerformLayout()
        StatusStrip.ResumeLayout(False)
        StatusStrip.PerformLayout()
        LeftSidePanel.ResumeLayout(False)
        LeftSidePanel.PerformLayout()
        ToolStrip.ResumeLayout(False)
        ToolStrip.PerformLayout()
        CType(BindingSource1, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents HelpMenu As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents StatusStrip As StatusStrip
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FileMenu As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents MenuStrip As MenuStrip
    Friend WithEvents ViewMenu As ToolStripMenuItem
    Friend WithEvents ToolBarToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StatusBarToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LeftSidePanel As Panel
    Friend WithEvents btnRefresh As Button
    Friend WithEvents ToolStrip As ToolStrip
    Friend WithEvents ShowAllFormButton As ToolStripButton
    Friend WithEvents btnStart As Button
    Friend WithEvents lstShowClients As ListBox
    Friend WithEvents btnShowImage As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label2 As Label
    Friend WithEvents chkShowAll As CheckBox
    Friend WithEvents View As ToolStripLabel
    Friend WithEvents cboRowAmount As ToolStripComboBox
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents BindingSource1 As BindingSource
    Friend WithEvents Connected As ToolStripMenuItem
    Friend WithEvents All As ToolStripMenuItem
    Friend WithEvents TreeView1 As TreeView
    Friend WithEvents lblSelectedIPlbl As Label
    Friend WithEvents lblSelectedIP As Label
    Friend WithEvents Timer2 As Timer
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents AutoRefreshButton As ToolStripButton
    Friend WithEvents RefreshButton As ToolStripButton
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents ControlButton As ToolStripButton
    Friend WithEvents Label1 As Label
    Friend WithEvents OPENAPPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CALCULATORToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DLSAUWebsiteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents NotePadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SendMessageALLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SENDTOSELECTEDIPToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents POWERMENUToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShutdownToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LogOffToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RestartToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents CONTROLPCToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MOUSECONTROLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VIEWPROCESSToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DISPLAYPCsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MSTeamsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MSWordToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MSPowerPointToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MSExcelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShareScreenToolStripMenuItem As ToolStripMenuItem
End Class
