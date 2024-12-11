using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ.YU.Mobility
{
    public class YU_FbxAutoSetupHelper : FbxAutoSetupHelper
    {
        protected override bool TrySpawnFbxInteractor(out FbxInteractor fbxInteractor)
        {
            return YU_FbxInteractorSpawner.Instance.TrySpawnFbxInteractor(transform, out fbxInteractor);
        }
    }
}
