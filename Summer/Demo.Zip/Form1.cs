using Ionic.Zip;
using Summer.System.IO.ZIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Zip
{
    public partial class Form1 : Form, IZipWorker
    {
        private BaseZip _zip;
        private List<ZFile> _zipfiles;
        private HashSet<String> _tag;
        private double _requireSpace;
        private string _directory;//压缩文件存放的目录
        private string _zipFullname;//压缩文件

        private int _readBytes = 0;
        private int _currentBytes = 0;

        private bool _cancel;

        public Form1()
        {
            InitializeComponent();
            _directory = AppDomain.CurrentDomain.BaseDirectory + @"TestData\ZipFile";
            _zip = new BaseZip(this);
            _zipfiles = new List<ZFile>();
            _tag = new HashSet<string>();
            this.listViewFiles.ItemSelectionChanged += ListViewFiles_ItemSelectionChanged;

            this.listViewFiles.Columns.Add("Files", 200, HorizontalAlignment.Left);
        }

        private void ListViewFiles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.btnRemove.Enabled = this.listViewFiles.SelectedItems.Count > 0;
        }
        /// <summary>
        /// 测试数据
        /// 获取指定名称的文件、指定后缀名的文件
        /// </summary>
        private void getFile()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => getFile()));
            }
            else
            {
                FileInfo[] fileInfos;
                _cancel = false;
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //1.获取指定文件夹下的指定文件名的文件
                string[] extension = new string[] { "SystemLog", "FConfig", "FCPlates", "SensorData", "log-file" };
                fileInfos = FileInfoHelper.GetFileInfo(baseDirectory, @"\TestData\", extension, true);
                _zipfiles.Clear();
                if (fileInfos != null)
                {
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        _zipfiles.Add(new ZFile(fileInfo.FullName, null, fileInfo.Length));
                    }
                }
                //2.获取指定文件夹下面的所有文件
                fileInfos = FileInfoHelper.GetFileInfo(baseDirectory, @"\TestData\Calibration Results");
                // Calibration Results
                if (fileInfos != null)
                {
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        if (fileInfo != null)
                        {
                            _zipfiles.Add(new ZFile(fileInfo.FullName, @"\TestData\Calibration Results", fileInfo.Length));
                        }
                    }
                }
                //3.获取所有的txt文本文件
                extension = new string[] { ".txt" };
                fileInfos = FileInfoHelper.GetFileInfo(baseDirectory, @"\TestData\Dump\", extension, false);
                if (fileInfos != null)
                {
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        _zipfiles.Add(new ZFile(fileInfo.FullName, @"\TestData\Dump\", fileInfo.Length));
                    }
                }

                //4.获取最新的一个ncf文件
                FileInfo qcFileInfo = FileInfoHelper.GetFileInfo(baseDirectory, @"\TestData\Raw Data\", "*.ncf");
                if (qcFileInfo != null)
                {
                    _zipfiles.Add(new ZFile(qcFileInfo.FullName, @"\TestData\Raw Data\", qcFileInfo.Length));
                }

                foreach (ListViewItem item in this.listViewFiles.Items)
                {
                    string fileFullName = item.Tag as string;
                    if (fileFullName != null)
                    {
                        _zipfiles.Add(new ZFile(fileFullName, null, GetFileLength(fileFullName)));
                    }
                }
            }
        }

        private long GetFileLength(string fileFullName)
        {
            return new FileInfo(fileFullName).Length;
        }

        private void initData()
        {
            _readBytes = 0;
            _currentBytes = 0;
            this.label.Text = "";
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                getFile();
                _requireSpace = calculateTotalFileLength() / (1024 * 1024.0);
                if (IsDiskFreeSpaceOK(_directory, _requireSpace))
                {
                    using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
                    {
                        zip.SaveProgress += Zip_SaveProgress;
                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
                        zip.UseZip64WhenSaving = Zip64Option.AsNecessary;
                        zip.ParallelDeflateThreshold = -1;
                        zip.ZipError += Zip_ZipError;
                        for (int i = 0; i < _zipfiles.Count; i++)
                        {
                            if (_zip.LoadFileTask.CancellationPending == true)
                            {
                                e.Cancel = true;
                                break;
                            }
                            zip.AddFile(_zipfiles[i].FileFullName, _zipfiles[i].Folder == null ? "" : _zipfiles[i].Folder);
                        }
                        _zipFullname = string.Format("{0}\\Problem Report  {1}.{2}", _directory, DateTime.Now.ToString("yyMMdd_HHmm"), "zip");
                        zip.Save(_zipFullname);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        public void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Zip_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Object, SaveProgressEventArgs>(Zip_SaveProgress), new Object[] { sender, e });
            }
            else
            {
                if (e.EventType == ZipProgressEventType.Saving_Started)
                {
                    this.progressBar.Maximum = (int)(_requireSpace * 1024);
                    this.progressBar.Value = 0;
                    this.progressBar.Minimum = 0;
                    //this.progressBar.Step = 1;
                }
                else if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
                {
                    //this.progressBar.Maximum = e.EntriesTotal;
                    //this.progressBar.Value = (int)e.EntriesSaved + 1;
                }
                else if (e.EventType == ZipProgressEventType.Saving_EntryBytesRead)
                {
                    if (e.BytesTransferred != e.TotalBytesToTransfer)
                    {
                        _currentBytes = (int)(e.BytesTransferred / 1024.0);
                        progressBar.Value = _readBytes + _currentBytes;
                    }
                    else
                    {
                        _readBytes += (int)(e.TotalBytesToTransfer / 1024.0);
                        progressBar.Value = _readBytes;
                    }
                }
                else if (e.EventType == ZipProgressEventType.Saving_Completed)
                {
                    if (!_cancel)
                    {
                        progressBar.Value = 0;
                        this.label.Text = "文件压缩成功，压缩文件存放于：\n" + _zipFullname;
                        System.Diagnostics.Process.Start("explorer.exe", "/select," + _zipFullname);
                    }
                    else
                    {
                        DeleteFile(_zipFullname);
                    }
                }
                this.Update();
                Application.DoEvents();
            }

        }

        private void Zip_ZipError(object sender, ZipErrorEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    e.Cancel = true;
                    this.label.Text = "压缩文件异常，异常消息为： \n" + e.Exception.Message;
                    //_canZip = false;
                    if (e.Exception.Message.Contains("There is not enough space on the disk."))
                    {
                        //ShowZipFailed();
                    }
                    else
                    {
                        //ShowZipException(e.Exception.Message);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DeleteFile(string fileFullName)
        {
            if (File.Exists(fileFullName))
            {
                File.Delete(fileFullName);
            }
        }

        /// <summary>
        /// //Check if the disk free space is more than require space (M) 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="requireSpace"></param>
        /// <returns></returns>
        private bool IsDiskFreeSpaceOK(string path, double requireSpace)
        {
            double freeSpace = -1;
            string root = Path.GetPathRoot(path);
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (string.Equals(root, drive.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    freeSpace = drive.TotalFreeSpace / (1024 * 1024.0);
                }
            }
            return freeSpace > requireSpace;
        }

        /// <summary>
        /// 计算压缩文件的大小
        /// </summary>
        /// <returns></returns>
        private long calculateTotalFileLength()
        {
            long totalFileSize = 0;
            if (_zipfiles != null)
            {
                foreach (var file in _zipfiles)
                {
                    totalFileSize += file.FileLength;
                }
            }
            return totalFileSize;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            showOpenFileDialog();
        }

        public void showOpenFileDialog()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                ofd.InitialDirectory = baseDirectory + @"TestData\";
                ofd.Multiselect = true;
                ofd.Filter = "All Files|*.*";
                //ofd.Filter = "NovoCyte® Files|*.ncf|All Files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fileName in ofd.FileNames)
                    {
                        try
                        {
                            AppendItem(Path.GetFileName(fileName), fileName);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }

                }
            }

            this.btnRemove.Enabled = this.listViewFiles.SelectedItems.Count > 0;
        }

        private void AppendItem(string name, string fullname)
        {
            ListViewItem lvi = new ListViewItem(name);
            lvi.Text = name;
            lvi.Tag = fullname;
            if (_tag.Add(fullname))
            {
                this.listViewFiles.Items.Add(lvi);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in this.listViewFiles.SelectedItems)
            {
                if (lvi != null)
                {
                    if (_tag.Remove((string)lvi.Tag))
                    {
                        this.listViewFiles.Items.Remove(lvi);
                    }
                }
            }
            if (this.listViewFiles.Items != null && this.listViewFiles.Items.Count == 0)
            {
                this.btnRemove.Enabled = false;
            }
        }

        private void btnZip_Click(object sender, EventArgs e)
        {
            if (!_zip.LoadFileTask.IsBusy)
            {
                initData();
                _zip.LoadFileTask.RunWorkerAsync();
            }
        }

    }

}
