﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace WeaverCore.Editor.Implementations
{
	public class EditorModLoaderImplementation : WeaverCore.Implementations.ModLoaderImplementation
    {
        static Type VModType = typeof(IWeaverMod);
        static List<IWeaverMod> LoadedMods = new List<IWeaverMod>();

        static void LoadMod(Type ModType)
        {
            try
            {
                var mod = (IWeaverMod)Activator.CreateInstance(ModType);
                LoadedMods.Add(mod);
            }
            catch (Exception e)
            {
                Debugger.LogError($"Failed to load mod : {ModType} : {e}");
            }
        }

        public override IEnumerable<IWeaverMod> LoadAllMods()
        {
            LoadedMods.Clear();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (VModType.IsAssignableFrom(type) && !type.ContainsGenericParameters && !type.IsAbstract)
                        {
                            LoadMod(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {

                }
                catch (Exception e)
                {
                    Debugger.LogError("ModLoader Error : " + e);
                }
            }
            LoadedMods.Sort((v1, v2) => v1.LoadPriority - v2.LoadPriority);
            foreach (var mod in LoadedMods)
            {
                Debugger.Log($"Loading Weaver Mod <b>{mod.Name}</b>, Version: {mod.Version}");
                mod.Load();
                yield return mod;
            }
        }
    }
}