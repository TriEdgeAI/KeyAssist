using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KeyAssist
{
	partial class KeyAssist
	{
		class Entry
		{
			public string pattern;
			public Regex regex;
			public Action<string> handler;
		}
	}
}
