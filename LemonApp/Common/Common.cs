﻿using System;using System.Runtime.InteropServices;namespace Clansty.tianlang{    static class C    {        /*         * TODO:         * 申請加群裏面的名字也校驗         * 2020届初中是实名信息有的，改不了新的 enrollment         */                internal const string Version = "4.0.1.1";//20200819        internal static bool recording = false;        internal static void Write(object text, ConsoleColor color = ConsoleColor.White)        {            Console.ForegroundColor = color;            Console.Write(text);            Console.ForegroundColor = ConsoleColor.White;        }        internal static void WriteLn(object text, ConsoleColor color = ConsoleColor.White)        {            Console.ForegroundColor = color;            Console.WriteLine(text);            Console.ForegroundColor = ConsoleColor.White;        }        [DllImport("kernel32.dll")]        internal static extern bool AllocConsole();        [DllImport("user32.dll")]        internal static extern bool SetProcessDPIAware();    }}