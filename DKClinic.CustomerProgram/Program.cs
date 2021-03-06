﻿using DKClinic.Data;
using System;
using System.Windows.Forms;

namespace DKClinic.CustomerProgram
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DKClinicEntities.Initialize();
            
            Application.Run(new MainForm());
        }
    }
}
