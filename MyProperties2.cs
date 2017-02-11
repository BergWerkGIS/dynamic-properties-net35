using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPropertiesNet35 {
	public class MyProperties2 {

		public MyProperties2() {
			_keys = new List<string>();
		}


		private List<string> _keys;
		//private List<dynamic> _values;

		public void Add<T>(string key, T value) {
			_keys.Add(key);
			var x = new { key = value };
		}
	}
}
