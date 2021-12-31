using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct INPUT
{
	public uint Type;
	public MOUSEKEYBDHARDWAREINPUT Data;
}