namespace CWJ.Singleton.Core
{
    public class SingletonCore : MonoBehaviourCWJ_LazyOnEnable
    {
        public virtual bool isDontDestroyOnLoad { get; }
        public virtual bool isOnlyUseNew { get; }
        public virtual bool isInstance { get; }
        public virtual bool isAutoCreated { get; }
        public virtual bool isDontAutoCreatedWhenNull { get; }
        public virtual bool isDontPreCreatedInScene { get; }
        public virtual bool isDontGetInstanceInEditorMode { get; }
        public virtual bool isDontSaveInBuild { get; }

        #region Use these methods instead of original unity's magic-methods
        //I had to break the naming convention to declare it with a similar name. ('_' underscore)

        /// <summary>
        /// Use instead of Reset 
        /// <para>Only UNITY_EDITOR</para>
        /// <para>CWJ.SingletonCore</para>
        /// </summary>
        protected virtual void _Reset() { }

        /// <summary>
        /// Use instead of OnValidate 
        /// <para>Only UNITY_EDITOR</para>
        /// <para>CWJ.SingletonCore</para>
        /// </summary>
        protected virtual void _OnValidate() { }

        /// <summary>
        /// Use instead of Awake 
        /// <para>CWJ.SingletonCore</para>
        /// </summary>
        protected virtual void _Awake() { }

        protected override void _OnEnable() { }

        /// <summary>
        /// Use instead of OnDisable 
        /// <para>CWJ.SingletonCore</para>
        /// </summary>
        protected virtual void _OnDisable() { }
        
        protected override void _Start() { }

        /// <summary>
        /// Use instead of OnDestroy 
        /// <para>CWJ.SingletonCore</para>
        /// </summary>
        protected override void _OnDestroy(){}

        /// <summary>
        /// Use instead of OnApplicationQuit 
        /// <para>CWJ.SingletonCore</para>
        /// </summary>
        protected override void _OnApplicationQuit(){}

        protected override void OnDispose() { }
        #endregion Use these methods instead of original unity's magic-methods
    }
}

namespace CWJ.Singleton
{
    /// <summary>
    /// Classes that override this interface are not automatically created object.
    /// <para/>So, you should pre-created component it in the scene.
    /// </summary>
    public interface IDontAutoCreatedWhenNull { }

    /// <summary>
    /// Classes that override this interface can not pre-created, not save in scene (both editor and build)
    /// <para/>Then, you can only use it as an auto-created instance.
    /// </summary>
    public interface IDontPrecreatedInScene { }

    /// <summary>
    /// Classes that override this interface can not '.Instance'(get instance) in editor mode.
    /// <para/>It'll prevent call Instance in editor method.
    /// </summary>
    public interface IDontGetInstanceInEditorMode { }

    /// <summary>
    /// Classes that override this interface cannot be saved in build.
    /// <para/>It'll not be include when building a scene.
    /// </summary>
    public interface IDontSaveInBuild { }
}
