using System;
using System.Linq;

using UnityEngine;

using CWJ.Singleton.OnlyUseNew;
using static CWJ.MonoBehaviourEventHelper;

namespace CWJ.Singleton.Core
{
    using static CWJ.FindUtil;

    public abstract class SingletonCoreAbstract<T> : SingletonCore where T : MonoBehaviour
    {

        #region Use these methods instead of original unity's magic-methods //I had to break the naming convention to declare it with a similar name. ('_')

        protected abstract override void _Reset();
        protected abstract override void _OnValidate();
        protected abstract override void _Awake();
        protected abstract override void _OnEnable();
        /// <summary>
        /// 앱종료시에는 불리지않게 처리했음.
        /// </summary>
        protected abstract override void _OnDisable();
        protected abstract override void _Start();
        protected abstract override void OnDispose();

        /// <summary>
        /// 앱 종료시에도 불릴수있게 해놨음
        /// </summary>
        protected abstract override void _OnDestroy();
        protected abstract override void _OnApplicationQuit();
        #endregion Use these methods instead of original unity's magic-methods //I had to break the naming convention to declare it with a similar name. ('_')

        /// <summary>
        /// When just before assigning an Instance
        /// 무한루프 돌 가능성 있음. 매우 위험함. 이 안에서는 Instance가 호출되는일이 없어야함
        /// <para>CWJ.SingletonCore</para>
        /// </summary>
        protected virtual void OnBeforeInstanceAssigned(T prevInstance , T newInstance) { }

        /// <summary>
        /// When after assigning an Instance
        /// <para>CWJ.SingletonCore</para>
        /// </summary>
        protected virtual void OnAfterInstanceAssigned() { }

        #region SingletonBehav Interface
        protected static readonly System.Type TargetType = typeof(T);
        protected static readonly string TargetTypeName = TargetType.Name;
        protected static readonly Type[] TargetInterfaces = typeof(T).GetInterfaces();

        private static readonly bool _IsDontAutoCreatedWhenNull = TargetInterfaces.IsExists(typeof(IDontAutoCreatedWhenNull));
        private static readonly string DontAutoCreateWhenNullErrorMsg = $"{TargetTypeName} 스크립트는 \n자동생성이 불가능한 Singleton입니다.\n('{nameof(Singleton.IDontAutoCreatedWhenNull)}' 인터페이스가 상속됨)";
        public static bool IsDontAutoCreatedWhenNull => _IsDontAutoCreatedWhenNull;
        public override sealed bool isDontAutoCreatedWhenNull => _IsDontAutoCreatedWhenNull;


        private static readonly bool _IsDontPreCreatedInScene = TargetInterfaces.IsExists(typeof(IDontPrecreatedInScene));
        private static readonly string DontPreCreatedErrorMsg = $"{TargetTypeName} 스크립트는 \n씬에 미리 만들어둘 수 없는 Singleton입니다.\n실행중에만 호출 및 생성이가능합니다\n('{nameof(IDontPrecreatedInScene)}' 인터페이스가 상속됨)";
        public static bool IsDontPreCreatedInScene => _IsDontPreCreatedInScene;
        public override sealed bool isDontPreCreatedInScene => _IsDontPreCreatedInScene;

        private static readonly bool _IsDontGetInstanceInEditorMode = TargetInterfaces.IsExists(typeof(IDontGetInstanceInEditorMode));
        public static bool IsDontGetInstanceInEditorMode => _IsDontGetInstanceInEditorMode;
        public override sealed bool isDontGetInstanceInEditorMode => _IsDontGetInstanceInEditorMode;

        private static readonly bool _IsDontSaveInBuild = TargetInterfaces.IsExists(typeof(IDontSaveInBuild));
        public static bool IsDontSaveInBuild => _IsDontSaveInBuild;
        public override sealed bool isDontSaveInBuild => _IsDontSaveInBuild;

        #endregion

        /// <summary>
        /// DontDestroyOnLoad Singleton
        /// </summary>
        public override abstract bool isDontDestroyOnLoad { get; }

        /// <summary>
        /// OnlyUseNew Singleton
        /// </summary>
        public override abstract bool isOnlyUseNew { get; }


        private static bool IsDialogPopupEnabled = true;

