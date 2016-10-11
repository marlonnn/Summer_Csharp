using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

using DirectShowLib;
using DirectShowLib.DES;
using Demo.Frames;

namespace VideoEditor
{
    public delegate void HotMediaEvent(Object sender);

    public class MediaPlayer : IDisposable
    {
        public enum FileType
        {
            Video,
            Audio,
            Invalid
        }

        public enum GraphState
        {
            Stopped,
            Paused,
            Running,
            Exiting
        }

        bool bLoop = false;
        bool bThru = false;
        bool isSeeking = false;
        bool isTimer = false;
        bool canStep = false;
        int nbFiles = 0;
        int minutes, seconds;

        public IVideoFrameStep frameStep = null;
        public FileType fileType = FileType.Invalid;
        public IGraphBuilder graphBuilder = null;
        public IMediaControl mediaControl = null;
        public IVideoWindow videoWindow = null;
        public IBasicVideo basicVideo = null;
        public ISampleGrabber sampleGrabber = null;
        public IMediaSeeking mediaSeeking = null;
        public IMediaPosition mediaPosition = null;
        public IMediaDet mediaDet = null;
        public IBasicAudio basicAudio = null;
        public IMediaEventEx mediaEvent;
        public Control control;
        public MediaInfo mediaInfo = new MediaInfo();
        public List<Frames> segmentationImages;
        private string filename;
        private ManualResetEvent manualResetEvent = null;
        volatile private GraphState currentState = GraphState.Stopped;
        public event HotMediaEvent StopPlay;
        public void Dispose()
        {
            CloseInterfaces();
        }

        ~MediaPlayer()
        {
            CloseInterfaces();
        }

