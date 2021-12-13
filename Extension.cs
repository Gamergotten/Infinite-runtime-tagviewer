using System;
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
                System.Reflection.FieldInfo? leftField = typeof(Window).GetField("_actualLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double) leftField.GetValue(window);
            }
            else
            {
                return window.Left;
            }
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
                System.Reflection.FieldInfo? topField = typeof(Window).GetField("_actualTop", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (double) topField.GetValue(window);
            }
            else
            {
                return window.Top;
            }
        }

        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        public static void UpdateBit(ref this byte aByte, int pos, bool value)
        {
            if (value)
            {
                //left-shift 1, then bitwise OR
                aByte = (byte) (aByte | (1 << pos));
            }
            else
            {
                //left-shift 1, then take complement, then bitwise AND
                aByte = (byte) (aByte & ~(1 << pos));
            }
        }

        // public static byte SetBit(this byte target, int field, bool value)
        // {
        //     if (value) //set value
        //     {
        //         return (byte) (target | field);
        //     }
        //     else //clear value
        //     {
        //         return (byte) (target & (~field));
        //     }
        // }
    }
}
