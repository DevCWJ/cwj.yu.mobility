using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace CWJ.UI
{
    public class Outline_UI : BaseMeshEffect
    {
        [SerializeField]
        private Color _effectColor = new Color(0f, 0f, 0f, 0.5f);

        public Color effectColor
        {
            get
            {
                return _effectColor;
            }
            set
            {
                if (this._effectColor == value)
                {
                    return;
                }
                this._effectColor = value;
                base.graphic?.SetVerticesDirty();
            }
        }

        [SerializeField]
        private Vector2 _effectDistance = new Vector2(1f, -1f);

        public Vector2 effectDistance
        {
            get
            {
                return this._effectDistance;
            }
            set
            {
                if (value.x > 600f)
                {
                    value.x = 600f;
                }
                if (value.x < -600f)
                {
                    value.x = -600f;
                }
                if (value.y > 600f)
                {
                    value.y = 600f;
                }
                if (value.y < -600f)
                {
                    value.y = -600f;
                }
                if (this._effectDistance == value)
                {
                    return;
                }
                this._effectDistance = value;
                base.graphic?.SetVerticesDirty();
            }
        }

        [SerializeField]
        private bool _useGraphicAlpha = true;

        public bool useGraphicAlpha
        {
            get
            {
                return this._useGraphicAlpha;
            }
            set
            {
                if (this._useGraphicAlpha == value)
                {
                    return;
                }
                this._useGraphicAlpha = value;
                base.graphic?.SetVerticesDirty();
            }
        }

        private List<UIVertex> vertexList = new List<UIVertex>();

        protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            UIVertex vt;

            var neededCpacity = verts.Count * 2;
            if (verts.Capacity < neededCpacity)
                verts.Capacity = neededCpacity;

            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                verts.Add(vt);

                Vector3 v = vt.position;
                v.x += x;
                v.y += y;
                vt.position = v;
                var newColor = color;
                if (_useGraphicAlpha)
                    newColor.a = (byte)((newColor.a * verts[i].color.a) / 255);
                vt.color = newColor;
                verts[i] = vt;
            }
        }

        protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            var neededCpacity = verts.Count * 2;
            if (verts.Capacity < neededCpacity)
                verts.Capacity = neededCpacity;

            ApplyShadowZeroAlloc(verts, color, start, end, x, y);
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!this.IsActive())
            {
                return;
            }

            vertexList.Clear();
            vh.GetUIVertexStream(vertexList);

            Text foundtext = GetComponent<Text>();

            float best_fit_adjustment = 1f;

            if (foundtext && foundtext.resizeTextForBestFit)
            {
                best_fit_adjustment = (float)foundtext.cachedTextGenerator.fontSizeUsedForBestFit / (foundtext.resizeTextMaxSize - 1);
            }

            float distanceX = this.effectDistance.x * best_fit_adjustment;
            float distanceY = this.effectDistance.y * best_fit_adjustment;

            int start = 0;
            int count = vertexList.Count;
            this.ApplyShadow(vertexList, this.effectColor, start, vertexList.Count, distanceX, distanceY);
            start = count;
            count = vertexList.Count;
            this.ApplyShadow(vertexList, this.effectColor, start, vertexList.Count, distanceX, -distanceY);
            start = count;
            count = vertexList.Count;
            this.ApplyShadow(vertexList, this.effectColor, start, vertexList.Count, -distanceX, distanceY);
            start = count;
            count = vertexList.Count;
            this.ApplyShadow(vertexList, this.effectColor, start, vertexList.Count, -distanceX, -distanceY);

            start = count;
            count = vertexList.Count;
            this.ApplyShadow(vertexList, this.effectColor, start, vertexList.Count, distanceX, 0);
            start = count;
            count = vertexList.Count;
            this.ApplyShadow(vertexList, this.effectColor, start, vertexList.Count, -distanceX, 0);

            start = count;
            count = vertexList.Count;
            this.ApplyShadow(vertexList, this.effectColor, start, vertexList.Count, 0, distanceY);
            start = count;
            count = vertexList.Count;
            this.ApplyShadow(vertexList, this.effectColor, start, vertexList.Count, 0, -distanceY);

            vh.Clear();
            vh.AddUIVertexTriangleStream(vertexList);
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            this.effectDistance = this._effectDistance;
            this.effectColor = this._effectColor;
            this.useGraphicAlpha = this._useGraphicAlpha;

            base.OnValidate();
        }

#endif
    }
}