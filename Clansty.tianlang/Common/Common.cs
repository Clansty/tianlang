﻿using CornSDK;
using System;using System.Runtime.InteropServices;namespace Clansty.tianlang{    static class C    {        /*         * TODO:         */                internal const string Version = "4.1.3.0";//20200903#if DEBUG
        internal const long self = 2981882373;
#else
        internal const long self = 1980853671;
#endif

        internal static void Write(object text, ConsoleColor color = ConsoleColor.White)        {            Console.ForegroundColor = color;            Console.Write(text);            Console.ForegroundColor = ConsoleColor.White;        }        internal static void WriteLn(object text, ConsoleColor color = ConsoleColor.White)        {            Console.ForegroundColor = color;            Console.WriteLine(text);            Console.ForegroundColor = ConsoleColor.White;        }        internal static Corn Robot = null;    }}