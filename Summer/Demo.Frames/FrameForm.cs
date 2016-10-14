using DirectShowLib;
using DirectShowLib.DES;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Frames
{
    public partial class FrameForm : System.Windows.Forms.Form
    {
        private bool bLoop = false;
        private bool bThru = false;
        private bool isSeeking = false;
        private bool isTimer = false;
        private bool canStep = false;
        private int nbFiles = 0;
        private string movieDir;
        private int minutes, seconds;

        private IGraphBuilder graphBuilder = null;
        private IMediaControl mediaCtrl = null;
        private IMediaEventEx mediaEvt = null;
        private IMediaPosition mediaPos = null;
        private IVideoFrameStep frameStep = null;
        private IVideoWindow videoWin = null;

        //different constant needed by the app
        private const int WM_GRAPHNOTIFY = 0x00008001;
        private const int WS_CHILD = 0x40000000;
        private const int WS_CLIPCHILDREN = 0x02000000;
        private const int WS_CLIPSIBLINGS = 0x04000000;
        private const int WM_MOVE = 0x00000003;
        private const int EC_COMPLETE = 0x00000001;

        public enum State { Playing, Paused, Stopped };
        private State graphState;

        private string _fileName;           //used to save the movie file name 
        private string _storagePath;        //used for the path where we save files

        private GrabFrames _grabFrames;
        public delegate void ReportProgress(double progress);

        private BackgroundWorker _saveFramesTask;

        // The index of the current frame.
        private int FrameNum = 0;

        private void InitilizeTask()
        {
            _saveFramesTask = new BackgroundWorker();
            _saveFramesTask.WorkerReportsProgress = true;
            _saveFramesTask.WorkerSupportsCancellation = true;
            _saveFramesTask.DoWork += SaveFramesTask_DoWork;
            _saveFramesTask.ProgressChanged += SaveFramesTask_ProgressChanged;
            _saveFramesTask.RunWorkerCompleted += SaveFramesTask_RunWorkerCompleted;
        }

        private void SaveFramesTask_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_grabFrames != null)
            {
                if (_grabFrames.Frames != null && _grabFrames.Frames.Count > 0)
                {
                    for (int i = 0; i <= _grabFrames.Frames.Count; i++)
                    {
                        if (_saveFramesTask.CancellationPending == true)
                        {
                            e.Cancel = true;
                            break;
                        }
                        try
                        {
                            using (Bitmap bitmap = new Bitmap(_grabFrames.Frames[i].Image))
                            {
                                bitmap.Save(string.Format("{0}\\{1}.png", _storagePath, i));
                            }
                        }
                        catch (Exception ee)
                        {
                        }
                        _saveFramesTask.ReportProgress(100 * (i + 1) / _grabFrames.Frames.Count);
                    }
                }
            }

        }

        private void SaveFramesTask_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressPanel1.ProgressBar.Value = e.ProgressPercentage;
            this.progressPanel1.ProgressBar.Value = e.ProgressPercentage;
        }

        private void SaveFramesTask_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressPanel1.ProgressBar.Value = 100;
            //this.progressPanel1.ProgressBar.Visible = false;
            this.progressPanel1.IsVisible(false);
            if (e.Cancelled)
            {
                return;
            }
            MessageBox.Show("成功导出全部帧图！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PerformSave()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "请选择将要保存帧图的文件路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (_fileName != null)
                    {
                        _storagePath = dialog.SelectedPath;
                        SaveFrames();
                    }
                    else
                    {
                        //to do
                    }

                }
            }
        }

        private void SaveFrames()
        {
            if (!_saveFramesTask.IsBusy)
            {
                this.progressPanel1.LabelInfo.Text = "正在保存帧图，请稍等......";
                this.progressPanel1.ProgressBar.Value = 0;
                this.progressPanel1.IsVisible(true);
                _saveFramesTask.RunWorkerAsync();
            }
        }

        private void ReportProgressHandler(double progress)
        {
            if (this.progressPanel1.ProgressBar.InvokeRequired)
            {
                ReportProgress d = new ReportProgress(ReportProgressHandler);
                this.progressPanel1.ProgressBar.Invoke(d, new object[] { progress });
            }
            else
            {
                int value = (int)(100 * (progress + 1) / this._grabFrames.MediaInfo.Duration);
                if (value >= 100)
                {
                    value = 100;
                    this.progressPanel1.IsVisible(false);
                    this.snapShots.LoadFrames(_grabFrames.Frames);
                    this.btnSave.Enabled = true;
                    this.btnVideoPlay.Enabled = true;
                    this.btnVideoStop.Enabled = true;
                    this.btnVideoSave.Enabled = true;
                    InitilizeFps();
                }
                this.progressPanel1.ProgressBar.Value = value;
            }
        }

        public FrameForm()
        {
            InitializeComponent();

            trackBar1.Minimum = 0;
            trackBar1.Maximum = 0;
            trackBar1.Enabled = false;
            timer1.Interval = 10;
            timer1.Enabled = true;
            movieFiles.SelectedIndexChanged += new EventHandler(sicListBox);
            movieDir = "";
            statusBar1.Panels[0].Text = "Duration: 00m:00s";
            this.progressPanel1.IsVisible(false);
            this.btnSave.Enabled = false;
            this.btnVideoPlay.Enabled = false;
            this.btnVideoStop.Enabled = false;
            this.btnVideoSave.Enabled = false;
            InitilizeTask();
        }

        // 
        //This method create the filter graph
        //
        private void InitInterfaces()
        {
            try
            {
                graphBuilder = (IGraphBuilder)new FilterGraph();
                mediaCtrl = (IMediaControl)graphBuilder;
                mediaEvt = (IMediaEventEx)graphBuilder;
                mediaPos = (IMediaPosition)graphBuilder;
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't start");
            }
        }

        //
        //This method stop the filter graph and ensures that we stop
        //sending messages to our window
        //
        private void CloseInterfaces()
        {
            if (mediaCtrl != null)
            {
                mediaCtrl.StopWhenReady();
                mediaEvt.SetNotifyWindow((IntPtr)0, WM_GRAPHNOTIFY, (IntPtr)0);
            }
            mediaCtrl = null;
            mediaEvt = null;
            mediaPos = null;
            if (canStep)
                frameStep = null;
            videoWin = null;
            if (graphBuilder != null)
                Marshal.ReleaseComObject(this.graphBuilder);
            graphBuilder = null;
        }

        //
        //Fill the list box when the Set media directory button is clicked
        //
        private void FillListBox(string filePath)
        {
            movieFiles.Items.Clear();
            nbFiles = 0;
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] files = di.GetFiles();
            movieFiles.BeginUpdate();
            foreach (FileInfo f in files)
            {
                if (CheckExtension(f))
                {
                    movieFiles.Items.Add(f.Name);
                    nbFiles++;
                }
            }
            movieFiles.EndUpdate();
            if (movieFiles.Items.Count > 0)
            {
                movieFiles.SelectedIndex = 0;
            }
        }

        private bool CheckExtension(FileInfo f)
        {
            if (f.Extension.StartsWith(".mp"))
                return true;
            if (f.Extension.StartsWith(".wm"))
                return true;
            if (f.Extension.StartsWith(".avi"))
                return true;
            if (f.Extension.StartsWith(".asf"))
                return true;
            if (f.Extension.StartsWith(".mov"))
                return true;
            if (f.Extension.StartsWith(".rm"))
                return true;
            if (f.Extension.StartsWith(".ram"))
                return true;
            return false;
        }

        //
        //Method to start to play a media file
        //
        private void LoadFile(string fName)
        {
            try
            {
                //get the graph filter ready to render
                graphBuilder.RenderFile(fName, null);

                //set the trackbar
                OABool bCsf, bCsb;
                mediaPos.CanSeekBackward(out bCsb);
                mediaPos.CanSeekForward(out bCsf);
                isSeeking = (bCsb == OABool.True) && (bCsf == OABool.True);
                if (isSeeking)
                    trackBar1.Enabled = true;
                else
                    trackBar1.Enabled = false;
                trackBar1.Minimum = 0;

                double duration;
                mediaPos.get_Duration(out duration);
                trackBar1.Maximum = (int)(duration * 100.0);
                Text = fName;

                //check for the ability to step
                frameStep = graphBuilder as IVideoFrameStep;
                if (frameStep.CanStep(1, null) == 0)
                {
                    canStep = true;
                    buttonFramestep.Enabled = true;
                }

                //prepare and set the video window
                videoWin = graphBuilder as IVideoWindow;
                videoWin.put_Owner((IntPtr)panel1.Handle);
                videoWin.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings | WindowStyle.ClipChildren);
                Rectangle rc = panel1.ClientRectangle;
                videoWin.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
                mediaEvt.SetNotifyWindow((IntPtr)this.Handle, WM_GRAPHNOTIFY, (IntPtr)0);

                //set the different values for controls
                trackBar1.Value = 0;
                minutes = (int)duration / 60;
                seconds = (int)duration % 60;
                statusBar1.Panels[0].Text = "Duration: " + minutes.ToString("D2")
                    + ":m" + seconds.ToString("D2") + ":s";
                graphState = State.Playing;

                this.buttonPlay.Text = "Pause";
                //start the playback
                mediaCtrl.Run();

            }
            catch (Exception) { Text = "Error loading file"; }
        }

        //  
        //This is the handler when the selected index changed in
        //the listbox
        //
        private void sicListBox(object o, EventArgs e)
        {
            try
            {
                CloseInterfaces();
                InitInterfaces();
                LoadFile(movieDir + movieFiles.Text);
                _fileName = movieDir + movieFiles.Text;
                _grabFrames = new GrabFrames(_fileName);
                if (_grabFrames != null)
                {
                    tmrNextFrame.Enabled = false;
                    this.btnSave.Enabled = false;
                    this.btnVideoPlay.Enabled = false;
                    this.btnVideoStop.Enabled = false;
                    this.btnVideoSave.Enabled = false;
                    this.progressPanel1.ProgressBar.Value = 0;
                    this.progressPanel1.LabelInfo.Text = "正在加载帧图，请稍等......";
                    this.progressPanel1.IsVisible(true);
                    _grabFrames.ReportProgressHandler += ReportProgressHandler;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during playback, the application will terminate"
                     + "\\nMessage: " + ex.Message);
                Application.Exit();
            }
        }

        //
        //override to process custom graph notify messages
        //
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                if (mediaEvt != null)
                    OnGraphNotify();
                return;
            }
            base.WndProc(ref m);
        }

        //
        //call to process WM_GRAPHNOTIFY message
        //get out of the loop when GetEvent returns not 0
        //
        private void OnGraphNotify()
        {
            IntPtr p1, p2;
            EventCode code;

            if (mediaEvt == null)
                return;
            while (mediaEvt.GetEvent(out code, out p1, out p2, 0) == 0)
            {
                mediaEvt.FreeEventParams(code, p1, p2);
                if (code == EventCode.Complete)
                    OnClipCompleted();
            }
        }

        //
        //method that update the graph filter and/or gui controls
        //when we have received a message that the media file is done playing
        //
        private void OnClipCompleted()
        {
            graphState = State.Stopped;
            if (mediaCtrl == null)
                return;
            mediaCtrl.Stop();
            if (!bThru && bLoop)
            {
                if (isSeeking)
                {
                    mediaPos.put_CurrentPosition(0.0);
                    mediaCtrl.Run();
                }
            }
            if (bThru)
            {
                if ((movieFiles.SelectedIndex + 1) < nbFiles)
                    movieFiles.SelectedIndex++;
                else
                    movieFiles.SelectedIndex = 0;
            }
        }

        //
        //handler to fill the listbox with media files in the chose dir
        //
        private void buttonSetmedia_Click(object sender, System.EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "请选择存放视频的文件夹";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    CloseInterfaces();
                    InitInterfaces();
                    movieDir = dialog.SelectedPath + "\\";
                    textBox1.Text = movieDir;
                    FillListBox(dialog.SelectedPath);
                }
            }
        }

        private void buttonPlay_Click(object sender, System.EventArgs e)
        {
            if (mediaCtrl != null && graphState == State.Paused)
            {
                graphState = State.Playing;
                this.buttonPlay.Text = "Pause";
                mediaCtrl.Run();
            }
            else if (mediaCtrl != null && graphState != State.Paused)
            {
                mediaCtrl.Pause();
                graphState = State.Paused;
                this.buttonPlay.Text = "Play";
            }
            else if (mediaCtrl != null && graphState == State.Stopped)
            {
                sicListBox(null, null);
            }
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            if (mediaCtrl != null && graphState != State.Stopped)
            {
                mediaCtrl.Stop();
                graphState = State.Stopped;
            }
        }

        //
        //when the user grab the trackbar, we seek the graph filter by
        //update the CurrentPosition property
        //
        private void trackBar1_ValueChanged(object sender, System.EventArgs e)
        {
            if (isSeeking && !isTimer)
            {
                mediaCtrl.Pause();
                graphState = State.Paused;
                mediaPos.put_CurrentPosition((double)trackBar1.Value / 100.0);
            }
        }

        private void buttonThru_Click(object sender, System.EventArgs e)
        {
            bThru = !bThru;
            if (bThru)
                buttonLoop.Enabled = false;
            else
                buttonLoop.Enabled = true;
        }

        private void buttonLoop_Click(object sender, System.EventArgs e)
        {
            bLoop = !bLoop;
        }

        //
        //handler to update the Position label and move the trackbar 
        //tick postion, we set isTimer to true, then changing the
        //trackbar Value property will generate a call to the ValueChanged
        //handler but we don't want to seek the filter, so ValueChanged
        //check if isTimer is true and ignore timer update to seek the filter
        //
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (isSeeking)
            {
                isTimer = true;
                double currentPosition;
                mediaPos.get_CurrentPosition(out currentPosition);
                trackBar1.Value = (int)(currentPosition * 100.0);
                isTimer = false;
                minutes = (int)currentPosition / 60;
                seconds = (int)currentPosition % 60;
                label1.Text = "Position: " + minutes.ToString("D2")
                    + ":m" + seconds.ToString("D2") + ":s";
            }
        }

        private void buttonGraphEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\graphedt.exe";
                if (path != String.Empty)
                {
                    Process.Start(path);
                }
                else
                {
                    MessageBox.Show("graphedt.exe cant not found in " + System.Environment.CurrentDirectory  
                        + "folder. ", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception) { } //ignore exceptions
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PerformSave();
        }

        private void MovieFiles_MouseClick(object sender, MouseEventArgs e)
        {
            int index = movieFiles.IndexFromPoint(e.X, e.Y);
            movieFiles.SelectedIndex = index;
            if (movieFiles.SelectedIndex != -1)
            {
            }
        }

        private void buttonFramestep_Click(object sender, System.EventArgs e)
        {
            if (frameStep != null)
            {
                mediaCtrl.Pause();
                graphState = State.Paused;
                frameStep.Step(1, null);
            }
        }

        private void btnVideoPlay_Click(object sender, EventArgs e)
        {
            tmrNextFrame.Enabled = true;
        }

        private void btnVideoStop_Click(object sender, EventArgs e)
        {
            tmrNextFrame.Enabled = false;
        }

        private void btnVideoSave_Click(object sender, EventArgs e)
        {

        }

        private void tmrNextFrame_Tick(object sender, EventArgs e)
        {
            if (this._grabFrames != null && _grabFrames.Frames != null)
            {
                FrameNum = ++FrameNum % _grabFrames.Frames.Count;
                picFrame.Image = _grabFrames.Frames[FrameNum].Image;
            }
        }

        private void hscrFps_Scroll(object sender, ScrollEventArgs e)
        {
            tmrNextFrame.Interval = 1000 / hScrollBar.Value;
            lblFps.Text = hScrollBar.Value.ToString();
        }

        private void InitilizeFps()
        {
            if (this._grabFrames != null && this._grabFrames.MediaInfo != null)
            {
                tmrNextFrame.Interval = (int)(1000 / _grabFrames.MediaInfo.FPS);
                lblFps.Text = _grabFrames.MediaInfo.FPS.ToString();
            }
        }

        private void LoadFrames()
        {
            //string[] filter = new string[] { ".png" };
            //FileInfo[] fileInfos = GetFileInfo(_storagePath, filter);

            //Frames = new List<Frame>();
            //for (int i = 0; i < fileInfos.Length; i++)
            //{
            //    string fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfos[i].FullName);
            //    Frame frame = new Frame(new Bitmap(fileInfos[i].FullName), fileName);
            //    Frames.Add(frame);
            //}
            //Frames.Sort((f1, f2) => f1.FrameName.CompareTo(f2.FrameName));
            //// Display the first frame.
            //picFrame.Image = Frames[FrameNum].Bitmap;

        }

        private FileInfo[] GetFileInfo(string fileFolder, string[] filter)
        {
            DirectoryInfo folder = new DirectoryInfo(fileFolder);
            try
            {
                FileInfo[] files;
                files = folder.GetFiles().Where(f => filter.Contains(f.Extension.ToLower())).ToArray();
                //Array.Sort(files, (f1, f2) => { return f1.Name.CompareTo(f2.Name); });
                SortAsFolderName(ref files);
                return files;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        private void SortAsFolderName(ref FileInfo[] dirs)
        {
            Array.Sort(dirs, delegate (FileInfo x, FileInfo y) { return x.FullName.CompareTo(y.FullName); });
        }
    }
}
