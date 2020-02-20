using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowPositionManager : SingletonMonoBehaviour<WindowPositionManager>
{
    // NOTE:
    // This instance will remove when the topmost is done as "Once ver. or "Delay ver"

    #region DllImport

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    protected static extern bool SetWindowPos(IntPtr hWnd,
                                              int hWndInsertAfter,
                                              int x,
                                              int y,
                                              int cx,
                                              int cy,
                                              int uFlags);

    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)]
    protected static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    protected static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    protected const int SWP_NOSIZE = 0x0001;

    #endregion DllImport

    #region Field

    public static readonly string CommandScreenPositionX = "-screen-position-x";
    public static readonly string CommandScreenPositionY = "-screen-position-y";
    public static readonly string CommandTopmost         = "-topmost";

    public    int    screenPositionX;
    public    int    screenPositionY;
    public    bool   topmost;
    public    float  topmostTime;
    protected float  topmostTimePrev;
    protected IntPtr windowHandle;

    #endregion Field

    #region Property

    protected string ProcessName
    {
        // NOTE:
        // for Debug.

        get
        {
            GetWindowThreadProcessId(this.windowHandle, out int processId);
            return System.Diagnostics.Process.GetProcessById(processId).ProcessName;
        }
    }

    #endregion Property

    #region Method

    protected override void Awake()
    {
        #pragma warning disable 0162

        base.Awake();

        #if UNITY_EDITOR

        Debug.Log("WindowPositionMangaer do nothing in editor.");

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
            this.topmostTime = time;
        }

        if (this.topmost && this.topmostTime >= 0)
        {
            SetWindowPosition(this.windowHandle, this.screenPositionX, this.screenPositionY, this.topmost);

            // NOTE:
            // Once ver.

            if (this.topmostTime == 0)
            {
                Destroy(this);
            }
        }

        #pragma warning restore 0162
    }

    protected virtual void FixedUpdate()
    {
        // NOTE:
        // It is enough to do SetWindowPosition once in each frame.

        if (this.topmost)
        {
            // NOTE:
            // Interval ver.

            if (this.topmostTime > 0 && Time.timeSinceLevelLoad - this.topmostTimePrev >= this.topmostTime)
            {
                this.topmostTimePrev = (int)Time.timeSinceLevelLoad;
                return;
            }

            // NOTE:
            // Delay ver.
            // This setting only enabled from Inspector
            // because of the CommandLineArgs cant get a minus"-" value.

            if (this.topmostTime < 0 && Time.timeSinceLevelLoad + this.topmostTime >= 0)
            {
                SetWindowPosition(this.windowHandle, this.screenPositionX, this.screenPositionY, this.topmost);
                Destroy(this);
            }
        }
    }

    protected static void SetWindowPosition(IntPtr windowHandle, int x, int y, bool topmost)
    {
        // NOTE:
        // Window will be set topmost when the value of "topmost" is -1.

        // CAUTION:
        // Need to set SWP_NOSIZE option.
        // Set Screen.width/height makes trouble
        // which resizes the window smaller than the previous size.

        SetWindowPos(windowHandle,
                     topmost ? -1 : 0,
                     x,
                     y,
                     0,
                     0,
                     SWP_NOSIZE);

        if (topmost)
        {
            SetForegroundWindow(windowHandle);
        }
    }

    #endregion Method
}