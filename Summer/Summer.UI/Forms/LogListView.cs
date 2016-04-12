using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Summer.System.Collections.Concurrent;

namespace Summer.UI.Forms
{
    public class LogListView : ListView
    {
        private Timer timer;
        public Timer Timer
        {
            get
            {
                return timer;
            }
            set
            {
                timer = value;
                if( timer!=null)
                    value.Tick +=new EventHandler(timer_Tick);
            }
        }

        public int MaxLogRecords { get; set; }

        private ConcurrentQueue<string[]> queue = new ConcurrentQueue<string[]>();

        public void AppendLog(List<string[]> logs)
        {
            queue.Push(logs);            
        }

        public void AppendLog(string[] log)
        {
            queue.Push(log);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (isPause == false)
            {
                List<string[]> data = queue.PopAll();
                if (data.Count > 0)
                {
                    UpdateListView(data);
                }
            }
        }

        protected void UpdateListView(List<string[]> logs)
        {
            ListViewItem lastLvi = null;
            this.BeginUpdate();
            if (logs.Count > MaxLogRecords)
            {
                this.Items.Clear();
                for (int index = logs.Count - MaxLogRecords; index < logs.Count; index++)
                {
                    string[] msg = logs[index];
                    ListViewItem lvi = AppendItem(msg);
                    if (lvi != null)
                    {
                        this.Items.Add(lvi);
                        lastLvi = lvi;
                    }
                }
            }
            else
            {
                if (this.Items.Count > MaxLogRecords+100)
                {
                    while (this.Items.Count > MaxLogRecords)
                    {
                        this.Items.RemoveAt(0);
                    }
                }
                foreach (var msg in logs)
                {
                    ListViewItem lvi = AppendItem(msg);
                    if( lvi != null)
                    {                        
                        this.Items.Add(lvi);
                        lastLvi = lvi;
                    }
                }
            }
            if( lastLvi != null )
                lastLvi.Selected = true;
            this.EndUpdate();
            this.EnsureVisible(this.Items.Count - 1);
        }

        protected ListViewItem AppendItem(string[] msg)
        {
            if (msg.Length == this.Columns.Count)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = msg[0];
                for (int i = 1; i < msg.Length; i++)
                {
                    lvi.SubItems.Add(msg[i]);
                }
                return lvi;
            }
            return null;
        }

        public LogListView()
        {
            // 开启双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

            this.MouseClick += new MouseEventHandler(this.logListView_MouseClick);

            this.FullRowSelect = true;
            this.GridLines = true;
            this.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.HideSelection = false;
            this.MultiSelect = false;
            this.Name = "logListView2";
            this.ShowGroups = false;
            this.TabIndex = 2;
            this.UseCompatibleStateImageBehavior = false;
            this.View = View.Details;

            MaxLogRecords = 300;
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        private bool isPause = false;

        private void logListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isPause = !isPause;
            }
        }
    }
}
