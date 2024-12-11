using UnityEngine;

namespace CWJ
{
    public class BackToQuit : MonoBehaviour
    {
        private bool isPreparedToQuit = false;
        [SerializeField] float quitCommandTime = 2;
        private void OnEnable()
        {
            ResetQuitFlag();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPreparedToQuit)
                {
                    isPreparedToQuit = true;
                    AndroidToast.Instance.ShowToastMessage("�ڷΰ��� ��ư�� �� �� �� �����ø� ����˴ϴ�.");
                    this.Invoke(nameof(ResetQuitFlag), quitCommandTime);
                }
                else
                {
                    Debug.Log("Quit");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                }
            }
        }

        private void ResetQuitFlag()
        {
            isPreparedToQuit = false;
        }
    }
}