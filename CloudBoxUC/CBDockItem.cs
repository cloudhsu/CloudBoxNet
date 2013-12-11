using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CloudBox.Controller
{
    public enum DockItemState
    {
        Normal,
        Enlarging,
        Narrowing,
        Enlarged
    }

    public enum DockItemAction
    {
        EnlargeMax,
        EnlargeMax1,
        EnlargeMax2,
        Jump
    }

    //[Serializable]
    public class CBDockItem
    {
        public CBDockItem()
        {
            m_State = DockItemState.Normal;
            m_Enlarge.Interval = 50;
            m_Narrow.Interval = 50;
            m_Enlarge.Tick += new EventHandler(Enlarge_Tick);
            m_Narrow.Tick += new EventHandler(Narrow_Tick);
        }

        public event EventHandler<EventArgs> AnimationTrigger;

        void Animation()
        {
            EventHandler<EventArgs> myEvent = AnimationTrigger;
            if (myEvent != null)
            {
                myEvent(this, EventArgs.Empty);
            }
        }

        DockItemState m_State;
        private Timer m_Enlarge = new Timer();
        private Timer m_Narrow = new Timer();

        private Image m_Image;
        [Category("Image"),
         DefaultValue(null),
         Description("The image displayed on the button that " +
                     "is used to help the user identify" +
                     "it's function if the text is ambiguous.")]
        public Image Image
        {
            get { return m_Image; }
            set { m_Image = value; }
        }
        private Point m_ImageTargetLocation = new Point(0, 0);

        private Point m_ImageNormalLocation = new Point(0, 0);
        [Category("Image"),
         DefaultValue(typeof(Point), "0, 0"),
         Description("The location of image.")]
        public Point ImageNormalLocation
        {
            get { return m_ImageNormalLocation; }
            set
            {
                m_ImageNormalLocation = value;
                m_ImageLocation = value;
            }
        }

        private Point m_ImageLocation = new Point(0, 0);
        [Category("Image"),
         DefaultValue(typeof(Point), "0, 0"),
         Description("The location of image.")]
        public Point ImageLocation
        {
            get { return m_ImageLocation; }
            set { m_ImageLocation = value; }
        }

        private Size m_ImageSize = new Size(32, 32);
        [Category("Image"),
         DefaultValue(typeof(Size), "32, 32"),
         Description("The size of the image to be displayed on the" +
                     "icon. This property defaults to 32x32.")]
        public Size ImageSize
        {
            get { return m_ImageSize; }
            set { m_ImageSize = value; }
        }

        private Size m_ImageNormalSize = new Size(32, 32);
        [Category("Image"),
         DefaultValue(typeof(Size), "32, 32"),
         Description("The size of the image to be displayed on the" +
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
                m_ImageSize = value;
            }
        }
        private Size m_ImageTargetSize = new Size(48, 48);
        private Size m_ImageEnlargeSize = new Size(48, 48);
        [Category("Image"),
         DefaultValue(typeof(Size), "48, 48"),
         Description("The size of the image to be displayed on the" +
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

        public void Jump()
        {
        }

        public void Enlarge(DockItemAction action)
        {
            if (m_State == DockItemState.Enlarging)
                return;
            int range = ImageEnlargeSize.Width - m_ImageNormalSize.Width;
            m_ImageTargetLocation = m_ImageNormalLocation;
            if (action == DockItemAction.EnlargeMax2)
            {
                Size newSize = ImageEnlargeSize;
                newSize.Width = m_ImageNormalSize.Width + (ImageEnlargeSize.Width - m_ImageNormalSize.Width) / 3;
                newSize.Height = m_ImageNormalSize.Height + (ImageEnlargeSize.Height - m_ImageNormalSize.Height) / 3;
                m_ImageTargetSize = newSize;
                m_ImageTargetLocation.X = m_ImageTargetLocation.X - (range / 2) / 3;
                m_ImageTargetLocation.Y = m_ImageTargetLocation.Y - (range / 2) / 3;
            }
            else if (action == DockItemAction.EnlargeMax1)
            {
                Size newSize = ImageEnlargeSize;
                newSize.Width = m_ImageNormalSize.Width + (ImageEnlargeSize.Width - m_ImageNormalSize.Width) / 3 * 2;
                newSize.Height = m_ImageNormalSize.Height + (ImageEnlargeSize.Height - m_ImageNormalSize.Height) / 3 * 2;
                m_ImageTargetSize = newSize;
                m_ImageTargetLocation.X = m_ImageTargetLocation.X - (range / 2) / 3 * 2;
                m_ImageTargetLocation.Y = m_ImageTargetLocation.Y - (range / 2) / 3 * 2;
            }
            else
            {
                m_ImageTargetSize = ImageEnlargeSize;
                m_ImageTargetLocation.X = m_ImageTargetLocation.X - (range / 2);
                m_ImageTargetLocation.Y = m_ImageTargetLocation.Y - (range / 2);
            }
            m_State = DockItemState.Enlarging;
            m_Narrow.Stop();
            m_Enlarge.Start();
        }

        public void Narrow()
        {
            if (m_State == DockItemState.Normal || m_State == DockItemState.Narrowing)
                return;
            m_ImageTargetSize = ImageNormalSize;
            m_State = DockItemState.Narrowing;
            m_Enlarge.Stop();
            m_Narrow.Start();
        }

        private void Enlarge_Tick(object sender, EventArgs e)
        {
            if (ImageSize.Width >= m_ImageTargetSize.Width)
            {
                m_ImageSize = m_ImageTargetSize;
                m_State = DockItemState.Enlarged;
                m_ImageLocation = m_ImageTargetLocation;
                m_Enlarge.Stop();
            }
            else
            {
                m_ImageSize.Width += 2;
                m_ImageSize.Height += 2;
                if (!(m_ImageLocation.X == m_ImageTargetLocation.X && m_ImageLocation.Y == m_ImageTargetLocation.Y))
                {
                    m_ImageLocation.X -= 1;
                    m_ImageLocation.Y -= 1;
                }
            }
            Animation();
        }

        private void Narrow_Tick(object sender, EventArgs e)
        {
            if (ImageSize.Width <= m_ImageTargetSize.Width)
            {
                m_ImageSize = m_ImageTargetSize;
                m_ImageLocation = m_ImageNormalLocation;
                m_State = DockItemState.Normal;
                m_Narrow.Stop();
            }
            else
            {
                m_ImageSize.Width -= 2;
                m_ImageSize.Height -= 2;
                if (!(m_ImageLocation.X == m_ImageNormalLocation.X && m_ImageLocation.Y == m_ImageNormalLocation.Y))
                {
                    m_ImageLocation.X += 1;
                    m_ImageLocation.Y += 1;
                }
            }
            Animation();
        }
    }
}
