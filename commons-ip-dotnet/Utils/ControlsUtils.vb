Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Controls.Primitives
Imports System.Windows.Threading
Imports log4net

Public Class ControlsUtils
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(ControlsUtils))

    Public Shared Function RetrieveDropFiles(e As DragEventArgs) As List(Of DataRowFile)
        Dim result As New List(Of DataRowFile)

        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
            Dim filesPaths() As String = e.Data.GetData("FileDrop", False)
            For Each path As String In filesPaths
                Dim pathAttrs = System.IO.File.GetAttributes(path)
                If pathAttrs.HasFlag(FileAttributes.Directory) Then
                    Dim folder As New DirectoryInfo(path)
                    For Each file In folder.GetFiles
                        result.Add(New DataRowFile(file.FullName, True, file.Directory.Name))
                    Next
                Else
                    result.Add(New DataRowFile(path))
                End If
            Next
        End If

        Return result
    End Function

    Public Shared Sub AddGridItemFromPath(ByVal grid As DataGrid, ByVal files As List(Of DataRowFile))
        For Each file In files
            grid.Items.Add(file)
        Next
    End Sub

    Public Shared Function RetrieveFiles(ByVal grid As DataGrid, ByVal Optional onlySelected As Boolean = False) As List(Of DataRowFile)
        Dim result As New List(Of DataRowFile)

        If grid IsNot Nothing AndAlso grid.Items.Count > 0 Then
            For Each item As DataRowFile In grid.Items
                If onlySelected Then
                    If item.IsSelected Then
                        result.Add(item)
                    End If
                Else
                    result.Add(item)
                End If
            Next
        End If

        Return result
    End Function

    ''' <summary>
    ''' Force refresh
    ''' </summary>
    Public Shared Sub UpdateUI()
        Dim frame As New DispatcherFrame()
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, New DispatcherOperationCallback(AddressOf ExitFrame), frame)
        Dispatcher.PushFrame(frame)
    End Sub

    ''' <summary>
    ''' Exit frame
    ''' </summary>
    ''' <param name="f"></param>
    ''' <returns></returns>
    Public Shared Function ExitFrame(ByVal f As Object) As Object
        CType(f, DispatcherFrame).Continue = False
        Return Nothing
    End Function


#Region "DataGrid"

    Public Shared Function GetSelectedRow(ByVal grid As DataGrid) As DataGridRow
        Return CType(grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem), DataGridRow)
    End Function

    Public Shared Function GetRow(ByVal grid As DataGrid, ByVal index As Integer) As DataGridRow
        Dim row As DataGridRow = CType(grid.ItemContainerGenerator.ContainerFromIndex(index), DataGridRow)

        If row Is Nothing Then
            grid.UpdateLayout()
            grid.ScrollIntoView(grid.Items(index))
            row = CType(grid.ItemContainerGenerator.ContainerFromIndex(index), DataGridRow)
        End If

        Return row
    End Function

    Public Shared Function GetCell(ByVal grid As DataGrid, ByVal row As DataGridRow, ByVal column As Integer) As DataGridCell
        If row IsNot Nothing Then
            Dim presenter As DataGridCellsPresenter = GetVisualChild(Of DataGridCellsPresenter)(row)

            If presenter Is Nothing Then
                grid.ScrollIntoView(row, grid.Columns(column))
                presenter = GetVisualChild(Of DataGridCellsPresenter)(row)
            End If

            Dim cell As DataGridCell = CType(presenter.ItemContainerGenerator.ContainerFromIndex(column), DataGridCell)
            Return cell
        End If

        Return Nothing
    End Function

    Public Shared Function GetVisualChild(Of T As Visual)(ByVal parent As Visual) As T
        Dim child As T = Nothing
        Dim numVisuals As Integer = VisualTreeHelper.GetChildrenCount(parent)

        For i As Integer = 0 To numVisuals - 1
            Dim v As Visual = CType(VisualTreeHelper.GetChild(parent, i), Visual)
            child = TryCast(v, T)

            If child Is Nothing Then
                child = GetVisualChild(Of T)(v)
            End If

            If child IsNot Nothing Then
                Exit For
            End If
        Next

        Return child
    End Function

    Public Shared Function GetCell(ByVal grid As DataGrid, ByVal row As Integer, ByVal column As Integer) As DataGridCell
        Dim rowContainer As DataGridRow = GetRow(grid, row)
        Return GetCell(grid, rowContainer, column)
    End Function

#End Region

End Class
