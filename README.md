# Unity_WindowPositionManager

This script sets window position and the topmost option.

![](https://github.com/XJINE/Unity_WindowPositionManager/blob/main/screenshot0.gif)

## Importing

You can use Package Manager or import it directly.

```
https://github.com/XJINE/Unity_WindowPositionManager.git?path=Assets/Packages/WindowPositionManager
```

### Dependencies

This project use following resources.

- https://github.com/XJINE/Unity_SingletonMonoBehaviour
- https://github.com/XJINE/Unity_CommandLineArgs

## How to Use

Add scripts into your scene and set the parameters from Inspector.

| Argument          | Description                                            |
|:------------------|:-------------------------------------------------------|
| Screen Position X | Window top-left position X in px.                      |
| Screen Position Y | Window top-left position Y in px.                      |
| Topmost           | Window will always keeps top or not.                   |
| Interval Time     | Set position in each interval time. If set 0, do once. |

### Arguments

You can also use some command line arguments.
If set, these parameters override existing parameters.

| Argument           | Description                       |
|:-------------------|:----------------------------------|
| -screen-position-x | Set "Screen Position X".          |
| -screen-position-y | Set "Screen Position Y".          |
| -topmost           | Set "Topmost" and "Interval Time".|

```
Sample.exe -popupwindow -screen-width 640 -screen-height 480 -screen-position-x 200 -screen-position-y 200 -topmost 0
```

## Limitation

- Windows only.
- Window must be popupwindow or windowed mode.
