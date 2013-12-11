using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CloudBox.MemoryDr
{
    public partial class UCMemoryMonitor : UserControl
    {
        bool m_isFirst;

        public UCMemoryMonitor()
        {
            InitializeComponent();
            m_isFirst = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblCurrent.Text = MemoryDetector.Instance.MainMemory.GetCurrentKB();
            lblMax.Text = MemoryDetector.Instance.MainMemory.GetMaximumKB();

            if (m_isFirst)
            {
                lsvModule.Items.Clear();
                foreach (var mem in MemoryDetector.Instance.ModuleMemorys.Values)
                {
                    ListViewItem item = new ListViewItem(new string[] { mem.Name, mem.GetCurrentKB(), mem.GetMaximumKB() });
                    lsvModule.Items.Add(item);
                }
                m_isFirst = false;
            }
            else
            {
                foreach (var mem in MemoryDetector.Instance.ModuleMemorys.Values)
                {
                    foreach (ListViewItem lvi in lsvModule.Items)
                    {
                        if (lvi.SubItems[0].Text.Equals(mem.Name))
                        {
                            lvi.SubItems[1].Text = mem.GetCurrentKB();
                            lvi.SubItems[2].Text = mem.GetMaximumKB();
                        }
                    }


                }
            }
        }

        private void UCMemoryMonitor_Load(object sender, EventArgs e)
        {

        }

        private void UCMemoryMonitor_ControlRemoved(object sender, ControlEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void UCMemoryMonitor_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                showView();
            }
            else
            {
                hideView();
            }
        }

        void showView()
        {
            btnClose.Location = new Point(this.Width - btnClose.Width, 0);
            this.BringToFront();
            timer1.Start();
        }

        void hideView()
        {
            timer1.Stop();
        }
    }
}
