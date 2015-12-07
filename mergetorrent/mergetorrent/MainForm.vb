﻿Public Class MainForm
    Private Const Sha1Hashbytes As Integer = 20

    Private _
        _utorrentConfigDir As String =
            My.Computer.FileSystem.CombinePath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                               "uTorrent")

    Private Class SourceItem
        Inherits ListViewItem

        Enum SourceItemType
            Torrent
            Directory
            File
        End Enum

        Private path_ As String
        Private Type_ As SourceItemType
        Private Processed_ As Double = -1
        Private Completion_ As Double = -1
        Private Recovered_ As Double = -1
        Private Status_ As String = ""

        Property Path As String
            Get
                Return path_
            End Get
            Set
                path_ = Value
            End Set
        End Property

        ReadOnly Property Type As SourceItemType
            Get
                Return Type_
            End Get
        End Property

        Property Processed As Double
            Get
                Return Processed_
            End Get
            Set
                Processed_ = Value
                Me.SubItems(1).Text = Processed_.ToString("P02")
            End Set
        End Property

        Property Completion As Double
            Get
                Return Completion_
            End Get
            Set
                Completion_ = Value
                Me.SubItems(2).Text = Completion_.ToString("P02")
            End Set
        End Property

        Property Recovered As Double
            Get
                Return Recovered_
            End Get
            Set
                Recovered_ = Value
                Me.SubItems(3).Text = Recovered_.ToString("P02")
            End Set
        End Property

        Property Status As String
            Get
                Return Status_
            End Get
            Set
                Status_ = Value
                Me.SubItems(4).Text = Status_
            End Set
        End Property

        Sub New(Path As String, Type As SourceItemType)
            If Type = SourceItemType.Torrent Then
                Me.ToolTipText = Path
                Me.Text = My.Computer.FileSystem.GetName(Path)
                Me.SubItems.Add("") 'processed
                Me.SubItems.Add("") 'completion
                Me.SubItems.Add("") 'recovered
                Me.SubItems.Add("") 'status
            Else
                Me.Text = Path
                Me.SubItems.Add("") 'processed
                Me.SubItems.Add("") 'completion
                Me.SubItems.Add("") 'recovered
                Me.SubItems.Add("") 'status
            End If
            Me.Path = Path
            Me.Type_ = Type
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Text
        End Function
    End Class

    Private Sub btnAddTorrents_Click(sender As System.Object, e As System.EventArgs) _
        Handles btnAddTorrents.Click
        Dim ofd As New OpenFileDialog

        ofd.AddExtension = True
        ofd.AutoUpgradeEnabled = True
        ofd.CheckFileExists = True
        ofd.CheckPathExists = True
        ofd.DefaultExt = ".torrent"
        ofd.DereferenceLinks = True
        ofd.Filter = "Torrents (*.torrent)|*.torrent|All files (*.*)|*.*"
        If System.IO.Directory.Exists(_utorrentConfigDir) Then
            ofd.InitialDirectory = _utorrentConfigDir
        End If
        ofd.Multiselect = True
        ofd.Title = "Find Torrent(s)"
        If ofd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            For Each filename As String In ofd.FileNames
                Dim si As New SourceItem(filename, SourceItem.SourceItemType.Torrent)
                si.Group = lvSources.Groups("lvgTorrents")
                lvSources.Items.Add(si)
            Next
            _utorrentConfigDir = My.Computer.FileSystem.GetParentPath(ofd.FileName)
        End If
        lvSources_ItemCountChanged(Me, Nothing)
    End Sub

    Private Sub btnAddFiles_Click(sender As System.Object, e As System.EventArgs) Handles btnAddFiles.Click
        Dim ofd As New OpenFileDialog

        ofd.AddExtension = True
        ofd.AutoUpgradeEnabled = True
        ofd.CheckFileExists = True
        ofd.CheckPathExists = True
        ofd.DereferenceLinks = True
        ofd.Filter = "All files (*.*)|*.*"
        ofd.Multiselect = True
        ofd.Title = "Find Source File(s)"
        If ofd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            For Each filename As String In ofd.FileNames
                Dim si As New SourceItem(filename, SourceItem.SourceItemType.File)
                si.Group = lvSources.Groups("lvgFilesAndDirectories")
                lvSources.Items.Add(si)
            Next
        End If
        lvSources_ItemCountChanged(Me, Nothing)
    End Sub

    Private Sub btnAddDirectory_Click(sender As System.Object, e As System.EventArgs) _
        Handles btnAddDirectory.Click
        Dim fbd As New FolderBrowserDialog
        fbd.ShowNewFolderButton = False
        fbd.Description = "Find Source Directory (all subdirectories will be included)"
        If fbd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim si As New SourceItem(fbd.SelectedPath, SourceItem.SourceItemType.Directory)
            si.Group = lvSources.Groups("lvgFilesAndDirectories")
            lvSources.Items.Add(si)
        End If
        lvSources_ItemCountChanged(Me, Nothing)
    End Sub

    Private Sub lvSources_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) _
        Handles lvSources.SelectedIndexChanged
        btnClear.Enabled = lvSources.SelectedIndices.Count > 0
    End Sub

    Private Sub lvSources_ItemCountChanged(sender As System.Object, e As System.EventArgs)
        btnClearAll.Enabled = lvSources.Items.Count > 0
        btnStart.Enabled = False
        For Each li As SourceItem In lvSources.Items
            If li.Type = SourceItem.SourceItemType.Torrent Then
                btnStart.Enabled = True
            End If
        Next
    End Sub

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        If lvSources.SelectedIndices.Count > 0 Then
            For i As Integer = lvSources.Items.Count - 1 To 0 Step -1
                If lvSources.Items(i).Selected Then
                    lvSources.Items.RemoveAt(i)
                End If
            Next
        End If
        lvSources_ItemCountChanged(Me, Nothing)
    End Sub

    Private Sub btnClearAll_Click(sender As System.Object, e As System.EventArgs) Handles btnClearAll.Click
        If lvSources.Items.Count > 0 Then
            lvSources.Items.Clear()
        End If
        lvSources_ItemCountChanged(Me, Nothing)
    End Sub

    Private Sub lnkMergeTorrent_LinkClicked(sender As System.Object,
                                            e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
        Handles lnkMergeTorrent.LinkClicked
        Process.Start("http://code.google.com/p/mergetorrent/")
    End Sub

    Private Function GetConfigFile(filename As String) As Dictionary(Of String, Object)
        Dim config_file_fs As System.IO.FileStream
        If System.IO.File.Exists(My.Computer.FileSystem.CombinePath(_utorrentConfigDir, filename)) Then
            config_file_fs = System.IO.File.OpenRead(My.Computer.FileSystem.CombinePath(_utorrentConfigDir, filename))
        Else
            Dim ofd As New OpenFileDialog

            ofd.AddExtension = True
            ofd.AutoUpgradeEnabled = True
            ofd.CheckFileExists = True
            ofd.CheckPathExists = True
            ofd.DefaultExt = ".dat"
            ofd.DereferenceLinks = True
            ofd.Filter = My.Computer.FileSystem.GetName(filename) & "|" & My.Computer.FileSystem.GetName(filename) &
                         "|All files (*.*)|*.*"
            ofd.Title = "Open " & filename & " or cancel if you're not using uTorrent..."
            If System.IO.Directory.Exists(_utorrentConfigDir) Then
                ofd.InitialDirectory = _utorrentConfigDir
            End If
            ofd.Multiselect = False
            If InvokeEx(AddressOf ofd.ShowDialog, Me, Me) = Windows.Forms.DialogResult.OK Then
                _utorrentConfigDir = My.Computer.FileSystem.GetParentPath(ofd.FileName)
                config_file_fs = System.IO.File.OpenRead(ofd.FileName)
            Else
                Throw New ApplicationException("Can't find " & filename)
            End If
        End If
        Dim config_file As Dictionary(Of String, Object) = DirectCast(Bencode.Decode(config_file_fs),
                                                                      Dictionary(Of String, Object))
        config_file_fs.Close()
        Return config_file
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="torrent_filename"></param>
    ''' <param name="ForOutput">The list will have exactly one path member per element, otherwise maybe more</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TorrentFilenameToMultiPath(torrent_filename As String, ForOutput As Boolean) _
        As List(Of MultiFileStream.FileInfo)
        TorrentFilenameToMultiPath = New List(Of MultiFileStream.FileInfo)
        Dim resume_dat As Dictionary(Of String, Object) = GetConfigFile("resume.dat")

        Dim torrent_fs As System.IO.FileStream = System.IO.File.OpenRead(torrent_filename)
        Dim torrent As Dictionary(Of String, Object) = DirectCast(Bencode.Decode(torrent_fs),
                                                                  Dictionary(Of String, Object))
        torrent_fs.Close()
        Dim current_torrent As Dictionary(Of String, Object) = Nothing
        For Each s As String In resume_dat.Keys
            If _
                My.Computer.FileSystem.CombinePath(_utorrentConfigDir, s) =
                My.Computer.FileSystem.GetFileInfo(torrent_filename).FullName Then
                current_torrent = DirectCast(resume_dat(s), Dictionary(Of String, Object))
                Exit For
            End If
        Next
        If current_torrent Is Nothing Then
            Throw New ApplicationException("Can't find " & torrent_filename & " in resume.dat")
        End If
        Dim info As Dictionary(Of String, Object)
        info = DirectCast(torrent("info"), Dictionary(Of String, Object))
        If info.ContainsKey("files") Then
            For file_index As Integer = 0 To DirectCast(info("files"), List(Of Object)).Count - 1
                Dim f As Dictionary(Of String, Object) =
                        DirectCast(DirectCast(info("files"), List(Of Object))(file_index), Dictionary(Of String, Object))
                Dim source_filenames As New List(Of String)
                source_filenames.Add(System.Text.Encoding.UTF8.GetString(DirectCast(current_torrent("path"), Byte())))
                For Each path_element As Byte() In DirectCast(f("path"), List(Of Object))
                    source_filenames(0) = My.Computer.FileSystem.CombinePath(source_filenames(0),
                                                                             System.Text.Encoding.UTF8.GetString(
                                                                                 path_element))
                Next

                If current_torrent.ContainsKey("targets") Then 'override
                    For Each current_target As List(Of Object) In
                        DirectCast(current_torrent("targets"), List(Of Object))
                        If DirectCast(current_target(0), Long) = file_index Then
                            If ForOutput Then
                                source_filenames(0) = System.Text.Encoding.UTF8.GetString(DirectCast(current_target(1),
                                                                                                     Byte()))
                            Else
                                source_filenames(1) = System.Text.Encoding.UTF8.GetString(DirectCast(current_target(1),
                                                                                                     Byte()))
                            End If
                            Exit For
                        End If
                    Next
                End If

                If ForOutput Then
                    If My.Computer.FileSystem.FileExists(source_filenames(0)) Then _
                        'if the file exists, this is the output
                        'do nothing
                    ElseIf My.Computer.FileSystem.FileExists(source_filenames(0) & ".!ut") Then _
                        'if it exists with .ut!, this is the output
                        source_filenames(0) &= ".!ut"
                    Else 'read the settings to determine how to store the file
                        Dim settings_dat As Dictionary(Of String, Object) = GetConfigFile("settings.dat")
                        If _
                            settings_dat.ContainsKey("append_incomplete") AndAlso
                            DirectCast(settings_dat("append_incomplete"), Long) <> 0 Then
                            source_filenames(0) &= ".!ut"
                        End If
                    End If
                Else 'read both with and without extension
                    For i As Integer = source_filenames.Count - 1 To 0 Step -1
                        source_filenames.Add(source_filenames(i) & ".!ut")
                    Next
                End If
                Dim fi As New MultiFileStream.FileInfo(source_filenames, DirectCast(f("length"), Long))
                TorrentFilenameToMultiPath.Add(fi)
            Next
        ElseIf info.ContainsKey("length") Then
            If current_torrent.ContainsKey("path") Then
                Dim source_filenames As New List(Of String)
                source_filenames.Add(System.Text.Encoding.UTF8.GetString(DirectCast(current_torrent("path"), Byte())))
                If ForOutput Then
                    If My.Computer.FileSystem.FileExists(source_filenames(0)) Then _
                        'if the file exists, this is the output
                        'do nothing
                    ElseIf My.Computer.FileSystem.FileExists(source_filenames(0) & ".!ut") Then
                        source_filenames(0) &= ".!ut"
                    Else
                        Dim settings_dat As Dictionary(Of String, Object) = GetConfigFile("settings.dat")
                        If _
                            settings_dat.ContainsKey("append_incomplete") AndAlso
                            DirectCast(settings_dat("append_incomplete"), Long) <> 0 Then
                            source_filenames(0) &= ".!ut"
                        End If
                    End If
                Else 'read both with and without extension
                    For i As Integer = source_filenames.Count - 1 To 0 Step -1
                        source_filenames.Add(source_filenames(i) & ".!ut")
                    Next
                End If
                Dim fi As New MultiFileStream.FileInfo(source_filenames, DirectCast(info("length"), Long))
                TorrentFilenameToMultiPath.Add(fi)
            End If
        Else
            Throw New ApplicationException("Can't find length/files of torrent in resume.dat")
        End If
    End Function

    ''' <summary>
    ''' Find all files that are a certain length
    ''' </summary>
    ''' <param name="target_length"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FindAllByLength(target_length As Long) As List(Of String)
        FindAllByLength = New List(Of String)

        For i As Integer = 0 To lvSources.Items.Count - 1
            Dim current_listitem As SourceItem = GetSourceItem(i)
            Dim possible_source_type As SourceItem.SourceItemType = current_listitem.Type
            Dim possible_source_path As String = current_listitem.Path
            Select Case possible_source_type
                Case SourceItem.SourceItemType.Torrent
                    Dim m As List(Of MultiFileStream.FileInfo) = TorrentFilenameToMultiPath(possible_source_path, False)
                    'now we have files to look at
                    For Each fi As MultiFileStream.FileInfo In m
                        For Each s As String In fi.Path
                            If MergeWorker.CancellationPending Then Exit Function
                            If Not FindAllByLength.Contains(s) AndAlso
                               My.Computer.FileSystem.FileExists(s) AndAlso
                               My.Computer.FileSystem.GetFileInfo(s).Length = target_length Then
                                FindAllByLength.Add(s)
                            End If
                        Next
                    Next
                Case SourceItem.SourceItemType.File
                    If Not FindAllByLength.Contains(possible_source_path) AndAlso
                       My.Computer.FileSystem.FileExists(possible_source_path) AndAlso
                       My.Computer.FileSystem.GetFileInfo(possible_source_path).Length = target_length Then
                        FindAllByLength.Add(possible_source_path)
                    End If
                Case SourceItem.SourceItemType.Directory
                    'we don't use GetFiles recursive feature because there might be some directories that we can't read.
                    Dim directory_stack As New Queue(Of System.IO.DirectoryInfo)
                    directory_stack.Enqueue(My.Computer.FileSystem.GetDirectoryInfo(possible_source_path))
                    Do While directory_stack.Count > 0
                        If MergeWorker.CancellationPending Then Exit Function
                        Try
                            For Each f As System.IO.FileInfo In directory_stack.Peek.GetFiles
                                If MergeWorker.CancellationPending Then Exit Function
                                If Not FindAllByLength.Contains(f.FullName) AndAlso
                                   f.Length = target_length Then
                                    FindAllByLength.Add(f.FullName)
                                End If
                            Next
                        Catch ex As UnauthorizedAccessException
                            'do nothing, we'll just skip this directory
                        End Try
                        Try
                            For Each d As System.IO.DirectoryInfo In directory_stack.Peek.GetDirectories
                                If MergeWorker.CancellationPending Then Exit Function
                                directory_stack.Enqueue(d)
                            Next
                        Catch ex As UnauthorizedAccessException
                            'do nothing, we'll just skip this directory
                        End Try
                        directory_stack.Dequeue() 'don't need it anymore
                    Loop
            End Select
        Next
    End Function

    Private Delegate Function GetSourceItemCallback(index As Integer) As SourceItem

    Private Function GetSourceItem(index As Integer) As SourceItem
        Return _
            InvokeEx(Of Integer, SourceItem)(Function(x As Integer) DirectCast(lvSources.Items(x), SourceItem), index,
                                              lvSources)
    End Function

    Private Sub btnStart_Click(sender As System.Object, e As System.EventArgs) Handles btnStart.Click
        If Not MergeWorker.IsBusy Then
            btnAddDirectory.Enabled = False
            btnAddFiles.Enabled = False
            btnAddTorrents.Enabled = False
            btnClear.Enabled = False
            btnClearAll.Enabled = False
            'lvSources_ItemCountChanged(Me, Nothing)
            'lvSources_SelectedIndexChanged(Me, Nothing)
            'lvSources.Enabled = False
            btnStart.Text = "Stop!"
            MergeWorker.RunWorkerAsync()
        Else
            MergeWorker.CancelAsync()
        End If
    End Sub

    Private Sub UpdateStatus(s As String, force As Boolean, Optional ByVal listitem As SourceItem = Nothing,
                             Optional ByVal Completion As Double = -1, Optional ByVal Processed As Double = -1,
                             Optional ByVal Recovered As Double = -1)
        Dim update_period As TimeSpan = New TimeSpan(0, 0, 0, 0, 500) 'every 500ms

        Dim last_update As Date = Date.MinValue
        If force OrElse last_update + update_period <= Now Then
            last_update = Now
            If listitem IsNot Nothing Then
                InvokeEx(Sub()
                             MergeTorrentStatusLabel.Text = listitem.ToString & ": " & s
                             listitem.Status = s
                             If Completion <> -1 Then listitem.Completion = Completion
                             If Processed <> -1 Then listitem.Processed = Processed
                             If Recovered <> -1 Then listitem.Recovered = Recovered
                         End Sub, Me)
            Else
                InvokeEx(Sub()
                             MergeTorrentStatusLabel.Text = s
                         End Sub, Me)
            End If
        End If
    End Sub

    Private Sub Merge()
        UpdateStatus("Mering...", True)
        'First get all the lists of files for output streams
        Dim output_files As New Dictionary(Of Integer, List(Of MultiFileStream.FileInfo))(lvSources.Items.Count) _
        'each element is a list of the output files for a torrent
        Dim file_lengths As New Dictionary(Of Long, List(Of String)) _
        'a dictionary of the lengths needed for the output files
        For current_listitem_index As Integer = 0 To lvSources.Items.Count - 1
            If MergeWorker.CancellationPending Then Exit Sub
            Dim current_listitem As SourceItem = GetSourceItem(current_listitem_index)
            If current_listitem.Type = SourceItem.SourceItemType.Torrent Then
                UpdateStatus("Finding destination files...", True, current_listitem)
                Dim files As List(Of MultiFileStream.FileInfo) = TorrentFilenameToMultiPath(current_listitem.Path, True)
                output_files.Add(current_listitem_index, files)
                For Each fi As MultiFileStream.FileInfo In files
                    If MergeWorker.CancellationPending Then Exit Sub
                    UpdateStatus("Finding destination files... (" & fi.Path(0) & ")", False, current_listitem)
                    If Not file_lengths.ContainsKey(fi.Length) Then
                        file_lengths.Add(fi.Length, New List(Of String)) _
                        'empty list for now until we find files of this length
                    End If
                Next

                UpdateStatus("Found destination files.", True, current_listitem)
            End If
        Next

        'Now fill out the file_lengths with files indexed by length
        For current_listitem_index As Integer = 0 To lvSources.Items.Count - 1
            If MergeWorker.CancellationPending Then Exit Sub
            Dim current_listitem As SourceItem = GetSourceItem(current_listitem_index)
            UpdateStatus("Finding source files...", True, current_listitem)
            Dim possible_source_type As SourceItem.SourceItemType = current_listitem.Type
            Dim possible_source_path As String = current_listitem.Path
            Select Case possible_source_type
                Case SourceItem.SourceItemType.Torrent
                    Dim m As List(Of MultiFileStream.FileInfo) = TorrentFilenameToMultiPath(possible_source_path, False)
                    'now we have files to look at
                    For Each fi As MultiFileStream.FileInfo In m
                        For Each s As String In fi.Path
                            If MergeWorker.CancellationPending Then Exit Sub
                            UpdateStatus("Finding source files... (" & s & ")", False, current_listitem)
                            If My.Computer.FileSystem.FileExists(s) AndAlso
                               file_lengths.ContainsKey(My.Computer.FileSystem.GetFileInfo(s).Length) AndAlso
                               Not file_lengths(My.Computer.FileSystem.GetFileInfo(s).Length).Contains(s) Then
                                file_lengths(My.Computer.FileSystem.GetFileInfo(s).Length).Add(s)
                            End If
                        Next
                    Next
                Case SourceItem.SourceItemType.File
                    If My.Computer.FileSystem.FileExists(possible_source_path) AndAlso
                       file_lengths.ContainsKey(My.Computer.FileSystem.GetFileInfo(possible_source_path).Length) AndAlso
                       Not _
                       file_lengths(My.Computer.FileSystem.GetFileInfo(possible_source_path).Length).Contains(
                           possible_source_path) Then
                        file_lengths(My.Computer.FileSystem.GetFileInfo(possible_source_path).Length).Add(
                            possible_source_path)
                    End If
                Case SourceItem.SourceItemType.Directory
                    'we don't use GetFiles recursive feature because there might be some directories that we can't read.
                    Dim directory_stack As New Queue(Of System.IO.DirectoryInfo)
                    directory_stack.Enqueue(My.Computer.FileSystem.GetDirectoryInfo(possible_source_path))
                    Do While directory_stack.Count > 0
                        If MergeWorker.CancellationPending Then Exit Sub
                        UpdateStatus("Entering directory... (" & directory_stack.Peek.FullName & ")", False,
                                     current_listitem)
                        Try
                            For Each f As System.IO.FileInfo In directory_stack.Peek.GetFiles
                                If MergeWorker.CancellationPending Then Exit Sub
                                UpdateStatus("Finding source files... (" & f.FullName & ")", False, current_listitem)
                                If file_lengths.ContainsKey(f.Length) AndAlso
                                   Not file_lengths(f.Length).Contains(f.FullName) Then
                                    file_lengths(f.Length).Add(f.FullName)
                                End If
                            Next
                        Catch ex As UnauthorizedAccessException
                            'do nothing, we'll just skip this directory
                        End Try
                        Try
                            For Each d As System.IO.DirectoryInfo In directory_stack.Peek.GetDirectories
                                If MergeWorker.CancellationPending Then Exit Sub
                                UpdateStatus("Enqueueing directory... (" & d.FullName & ")", False, current_listitem)
                                directory_stack.Enqueue(d)
                            Next
                        Catch ex As UnauthorizedAccessException
                            'do nothing, we'll just skip this directory
                        End Try
                        directory_stack.Dequeue() 'don't need it anymore
                    Loop
            End Select
            UpdateStatus("Found source files.", True, current_listitem)
        Next

        'Now get all the lists of the files for input streams
        Dim input_files As New Dictionary(Of Integer, List(Of MultiFileStream.FileInfo))(lvSources.Items.Count)
        For current_listitem_index As Integer = 0 To lvSources.Items.Count - 1
            If MergeWorker.CancellationPending Then Exit Sub
            Dim current_listitem As SourceItem = GetSourceItem(current_listitem_index)
            If current_listitem.Type = SourceItem.SourceItemType.Torrent Then
                UpdateStatus("Building input list...", True, current_listitem)
                input_files.Add(current_listitem_index, New List(Of MultiFileStream.FileInfo))
                For Each fi As MultiFileStream.FileInfo In output_files(current_listitem_index)
                    If MergeWorker.CancellationPending Then Exit Sub
                    UpdateStatus("Building input list... (" & fi.Path(0) & ")", False, current_listitem)
                    Dim new_paths As New List(Of String)(file_lengths(fi.Length)) _
                    'make a copy because we change the sort order as needed
                    If new_paths.IndexOf(fi.Path(0)) > 0 Then _
                        'if it's there but not first then make it first.  This might speed things up, who knows?
                        new_paths.Remove(fi.Path(0))
                        new_paths.Insert(0, fi.Path(0))
                    End If

                    Dim new_fi As New MultiFileStream.FileInfo(new_paths, fi.Length)
                    input_files(current_listitem_index).Add(new_fi)
                Next
                UpdateStatus("Built input list.", True, current_listitem)
            End If
        Next

        For current_listitem_index As Integer = 0 To lvSources.Items.Count - 1
            If MergeWorker.CancellationPending Then Exit Sub
            Dim current_listitem As SourceItem = DirectCast(GetSourceItem(current_listitem_index), SourceItem)
            If current_listitem.Type = SourceItem.SourceItemType.Torrent Then
                UpdateStatus("Merging...", True, current_listitem)
                Dim _
                    out_stream As _
                        New MultiFileStream(output_files(current_listitem_index), IO.FileMode.OpenOrCreate,
                                            IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)
                Dim _
                    in_stream As _
                        New MultiFileStream(input_files(current_listitem_index), IO.FileMode.Open, IO.FileAccess.Read,
                                            IO.FileShare.ReadWrite)
                'now we have all the files that might work.  Start checking and merging.

                Dim torrent As Dictionary(Of String, Object)
                Dim br As New System.IO.BinaryReader(System.IO.File.OpenRead(current_listitem.Path))
                torrent = Bencode.DecodeDictionary(br)
                br.Close()
                Dim info As Dictionary(Of String, Object)
                info = DirectCast(torrent("info"), Dictionary(Of String, Object))

                Dim piece_len As Integer = CType(info("piece length"), Integer)
                Dim buffer(0 To (piece_len - 1)) As Byte
                Dim hash_result() As Byte
                Dim pieces() As Byte = DirectCast(info("pieces"), Byte())
                Dim pieces_position As Integer = 0
                Dim complete_bytes As Long = 0
                Dim recovered_bytes As Long = 0

                Do While pieces_position < pieces.Length
                    If MergeWorker.CancellationPending Then Exit Sub
                    UpdateStatus(
                        "Merging " & in_stream.GetCurrentFileName() & " with " & out_stream.GetCurrentFileName & " ...",
                        False,
                        current_listitem,
                        CDbl(complete_bytes) / CDbl(out_stream.Length),
                        CDbl(out_stream.Position) / CDbl(out_stream.Length),
                        CDbl(recovered_bytes) / CDbl(out_stream.Length))
                    Dim read_len As Integer

                    read_len = CInt(Math.Min(piece_len, in_stream.Length - in_stream.Position))
                    'try the out_stream first
                    Do
                        Try
                            out_stream.Read(buffer, 0, read_len)
                            Exit Do
                        Catch ex As System.IO.IOException
                            If _
                                MessageBox.Show(
                                    "Couldn't read from " & out_stream.GetCurrentFileName() & vbCrLf & vbCrLf &
                                    ex.ToString, "Couldn't read", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error,
                                    MessageBoxDefaultButton.Button1) <> DialogResult.Retry Then
                                Exit Sub
                            End If
                        End Try
                    Loop
                    hash_result = CheckHash.Hash(buffer, read_len)
                    Dim i As Integer = 0
                    Do While i < Sha1Hashbytes AndAlso pieces(pieces_position + i) = hash_result(i)
                        i += 1
                    Loop
                    If i = Sha1Hashbytes Then
                        'match!  No need to read from the in_stream
                        in_stream.Position += read_len
                        complete_bytes += read_len
                    Else
                        out_stream.Position -= read_len 'back up
                        Dim useful_permutation As List(Of Integer) = in_stream.GetPermutation()
                        Do
                            If MergeWorker.CancellationPending Then Exit Sub
                            Do
                                Try
                                    in_stream.Read(buffer, 0, read_len)
                                    Exit Do
                                Catch ex As System.IO.IOException
                                    If _
                                        MessageBox.Show(
                                            "Couldn't read from " & in_stream.GetCurrentFileName() & vbCrLf & vbCrLf &
                                            ex.ToString, "Couldn't read", MessageBoxButtons.RetryCancel,
                                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) <> DialogResult.Retry _
                                        Then
                                        Exit Sub
                                    End If
                                End Try
                            Loop
                            hash_result = CheckHash.Hash(buffer, read_len)
                            i = 0
                            Do While i < Sha1Hashbytes AndAlso pieces(pieces_position + i) = hash_result(i)
                                i += 1
                            Loop
                            If i = Sha1Hashbytes Then
                                'match!
                                complete_bytes += read_len
                                recovered_bytes += read_len
                                Do
                                    Try
                                        out_stream.Write(buffer, 0, read_len)
                                        Exit Do
                                    Catch ex As Exception
                                        If _
                                            MessageBox.Show(
                                                "Couldn't write to " & in_stream.GetCurrentFileName() & vbCrLf & vbCrLf &
                                                ex.ToString, "Couldn't read", MessageBoxButtons.RetryCancel,
                                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) <>
                                            DialogResult.Retry Then
                                            Exit Sub
                                        End If
                                    End Try
                                Loop
                                Exit Do
                            Else
                                'no match, try the next permutation
                                in_stream.NextPermutation(in_stream.Position - read_len, read_len)
                                If MultiFileStream.ComparePermutation(in_stream.GetPermutation, useful_permutation) Then
                                    'this piece can't be completed, let's move on
                                    out_stream.Position += read_len
                                    Exit Do
                                Else
                                    in_stream.Position -= read_len 'try again with the new permutation
                                End If
                            End If
                        Loop
                    End If
                    pieces_position += Sha1Hashbytes
                Loop
                UpdateStatus("Merge complete.", True,
                             current_listitem,
                             CDbl(complete_bytes) / CDbl(out_stream.Length),
                             CDbl(out_stream.Position) / CDbl(out_stream.Length),
                             CDbl(recovered_bytes) / CDbl(out_stream.Length))
            End If
        Next
    End Sub

    Private Sub MergeWorker_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) _
        Handles MergeWorker.DoWork
        Merge()
    End Sub

    Private Sub MergeWorker_RunWorkerCompleted(sender As System.Object,
                                               e As System.ComponentModel.RunWorkerCompletedEventArgs) _
        Handles MergeWorker.RunWorkerCompleted
        If e.Cancelled Then
            UpdateStatus("Cancelled.", True)
        Else
            UpdateStatus("Done.", True)
        End If
        If close_requested Then Me.Close()
        btnAddDirectory.Enabled = True
        btnAddFiles.Enabled = True
        btnAddTorrents.Enabled = True
        lvSources_ItemCountChanged(Me, Nothing)
        lvSources_SelectedIndexChanged(Me, Nothing)
        'lvSources.Enabled = True
        btnStart.Text = "Start!"
    End Sub

    Dim close_requested As Boolean = False

    Private Sub MainForm_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) _
        Handles MyBase.FormClosing
        If MergeWorker.IsBusy Then
            close_requested = True 'remember that we want to close later
            e.Cancel = True 'don't close just yet
            MergeWorker.CancelAsync()
        End If
    End Sub
End Class
