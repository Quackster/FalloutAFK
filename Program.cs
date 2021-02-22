using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace FalloutAFK
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private static bool m_FoundFalloutWindow = false;
        private static InputSimulator m_InputSimulator = null;
        private static Random m_Random = null;
        private static long m_TimeUntilJumpTimer = 0;

        static void Main(string[] args)
        {
            while (!m_FoundFalloutWindow) FindWindow();

            m_InputSimulator = new InputSimulator();
            m_Random = new Random();
            ResetJumpTime();


            Console.WriteLine("Found Fallout window");

            while (true)
            {
                try
                {
                    m_InputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                    m_InputSimulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);

                    Thread.Sleep(1000);

                    if (m_TimeUntilJumpTimer == 0)
                    {
                        ResetJumpTime();

                        m_InputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        Thread.Sleep(500);

                        m_InputSimulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                        Thread.Sleep(500);

                        m_InputSimulator.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                        Thread.Sleep(500);


                    }

                    m_TimeUntilJumpTimer--;
                }
                catch
                {

                }
            }
        }

        private static void ResetJumpTime()
        {
            m_TimeUntilJumpTimer = m_Random.Next(5, 25) + 1;
        }

        private static void FindWindow()
        {
            Process[] processList = Process.GetProcessesByName("Fallout76");

            if (processList == null || processList.Length == 0)
            {
                Console.WriteLine("Finding Fallout 76 window");
                Thread.Sleep(2000);
                m_FoundFalloutWindow = false;
                return;
            }


            Process p = processList[0];
            p.WaitForInputIdle();
            IntPtr h = p.MainWindowHandle;
            SetForegroundWindow(h);

            m_FoundFalloutWindow = true;
        }
    }
}
