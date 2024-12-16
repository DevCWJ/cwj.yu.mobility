using System;
using System.Linq;
using System.Reflection;
using System.Text;
using CWJ.AccessibleEditor;
using CWJ.Singleton.OnlyUseNew;
using CWJ.Singleton.SwapSingleton;
using UnityEditor;
using UnityEngine;
using static CWJ.MonoBehaviourEventHelper;
using Object = UnityEngine.Object;

namespace CWJ.Singleton.Core
{
    using static FindUtil;

    public abstract class SingletonCoreAbstract<T> : SingletonCore
        where T : MonoBehaviour
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

        protected static readonly Type TargetType     = typeof(T);
        protected static readonly int  TargetTypeHash = TargetType.GetHashCode();
        protected static readonly string TargetTypeName = TargetType.Name;
        protected static readonly Type[] TargetInterfaces = typeof(T).GetInterfaces();

        private static readonly bool _IsDontAutoCreatedWhenNull = TargetInterfaces.IsExists(typeof(IDontAutoCreatedWhenNull));

        private static readonly string DontAutoCreateWhenNullErrorMsg =
            $"{TargetTypeName} 스크립트는 \n자동생성이 불가능한 Singleton입니다.\n('{nameof(IDontAutoCreatedWhenNull)}' 인터페이스가 상속됨)";
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


        private static bool IsDialogPopupEnabled
#if UNITY_EDITOR
                = true
#endif
            ;

        /// <summary>
        /// Call MySelf
        /// </summary>
        /// <param name="isPrintLogOrPopup"></param>
        public static void UpdateInstance(bool isPrintLogOrPopup = false)
        {
            __HasInstance_Marked = __INSTANCE && __INSTANCE.gameObject;
            if (__HasInstance_Marked)
            {
                return;
            }
#if UNITY_EDITOR
            IsDialogPopupEnabled = isPrintLogOrPopup;
#endif
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
                SingletonCoreAbstract<T> valueOfSingleton = null;
                bool hasNewInstance = false;
                if (value && value is SingletonCoreAbstract<T> valueConverted)
                {
                    hasNewInstance = true;
                    valueOfSingleton = valueConverted;
                    valueOfSingleton.OnBeforeInstanceAssigned(__INSTANCE, value);
                }

                if (__HasInstance_Marked && __INSTANCE && __INSTANCE is SingletonCoreAbstract<T> lastInstance)
                {
                    lastInstance._isInstance = false;
                    SingletonHelper.RemoveSingletonInstanceElem(__INSTANCE);
                }

                __INSTANCE = value;
                __HasInstance_Marked = hasNewInstance;

                if (hasNewInstance)
                {
                    valueOfSingleton._isInstance = true;
                    GameObjectName = valueOfSingleton.gameObject.name;
                    if (valueOfSingleton.isDontDestroyOnLoad && !IsBackendSingleton)
                        valueOfSingleton.SetDontDestroyOnLoad();
                    valueOfSingleton.OnAfterInstanceAssigned();
                    SingletonHelper.AddSingletonInstanceElem(value);
                }
            }
        }

#if UNITY_EDITOR
        private const string OptimizeTip =
            "싱글톤 최적화 하고싶으면 아래 두가지 잘지킬것\n1. Scene에서 싱글톤 게임오브젝트가 활성화상태로 저장되어있을것.(enabled는 상관없음)\n2. OnEnable() 이나 Start() 타이밍 전엔 Instance 호출 되지않게할것." +
            "\n위 두가지가 유지되면 Instance 검색비용 없도록 설계해놨음. (*내부에서 Instance 셀프호출하는 방식 아님. 불리기전까지 Instance는 null상태의 Lazy Singleton기반임)\n조건을 지키기 힘들다면 이 메세지 무시하고 호출해도 무관함. 검색비용 효율적으로 최적화해놨으니 걱정할필욘 없음.";
