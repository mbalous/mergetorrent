﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Torrents", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Files And Directories", System.Windows.Forms.HorizontalAlignment.Left)
        Me.btnAddTorrents = New System.Windows.Forms.Button()
        Me.btnAddFiles = New System.Windows.Forms.Button()
        Me.btnAddDirectory = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.lnkMergeTorrent = New System.Windows.Forms.LinkLabel()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnClearAll = New System.Windows.Forms.Button()
        Me.lvSources = New System.Windows.Forms.ListView()
        Me.colName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colProcessed = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colComplete = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colRecovered = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MergeWorker = New System.ComponentModel.BackgroundWorker()
        Me.SuspendLayout()
        '
        'btnAddTorrents
        '
        Me.btnAddTorrents.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddTorrents.Location = New System.Drawing.Point(885, 12)
        Me.btnAddTorrents.Name = "btnAddTorrents"
        Me.btnAddTorrents.Size = New System.Drawing.Size(119, 28)
        Me.btnAddTorrents.TabIndex = 1
        Me.btnAddTorrents.Text = "Add Torrents..."
        Me.btnAddTorrents.UseVisualStyleBackColor = True
        '
        'btnAddFiles
        '
        Me.btnAddFiles.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddFiles.Location = New System.Drawing.Point(885, 46)
        Me.btnAddFiles.Name = "btnAddFiles"
        Me.btnAddFiles.Size = New System.Drawing.Size(119, 28)
        Me.btnAddFiles.TabIndex = 2
        Me.btnAddFiles.Text = "Add Files..."
        Me.btnAddFiles.UseVisualStyleBackColor = True
        '
        'btnAddDirectory
        '
        Me.btnAddDirectory.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddDirectory.Location = New System.Drawing.Point(885, 80)
        Me.btnAddDirectory.Name = "btnAddDirectory"
        Me.btnAddDirectory.Size = New System.Drawing.Size(119, 28)
        Me.btnAddDirectory.TabIndex = 3
        Me.btnAddDirectory.Text = "Add Directory..."
        Me.btnAddDirectory.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Enabled = False
        Me.btnStart.Location = New System.Drawing.Point(885, 370)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(119, 28)
        Me.btnStart.TabIndex = 4
        Me.btnStart.Text = "Start!"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'lnkMergeTorrent
        '
        Me.lnkMergeTorrent.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkMergeTorrent.AutoSize = True
        Me.lnkMergeTorrent.Location = New System.Drawing.Point(885, 354)
        Me.lnkMergeTorrent.Name = "lnkMergeTorrent"
        Me.lnkMergeTorrent.Size = New System.Drawing.Size(119, 13)
        Me.lnkMergeTorrent.TabIndex = 5
        Me.lnkMergeTorrent.TabStop = True
        Me.lnkMergeTorrent.Text = "mergetorrent homepage"
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.Enabled = False
        Me.btnClear.Location = New System.Drawing.Point(885, 131)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(119, 28)
        Me.btnClear.TabIndex = 6
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnClearAll
        '
        Me.btnClearAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearAll.Enabled = False
        Me.btnClearAll.Location = New System.Drawing.Point(885, 165)
        Me.btnClearAll.Name = "btnClearAll"
        Me.btnClearAll.Size = New System.Drawing.Size(119, 28)
        Me.btnClearAll.TabIndex = 7
        Me.btnClearAll.Text = "Clear All"
        Me.btnClearAll.UseVisualStyleBackColor = True
        '
        'lvSources
        '
        Me.lvSources.AllowColumnReorder = True
        Me.lvSources.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSources.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colProcessed, Me.colComplete, Me.colRecovered, Me.colStatus})
        Me.lvSources.FullRowSelect = True
        ListViewGroup1.Header = "Torrents"
        ListViewGroup1.Name = "lvgTorrents"
        ListViewGroup2.Header = "Files And Directories"
        ListViewGroup2.Name = "lvgFilesAndDirectories"
        Me.lvSources.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2})
        Me.lvSources.Location = New System.Drawing.Point(12, 12)
        Me.lvSources.Name = "lvSources"
        Me.lvSources.ShowItemToolTips = True
        Me.lvSources.Size = New System.Drawing.Size(867, 386)
        Me.lvSources.TabIndex = 8
        Me.lvSources.UseCompatibleStateImageBehavior = False
        Me.lvSources.View = System.Windows.Forms.View.Details
        '
        'colName
        '
        Me.colName.Text = "Name"
        Me.colName.Width = 460
        '
        'colProcessed
        '
        Me.colProcessed.Text = "Processed"
        Me.colProcessed.Width = 70
        '
        'colComplete
        '
        Me.colComplete.Text = "Complete"
        Me.colComplete.Width = 70
        '
        'colRecovered
        '
        Me.colRecovered.Text = "Recovered"
        Me.colRecovered.Width = 70
        '
        'colStatus
        '
        Me.colStatus.Text = "Status"
        Me.colStatus.Width = 187
        '
        'MergeWorker
        '
        Me.MergeWorker.WorkerSupportsCancellation = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1016, 410)
        Me.Controls.Add(Me.lvSources)
        Me.Controls.Add(Me.btnClearAll)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.lnkMergeTorrent)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.btnAddDirectory)
        Me.Controls.Add(Me.btnAddFiles)
        Me.Controls.Add(Me.btnAddTorrents)
        Me.Name = "MainForm"
        Me.Text = "mergetorrent"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAddTorrents As System.Windows.Forms.Button
    Friend WithEvents btnAddFiles As System.Windows.Forms.Button
    Friend WithEvents btnAddDirectory As System.Windows.Forms.Button
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents lnkMergeTorrent As System.Windows.Forms.LinkLabel
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnClearAll As System.Windows.Forms.Button
    Friend WithEvents lvSources As System.Windows.Forms.ListView
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colProcessed As System.Windows.Forms.ColumnHeader
    Friend WithEvents colComplete As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecovered As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents MergeWorker As System.ComponentModel.BackgroundWorker

End Class