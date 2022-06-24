using System;
using System.Runtime.InteropServices;
using UnityEngine;

public sealed class WindowPositionManager : SingletonMonoBehaviour<WindowPositionManager>
{
    #region DllImport

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hWnd,
                                            int    hWndInsertAfter,
                                            int    x,
                                            int    y,
                                            int    cx,
                                            int    cy,
                                            int    uFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    #endregion DllImport

    #region Field

    public static readonly string CommandScreenPositionX = "-screen-position-x";
    public static readonly string CommandScreenPositionY = "-screen-position-y";
    public static readonly string CommandTopmost         = "-topmost";

    public  int    screenPositionX;
    public  int    screenPositionY;
    public  bool   topmost;
    public  float  intervalTime;
    private float  _intervalTimePrev;
    private IntPtr _windowHandle;

    #endregion Field

    #region Method

    protected override void Awake()
    {
        #pragma warning disable 0162

        base.Awake();

        #if UNITY_EDITOR

        Debug.Log("WindowPositionManager do nothing in editor, and the instance will be Destroyed.");

        return;

        #endif

        _windowHandle = FindWindow(null, Application.productName);

        if (CommandLineArgs.GetValueAsInt(CommandScreenPositionX, out var x))
        {
            screenPositionX = x;
        }

        if (CommandLineArgs.GetValueAsInt(CommandScreenPositionY, out var y))
        {
            screenPositionY = y;
        }

        topmost = topmost || CommandLineArgs.HasParameter(CommandTopmost);

        if (CommandLineArgs.GetValueAsFloat(CommandTopmost, out var time))
        {
            intervalTime = time;
        }

        if (topmost && intervalTime >= 0)
        {
            SetWindowPosition(_windowHandle, screenPositionX, screenPositionY, topmost);

            // NOTE:
            // Once ver.

            if (intervalTime == 0)
            {
                Destroy(this);
            }
        }

        #pragma warning restore 0162
    }

    private void FixedUpdate()
    {
        if (!topmost)
        {
            return;
        }

        if (intervalTime > 0 && Time.timeSinceLevelLoad - _intervalTimePrev >= intervalTime)
        {
            _intervalTimePrev = Time.timeSinceLevelLoad;

            SetWindowPosition(_windowHandle, screenPositionX, screenPositionY, topmost);
        }
    }

    private static void SetWindowPosition(IntPtr windowHandle, int x, int y, bool topmost)
    {
        // NOTE:
        // Window will be set topmost when the value of "topmost" is -1.

        const int HWND_TOPMOST = -1;
        const int HWND_TOP     =  0;

        // CAUTION:
        // Need to set SWP_NOSIZE option.
        // Set Screen.width/height makes trouble
        // which resizes the window smaller than the previous size.

        const int SWP_NOSIZE = 0x0001;

        SetWindowPos(windowHandle,
                     topmost ? HWND_TOPMOST : HWND_TOP,
                     x,
                     y,
                     0,
                     0,
                     SWP_NOSIZE);
    }

    #endregion Method
}