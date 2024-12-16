using UnityEngine;
using CWJ.Singleton;
using System.Linq;
namespace CWJ
{
    /// <summary>
    /// 에디터, 빌드, 실행중 등 상관없이 카메라만 있으면 스크린샷 가능
    /// <para/>UI를 찍고싶다면 Canvas의 RenderMode를 ScreenSpace - Camera로 변경, Render Camera를 targetCamera로 설정하기
    /// </summary>
    public class CameraScreenshotFunction : SingletonBehaviour<CameraScreenshotFunction>
    {
        [FindObject(true), SerializeField] private Camera targetCamera = null;

        protected override void _Reset()
        {
            InitCamera();
        }
        void InitCamera()
        {
            if (targetCamera == null) targetCamera = FindObjectsOfType<Camera>().Where(c => c.gameObject.activeInHierarchy && c.enabled).First();

        }
        protected override void _Start()
        {
            InitCamera();
        }

        public byte[] GetScreenshotBytes(Camera setCamera = null)
        {
            if (setCamera != null) targetCamera = setCamera;
            if (targetCamera == null) return null;

            return Screenshot(targetCamera, targetCamera.pixelWidth, targetCamera.pixelHeight);
        }

        byte[] Screenshot(Camera camera, int width, int height)
        {
            var camTextureBackup = camera.targetTexture;
            var renderTextureTmp = RenderTexture.GetTemporary(width, height, 16);//스텐실 버퍼나 Z-Fighting때문 아니라면 16으로 하기. 24보단 16이 훨 빠름
            camera.targetTexture = renderTextureTmp;

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = renderTextureTmp;
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();
            byte[] result = texture.EncodeToPNG();

            RenderTexture.ReleaseTemporary(renderTextureTmp);
            camera.targetTexture = camTextureBackup;
            if (camTextureBackup != null)
                camera.Render();

            return result;
        }

        void Screenshot(Camera camera, int width, int height, string savePath)
        {
            byte[] bytes = Screenshot(camera, width, height);

            if (string.IsNullOrEmpty(savePath) || savePath.IsFolderExists(false))
            {
                string fileName = $"{System.DateTime.Now.ToString("yy-MM-dd_HH-mm-ss") }_CameraScreenshot";
                string extension = "png";
                if (Application.isEditor)
                {
                    savePath = AccessibleEditor.AccessibleEditorUtil.SelectFilePath(name: fileName, extension: extension);
                }
                else
                {
                    savePath = Application.dataPath + $"/CWJ_CameraScreenshot/{fileName}.{extension}";
                }
            }

            if (savePath.IsFolderExists(true))
            {
                System.IO.File.WriteAllBytes(savePath, bytes);
#if UNITY_EDITOR
                AccessibleEditor.AccessibleEditorUtil.PingAssetFile(savePath, "Saved " + savePath);
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
        }

        [InvokeButton]
        public void FullSizeScreenshot(string savePath = null, Camera setCamera = null)
        {
            if (setCamera != null) targetCamera = setCamera;
            if (targetCamera == null) return;

            Screenshot(targetCamera, targetCamera.pixelWidth, targetCamera.pixelHeight, savePath);
        }

        [InvokeButton]
        public void CustomSizeScreenshot(int width, int height, string savePath = null, Camera setCamera = null)
        {
            if (setCamera != null) targetCamera = setCamera;
            if (targetCamera == null) return;

            if (width > targetCamera.pixelWidth || width <= 0) width = targetCamera.pixelWidth;
            if (height > targetCamera.pixelHeight || height <= 0) height = targetCamera.pixelHeight;
            Screenshot(targetCamera, width, height, savePath);
        }


        //실행중에만 되던방식
        //protected override void _Awake()
        //{
        //    this.enabled = false;
        //}

        //private void OnPostRender()
        //{
        //    int w = (width <= camera.pixelWidth && width > 0) ? width : camera.pixelWidth;
        //    int h = (height <= camera.pixelHeight && height > 0) ? height : camera.pixelHeight;

        //    camera.targetTexture = RenderTexture.GetTemporary(width, height, 24);//16 or 24
        //    RenderTexture renderTexture = camera.targetTexture;

        //    Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        //    Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        //    renderResult.ReadPixels(rect, 0, 0);

        //    byte[] byteArray = renderResult.EncodeToPNG();

        //    string path = GetSavePath();
        //    if (path.IsFolderExists(true))
        //    {
        //        System.IO.File.WriteAllBytes(path, byteArray);
        //        Debug.Log("Saved " + path);
        //    }

        //    RenderTexture.ReleaseTemporary(renderTexture);
        //    camera.targetTexture = null;

        //    this.enabled = false;
        //}
    }
}