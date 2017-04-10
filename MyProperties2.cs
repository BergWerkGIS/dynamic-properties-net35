using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPropertiesNet35 {
	public class MyProperties2 {

		public MyProperties2() {
			_keys = new List<string>();
			_valuesObject = new List<object>();
			_valuesInt = new List<int>();
			_valuesString = new List<string>();
		}


		private List<string> _keys;
		private List<object> _valuesObject;
		private List<int> _valuesInt;
		private List<string> _valuesString;
		private Dictionary<Type, Dictionary<string, object>> _typesDic = new Dictionary<Type, Dictionary<string, object>>();


		public void Add<T>(string key, T value) {

			_keys.Add(key);
			var x = new { key = key, value = value };

			_valuesObject.Add(x);

			var y = new KeyValuePair<string, T>(key, value);

			_valuesObject.Add(y);

			List<KeyValuePair<string, T>> myList = new List<KeyValuePair<string, T>>();
			myList.Add(new KeyValuePair<string, T>(key, value));

			Type t = typeof(T);
			if (!_typesDic.Keys.Contains(t)) {
				_typesDic[t] = new Dictionary<string, object>();
			}
			_typesDic[t].Add(key, value);
		}

		public T Get<T>(string key) {
			Type t = typeof(T);
			if (!_typesDic.Keys.Contains(t)) { return default(T); }
			if (!_typesDic[typeof(T)].Keys.Contains(key)) { return default(T); }

			//T retVal = _typesDic[typeof(T)][key] as T;

			return (T)_typesDic[typeof(T)][key];
		}



	}
}