        /// <summary>
        /// Call MySelf
        /// </summary>
        /// <param name="isPrintLogOrPopup"></param>
        public static void UpdateInstance(bool isPrintLogOrPopup = true)
        {
            __HasInstance_Marked = __INSTANCE && __INSTANCE.gameObject;
            if (__HasInstance_Marked)
            {
                return;
            }

            IsDialogPopupEnabled = isPrintLogOrPopup;
            _Instance = GetInstance();
        }

        protected void UpdateInstanceForcibly()
        {
            if (!isOnlyUseNew) return;
            IsDialogPopupEnabled = false;
            _Instance = GetInstance();
        }

        public static bool HasInstance => __HasInstance_Marked && __INSTANCE;

        public static bool IsExists => (HasInstance || FindObjects_ForSingleton<T>().Length > 0);

        private bool _isInstance = false;
        public sealed override bool isInstance => _isInstance;
        public static string GameObjectName = null;

        private bool _isAutoCreated = false;
        public sealed override bool isAutoCreated => _isAutoCreated;
        private const string AutoCreatedNameTag = " (Created)";

        private static readonly object lockObj = new object();

        /// <summary>
        /// _instance는 SingletonCoreGeneric 외에는 사용금지
        /// </summary>
        private static T __INSTANCE;

        protected static bool __HasInstance_Marked { get; private set; } = false;
        public static T __UnsafeFastIns => __HasInstance_Marked ? __INSTANCE : (_Instance = GetInstance());
        protected static T _Instance
        {
            get => __INSTANCE;
            set
            {
                var valueOfCore = value ? (value as SingletonCoreAbstract<T>) : null;
                bool hasNewInstance = false;
                if (valueOfCore)
                {
                    hasNewInstance = true;
                    valueOfCore.OnBeforeInstanceAssigned(__INSTANCE, value);
                }

                if (__HasInstance_Marked && __INSTANCE)
                {
                    SingletonCoreAbstract<T> lastInstance;
                    if (lastInstance = (__INSTANCE as SingletonCoreAbstract<T>))
                    {
                        lastInstance._isInstance = false;
                        SingletonHelper.RemoveSingletonInstanceElem(__INSTANCE);
                    }
                }

                __INSTANCE = value;
                __HasInstance_Marked = hasNewInstance;

                if (hasNewInstance)
                {
                    valueOfCore._isInstance = true;
                    GameObjectName = valueOfCore.gameObject.name;
                    if (valueOfCore.isDontDestroyOnLoad)
                        valueOfCore.SetDontDestroyOnLoad();
                    valueOfCore.OnAfterInstanceAssigned();
                    SingletonHelper.AddSingletonInstanceElem(value);
                }
            }
        }

        /// <summary>
        /// <para>Instance 호출 시 null이면 자동으로 생성해서 할당시킴</para>
        /// <para>Instance를 호출하지않고 씬에서 <see cref="T"/> 존재유무만 알고싶으면 <see cref="IsExists"/> 사용할것</para>
        /// <para>Instance를 호출하지않고 Instance 할당여부 확인은 <see cref="HasInstance"/> </para>
        /// </summary>
        public static T Instance
        {
            get
            {
                CheckValidateForGetInstance();

                if (!HasInstance)
                {
                    lock (lockObj)
                    {
                        UpdateInstance();
                    }
                }

                return __INSTANCE;
            }
        }

        public static readonly System.Type[] IgnoreLogTypes = new Type[]
        {
            typeof(SingletonHelper),
            typeof(MonoBehaviourEventHelper),
            typeof(AccessibleEditor.DebugSetting.UnityDevConsoleVisible)
        };

        public static readonly bool IsIgnoreLogType = IgnoreLogTypes.IsExists(TargetType);
        protected static void CheckValidateForGetInstance()
        {
            if (MonoBehaviourEventHelper.IS_PLAYING)
            {
                if (IS_QUIT)
                {
                    throw new ObjectDisposedException(objectName: GameObjectName, message: $"{TargetTypeName} was called when the application is quitted or Scene is disabled or object is destroyed\nTo avoid error, add 'if ({nameof(SingletonHelper)}.{nameof(IS_QUIT)}) return;' codes in first line");
                    //^이게 불렸다면 OnDisable이나 OnDestroy내에서 Instance 가 불리는 곳이 있는거임 if(!Application.isPlaying) return; 처리해주기
                }
            }
#if UNITY_EDITOR
            else
            {
                if (_IsDontGetInstanceInEditorMode && !Editor_IsManagedByEditorScript)
                    throw new ObjectDisposedException(objectName: GameObjectName, message: $"{TargetTypeName} was called when editor mode\nTo avoid error, cancel the instance call or remove '{nameof(IDontGetInstanceInEditorMode)}' interface");
                //^이게 불렸다면 실행중이 아닐때 Instance를 호출했다는거임 (해결방법은 Editor모드에서 Instance 호출하는 코드를 제거하거나 ICannotCreateInEditorMode 인터페이스를 제거하거나)
            }
#endif
        }

