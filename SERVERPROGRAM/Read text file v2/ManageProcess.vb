Imports System.Formats.Asn1.AsnWriter
Imports System.IO
Imports System.Text

Public Class ManageProcess
    Dim AutoRefresh As Boolean = False
    Public Sub New(ByVal MDIParent1 As MDIParent1)
        InitializeComponent()
        Me.Owner = MDIParent1
    End Sub
    Sub PopulateDataGridView2(data As String)
        DataGridView.Rows.Clear()
        DataGridView.Columns.Clear()
        ' Split the data into lines
        Dim lines() As String = data.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
        If lines.Length > 0 Then
            ' Extract headers
            Dim headers() As String = lines(0).Split("␟"c)
            ' Add columns to DataGridView
            For Each header As String In headers
                DataGridView.Columns.Add(header.Trim(), header.Trim())
            Next
            ' Add rows to DataGridView
            For i As Integer = 1 To lines.Length - 1
                If Not String.IsNullOrWhiteSpace(lines(i)) Then
                    Dim cells() As String = lines(i).Split("␟"c)
                    DataGridView.Rows.Add(cells)
                End If
            Next
        End If
        DGVColumnSizer(DataGridView)
    End Sub

    Dim selectedindex As Integer = 0
    Dim selectedcolumn As Integer = 0
    Dim scrollingindex As Integer = 0
    Sub PopulateDataGridView(data As String)
        If DataGridView IsNot Nothing AndAlso DataGridView.CurrentRow IsNot Nothing Then Me.Invoke(New Action(AddressOf GetScrollingIndex))
        PopulateDataGridView2(data)
        If DataGridView IsNot Nothing AndAlso DataGridView.CurrentRow IsNot Nothing Then Me.Invoke(New Action(AddressOf SetScrollingIndex))
    End Sub
    Private Function GetScrollingIndex()
        selectedindex = DataGridView.CurrentRow.Index
        selectedcolumn = DataGridView.CurrentCell.ColumnIndex
        scrollingindex = DataGridView.FirstDisplayedCell.RowIndex
    End Function

    Private Sub SetScrollingIndex()
        DataGridView.FirstDisplayedScrollingRowIndex = scrollingindex
        If selectedindex < (DataGridView.RowCount - 1) Then
            DataGridView.CurrentCell = DataGridView.Rows(selectedindex).Cells(selectedcolumn)
        Else
            DataGridView.CurrentCell = DataGridView.Rows(0).Cells(selectedcolumn)
        End If

        GetSelected(selectedcolumn)
    End Sub
    Dim selrow As Int16
    Private Sub DataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView.SelectionChanged
        If DataGridView.Focused Then
            If (DataGridView.SelectedCells.Count > 0) Then
                selrow = DataGridView.CurrentCell.RowIndex
                GetSelected(selrow)
            End If
        End If
    End Sub


    Sub AutoRefreshButton_Click()
        If AutoRefresh Then
            DisableAutoRefresh()
        Else
            EnableAutoRefresh()
        End If
    End Sub

    Sub DisableAutoRefresh()
        AutoRefresh = False
        MDIParent1.AutoRefreshButton.Text = "Auto-Refresh Disabled"
        MDIParent1.AutoRefreshButton.ForeColor = Color.FromArgb(&H212121)
    End Sub
    Sub EnableAutoRefresh()
        AutoRefresh = True
        MDIParent1.AutoRefreshButton.Text = "Auto-Refresh Enabled"
        MDIParent1.AutoRefreshButton.ForeColor = Color.FromArgb(&H1F801F)
        MDIParent1.showProcess(True)
    End Sub

    Function IsAutoRefresh()
        Dim s As Boolean = AutoRefresh
        Return AutoRefresh
    End Function

    Sub btnRefresh_Click()
        MDIParent1.showProcess(True)
    End Sub

    Sub btnKillTask_Click()

        If MDIParent1.isConnected(MDIParent1.getSelectedIP) Then
            Dim result = MessageBox.Show("Close this process?", "Close Process", MessageBoxButtons.OKCancel)
            If result = DialogResult.Cancel Then

            ElseIf result = DialogResult.OK Then
                MDIParent1.SendMessage(MDIParent1.getSelectedIP, "(CLS)" & col2)
            End If
        End If
    End Sub


    Dim col0 As Object
    Dim col1 As Object
    Dim col2 As Object
    Dim col3 As Object
    Dim col4 As Object
    Private Sub GetSelected(ByVal RowIndex)
        If RowIndex < 0 OrElse RowIndex >= DataGridView.Rows.Count Then
            col0 = ""
            col1 = ""
            col2 = ""
            col3 = ""
            col4 = ""
        Else
            Dim zero As Object = DataGridView.Rows(RowIndex).Cells(0).Value
            Dim one As Object = DataGridView.Rows(RowIndex).Cells(1).Value
            Dim two As Object = DataGridView.Rows(RowIndex).Cells(2).Value
            Dim three As Object = DataGridView.Rows(RowIndex).Cells(3).Value
            Dim four As Object = DataGridView.Rows(RowIndex).Cells(4).Value

            col0 = If(zero Is DBNull.Value, "", CType(zero, String))
            col1 = If(one Is DBNull.Value, "", CType(one, String))
            col2 = If(two Is DBNull.Value, "", CType(two, String))
            col3 = If(three Is DBNull.Value, "", CType(three, String))
            col4 = If(four Is DBNull.Value, "", CType(four, String))
        End If
    End Sub

    Private Sub DataGridView_SizeChanged(sender As Object, e As EventArgs) Handles DataGridView.SizeChanged
        DGVColumnSizer(DataGridView)
    End Sub

    Sub DGVColumnSizer(ByVal DGV As DataGridView)
        Dim totalWidth As Integer = DGV.ClientSize.Width

        Dim percentages As Integer() = {30, 20, 10, 10, 30}
        Dim minWidths As Integer() = {0, 0, 100, 100, 0}

        Dim maxWidths As Integer() = {0, 0, 150, 150, 0}

        If percentages.Sum() <> 100 Then
            Throw New ArgumentException("Percentages must add up to 100")
        End If

        Try
            For i As Integer = 0 To DGV.Columns.Count - 1
                If i < percentages.Length Then
                    Dim calculatedWidth As Integer = (totalWidth * percentages(i)) \ 100
                    Dim finalWidth As Integer = calculatedWidth
                    If i < minWidths.Length AndAlso minWidths(i) > 0 Then
                        finalWidth = Math.Max(calculatedWidth, minWidths(i))
                    End If
                    If i < maxWidths.Length AndAlso maxWidths(i) > 0 Then
                        finalWidth = Math.Min(finalWidth, maxWidths(i))
                    End If
                    DGV.Columns(i).Width = finalWidth
                End If
            Next
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Sub DGVClear()
        DataGridView.Rows.Clear()
    End Sub
End Class