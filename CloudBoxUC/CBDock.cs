using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;

namespace CloudBox.Controller
{
    public class CBDock : UserControl
    {
        const int yShift = 15;
        const int tableHeight = 10;
        const int xShift = 5;

        //static int componentWidth = 32 * 10 + 20;
        //static int componentHeight = 48 + 10;

        const int StartX = 10;
        const int StartY = 15;

        List<CBDockItem> m_Items;

        //[Category("Dock Item"), Description("Dock items")]
        //public List<CBDockItem> Items
        //{
        //    get { return m_Items; }
        //    set { m_Items = value; }
        //}

        private Size m_ImageSize = new Size(32, 32);

        public Size ImageSize
        {
            get { return m_ImageSize; }
            set { m_ImageSize = value; }
        }

        private Size m_ImageNormalSize = new Size(32, 32);
        [Category("Dock Items"),
         DefaultValue(typeof(Size), "32, 32"),
         Description("The normal size of the image to be displayed on the" +
                     "icon. This property defaults to 32x32.")]
        public Size ImageNormalSize
        {
            get { return m_ImageNormalSize; }
            set
            {
                if (value.Width > m_ImageEnlargeSize.Width ||
                    value.Height > m_ImageEnlargeSize.Height)
                    throw new ArgumentOutOfRangeException("Can't large more than ImageEnlargeSize");
                m_ImageNormalSize = value;
            }
        }

        private Size m_ImageEnlargeSize = new Size(48, 48);
        [Category("Dock Items"),
         DefaultValue(typeof(Size), "48, 48"),
         Description("The max size of the image to be displayed on the" +
                     "icon. This property defaults to 48x48.")]
        public Size ImageEnlargeSize
        {
            get { return m_ImageEnlargeSize; }
            set
            {
                if (value.Width < m_ImageNormalSize.Width ||
                    value.Height < m_ImageNormalSize.Height)
                    throw new ArgumentOutOfRangeException("Can't less than ImageNormalSize");
                m_ImageEnlargeSize = value;
            }
        }

