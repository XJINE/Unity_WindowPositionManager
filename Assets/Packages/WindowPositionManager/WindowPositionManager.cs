using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowPositionManager : SingletonMonoBehaviour<WindowPositionManager>
{
    #region DllImport

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    protected static extern bool SetWindowPos(IntPtr hWnd,
                                              int    hWndInsertAfter,
                                              int    x,
                                              int    y,
                                              int    cx,
                                              int    cy,
                                              int    uFlags);

    [DllImport("user32.dll")]
    protected static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    #endregion DllImport

    #region Field

    public static readonly string CommandScreenPositionX = "-screen-position-x";
    public static readonly string CommandScreenPositionY = "-screen-position-y";
    public static readonly string CommandTopmost         = "-topmost";

    public    int    screenPositionX;
    public    int    screenPositionY;
    public    bool   topmost;
    public    float  intervalTime;
    protected float  intervalTimePrev;
    protected IntPtr windowHandle;

    #endregion Field

    #region Method

    protected override void Awake()
    {
        #pragma warning disable 0162

        base.Awake();

        #if UNITY_EDITOR

        Debug.Log("WindowPositionMangaer do nothing in editor, and the instance will be Destroyed.");

        return;

        #endif

        windowHandle = FindWindow(null, Application.productName);

        if (CommandLineArgs.GetValueAsInt(CommandScreenPositionX, out int x))
        {
            this.screenPositionX = x;
        }

        if (CommandLineArgs.GetValueAsInt(CommandScreenPositionY, out int y))
        {
            this.screenPositionY = y;
        }

        this.topmost = this.topmost || CommandLineArgs.HasParameter(CommandTopmost);

        if (CommandLineArgs.GetValueAsFloat(CommandTopmost, out float time))
        {
            this.intervalTime = time;
        }

        if (this.topmost && this.intervalTime >= 0)
        {
            SetWindowPosition(this.windowHandle, this.screenPositionX, this.screenPositionY, this.topmost);

            // NOTE:
            // Once ver.

            if (this.intervalTime == 0)
            {
                Destroy(this);
            }
        }

        #pragma warning restore 0162
    }

    protected virtual void FixedUpdate()
    {
        if (!this.topmost)
        {
            return;
        }

        if (this.intervalTime > 0 && Time.timeSinceLevelLoad - this.intervalTimePrev >= this.intervalTime)
        {
            this.intervalTimePrev = Time.timeSinceLevelLoad;

            SetWindowPosition(this.windowHandle, this.screenPositionX, this.screenPositionY, this.topmost);

            return;
        }
    }

    protected static void SetWindowPosition(IntPtr windowHandle, int x, int y, bool topmost)
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