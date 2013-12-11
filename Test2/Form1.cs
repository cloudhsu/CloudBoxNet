using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Test2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ArrayList ary = new ArrayList();
            ABC a = ary[0] as ABC;
            List<ABC> ary2 = new List<ABC>();
            ABC b = ary2[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form f = sender as Form;
            doA(new ABC());
            doA(new BCD());
            doA<ABC>(new ABC());
            doA<BCD>(new BCD());
        }

        void doA(A a)
        {


        }

        void doA(BCD a)
        {


        }

        void doA<T>(T a)
        {

        }


        interface A
        {
        }

        class ABC : A
        {
        }
        class BCD
        {
        }
    }
}
