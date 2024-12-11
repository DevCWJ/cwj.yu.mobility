using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

namespace CWJ
{
    [DisallowMultipleComponent]
    public class FbxAutoSetupHelper : MonoBehaviour
    {
        [DrawHeaderAndLine("Settings")]
        public bool isAnimLoop = true;
        public bool isAnimLoopPose = false;

        [DrawHeaderAndLine("Cache")]
        [Readonly, SerializeField] string[] clipNamesCache = null;
        [Readonly, SerializeField] string[] triggerNamesCache = null;
        public const string AnimEmptyState = "EmptyState";
        public const string PreviewAnimNameTag = "__preview__";
        public const string TurnOffTrigger = "off";
        protected const string AutoSetupFolderName = "CWJ_AnimatorSetup";
        protected const string AnimStartCallbackMethodName = nameof(AnimatorHandler.Evt_OnAnimationStart);
        protected const string AnimEndCallbackMethodName = nameof(AnimatorHandler.Evt_OnAnimationEnd);

#if UNITY_EDITOR
        [Readonly] public string prefabFilePathCache;
        static bool GetFbxPathFromPrefabPathCache(string prefabFilePathCache, out string fbxPath, out string prefabPath)
        {
            if (string.IsNullOrEmpty(prefabFilePathCache))
            {
                prefabPath = null;
                fbxPath = null;
                return false;
            }
            else
            {
                if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabFilePathCache) != null)
                {
                    prefabPath = prefabFilePathCache;
                    fbxPath = prefabPath.ReplaceEnd(FbxInteractor.NameTag + ".prefab", ".fbx"); //프리팹이름이 ~_Interactor.prefab일거라서
                    return true;
                }
                else
                {
                    prefabPath = null;
                    fbxPath = null;
                    prefabFilePathCache = null;
                    return false;
                }
            }
        }


