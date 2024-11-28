using System.Runtime.InteropServices;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using System.Reflection;
using static UnityEngine.PlayerLoop.PostLateUpdate;

public class GameManagerOption : MonoBehaviour
{
    public TMP_Dropdown windowModeDropdown;  // Dropdown for window modes
    public TMP_Dropdown resolutionDropdown;
    private GameObject stopChangePanel; // 缓存 StopChange 对象
    private bool Window_true = false;

    // Windows API函数
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private const int SWP_NOZORDER = 0x0004;
    private const int SWP_NOACTIVATE = 0x0010;

    private IntPtr windowHandle;

    private void Start()
    {
        windowHandle = GetActiveWindow();

        // 设置为全屏并使用最大分辨率
        SetInitialFullScreenMode();

        // 注册窗口模式的监听器
        windowModeDropdown.onValueChanged.AddListener(SetWindowMode);

        // 注册分辨率下拉菜单的监听器
        resolutionDropdown.onValueChanged.AddListener(UpdateResolution);

    }

    private void UpdateResolution(int resolutionIndex)
    {
        CouldSetChange(resolutionIndex); // 在选择分辨率时调用
    }

    // 游戏启动时设置为全屏模式
    private void SetInitialFullScreenMode()
    {
        Resolution maxResolution = Screen.resolutions[Screen.resolutions.Length - 1];
        Screen.SetResolution(maxResolution.width, maxResolution.height, FullScreenMode.ExclusiveFullScreen);
        windowModeDropdown.value = 0; // 初始值设置为全屏
        Window_true = true;
    }

    // 窗口模式设置
    public void SetWindowMode(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0:  // 全屏模式
                Resolution maxResolution = Screen.resolutions[Screen.resolutions.Length - 1];
                Screen.SetResolution(maxResolution.width, maxResolution.height, FullScreenMode.ExclusiveFullScreen);
                Window_true = true;
                CouldSetChange(resolutionDropdown.value);
                break;

            case 1:  // 窗口模式
                // 启用窗口模式
                Screen.fullScreenMode = FullScreenMode.Windowed;
                ResizeWindow(800, 600); // 设置初始窗口大小
                Window_true = false;
                CouldSetChange(resolutionDropdown.value);
                break;

            case 2:  // 无边框窗口模式
                // 启用无边框窗口模式
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Window_true = true;
                CouldSetChange(resolutionDropdown.value);
                break;
        }

        if (stopChangePanel != null)
        {
            stopChangePanel.SetActive(Window_true);
        }
    }

    public void CouldSetChange(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0:
                ResizeWindow(1920, 1080);
                break;

            case 1:
                ResizeWindow(1600, 900);
                break;

            case 2:
                ResizeWindow(1280, 720);
                break;

            case 3:
                ResizeWindow(1024, 576);
                break;

            case 4:
                ResizeWindow(640, 360);
                break;
        }
    }

    // 调整窗口大小的方法
    public void ResizeWindow(int width, int height)
    {
        SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
    }

    [Header("音频混响器")]
    public AudioMixer mixer;

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("Master", value);
    }
}
