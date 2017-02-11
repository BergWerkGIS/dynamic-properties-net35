using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace DynamicPropertiesNet35 {



	public class MyProperties {

		public MyProperties() {
			_propertiesToCreate = new Dictionary<string, Type>();
		}


		private Dictionary<string, Type> _propertiesToCreate;


		public void AddProperty<T>(string key) {
			_propertiesToCreate.Add(key, typeof(T));
		}


		public Type CreateDynamicPropertyTypeInstance() {
			return BuildDynamicTypeWithProperties();
		}

		/// <summary>
		/// based on https://msdn.microsoft.com/en-us/library/2sd82fz7(v=vs.90).aspx
		/// </summary>
		/// <returns></returns>
		private  Type BuildDynamicTypeWithProperties() {
			AppDomain myDomain = Thread.GetDomain();
			AssemblyName myAsmName = new AssemblyName();
			myAsmName.Name = "DynamicPropertiesAssembly";

			// To generate a persistable assembly, specify AssemblyBuilderAccess.RunAndSave.
			AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(
				myAsmName,
				AssemblyBuilderAccess.RunAndSave
			);
			// Generate a persistable single-module assembly.
			ModuleBuilder myModBuilder = myAsmBuilder.DefineDynamicModule(myAsmName.Name, myAsmName.Name + ".dll");

			TypeBuilder myTypeBuilder = myModBuilder.DefineType(
				"DynamicProperties",
				TypeAttributes.Public
			);

			foreach(var key in _propertiesToCreate) {
				addProperty(key.Key, key.Value, myTypeBuilder);
			}

			Type retval = myTypeBuilder.CreateType();

			// Save the assembly so it can be examined with Ildasm.exe, 
			// or referenced by a test program.
			myAsmBuilder.Save(myAsmName.Name + ".dll");
			return retval;
		}


		private void addProperty(string key, Type type, TypeBuilder myTypeBuilder) {


			FieldBuilder customerNameBldr = myTypeBuilder.DefineField(
				"_" + key,
				type,
				FieldAttributes.Private
			);

			// The last argument of DefineProperty is null, because the 
			// property has no parameters. (If you don't specify null, you must 
			// specify an array of Type objects. For a parameterless property, 
			// use an array with no elements: new Type[] {})
			PropertyBuilder custNamePropBldr = myTypeBuilder.DefineProperty(
				key,
				PropertyAttributes.HasDefault,
				type,
				null
		);

			// The property set and property get methods require a special 
			// set of attributes.
			//MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
			MethodAttributes getSetAttr = MethodAttributes.Public |	MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual;

			// Define the "get" accessor method for CustomerName.
			MethodBuilder custNameGetPropMthdBldr =
				myTypeBuilder.DefineMethod(
					"get_" + key,
					getSetAttr,
					type,
					Type.EmptyTypes
				);

			ILGenerator custNameGetIL = custNameGetPropMthdBldr.GetILGenerator();

			custNameGetIL.Emit(OpCodes.Ldarg_0);
			custNameGetIL.Emit(OpCodes.Ldfld, customerNameBldr);
			custNameGetIL.Emit(OpCodes.Ret);

			// Define the "set" accessor method for CustomerName.
			MethodBuilder custNameSetPropMthdBldr = myTypeBuilder.DefineMethod(
				"set_" + key,
				getSetAttr,
				null,
				new Type[] { type }
			);

			ILGenerator custNameSetIL = custNameSetPropMthdBldr.GetILGenerator();

			custNameSetIL.Emit(OpCodes.Ldarg_0);
			custNameSetIL.Emit(OpCodes.Ldarg_1);
			custNameSetIL.Emit(OpCodes.Stfld, customerNameBldr);
			custNameSetIL.Emit(OpCodes.Ret);

			// Last, we must map the two methods created above to our PropertyBuilder to  
			// their corresponding behaviors, "get" and "set" respectively. 
			custNamePropBldr.SetGetMethod(custNameGetPropMthdBldr);
			custNamePropBldr.SetSetMethod(custNameSetPropMthdBldr);

		}


	}



}
