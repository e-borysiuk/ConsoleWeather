using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ConsoleDraw;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows.Base;

namespace ConsoleWeather
{
    class Program
    {
        static void Main(string[] args)
        {
            if (new FileInfo("fav.dat").Exists && new FileInfo("fav.dat").Length > 0)
            {
                WindowManager.UpdateWindow(84, 28);
                WindowManager.UpdateWindow(84, 28);
            }
            else
            {
                WindowManager.UpdateWindow(84, 20);
                WindowManager.UpdateWindow(84, 20);
            }

            WindowManager.SetWindowTitle("ConsoleWeather");
            new MenuWindow();
        }
    }
}