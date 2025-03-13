using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace PantheonLootParser
{
	internal class Win32
	{
		[Flags]
		internal enum SendMessageTimeoutFlags : uint
		{
			SMTO_NORMAL = 0x0,
			SMTO_BLOCK = 0x1,
			SMTO_ABORTIFHUNG = 0x2,
			SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
			SMTO_ERRORONEXIT = 0x20
		}

		internal const uint BM_CLICK = 0xF5;

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void SendMessageDelegate(IntPtr hWnd, uint uMsg, UIntPtr dwData, IntPtr lResult);

		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr FindWindow(string strclass, string strname);

		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr FindWindowEx(IntPtr hwnd, IntPtr hwndAfter, string strclass, string strname);

		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool SetForegroundWindow(IntPtr hwnd);

		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool SendMessage(IntPtr hwnd, uint msg, UIntPtr wParam, IntPtr lParam);

		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool SendMessageTimeout(IntPtr hwnd, uint msg, UIntPtr wParam, IntPtr lParam,
			SendMessageTimeoutFlags fuflags, uint uTimeout, out UIntPtr result);

		[DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool SendMessageCallback(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam,
			SendMessageDelegate lpCallBack, UIntPtr dwData);

		// Импортировать функцию SetCursorPos из библиотеки user32.dll
		[DllImport("user32.dll")]
		private static extern bool SetCursorPos(int x, int y);

		// Импортировать функцию mouse_event из библиотеки user32.dll
		[DllImport("user32.dll")]
		private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, IntPtr dwExtraInfo);


		public static void SetCursorPos(Point point)
		{
			SetCursorPos(point.X, point.Y);
		}

		// Константы для определения действий мыши
		private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
		private const uint MOUSEEVENTF_LEFTUP = 0x0004;

		public static void MouseClick(Point point)
		{
			SetCursorPos(point.X, point.Y);
			Thread.Sleep(100);
			mouse_event(MOUSEEVENTF_LEFTDOWN, point.X, point.Y, 0, IntPtr.Zero);
			Thread.Sleep(100);
			mouse_event(MOUSEEVENTF_LEFTUP, point.X, point.Y, 0, IntPtr.Zero);
		}

		// Импортировать функцию PostMessage из библиотеки user32.dll
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Константы для определения сообщений клавиатуры
		private const int WM_KEYDOWN = 0x0100;
		private const int WM_KEYUP = 0x0101;

		public static void KeyDown(IntPtr hWnd, Keys key)
		{
			PostMessage(hWnd, WM_KEYDOWN, (IntPtr)key, IntPtr.Zero);
		}

		public static void KeyUp(IntPtr hWnd, Keys key)
		{
			PostMessage(hWnd, WM_KEYUP, (IntPtr)key, IntPtr.Zero);
		}

		public static void KeyPress(IntPtr hWnd, Keys key)
		{
			KeyDown(hWnd, key);
			KeyUp(hWnd, key);
		}
	}
}
