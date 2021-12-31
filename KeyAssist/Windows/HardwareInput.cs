using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct HARDWAREINPUT
{
	public uint Msg;
	public ushort ParamL;
	public ushort ParamH;
}
