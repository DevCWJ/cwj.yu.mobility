using System;
using System.IO;
using System.Collections.Generic;

using CWJ.AccessibleEditor;

using UnityEngine;

namespace CWJ.EzPaint
{
    /// <summary>
    /// _2D, _3D 모두 아직 부족한점이 있음
    /// <para/>[TODO]<br/>
    /// 1. drawableSprite가 회전되어있으면 제대로 그려지지않음.
    /// </summary>
    public abstract class EzPaintSystem : MonoBehaviour
    {
        protected abstract bool is3DPaint { get; }

        [GetComponent] public new Transform transform;

        [Header("Sprite Setting")]
        [HelpBox("[Drawable Sprite Setting]\nSprite Mode:Single/ Pixel Per Unit:100/\nAlpha Is Transparency:true/ Read/Write Enabled:true/\nWrapMode:Clamp/ FilterMode:Bilinear/\nMaxSize:2048/ Compression:None")]
        [SerializeField, OnValueChanged(nameof(UpdateSpriteRenderer)), ErrorIfNull] protected Sprite sprite = null;

        protected Texture2D GetSpriteTexture() => sprite.texture;

        public Sprite Sprite
        {
            get => sprite;
            set
            {
                if (value == null || sprite == value)
                {
                    return;
                }
                sprite = value;
                UpdateSpriteRenderer();
            }
        }

        // ADD : 획 저장 큐와 스택
        protected Queue<int> previousDraw = new Queue<int>();
        protected Stack<Queue<int>> unDoStack = new Stack<Queue<int>>();

        private void UpdateSpriteRenderer()
        {
            SetSpriteRenderer(sprite, true);
        }

        public LayerMask spriteLayer;

        [SerializeField, OnValueChanged(nameof(UpdateSpriteOutlineWidth))] protected float spriteOutlineWidth = .1f;

        public float SpriteOutlineWidth
        {
            get => spriteOutlineWidth;
            set
            {
                if (spriteOutlineWidth == value)
                {
                    return;
                }
                spriteOutlineWidth = value;
                UpdateSpriteOutlineWidth();
            }
        }

        private void UpdateSpriteOutlineWidth()
        {
            SetSpriteOutlineWidth(spriteOutlineWidth);
        }

        [SerializeField, OnValueChanged(nameof(UpdateSpriteOutlineColor))] protected Color spriteOutlineColor = Color.black;

        public Color SpriteOutlineColor
        {
            get => spriteOutlineColor;
            set
            {
                if (spriteOutlineColor == value)
                {
                    return;
                }
                spriteOutlineColor = value;
                UpdateSpriteOutlineColor();
            }
        }

        private void UpdateSpriteOutlineColor()
        {
            SetSpriteOutlineColor(spriteOutlineColor);
        }

        [SerializeField, OnValueChanged(nameof(UpdateSpriteOutlineEnalbed))] protected bool spriteOutlineEnabled = true;

        public bool SpriteOutlineEnabled
        {
            get => spriteOutlineEnabled;
            set
            {
                if (spriteOutlineEnabled == value)
                {
                    return;
                }
                spriteOutlineEnabled = value;
                UpdateSpriteOutlineEnalbed();
            }
        }

        private void UpdateSpriteOutlineEnalbed()
        {
            OutlineSetActive(spriteOutlineEnabled);
        }

        [Header("Paint Setting")]
        public Color penColor = Color.black;

        public int penWidth = 3;

        private Color backupPenColor;

        private void ChangePenColorWithBackup(Color newColor)
        {
            backupPenColor = penColor;
            penColor = newColor;
        }

        [SerializeField, OnValueChanged(nameof(UpdateEraserEnalbed))] private bool isEraserEnabled = false;

        public bool IsEraserEnabled
        {
            get => isEraserEnabled;
            set
            {
                if (isPartialEraserEnabled == value)
                {
                    return;
                }
                isEraserEnabled = value;
                UpdateEraserEnalbed();
            }
        }

        private void UpdateEraserEnalbed()
        {
            if (isEraserEnabled)
            {
                ChangePenColorWithBackup(resetColor);
            }
            else
            {
                penColor = backupPenColor;
            }
        }

        [SerializeField, OnValueChanged(nameof(UpdatePartialEraserEnalbed))] private bool isPartialEraserEnabled = false;

        public bool IsPartialEraserEnabled
        {
            get => isPartialEraserEnabled;
            set
            {
                if (isPartialEraserEnabled == value)
                {
                    return;
                }
                isPartialEraserEnabled = value;
                UpdatePartialEraserEnalbed();
            }
        }

        private void UpdatePartialEraserEnalbed()
        {
            if (isPartialEraserEnabled)
            {
                Color partialEraserColor = resetColor;
                partialEraserColor.a = 0.5f;
                ChangePenColorWithBackup(partialEraserColor);
            }
            else
            {
                penColor = backupPenColor;
            }
        }

        public Color resetColor = Color.white;
        public bool isResetCanvasOnStart = true;
        public bool isResetCanvasOnQuit = false;

#if UNITY_EDITOR

