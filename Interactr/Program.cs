﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interactr.View;
using Interactr.ViewModel;

namespace Interactr
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainViewModel vm = new MainViewModel();
            MainView view = new MainView();
            view.Show();
        }
    }
}