        public MediaPlayer(string fileName)
        {
            try
            {
                IntPtr eventHandler;
                this.filename = fileName;
                this.SetupInterfaces(fileName);
                DsError.ThrowExceptionForHR(mediaEvent.GetEventHandle(out eventHandler));
                manualResetEvent = new ManualResetEvent(false);
                manualResetEvent.SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(eventHandler, true);
                Thread t = new Thread(new ThreadStart(this.EventWait));
                t.Name = "HotMediaEvent Thread";
                t.Start();
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public void LoadSnapShots()
        {
            double interval = 1;
            double fps = 0;
            double duration = 0;
            fps = this.getFramePerSecond();
            duration = this.getDuration();

            if (duration < 101)
                interval = 1;

            if (duration > 100)
                interval = duration / 100;

            List<Frames> list = new List<Frames>();
            if (fileType == FileType.Video)
            {
                Bitmap snapshot;
                for (double i = 0; i < duration; i = i + interval)
                {
                    snapshot = this.SnapShot(i);
                    list.Add(new Frames(i, snapshot));
                }
            }

            this.segmentationImages = list;
        }


        public void SetupInterfaces(string fileName)
        {
            this.graphBuilder = (IGraphBuilder)new FilterGraph();
            this.sampleGrabber = (ISampleGrabber)new SampleGrabber();
            ConfigSampleGrabber(this.sampleGrabber);
            this.graphBuilder.AddFilter((IBaseFilter)sampleGrabber, "SampleGrabber");
            DsError.ThrowExceptionForHR(this.graphBuilder.RenderFile(fileName, null));
            this.mediaControl = (IMediaControl)this.graphBuilder;
            this.mediaEvent = (IMediaEventEx)this.graphBuilder;
            this.mediaSeeking = (IMediaSeeking)this.graphBuilder;
            this.mediaPosition = (IMediaPosition)this.graphBuilder;
            this.videoWindow = this.graphBuilder as IVideoWindow;
            this.basicVideo = this.graphBuilder as IBasicVideo;
            this.basicAudio = this.graphBuilder as IBasicAudio;

            this.frameStep = this.graphBuilder as IVideoFrameStep;

            this.mediaDet = (IMediaDet)new MediaDet();
            this.mediaDet.put_Filename(fileName);
            this.CheckFileType();
            if (this.fileType == FileType.Video)
                this.GetMediaInfo();
        }

        //public void SetupVideoWindow(Control control)
        //{
        //    int hr = 0;
        //    this.control = control;
        //    if (this.fileType == FileType.Video)
        //    {
        //        this.sampleGrabber.SetCallback(((MainForm)this.control.Parent.Parent), 1);
        //        this.GetMediaInfo();
        //        hr = this.videoWindow.put_Owner(control.Handle);
        //        DsError.ThrowExceptionForHR(hr);
        //        hr = this.videoWindow.put_WindowStyle((WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings));
        //        DsError.ThrowExceptionForHR(hr);
        //        hr = this.videoWindow.put_Visible(OABool.True);
        //        DsError.ThrowExceptionForHR(hr);
        //        hr = this.videoWindow.put_MessageDrain(control.Handle);
        //        this.SizeWindow();
        //        DsError.ThrowExceptionForHR(hr);
        //    }
        //}

        public void ClearVideoWindow()
        {
            int hr = 0;
            if (this.fileType == FileType.Video)
            {
                hr = this.videoWindow.put_Owner(IntPtr.Zero);
                DsError.ThrowExceptionForHR(hr);
                hr = this.videoWindow.put_Visible(OABool.False);
                DsError.ThrowExceptionForHR(hr);
                hr = this.videoWindow.put_MessageDrain(IntPtr.Zero);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        private void CheckFileType()
        {
            int hr;
            if ((this.videoWindow != null) || (this.basicVideo != null))
                this.fileType = FileType.Video;
            else if ((this.basicAudio != null))
                this.fileType = FileType.Audio;
            else
                this.fileType = FileType.Invalid;

            OABool lVisible;
            hr = this.videoWindow.get_Visible(out lVisible);
            if (hr < 0)
            {
                if (hr == unchecked((int)0x80004002)) //E_NOINTERFACE
                    this.fileType = FileType.Audio;
                else
                    DsError.ThrowExceptionForHR(hr);
            }
        }

        private void EventWait()
        {
            int hr;
            //WS UPDATED changed int to IntPtr
            IntPtr firstParameter, secondParameter;
            EventCode eventCode;
            do
            {
                manualResetEvent.WaitOne(-1, true);
                lock (this)
                {
                    if (currentState != GraphState.Exiting)
                    {
                        hr = mediaEvent.GetEvent(out eventCode, out firstParameter, out secondParameter, 0);
                        while (hr >= 0)
                        {
                            if (eventCode == EventCode.Complete)
                            {
                                Stop();
                                if (StopPlay != null)
                                    StopPlay(this);
                            }
                            hr = mediaEvent.FreeEventParams(eventCode, firstParameter, secondParameter);
                            DsError.ThrowExceptionForHR(hr);
                            hr = mediaEvent.GetEvent(out eventCode, out firstParameter, out secondParameter, 0);
                        }
                        if (hr != unchecked((int)0x80004004))
                            DsError.ThrowExceptionForHR(hr);
                    }
                    else
                        break;
                }
            } while (true);
        }

        public void SizeWindow()
        {
            int hr;
            Rectangle rc = control.ClientRectangle;
            int windowWidth, windowHeight;
            windowWidth = rc.Width;
            windowHeight = rc.Height;
            int videoWidth = this.mediaInfo.MediaWidth;
            int videoHeight = this.mediaInfo.MediaHeight;
            int x, y, width, height;
            double videoRatio = (double)videoWidth / (double)videoHeight;
            double windowRatio = (double)windowWidth / (double)windowHeight;
            if (videoRatio > windowRatio)
            {
                double ratio = (double)windowWidth / (double)videoWidth;
                x = (int)((windowWidth - videoWidth * ratio) / 2);
                y = (int)((windowHeight - videoHeight * ratio) / 2);
                width = (int)(videoWidth * ratio);
                height = (int)(videoHeight * ratio);
                hr = videoWindow.SetWindowPosition(x, y, width, height);
            }
            else
            {
                double ratio = (double)windowHeight / (double)videoHeight;
                x = (int)((windowWidth - videoWidth * ratio) / 2);
                y = (int)((windowHeight - videoHeight * ratio) / 2);
                width = (int)(videoWidth * ratio);
                height = (int)(videoHeight * ratio);
                hr = videoWindow.SetWindowPosition(x, y, width, height);
            }
            DsError.ThrowExceptionForHR(hr);
        }

        void ConfigSampleGrabber(ISampleGrabber sampGrabber)
        {
            AMMediaType media;
            media = new AMMediaType();
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            this.sampleGrabber.SetMediaType(media);
            DsUtils.FreeAMMediaType(media);
            media = null;
            int hr = sampGrabber.SetBufferSamples(true);
            DsError.ThrowExceptionForHR(hr);
        }

        private void GetMediaInfo()
        {
            AMMediaType media = new AMMediaType();
            this.sampleGrabber.GetConnectedMediaType(media);
            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {
                throw new Exception("Format type incorrect");
            }

            int videoWidth, videoHeight, videoStride;
            this.basicVideo.GetVideoSize(out videoWidth, out videoHeight);
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            videoStride = videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);
            this.mediaInfo.MediaWidth = videoWidth;
            this.mediaInfo.MediaHeight = videoHeight;
            this.mediaInfo.MediaStride = videoStride;
            this.mediaInfo.MediaBitCount = videoInfoHeader.BmiHeader.BitCount;
            this.mediaInfo.fps = this.getFramePerSecond();
            this.mediaInfo.duration = this.getDuration();

            this.mediaInfo.sFilePath = this.filename.ToString();

            DsUtils.FreeAMMediaType(media);
            media = null;
        }

        public IntPtr SnapShot()
        {
            int hr;
            IntPtr ip = IntPtr.Zero;
            int iBuffSize = 0;
            hr = this.sampleGrabber.GetCurrentBuffer(ref iBuffSize, ip);
            DsError.ThrowExceptionForHR(hr);
            ip = Marshal.AllocCoTaskMem(iBuffSize);
            hr = this.sampleGrabber.GetCurrentBuffer(ref iBuffSize, ip);
            DsError.ThrowExceptionForHR(hr);
            return ip;
        }

        public Bitmap SnapShot(double position)
        {
            int hr;
            IntPtr ip = IntPtr.Zero;
            int iBuffSize;
            hr = this.mediaDet.GetBitmapBits(position, out iBuffSize, ip, this.mediaInfo.MediaWidth, this.mediaInfo.MediaHeight);
            ip = Marshal.AllocCoTaskMem(iBuffSize);
            hr = this.mediaDet.GetBitmapBits(position, out iBuffSize, ip, this.mediaInfo.MediaWidth, this.mediaInfo.MediaHeight);
            Bitmap bm = new Bitmap(this.mediaInfo.MediaWidth, this.mediaInfo.MediaHeight);
            try
            {
                bm = new Bitmap(
                    this.mediaInfo.MediaWidth,
                    this.mediaInfo.MediaHeight,
                    -this.mediaInfo.MediaStride,
                    PixelFormat.Format24bppRgb,
                    (IntPtr)(ip.ToInt32() + iBuffSize - this.mediaInfo.MediaStride)
                    );
            }catch(Exception e)
            {
                Console.Out.WriteLine("Could not convert bitmapbits to bitmap: " + e.Message);
            }

            return bm;
        }

        public Bitmap IPToBmp(IntPtr ip, MediaInfo mediaInfo)
        {
            int iBufSize = mediaInfo.MediaWidth * mediaInfo.MediaHeight * 3;
            return new Bitmap(
                mediaInfo.MediaWidth,
                mediaInfo.MediaHeight,
                -mediaInfo.MediaStride,
                PixelFormat.Format24bppRgb,
                (IntPtr)(ip.ToInt32() + iBufSize - mediaInfo.MediaStride)
                );
        }

        private void CloseInterfaces()
        {
            lock (this)
            {
                int hr;
                try
                {
                    if (this.mediaControl != null)
                    {
                        hr = this.mediaControl.StopWhenReady();
                        this.mediaControl = null;
                    }
                    if (currentState != GraphState.Exiting)
                    {
                        currentState = GraphState.Exiting;
                        if (manualResetEvent != null)
                            manualResetEvent.Set();
                    }
                    if (this.videoWindow != null)
                    {
                        hr = this.videoWindow.put_Visible(OABool.False);
                        hr = this.videoWindow.put_Owner(IntPtr.Zero);
                        this.videoWindow = null;
                    }
                    this.mediaSeeking = null;
                    this.mediaPosition = null;
                    this.basicVideo = null;
                    this.basicAudio = null;
                    this.mediaDet = null;
                    if (this.graphBuilder != null)
                        Marshal.ReleaseComObject(this.graphBuilder);
                    this.graphBuilder = null;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
            }
            GC.Collect();
        }

        public void Rewind()
        {
            DsError.ThrowExceptionForHR(this.mediaPosition.put_CurrentPosition(0));
        }


        public void FastBackwards()
        {
            if (this.currentState == GraphState.Running || this.currentState == GraphState.Paused)
            {
                DsError.ThrowExceptionForHR(mediaControl.Pause());
                currentState = GraphState.Paused;
            }

            double fps, currentPosition;
            fps = this.getFramePerSecond();
            this.mediaPosition.get_CurrentPosition(out currentPosition);
            double newPosition = currentPosition - 1 / fps;
            this.mediaPosition.put_CurrentPosition(Math.Max(0, newPosition));
        }

        public void Start()
        {
            if (this.currentState == GraphState.Stopped || this.currentState == GraphState.Paused)
            {
                DsError.ThrowExceptionForHR(mediaControl.Run());
                this.currentState = GraphState.Running;
            }
        }

        public void Pause()
        {
            if (this.currentState == GraphState.Running)
            {
                DsError.ThrowExceptionForHR(mediaControl.Pause());
                this.currentState = GraphState.Paused;
            }
        }

        public void Stop()
        {
            if (this.currentState == GraphState.Running || this.currentState == GraphState.Paused)
            {
                DsError.ThrowExceptionForHR(mediaControl.Stop());
                currentState = GraphState.Stopped;
            }
        }

        public void FastForward()
        {
            if (this.currentState == GraphState.Running || this.currentState == GraphState.Paused)
            {
                DsError.ThrowExceptionForHR(mediaControl.Pause());
                currentState = GraphState.Paused;
            }
            double fps, currentPosition, duration;
            fps = this.getFramePerSecond();
            this.mediaPosition.get_CurrentPosition(out currentPosition);
            this.mediaPosition.get_Duration(out duration);
            double newPosition = currentPosition + 1 / fps;
            this.mediaPosition.put_CurrentPosition(Math.Min(duration, newPosition));
        }

        public void End()
        {
            double endPosition = 0;
            this.mediaDet.get_StreamLength(out endPosition);
            DsError.ThrowExceptionForHR(mediaPosition.put_CurrentPosition(endPosition));
        }

        public double getFramePerSecond()
        {
            double result = 0;
            this.mediaDet.get_FrameRate(out result);
            if (result != 0)
                return result;

            this.basicVideo.get_AvgTimePerFrame(out result);
            if (result != 0)
                return 1 / result;

            if (result == 0)
                result = 25.0;

            return result;
        }

        public double getPosition()
        {
            double result = 0;
            this.mediaPosition.get_CurrentPosition(out result);
            return result;
        }

        public double getDuration()
        {
            double result = 0;
            this.mediaPosition.get_Duration(out result);
            return result;
        }

        public void setVolume(int volume)
        {
            this.basicAudio.put_Volume(volume);
        }

        public void setPosition(int frame)
        {
            double fps = this.getFramePerSecond();
            double currentPosition = frame / fps;
            this.mediaPosition.put_CurrentPosition(currentPosition);
        }

        public void setPosition(double position)
        {
            double duration;
            this.mediaPosition.get_Duration(out duration);
            double newPosition = Math.Max(0, Math.Min(duration, position));
            this.mediaPosition.put_CurrentPosition(newPosition);
        }
    }




    public class MediaManager
    {
        public MediaManager()
        {
            this.videos = new Hashtable();
        }

        public void AddVideo(MediaPlayer video, string filename)
        {
            if (!this.videos.Contains(filename))
                this.videos.Add(filename, video);
        }

        public MediaPlayer GetVideo(String filename)
        {
            return (MediaPlayer)videos[filename];
        }

        public void CloseCurrentVideo()
        {
            if (this.currentMedia != null)
            {
                this.currentMedia.Stop();
                this.currentMedia.Dispose();
                this.currentMedia = null;
            }
        }

        public void RemoveVideo(string filename)
        {
            MediaPlayer video = this.GetVideo(filename);
            video.Dispose();
            video = null;

            this.videos.Remove(filename);
        }

        public void SetCurrentVideo(string filename)
        {
            this.currentMedia = (MediaPlayer)videos[filename];
        }

        public void Clear()
        {
            this.videos.Clear();
        }

        public MediaPlayer currentMedia;

        public Hashtable videos;
    }



    public class MediaInfo
    {
        public MediaInfo()
        {
            this.MediaWidth = 0;
            this.MediaHeight = 0;
            this.MediaStride = 0;
            this.MediaBitCount = 0;
            this.fps = 0;
            this.duration = 0;
            this.sFileName = string.Empty;
            this.sFilePath = string.Empty;
        }

        public int MediaWidth;
        public int MediaHeight;
        public int MediaStride;
        public int MediaBitCount;
        public double fps;
        public double duration;
        public string sFileName;
        public string sFilePath;


    }

}
