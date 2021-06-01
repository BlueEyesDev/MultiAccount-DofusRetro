using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
namespace MultiWindows
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            if (!File.Exists("DofusPath.txt"))
                new DofusPath().ShowDialog();
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}
