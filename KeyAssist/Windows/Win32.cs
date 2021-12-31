using System;
using System.Runtime.InteropServices;
using System.Text;

static class Win32
{
	[DllImport("user32.dll")]
	public static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState, 
		[Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);

	[DllImport("user32.dll")]
	public static extern bool GetKeyboardState(byte[] lpKeyState);

	[DllImport("user32.dll")]
	public static extern uint MapVirtualKey(uint uCode, uint uMapType);

	[DllImport("user32.dll")]
	public static extern IntPtr GetKeyboardLayout(uint idThread);

	[DllImport("user32.dll")]
	public static extern short GetKeyState(uint vKey);

	[DllImport("user32.dll")]
	public static extern short GetAsyncKeyState(uint vKey);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
}
