using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly.Inspector
{
    public abstract class CWJ_Inspector_ElementAbstract
    {
        protected abstract string ElementClassName { get; }

        public interface IUseMemberInfo<T> where T : MemberInfo
        {
            bool MemberInfoClassifyPredicate(T info);
            T[] memberInfoArray { get; set; }
        }

        protected bool isUseFieldInfo { get; private set; } = false;
        protected IUseMemberInfo<FieldInfo> useFieldInfo = null;
        protected bool isUsePropertyInfo { get; private set; } = false;
        protected IUseMemberInfo<PropertyInfo> usePropertyInfo { get; private set; } = null;
        protected bool isUseMethodInfo { get; private set; } = false;
        protected IUseMemberInfo<MethodInfo> useMethodInfo { get; private set; } = null;


        public bool GetHasMemberToDraw() => hasMemberToDraw;
        /// <summary>
        /// endClassifyEvent && Set hasAttribute
        /// </summary>
        /// <returns></returns>
        protected abstract bool HasMemberToDraw();
        private bool hasMemberToDraw;


        public bool CheckIsDrawing() => isRootFoldoutExpand && hasMemberToDraw;

        protected abstract void OnEndClassify();

        protected readonly CWJ_Inspector_Core inspectorCore;

        protected readonly UnityEngine.Object target;
        protected readonly UnityEngine.Object[] targets;
        protected readonly MonoBehaviour targetComp;
        protected readonly Type targetType;
        protected readonly int targetInstanceID;
        protected readonly string targetInstanceIDStr;

        protected readonly SerializedObject serializedObject;

        public bool isForciblyDrawAllMembers { get; private set; }
        /// <summary>
        /// or draw specialFoldoutGroup
        /// </summary>
        public bool isDrawBodyPart { get; private set; }
        public readonly bool isBodyAndFoldoutDrawer;

        public CWJ_Inspector_ElementAbstract(CWJ_Inspector_Core inspectorCore, bool isOnlyDrawBodyPart, bool isForciblyDrawAllMembers)
        {
            hasMemberToDraw = false;
            this.inspectorCore = inspectorCore;
            this.target = inspectorCore.target;
            this.targets = inspectorCore.targets;
            this.targetComp = inspectorCore.targetComp;
            this.targetType = inspectorCore.targetType;
            this.targetInstanceID = inspectorCore.targetInstanceID;
            this.targetInstanceIDStr = inspectorCore.targetInstanceIDStr;
            this.serializedObject = inspectorCore.serializedObject;
            this.isForciblyDrawAllMembers = isForciblyDrawAllMembers;
            isBodyAndFoldoutDrawer = isDrawBodyPart = isOnlyDrawBodyPart;

            SubscribeInspectorEvent(isNeedInit: true);
        }

        private void SubscribeInspectorEvent(bool isNeedInit)
        {

            Type[] useMemberInfoTypes = (this.GetType().GetInterfaces()
                                        .Where(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IUseMemberInfo<>))
                                        .Select(t => t.GetGenericArguments()[0]).ToArray());

            if (isUseFieldInfo = useMemberInfoTypes.Contains(typeof(FieldInfo)))
            {
                useFieldInfo = GetInterfaceAndSubscribeEvent(ref inspectorCore.addFieldEvent);
            }
            if (isUsePropertyInfo = useMemberInfoTypes.Contains(typeof(PropertyInfo)))
            {
                usePropertyInfo = GetInterfaceAndSubscribeEvent(ref inspectorCore.addPropertyEvent);
            }
            if (isUseMethodInfo = useMemberInfoTypes.Contains(typeof(MethodInfo)))
            {
                useMethodInfo = GetInterfaceAndSubscribeEvent(ref inspectorCore.addMethodEvent);
            }

            inspectorCore.endClassifyEvent += () =>
            {
                hasMemberToDraw = HasMemberToDraw();
                isDrawBodyPart = isBodyAndFoldoutDrawer || (!isForciblyDrawAllMembers && hasMemberToDraw);

                OnEndClassify();

                if (isNeedInit)
                {
                    inspectorCore.onEnableEvent += OnEnable;
                    inspectorCore.onDisableEvent += OnDisable;
                    inspectorCore.AddDrawEvent(isDrawBodyPart, drawOrder, DrawInspector);
                }
            };
            _StartClassifyMemberInfos();
        }
        protected virtual void _StartClassifyMemberInfos() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns>Changed toggle state</returns>
        protected bool ForciblyDrawAllMembersButton(string buttonName)
        {
            bool isToggleOn = GUILayout.Toggle(isForciblyDrawAllMembers, GetForciblyDrawAllMembersContent(buttonName, isForciblyDrawAllMembers, hasMemberToDraw));
            if(isToggleOn != isForciblyDrawAllMembers)
            {
                isForciblyDrawAllMembers = isToggleOn;
                SubscribeInspectorEvent(isNeedInit: false);
                inspectorCore.InvokeInitMemberInfoEvent();
                return true;
            }
            return false;
        }

        protected IUseMemberInfo<T> GetInterfaceAndSubscribeEvent<T>(ref CWJ_Inspector_Core.AddMemberHandler<T> addMemberHandler) where T : MemberInfo
        {
            IUseMemberInfo<T> infoInterface = this as IUseMemberInfo<T>;
            infoInterface.memberInfoArray = null;

            var memberInfoList = new List<T>();
            
            addMemberHandler += (mi) =>
            {
                if (infoInterface.MemberInfoClassifyPredicate(mi)) { memberInfoList.Add(mi); }
            };
            inspectorCore.endClassifyEvent += () =>
            {
                infoInterface.memberInfoArray = memberInfoList.ToArray();
                memberInfoList = null;
            };

            return infoInterface;
        }
        private string PrefsKey_RootFoldout() => VolatileEditorPrefs.GetVolatilePrefsKey_Root(ElementClassName + ".RootFoldoutExpanded");
        protected bool isRootFoldoutExpand;

        protected void OnEnable()
        {
            if (!isBodyAndFoldoutDrawer)
            {
                isRootFoldoutExpand = VolatileEditorPrefs.ExistsStackValue(PrefsKey_RootFoldout(), targetInstanceID.ToString());
            }

            _OnEnable();
        }

        protected virtual void _OnEnable() { }

        public abstract int drawOrder { get; }

        protected abstract void DrawInspector();

        protected void OnDisable(bool isDestroyByUser)
        {
            _OnDisable(isDestroyByUser);

            if (!isBodyAndFoldoutDrawer)
            {
                if (!isDestroyByUser && (isDrawBodyPart || inspectorCore.isSpecialFoldoutExpand) && isRootFoldoutExpand)
                {
                    inspectorCore.isElementFoldoutExpanded = true;
                    VolatileEditorPrefs.AddStackValue(PrefsKey_RootFoldout(), targetInstanceID.ToString());
                }
                else
                {
                    VolatileEditorPrefs.RemoveStackValue(PrefsKey_RootFoldout(), targetInstanceID.ToString());
                }
            }
        }

        protected virtual void _OnDisable(bool isDestroy) { }

        GUIContent _foldoutContent_root;
        protected GUIContent foldoutContent_root
        {
            get
            {
                if (_foldoutContent_root == null)
                {
                    _foldoutContent_root = new GUIContent();
                    _foldoutContent_root.image = AccessibleEditorUtil.EditorHelperObj.IconTexture;
                }
                return _foldoutContent_root;
            }
        }

        GUIContent _forciblyDrawAllMembersContent;

        protected GUIContent GetForciblyDrawAllMembersContent(string text, bool isEnabled, bool hasMembers)
        {
            if (_forciblyDrawAllMembersContent == null)
            {
                _forciblyDrawAllMembersContent = new GUIContent(text: " " + text, image: inspectorCore.DisabledImg);
            }

            _forciblyDrawAllMembersContent.image = isEnabled ? (hasMembers ? inspectorCore.EnabledImg : inspectorCore.Enabled_nullImg) : inspectorCore.DisabledImg;

            return _forciblyDrawAllMembersContent;
        }

        //deprecated soon
        public void WarningThatNonSerialized()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("When unity is compiled, they'll be initializing.\nYou should keep in mind that they hadn't been serialized.", EditorGUICustomStyle.InspectorBox);
            }
        }

        protected void SetDirty()
        {
            serializedObject.ApplyModifiedProperties();
            if (!Application.isPlaying)
                EditorUtility.SetDirty(target);
        }
    }
}