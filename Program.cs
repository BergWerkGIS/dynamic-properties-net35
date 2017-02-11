using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicPropertiesNet35 {
	class Program {
		static void Main(string[] args) {


			MyProperties2 p2 = new MyProperties2();
			p2.Add<string>("key1", "my string");
			p2.Add<int>("key2", -123);
			p2.Add<uint>("key3", 1);

			TestClass tc = new TestClass();
			PropertyInfo[] propInfosTc = typeof(TestClass).GetProperties();
			foreach(PropertyInfo pi in propInfosTc) {
				Console.WriteLine("TestClass property [{0}] created", pi.ToString());
			}

			MyProperties p = new MyProperties();
			p.AddProperty<uint>("key3");
			p.AddProperty<string>("key1");
			p.AddProperty<int>("key2");

			Type dynPropsType = p.CreateDynamicPropertyTypeInstance();
			PropertyInfo[] propInfos = dynPropsType.GetProperties();
			foreach(PropertyInfo pi in propInfos) {
				Console.WriteLine("DynProps property [{0}] created", pi.ToString());
			}

			object dynProps = Activator.CreateInstance(dynPropsType);

			//set values
			foreach(PropertyInfo pi in propInfos) {

				object val = null;
				if(pi.Name == "key1") { val = "this is a string"; }
				if(pi.Name == "key2") { val = -123; }
				//doesn't work for unit
				if(pi.Name == "key3") {/* val = 1;*/ val = null; }

				if(null == val) { continue; }
				dynPropsType.InvokeMember(
					pi.Name
					, BindingFlags.SetProperty
					, null
					, dynProps
					, new object[] { val }
				);
			}

			//get values
			foreach(PropertyInfo pi in propInfos) {
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
