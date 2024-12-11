using UnityEngine;

namespace CWJ
{

    /// <summary>
    ///<br/>  using (fileChangedChecker = gameObject.AddComponent(FileChangedChecker()))
    ///<br/>  {
    ///<br/>      fileChangedChecker.fileChangedEvent.AddListener_New(UpdateCommandByTxt);
    ///<br/>      fileChangedChecker.InitSystemWatcher(Path.GetDirectoryName(commandTxtPath), Path.GetFileName(commandTxtPath));
    ///<br/>  }
    ///<para>이런게 가능해짐</para>
    /// </summary>
    public abstract class DisposableMonoBehaviour : MonoBehaviour, System.IDisposable
    {
        protected abstract void OnDispose();

        /// <summary>
        /// Dispose가 될때 (using 끝에) Destroy되길 원하면 true
        /// <br/> default : true
        /// </summary>
        protected virtual bool isAutoDestroy => true;
        /// <summary>
        /// OnDispose가 불렸고, 삭제처리가 되었는지? 중복실행방지용.
        /// </summary>
        protected bool isDesposed { get; private set; } = false;
        /// <summary>
        /// 오브젝트 Destroy 혹은 앱종료가 되는걸 미리알때 불러주면 빠름.
        /// <br/>call할땐 얘로 해야 중복실행 방지됨.
        /// </summary>
        public void Dispose()
        {
            if (isDesposed)
            {
                return;
            }
            isDesposed = true;
            OnDispose();
            if (isAutoDestroy && !isDestroyed)
            {
                if (!MonoBehaviourEventHelper.IS_QUIT)
                    Destroy(this);
                else
                    _OnDestroy();
            }
        }

        protected bool isDestroyed { get; private set; } = false;
        protected virtual void _OnDestroy() { }
        protected virtual void OnDestroy()
        {
            if (isDestroyed)
            {
                return;
            }
            isDestroyed = true;
            _OnDestroy();
            Dispose();
        }
        protected virtual void _OnApplicationQuit() { }
        protected virtual void OnApplicationQuit()
        {
            _OnApplicationQuit();
            Dispose();
        }
    }
}
