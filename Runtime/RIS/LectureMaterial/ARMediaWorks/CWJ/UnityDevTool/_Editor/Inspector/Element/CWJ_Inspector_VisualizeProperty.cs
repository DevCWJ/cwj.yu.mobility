using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly.Inspector
{
    public class CWJ_Inspector_VisualizeProperty : CWJ_Inspector_ElementAbstract, CWJ_Inspector_ElementAbstract.IUseMemberInfo<PropertyInfo>
    {
        protected override string ElementClassName => nameof(CWJ_Inspector_VisualizeProperty);
        public CWJ_Inspector_VisualizeProperty(CWJ_Inspector_Core inspectorCore, bool isForciblyDrawAllMembers = false) : base(inspectorCore, false, isForciblyDrawAllMembers)
        {
            var visualizeAllPropertyAttribute = targetType.GetCustomAttribute<VisualizeProperty_AllAttribute>();
            hasVisualizeAllAtt = visualizeAllPropertyAttribute != null;
            if (!hasVisualizeAllAtt)
            {
                origin_isFindAllBaseClass = true;
                origin_isAllReadonly = targetComp.hideFlags.HasFlag(HideFlags.NotEditable);
            }
            else
            {
                origin_isFindAllBaseClass = visualizeAllPropertyAttribute.isFindAllBaseClass;
                origin_isAllReadonly = visualizeAllPropertyAttribute.isReadonly || targetComp.hideFlags.HasFlag(HideFlags.NotEditable);
            }
            propAndVariousTypeDrawerList = new List<PropAndDrawer>();

        }

        protected override bool HasMemberToDraw()
        {
            return usePropertyInfo.memberInfoArray.Length > 0;
        }

        public override int drawOrder => 6;

        private bool hasVisualizeAllAtt;
        private bool origin_isFindAllBaseClass;
        private bool origin_isAllReadonly;

        private bool isFindAllBaseClass;
        private bool isAllReadonly;

        bool IUseMemberInfo<PropertyInfo>.MemberInfoClassifyPredicate(PropertyInfo info)
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
                    if (!info.IsDefined(typeof(VisualizePropertyAttribute), true))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!isFindAllBaseClass && !info.IsDeclaredTargetType(targetType))
                    {
                        return false;
                    }
                }
            }

            if (info.GetGetMethod(true) == null)
            {
                return false;
            }

            var variousTypeDrawer = EditorGUI_CWJ.GetDrawVariousTypeDelegate(info.PropertyType);

            if (variousTypeDrawer != null)
            {
                propAndVariousTypeDrawerList.Add(new PropAndDrawer(info, variousTypeDrawer));
                return true;
            }
            else
            {
                return false;
            }
        }
        PropertyInfo[] IUseMemberInfo<PropertyInfo>.memberInfoArray { get; set; }

        struct PropAndDrawer
        {
            public PropertyInfo propertyInfo;
            public EditorGUI_CWJ.DrawVariousTypeHandler variousTypeDrawer;

            public PropAndDrawer(PropertyInfo propertyInfo, EditorGUI_CWJ.DrawVariousTypeHandler variousTypeDrawer)
            {
                this.propertyInfo = propertyInfo;
                this.variousTypeDrawer = variousTypeDrawer;
            }
        }

        List<PropAndDrawer> propAndVariousTypeDrawerList;
        PropAndDrawer[] propAndVariousTypeDrawers;

        const string VisualizeAllProperties = "Visualize All Properties";

        protected override void OnEndClassify()
        {
            propAndVariousTypeDrawers = propAndVariousTypeDrawerList.ToArray();
            propAndVariousTypeDrawerList.Clear();
            propAndVariousTypeDrawerList.Capacity = propAndVariousTypeDrawers.Length;

            foldoutContent_root.text = " Visualized Properties " + (GetHasMemberToDraw() ? $"[{usePropertyInfo.memberInfoArray.Length}]" : "");
            if (!isDrawBodyPart && foldoutContent_root.image != null)
            {
                foldoutContent_root.image = null;
            }
        }

        protected override void DrawInspector()
        {
            EditorGUI_CWJ.DrawBigFoldout(ref isRootFoldoutExpand, foldoutContent_root, (isExpand) =>
            {
                if (!isExpand) return;

                if (ForciblyDrawAllMembersButton(VisualizeAllProperties))
                {

                }

                if (!GetHasMemberToDraw()) return;

                if (!isAllReadonly) WarningThatNonSerialized();

                foreach (var pd in propAndVariousTypeDrawers)
                {
                    DrawProperty(pd.propertyInfo, pd.variousTypeDrawer);
                }
            });
        }

        private void DrawProperty(PropertyInfo propertyInfo, EditorGUI_CWJ.DrawVariousTypeHandler drawVariousType)
        {
            var result = EditorGUI_CWJ.DrawVariousPropertyTypeWithAtt(propertyInfo, propertyInfo.Name, target, isAllReadonly, drawVariousType);

            //if (result.isChanged) //changed
        }


    }
}