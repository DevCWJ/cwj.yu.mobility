using UnityEditor;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly
{
    public class RequiredLayerTagHandler
    {
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_EditorEventHelper.CompSelectedEvent += RequiredLayerTagLooper;

            CustomAttributeHandler.EditorWillSaveAfterModifiedEvent += RequiredLayerTagLooper; //유니티 저장 시도를 할 때
            CustomAttributeHandler.ExitingEditModeEvent += RequiredLayerTagLooper; // 런타임 첫프레임
            CustomAttributeHandler.EditorSceneOpenedEvent += RequiredLayerTagLooper; //씬 열 때 + 프로젝트 열 때
            CustomAttributeHandler.ReloadedScriptEvent += RequiredLayerTagLooper; //컴파일될때
            CustomAttributeHandler.BeforeBuildEvent += RequiredLayerTagLooper; //빌드 전
        }

        private static void RequiredLayerTagLooper(MonoBehaviour comp, System.Type type)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            //if (instanceID != 0 && comp.GetInstanceID() != instanceID)
            //{
            //    continue;
            //}
            object[] attributes = type.GetCustomAttributes(true);

            for (int i = 0; i < attributes.Length; ++i)
            {
                RequiredLayerAttribute layerAttribute = attributes[i] as RequiredLayerAttribute;
                RequiredTagAttribute tagAttribute = attributes[i] as RequiredTagAttribute;

                if (layerAttribute != null)
                {
                    foreach (string layer in layerAttribute.layers)
                    {
                        if (!LayerEditorUtil.IsExists(layer, printLog: false))
                        {
                            LayerEditorUtil.AddNewLayer(layer);
                        }

                        if (layerAttribute.isMyLayer)
                        {
                            int requiredLayer = LayerMask.NameToLayer(layer);

                            if (comp.gameObject.layer != requiredLayer || (comp.transform.childCount > 0 && comp.transform.GetChild(comp.transform.childCount - 1).gameObject.layer != requiredLayer))
                            {
                                comp.gameObject.SetLayer(layer, isRecursively: layerAttribute.isRecursively);
                            }
                        }
                    }
                    continue;
                }

                if (tagAttribute != null)
                {
                    foreach (string tag in tagAttribute.tags)
                    {
                        if (!TagEditorUtil.IsExists(tag, printLog: false))
                        {
                            TagEditorUtil.AddNewTag(tag);
                        }
                        if (tagAttribute.isMyTag && !comp.CompareTag(tag))
                        {
                            comp.gameObject.SetTag(tag, tagAttribute.isRecursively);
                        }
                    }
                    continue;
                }
            }
        }
    }
}