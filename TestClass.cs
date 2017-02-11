using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPropertiesNet35 {
	public class TestClass {

		private string _key1;
		private int _key2;
		private uint _key3;

		public string key1 { get { return _key1; } set { _key1 = value; } }
		public int key2 { get { return _key2; } set { _key2 = value; } }
		public uint key3 { get { return _key3; } set { _key3 = value; } }
	}
}
