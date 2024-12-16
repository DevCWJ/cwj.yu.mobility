using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CWJ.Singleton;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CWJ
{
    //개발중
    [CWJInfoBox("에디터 전용 스크립트입니다. 개발중. [CWJ Inspector]에서 Debug Setting 에는 버튼만 있게하기")]
    public class RuntimeLogSetting : Singleton.SingletonBehaviourDontDestroy<RuntimeLogSetting>/*, ICannotPreCreatedInScene*/
    {
#if UNITY_EDITOR

        [Foldout("Ignore Type")]
        [SerializeField, SyncValue_Property(nameof(IsIgnoreSpecifiedType))] bool _IsIgnoreSpecifiedType;
        public bool IsIgnoreSpecifiedType
        {
            get => _IsIgnoreSpecifiedType;
            set
            {
                _IsIgnoreSpecifiedType = value;
                CWJ_Debug.IsIgnoreSpecifiedType = value;
            }
        }
        [Foldout("Ignore Type")]
        [SerializeField, HideConditional(nameof(_IsIgnoreSpecifiedType))] List<MonoScript> ignoreTypeList;

        [Foldout("Allow Type")]
        [SerializeField, SyncValue_Property(nameof(IsAllowSpecifiedType))] private bool _IsAllowSpecifiedType;
        public bool IsAllowSpecifiedType
        {
            get => _IsAllowSpecifiedType;
            set
            {
                _IsAllowSpecifiedType = value;
                CWJ_Debug.IsAllowSpecifiedType = value;
            }
        }
        [OnValueChanged("OnValueChange_AllowType")] public UnityDevToolExample attributeTest;
        [Foldout("Allow Type")]
        [SerializeField, HideConditional(nameof(_IsAllowSpecifiedType)), OnValueChanged(nameof(OnValueChange_AllowType))] MonoScript[] allowTypes = new MonoScript[0];
        void OnValueChange_AllowType()
        {
            allowTypes = allowTypes.Where(m => m != null).Distinct().ToArray();
            CWJ_Debug.ClearAllowTypes();
            CWJ_Debug.RegistAllowType(allowTypes.ConvertAll(m => m.GetClass().GetHashCode()), true);
        }

        [DrawHeaderAndLine("Obj Setting")]
        [SerializeField] private bool _IsIgnoreSpecifiedObj;
        public bool IsIgnoreSpecifiedObj
        {
            get => _IsIgnoreSpecifiedObj;
            set
            {
                _IsIgnoreSpecifiedObj = value;
                CWJ_Debug.IsIgnoreSpecifiedObj = value;
            }
        }

        [SerializeField] private bool _IsAllowSpecifiedObj;
        public bool IsAllowSpecifiedObj
        {
            get => _IsAllowSpecifiedObj;
            set
            {
                _IsAllowSpecifiedObj = value;
                CWJ_Debug.IsAllowSpecifiedObj = value;
            }
        }

        [OnValueChanged(nameof(OnValueChange_AllowType)), SerializeField] private bool isValueChange;




        /// <summary>
        /// 말그대로 빌드에 영향을 주는 Debug설정창 열기 
        /// <para>빌드에 영향주는것 말고도 저장을 시킬수도있음</para>
        /// </summary>
        [InvokeButton]
        void OpenDebugSettingThatAffectBuild()
        {
            isValueChange = !isValueChange;
            CWJ.AccessibleEditor.DebugSetting.DebugSetting_Window.Open();
        }
        protected override void _OnValidate()
        {
        }
#else
        protected override void _Awake()
        {
            DestroySingletonObj(gameObject);
        }
#endif
    }
}