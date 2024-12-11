using UnityEngine;
using CWJ.Singleton;
namespace CWJ
{
    public class SceneChangeBeforEvent : SingletonBehaviour<SceneChangeBeforEvent>
    {
        public UnityEngine.Events.UnityEvent onSceneChangeBefore = new UnityEngine.Events.UnityEvent();

        protected override void _OnDestroy()
        {
            onSceneChangeBefore.Invoke();
        }
    }
}
