namespace lol
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;

    public class Utils
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        public static void CreateConsole()
        {
            if (AllocConsole())
            {
                IntPtr hHandle = CreateFile("CONOUT$", DesiredAccess.GenericAll, FileShare.ReadWrite, 0, FileMode.Open, FileAttributes.Normal, 0);
                if (hHandle == new IntPtr(-1))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                if (!SetStdHandle(StdHandle.Output, hHandle))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                StreamWriter newOut = new StreamWriter(Console.OpenStandardOutput());
                newOut.AutoFlush = true;
                Console.SetOut(newOut);
                StreamWriter newError = new StreamWriter(Console.OpenStandardError());
                newError.AutoFlush = true;
                Console.SetError(newError);
            }
        }

        [DllImport("kernel32.dll", SetLastError=true)]
        private static extern IntPtr CreateFile(string lpFileName, [MarshalAs(UnmanagedType.U4)] DesiredAccess dwDesiredAccess, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode, uint lpSecurityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition, [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes, uint hTemplateFile);
        [DllImport("kernel32.dll", SetLastError=true)]
        private static extern bool SetStdHandle(StdHandle nStdHandle, IntPtr hHandle);

        [Flags]
        private enum DesiredAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000
        }

        private enum StdHandle
        {
            Input = -10,
            Output = -11,
            Error = -12
        }
    }
}

