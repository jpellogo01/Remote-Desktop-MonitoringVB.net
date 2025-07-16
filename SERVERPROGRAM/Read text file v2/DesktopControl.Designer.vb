<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DesktopControl
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
        ViewDesktop = New PictureBox()
        Timer1 = New Timer(components)
        Label1 = New Label()
        Label3 = New Label()
        CType(ViewDesktop, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' ViewDesktop
        ' 
        ViewDesktop.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ViewDesktop.BorderStyle = BorderStyle.FixedSingle
        ViewDesktop.Location = New Point(12, 40)
        ViewDesktop.Name = "ViewDesktop"
        ViewDesktop.Size = New Size(581, 403)
        ViewDesktop.SizeMode = PictureBoxSizeMode.StretchImage
        ViewDesktop.TabIndex = 6
        ViewDesktop.TabStop = False
        ' 
        ' Timer1
        ' 
        Timer1.Interval = 2
        ' 
        ' Label1
        ' 
        Label1.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label1.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = Color.Firebrick
        Label1.Location = New Point(689, 9)
        Label1.Name = "Label1"
        Label1.RightToLeft = RightToLeft.Yes
        Label1.Size = New Size(160, 24)
        Label1.TabIndex = 13
        Label1.Text = "Disabled"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(12, 10)
        Label3.Name = "Label3"
        Label3.Size = New Size(123, 20)
        Label3.TabIndex = 18
        Label3.Text = "Desktop Control"
        ' 
        ' DesktopControl
        ' 
        AutoScaleDimensions = New SizeF(8F, 19F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(861, 510)
        ControlBox = False
        Controls.Add(Label3)
        Controls.Add(Label1)
        Controls.Add(ViewDesktop)
        FormBorderStyle = FormBorderStyle.None
        Name = "DesktopControl"
        Text = "DesktopControl"
        TopMost = True
        CType(ViewDesktop, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents ViewDesktop As PictureBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label

End Class
