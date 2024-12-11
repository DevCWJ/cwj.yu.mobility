using System;
using System.IO;
using System.Collections.Generic;

using CWJ.AccessibleEditor;

using UnityEngine;

namespace CWJ.EzPaint
{
    /// <summary>
    /// _2D, _3D ��� ���� ���������� ����
    /// <para/>[TODO]<br/>
    /// 1. drawableSprite�� ȸ���Ǿ������� ����� �׷���������.
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

        // ADD : ȹ ���� ť�� ����
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
                $"[�ſ� �߿��� {nameof(EzPaintSystem)} ����]" +
                $"\n\n1.spriteRenderer �� Sprite �������ֱ� \n(���� �ٷ� �׽�Ʈ �� �� �ְ� 1000x750 ũ���� ���� Sprite �����Ǿ�����)" +
                $"\n\n2.spriteCollider ũ�� ���� ���ֱ� (Texture Sprite ũ�⿡�� ������ 100)\n(���� �ٷ� �׽�Ʈ �� �� �ְ� 1.�� ���� Sprite�������� �����Ǿ�����)" +
                $"\n\n3.{nameof(spriteLayer)} ���̾� �����ϱ� \n(���� �ٷ� �׽�Ʈ �� �� �ְ� spriteCollider�� ���� ���̾�� �����Ǿ�����)" +
                $"\n\n4.{nameof(targetCamera)} �Ҵ�������� (����� {targetCamera.name}���� ����)");
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
                typeof(EzPaintSystem).PrintLogWithClassName($"{nameof(EzPaintSystem)}�� �ܵ����� ����������մϴ�.\n{nameof(EzPaintManager)}�� �ʿ��մϴ�.\n{nameof(EzPaintManager)}�� ����� �ڵ����� �׽�Ʈ�� ���� �� ���ر��� �������ݴϴ�.", LogType.Warning, gameObject);
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

        //�������������� �������� �������� �ȼ� ���� ����
        protected void PaintColorBetween(Vector2 startPoint, Vector2 endPoint, int width, Color color)
        {
            float distance = Vector2.Distance(startPoint, endPoint);
            Vector2 direction = (startPoint - endPoint).normalized;

            Vector2 curPosition = startPoint;

            float lerpSteps = 1 / (distance * 2);

            //������ ������Ʈ �� elapsed ���̿� startPoint�� endPoint���� ��������
            for (float lerp = 0; lerp <= 1; lerp += lerpSteps)
            {
                curPosition = Vector2.Lerp(startPoint, endPoint, lerp);
                MarkPixelsToColor(curPosition, width, color);
            }
        }

        protected void MarkPixelsToColor(Vector2 centerPixel, int penThickness, Color penColor)
        {
            // �� ���⿡�� �� �󸶳� ĥ�ؾ��ϴ��� �ľ�
            int centerX = (int)centerPixel.x;
            int centerY = (int)centerPixel.y;
            //int extraRadius = Mathf.Min(0, pen_thickness - 2);

            for (int x = centerX - penThickness; x <= centerX + penThickness; x++)
            {
                //x���� �̹����� ������� Ȯ��
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
            // x �� y ��ǥ�� �迭�� ��� ��ǥ�� ��ȯ
            int arrayPos = y * (int)sprite.rect.width + x;

            if (arrayPos >= curColors.Length || arrayPos < 0)
            {
                    Debug.LogError("MarkPixelToChange?");
                return;
            }

            // ADD : �ǵ����� ��� �߰��� ���� �迭 ����
            // TODO : ���� �ٸ���쿡 ������ ĥ������������ �������������
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

            //������ǥ�� �߾ӿ� ��ġ
            float centerX = localPos.x * unitsToPixels + pixelWidth / 2;
            float centerY = localPos.y * unitsToPixels + pixelHeight / 2;

            //���� ���콺 ��ġ�� ��ó �ȼ��� �ݿø�
            Vector2 pixelPos = new Vector2(Mathf.RoundToInt(centerX), Mathf.RoundToInt(centerY));

            return pixelPos;
        }

        // ADD : ȹ ���� (by �輺��)
        protected void SaveLastDraw()
        {
            if (previousDraw.Count != 0)
            {
                Queue<int> lastDraw = new Queue<int>(previousDraw);
                previousDraw.Clear();
                unDoStack.Push(lastDraw);
            }
        }
        // ADD : ȹ ����� (by �輺��)
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

        //// �ȼ��� ���� ä���ϴ� ��� (MarkPixelsToColor ���� ApplyMarkedPixelChange �ϴ� ��� ���� ����)
        //// SetPixels32 �� SetPixel���� �ξ�����
        //// �汽�⸦ �������� ��� �ȼ��� ��� �ȼ� ������ ���� �ȼ��� ��� ä��
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