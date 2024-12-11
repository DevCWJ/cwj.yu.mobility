
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace CWJ
{
    public class _FbxInteractorSpawner : CWJ.Singleton.SingletonBehaviour<_FbxInteractorSpawner>
    {
        public FbxInteractor prefab_fbxInteractor;
        public Vector2 fbxMaxRectSize;
        public bool TrySpawnFbxInteractor(Transform fbxModelingTrf, out FbxInteractor fbxInteractor)
        {
            fbxInteractor = fbxModelingTrf.GetComponentInParent<FbxInteractor>();
            if (fbxInteractor && fbxInteractor.IsInit) // already setup
            {
                return false;
            }

            var interactorParent = GetInteractorParent();
            if (!interactorParent)
            {
                fbxInteractor = null;
                return false;
            }
            prefab_fbxInteractor.gameObject.SetActive(true);
            fbxInteractor = Instantiate(prefab_fbxInteractor, interactorParent, true);
            prefab_fbxInteractor.gameObject.SetActive(false);
            return true;
        }
        protected virtual Transform GetInteractorParent()
        {
            return null;
        }

    }
}
