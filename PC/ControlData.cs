using System;
using System.Collections.Generic;
using System.Text;

namespace FeederDemoCS
{
	class ControlData
	{
		public enum DataType { Button, Axis };
		public DataType Type;
		public int Key;
		public int Value;
		public ControlData(string s)
		{
			if (s[0] == 'A')
			{
				Type = DataType.Axis;
				Key = int.Parse(s.Substring(1, 1));
				Value = int.Parse(s.Substring(3));
			}
			if (s[0] == 'B')
			{
				Type = DataType.Button;
				Key = int.Parse(s.Substring(1, 2));
				Value = int.Parse(s.Substring(4, 1));
			}
			Console.WriteLine(this.ToString());
		}

		override public string ToString()
		{
			string result = "";
			result += "    >--Type = ";
			if (Type == DataType.Axis) result += "Axix";
			if (Type == DataType.Button) result += "Button";
			result += string.Format("; Key = {0}; Value = {1}", Key.ToString(), Value.ToString());
			return result;
		}
	}
}
