﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CsharpCocBot
{
    public static class GlobalVariables
    {
        public static Bitmap hBitmap;
        public static Bitmap hHBitmap;

        public static int HWnD;

        public static string configPath = @"COCBot\config.ini";
        public static string logPath;
        public static int hLogFileHandle;
        public static bool restart = false;
        public static bool runState = false;
    }
}