        private static T GetInstance()
        {
            T[] findArray = FindObjects_ForSingleton<T>();

            if (findArray.Length == 0)
            {
                if (_IsDontAutoCreatedWhenNull
#if UNITY_EDITOR
                    && !Editor_IsManagedByEditorScript
#endif
                    )
                {
                    string errorLog = DontAutoCreateWhenNullErrorMsg;
#if UNITY_EDITOR
                    if (!Editor_IsSilentlyCreateInstance)
                        typeof(SingletonCore).PrintLogWithClassName(errorLog, LogType.Error, isBigFont: false, isPreventStackTrace: false);
#else
                    Debug.LogError(errorLog);
#endif
                    return null;
                }

                if (!GetIsValidCreateObject())
                {
                    return null;
                }

                string newObjName = TargetTypeName;
                if (!IsIgnoreLogType)
                {
                    newObjName += AutoCreatedNameTag;
                    string log =
                        $"{(IS_PLAYING ? "플레이 중에" : "Editor 작업중에")} '{TargetTypeName}' 가 씬에 존재하지 않아서 생성시켰음";
#if UNITY_EDITOR
                    if (!Editor_IsSilentlyCreateInstance)
                        typeof(SingletonCore).PrintLogWithClassName(log, isPreventStackTrace: false);
#else
                    Debug.Log(log);
#endif
                }
                GameObject instanceObj = new(newObjName, TargetType);
                var newIns = instanceObj.GetComponent<T>();
                var singleton = (newIns as SingletonCoreAbstract<T>);
                singleton._isAutoCreated = true;
                return newIns;
            }
            else if (findArray.Length == 1)
            {
                return findArray[0];
            }
            else //findArray.Length > 1
            {
                T returnIns = null;
                SingletonCoreAbstract<T> tmpElem = (findArray[0] as SingletonCoreAbstract<T>);

                System.Action<T> afterAssigned = null;
                if (!IsIgnoreLogType)
                {
                    string nameList = string.Join("\n", System.Array.ConvertAll(findArray, (f) => f.gameObject.scene.name + "/" + f.gameObject.name));
#if UNITY_EDITOR
                    if (!Editor_IsSilentlyCreateInstance && TypeUtil.IsAssignableFromGenericType(typeof(T), typeof(SwapSingleton.SingletonBehaviourDontDestroy_Swap<>)))
#endif
                        afterAssigned = (ins) =>
                        {
                            string log = $"Singleton인 {TargetTypeName} 가 중복되게 존재함 " +
                                         $"\n(총{findArray.Length}개 이며 {(tmpElem.isOnlyUseNew ? "새로 생성된" : "기존에 있던")} {ins.gameObject.scene.name}/{ins.gameObject.name}으로 Instance 할당됩니다" +
                                         $"\n전체 오브젝트 리스트:\n{nameList}" +
                                         $"\nInstance 외에는 모두 제거되었음.";
                            #if UNITY_EDITOR
                            typeof(SingletonCore).PrintLogWithClassName(log, isBigFont: false,
                                isPreventStackTrace: false);
                            #else
                            Debug.Log(log);
                            #endif
                        };
                }

                System.Action<T> othersDestroyCallback = null;

                if (tmpElem.isOnlyUseNew &&
                    (!tmpElem.isDontDestroyOnLoad || !(findArray[0] as SingletonBehaviourDontDestroy_OnlyUseNew<T>).isConfirmedInstance))
                {
                    othersDestroyCallback = DestroySingletonObj;
                }
                else
                {
                    othersDestroyCallback = (e) => DestroySingletonObj((e as SingletonCoreAbstract<T>).GetRootObj());
                }

                if (tmpElem.isOnlyUseNew)
                {
                    returnIns = findArray.Min((f) => f.GetInstanceID(), othersDestroyCallback);
                }
                else
                {
                    if (!__INSTANCE)
                    {
                        returnIns = findArray.Max((f) => f.GetInstanceID(), othersDestroyCallback);
                    }
                    else
                    {
                        returnIns = __INSTANCE;

                        for (int i = 0; i < findArray.Length; ++i)
                        {
                            if (returnIns != findArray[i])
                                DestroySingletonObj(findArray[i].gameObject);
                        }
                    }
                }

                afterAssigned?.Invoke(returnIns);

                return returnIns;
            }
        }

