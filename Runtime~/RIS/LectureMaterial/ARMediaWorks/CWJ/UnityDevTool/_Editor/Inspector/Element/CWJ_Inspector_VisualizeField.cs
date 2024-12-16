using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly.Inspector
{
    using static ForcedFieldSerialization;

    public class CWJ_Inspector_VisualizeField : CWJ_Inspector_ElementAbstract, CWJ_Inspector_ElementAbstract.IUseMemberInfo<FieldInfo>
    {
        protected override string ElementClassName => nameof(CWJ_Inspector_VisualizeField);

        public CWJ_Inspector_VisualizeField(CWJ_Inspector_Core inspectorCore, bool isForciblyDrawAllMembers = false) : base(inspectorCore, false, isForciblyDrawAllMembers)
        {
            var visualizeAllNonPublicAttribute = targetType.GetCustomAttribute<VisualizeField_AllAttribute>();
            hasVisualizeAllAtt = visualizeAllNonPublicAttribute != null;
            if (!hasVisualizeAllAtt)
            {
                origin_isFindAllBaseClass = true;
                origin_isAllReadonly = targetComp.hideFlags.HasFlag(HideFlags.NotEditable);
            }
            else
            {
                origin_isFindAllBaseClass = visualizeAllNonPublicAttribute.isFindAllBaseClass;
                origin_isAllReadonly = visualizeAllNonPublicAttribute.isReadonly || targetComp.hideFlags.HasFlag(HideFlags.NotEditable);
            }

            fieldAndVariousTypeDrawerList = new List<FieldAndDrawer>();
        }

        public override int drawOrder => 5;

        private bool hasVisualizeAllAtt;
        private bool origin_isFindAllBaseClass;
        private bool origin_isAllReadonly;

        private bool isFindAllBaseClass; //for ForcedSerialization
        private bool isAllReadonly;

        bool IUseMemberInfo<FieldInfo>.MemberInfoClassifyPredicate(FieldInfo info)
        {
            if (isForciblyDrawAllMembers)
            {
                isFindAllBaseClass = true;
                isAllReadonly = !EditorUtil.isCWJDebuggingMode;
            }
            else
            {
                isFindAllBaseClass = origin_isFindAllBaseClass;
                isAllReadonly = origin_isAllReadonly;

                if (!hasVisualizeAllAtt)
                {
                    if (!info.IsDefined(typeof(VisualizeFieldAttribute), true))
                    {
                        return false;
                    }
                }
                else
                {
                    if (info.IsSerializeField() && !info.IsDefined(typeof(VisualizeFieldAttribute)))
                    {
                        return false;
                    }

                    if (!isFindAllBaseClass && !info.IsDeclaredTargetType(targetType))
                    {
                        return false;
                    }
                }
            }

            if (info.IsAutoPropertyField())
            {
                return false;
            }

            var variousTypeDrawer = EditorGUI_CWJ.GetDrawVariousTypeDelegate(info.FieldType);

            if (variousTypeDrawer != null)
            {
                fieldAndVariousTypeDrawerList.Add(new FieldAndDrawer(info, variousTypeDrawer));

                return true;
            }
            else
            {
                return false;
            }
        }

        FieldInfo[] IUseMemberInfo<FieldInfo>.memberInfoArray { get; set; }

        struct FieldAndDrawer
        {
            public FieldInfo fieldInfo;
            public EditorGUI_CWJ.DrawVariousTypeHandler variousTypeDrawer;

            public FieldAndDrawer(FieldInfo fieldInfo, EditorGUI_CWJ.DrawVariousTypeHandler variousTypeDrawer)
            {
                this.fieldInfo = fieldInfo;
                this.variousTypeDrawer = variousTypeDrawer;
            }
        }

        List<FieldAndDrawer> fieldAndVariousTypeDrawerList;
        FieldAndDrawer[] fieldAndVariousTypeDrawers;


        protected override bool HasMemberToDraw()
        {
            return useFieldInfo.memberInfoArray.Length > 0;
        }

        protected override void OnEndClassify()
        {
            fieldAndVariousTypeDrawers = fieldAndVariousTypeDrawerList.ToArray();
            fieldAndVariousTypeDrawerList.Clear();
            fieldAndVariousTypeDrawerList.Capacity = fieldAndVariousTypeDrawers.Length;

            foldoutContent_root.text = " Visualized Fields " + (GetHasMemberToDraw() ? $"[{useFieldInfo.memberInfoArray.Length}]" : "");
            if (!isDrawBodyPart && foldoutContent_root.image != null)
            {
                foldoutContent_root.image = null;
            }
        }

        const string VisualizeAllFields = "Visualize All Fields";

        protected override void DrawInspector()
        {
            EditorGUI_CWJ.DrawBigFoldout(ref isRootFoldoutExpand, foldoutContent_root, (isExpand) =>
            {
                if (!isExpand) return;

                if (ForciblyDrawAllMembersButton(VisualizeAllFields))
                {
                }

                if (!GetHasMemberToDraw()) return;

                if (!isAllReadonly) WarningThatNonSerialized();

                foreach (var fieldAndDrawer in fieldAndVariousTypeDrawers)
                {
                    DrawNonSerializeFields(fieldAndDrawer.fieldInfo, fieldAndDrawer.variousTypeDrawer);
                }
            });
        }

        private void DrawNonSerializeFields(FieldInfo fieldInfo, EditorGUI_CWJ.DrawVariousTypeHandler drawVariousType)
        {
            var result = EditorGUI_CWJ.DrawVariousFieldTypeWithAtt(fieldInfo, fieldInfo.Name, target, isAllReadonly, drawVariousType);

            if (result.isChanged) //changed
            {
                if (CodeContainer.IsPossibleConvertToCode(fieldInfo.FieldType))
                {
                    AddSerializationCache(fieldInfo, result.value, isFindAllBaseClass);
                    SetDirty();
                }
            }

            GUI.enabled = true;
        }

        protected override void _OnDisable(bool isDestroy)
        {

        }
    }
}