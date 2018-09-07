# Unity_WindowPositionManager

Add some command line arguments to set window position.

## Import to Your Project

You can import this asset from UnityPackage.

- [WindowPositionManager.unitypackage](https://github.com/XJINE/Unity_WindowPositionManager/blob/master/WindowPositionManager.unitypackage)

### Dependencies

You have to import following assets to use this asset.

- [Unity_SingletonMonoBehaviour](https://github.com/XJINE/Unity_SingletonMonoBehaviour)
- [Unity_CommandLineArgs](https://github.com/XJINE/Unity_CommandLineArgs)

## How to Use

### Arguments

| Argument             | Description                           |
|:---------------------|:--------------------------------------|
| *-screen-position-x* | Set window top-left position X in px. |
| *-screen-position-y* | Set window top-left position Y in px. |
| *-topmost*           | Set window topmost.                   |

### Sample

```
WindowPositionManager.exe -popupwindow -screen-width 640 -screen-height 480 -screen-position-x 200 -screen-position-y 200 -topmost
```

## Limitation

- Windows only.
- Window must be popupwindow or windowed mode.
- Window must be active while loading scene(script).
    - If you activate the another window while scene load, these settings works wrong.

## Screenshot

![](https://github.com/XJINE/Unity3D_WindowPositionManager/blob/master/screenshot0.gif)
