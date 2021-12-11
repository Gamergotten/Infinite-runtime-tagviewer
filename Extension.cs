﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Assembly69
{
    public static class Extension
    {
        /// <summary>
        /// Gets the window left.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns></returns>
        public static double GetWindowLeft(this Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                var leftField = typeof(Window).GetField("_actualLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double) leftField.GetValue(window);
            }
            else
                return window.Left;
        }

        /// <summary>
        /// Gets the window top.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns></returns>
        public static double GetWindowTop(this Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                var topField = typeof(Window).GetField("_actualTop", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double) topField.GetValue(window);
            }
            else
                return window.Top;
        }

        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }
    }
}