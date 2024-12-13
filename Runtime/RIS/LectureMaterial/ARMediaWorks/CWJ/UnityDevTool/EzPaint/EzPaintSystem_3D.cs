using UnityEngine;

namespace CWJ.EzPaint
{
    public sealed class EzPaintSystem_3D : EzPaintSystem
    {
        protected override sealed bool is3DPaint => true;

        [SerializeField, Readonly] private SpriteRenderer spriteRenderer = null;
        [SerializeField, Readonly] private BoxCollider spriteCollider = null;
        [SerializeField, Readonly] private SpriteRenderer spriteOutline = null;

        /// <summary>
        /// Sprite 설정
        /// 이거만해도 자동으로 Outline은 재설정됨
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="isReset"></param>
        public override sealed void SetSpriteRenderer(Sprite sprite, bool isReset)
        {
            if (sprite == null)
            {
                return;
            }
            spriteRenderer.sprite = sprite;

            Vector2 spriteSize = Vector2.Scale(sprite.rect.size, Vector2.one * .01f);
            spriteCollider.size = new Vector3(spriteSize.x, spriteSize.y, .01f);
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
            spriteOutline.sprite = sprite;
            spriteOutline.drawMode = SpriteDrawMode.Sliced;
            Vector2 size = spriteRenderer.size;
            spriteOutline.size = new Vector2(size.x + width, size.y + width);
        }

        protected override void SetSpriteOutlineColor(Color color)
        {
            spriteOutlineColor = color;
            spriteOutline.color = color;
        }

        protected override sealed void OutlineSetActive(bool enabled)
        {
            spriteOutline.gameObject.SetActive(enabled);
        }

        protected override sealed void _Initialized()
        {
            targetCamera = Camera.main;
            if (targetCamera == null) targetCamera = FindObjectOfType<Camera>();
            SetCamOffset(10);
            spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();
            spriteRenderer.drawMode = SpriteDrawMode.Simple;
            spriteRenderer.sortingOrder = 1; //sorting order 디폴트 1

            spriteCollider = gameObject.GetOrAddComponent<BoxCollider>();
            spriteCollider.isTrigger = true;

            Transform outlineTrf = transform.Find("OutlineObj");
            if (outlineTrf == null) outlineTrf = new GameObject("OutlineObj", typeof(SpriteRenderer)).transform;
            spriteOutline = outlineTrf.GetOrAddComponent_New<SpriteRenderer>();
            spriteOutline.transform.SetParent(transform);
            spriteOutline.transform.Reset();
            spriteOutline.transform.localPosition = new Vector3(0, 0, .002f);
        }

        [InvokeButton]
        public void SetCamOffset(float offset)
        {
            transform.localPosition = transform.parent.WorldToLocalPosition(targetCamera.transform.position + (targetCamera.transform.forward * offset));
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
            Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, targetCamera.farClipPlane+1, spriteLayer))
            {
                PaintOnSprite(hit.point);
#if UNITY_EDITOR
                Debug.DrawLine(ray.origin, hit.point, penColor, Time.deltaTime);
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