        public CBDock()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.DoubleBuffer, true);
            m_Items = new List<CBDockItem>();
            AddDockItem("CloudBoxUC.Resources.icon1.png");
            AddDockItem("CloudBoxUC.Resources.icon2.png");
            AddDockItem("CloudBoxUC.Resources.icon3.png");
            AddDockItem("CloudBoxUC.Resources.icon4.png");
            AddDockItem("CloudBoxUC.Resources.icon2.png");
            AddDockItem("CloudBoxUC.Resources.icon1.png");
            AddDockItem("CloudBoxUC.Resources.icon4.png");
            AddDockItem("CloudBoxUC.Resources.icon3.png");
        }

        public void AddDockItem(Image image)
        {
            CBDockItem item = new CBDockItem();
            item.Image = image;
            item.ImageNormalSize = ImageNormalSize;
            item.ImageEnlargeSize = ImageEnlargeSize;
            int size = ImageNormalSize.Width + (ImageEnlargeSize.Width - ImageNormalSize.Width) / 2;
            item.ImageNormalLocation = new Point(StartX + size * m_Items.Count, StartY);
            item.AnimationTrigger += new EventHandler<EventArgs>(item_AnimationTrigger);
            m_Items.Add(item);
        }

        public void AddDockItem(string resourceName)
        {
            Assembly thisExe = Assembly.GetExecutingAssembly();
            Stream file = thisExe.GetManifestResourceStream(resourceName);
            Image image = Image.FromStream(file);
            AddDockItem(image);
        }

        void item_AnimationTrigger(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        #region -  Component Designer generated code  -

        private void InitializeComponent()
        {
            // 
            // CBDock
            // 
            this.Name = "CBDock";
            this.Size = new System.Drawing.Size(340, 58);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CBDock_Paint);
            this.Resize += new EventHandler(CBDock_Resize);
            this.MouseEnter += new System.EventHandler(this.CBDock_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.CBDock_MouseLeave);
            this.MouseMove += new MouseEventHandler(CBDock_MouseMove);
        }

        #endregion

        private void DrawOuterStroke(Graphics g)
        {
            Rectangle rect = this.ClientRectangle;
            rect.X += xShift;
            rect.Width -= (xShift*2);
            rect.Y = rect.Height - tableHeight - 3;
            rect.Height = tableHeight;
            using (GraphicsPath rr = RoundRect(rect, 0, 0, 0, 0))
            {
                using (Pen p = new Pen(Color.Green))
                {
                    g.DrawPath(p, rr);
                }
            }
        }

        private void DrawInnerStroke(Graphics g)
        {
            Rectangle rect = this.ClientRectangle;
            rect.X += xShift;
            rect.Width -= (xShift*2);
            rect.Y = rect.Height - tableHeight - 3;
            rect.Height = tableHeight;
            using (GraphicsPath rr = RoundRect(rect, 0, 0, 0, 0))
            {
                using (Pen p = new Pen(Color.SlateBlue))
                {
                    g.DrawPath(p, rr);
                }
            }
        }

        private void DrawTable(Graphics g)
        {
            Rectangle rect = this.ClientRectangle;
            rect.X += xShift;
            rect.Width -= (xShift*2);
            rect.Y = rect.Height - tableHeight - 3;
            rect.Height = tableHeight;

            GraphicsPath rr = new GraphicsPath();
            rr.AddLine(new Point(rect.X + 25 + xShift, rect.Y - yShift), new Point(rect.Width - 25, rect.Y - yShift));
            rr.AddLine(new Point(rect.Width - 25, rect.Y - yShift), new Point(rect.Width + xShift, rect.Y));
            rr.AddLine(new Point(rect.Width, rect.Y), new Point(rect.X, rect.Y));
            rr.AddLine(new Point(rect.X, rect.Y), new Point(rect.X + 25 + xShift, rect.Y - yShift));
            
            
            //g.FillRectangle(Brushes.Blue, r);
            using (GraphicsPath p1 = RoundRect(rect, 0, 0, 0, 0))
            {
                using (LinearGradientBrush sb = new LinearGradientBrush(new Point(0,0),new Point(1,1),Color.SkyBlue,Color.SlateBlue))
                {
                    g.FillPath(sb, p1);
                }
                using (SolidBrush sb = new SolidBrush(Color.SkyBlue))
                {
                    g.FillPath(sb, rr);
                }
                //SetClip(g);
            }
        }

        private void DrawImage(Graphics g)
        {
            foreach (CBDockItem item in m_Items)
            {
                //Rectangle rect = new Rectangle(StartX+32*i,StartY,32,32);
                Rectangle rect = new Rectangle(item.ImageLocation, item.ImageSize);
                g.DrawImage(item.Image, rect);
            }
        }

        private void CBDock_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            DrawTable(e.Graphics);
            DrawOuterStroke(e.Graphics);
            DrawInnerStroke(e.Graphics);
            DrawImage(e.Graphics);
        }

        private void CBDock_Resize(object sender, EventArgs e)
        {
        }

        private void CBDock_MouseEnter(object sender, EventArgs e)
        {
            
        }
        private void CBDock_MouseLeave(object sender, EventArgs e)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                CBDockItem item = m_Items[i];
                item.Narrow();
            }
        }

        void CBDock_MouseMove(object sender, MouseEventArgs e)
        {
            int itemIndex = -1;
            for (int i = 0; i < m_Items.Count; i++)
            {
                CBDockItem item = m_Items[i];
                bool inX = false;
                bool inY = false;
                if (e.X >= item.ImageLocation.X && e.X <= (item.ImageLocation.X + item.ImageSize.Width))
                {
                    inX = true;
                }
                if (e.Y >= item.ImageLocation.Y && e.Y <= (item.ImageLocation.Y + item.ImageSize.Height))
                {
                    inY = true;
                }
                if (inX && inY)
                {
                    itemIndex = i;
                    break;
                }
            }
            if (itemIndex == -1)
            {
                for (int i = 0; i < m_Items.Count; i++)
                {
                    CBDockItem item = m_Items[i];
                    item.Narrow();
                }
            }
            else
            {
                for (int i = 0; i < m_Items.Count; i++)
                {
                    CBDockItem item = m_Items[i];
                    if ((itemIndex - 2) == i || (itemIndex + 2) == i)
                    {
                        item.Enlarge(DockItemAction.EnlargeMax2);
                    }
                    else if ((itemIndex - 1) == i || (itemIndex + 1) == i)
                    {
                        item.Enlarge(DockItemAction.EnlargeMax1);
                    }
                    else if (itemIndex == i)
                    {
                        item.Enlarge(DockItemAction.EnlargeMax);
                    }
                    else
                    {
                        item.Narrow();
                    }
                }
            }
        }

        private GraphicsPath RoundRect(RectangleF r, float r1, float r2, float r3, float r4)
        {
            float x = r.X, y = r.Y, w = r.Width, h = r.Height;
            GraphicsPath rr = new GraphicsPath();
            rr.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
            rr.AddLine(x + r1, y, x + w - r2, y);
            rr.AddBezier(x + w - r2, y, x + w, y, x + w, y + r2, x + w, y + r2);
            rr.AddLine(x + w, y + r2, x + w, y + h - r3);
            rr.AddBezier(x + w, y + h - r3, x + w, y + h, x + w - r3, y + h, x + w - r3, y + h);
            rr.AddLine(x + w - r3, y + h, x + r4, y + h);
            rr.AddBezier(x + r4, y + h, x, y + h, x, y + h - r4, x, y + h - r4);
            rr.AddLine(x, y + h - r4, x, y + r1);
            return rr;
        }
    }

}
