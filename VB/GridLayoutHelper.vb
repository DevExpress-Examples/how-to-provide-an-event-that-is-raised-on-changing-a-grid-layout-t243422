﻿Imports System
Imports System.Windows
Imports System.ComponentModel
Imports System.Collections.Generic
Imports DevExpress.Xpf.Grid
Imports DevExpress.Xpf.Mvvm.UI.Interactivity
    Imports DevExpress.Data.Browsing

Namespace GridLayoutHelper
    Public Class GridLayoutHelper
        Inherits Behavior(Of GridControl)

        Public Event LayoutChanged As EventHandler(Of MyEventArgs)
        Private LayoutChangedTypes As New List(Of LayoutChangedType)()
        Private ReadOnly Property Grid() As GridControl
            Get
                Return AssociatedObject
            End Get
        End Property
        Private IsLocked As Boolean

        #Region "DependencyPropertyDescriptors"
        Private _ActualWidthDescriptor As DependencyPropertyDescriptor
        Private ReadOnly Property ActualWidthDescriptor() As DependencyPropertyDescriptor
            Get
                If _ActualWidthDescriptor Is Nothing Then
                    _ActualWidthDescriptor = GetPropertyDescriptor("ActualWidth")
                End If
                Return _ActualWidthDescriptor
            End Get
        End Property
        Private _VisibleIndexDescriptor As DependencyPropertyDescriptor
        Private ReadOnly Property VisibleIndexDescriptor() As DependencyPropertyDescriptor
            Get
                If _VisibleIndexDescriptor Is Nothing Then
                    _VisibleIndexDescriptor = GetPropertyDescriptor("VisibleIndex")
                End If
                    Return _VisibleIndexDescriptor
            End Get
        End Property
        Private _GroupIndexDescriptor As DependencyPropertyDescriptor
        Private ReadOnly Property GroupIndexDescriptor() As DependencyPropertyDescriptor
            Get
                If _GroupIndexDescriptor Is Nothing Then
                    _GroupIndexDescriptor = GetPropertyDescriptor("GroupIndex")
                End If
                Return _GroupIndexDescriptor
            End Get
        End Property
        Private _VisibleDescriptor As DependencyPropertyDescriptor
        Private ReadOnly Property VisibleDescriptor() As DependencyPropertyDescriptor
            Get
                If _VisibleDescriptor Is Nothing Then
                    _VisibleDescriptor = GetPropertyDescriptor("Visible")
                End If
                Return _VisibleDescriptor
            End Get
        End Property
        #End Region

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            If Grid.Columns IsNot Nothing Then
                SubscribeColumns()
            Else
                AddHandler Grid.Loaded, AddressOf OnGridLoaded
            End If
            AddHandler Grid.FilterChanged, AddressOf OnGridFilterChanged
            AddHandler Grid.AutoGeneratedColumns, AddressOf OnGridColumnsPopulated
        End Sub
        Protected Overrides Sub OnDetaching()
            UnSubscribeColumns()
            RemoveHandler Grid.Loaded, AddressOf OnGridLoaded
            RemoveHandler Grid.FilterChanged, AddressOf OnGridFilterChanged
            RemoveHandler Grid.AutoGeneratedColumns, AddressOf OnGridColumnsPopulated
            MyBase.OnDetaching()
        End Sub

        Private Sub SubscribeColumns()
            AddHandler Grid.Columns.CollectionChanged, AddressOf ColumnsCollectionChanged
            For Each column As GridColumn In Grid.Columns
                SubscribeColumn(column)
            Next column
        End Sub
        Private Sub UnSubscribeColumns()
            RemoveHandler Grid.Columns.CollectionChanged, AddressOf ColumnsCollectionChanged
            For Each column As GridColumn In Grid.Columns
                UnSubscribeColumn(column)
            Next column
        End Sub
        Private Sub SubscribeColumn(ByVal column As GridColumn)
            ActualWidthDescriptor.AddValueChanged(column, AddressOf OnColumnWidthChanged)
            VisibleIndexDescriptor.AddValueChanged(column, AddressOf OnColumnVisibleIndexChanged)
            GroupIndexDescriptor.AddValueChanged(column, AddressOf OnColumnGroupIndexChanged)
            VisibleDescriptor.AddValueChanged(column, AddressOf OnColumnVisibleChanged)
        End Sub
        Private Sub UnSubscribeColumn(ByVal column As GridColumn)
            ActualWidthDescriptor.RemoveValueChanged(column, AddressOf OnColumnVisibleIndexChanged)
            VisibleIndexDescriptor.RemoveValueChanged(column, AddressOf OnColumnVisibleIndexChanged)
            GroupIndexDescriptor.RemoveValueChanged(column, AddressOf OnColumnGroupIndexChanged)
            VisibleDescriptor.RemoveValueChanged(column, AddressOf OnColumnVisibleChanged)
        End Sub

        Private Sub ProcessLayoutChanging(ByVal type As LayoutChangedType)
            If Not LayoutChangedTypes.Contains(type) Then
                LayoutChangedTypes.Add(type)
            End If
            If IsLocked Then
                Return
            End If
            IsLocked = True
            Dispatcher.BeginInvoke(New Action(Sub()
                IsLocked = False
                RaiseEvent LayoutChanged(Me, New MyEventArgs With {.LayoutChangedTypes = LayoutChangedTypes})
                LayoutChangedTypes.Clear()
            End Sub))
        End Sub
        Private Sub OnGridLoaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
            SubscribeColumns()
            AddHandler Grid.Columns.CollectionChanged, AddressOf ColumnsCollectionChanged
        End Sub
        Private Sub OnGridColumnsPopulated(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ProcessLayoutChanging(LayoutChangedType.ColumnsCollection)
            SubscribeColumns()
        End Sub
        Private Sub ColumnsCollectionChanged(ByVal sender As Object, ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)
            If e.NewItems IsNot Nothing OrElse e.OldItems IsNot Nothing Then
                ProcessLayoutChanging(LayoutChangedType.ColumnsCollection)
            End If
            If e.OldItems IsNot Nothing Then
                For Each column As GridColumn In e.OldItems
                    UnSubscribeColumn(column)
                Next column
            End If
            If e.NewItems IsNot Nothing Then
                For Each column As GridColumn In e.NewItems
                    SubscribeColumn(column)
                Next column
            End If
        End Sub
        Private Sub OnGridFilterChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ProcessLayoutChanging(LayoutChangedType.FilerChanged)
        End Sub
        Private Sub OnColumnWidthChanged(ByVal sender As Object, ByVal args As EventArgs)
            ProcessLayoutChanging(LayoutChangedType.ColumnWidth)
        End Sub
        Private Sub OnColumnVisibleIndexChanged(ByVal sender As Object, ByVal args As EventArgs)
            ProcessLayoutChanging(LayoutChangedType.ColumnVisibleIndex)
        End Sub
        Private Sub OnColumnGroupIndexChanged(ByVal sender As Object, ByVal args As EventArgs)
            ProcessLayoutChanging(LayoutChangedType.ColumnGroupIndex)
        End Sub
        Private Sub OnColumnVisibleChanged(ByVal sender As Object, ByVal args As EventArgs)
            ProcessLayoutChanging(LayoutChangedType.ColumnVisible)
        End Sub
        Private Function GetPropertyDescriptor(ByVal name As String) As DependencyPropertyDescriptor
            Return DependencyPropertyDescriptor.FromProperty(TypeDescriptor.GetProperties(GetType(GridColumn))(name))
        End Function
    End Class
    Public Class MyEventArgs
        Inherits EventArgs

        Public Property LayoutChangedTypes() As List(Of LayoutChangedType)
    End Class
    Public Enum LayoutChangedType
        ColumnsCollection
        FilerChanged
        ColumnGroupIndex
        ColumnVisibleIndex
        ColumnWidth
        ColumnVisible
        None
    End Enum
End Namespace