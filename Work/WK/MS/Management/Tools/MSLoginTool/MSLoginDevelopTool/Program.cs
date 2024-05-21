using MSLoginDevelopTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSLoginTool
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            bool isFirstOpen;

            Mutex mutex = new Mutex(false, Application.ProductName, out isFirstOpen);

            if (isFirstOpen)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new DevelopLoginForm());
            }
            else
            {
                MessageBox.Show("应用程序重复开启，请检查!");
            }

            mutex.Dispose();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}