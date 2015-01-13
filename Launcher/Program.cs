﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BoggleServer;
using BoggleClientView;
using System.Windows;

namespace Launcher
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Thread(() => new BoggleServerClass(new string[2] { "60", "../../../Resources/Dictionary/Words.txt"})).Start();

             //James was here when we pulled this from a MSDN blog
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = @"..\..\..\BoggleClient\bin\Debug\BoggleClient.exe";
            new Thread(() => p.Start()).Start();

            System.Diagnostics.Process p1 = new System.Diagnostics.Process();
            p1.StartInfo.FileName = @"..\..\..\BoggleClient\bin\Debug\BoggleClient.exe";
            new Thread(() => p1.Start()).Start();

            Console.ReadLine();

        }
    }
}
