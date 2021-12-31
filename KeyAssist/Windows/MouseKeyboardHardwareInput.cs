﻿using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
internal struct MOUSEKEYBDHARDWAREINPUT
{
	[FieldOffset(0)]
	public HARDWAREINPUT Hardware;
	[FieldOffset(0)]
	public KEYBDINPUT Keyboard;
	[FieldOffset(0)]
	public MOUSEINPUT Mouse;
}