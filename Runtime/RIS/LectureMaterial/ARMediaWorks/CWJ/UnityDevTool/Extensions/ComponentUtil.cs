using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
namespace CWJ
{
    public static class ComponentUtil
    {
        public static bool IsGoHasOnlyThisCompWithRequireComps(Component targetComp)
        {
            if (targetComp == null) return false;
            var targetType = targetComp.GetType();
            var allRequireTypes = CWJ.AccessibleEditor.AttributeUtil.GetAllRequireCompTypes(targetType);

            foreach (var comp in targetComp.transform.GetComponents<Component>())
            {
                if (comp.IsNullOrMissing()) continue;
                var t = comp.GetType();
                if (!allRequireTypes.Contains(t) && !t.Equals(targetType) 
                    && !t.Equals(typeof(Transform)) && !t.Equals(typeof(RectTransform)))
                {
                    return false;
                }
            }
;
            return true;
        }

        public static T CopyComponent<T>(this GameObject destinationObj, T originalComponent) where T : Component
        {
            Type type = originalComponent?.GetType();

            T comp = destinationObj.GetOrAddComponent<T>();

            var fields = ReflectionUtil.GetAllFields(type);

            foreach (var field in fields)
            {
                if (field.IsStatic) continue;

                if (field.Name.Equals("m_CachedPtr") ||
                    field.Name.Equals("m_InstanceID") ||
                    field.Name.Equals("m_UnityRuntimeErrorString") ||
                    field.Name.Equals("OffsetOfInstanceIDInCPlusPlusObject") ||
                    field.Name.Equals("objectIsNullMessage") ||
                    field.Name.Equals("cloneDestroyedMessage")) continue; //deepcopy하려면 이것들 지우면될거임(위험함..)

                field.SetValue(comp, field.GetValue(originalComponent));
            }

            PropertyInfo[] props = type.GetProperties();
            foreach (var prop in props)
            {
                if (prop == null || !prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;

                prop.SetValue(comp, prop.GetValue(originalComponent, null), null);
            }

            return comp as T;
        }

        public static T CopyComponent<T>(this Transform destinationTrf, T originalComponent) where T : Component
        {
            return destinationTrf.gameObject.CopyComponent<T>(originalComponent);
        }

        /// <summary>
        /// 파티클이 종료 시 실행될 콜백이벤트를 추가합니다
        /// </summary>
        /// <param name="particleSystem"></param>
        /// <param name="callback"></param>
        public static void AddParticleStopCallback(this ParticleSystem particleSystem, UnityAction<GameObject> callback)
        {
            if (particleSystem.IsNullOrMissing()) return;
            var callbackHelper = particleSystem.gameObject.GetOrAddComponent<ParticleSystemStopCallbackComp>();
            if (callbackHelper) callbackHelper.stopCallback.AddListener(callback);
        }
    }
}