#region Animator Auto Setup

        /// <summary>
        /// AnimationClip, Animator 자동세팅
        /// </summary>
        [InvokeButton(isEmphasizeBtn:true)]
        protected void Start_AnimatorSetup()
        {
            try
            {
                if (!TryGetThisFilePath(gameObject, prefabFilePathCache, out string fbxFilePath, out string prefabFilePath))
                {
                    return;
                }

                string fbxFolderPath = GetFbxFolderDirectory(ref fbxFilePath, ref prefabFilePath, out string fileName);
                if (string.IsNullOrEmpty(fbxFolderPath))
                {
                    return;
                }

                int clipCnt = ApplyLoopSettingsToAnimationClips(fbxFilePath, isAnimLoop, isAnimLoopPose);

                if (clipCnt > 0)
                {
                    string controllerPath = Path.Combine(fbxFolderPath, fileName + ".controller");
                    bool shouldCreateController = true;
                    AnimatorController animatorController = null;

                    if (File.Exists(controllerPath))
                    {
                        int option = EditorUtility.DisplayDialogComplex(
                            "경고 : Animator Controller 중첩 확인",
                            $"이미 아래 경로에 존재합니다:\n{controllerPath}\n\n기존 Animator Controller를 덮어쓸까요?",
                            "덮어쓰기 (새로 생성)",
                            "작업 중단",
                            "기존 것 유지");

                        if (option == 1)
                        {
                            Debug.Log("사용자에 의해 작업이 취소되었습니다.");
                            return;
                        }
                        if (option == 0)
                        {
                            AssetDatabase.DeleteAsset(controllerPath);
                            shouldCreateController = true;
                        }
                        else
                        {
                            shouldCreateController = false;
                            animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
                        }
                    }

                    if (shouldCreateController)
                    {
                        AnimationClip[] clips = GetValidAnimationClipWithEvent(fbxFilePath, fbxFolderPath, isAnimLoop, isAnimLoopPose, out clipNamesCache);
                        if (clips.Length > 0)
                        {
                            // 언더스코어('_')가 포함된 애니메이션 클립 이름 가져오기
                            var underscoreNames = clipNamesCache.Where(c => c.Contains("_")).ToArray();

                            bool shouldAnimatorHasMultiLayer = false;
                            if (underscoreNames.Length > 0)
                            {
                                int option = EditorUtility.DisplayDialogComplex(
                                    "Animator Controller 아바타 마스크 설정",
                                    "여러 레이어가 필요한 애니메이션이 감지되었습니다.\n" +
                                    "아래와 같은 애니메이션 이름들을 발견했습니다:\n" +
                                            $"{string.Join(", ", underscoreNames)}\n\n" +
                                    "멀티 레이어를 사용하시겠습니까?",
                                    "예 (멀티레이어 적용)",
                                    "작업 중단",
                                    "아니오");
                                if (option == 1)
                                {
                                    Debug.Log("사용자에 의해 작업이 취소되었습니다.");
                                    return;
                                }
                                shouldAnimatorHasMultiLayer = option == 0;
                            }

                            animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

                            if (shouldAnimatorHasMultiLayer)
                            {
                                SetupAnimatorMultiLayer(animatorController, clips, controllerPath, transform, fbxFolderPath, out triggerNamesCache);
                            }
                            else
                            {
                                SetupAnimatorSingleLayer(animatorController, clips, controllerPath, out triggerNamesCache);
                            }
                            AssetDatabase.SaveAssets();
                        }
                        else
                        {
                            Debug.LogWarning("FBX 파일에서 애니메이션 클립을 찾을 수 없습니다.");
                        }
                    }

                    if (animatorController != null)
                    {
                        var _animator = gameObject.GetOrAddComponent<Animator>();
                        _animator.runtimeAnimatorController = animatorController;
                        var animatorHandler = gameObject.GetOrAddComponent<AnimatorHandler>();
                        animatorHandler.InitWhenAutoSetup(_animator, triggerNamesCache, clipNamesCache);

                        Selection.activeObject = animatorController;
                        EditorGUIUtility.PingObject(animatorController);

                    }
                    else
                    {
                        Debug.LogError("Animator Controller를 생성하거나 로드하는 데 실패했습니다.");
                    }
                }



                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Start_AnimatorSetup 처리 중 오류 발생: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                bool flag = true;
                while (flag)
                {
                    flag = UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
                    // 맨 위로 올라갔을 경우 false 리턴
                }
            }
        }

        static string GetFbxFolderDirectory(ref string fbxAssetPath, ref string prefabPath, out string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fbxAssetPath);

            string targetFbxDirectory = Path.GetDirectoryName(fbxAssetPath);
            string parentDirectory = Path.GetDirectoryName(targetFbxDirectory);
            prefabPath = null;
            if (string.IsNullOrEmpty(parentDirectory))
            {
                Debug.LogError($"상위 디렉터리를 찾을 수 없습니다. 현재 디렉터리: {targetFbxDirectory}");
                return null;
            }

            string grandParentDirectory = Path.GetDirectoryName(parentDirectory);
            string parentFolderName = Path.GetFileName(parentDirectory);
            string grandParentFolderName = string.Empty;
            if (!string.IsNullOrEmpty(grandParentDirectory))
            {
                grandParentFolderName = Path.GetFileName(grandParentDirectory);
            }

            if (parentFolderName == AutoSetupFolderName || grandParentFolderName == AutoSetupFolderName)
            {
                targetFbxDirectory = Path.GetDirectoryName(fbxAssetPath);
            }
            else
            {
                string cwjFolderPath = Path.Combine(parentDirectory, AutoSetupFolderName);
                if (!AssetDatabase.IsValidFolder(cwjFolderPath))
                {
                    AssetDatabase.CreateFolder(parentDirectory, AutoSetupFolderName);
                }

                string targetFolderPath = Path.Combine(cwjFolderPath, fileName);
                if (!AssetDatabase.IsValidFolder(targetFolderPath))
                {
                    AssetDatabase.CreateFolder(cwjFolderPath, fileName);
                }

                string newAssetPath = Path.Combine(targetFolderPath, Path.GetFileName(fbxAssetPath));
                AssetDatabase.MoveAsset(fbxAssetPath, newAssetPath);
                if (prefabPath != null)
                {
                    AssetDatabase.MoveAsset(prefabPath, Path.Combine(targetFolderPath, Path.GetFileName(prefabPath)));
                }

                fbxAssetPath = newAssetPath; // 호출부에 반영
                targetFbxDirectory = Path.GetDirectoryName(fbxAssetPath);
            }
            prefabPath = Path.Combine(targetFbxDirectory, fileName+".prefab");
            return targetFbxDirectory;
        }

        static AnimationClip[] GetValidAnimationClipWithEvent(string fbxAssetPath, string fbxFolderPath, bool isAnimLoop, bool isAnimLoopPose, out string[] clipNames)
        {
            var clipNameList = new List<string>();
            List<AnimationClip> cloneAnimClips = new();
            var fbxAssets = AssetDatabase.LoadAllAssetsAtPath(fbxAssetPath);

            foreach (var srcClip in fbxAssets.OfType<AnimationClip>())
            {
                if (srcClip == null) continue;
                string clipName = srcClip.name;
                if (string.IsNullOrEmpty(clipName) || clipName.Contains(PreviewAnimNameTag))
                {
                    continue;
                }
                clipNameList.Add(clipName);

                // 저장할 경로 생성
                string cloneClipPath = System.IO.Path.Combine(fbxFolderPath, clipName + ".anim").Replace("\\", "/");

                // 같은 이름의 에셋이 이미 있는지 확인
                AnimationClip existingClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(cloneClipPath);
                var cloneClip = (existingClip) ? Instantiate(existingClip) : Instantiate(srcClip);
                cloneClip.name = clipName; // 원본 이름 유지

                var clipSettings = AnimationUtility.GetAnimationClipSettings(cloneClip);
                clipSettings.loopTime = isAnimLoop; // 애니메이션을 시간 기준으로 반복
                clipSettings.loopBlend = isAnimLoop && isAnimLoopPose; // 루프 시 포즈가 자연스럽게 연결되도록 설정
                AnimationUtility.SetAnimationClipSettings(cloneClip, clipSettings);

                // 기존 이벤트 가져오기
                List<AnimationEvent> existingEventList = AnimationUtility.GetAnimationEvents(cloneClip).ToList();

                if (!existingEventList.IsExists(ev => ev.functionName.Equals(AnimStartCallbackMethodName)))
                {
                    var animStartEvent = GetNewAnimationEvent(AnimStartCallbackMethodName, 0, clipName);
                    existingEventList.Add(animStartEvent);
                }
                if (!existingEventList.IsExists(ev => ev.functionName.Equals(AnimEndCallbackMethodName)))
                {
                    var animEndEvent = GetNewAnimationEvent(AnimEndCallbackMethodName, srcClip.length - 0.1f, clipName);
                    existingEventList.Add(animEndEvent);
                }

                // 수정된 이벤트 배열 설정
                AnimationUtility.SetAnimationEvents(cloneClip, existingEventList.ToArray());


                cloneAnimClips.Add(cloneClip);
                SaveAnimationClipAsAsset(fbxFolderPath, cloneClip, cloneClipPath, existingClip);
            }
            clipNames = clipNameList.ToArray();

            AnimationEvent GetNewAnimationEvent(string functionName, float time, string clipName)
            {
                // 이벤트 생성 및 추가
                return new AnimationEvent()
                {
                    functionName = functionName,
                    time = time,
                    stringParameter = clipName,
                };
            }

            void SaveAnimationClipAsAsset(string folderPath, AnimationClip saveClip, string clipFilePath, AnimationClip existsSrcClip)
            {
                if (existsSrcClip != null)
                {
                    // Backup 폴더 경로 생성
                    string backupFolderPath = Path.Combine(folderPath, "Backup").Replace("\\", "/");

                    if (!AssetDatabase.IsValidFolder(backupFolderPath))
                    {
                        AssetDatabase.CreateFolder(folderPath, "Backup");
                        Debug.Log($"Backup 폴더를 생성했습니다: {backupFolderPath}");
                    }

                    // 백업 파일 이름 생성
                    string timestamp = System.DateTime.Now.ToString("yyMMdd_HHmmss");
                    string backupName = $"backup_{timestamp}_{existsSrcClip.name}.anim";
                    string backupPath = Path.Combine(backupFolderPath, backupName).Replace("\\", "/");


                    // 기존 에셋을 백업 위치로 이동
                    string moveResult = AssetDatabase.MoveAsset(clipFilePath, backupPath);
                    if (string.IsNullOrEmpty(moveResult))
                    {
                        Debug.Log($"기존 애니메이션 클립을 백업했습니다: {backupPath}", existsSrcClip);
                    }
                    else
                    {
                        Debug.LogError($"애셋 이동에 실패했습니다: {moveResult}");
                        return;
                    }
                }

                // 새로운 에셋으로 저장
                AssetDatabase.CreateAsset(saveClip, clipFilePath);
                AssetDatabase.SaveAssets();
            }

            return cloneAnimClips.ToArray();
        }

        //Animator 에 덮어씌우는방식
        //static void AddAnimationEvent(Animator animator, AnimationClip clip, string functionName, float time)
        //{
        //    // 애니메이션 클립 복제
        //    AnimationClip clonedClip = Instantiate(clip);
        //    clonedClip.name = clip.name; // 원본 이름 유지

        //    // 이벤트 생성 및 추가
        //    AnimationEvent animEvent = new AnimationEvent();
        //    animEvent.functionName = functionName;
        //    animEvent.time = time;
        //    clonedClip.AddEvent(animEvent);

        //    // AnimatorOverrideController 생성
        //    AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        //    animator.runtimeAnimatorController = overrideController;

        //    // 클립 교체
        //    var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        //    overrideController.GetOverrides(overrides);
        //    for (int i = 0; i < overrides.Count; i++)
        //    {
        //        if (overrides[i].Key.name == clip.name)
        //        {
        //            overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, clonedClip);
        //            break;
        //        }
        //    }
        //    overrideController.ApplyOverrides(overrides);
        //}

        static void SetupAnimatorSingleLayer(AnimatorController animatorController, AnimationClip[] clips, string controllerPath,
            out string[] triggerNames)
        {
            AnimatorStateMachine stateMachine = animatorController.layers[0].stateMachine;

            HashSet<string> existingStateNames = new HashSet<string>();
            foreach (AnimationClip clip in clips)
            {
                string clipName = clip.name;

                if (!existingStateNames.Add(clipName))
                {
                    Debug.LogWarning($"중복된 애니메이션 클립 이름이 있습니다: {clipName}. 이름을 고유하게 변경해주세요.");
                    continue;
                }
                else
                {
                    animatorController.AddParameter(clipName, AnimatorControllerParameterType.Trigger);
                }
                AnimatorState state = stateMachine.AddState(clipName);
                state.motion = clip;

                CreateAnyStateTransition(stateMachine, state, clipName);
            }
            triggerNames = existingStateNames.ToArray();
            animatorController.AddParameter(TurnOffTrigger, AnimatorControllerParameterType.Trigger);

            AnimatorState emptyState = AddEmptyState(stateMachine, controllerPath);
            stateMachine.defaultState = emptyState;

            CreateAnyStateTransition(stateMachine, emptyState, TurnOffTrigger);
        }

        static void SetupAnimatorMultiLayer(AnimatorController animatorController, AnimationClip[] clips, string controllerPath, Transform rootTransform, string targetFbxDirectory
            , out string[] triggerNameArr)
        {
            // Base Layer 삭제. 새로 생성한 경우에만 진입한다는 가정하에.
            if (animatorController.layers.Length > 0 && animatorController.layers[0].name.Equals("Base Layer"))
                animatorController.RemoveLayer(0);

            Dictionary<string, List<AnimationClip>> layerToClips = new();
            HashSet<string> triggerNames = new();

            foreach (AnimationClip clip in clips)
            {
                string clipName = clip.name;

                if (clipName.Contains("_"))
                {
                    string[] parts = clipName.Split('_');
                    if (parts.Length == 2)
                    {
                        string triggerName = parts[0];
                        string layerName = parts[1];

                        if (triggerNames.Add(triggerName))
                            animatorController.AddParameter(triggerName, AnimatorControllerParameterType.Trigger);

                        if (!layerToClips.TryGetValue(layerName, out var animClipList))
                        {
                            animClipList = new List<AnimationClip>();
                            layerToClips.Add(layerName, animClipList);
                        }
                        animClipList.Add(clip);
                    }
                    else
                    {
                        Debug.LogWarning($"클립 이름 '{clipName}'이 올바르지 않습니다. '_'로 구분된 두 부분이 있어야 합니다.");
                    }
                }
                else
                {
                    Debug.LogWarning($"클립 이름 '{clipName}'에 '_'가 없습니다. 해당 클립은 처리되지 않습니다.");
                }
            }

            triggerNameArr = triggerNames.ToArray();
            animatorController.AddParameter(TurnOffTrigger, AnimatorControllerParameterType.Trigger);

            foreach (var layerEntry in layerToClips)
            {
                string layerName = layerEntry.Key;
                List<AnimationClip> layerClips = layerEntry.Value;

                AvatarMask avatarMask = new AvatarMask();
                avatarMask.name = layerName;

                // Avatar Mask 설정 (여기서는 모든 트랜스폼을 활성화)
                Transform[] transforms = rootTransform.GetComponentsInChildren<Transform>(true);
                avatarMask.transformCount = transforms.Length;
                for (int i = 0; i < transforms.Length; i++)
                {
                    string path = GetTransformPath(transforms[i], rootTransform);
                    avatarMask.SetTransformPath(i, path);
                    avatarMask.SetTransformActive(i, true);
                }

                // Avatar Mask 저장
                string avatarMaskPath = Path.Combine(targetFbxDirectory, layerName + ".mask");
                AssetDatabase.CreateAsset(avatarMask, avatarMaskPath);
                AssetDatabase.SaveAssets();

                // Avatar Mask 로드
                AvatarMask loadedAvatarMask = AssetDatabase.LoadAssetAtPath<AvatarMask>(avatarMaskPath);

                // 새로운 레이어 생성
                AnimatorControllerLayer layer = new AnimatorControllerLayer
                {
                    name = layerName,
                    defaultWeight = 1f,
                    blendingMode = AnimatorLayerBlendingMode.Additive,
                    avatarMask = loadedAvatarMask
                };

                // 레이어의 상태머신 생성
                AnimatorStateMachine layerStateMachine = new AnimatorStateMachine();
                AssetDatabase.AddObjectToAsset(layerStateMachine, controllerPath);
                layer.stateMachine = layerStateMachine;

                // 레이어 추가
                animatorController.AddLayer(layer);

                // EmptyState 생성 및 기본 상태로 설정
                AnimatorState emptyState = AddEmptyState(layerStateMachine, controllerPath);
                layerStateMachine.defaultState = emptyState;

                // "off" 트리거로 EmptyState로의 전이 추가
                CreateAnyStateTransition(layerStateMachine, emptyState, TurnOffTrigger);

                // 레이어의 각 애니메이션 클립 처리
                foreach (AnimationClip clip in layerClips)
                {
                    string clipName = clip.name;
                    string[] parts = clipName.Split('_');
                    string triggerName = parts[0];

                    AnimatorState state = layerStateMachine.AddState(clipName);
                    state.motion = clip;
                    CreateAnyStateTransition(layerStateMachine, state, triggerName);
                }
            }
        }

        static AnimatorState AddEmptyState(AnimatorStateMachine stateMachine, string controllerPath)
        {
            var emptyClip = new AnimationClip { name = AnimEmptyState};
            AssetDatabase.AddObjectToAsset(emptyClip, controllerPath);
            AssetDatabase.SaveAssets();

            var emptyState = stateMachine.AddState(AnimEmptyState);
            emptyState.motion = emptyClip;

            return emptyState;
        }

        static void CreateAnyStateTransition(AnimatorStateMachine stateMachine, AnimatorState targetState, string triggerName)
        {
            AnimatorStateTransition transition = stateMachine.AddAnyStateTransition(targetState);
            transition.AddCondition(AnimatorConditionMode.If, 0, triggerName);
            transition.hasExitTime = false;
            transition.duration = 0;
            transition.canTransitionToSelf = false;
        }

        static string GetTransformPath(Transform transform, Transform root)
        {
            if (transform == root)
                return "";
            string parentPath = GetTransformPath(transform.parent, root);
            if (string.IsNullOrEmpty(parentPath))
                return transform.name;
            else
                return parentPath + "/" + transform.name;
        }

        static int ApplyLoopSettingsToAnimationClips(string assetPath, bool isLoop, bool isLoopPose)
        {
            ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (modelImporter != null)
            {
                ModelImporterClipAnimation[] clipAnimations = modelImporter.clipAnimations;

                if (clipAnimations.Length == 0)
                {
                    clipAnimations = modelImporter.defaultClipAnimations;
                }
                bool needToReimport = false;

                for (int i = 0; i < clipAnimations.Length; i++)
                {
                    var modelImporterAnim = clipAnimations[i];
                    if (modelImporterAnim.loop != isLoop || modelImporterAnim.loopTime != isLoop || modelImporterAnim.loopPose != isLoopPose)
                    {
                        modelImporterAnim.loopTime = isLoop;
                        modelImporterAnim.loop = isLoop;
                        modelImporterAnim.loopPose = isLoop && isLoopPose;
                        needToReimport = true;
                    }
                }

                if (needToReimport)
                {
                    modelImporter.clipAnimations = clipAnimations;
                    AssetDatabase.WriteImportSettingsIfDirty(assetPath);
                    modelImporter.SaveAndReimport();
                }
                return clipAnimations.Length;
            }
            else
            {
                Debug.LogError("ModelImporter를 가져올 수 없습니다. " + assetPath);
            }
            return 0;
        }



        #endregion

        #region Interactor Auto Setup

        /// <summary>
        /// 애니메이션 실행버튼 달아주는 등의 상호작용 자동세팅
        /// </summary>
        [InvokeButton(isEmphasizeBtn:true)]
        protected void Start_InteractorSetup()
        {
            if (!_FbxInteractorSpawner.IsExists)
            {
                Debug.LogError(nameof(_FbxInteractorSpawner) + "가 씬에 존재하지 않습니다");
                return;
            }
            if(clipNamesCache == null)
            {
                Debug.LogError($"animationClip 이 없거나 최초 Setup이 잘못되었습니다.\n애니메이션이 있는 fbx의 경우 {nameof(FbxAutoSetupHelper)}을 지웠다가 다시 AddComponent 해보십시오");
                return;
            }
            try
            {
                //여기서 FbxInteractor 프리팹을 생성하고 FbxInteractor.SetupInteractor(fbxTrf);을 실행해줌
                if (TrySpawnFbxInteractor(out FbxInteractor fbxRoot))
                {
                    fbxRoot.SetupInteractor(transform, clipNamesCache);
                    //자동 셋업 시작
                    CWJ.AccessibleEditor.EditorSetDirty.SetObjectDirty(fbxRoot);
                    //AssetDatabase.SaveAssets();
                    //AssetDatabase.Refresh();
                }
                else
                {
                    if (fbxRoot)
                    {
                        //이미 작업되어잇음
                    }
                    else
                    {
                        //FbxRootAutoSetupManager.targetTopicIndex 문제
                    }
                }
                if (fbxRoot)
                {
                    EditorGUIUtility.PingObject(fbxRoot.transform);
                    Selection.activeObject = fbxRoot.transform;
                    if (TryGetThisFilePath(gameObject, prefabFilePathCache, out _, out string prefabFilePath))
                    {
                        SaveGameObjectAsPrefab(fbxRoot.gameObject, Path.GetDirectoryName(prefabFilePath), fbxRoot.gameObject.name);
                    }

                }
                else
                {
                    Selection.activeObject = _FbxInteractorSpawner.Instance;
                }
            }
            catch(Exception ex)
            {
                Debug.LogError($"Start_InteractorSetup 처리 중 오류 발생: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                bool flag = true;
                while (flag)
                {
                    flag = UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
                    // 맨 위로 올라갔을 경우 false 리턴
                }
            }

        }

        static void SaveGameObjectAsPrefab(GameObject targetGo, string folderPath, string fileName)
        {
            string prefabPath = Path.Combine(folderPath, fileName)+".prefab";
            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (existingPrefab != null)
            {
                // 백업 파일 경로 생성
                // Backup 폴더 경로 생성
                string backupFolderPath = Path.Combine(folderPath, "Backup").Replace("\\", "/");

                if (!AssetDatabase.IsValidFolder(backupFolderPath))
                {
                    AssetDatabase.CreateFolder(folderPath, "Backup");
                    Debug.Log($"Backup 폴더를 생성했습니다: {backupFolderPath}");
                }

                string timestamp = System.DateTime.Now.ToString("yyMMdd_HHmmss");
                string backupName = $"backup_{timestamp}_{fileName}.prefab";
                string backupPath = Path.Combine(backupFolderPath, backupName).Replace("\\", "/");

                // 기존 프리팹을 백업 경로로 복사
                AssetDatabase.CopyAsset(prefabPath, backupPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                // 백업 성공 여부 확인
                if (AssetDatabase.LoadAssetAtPath<GameObject>(backupPath) != null)
                {
                    Debug.Log($"기존 프리팹을 백업했습니다: {backupPath}");
                }
                else
                {
                    EditorUtility.DisplayDialog("오류", $"프리팹 백업에 실패했습니다: {backupPath}", "확인");
                    return;
                }
            }

            GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(targetGo, prefabPath, InteractionMode.UserAction);
            if (prefab == null)
            {
                EditorUtility.DisplayDialog("오류", "프리팹 생성에 실패했습니다.", "확인");
                return;
            }

            Debug.Log($"프리팹이 생성되었습니다: {prefabPath}");

            GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (prefabInstance == null)
            {
                EditorUtility.DisplayDialog("오류", "프리팹 인스턴스 생성에 실패했습니다.", "확인");
                return;
            }

            // 프리팹 인스턴스의 트랜스폼을 원본과 동일하게 설정
            prefabInstance.transform.SetParent(targetGo.transform.parent);
            prefabInstance.transform.localPosition = targetGo.transform.localPosition;
            prefabInstance.transform.localRotation = targetGo.transform.localRotation;
            prefabInstance.transform.localScale = targetGo.transform.localScale;

            // Undo 기능 등록 (되돌리기 가능하게)
            Undo.RegisterFullObjectHierarchyUndo(prefabInstance, "프리팹으로 교체");

            GameObject.DestroyImmediate(targetGo);

            Selection.activeGameObject = prefabInstance;
            EditorGUIUtility.PingObject(prefabInstance);
        }
        #endregion

        static bool TryGetThisFilePath(GameObject go, string prefabPathCache,out string fbxFilePath, out string prefabFilePath)
        {

            var prefabGo = PrefabUtility.GetCorrespondingObjectFromSource(go);
            if (prefabGo == null)
                prefabGo = go;

            fbxFilePath = AssetDatabase.GetAssetPath(prefabGo);
            prefabFilePath = null;

            if (string.IsNullOrEmpty(fbxFilePath))
            {
                if (!GetFbxPathFromPrefabPathCache(prefabPathCache, out fbxFilePath, out prefabFilePath))
                {
                    Debug.LogError("FBX 파일이 프로젝트 내에 있어야 합니다.");
                    return false;
                }
                else
                {
                    prefabFilePath = Path.GetDirectoryName(prefabFilePath) + go.name + ".prefab";
                }
            }
            string fbx_file_path = fbxFilePath.ToLower();
            if (!fbx_file_path.EndsWith(".fbx"))
            {
                if (fbx_file_path.EndsWith(".prefab"))
                {
                    prefabFilePath = fbxFilePath;
                    fbxFilePath = fbxFilePath.ReplaceEnd(".prefab", ".fbx");
                    if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(fbxFilePath)))
                    {
                        Debug.LogError("prefab파일과 fbx는 같은 폴더에 둬주세요");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("fbx나 prefab파일만 가능합니다");
                    return false;
                }
            }
            else
            {
                prefabFilePath = fbxFilePath.ReplaceEnd(".fbx", ".prefab");
            }
            return true;
        }
#endif
        protected virtual bool TrySpawnFbxInteractor(out FbxInteractor fbxInteractor)
        {
            return _FbxInteractorSpawner.Instance.TrySpawnFbxInteractor(transform, out fbxInteractor);
        }
    }
}
