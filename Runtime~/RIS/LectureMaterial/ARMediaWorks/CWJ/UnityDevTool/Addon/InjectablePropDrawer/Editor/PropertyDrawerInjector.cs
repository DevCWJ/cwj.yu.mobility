#if UNITY_EDITOR
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    public static class PropertyDrawerInjector
    {
        const BindingFlags kBfAll = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        static readonly Type _TypeScriptAttributeUtility = Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor");
        static readonly Type _TypeDrawerKeySet = Type.GetType("UnityEditor.ScriptAttributeUtility+DrawerKeySet, UnityEditor"); //class내에 선언된 구조체의 경우 이렇게 +로 표시
        static readonly MethodInfo _MiBuildDrawerTypeForTypeDictionary = _TypeScriptAttributeUtility.GetMethod("BuildDrawerTypeForTypeDictionary", kBfAll);
        static readonly FieldInfo _FiDrawerTypeForType = _TypeScriptAttributeUtility.GetField("s_DrawerTypeForType", kBfAll);
        static readonly FieldInfo _FiDrawer = _TypeDrawerKeySet.GetField("drawer", kBfAll);
        static readonly FieldInfo _FiType = _TypeDrawerKeySet.GetField("type", kBfAll);

        static IDictionary _dicDrawerTypeForType;
        /// <summary>
        /// Gets [Type -> DrawerType] dictionary.
        /// </summary>
        public static IDictionary drawerTypeForType
        {
            get
            {
                if (_dicDrawerTypeForType == null)
                {
                    // Get [Type -> DrawerType] dictionary from ScriptAttributeUtility class. 
                    _dicDrawerTypeForType = _FiDrawerTypeForType.GetValue(null) as IDictionary;
                    if (_dicDrawerTypeForType == null)
                    {
                        _MiBuildDrawerTypeForTypeDictionary.Invoke(null, new object[0]);
                        _dicDrawerTypeForType = _FiDrawerTypeForType.GetValue(null) as IDictionary;
                    }
                }

                return _dicDrawerTypeForType;
            }
        }

        static Type[] _loadedTypes;
        /// <summary>
        /// Gets all loaded types in current domain.
        /// </summary>
        public static Type[] loadedTypes
        {
            get
            {
                if (_loadedTypes == null)
                {
                    _loadedTypes = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(x => x.GetTypes())
                        .ToArray();
                }
                return _loadedTypes;
            }
        }


        /// <summary>
        /// Inject property drawer on load method.
        /// </summary>
        [UnityEditor.InitializeOnLoadMethod]
        public static void InjectPropertyDrawer()
        {
            // Find all drawers.
            foreach (var drawerType in loadedTypes.Where(x => x.IsSubclassOf(typeof(GUIDrawer))))
            {
                // Find all InjectablePropertyDrawer attributes.
                object[] attrs = drawerType.GetCustomAttributes(typeof(InjectablePropertyDrawer), true);
                foreach (InjectablePropertyDrawer attr in attrs)
                {
                    // Inject drawer type.
                    InjectPropertyDrawer(drawerType, attr);
                }
            }
        }

        /// <summary>
        /// Inject property drawer.
        /// </summary>
        /// <param name="drawerType">CustomPropertyDrawer type.</param>
        /// <param name="attr">InjectablePropertyDrawer attribute.</param>
        public static void InjectPropertyDrawer(Type drawerType, InjectablePropertyDrawer attr)
        {
            // Create drawer key set.
            object keyset = Activator.CreateInstance(_TypeDrawerKeySet);
            _FiDrawer.SetValue(keyset, drawerType);
            _FiType.SetValue(keyset, attr.type);

            // Inject drawer type.
            drawerTypeForType[attr.type] = keyset;

            // Inject drawer type for subclass.
            if (attr.useForChildren)
            {
                foreach (var type in loadedTypes.Where(x => x.IsSubclassOf(attr.type)))
                {
                    drawerTypeForType[type] = keyset;
                }
            }
        }

        /// <summary>
        /// Gets the drawer type for type.
        /// </summary>
        /// <returns>The drawer type.</returns>
        /// <param name="type">The type.</param>
        public static Type GetDrawerType(Type type)
        {
            return drawerTypeForType.Contains(type)
                ? _FiDrawer.GetValue(drawerTypeForType[type]) as Type
                : null;
        }
    }
} 
#endif