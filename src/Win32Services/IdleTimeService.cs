using System.Runtime.InteropServices;



public class IdleTimeService
{
    private struct LASTINPUTINFO
    {
        public uint cbSize;

        public uint dwTime;
    }

    [DllImport("User32.dll")]
    private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

    [DllImport("Kernel32.dll")]
    private static extern uint GetLastError();

    public long GetIdleTime()
    {
        LASTINPUTINFO lastInPut = new LASTINPUTINFO();
        lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
        GetLastInputInfo(ref lastInPut);

        return Environment.TickCount - lastInPut.dwTime;
    }

    public long GetLastInputTime()
    {
        LASTINPUTINFO lastInPut = new LASTINPUTINFO();
        lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
        if (!GetLastInputInfo(ref lastInPut))
        {
            throw new Exception(GetLastError().ToString());
        }
        return lastInPut.dwTime;
    }
}