﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using RawInput_dll;
using System.Globalization;
using System.IO;
namespace MultiWindows
{
    public partial class Form1 : Form
    {
        private readonly RawInput _rawinput;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        public static string GetWindowTitle(IntPtr hWnd)
        {
            var length = GetWindowTextLength(hWnd) + 1;
            var title = new StringBuilder(length);
            GetWindowText(hWnd, title, length);
            return title.ToString();
        }

        static string DofusPath = File.ReadAllText("DofusPath.txt");
    

        static List<ProcessInfo> DofusProcess = new List<ProcessInfo>();
        class ProcessInfo {
            public TabPage tabpage { get; set; }
            public IntPtr handle { get; set; }
        }
        static void TabName() {
            while (true) {
                try {
                    foreach (ProcessInfo item in DofusProcess)
                        item.tabpage.BeginInvoke((MethodInvoker)delegate () { item.tabpage.Text = GetWindowTitle(item.handle).Split(new string[] { " - " }, StringSplitOptions.None)[0]; });
                }
                catch { }
                Thread.Sleep(1);
            } 
        }

        public Form1()
        {
            new Thread(new ThreadStart(TabName)) { IsBackground = true }.Start();
            InitializeComponent();
            Rectangle SizeScreen = Screen.PrimaryScreen.Bounds;
            flatTabControl1.Size = new Size(SizeScreen.Width, SizeScreen.Height - 50);
            _rawinput = new RawInput(Handle, false);
            _rawinput.KeyPressed += OnKeyPressed;

        }
        private void OnKeyPressed(object sender, RawInputEventArg e)
        {
            int key = (e.KeyPressEvent.VKey - 112);
            if (key >= 0 && key <= 8 && flatTabControl1.TabPages.Count > key) {
               
                flatTabControl1.SelectTab(key);
            }
              
        }
 
        private void flatButton1_Click(object sender, EventArgs e) {
            Process Dofus = Process.Start(DofusPath);
            TabPage NewTabPage = new TabPage()
            {

                Name = "TabPage" + (flatTabControl1.TabCount + 1).ToString(),
                Text = GetWindowTitle(Dofus.MainWindowHandle).Split(new string[] { " - " }, StringSplitOptions.None)[0]
            };    
            flatTabControl1.TabPages.Add(NewTabPage);
            AttachDofus(NewTabPage, Dofus.Id);
        }

        public static void AttachDofus(TabPage tabPage, int Id)
        {
            Process _Process;
            IntPtr _handleParent;
            IntPtr _handleWindows = IntPtr.Zero;
            IntPtr hWndParent = IntPtr.Zero;
            if (_handleWindows != IntPtr.Zero)
                return;
            _Process = Process.GetProcessById(Id);
            while (_handleWindows == IntPtr.Zero) {
                _Process.WaitForInputIdle(1000);
                _Process.Refresh();
                if (_Process.HasExited)
                    return;
                _handleWindows = _Process.MainWindowHandle;
            }
            _handleParent = SetParent(_handleWindows, tabPage.Handle);
            DofusProcess.Add(new ProcessInfo() { handle = _handleWindows, tabpage = tabPage });
            MoveWindow(_handleWindows, -10, -50, tabPage.Width + 20, tabPage.Height + 60, true);
        }
    }
}