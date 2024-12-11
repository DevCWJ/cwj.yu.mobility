using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

using static CWJ.LinePoint_EzDrawer;

namespace CWJ.YU.Mobility
{
    public abstract class TopicAction : MonoBehaviour
    {
        public Topic topic;
        public abstract int subTopicNum { get; }

        [SerializeField] protected TMP_InputField xPosIpf;
        [SerializeField] protected TMP_InputField yPosIpf;
        [SerializeField] protected TMP_InputField rotIpf;

        public Topic.Scenario myScenario;

        public float animTime = 2.0f;
        [SerializeField] protected SyncPdcPackage[] syncPdcPackages;


        protected void Awake()
        {
            myScenario = topic.scenarios[subTopicNum];
            myScenario.enableEvent.AddListener_New(_EnableAction);
            myScenario.disableEvent.AddListener_New(_DisableAction);


            for (int i = 0; i < syncPdcPackages.Length; i++)
            {
                syncPdcPackages[i].Init(myScenario.lineConfigure);
            }
        }

        protected void _EnableAction()
        {
            gameObject.SetActive(true);
            for (int i = 0; i < syncPdcPackages.Length; i++)
            {
                syncPdcPackages[i].Enable();
            }

            EnableAction();

            if (rotIpf != null)
                rotIpf.gameObject.SetActive(true);
            if (xPosIpf != null)
                xPosIpf.transform.parent.gameObject.SetActive(true);
        }
        public abstract void EnableAction();

        protected void _DisableAction()
        {
            gameObject.SetActive(false);

            if (rotIpf != null)
                rotIpf.gameObject.SetActive(false);
            if (xPosIpf != null)
                xPosIpf.transform.parent.gameObject.SetActive(false);

            for (int i = 0; i < syncPdcPackages.Length; i++)
            {
                syncPdcPackages[i].Disable();
            }

            DisableAction();
        }

        public abstract void DisableAction();

        [System.Serializable]
        public class SyncPdcPackage
        {
            [SerializeField] string packageName;
            public SyncablePdc srcPdc;
            public SyncablePdc destPdc;

            public void Init(LinePoint_EzDrawer lineEditor)
            {
                srcPdc.Init(lineEditor);
                if (destPdc != null && destPdc.targetTrf != null)
                    destPdc.Init(lineEditor);
            }

            public void ChangeColor(bool needDrawProcess = false)
            {
                if (needDrawProcess)
                    destPdc.lineEditor.DisableDraw();

                destPdc.targetTrf.gameObject.SetActive(false);
                srcPdc.SetChangeColor();
                destPdc.SetChangeColor();
                //destPdc.SyncLocation(srcPdc);
                destPdc.targetTrf.gameObject.SetActive(true);

                if (needDrawProcess)
                    destPdc.lineEditor.Draw();
            }

            public void Enable()
            {
                srcPdc.RestoreColor();
                srcPdc.UpdateNameByPos();

                srcPdc.targetTrf.gameObject.SetActive(true);

                if (destPdc != null && destPdc.targetTrf != null)
                {
                    destPdc.RestoreColor();
                    destPdc.targetTrf.gameObject.SetActive(false);
                    destPdc.SyncLocation(srcPdc);
                }
            }
            public void Disable()
            {
                if (destPdc != null && destPdc.targetTrf != null)
                    destPdc.targetTrf.gameObject.SetActive(false);
            }
        }


        [System.Serializable]
        public class SyncablePdc
        {
            public Transform targetTrf;
            [ColorUsage(true)]
            public Color changeColor;
            public int pdcIndex { get; private set; }
            public Color backupColor { get; private set; }
            public LinePoint_EzDrawer lineEditor { get; private set; }
            public Transform backupParentTrf { get; private set; }
            public string backupFullName { get; private set; }
            /// <summary>
            /// Start때 불러주자
            /// </summary>
            /// <param name="lineEditor"></param>
            public void Init(LinePoint_EzDrawer lineEditor)
            {
                this.lineEditor = lineEditor;
                backupParentTrf = targetTrf.parent;
                backupFullName = targetTrf.gameObject.name.Trim();
                pdcIndex = lineEditor.pointDataContainers.FindIndex(pdc => pdc.pointTrfs[0] == targetTrf);
                Debug.Assert(pdcIndex >= 0);
                backupColor = lineEditor.pointDataContainers[pdcIndex].color;
            }

            public void RestoreColor()
            {
                lineEditor.pointDataContainers[pdcIndex].color = backupColor;
            }

            public void SetChangeColor()
            {
                lineEditor.pointDataContainers[pdcIndex].color = changeColor;
            }
            public void SyncLocation(SyncablePdc syncTarget)
            {
                this.SetParent(syncTarget.targetTrf.parent);
                targetTrf.localScale = syncTarget.targetTrf.localScale;
                targetTrf.localPosition = syncTarget.targetTrf.localPosition;
                targetTrf.localRotation = syncTarget.targetTrf.localRotation;
                this.RestoreParent();
                this.UpdateNameByPos();
            }

            public void UpdateNameByPos()
            {
                targetTrf.gameObject.name = Extension.UpdateNameViaPos(backupFullName, targetTrf.localPosition);
            }

            public void SetParent(Transform parent)
            {
                targetTrf.SetParent(parent, true);
            }

            public void RestoreParent()
            {
                targetTrf.SetParent(backupParentTrf, true);
            }

            public void ChangeLocationBeforeAddChild(Action<Transform> changeLocationAction, float animTime = 0.5f, bool isChangeAllColor = true)
            {
                if (!targetTrf.gameObject.activeSelf)
                    targetTrf.gameObject.SetActive(true);

                changeLocationAction.Invoke(targetTrf);
                UpdateNameByPos();

                if (CO_updateName != null) lineEditor.StopCoroutine(CO_updateName);
                CO_updateName = lineEditor.StartCoroutine(DO_UpdateName(animTime));
            }

            Coroutine CO_updateName = null;

            IEnumerator DO_UpdateName(float animTime)
            {
                yield return null;

                var p = lineEditor.pointDataContainers.FirstOrDefault(pdc => pdc.pointTrfs.IsExists(t => t == targetTrf));

                var cachePackList = p.caches.SelectMany(c => LinePoint_Generator.Instance.TryGetByCacheDic(c)).ToArray();


                float t = 0;
                do
                {
                    targetTrf.gameObject.name = Extension.UpdateNameViaPos(backupFullName, targetTrf.localPosition);
                    cachePackList.Do(c => c?.UpdateUIs());
                    yield return null;
                    t += Time.deltaTime;
                } while (animTime > t);

                CO_updateName = null;
            }
        }


    }
}
