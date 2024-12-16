using UnityEngine;

namespace CWJ.UI
{
    //URP에서는 안될수있음
    //실행후 테스트하기
    [RequireComponent(typeof(SpriteRenderer))]
    public class Outline_SpriteRenderer : MonoBehaviour
    {
        private static Material defaultMaterial = null;

        public static Material DefaultMaterial
        {
            get
            {
                if (defaultMaterial == null)
                {
                    defaultMaterial = Resources.Load<Material>("Sprite-Outline"); //URP 에서는 불가능
                }
                return defaultMaterial;
            }
        }

        public Color color = Color.red;

        [HelpBox("Sprite 가장자리가 Transparent여야 외곽선이 그려짐")]
        [GetComponent] public SpriteRenderer spriteRenderer;

        private float outlineSize = 1.25f;

        [VisualizeProperty]
        public float OutlineSize
        {
            get => outlineSize;
            set
            {
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                spriteRenderer.GetPropertyBlock(mpb);
                mpb.SetFloat("_OutlineSize", value);
                mpb.SetColor("_OutlineColor", (enabled && value > 0) ? color : new Color(0, 0, 0, 0));
                spriteRenderer.SetPropertyBlock(mpb);
                outlineSize = value;
            }
        }

        private Material prevMat;

        private void Reset()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            if (spriteRenderer == null) return;
            prevMat = spriteRenderer.sharedMaterial;
            spriteRenderer.sharedMaterial = DefaultMaterial; //URP 에서는 불가능
            OutlineSize = outlineSize;
        }

        private void OnDisable()
        {
            spriteRenderer.sharedMaterial = prevMat;
        }
    }
}