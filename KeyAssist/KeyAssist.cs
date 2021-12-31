using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyAssist
{
	partial class KeyAssist
	{
		string buffer = "";

		List<Entry> entries = new List<Entry>();

		public void AddPattern(string pattern, Action<string> handler)
		{
			entries.Add(new Entry()
			{
				pattern = pattern,
				regex = null,
				handler = handler
			});
		}

		public void AddPattern(Regex pattern, Action<string> handler)
		{
			entries.Add(new Entry()
			{
				pattern = null,
				regex = pattern,
				handler = handler
			});
		}

		public void Input(string text)
		{
			List<INPUT> inputs = new List<INPUT>();

			for(int i = 0; i < text.Length; i++)
			{
				inputs.Add(GenerateInput(0, text[i], 0b0100));
				inputs.Add(GenerateInput(0, text[i], 0b0110));
			}

			Win32.SendInput((uint)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(INPUT)));
		}

		public void Map(string pattern, string text)
		{
			AddPattern(pattern, (string m) =>
			{
				Input(text);
			});
		}

		INPUT GenerateInput(ushort vk, ushort scan, uint flags)
		{
			return new INPUT
			{
				Type = 1,
				Data = new MOUSEKEYBDHARDWAREINPUT()
				{
					Keyboard = new KEYBDINPUT()
					{
						Vk = vk,
						Scan = scan,
						Flags = flags,
						Time = 0,
						ExtraInfo = IntPtr.Zero,
					}
				}
			};
		}

		public void Down(char ch)
		{
			INPUT[] inputs = new INPUT[]
			{
				GenerateInput(0, ch, 0b0100)
			};

			Win32.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
		}

		public void Down(KeyCode key)
		{
			INPUT[] inputs = new INPUT[]
			{
				GenerateInput((ushort)key, 0, 0b0000)
			};

			Win32.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
		}

		public void Up(char ch)
		{
			INPUT[] inputs = new INPUT[]
			{
				GenerateInput(0, ch, 0b0110)
			};

			Win32.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
		}

		public void Up(KeyCode key)
		{
			INPUT[] inputs = new INPUT[]
			{
				GenerateInput((ushort)key, 0, 0b0010)
			};

			Win32.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
		}

		public void Press(char ch)
		{
			INPUT[] inputs = new INPUT[]
			{
				GenerateInput(0, ch, 0b0100),
				GenerateInput(0, ch, 0b0110)
			};

			Win32.SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
		}

		public void Press(KeyCode key)
		{
			INPUT[] inputs = new INPUT[]
			{
				GenerateInput((ushort)key, 0, 0b0000),
				GenerateInput((ushort)key, 0, 0b0010)
			};

			Win32.SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
		}

		public void Backspace(int count)
		{
			INPUT[] inputs = new INPUT[count * 2];

			for(int i = 0; i < inputs.Length; i += 2)
			{
				inputs[i] = GenerateInput(0x08, 0, 0b0000);
				inputs[i + 1] = GenerateInput(0x08, 0, 0b0010);
			}

			Win32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
		}

		public void Execute(string path, string arguments)
		{
			Process process = new Process();
			process.StartInfo.FileName = path;
			process.StartInfo.Arguments = arguments;
			process.Start();
		}

		public void Run()
		{
			Install();

			while(true)
			{
				GatherInput();
				MatchPatterns();

				Thread.Sleep(25);
			}
		}

		void Install()
		{
			string executingPath = Assembly.GetExecutingAssembly().Location;
			string localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");
			string installationPath = Path.Combine(localAppData, "KeyAssist");

			if(Path.GetDirectoryName(executingPath) == installationPath)
			{
				return;
			}

			Directory.CreateDirectory(installationPath);

			File.Copy(executingPath, Path.Combine(installationPath, "KeyAssist.exe"), true);
			
			Environment.Exit(0);
		}

		void GatherInput()
		{
			for(uint i = 1; i < 256; i++)
			{
				if(Win32.GetAsyncKeyState(i) == -32767)
				{
					if(i == 0x08)
					{
						if(buffer.Length > 0)
						{
							buffer = buffer.Substring(0, buffer.Length - 1);
						}
					}
					else if(i == 0x0D)
					{
						buffer = "";
					}
					else
					{
						buffer += GetCharsFromKey(i);

						while(buffer.Length > 256)
						{
							buffer = buffer.Substring(1);
						}
					}
				}
			}
		}

		void MatchPatterns()
		{
			foreach(Entry entry in entries)
			{
				if(entry.pattern != null)
				{
					if(buffer.EndsWith(entry.pattern))
					{
						Backspace(entry.pattern.Length);

						buffer = buffer.Substring(0, buffer.Length - entry.pattern.Length);
						entry.handler(entry.pattern);
					}
				}
				else if(entry.regex != null)
				{
					Match match = entry.regex.Match(buffer);

					if(match.Success)
					{
						Backspace(match.Value.Length);

						buffer = buffer.Substring(0, buffer.Length - match.Value.Length);
						entry.handler(match.Value);
					}
				}
			}
		}

		string GetCharsFromKey(uint keys)
		{
			var buffer = new StringBuilder(256);
			var keyboardState = new byte[256];

			if(Win32.GetAsyncKeyState(0x10) != 0)
			{
				keyboardState[0x10] = 0xFF;
			}

			if(Win32.GetAsyncKeyState(0x11) != 0)
			{
				keyboardState[0x11] = 0xFF;
				keyboardState[0x12] = 0xFF;
			}

			if(Win32.GetKeyState(0x14) != 0)
			{
				keyboardState[0x14] = 0xFF;
			}

			Win32.ToUnicode(keys, 0, keyboardState, buffer, 256, 0);
			return buffer.ToString();
		}

	}
}
