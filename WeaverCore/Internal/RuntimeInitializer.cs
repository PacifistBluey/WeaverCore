﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WeaverCore.Utilities;
using WeaverCore.Implementations;
using System.Reflection;
using WeaverCore.Interfaces;

namespace WeaverCore.Internal
{
	static class RuntimeInitializer
	{
		[RuntimeInitializeOnLoadMethod]
		static void Load()
		{
			IRuntimePatchRunner.RuntimeInit();
		}

		class IRuntimePatchRunner
		{
			public static void RuntimeInit()
			{
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					DoRuntimeInit(assembly);
				}

				AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
			}

			private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
			{
				DoRuntimeInit(args.LoadedAssembly);
			}

			static void DoRuntimeInit(Assembly assembly)
			{
				foreach (var type in assembly.GetTypes().Where(t => typeof(IRuntimeInit).IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericTypeDefinition))
				{
					var rInit = (IRuntimeInit)Activator.CreateInstance(type);
					try
					{
						rInit.RuntimeInit();
					}
					catch (Exception e)
					{
						Debugger.LogError("Runtime Init Error: " + e);
					}
				}
			}
		}
	}
}