        private string GetDefaultSpritePath()
        {
            return Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.MonoScript.FromMonoBehaviour(this))) + "\\EzPaintDefaultSprite.png";
        }

        private void Reset()
        {
            Initialized();

            DisplayDialogUtil.DisplayDialog<EzPaintSystem>(
                $"[매우 중요한 {nameof(EzPaintSystem)} 설정]" +
                $"\n\n1.spriteRenderer 에 Sprite 설정해주기 \n(현재 바로 테스트 할 수 있게 1000x750 크기의 예제 Sprite 설정되어있음)" +
                $"\n\n2.spriteCollider 크기 설정 해주기 (Texture Sprite 크기에서 나누기 100)\n(현재 바로 테스트 할 수 있게 1.의 예제 Sprite기준으로 설정되어있음)" +
                $"\n\n3.{nameof(spriteLayer)} 레이어 설정하기 \n(현재 바로 테스트 할 수 있게 spriteCollider와 같은 레이어로 설정되어있음)" +
                $"\n\n4.{nameof(targetCamera)} 할당해줘야함 (현재는 {targetCamera.name}으로 설정)");
        }

#endif

        [Header("System Setting")]
        [SerializeField, Readonly] protected bool isInitialized = false;

        private void Initialized()
        {
            if (isInitialized) return;

            transform = GetComponent<Transform>();
            transform.Reset();

            if (!spriteLayer.LayerContains(gameObject.layer))
            {
                spriteLayer = spriteLayer.Include(gameObject);
            }

            _Initialized();

#if UNITY_EDITOR
            if (sprite == null) { SetSpriteRenderer(UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(GetDefaultSpritePath()), true); }
#endif
            if (!EzPaintManager.IsExists)
            {
                typeof(EzPaintSystem).PrintLogWithClassName($"{nameof(EzPaintSystem)}은 단독으론 사용하지못합니다.\n{nameof(EzPaintManager)}가 필요합니다.\n{nameof(EzPaintManager)}를 만들면 자동으로 테스트가 가능 한 수준까지 설정해줍니다.", LogType.Warning, gameObject);
            }

            isInitialized = true;
        }

        protected abstract void _Initialized();

        [ContextMenu("(Dangerous) " + nameof(SetInitialized))]
        protected void SetInitialized()
        {
            isInitialized = false;
            Initialized();
        }

        [FindObject(true)] public Camera targetCamera;

        public abstract void SetSpriteRenderer(Sprite sprite, bool isReset);

        protected abstract void SetSpriteOutlineWidth(float width);

        protected abstract void SetSpriteOutlineColor(Color color);

        protected abstract void OutlineSetActive(bool enabled);

        private void Awake()
        {
            Initialized();

            if (isResetCanvasOnStart)
            {
                ResetCanvas();
            }
        }

        private void OnApplicationQuit()
        {
            if (isResetCanvasOnQuit)
            {
                ResetCanvas();
            }
        }

        #region Input Process via TouchManager

        public abstract void TouchHandler_HoldDown();

        public abstract void TouchHandler_Ended();

        public abstract void TouchHandler_UpdateEnded();

        #endregion Input Process via TouchManager

        #region Canvas , Pen Setting

        private Color[] cleanColorArray = null;
        private Color32[] curColors = null;
        protected Vector2 prevDragPos;

        //protected Action<Vector2> drawInSpriteEvent;
        [InvokeButton]
        public void ResetCanvas()
        {
            if (cleanColorArray == null)
            {
                int length = (int)sprite.rect.width * (int)sprite.rect.height;
                cleanColorArray = new Color[length];
                for (int i = 0; i < length; ++i)
                    cleanColorArray[i] = resetColor;
            }
            GetSpriteTexture().SetPixels(cleanColorArray);
            GetSpriteTexture().Apply();
            curColors = GetSpriteTexture().GetPixels32();
        }

        protected void PaintOnSprite(Vector2 inputPosition)
        {
            Vector2 pixelPos = WorldToPixelCoordinates(inputPosition);
            //curColors = GetSpriteTexture().GetPixels32();

            //

            if (prevDragPos == Vector2.zero)
            {
                MarkPixelsToColor(pixelPos, penWidth, penColor);
            }
            else
            {
                PaintColorBetween(prevDragPos, pixelPos, penWidth, penColor);
            }
            ApplyMarkedPixelChange();

            prevDragPos = pixelPos;
        }

        #endregion Canvas , Pen Setting

        #region Draw Process

        //시작점에서부터 끝점까지 직선으로 픽셀 색상 설정
        protected void PaintColorBetween(Vector2 startPoint, Vector2 endPoint, int width, Color color)
        {
            float distance = Vector2.Distance(startPoint, endPoint);
            Vector2 direction = (startPoint - endPoint).normalized;

            Vector2 curPosition = startPoint;

            float lerpSteps = 1 / (distance * 2);

            //마지막 업데이트 후 elapsed 사이에 startPoint와 endPoint간에 선형보간
            for (float lerp = 0; lerp <= 1; lerp += lerpSteps)
            {
                curPosition = Vector2.Lerp(startPoint, endPoint, lerp);
                MarkPixelsToColor(curPosition, width, color);
            }
        }

        protected void MarkPixelsToColor(Vector2 centerPixel, int penThickness, Color penColor)
        {
            // 각 방향에서 색 얼마나 칠해야하는지 파악
            int centerX = (int)centerPixel.x;
            int centerY = (int)centerPixel.y;
            //int extraRadius = Mathf.Min(0, pen_thickness - 2);

            for (int x = centerX - penThickness; x <= centerX + penThickness; x++)
            {
                //x값이 이미지를 벗어났는지 확인
                if (x >= (int)sprite.rect.width || x < 0)
                {
                    continue;
                }

                for (int y = centerY - penThickness; y <= centerY + penThickness; y++)
                {
                    MarkPixelToChange(x, y, penColor);
                }
            }
        }

        protected void MarkPixelToChange(int x, int y, Color color)
        {
            // x 및 y 좌표를 배열의 평면 좌표로 변환
            int arrayPos = y * (int)sprite.rect.width + x;

            if (arrayPos >= curColors.Length || arrayPos < 0)
            {
                    Debug.LogError("MarkPixelToChange?");
                return;
            }

            // ADD : 되돌리기 기능 추가를 위한 배열 저장
            // TODO : 색이 다른경우에 이전에 칠해진색상으로 복구시켜줘야함
            if (curColors[arrayPos] == resetColor)
                previousDraw.Enqueue(arrayPos);

            curColors[arrayPos] = color;
        }

        protected void ApplyMarkedPixelChange()
        {
            GetSpriteTexture().SetPixels32(curColors);
            GetSpriteTexture().Apply();
        }

        protected Vector2 WorldToPixelCoordinates(Vector2 worldPosition)
        {
            Vector3 localPos = transform.InverseTransformPoint(worldPosition);
            float pixelWidth = sprite.rect.width;
            float pixelHeight = sprite.rect.height;
            float unitsToPixels = pixelWidth / sprite.bounds.size.x * transform.localScale.x;

            //로컬좌표를 중앙에 배치
            float centerX = localPos.x * unitsToPixels + pixelWidth / 2;
            float centerY = localPos.y * unitsToPixels + pixelHeight / 2;

            //현재 마우스 위치를 근처 픽셀로 반올림
            Vector2 pixelPos = new Vector2(Mathf.RoundToInt(centerX), Mathf.RoundToInt(centerY));

            return pixelPos;
        }

        // ADD : 획 저장 (by 김성수)
        protected void SaveLastDraw()
        {
            if (previousDraw.Count != 0)
            {
                Queue<int> lastDraw = new Queue<int>(previousDraw);
                previousDraw.Clear();
                unDoStack.Push(lastDraw);
            }
        }
        // ADD : 획 지우기 (by 김성수)
        [InvokeButton]
        public void UndoPreviousDraw()
        {
            if (unDoStack.Count == 0)
                return;

            Queue<int> unDoDraw = unDoStack.Pop();
            while (unDoDraw.Count != 0)
            {
                int array_pos = unDoDraw.Dequeue();
                curColors[array_pos] = resetColor;
            }
            unDoDraw.Clear();
            ApplyMarkedPixelChange();
        }

        //// 픽셀을 직접 채색하는 방식 (MarkPixelsToColor 이후 ApplyMarkedPixelChange 하는 방식 보다 느림)
        //// SetPixels32 이 SetPixel보다 훨씬빠름
        //// 펜굵기를 기준으로 가운데 픽셀과 가운데 픽셀 주위의 여러 픽셀을 모두 채색
        //public void SetColorPixels(Vector2 centerPixel, int penThickness, Color penColor)
        //{
        //    int center_x = (int)centerPixel.x;
        //    int center_y = (int)centerPixel.y;
        //    //int extra_radius = Mathf.Min(0, pen_thickness - 2);

        //    for (int x = center_x - penThickness; x <= center_x + penThickness; x++)
        //    {
        //        for (int y = center_y - penThickness; y <= center_y + penThickness; y++)
        //        {
        //            drawableTexture.SetPixel(x, y, penColor);
        //        }
        //    }

        //    drawableTexture.Apply();
        //}

        #endregion Draw Process
       
        [InvokeButton]
        public void SaveTextureToPng()
        {
            string path = (Application.persistentDataPath + $"/EzPaint/{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}_Painting.png").ToValidDirectoryPathByApp(false);
            SaveTextureToPng(path);
#if UNITY_EDITOR
            path.OpenFolder();
#endif
        }

        public void SaveTextureToPng(string path)
        {
            Texture2D texture2D = sprite.texture;
            byte[] pngBytes = texture2D.EncodeToPNG();

            path.IsFolderExists(true);

            File.WriteAllBytes(path, pngBytes);
        }

        protected virtual void OnEnable()
        {
            OutlineSetActive(spriteOutlineEnabled);
        }
        protected virtual void OnDisable()
        {
            OutlineSetActive(false);
        }
    }
}