#endif

        /// <summary>
        /// <para><see cref="Instance"/> 호출 시 null이면 자동으로 생성해서 할당시킴</para>
        /// <para><see cref="Instance"/>를 호출하지않고 씬에서 <see cref="T"/> 존재유무만 알고싶으면 <see cref="IsExists"/>, Instance가 할당되었는지 유무는 <see cref="HasInstance"/> 사용할것</para>
        /// <para>[<see cref= "OptimizeTip"/>] 최적화 하고싶으면 아래 두가지 잘지킬것<br/>1. Scene에서 싱글톤 게임오브젝트가 활성화상태로 저장되어있을것.(enabled는 상관없음)<br/>2. OnEnable() 이나 Start() 타이밍 전엔 Instance 호출 되지않게할것.</para>
        /// <para>위 두가지가 유지되면 Instance 검색비용 없도록 설계해놨음. (*내부에서 Instance 셀프호출하는 방식 아님. 불리기전까지 Instance는 null상태의 Lazy Singleton기반임)</para>
        /// 조건을 지키기 힘들다면 이 메세지 무시하고 호출해도 무관함. 검색비용 효율적으로 최적화해놨으니 걱정할필욘 없음.
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
                        UpdateInstance(true);
                    }
                }

                return __INSTANCE;
            }
        }

        public static readonly bool IsBackendSingleton = SingletonHelper.BackendSingletonTypes.IsExists(t => t == TargetType);
        public static readonly bool IsIgnoreLogType    = IsBackendSingleton;


        protected static void CheckValidateForGetInstance()
        {
            if (IS_PLAYING)
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

        protected static T __ImmediatelyCreateForBackendIns()
        {
#if UNITY_EDITOR
            if (!IsBackendSingleton || _IsDontAutoCreatedWhenNull)
            {
                throw new NotSupportedException("맘대로 쓰지마셈");
            }
#endif
            if (HasInstance)
            {
                return Instance;
            }

            return _ImmediatelyCreateForNewIns();
        }

        private static T _ImmediatelyCreateForNewIns()
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
            if (!IsBackendSingleton)
            {
                newObjName += AutoCreatedNameTag;
                string log =
                    $"{(IS_PLAYING ? "플레이 중에" : "Editor 작업중에")} '{TargetTypeName}' 가 씬에 존재하지 않아서 생성시켰음";
#if UNITY_EDITOR
                if (!Editor_IsSilentlyCreateInstance)
                    typeof(SingletonCore).PrintLogWithClassName(log, isPreventStackTrace: false);
#else
                    UnityEngine.Debug.Log(log);
#endif
            }

            GameObject instanceObj = new(newObjName);
            instanceObj.SetActive(false);
            var newComp = instanceObj.AddComponent(TargetType) as T;
            if (newComp is SingletonCoreAbstract<T> singleton)
            {
                singleton._isAutoCreated = true;
                if (singleton.isDontDestroyOnLoad)
                    singleton.SetDontDestroyOnLoad(isNewCreateObj: true);
                singleton.AddSingletonCache();
            }

            instanceObj.SetActive(true);
            return newComp;
        }


        private static T GetInstance()
        {
            T[]  foundItems = Array.Empty<T>();
            bool isCached = IS_PLAYING && SingletonHelper.TryGetSingletonCache(TargetTypeHash, out foundItems);

            if (!IsDontPreCreatedInScene) //미리 만들어지는애는 아니므로 더 찾을것도 없음
            {
                if (foundItems.Length == 0) //캐싱된게 없다면 새로 검색함
                {
                    if (!SingletonHelper.IsFoundAllSingleton && IS_PLAYING)
                    {
                        SingletonHelper.IsFoundAllSingleton = true;

                        SingletonCore[] allSingleton = FindObjects_ForSingleton<SingletonCore>();
                        foreach (var item in allSingleton)
                        {
                            if (item && item is SingletonCore itemOfSingleton && !itemOfSingleton.isCachedOnHelper)
                            {
                                itemOfSingleton.AddSingletonCache();
                            }
                        }

                        SingletonHelper.TryGetSingletonCache(typeHeshCode: TargetTypeHash, out foundItems);
#if UNITY_EDITOR
                        typeof(SingletonCore).PrintLogWithClassName($"<{TargetTypeName}> 가 최적화 조건에 맞지않아 안내하는 TIP.\n{OptimizeTip}", isBigFont: false,
                                                                    obj: foundItems.LengthSafe() > 0 ? foundItems[0] : null,
                                                                    isPreventStackTrace: false);
#endif
                    }
                    else
                    {
                        foundItems = FindObjects_ForSingleton<T>();
                        foreach (var item in foundItems)
                        {
                            if (item && item is SingletonCoreAbstract<T> itemOfSingleton
                                     && !itemOfSingleton.isCachedOnHelper)
                                itemOfSingleton.AddSingletonCache();
                        }
                    }
                }
            }

            int foundLength = foundItems.Length;

            if (foundLength == 0)
            {
                return _ImmediatelyCreateForNewIns();
            }
            else if (foundLength == 1)
            {
                return foundItems[0];
            }
            else //findArray.Length > 1
            {
                var validTmp = foundItems.FirstOrDefault(e => e);
                if (validTmp)
                {
                    var singletonTmp = (validTmp as SingletonCoreAbstract<T>);

                    Action<T> afterAssigned = null;
                    if (!IsIgnoreLogType)
                    {
                        var nameListBuilder = new StringBuilder();

                        foreach (var item in foundItems)
                        {
                            if (item && item.gameObject)
                            {
                                nameListBuilder.Append(item.gameObject.scene.name).Append("/").Append(item.gameObject.name).Append("\n");
                            }
                        }

                        string nameList = nameListBuilder.ToString();
                        nameListBuilder.Clear();

#if UNITY_EDITOR
                        if (!Editor_IsSilentlyCreateInstance
                            && typeof(T).IsAssignableFromGenericType(typeof(SingletonBehaviourDontDestroy_Swap<>)))
#endif
                            afterAssigned = (ins) =>
                            {
                                string log = $"Singleton인 {TargetTypeName} 가 중복되게 존재함 "
                                             + $"\n(총{foundLength}개 이며 {(singletonTmp.isOnlyUseNew ? "새로 생성된" : "기존에 있던")} {ins.gameObject.scene.name}/{ins.gameObject.name}으로 Instance 할당됩니다"
                                             + $"\n전체 오브젝트 리스트:\n{nameList}"
                                             + $"\nInstance 외에는 모두 제거되었음.";
#if UNITY_EDITOR
                                typeof(SingletonCore).PrintLogWithClassName(log, isBigFont: false,
                                                                            isPreventStackTrace: false);
#else
                            Debug.Log(log);
#endif
                            };
                    }

                    Action<T> othersDestroyCallback = null;

                    if (singletonTmp.isOnlyUseNew
                        && (singletonTmp.isDontDestroyOnLoad == false
                            || (validTmp as SingletonBehaviourDontDestroy_OnlyUseNew<T>).isConfirmedInstance == false))
                    {
                        othersDestroyCallback = DestroySingletonObj;
                    }
                    else
                    {
                        othersDestroyCallback = (e) => DestroySingletonObj((e as SingletonCoreAbstract<T>).GetRootObj());
                    }


                    T returnIns;

                    if (singletonTmp.isOnlyUseNew)
                    {
                        returnIns = foundItems.FindMin((f) => f.GetInstanceID(), othersDestroyCallback);
                    }
                    else
                    {
                        if (!__INSTANCE)
                        {
                            returnIns = foundItems.FindMax((f) => f.GetInstanceID(), othersDestroyCallback);
                        }
                        else
                        {
                            returnIns = __INSTANCE;

                            for (int i = 0; i < foundLength; ++i)
                            {
                                if (returnIns != foundItems[i])
                                    DestroySingletonObj(foundItems[i].gameObject);
                            }
                        }
                    }

                    afterAssigned?.Invoke(returnIns);

                    return returnIns;
                }
            }

            return null;
        }

        public void Ping(GameObject pingObj = null)
        {
#if UNITY_EDITOR
            if (pingObj == null) pingObj = this.gameObject;
            AccessibleEditorUtil.PingObj(pingObj);
#endif
        }