        public void Ping(GameObject pingObj = null)
        {
#if UNITY_EDITOR
            if (pingObj == null) pingObj = this.gameObject;
            AccessibleEditor.AccessibleEditorUtil.PingObj(pingObj);
#endif
        }


#if UNITY_EDITOR
        [NonSerialized] bool editor_isChecked = false;
        protected void OnValidate()
        {
            if (!editor_isChecked
                && !Application.isPlaying && !Editor_IsManagedByEditorScript)
            {
                WillDestroyInterface(isOnValidate: true);
                editor_isChecked = true;
            }

            _OnValidate();
        }


        bool WillDestroyInterface(bool isOnValidate = false)
        {
            if (_IsDontPreCreatedInScene)
            {
                if(!Editor_IsManagedByEditorScript
                    && (IsIgnoreLogType || CWJ.AccessibleEditor.DisplayDialogUtil.DisplayDialog<T>
                    (DontPreCreatedErrorMsg, ok: "Destroy now", cancel: "Wait! I'll destroy myself", logObj: gameObject)))
                {
                    if (isOnValidate)
                        CWJ.AccessibleEditor.EditorCallback.AddWaitForFrameCallback(() => DestroySingletonObj(GetRootObj()));
                    else
                    {
                        Debug.LogError(DontPreCreatedErrorMsg);
                        DestroyImmediate(this);
                    }
                    return true;
                }
            }
            else if (_IsDontSaveInBuild)
            {
                SetHideFlag(HideFlags.DontSaveInBuild);
            }
            return false;
        }

        protected void Reset() //에디터에서 && Component추가 시 실행됨
        {
            if (WillDestroyInterface())
            {
                return;
            }

            string message = $"싱글톤 {TargetTypeName}\n 컴포넌트 추가 시도 감지. [결과 :";
            bool isPopupEnabled = IsDialogPopupEnabled;
            IsDialogPopupEnabled = true;

            SingletonCoreAbstract<T>[] singletonRootArray = FindObjects_ForSingleton<SingletonCoreAbstract<T>>();

            if (singletonRootArray.Length > 1)
            {
                Predicate<SingletonCoreAbstract<T>> findOther = (i) => i != this;
                UnityEditor.Selection.objects = (from item in singletonRootArray
                                                 where findOther(item)
                                                 select item.gameObject).ToArray();
                UnityEditor.EditorGUIUtility.PingObject(UnityEditor.Selection.activeInstanceID);
                UnityEditor.EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");

                message = "[ERROR] " + message + $" 추가 실패]\n\n현재 씬에 이미 존재하는 싱글톤입니다.\n컴포넌트 추가를 취소합니다\n(base:{nameof(SingletonCoreAbstract<T>)})";
                if (!Editor_IsSilentlyCreateInstance) CWJ.AccessibleEditor.DisplayDialogUtil.DisplayDialog<T>(message);
                DestroyImmediate(this);
                return;
            }

            message += " 추가 성공]\n\n(에디터에서 컴포넌트를 AddComponent 했거나, 런타임중이 아닐때 컴포넌트가 없는데 Instance를 호출한 경우 나오는 메세지입니다)";

            //if(GetComponents<Component>().Length <= 2 && transform.childCount==0)
            //{
            //    gameObject.name = TargetTypeName;
            //}

            if (isPopupEnabled)
            {
                if (!Editor_IsSilentlyCreateInstance) CWJ.AccessibleEditor.DisplayDialogUtil.DisplayDialog<T>(
                    message + $"\n\n({nameof(isDontDestroyOnLoad)}:{isDontDestroyOnLoad}\n{nameof(isOnlyUseNew)}:{isOnlyUseNew}\n{nameof(isDontAutoCreatedWhenNull)}:{_IsDontAutoCreatedWhenNull}\n{nameof(isDontPreCreatedInScene)}:{_IsDontPreCreatedInScene})");
            }


            string prevGameObjName = gameObject.name;

            GameObject go = gameObject;

            _Reset();

            if (!this && go )
            {
                go.name = prevGameObjName;
            }
        }

