using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyAssist
{
	class Program
	{
		static Random random = new Random();

		[STAThread]
		static void Main(string[] args)
		{
			Debugger.Launch();

			KeyAssist keyAssist = new KeyAssist();

			keyAssist.Map("@y;", "✔");
			keyAssist.Map("@x;", "✘");
			keyAssist.Map("@tm;", "™");
			keyAssist.Map("@c;", "©");
			keyAssist.Map("@r;", "®");
			keyAssist.Map("@sharp;", "♯");
			keyAssist.Map("@deg;", "°");
			keyAssist.Map("@ae;", "æ");
			keyAssist.Map("@AE;", "Æ");
			keyAssist.Map("@lenny;", "( ͡° ͜ʖ ͡°)");
			keyAssist.Map("@disapproval;", "ಠ_ಠ");

			keyAssist.AddPattern("@beep;", (string m) => 
			{
				Console.Beep();
			});
			
			keyAssist.AddPattern("@ip;", (string m) =>
			{
				string ip = new WebClient().DownloadString("https://api.ipify.org/");
				keyAssist.Input(ip);
			});

			keyAssist.AddPattern("@now;", (string m) =>
			{
				keyAssist.Input(DateTime.Now.ToString());
			});

			keyAssist.AddPattern("@roll;", (string m) => 
			{
				keyAssist.Input(random.Next(1, 7).ToString());
			});

			keyAssist.AddPattern("@jobs;", (string m) =>
			{
				keyAssist.Input("siriussoftware.bg");
				keyAssist.Press(KeyCode.TAB);
				keyAssist.Input("MaleMalePisnaMiOtP@roli123");
				keyAssist.Press(KeyCode.ENTER);
			});

			keyAssist.AddPattern("@coin;", (string m) =>
			{
				if(random.Next(2) == 0)
				{
					keyAssist.Input("Heads");
				}
				else
				{
					keyAssist.Input("Tails");
				}
			});

			keyAssist.AddPattern("@gay;", (string m) => 
			{
				SoundPlayer player = new SoundPlayer(@"Audio/HaGay.wav");
				player.Play();
			});

			keyAssist.AddPattern("@jerkoff;", (string m) =>
			{
				for(int i = 0; i < 5; i++)
				{
					for(int k = 5; k >= 0; k--)
					{
						keyAssist.Input(Penis(k));
						keyAssist.Press(KeyCode.ENTER);
						Thread.Sleep(100);
						keyAssist.Press(KeyCode.UP);
						keyAssist.Down(KeyCode.LCONTROL);
						keyAssist.Press(KeyCode.KEY_A);
						keyAssist.Up(KeyCode.LCONTROL);
					}

					for(int k = 0; k < 5; k++)
					{
						keyAssist.Input(Penis(k));
						keyAssist.Press(KeyCode.ENTER);
						Thread.Sleep(100);
						keyAssist.Press(KeyCode.UP);
						keyAssist.Down(KeyCode.LCONTROL);
						keyAssist.Press(KeyCode.KEY_A);
						keyAssist.Up(KeyCode.LCONTROL);
					}
				}
				
				for(int i = 0; i < 5; i++)
				{
					for(int k = -5; k < 5; k++)
					{
						keyAssist.Input(Penis(5) + " " + Cum(k));
						keyAssist.Press(KeyCode.ENTER);
						Thread.Sleep(100);
						keyAssist.Press(KeyCode.UP);
						keyAssist.Down(KeyCode.LCONTROL);
						keyAssist.Press(KeyCode.KEY_A);
						keyAssist.Up(KeyCode.LCONTROL);
					}
				}

				keyAssist.Input(Penis(5));
				keyAssist.Press(KeyCode.ENTER);
			});

			keyAssist.Run();
		}

		static string Penis(int handPosition)
		{
			string penis = "8";

			for(int k = 0; k < handPosition; k++)
			{
				penis += "=";
			}

			penis += "m";

			for(int k = handPosition; k < 5; k++)
			{
				penis += "=";
			}

			penis += ">";

			return penis;
		}

		static string Cum(int position)
		{
			int start = Math.Max(position, 0);
			int end = Math.Min(position + 4, 5);

			string cum = "";

			for(int i = 0; i < start; i++)
			{
				cum += " ";
			}

			for(int i = start; i < end; i++)
			{
				cum += "~";
			}

			return cum;
		}
	}
}
