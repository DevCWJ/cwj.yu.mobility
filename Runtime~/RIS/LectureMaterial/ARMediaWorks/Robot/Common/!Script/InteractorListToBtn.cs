using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace CWJ.YU.Mobility
{
    public class InteractorListToBtn : MonoBehaviour
    {
        public Topic topic;
        public FbxInteractor[] fbxInteractors_left;
        public FbxInteractor[] fbxInteractors_right;
        public Button prefabBtn;
        public Transform btnParent_left, btnParent_right;
        List<FbxInteractor> allInteractors;
        //static string CamelToNormal(string text)
        //{
        //    // 대문자 앞에 공백을 추가하는 정규 표현식
        //    return Regex.Replace(text, "(?<!^)(?=[A-Z])", " ");
        //}

        private void Start()
        {
            allInteractors = new List<FbxInteractor>();
            prefabBtn.gameObject.SetActive(true);
            for (int i = 0; i < fbxInteractors_left.Length; i++)
            {
                FiToBtnProcess(fbxInteractors_left[i], btnParent_left);
            }
            for (int i = 0; i < fbxInteractors_right.Length; i++)
            {
                FiToBtnProcess(fbxInteractors_right[i], btnParent_right);
            }
            prefabBtn.gameObject.SetActive(false);
        }

        void FiToBtnProcess(FbxInteractor fi, Transform parent)
        {
            allInteractors.Add(fi);
            var newBtn = Instantiate(prefabBtn, parent);
            string displayName = fi.animHandler.GetTriggerNames()[0];
            newBtn.gameObject.name = displayName;
            newBtn.GetComponentInChildren<TextMeshProUGUI>().SetText(displayName);
            newBtn.onClick.AddListener(() => OnClickBtn(fi));
        }

        void OnClickBtn(FbxInteractor fi)
        {
            allInteractors.ForEach(f => f.gameObject.SetActive(false));
            fi.gameObject.SetActive(true);
            topic.topicUI.SetTarget(fi.rotateObjByDrag.transform, false);
            string triggerName = fi.animHandler.GetTriggerNames()[0];
            topic.topicUI.SendLogTxt($"'{triggerName}' 를 활성화 했습니다");
            MultiThreadHelper.LateUpdateQueue(() => fi.animHandler.SetTrigger(triggerName));
        }
    }
}
