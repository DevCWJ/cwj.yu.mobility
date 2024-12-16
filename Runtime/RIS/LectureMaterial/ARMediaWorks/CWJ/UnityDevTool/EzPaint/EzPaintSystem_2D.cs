using CWJ.AccessibleEditor;
using CWJ.UI;

using UnityEngine;
using UnityEngine.UI;

namespace CWJ.EzPaint
{
    public sealed class EzPaintSystem_2D : EzPaintSystem
    {
        protected override sealed bool is3DPaint => false;

        [GetComponentInParent] public Canvas rootCanvas;
        [SerializeField, Readonly] private Image spriteRenderer = null;
        [SerializeField, Readonly] private BoxCollider2D spriteCollider = null;
        [SerializeField, Readonly] private Outline_UI spriteOutline = null;

        public override sealed void SetSpriteRenderer(Sprite sprite, bool isReset)
        {
            if (sprite == null)
            {
                return;
            }
            spriteRenderer.sprite = sprite;

            Vector2 spriteSize = Vector2.Scale(sprite.rect.size, Vector2.one * .01f);
            spriteCollider.size = spriteSize;
            GetComponent<RectTransform>().sizeDelta = spriteSize;
            base.sprite = sprite;

            if (isReset)
            {
                ResetCanvas();
            }
            SetSpriteOutlineWidth(SpriteOutlineWidth);
            SetSpriteOutlineColor(SpriteOutlineColor);
            OutlineSetActive(SpriteOutlineEnabled);
        }

        protected override sealed void SetSpriteOutlineWidth(float width)
        {
            spriteOutlineWidth = width;
            spriteOutline.effectDistance = new Vector2(width, width);
        }

        protected override void SetSpriteOutlineColor(Color color)
        {
            spriteOutlineColor = color;
            spriteOutline.effectColor = color;
        }

        protected override sealed void OutlineSetActive(bool enabled)
        {
            spriteOutline.enabled = enabled;
        }

        protected override sealed void _Initialized()
        {
            rootCanvas = GetComponentInParent<Canvas>();

            if (rootCanvas == null)
            {
                typeof(EzPaintSystem).PrintLogWithClassName($"{nameof(EzPaintSystem_2D)}는 Canvas아래에서만 작동합니다", LogType.Error);
                return;
            }

            targetCamera = (rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay && rootCanvas.worldCamera != null) ? rootCanvas.worldCamera : Camera.main;
            if (targetCamera == null) targetCamera = FindObjectOfType<Camera>();

            gameObject.GetOrAddComponent<RectTransform>();
            transform = GetComponent<Transform>();
            transform.parent?.gameObject.GetOrAddComponent<RectTransform>();

            spriteRenderer = gameObject.GetOrAddComponent<Image>();
            spriteOutline = gameObject.GetOrAddComponent<Outline_UI>();
            spriteCollider = gameObject.GetOrAddComponent<BoxCollider2D>();
            spriteCollider.isTrigger = true;
        }

        protected override void OnEnable()
        {
            //SetCamOffset(10);
            spriteCollider.enabled = spriteRenderer.enabled = true;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            spriteCollider.enabled = spriteRenderer.enabled = false;
            base.OnDisable();
        }


        public override sealed void TouchHandler_HoldDown()
        {
            Vector3 mousePos = Input.mousePosition;
            if (rootCanvas.renderMode == RenderMode.ScreenSpaceCamera) 
            {
                mousePos = mousePos.CanvasToWorldPos_ScreenSpaceRenderMode(targetCamera, rootCanvas);
            }
            else if (rootCanvas.renderMode == RenderMode.WorldSpace)
            {
                mousePos = mousePos.CanvasToWorldPos_WorldSpaceRenderMode(targetCamera, rootCanvas);
            }

            Collider2D hit = Physics2D.OverlapPoint(mousePos, spriteLayer.value);
            if (hit != null && hit.transform != null)
            {
                PaintOnSprite(mousePos);
#if UNITY_EDITOR
                Debug.DrawLine(targetCamera.transform.position, mousePos, penColor, Time.deltaTime);
#endif
            }
            else//커서가 타겟이미지 밖
            {
                prevDragPos = Vector2.zero;
            }
        }

        public override sealed void TouchHandler_Ended()
        {
            prevDragPos = Vector2.zero;
            SaveLastDraw();
        }

        public override sealed void TouchHandler_UpdateEnded()
        {
        }
    }
}