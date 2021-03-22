using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;

namespace ComputerManager
{
    class Computer
    {
        public class Mouse
        {
            [Flags]
            public enum MouseEventFlags
            {
                LeftDown = 0x00000002,
                LeftUp = 0x00000004,
                MiddleDown = 0x00000020,
                MiddleUp = 0x00000040,
                Move = 0x00000001,
                Absolute = 0x00008000,
                RightDown = 0x00000008,
                RightUp = 0x00000010
            }

            [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool SetCursorPos(int x, int y);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool GetCursorPos(out MousePoint lpMousePoint);

            [DllImport("user32.dll")]
            private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

            public static void SetCursorPosition(int x, int y)
            {
                SetCursorPos(x, y);
            }

            public static void SetCursorPosition(MousePoint point)
            {
                SetCursorPos(point.X, point.Y);
            }

            public static MousePoint GetCursorPosition()
            {
                MousePoint currentMousePoint;
                var gotPoint = GetCursorPos(out currentMousePoint);
                if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
                return currentMousePoint;
            }

            public static void MouseEvent(MouseEventFlags value)
            {
                MousePoint position = GetCursorPosition();

                mouse_event
                    ((int)value,
                     position.X,
                     position.Y,
                     0,
                     0)
                    ;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MousePoint
            {
                public int X;
                public int Y;

                public MousePoint(int x, int y)
                {
                    X = x;
                    Y = y;
                }
            }
        }
        public static ulong GetMaxDiskSpace()
        {
            return new ComputerInfo().TotalPhysicalMemory;
        }

        public static int GetDrivesCount()
        {
            return new Microsoft.VisualBasic.Devices.Computer().FileSystem.Drives.Count;
        }

        public static List<System.IO.DriveInfo> GetDrives()
        {
            return new Microsoft.VisualBasic.Devices.Computer().FileSystem.Drives.ToList();
        }

        public static void SendKeys(string keys)
        {
            new Microsoft.VisualBasic.Devices.Computer().Keyboard.SendKeys(keys);
        }

        public static void MoveMouse(int x, int y)
        {
            Mouse.SetCursorPosition(x, y);
        }

        public static void ClickMouse(int click)
        {
            if (click == 0)
            {
                Mouse.MouseEvent(Mouse.MouseEventFlags.LeftDown);
                Mouse.MouseEvent(Mouse.MouseEventFlags.LeftUp);
            }
            else if (click == 1)
            {
                Mouse.MouseEvent(Mouse.MouseEventFlags.RightDown);
                Mouse.MouseEvent(Mouse.MouseEventFlags.RightUp);
            }
            else if (click == 2)
            {
                Mouse.MouseEvent(Mouse.MouseEventFlags.MiddleDown);
                Mouse.MouseEvent(Mouse.MouseEventFlags.MiddleUp);
            }
        }
    }
}
