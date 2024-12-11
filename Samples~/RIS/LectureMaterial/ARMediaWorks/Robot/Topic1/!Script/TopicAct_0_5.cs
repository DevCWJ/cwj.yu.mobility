using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

namespace CWJ.YU.Mobility
{
    public class TopicAct_0_5 : TopicAction
    {
        public override int subTopicNum => 5;

        SyncablePdc prevXY => syncPdcPackages[0].srcPdc;
        SyncablePdc curXY => syncPdcPackages[0].destPdc;
        SyncablePdc curPpoint => syncPdcPackages[1].destPdc;

        bool isOpenDesc = false;

        string lastRotStr = null;


        Sequence lastRotSequence;
        void OnRotChanged(string rotStr)
        {
            if (!float.TryParse(rotStr, out float rotValue))
            {
                rotIpf.SetTextWithoutNotify(lastRotStr == null ? "0" : lastRotStr);
                return;
            }
            float additionalRotValue = (rotValue -lastAngleOffset);
            lastAngleOffset =  rotValue;
            lastRotStr = rotStr;
            myScenario.lineConfigure.DisableDraw();
            syncPdcPackages.Do(s => s.ChangeColor());
            myScenario.lineConfigure.Draw();

            curXY.ChangeLocationBeforeAddChild((trf) =>
            {
                if (lastRotSequence != null)
                    lastRotSequence.Kill();

                lastRotSequence = DOTween.Sequence()
                                .SetAutoKill(true)
                                .Append(trf.DOLocalRotate(new Vector3(0, 0, -additionalRotValue), animTime, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuad))
                                .OnComplete(() => lastRotSequence = null);
            });


            //MainUIManager.Instance.SendLogTxt(rotValue.ToString() + "' 회전시켰습니다");

            if (!isOpenDesc)
            {
                isOpenDesc = true;
                DescriptonManager.Instance.OpenDescription(curPpoint.targetTrf, out _);
            }
        }

        void OnXValueChanged(string input)
        {
            if (int.TryParse(input, out int resultNum))
            {
                xOffset = resultNum * 10;
                ChangePos(CurLocalPos());
            }
        }
        //char OnXValueChanged(string input, int charIndex, char addedChar)
        //{
        //    if (UGUIUtil.TmpIpf_ValidateAndTryConvert(xPosIpf, int.TryParse, ref input, ref charIndex, ref addedChar, out int resultNum))
        //    {
        //        xOffset = resultNum * 10;
        //        ChangePos(CurLocalPos());
        //    }

        //    return addedChar;
        //}
        void OnYValueChanged(string input)
        {
            if (int.TryParse(input, out int resultNum))
            {
                yOffset = resultNum * 10;
                ChangePos(CurLocalPos());
            }
        }
        //char OnYValueChanged(string input, int charIndex, char addedChar)
        //{
        //    if (UGUIUtil.TmpIpf_ValidateAndTryConvert(yPosIpf, int.TryParse, ref input, ref charIndex, ref addedChar, out int resultNum))
        //    {
        //        yOffset = resultNum * 10;
        //        ChangePos(CurLocalPos());
        //    }
        //    return addedChar;
        //}

        Vector3 CurLocalPos()
        {
            return new Vector3(backupPos.x + xOffset, backupPos.y + yOffset, 0);
        }

        int xOffset = 0, yOffset = 0;
        float lastAngleOffset = 0;

        Sequence lastPosSequence;
        void ChangePos(Vector3 localPos)
        {
            myScenario.lineConfigure.DisableDraw();
            syncPdcPackages.Do(s => s.ChangeColor());
            myScenario.lineConfigure.Draw();

            curXY.ChangeLocationBeforeAddChild((trf) =>
            {
                if (lastPosSequence != null)
                    lastPosSequence.Kill();

                lastPosSequence = DOTween.Sequence()
                                .SetAutoKill(true)
                                .Append(trf.DOLocalMove(CurLocalPos(), animTime))
                                .OnComplete(() => lastPosSequence = null);
            });

            if (!isOpenDesc)
            {
                isOpenDesc = true;
                DescriptonManager.Instance.OpenDescription(curPpoint.targetTrf, out _);
            }
        }

        float backupAngle;
        Vector3 backupPos;
        public override void EnableAction()
        {
            xOffset = 0; yOffset = 0;
            lastAngleOffset = 0;
            xPosIpf.SetTextWithoutNotify("0");
            yPosIpf.SetTextWithoutNotify("0");
            rotIpf.SetTextWithoutNotify("0");

            backupPos = prevXY.targetTrf.localPosition;

            xPosIpf.onEndEdit.RemoveAllListeners();
            xPosIpf.onEndEdit.AddListener(OnXValueChanged);
            //xPosIpf.gameObject.SetActive(true);
            yPosIpf.onEndEdit.RemoveAllListeners();
            yPosIpf.onEndEdit.AddListener(OnYValueChanged);
            //yPosIpf.gameObject.SetActive(true);

            float z = TransformUtil.NormalizeAngle(prevXY.targetTrf.localEulerAngles.z);
            backupAngle = Mathf.FloorToInt(z);
            rotIpf.onEndEdit.RemoveAllListeners();
            rotIpf.onEndEdit.AddListener(OnRotChanged);
            isOpenDesc = false;

            curPpoint.SetParent(curXY.targetTrf);
        }

        public override void DisableAction()
        {
            xPosIpf.onEndEdit.RemoveListener(OnXValueChanged);
            yPosIpf.onEndEdit.RemoveListener(OnYValueChanged);
            rotIpf.onEndEdit.RemoveListener(OnRotChanged);
            
        }
        //{
        //    curPpoint.targetTrf.SetParent(curXY.targetTrf.parent);

        //    curXY.targetTrf.gameObject.SetActive(false);
        //    curPpoint.targetTrf.gameObject.SetActive(false);
        //    rotInputField.gameObject.SetActive(false);

        //}
    }
}
