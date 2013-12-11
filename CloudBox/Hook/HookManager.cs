/*
* Copyright (c) 2011, Cloud Hsu
* All rights reserved.
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the name of the Cloud Hsu nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY CLOUD HSU "AS IS" AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL THE REGENTS AND CONTRIBUTORS BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
#if !WindowsCE
namespace CloudBox.EasyHook
{
    public enum HookType
    {
        WH_MSGFILTER = -1,
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }

    public sealed class CustomHookProc
    {
        private CustomHookProc(){}
        public delegate void HookProcHandler(int nCode, IntPtr wParam, IntPtr lParam);
    }

    public sealed class HookManager
    {
        private HookManager(){}

        static readonly HookManager m_instance = new HookManager();
        Dictionary<HookType, _HookProc> m_hooks = new Dictionary<HookType, _HookProc>();

        public static HookManager Instance
        {
            get { return m_instance; }
        }

        public void RegisterHook(HookType a_eHookType, CustomHookProc.HookProcHandler a_pHookProc)
        {
            if(!m_hooks.ContainsKey(a_eHookType))
            {
                m_hooks.Add(a_eHookType, new _HookProc(a_eHookType, a_pHookProc));
            }
            else
            {
                throw new Exception(string.Format("{0} already exist!", a_eHookType.ToString()));
            }
        }
        public void Unregister(HookType a_eHookType)
        {
            m_hooks.Remove(a_eHookType);
        }
    }

    class _HookProc
    {
        #region "Declare API for Hook"
        [DllImport("user32.dll", CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.StdCall)]
        static extern int SetWindowsHookEx(int idHook, _HookProcHandler lpfn,
        IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.StdCall)]
        static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
        CallingConvention = CallingConvention.StdCall)]
        static extern int CallNextHookEx(int idHook, int nCode,
        IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();
        #endregion

        #region "Hook Proc"
        int MyHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (m_CustomHookProc != null)
                m_CustomHookProc(nCode, wParam, lParam);
            return CallNextHookEx(m_HookHandle, nCode, wParam, lParam);
        }
        #endregion

        CustomHookProc.HookProcHandler m_CustomHookProc;
        delegate int _HookProcHandler(int nCode, IntPtr wParam, IntPtr lParam);
        _HookProcHandler m_KbdHookProc;
        int m_HookHandle = 0;

        public _HookProc(HookType a_eHookType, CustomHookProc.HookProcHandler a_pHookProc)
        {
            m_CustomHookProc = a_pHookProc;
            m_KbdHookProc = new _HookProcHandler(MyHookProc);
            m_HookHandle = SetWindowsHookEx((int)a_eHookType, m_KbdHookProc, IntPtr.Zero, GetCurrentThreadId());
            if (m_HookHandle == 0)
            {
                throw new Exception(string.Format("Hook {0} to {1} Error:{2}", a_eHookType.ToString(), a_pHookProc.ToString(), Marshal.GetLastWin32Error()));
            }
        }
        ~_HookProc()
        {
            UnhookWindowsHookEx(m_HookHandle);
            Debug.WriteLine(Marshal.GetLastWin32Error());
            m_HookHandle = 0;
        }
    }
}
#endif