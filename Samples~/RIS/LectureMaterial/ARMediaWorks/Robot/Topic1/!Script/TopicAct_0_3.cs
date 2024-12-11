using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;

using static UnityEngine.GraphicsBuffer;

namespace CWJ.YU.Mobility
{
    public class TopicAct_0_3 : TopicAction
    {
        public override int subTopicNum => 3;

        SyncablePdc prevXY => syncPdcPackages[0].srcPdc;
        SyncablePdc curXY => syncPdcPackages[0].destPdc;
        SyncablePdc curPpoint => syncPdcPackages[1].destPdc;

        bool isOpenDesc = false;
        Sequence lastSequence;
        void OnRotChanged(string rotStr)
        {
            if(!float.TryParse(rotStr, out float rotValue))
            {
                rotIpf.SetTextWithoutNotify(string.Empty);
                return;
            }
            float additionalRotValue = rotValue - backupAngle;
            backupAngle = rotValue;

            myScenario.lineConfigure.DisableDraw();
            syncPdcPackages.Do(s => s.ChangeColor());
            myScenario.lineConfigure.Draw();

            curXY.ChangeLocationBeforeAddChild((trf) =>
            {
                if (lastSequence != null)
                    lastSequence.Kill();
                //Quaternion targetRotation = trf.localRotation * Quaternion.AngleAxis(rotValue, -trf.forward);
                //trf.DOLocalRotateQuaternion(targetRotation, animTime) //LocalRotateQuaternion은 정확한 회전인 대신 절대값회전이라 0부터 시작함.
                //    .SetEase(Ease.InOutQuad);
                lastSequence = DOTween.Sequence()
                .SetAutoKill(true)
                .Append(trf.DOLocalRotate(new Vector3(0, 0, -additionalRotValue), animTime, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad))
                .OnComplete(() => lastSequence = null);
            }, animTime);


            //MainUIManager.Instance.SendLogTxt(rotValue.ToString() + "' 회전시켰습니다");

            if (!isOpenDesc)
            {
                isOpenDesc = true;
                DescriptonManager.Instance.OpenDescription(curPpoint.targetTrf, out _);
            }
            //myScenario.lineConfigure.
        }

        float backupAngle;

        public override void EnableAction()
        {
            float z = TransformUtil.NormalizeAngle(prevXY.targetTrf.localEulerAngles.z);
            backupAngle = Mathf.FloorToInt(z);
            rotIpf.SetTextWithoutNotify(backupAngle.ToString());
            isOpenDesc = false;
            rotIpf.onEndEdit.RemoveAllListeners();
            rotIpf.onEndEdit.AddListener(OnRotChanged);

            curPpoint.SetParent(curXY.targetTrf);
        }

        public override void DisableAction()
        {
            //curPpoint.targetTrf.SetParent(curXY.targetTrf.parent);
            rotIpf.onEndEdit.RemoveListener(OnRotChanged);
        }
    }
}
