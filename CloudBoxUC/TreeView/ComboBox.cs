using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CloudBox.Controller
{
	public class FloatComboBox : NameObjectComboBox<float>
	{
	}
	public class NameObjectComboBox<T> : System.Windows.Forms.ComboBox
	{
		CloudBox.Controller.NameObjectCollection<T> m_items = new CloudBox.Controller.NameObjectCollection<T>();
		public new CloudBox.Controller.NameObjectCollection<T> Items
		{
			get { return m_items; }
			set
			{
				m_items = value;
				DataSource = m_items;
			}
		}
		public new CloudBox.Controller.NameObject<T> SelectedItem
		{
			get { return base.SelectedItem as CloudBox.Controller.NameObject<T>; }
			set { base.SelectedItem = value; } 
		}
		public NameObjectComboBox()
		{
			DisplayMember = "Name";
			ValueMember = "Object";
		}
		protected override void OnLeave(EventArgs e)
		{
			if (DataBindings.Count > 0)
				DataBindings[0].WriteValue();
			base.OnLeave(e);
		}
	}
}
