using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using ProcessId = System.UInt32;

public class ActiveWindowInfo
{
    public string? Title { get; set; }
    public string ProcessName { get; set; } = "";
    public ProcessId ProcessId { get; set; }
    public ProcessId ThreadId { get; set; }

    override public string ToString()
    {
        return $"[Title: {Title}; ProcessName: {ProcessName}; ProcessId: {ProcessId}; ThreadId: {ThreadId}]";
    }
}

public class ActiveWindowService
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hwnd, StringBuilder ss, int count);
    [DllImport("user32.dll")]
    static extern ProcessId GetWindowThreadProcessId(IntPtr hwdn, out ProcessId processId);

    public ActiveWindowInfo GetActiveWindowInfo()
    {
        const int nChar = 256;
        StringBuilder ss = new StringBuilder(nChar);
        IntPtr handle = IntPtr.Zero;
        handle = GetForegroundWindow();
        ProcessId processID = 0;
        ProcessId threadID = GetWindowThreadProcessId(handle, out processID); // Get PID from window handle
        Process foregroundProcess = Process.GetProcessById(Convert.ToInt32(processID)); // Get it as a C# obj.
        string? activeWindowName = null;
        if (GetWindowText(handle, ss, nChar) > 0)
        {
            activeWindowName = ss.ToString();
        }

        return new ActiveWindowInfo
        {
            Title = activeWindowName,
            ProcessName = foregroundProcess.ProcessName,
            ProcessId = processID,
            ThreadId = threadID
        };
    }
}