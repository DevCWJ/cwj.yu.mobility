using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UIElements;

namespace CWJ.YU.Mobility
{
    public class TopicAct_0_4 : TopicAction
    {
        public override int subTopicNum => 4;

        SyncablePdc curXY => syncPdcPackages[0].destPdc;
        SyncablePdc curPpoint => syncPdcPackages[1].destPdc;

        bool isOpenDesc = false;

        Vector3 _localXyPos;
        void OnXValueChanged(string input)
        {
            if (int.TryParse(input, out int resultNum))
                ChangePos(new Vector3(resultNum * 10, _localXyPos.y, 0));
        }
        //char OnXValueChanged(string input, int charIndex, char addedChar)
        //{
        //    if (UGUIUtil.TmpIpf_ValidateAndTryConvert(xPosIpf, int.TryParse, ref input, ref charIndex, ref addedChar, out int resultNum))
        //    {
        //        ChangePos(new Vector3(resultNum * 10, _localXyPos.y, 0));
        //    }

        //    return addedChar;
        //}
        void OnYValueChanged(string input)
        {
            if (int.TryParse(input, out int resultNum))
                ChangePos(new Vector3(_localXyPos.x, resultNum * 10, 0));
        }
        //char OnYValueChanged(string input, int charIndex, char addedChar)
        //{
        //    if (UGUIUtil.TmpIpf_ValidateAndTryConvert(yPosIpf, int.TryParse, ref input, ref charIndex, ref addedChar, out int resultNum))
        //    {
        //        ChangePos(new Vector3(_localXyPos.x, resultNum * 10, 0));
        //    }
        //    return addedChar;
        //}

        Sequence lastSequence;
        void ChangePos(Vector3 localPos)
        {
            if (_localXyPos == localPos)
            {
                return;
            }
            _localXyPos = localPos;

            myScenario.lineConfigure.DisableDraw();
            syncPdcPackages.Do(s => s.ChangeColor());
            myScenario.lineConfigure.Draw();

            curXY.ChangeLocationBeforeAddChild((trf) =>
            {
                if (lastSequence != null)
                    lastSequence.Kill();
                lastSequence = DOTween.Sequence()
                                    .SetAutoKill(true)
                                    .Append(trf.DOLocalMove(_localXyPos, animTime))
                                    .OnComplete(() => lastSequence = null);
            }, animTime);


            if (!isOpenDesc)
            {
                isOpenDesc = true;
                DescriptonManager.Instance.OpenDescription(curPpoint.targetTrf, out _);
            }
        }


        public override void EnableAction()
        {
            isOpenDesc = false;
            _localXyPos = curXY.targetTrf.localPosition;
            xPosIpf.SetTextWithoutNotify((_localXyPos.x * 0.1f).ToString());
            yPosIpf.SetTextWithoutNotify((_localXyPos.y * 0.1f).ToString());

            xPosIpf.onEndEdit.RemoveAllListeners();
            xPosIpf.onEndEdit.AddListener(OnXValueChanged);
            //xPosIpf.gameObject.SetActive(true);
            yPosIpf.onEndEdit.RemoveAllListeners();
            yPosIpf.onEndEdit.AddListener(OnYValueChanged);
            //yPosIpf.gameObject.SetActive(true);

            curPpoint.SetParent(curXY.targetTrf);
        }

        public override void DisableAction()
        {
            xPosIpf.onEndEdit.RemoveListener(OnXValueChanged);
            yPosIpf.onEndEdit.RemoveListener(OnYValueChanged);
            //xPosIpf.gameObject.SetActive(false);
            //yPosIpf.gameObject.SetActive(false);
        }
        //{
        //    curPpoint.targetTrf.SetParent(curXY.targetTrf.parent);

        //    curXY.targetTrf.gameObject.SetActive(false);
        //    curPpoint.targetTrf.gameObject.SetActive(false);
        //    rotInputField.gameObject.SetActive(false);

        //}
    }
}
