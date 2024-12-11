using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ.YU.Mobility
{
    public class TopicAct_0_6 : TopicAction
    {
        public override int subTopicNum => 6;

        public override void DisableAction()
        {
        }

        SyncablePdc prevPpoint => syncPdcPackages[1].srcPdc;
        public override void EnableAction()
        {
            syncPdcPackages[0].srcPdc.SetChangeColor();
            syncPdcPackages[1].srcPdc.SetChangeColor();

            StartCoroutine(IE_OpenDesc());
        }

        IEnumerator IE_OpenDesc()
        {
            yield return null;

            DescriptonManager.Instance.CloseDescription();
            yield return new WaitForSeconds(1);

            DescriptonManager.Instance.OpenDescription(prevPpoint.targetTrf, out _);
        }
    }
}
