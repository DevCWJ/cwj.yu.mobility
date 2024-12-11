using CWJ.AccessibleEditor;
using CWJ.Singleton;

using UnityEngine;

namespace CWJ.EzPaint
{
    /// <summary>
    /// 런타임중에 기능끌때 gameObject.SetActive(false)하지말고 enabled=false; 하기
    /// </summary>
    //[ExecuteInEditMode]
    public class EzPaintManager : SingletonBehaviour<EzPaintManager>
    {
        private const string EzPaintLayer = nameof(EzPaintLayer);

#if UNITY_EDITOR
        private const string MenuItem_Create2DEzPaintManager = "GameObject/" + nameof(CWJ) + "/" + nameof(EzPaintManager) + "/2D_" + nameof(EzPaintManager);
        private const string MenuItem_Create3DEzPaintManager = "GameObject/" + nameof(CWJ) + "/" + nameof(EzPaintManager) + "/3D_" + nameof(EzPaintManager);
        private static Transform backupParent = null;

        private static bool Is3DPaintManager_Editor;
        private static bool IsCreatedViaMenu_Editor;

        //[UnityEditor.MenuItem(MenuItem_Create2DEzPaintManager, true)]
        private static bool Validate_Create2DEzPaintManager()
        {
            Transform selectTrf = UnityEditor.Selection.activeTransform;
            if (selectTrf != null && selectTrf.GetComponentInParent<Canvas>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [UnityEditor.MenuItem(MenuItem_Create2DEzPaintManager)]
        private static void Create2DEzPaintManager()
        {
            if (!Validate_Create2DEzPaintManager())
            {
                DisplayDialogUtil.DisplayDialogReflection("2D 전용 EzPaintManager를 만드려면\nHierarchy에서 Canvas를 부모로 가진\n오브젝트를 클릭후 다시 메뉴를 선택해주세요", ok: "OK");
                return;
            }
            CreateEzPaintManager(false);
        }

        //[UnityEditor.MenuItem(MenuItem_Create3DEzPaintManager, true)]
        private static bool Validate_Create3DEzPaintManager()
        {
            Transform selectTrf = UnityEditor.Selection.activeTransform;
            if (selectTrf == null || (selectTrf != null && selectTrf.GetComponentInParent<Canvas>() == null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [UnityEditor.MenuItem(MenuItem_Create3DEzPaintManager)]
        private static void Create3DEzPaintManager()
        {
            if (!Validate_Create3DEzPaintManager())
            {
                DisplayDialogUtil.DisplayDialogReflection("3D 전용 EzPaintManager를 만드려면\nHierarchy에서 Canvas를 부모로 갖고있지 않는\n오브젝트를 클릭후 다시 메뉴를 선택해주세요", ok: "OK");
                return;
            }
            CreateEzPaintManager(true);
        }

        private static void CreateEzPaintManager(bool is3DPaint)
        {
            backupParent = UnityEditor.Selection.activeTransform;

            if (IsExists)
            {
                backupParent = null;
                UnityEditor.Selection.activeTransform = Instance.transform;
                return;
            }

            IsCreatedViaMenu_Editor = true;
            Is3DPaintManager_Editor = is3DPaint;
            GameObject managerObj = new GameObject("CWJ_" + nameof(EzPaintManager) + (is3DPaint ? "_3D" : "_2D"), typeof(EzPaintManager));
        }

        protected override void _Reset()
        {
            if (IsCreatedViaMenu_Editor)
            {
                IsCreatedViaMenu_Editor = false;
            }
            else
            {
                return;
            }

            transform.Reset();

            if (backupParent != null)
            {
                transform.SetParentAndReset(backupParent);
                if (Is3DPaintManager_Editor)
                {
                    transform.LossyToLocalScale(Vector3.one);
                }
                else
                {
                    transform.localScale = Vector3.one * 100;
                }
            }

            GameObject paintSystemObj = new GameObject(nameof(EzPaintSystem) + (Is3DPaintManager_Editor ? "_3D" : "_2D"));
            paintSystemObj.transform.SetParentAndReset(transform);
            paintSystemObj.SetLayer(EzPaintLayer);
            paintSystem = (Is3DPaintManager_Editor ? paintSystemObj.AddComponent<EzPaintSystem_3D>() as EzPaintSystem : paintSystemObj.AddComponent<EzPaintSystem_2D>() as EzPaintSystem);

            GameObject touchListenerObj = new GameObject($"{nameof(KeyListener)}_{nameof(EzPaint)}");
            touchListenerObj.transform.SetParentAndReset(transform);
            touchListener = touchListenerObj.AddComponent<KeyListener>();

            _KeyEventManager.UpdateInstance(false);

            UnityEditor.Selection.activeGameObject = touchListener.gameObject;
            transform.hasChanged = true;

            EditorCallback.AddWaitForSecondsCallback(() =>
            {
                UnityEditor.Selection.activeGameObject = touchListener.gameObject;
                touchListener.isMultiTouchOnly = false;
                ResetAddListener(touchListener.onTouchBegan, paintSystem.TouchHandler_HoldDown);
                ResetAddListener(touchListener.onTouchMoving, paintSystem.TouchHandler_HoldDown);
                ResetAddListener(touchListener.onTouchEnded, paintSystem.TouchHandler_Ended);
                //touchListener.onUpdateEnded.AddListener_New(paintSystem.TouchHandler_UpdateEnded);

                UnityEditor.Selection.activeGameObject = gameObject;
                //DisplayDialogUtil.DisplayDialog<EzPaintManager>(gameObject.name + " 생성 완료");
                EditorSetDirty.SetCreateObjDirty(gameObject);
            }, 0.1f);
        }

        private void ResetAddListener(UnityEngine.Events.UnityEvent unityEvent, UnityEngine.Events.UnityAction unityAction)
        {
            unityEvent.RemoveListener_New(unityAction);
            unityEvent.AddListener_New(unityAction);
        }

#endif

        [ErrorIfNull] public EzPaintSystem paintSystem;
        [ErrorIfNull] public KeyListener touchListener;

        protected override void _OnEnable()
        {
            if (paintSystem != null)
            {
                paintSystem.enabled = true;
            }
            if (touchListener != null)
            {
                touchListener.enabled = true;
            }
        }

        protected override void _OnDisable()
        {
            if (paintSystem != null)
            {
                paintSystem.enabled = false;
            }
            if (touchListener != null)
            {
                touchListener.enabled = false;
            }
        }

    }
}