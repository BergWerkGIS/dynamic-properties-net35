using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicPropertiesNet35 {
	class Program {
		static void Main(string[] args) {


			//TestClass tc = new TestClass();
			//PropertyInfo[] propInfosTc = typeof(TestClass).GetProperties();
			//foreach (PropertyInfo pi in propInfosTc) {
			//	Console.WriteLine("TestClass property [{0}] created", pi.ToString());
			//}


			MyProperties2 p2 = new MyProperties2();
			p2.Add<string>("key1", "this is key1");
			p2.Add<int>("key2", -123);
			p2.Add<uint>("key3", 1);
			p2.Add<string>("key4", "key4 it is");

			Console.WriteLine(p2.Get<string>("key1"));
			Console.WriteLine(p2.Get<int>("key2"));
			Console.WriteLine(p2.Get<uint>("key3"));
			Console.WriteLine(p2.Get<string>("key4"));

			Console.WriteLine("key not valid: " + p2.Get<string>("key does not exists"));
			Console.WriteLine("type not valid: " + p2.Get<uint>("key1"));


			Console.WriteLine("");
			Console.WriteLine("");



			PropertiesGenerator propGen = new PropertiesGenerator();
			propGen.AddProperty<string>("key1");
			propGen.AddProperty<int>("key2");
			propGen.AddProperty<uint>("key3");

			Type dynPropsType = propGen.CreateDynamicPropertyTypeInstance();
			PropertyInfo[] propInfos = dynPropsType.GetProperties();
			foreach (PropertyInfo pi in propInfos) {
				Console.WriteLine("DynProps property [{0}] created", pi.ToString());
			}

			object dynProps = Activator.CreateInstance(dynPropsType);

			//set values
			foreach (PropertyInfo pi in propInfos) {

				object val = null;
				if (pi.Name == "key1") { val = "this is a string"; }
				if (pi.Name == "key2") { val = -123; }
				if (pi.Name == "key3") { val = 1U; }

				if (null == val) { continue; }
				try {
					dynPropsType.InvokeMember(
						pi.Name
						, BindingFlags.SetProperty
						, null
						, dynProps
						, new object[] { val }
					);
				}
				catch (Exception ex) {
					Debug.WriteLine(ex);
				}
			}

			//get values
			foreach (PropertyInfo pi in propInfos) {
				Console.WriteLine(
					"[{0}] field of DynProps instance has been set to [{1}]."
					, pi.Name
					, dynPropsType.InvokeMember(
						pi.Name
						, BindingFlags.GetProperty,
						null
						, dynProps
						, new object[] { }
					)
				);
			}

		}
	}
}
