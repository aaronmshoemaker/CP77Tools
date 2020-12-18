﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WolvenKit.Common.Services;
using CP77Tools.UI;

namespace CP77Tools.UI.Functionality
{
    public class Logging
    {
        private MainWindow App_UI;

        public Logging(MainWindow mainWindow)        {            this.App_UI = mainWindow;        }


        public void UI_Logger_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            Trace.Write(e.PropertyName);
        }

        public void UI_Logger_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is LoggerService _logger)
            {
                switch (e.PropertyName)
                {
                    case "Progress":
                        {
                            UIProgressCounter(_logger); break;
                        }
                    default:
                        break;
                }
            }
            else
            {
                Trace.Write(e.PropertyName);
            }
        }

        public void UI_Logger_OnStringLogged(object sender, LogStringEventArgs e)
        {
            Trace.Write(e.Message + e.Logtype);
        }

        


        private int TaskCounter = 0;
  

        public void UIProgressCounter(LoggerService _logger)
        {
            TaskCounter += 1;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                Logtype TYPE = _logger.Logtype;
                var CURRENTTASK = TaskCounter;
                string OUTPUTSTRING = "[" + TYPE.ToString() + "]" + " - Working on Task : " + CURRENTTASK + Environment.NewLine;
                App_UI.UIElement_Progressbar.Value += _logger.Progress.Item1;
                App_UI.UIElement_ProgressOutput.Text = OUTPUTSTRING;

            }));
        }

        public void TaskFinished(MainWindow.TaskType CurrentTaskType)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                App_UI.UIElement_Progressbar.Value = 0;
                App_UI.UIElement_ProgressOutput.Text = "[Normal] - Finished : " + CurrentTaskType.ToString();
                TaskCounter = 0;
            }));
        }
    }
}
