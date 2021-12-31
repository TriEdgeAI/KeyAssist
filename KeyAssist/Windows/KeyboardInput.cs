using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct KEYBDINPUT
{
	public ushort Vk;
	public ushort Scan;
	public uint Flags;
	public uint Time;
	public IntPtr ExtraInfo;
}