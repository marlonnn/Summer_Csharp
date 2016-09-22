using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Mdi
{
    public partial class Form : System.Windows.Forms.Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetSystemMetrics(int nIndex);

        private int _boardHeight = 10;
        private int _boardWidth = 10;

        private int _childFHeight = 250;
        private int _childFWidth = 250;

        private int _colsPerRow = 4;

        private List<ChildForm> _childForms;
        private int _formIndex = 1;

        private List<int> _topLists;
        private int _scrollGap = 0;
        private int _formCount = 0;

        public Form()
        {
            IsMdiContainer = true;
            InitializeComponent();

            _childForms = new List<ChildForm>();
            _topLists = new List<int>();
            this.MouseWheel += Form_MouseWheel;
            this.FormClosed += Form_FormClosed;
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                //Zoom out/in
            }
            else if (MdiClient.Bounds.Contains(e.Location))
            {
                ScrollForms(e.Delta);
                YAxisOffset();
                var v1 = e.Delta;
            }
        }

        private void ScrollForms(int delta)
        {
            if (_childForms.Count == 0) return;
            List<ChildForm> forms = _childForms;
            ChildForm lastForm = forms.Last();
            if (lastForm.Bottom + delta < MdiClient.Height) delta = MdiClient.Height - lastForm.Bottom;

            ChildForm firstForm = forms.First();
            if (firstForm.Top + delta > 0) delta = -firstForm.Top;

            foreach (ChildForm form in forms)
            {
                form.Top += delta;
            }
        }

        public MdiClient MdiClient
        {
            get { return this.Controls.OfType<MdiClient>().Select(ctrl => ctrl as MdiClient).FirstOrDefault(); }
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            _formCount++;
            ChildForm childForm = new ChildForm(new Size(_childFWidth, _childFHeight), _formIndex);
            _childForms.Add(childForm);
            childForm.Visible = false;
            childForm.MdiParent = Program.MainForm;
            childForm.Activate();
            childForm.Visible = true;
            childForm.Location = CalculateChildLocation();
            childForm.Show();
            _formIndex++;
            childForm.FormClosed += ChildForm_FormClosed;
        }

        private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChildForm childForm = (ChildForm)sender;
            //int index = _childForms.IndexOf(childForm);
            _childForms.Remove(childForm);
            //_topLists.RemoveAt(index - 1);
        }

        private void YAxisOffset()
        {
            var currentForm = _childForms[_childForms.Count - 1];
            var activeForm = (ChildForm)this.ActiveMdiChild;
            var v1 = activeForm.Top;
            var v2 = currentForm.Top;

            int index = _childForms.IndexOf(activeForm);
            int lastTop = _topLists[index];
            _scrollGap = lastTop - activeForm.Top;
        }

        private void AutoSizeLayout (ChildForm childForm)
        {
            List<Rectangle> rects = new List<Rectangle>();
            for (int i=0; i<_childForms.Count; i++)
            {

            }
        }

        private Point CalculateChildLocation()
        {
            //int formCount = _childForms.Count;
            int currentCols = (_formCount - 1) % _colsPerRow;//current columns
            var currentRows = (_formCount - 1) / _colsPerRow;
            //int currentRows = (v1 == 0) ? v1 : (v1 + 1);

            Point point = new Point(currentCols * _childFWidth, currentRows * _childFHeight - _scrollGap);
            _topLists.Add(currentRows * _childFHeight);
            return point;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = _childForms.Count;
            if (index > 0)
            {
                ChildForm childForm = _childForms[index - 1];
                _childForms.RemoveAt(index - 1);
                _topLists.RemoveAt(index - 1);
                childForm.Close();
                _formIndex--;
                _formCount--;
            }

        }

        private void CalculateChildFormSize()
        {
            var nonClientInfo = new ChildForm.NonClientInfo();
            _boardHeight = nonClientInfo.TotalHeight;
            _boardWidth = nonClientInfo.TotalWidth;

            Size childFormSize = new Size(_childFWidth + _boardWidth, _childFHeight = _boardHeight);
        }

        private void AutoSizeLayout(Size childFormSize)
        {
            // determine cols of a row
            int totalWidth = MdiClient.Width - (MdiClient.IsVScrollVisible() ? NativeMethods.GetSystemMetrics(NativeMethods.SM_CXVSCROLL) : 0);
            _colsPerRow = totalWidth / childFormSize.Width;
            if (_colsPerRow < 1)
            {
                _colsPerRow = 1;
            }
        }
    }
}