#if UNITY_EDITOR

        [NonSerialized] bool editor_isChecked = false;
        protected void OnValidate()
        {
            if (!Application.isPlaying)
            {
                if (!editor_isChecked && !Editor_IsManagedByEditorScript)
                {
                    WillDestroyInterface(isOnValidate: true);
                    editor_isChecked = true;
                }

            }

            _OnValidate();
        }



        bool WillDestroyInterface(bool isOnValidate = false)
        {
            if (_IsDontPreCreatedInScene)
            {
                if(!Editor_IsManagedByEditorScript
                   && (IsIgnoreLogType
                       || DisplayDialogUtil.DisplayDialog<T>
                           (DontPreCreatedErrorMsg, "Destroy now", "Wait! I'll destroy myself", logObj: gameObject)))
                {
                    if (isOnValidate)
                        EditorCallback.AddWaitForFrameCallback(() => DestroySingletonObj(GetRootObj()));
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
            IsDialogPopupEnabled = false;

            SingletonCoreAbstract<T>[] singletonRootArray = FindObjects_ForSingleton<SingletonCoreAbstract<T>>();

            if (singletonRootArray.Length > 1)
            {
                Predicate<SingletonCoreAbstract<T>> findOther = (i) => i != this;
                Selection.objects = (from item in singletonRootArray
                                     where findOther(item)
                                     select item.gameObject).ToArray();
                EditorGUIUtility.PingObject(Selection.activeInstanceID);
                EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");

                message = "[ERROR] " + message + $" 추가 실패]\n\n현재 씬에 이미 존재하는 싱글톤입니다.\n컴포넌트 추가를 취소합니다\n(base:{nameof(SingletonCoreAbstract<T>)})";
                if (!Editor_IsSilentlyCreateInstance) DisplayDialogUtil.DisplayDialog<T>(message);
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
                if (!Editor_IsSilentlyCreateInstance)
                    DisplayDialogUtil.DisplayDialog<T>(message
                                                       + $"\n\n({nameof(isDontDestroyOnLoad)}:{isDontDestroyOnLoad}\n{nameof(isOnlyUseNew)}:{isOnlyUseNew}\n{nameof(isDontAutoCreatedWhenNull)}:{_IsDontAutoCreatedWhenNull}\n{nameof(isDontPreCreatedInScene)}:{_IsDontPreCreatedInScene})");
            }
            else
            {
                Debug.Log(message);
            }


            string prevGameObjName = gameObject.name;

            GameObject go = gameObject;

            _Reset();

            if (!this && go)
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

        public override void AddSingletonCache()
        {
            if (!isCachedOnHelper)
            {
                base.AddSingletonCache();
                SingletonHelper.AddSingletonCache(TargetTypeHash, this);
            }
        }

        protected override void RemoveSingletonCache()
        {
            if (isCachedOnHelper)
            {
                base.RemoveSingletonCache();
                SingletonHelper.RemoveSingletonCache(TargetTypeHash, this);
            }
        }

        protected virtual void Awake()
        {
            AddSingletonCache();
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

            RemoveSingletonCache();

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

        protected Object GetRootObj()
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

        protected static void DestroySingletonObj(Object obj)
        {
            if (!obj) return;
#if UNITY_EDITOR
            if (obj is Transform)
            {
                Debug.LogError("Transform지우려고 했음", obj);
                throw new TargetException("Transform can't be destroyed.");
            }
#endif
            if (!GetIsValidCreateObject()) return;

#if UNITY_EDITOR
            if (!IS_PLAYING)
            {
                //OnValidate에선 한프레임 대기후 실행해야함.
                DestroyImmediate(obj);
            }
            else
#endif
                Destroy(obj);
        }

        /// <summary>
        /// DontDestroyOnLoad 스크립트에서만 씀
        /// </summary>
        protected void SetDontDestroyOnLoad(bool isNewCreateObj = false)
        {
            if (!isDontDestroyOnLoad) return;
            if (IsBackendSingleton)
            {
                GameObjectUtil.SetDontDestroySafety(gameObject);
                return;
            }

            if (!isNewCreateObj && gameObject.IsDontDestroyOnLoad())
            {
                return;
            }

            var rootObj = GameObjectUtil.SetDontDestroySafety(gameObject);
            if (rootObj && rootObj != gameObject)
            {
                typeof(SingletonCore).PrintLogWithClassName($"{TargetTypeName}때문에 '{transform.root.name}'(오브젝트이름)에 DontDestroyOnLoad를 실행했습니다", LogType.Log,
                                                            false, obj: gameObject, isPreventStackTrace: true);
            }
        }

        protected void HideGameObject()
        {
            bool isDebugBuild =
#if CWJ_DEVELOPMENT_BUILD
            true;
#else
            false;
#endif

            HideFlags addHideFlag = isDebugBuild ? HideFlags.NotEditable : HideFlags.HideInHierarchy;

            if (!gameObject.hideFlags.Flags_Contains(addHideFlag))
            {
                gameObject.hideFlags |= addHideFlag;
            }
        }
    }
}
