﻿using Microsoft.Win32;
using SkiaSharp.Views.Maui.Controls.Compatibility;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SkiaSharp.Views.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using KeyboardHookLite;

namespace AOdia5
{
    /*
     * AOdiaで使用するキーイベントを管理します
     */
    public static class AOdiaKeyBoard
    {

        private static bool windowIsActive = false;
        private static ModifierKey modifierKey = new ModifierKey();

        public static Page mainPage;

        public static void Init(Window window, Page page)
        {
            window.Created += (s, e) =>
            {
                windowIsActive = true;
            };
            window.Resumed += (s, e) =>
            {
                windowIsActive = true;
            };
            window.Deactivated += (s, e) =>
            {
                windowIsActive = false;
            };

            window.Stopped += (s, e) =>
            {
                windowIsActive = false;
            };
            mainPage = page;
        }

        public static void OnKeyPress(AOdiaKey key, AOdiaKeyPressType pressType)
        {
            if (!windowIsActive)
            {
                return;
            }
            if(pressType == AOdiaKeyPressType.Down)
            {
                if (key == AOdiaKey.LeftShift)
                {
                    modifierKey.ShiftPressed = true;
                    return;
                }
                if (key == AOdiaKey.LeftCtrl)
                {
                    modifierKey.CtrlPressed = true;
                    return;
                }
                if (key == AOdiaKey.LeftAlt)
                {
                    modifierKey.AltPressed = true;
                    return;
                }
            }
            if (pressType == AOdiaKeyPressType.Up)
            {
                if (key == AOdiaKey.LeftShift)
                {
                    modifierKey.ShiftPressed = false;
                    return;
                }
                if (key == AOdiaKey.LeftCtrl)
                {
                    modifierKey.CtrlPressed = false;
                    return;
                }
                if (key == AOdiaKey.LeftAlt)
                {
                    modifierKey.AltPressed = false;
                    return;
                }

            }


            if (pressType == AOdiaKeyPressType.Down)
            {

            }



        }





    }


    public interface KeyEventListener
    {
        public void OnKeyPress(AOdiaKey keyCode,ModifierKey modifierKey);
    }


    public enum AOdiaKey
    {
        None = 0,
        Cancel = 1,
        Back = 2,
        Tab = 3,
        LineFeed = 4,
        Clear = 5,
        Enter = 6,
        Return = 6,
        Pause = 7,
        Capital = 8,
        CapsLock = 8,
        HangulMode = 9,
        KanaMode = 9,
        JunjaMode = 10,
        FinalMode = 11,
        HanjaMode = 12,
        KanjiMode = 12,
        Escape = 13,
        ImeConvert = 14,
        ImeNonConvert = 15,
        ImeAccept = 16,
        ImeModeChange = 17,
        Space = 18,
        PageUp = 19,
        Prior = 19,
        Next = 20,
        PageDown = 20,
        End = 21,
        Home = 22,
        Left = 23,
        Up = 24,
        Right = 25,
        Down = 26,
        Select = 27,
        Print = 28,
        Execute = 29,
        PrintScreen = 30,
        Snapshot = 30,
        Insert = 31,
        Delete = 32,
        Help = 33,
        D0 = 34,
        D1 = 35,
        D2 = 36,
        D3 = 37,
        D4 = 38,
        D5 = 39,
        D6 = 40,
        D7 = 41,
        D8 = 42,
        D9 = 43,
        A = 44,
        B = 45,
        C = 46,
        D = 47,
        E = 48,
        F = 49,
        G = 50,
        H = 51,
        I = 52,
        J = 53,
        K = 54,
        L = 55,
        M = 56,
        N = 57,
        O = 58,
        P = 59,
        Q = 60,
        R = 61,
        S = 62,
        T = 63,
        U = 64,
        V = 65,
        W = 66,
        X = 67,
        Y = 68,
        Z = 69,
        LWin = 70,
        RWin = 71,
        Apps = 72,
        Sleep = 73,
        NumPad0 = 74,
        NumPad1 = 75,
        NumPad2 = 76,
        NumPad3 = 77,
        NumPad4 = 78,
        NumPad5 = 79,
        NumPad6 = 80,
        NumPad7 = 81,
        NumPad8 = 82,
        NumPad9 = 83,
        Multiply = 84,
        Add = 85,
        Separator = 86,
        Subtract = 87,
        Decimal = 88,
        Divide = 89,
        F1 = 90,
        F2 = 91,
        F3 = 92,
        F4 = 93,
        F5 = 94,
        F6 = 95,
        F7 = 96,
        F8 = 97,
        F9 = 98,
        F10 = 99,
        F11 = 100,
        F12 = 101,
        F13 = 102,
        F14 = 103,
        F15 = 104,
        F16 = 105,
        F17 = 106,
        F18 = 107,
        F19 = 108,
        F20 = 109,
        F21 = 110,
        F22 = 111,
        F23 = 112,
        F24 = 113,
        NumLock = 114,
        Scroll = 115,
        LeftShift = 116,
        RightShift = 117,
        LeftCtrl = 118,
        RightCtrl = 119,
        LeftAlt = 120,
        RightAlt = 121,
        BrowserBack = 122,
        BrowserForward = 123,
        BrowserRefresh = 124,
        BrowserStop = 125,
        BrowserSearch = 126,
        BrowserFavorites = 127,
        BrowserHome = 128,
        VolumeMute = 129,
        VolumeDown = 130,
        VolumeUp = 131,
        MediaNextTrack = 132,
        MediaPreviousTrack = 133,
        MediaStop = 134,
        MediaPlayPause = 135,
        LaunchMail = 136,
        SelectMedia = 137,
        LaunchApplication1 = 138,
        LaunchApplication2 = 139,
        Oem1 = 140,
        OemSemicolon = 140,
        OemPlus = 141,
        OemComma = 142,
        OemMinus = 143,
        OemPeriod = 144,
        Oem2 = 145,
        OemQuestion = 145,
        Oem3 = 146,
        OemTilde = 146,
        AbntC1 = 147,
        AbntC2 = 148,
        Oem4 = 149,
        OemOpenBrackets = 149,
        Oem5 = 150,
        OemPipe = 150,
        Oem6 = 151,
        OemCloseBrackets = 151,
        Oem7 = 152,
        OemQuotes = 152,
        Oem8 = 153,
        Oem102 = 154,
        OemBackslash = 154,
        ImeProcessed = 155,
        System = 156,
        DbeAlphanumeric = 157,
        OemAttn = 157,
        DbeKatakana = 158,
        OemFinish = 158,
        DbeHiragana = 159,
        OemCopy = 159,
        DbeSbcsChar = 160,
        OemAuto = 160,
        DbeDbcsChar = 161,
        OemEnlw = 161,
        DbeRoman = 162,
        OemBackTab = 162,
        Attn = 163,
        DbeNoRoman = 163,
        CrSeaal = 164,
        DbeEnterWordRegisterMode = 164,
        DbeEnterImeConfigureMode = 165,
        ExSel = 165,
        DbeFlushString = 166,
        EraseEof = 166,
        DbeCodeInput = 167,
        Play = 167,
        DbeNoCodeInput = 168,
        Zoom = 168,
        DbeDetermineString = 169,
        NoName = 169,
        DbeEnterDialogConversionMode = 170,
        Pa1 = 170,
        OemClear = 171,
        DeadCharProcessed = 172
    }
    public enum AOdiaKeyPressType
    {
        Down = 0,
        Up = 1,

    }

    public class ModifierKey
    {
        public bool ShiftPressed = false;
        public bool CtrlPressed = false;
        public bool AltPressed = false;
    }
}
