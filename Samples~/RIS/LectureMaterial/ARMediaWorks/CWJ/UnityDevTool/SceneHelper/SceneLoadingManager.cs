#if (CWJ_SCENEENUM_ENABLED || CWJ_SCENEENUM_DISABLED)

using UnityEngine;
using UnityEngine.UI;

using CWJ.Singleton;

namespace CWJ.SceneHelper
{
    public class SceneLoadingManager : SingletonBehaviourDontDestroy<SceneLoadingManager>
    {
        [System.Serializable]
        public struct LoadingUIPackage
        {
            public Sprite bg;
            public Color bgColor;
            public Sprite bar;
            public Color barColor;

            public string txt;

            public LoadingUIPackage(Sprite bg, Color bgColor, Sprite bar, Color barColor, string txt)
            {
                this.bg = bg;
                this.bgColor = bgColor;
                this.bar = bar;
                this.barColor = barColor;
                this.txt = txt;
            }
        }

        public void SetLoadingUIPackage(LoadingUIPackage loadingUIPackage)
        {
            if (loadingUIPackage.bg == null)
            {
                loadingBGImg.sprite = null;
                loadingBGImg.color = Color.clear;
            }
            else
            {
                loadingBGImg.sprite = loadingUIPackage.bg;
                loadingBGImg.color = loadingUIPackage.bgColor;
            }

            if (loadingUIPackage.bar == null)
            {
                loadingBarImg.sprite = null;
                loadingBarImg.color = Color.clear;
            }
            else
            {
                loadingBarImg.sprite = loadingUIPackage.bar;
                loadingBarImg.color = loadingUIPackage.barColor;
            }
            loadingText.text = loadingUIPackage.txt;
        }

        public Canvas loadingRootCanvas;
        public Image loadingBGImg;
        public Image loadingBarImg;

        public void SetProgressBar(float progress)
        {
            if (loadingBarImg.sprite == null)
            {
                return;
            }
            loadingBarImg.fillAmount = progress;
        }

        public Text loadingText;

        public Text loadingLogText;
        public Button skipButton;
        [Header("--ui set--")]
        public LoadingUIPackage loginLoadingPackage;
        public LoadingUIPackage mainLoadingPackage;
        public LoadingUIPackage gameLoadingPackage;

        public LoadingUIPackage elseLoadingPackage;

        protected override void _Awake()
        {
#if CWJ_MULTI_DISPLAY
            MultiDisplayManager.Instance.AddStaticCanvas(loadingRootCanvas);
#endif
            skipButton.onClick.AddListener_New(() => SceneControlManager.Instance.isLoadingSkip = true, isPrintWarningLog: false);
            OnEnable();
            OnDisable();
            this.enabled = false;
        }
        protected override void _OnEnable()
        {
            UiInit();

            loadingRootCanvas.gameObject.SetActive(true);
        }

        private void ConnectTimeOutCallback()
        {
            skipButton.gameObject.SetActive(true);
        }

        protected override void _OnDisable()
        {
            loadingRootCanvas.gameObject.SetActive(false);
        }

        private void SetLogText(string log)
        {
            loadingLogText.text = log;
        }

        private void UiInit()
        {
            skipButton.gameObject.SetActive(false);
#if CWJ_SCENEENUM_ENABLED
            SceneEnum _curScene, _nextScene;
            _curScene = SceneControlManager.Instance.curSceneType;
            _nextScene = SceneControlManager.Instance.nextSceneType;
            //if (_curScene.Equals(/*SceneEnum.Intro*/)) //로그인중
            //{
            //    SetLoadingUIPackage(loginLoadingPackage);
            //}
            //else //로그인이후 씬전환
            //{
            //if (_nextScene.Equals(SceneType.Main))
            //{
            //    SetLoadingUIPackage(mainLoadingPackage);
            //}
            //else if (_nextScene.Equals(SceneType.Game))
            //{
            //    SetLoadingUIPackage(gameLoadingPackage);
            //}
            //else
            //{
            //    SetLoadingUIPackage(elseLoadingPackage);
            //}
            //}
#endif
        }
    }
}
#endif
