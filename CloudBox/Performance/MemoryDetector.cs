using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CloudBox.EasyHook;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace CloudBox.MemoryDr
{
    public class MemoryDetector
    {
        MemoryInfo m_mainMemory;
        Dictionary<string, MemoryInfo> m_moduleMemorys;
        Form m_mainForm;
        UCMemoryMonitor m_memoryForm;
        bool m_isMonitoring;

        public Dictionary<string, MemoryInfo> ModuleMemorys
        {
            get { return m_moduleMemorys; }
            set { m_moduleMemorys = value; }
        }
        public MemoryInfo MainMemory
        {
            get { return m_mainMemory; }
            set { m_mainMemory = value; }
        }

        private MemoryDetector()
        {
        }

        static MemoryDetector m_instance = new MemoryDetector();

        public static MemoryDetector Instance
        {
            get { return m_instance; }
            set {}
        }

        void update()
        {
            Process currentProcess = Process.GetCurrentProcess();
            m_mainMemory.update(currentProcess.WorkingSet64);

            ProcessModuleCollection myProcessModuleCollection = currentProcess.Modules;
            // Display the 'ModuleMemorySize' of each of the modules.
            ProcessModule myProcessModule;
            for (int i = 0; i < myProcessModuleCollection.Count; i++)
            {
                myProcessModule = myProcessModuleCollection[i];
                if (m_moduleMemorys.Keys.Contains(myProcessModule.ModuleName))
                    m_moduleMemorys[myProcessModule.ModuleName].update(myProcessModule.ModuleMemorySize);
                else
                {
                    MemoryInfo mem = new MemoryInfo();
                    mem.Name = myProcessModule.ModuleName;
                    mem.update(myProcessModule.ModuleMemorySize);
                    m_moduleMemorys[myProcessModule.ModuleName] = mem;
                }
            }
        }

        public void initialize()
        {
            m_mainMemory = new MemoryInfo();
            m_mainMemory.Name = "Main Process";

            m_moduleMemorys = new Dictionary<string, MemoryInfo>();

            m_isMonitoring = true;
            Thread t = new Thread(new ThreadStart(this.updateThread));
            t.Start();
        }

        public void initialize(Form form)
        {
            initialize();

            m_mainForm = form;
            m_memoryForm = new UCMemoryMonitor();
            form.Controls.Add(m_memoryForm);
            m_memoryForm.Hide();

            HookManager.Instance.RegisterHook(HookType.WH_KEYBOARD, new CustomHookProc.HookProcHandler(KeyboardHookProc));
        }

        public void destory()
        {
            m_isMonitoring = false;
            m_mainForm.Controls.Remove(m_memoryForm);
            m_memoryForm.Dispose();
        }

        void updateThread()
        {
            while (m_isMonitoring)
            {
                try
                {
                    update();
                }catch{}
                Thread.Sleep(500);
            }
        }

        void KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            KeyStateInfo ctrlKey = KeyboardInfo.GetKeyState(Keys.ControlKey);
            KeyStateInfo altKey = KeyboardInfo.GetKeyState(Keys.Alt);
            KeyStateInfo mKey = KeyboardInfo.GetKeyState(Keys.M);

            if (ctrlKey.IsPressed && altKey.IsPressed && mKey.IsPressed && m_memoryForm != null)
            {
                m_memoryForm.Size = new Size(m_mainForm.Width-10, m_mainForm.Height-10);
                m_memoryForm.Show();
            }
        }
    }
}
