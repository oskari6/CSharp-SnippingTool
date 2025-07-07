using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;

namespace SnippingTool
{
public partial class MainWindow : Window
{
    private const int HOTKEY_ID = 9000;
    private const uint MOD_ALT = 0x0001;
    private const uint MOD_SHIFT = 0x0004;
    private const uint VK_S = 0x53;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => RegisterHotkey();
        Closing += (_, _) => UnregisterHotkey();
    }

    private void RegisterHotkey()
    {
        var helper = new WindowInteropHelper(this);
        var source = HwndSource.FromHwnd(helper.Handle);
        source.AddHook(HwndHook);
        
    RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_ALT | MOD_SHIFT, VK_S);

    }

    private void UnregisterHotkey()
    {
        var helper = new WindowInteropHelper(this);
        UnregisterHotKey(helper.Handle, HOTKEY_ID);
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_HOTKEY = 0x0312;

        if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
        {
            StartSnipping();
            handled = true;
        }

        return IntPtr.Zero;
    }

    private void StartSnipping()
    {
        var overlay = new SnippingOverlay();
        overlay.ShowDialog();
    }
}
}