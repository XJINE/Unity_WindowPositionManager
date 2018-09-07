using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowPositionManager : SingletonMonoBehaviour<WindowPositionManager>
{
    #region DllImport

    // # NOTE:
    // https://msdn.microsoft.com/ja-jp/library/cc411206.aspx

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hWnd,
                                            int hWndInsertAfter,
                                            int x,
                                            int y,
                                            int cx,
                                            int cy,
                                            int uFlags);

    #endregion DllImport

    #region Field

    public static readonly string CommandScreenPositionX = "-screen-position-x";
    public static readonly string CommandScreenPositionY = "-screen-position-y";
    public static readonly string CommandTopmost         = "-topmost";

    #endregion Field

    protected override void Awake()
    {
        #pragma warning disable 0162

        base.Awake();

        #if UNITY_EDITOR

        return;

        #endif

        int  screenPositionX = CommandLineArgs.GetValueAsInt (CommandScreenPositionX, out screenPositionX) ? screenPositionX : 0;
        int  screenPositionY = CommandLineArgs.GetValueAsInt (CommandScreenPositionY, out screenPositionY) ? screenPositionY : 0;
        bool topmost         = CommandLineArgs.HasParameter  (CommandTopmost);

        SetWindowPosition(screenPositionX, screenPositionY, topmost);

        #pragma warning restore 0162
    }

    protected virtual void SetWindowPosition(int x, int y, bool topmost)
    {
        // NOTE:
        // Window will be set topmost when the value of "topmost" is -1.

        SetWindowPos(GetActiveWindow(),
                     topmost ? -1 : 0,
                     x,
                     y,
                     Screen.width,
                     Screen.height,
                     0);
    }
}