        private void SetHideFlag(HideFlags setHideFlag)
        {
            if (this.hideFlags.HasFlag(setHideFlag))
            {
                return;
            }
            //if (gameObject.GetComponents<Component>().Length <= 2 && transform.childCount == 0)
            //{
            //    gameObject.hideFlags |= setHideFlag;
            //}
            this.hideFlags |= setHideFlag;
        }
#endif

        protected virtual void Awake()
        {
            SingletonHelper.AddSingletonAllElem(this);
        }

        protected void OnDisable()
        {
            if (IS_QUIT) return;
            _OnDisable();
        }

        protected override sealed void OnDestroy()
        {
            base.OnDestroy();
            if (IS_QUIT) return;

            SingletonHelper.RemoveSingletonAllElem(this);

            if (HasInstance && __INSTANCE == this)
            {
                _Instance = null;
            }
        }

        protected override sealed void OnApplicationQuit()
        {
            GameObjectName ??=
#if UNITY_EDITOR || CWJ_DEVELOPMENT_BUILD
            gameObject ? gameObject.name :
#endif
            string.Empty;

            base.OnApplicationQuit();
        }

        /// <summary>
        /// 새 instance생성을 막아야하는경우에 true를 return
        /// </summary>
        /// <returns></returns>
        protected bool IsPreventNewInstance()
        {
            if (isOnlyUseNew)
            {
                return false;
            }

            if (HasInstance && __INSTANCE != this)
            {
                DestroySingletonObj(GetRootObj());
                return true;
            }
            else
            {
                return false;
            }
        }

        protected UnityEngine.Object GetRootObj()
        {
            if (_isAutoCreated)
                return gameObject;
            else
            {
                if (ComponentUtil.IsGoHasOnlyThisCompWithRequireComps(this))
                    return gameObject;
                else
                    return this;
            }
        }

        protected static void DestroySingletonObj(UnityEngine.Object obj)
        {
            if (!obj || obj is Transform) return;
            if (!GetIsValidCreateObject()) return;
            if (IS_PLAYING)
            {
                Destroy(obj);
            }
            else
            {
                //OnValidate에선 한프레임 대기후 실행해야함.
                DestroyImmediate(obj);
            }
        }

        /// <summary>
        /// DontDestroyOnLoad 스크립트에서만 씀
        /// </summary>
        protected void SetDontDestroyOnLoad()
        {
            if (!isDontDestroyOnLoad || !GetIsPlayingBeforeQuit()) return;
            if (!TargetTypeName.Equals(nameof(SingletonHelper)) && gameObject.IsDontDestroyOnLoad()) return;
            var rootObj = transform.root.gameObject;
#if UNITY_EDITOR
            bool isHidden = UnityEditor.SceneVisibilityManager.instance.IsHidden(rootObj, false);
            if (isHidden)
                UnityEditor.SceneVisibilityManager.instance.Show(rootObj, false);
#endif
            DontDestroyOnLoad(rootObj);
            if (rootObj.transform != transform)
            {
                typeof(SingletonCore).PrintLogWithClassName($"{TargetTypeName}때문에 '{rootObj.name}'(오브젝트이름)에 DontDestroyOnLoad를 실행했습니다", LogType.Log, false, obj: gameObject, isPreventStackTrace: true);
            }
#if UNITY_EDITOR
            if (isHidden)
                UnityEditor.SceneVisibilityManager.instance.Hide(rootObj, false);
#endif
        }

        protected void HideGameObject()
        {
            bool isDebugBuild =
#if CWJ_DEVELOPMENT_BUILD
            true;
#else
            false;
#endif

            var addHideFlag = isDebugBuild ? UnityEngine.HideFlags.NotEditable : UnityEngine.HideFlags.HideInHierarchy;

            if (!gameObject.hideFlags.Flags_Contains(addHideFlag))
            {
                gameObject.hideFlags |= addHideFlag;
            }
        }
    }
}
