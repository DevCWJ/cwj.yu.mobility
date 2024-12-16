using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace CWJ
{
    public static class UGUIUtil
    {
        public delegate bool TryParser<T>(string a, out T b);

        /// <summary>
        /// TMP_InputField.onValidateInput 사용할때 써야함
        /// </summary>
        /// <param name="ipf"></param>
        /// <param name="resultText"></param>
        /// <param name="pos"></param>
        /// <param name="addedChar"></param>
        /// <param name="inputNum"></param>
        /// <returns></returns>
        public static bool TmpIpf_ValidateAndTryConvert<T>(TMP_InputField ipf, TryParser<T> tryParse, ref string resultText, ref int pos, ref char addedChar, out T outputValue, Func<char, bool> validatorIgnoreCase = null)
        {
            TmpIpf_OnValidateInput(ipf, ref resultText, ref pos, ref addedChar, validatorIgnoreCase);
            return tryParse(resultText, out outputValue);
            //아래는 예시
            //xInput.onValidateInput -= OnXValueChanged;
            //char OnXValueChanged(string input, int charIndex, char addedChar)
            //{
            //    if (UGUIUtil.TmpIpf_ValidateAndTryConvert(xInput, float.TryParse, ref input, ref charIndex, ref addedChar, out float resultNum))
            //    {
            //        //Do Something
            //    }

            //    return addedChar;
            //}
        }

        public static void TmpIpf_OnValidateInput(TMP_InputField ipf, ref string resultText, ref int pos, ref char addedChar, Func<char, bool> validatorIgnoreCase = null)
        {
            Debug.Assert(ipf != null);

            if (ipf.inputValidator != null)
            {
                if (validatorIgnoreCase == null || !validatorIgnoreCase(addedChar))
                    addedChar = ipf.inputValidator.Validate(ref resultText, ref pos, addedChar);
            }
            else
            {
                resultText = resultText + addedChar;
                pos += 1;
            }

            ipf.SetTextWithoutNotify(resultText);
            ipf.selectionStringFocusPosition = ipf.selectionStringAnchorPosition = pos;
        }

        /// <summary>
        /// 간혹가다가 사용하는게 아니라면 직접 코드 작성하는게 성능상에서 유리 (ped와 results를 만들어놓는 차이)
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        public static RaycastResult[] GetPointerOverUIRayResults(this Vector2 mousePosition)
        {
            PointerEventData ped = new PointerEventData(EventSystem.current);

            ped.position = mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(ped, results);

            return results.ToArray();
        }

        public static bool FindLastRaycast(this List<RaycastResult> candidates, out int lastIndex, GameObject ignoreObj = null)
        {
            int cnt = candidates.Count;
            for (var i = cnt - 1; i >= 0; --i)
            {
                if (candidates[i].gameObject.IsNullOrMissing() || (ignoreObj != null && candidates[i].gameObject == ignoreObj))
                    continue;
                lastIndex = i;
                return true;
            }
            lastIndex = -1;
            return false;
        }


        /// <summary>
        /// availableTime : longPressEvent 시작 시간
        /// </summary>
        /// <param name="button"></param>
        /// <param name="unityAction"></param>
        /// <param name="availableTime"></param>
        public static void AddLongPressStartEvent<T>(this T button, UnityEngine.Events.UnityAction unityAction, float availableTime = 0.5f)
            where T : Selectable
        {
            var longPressEventSystem = button.targetGraphic.gameObject.GetOrAddComponent<CWJ.UI.LongPressEventSystem>();
            longPressEventSystem.ConstructorDown(button, unityAction, availableTime);
        }

        public static void RemoveLongPressStartEvent<T>(this T button, UnityEngine.Events.UnityAction unityAction)
            where T : Selectable
        {
            var longPressEventSystem = button.targetGraphic.GetComponent<CWJ.UI.LongPressEventSystem>();
            if (longPressEventSystem == null) return;
            longPressEventSystem.longPressStartEvent.RemoveListener_New(unityAction);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="unityAction"></param>
        /// <param name="availableTime"></param>
        /// <param name="loopInterval">0: Time.deltaTime</param>
        /// <param name="isOnClickEventSame"></param>
        public static void AddLongPressLoopEvent<T>(this T button, UnityEngine.Events.UnityAction unityAction, float availableTime = 0.5f, float loopInterval = 0, bool isOnClickEventSame = false)
            where T : Selectable
        {
            var longPressEventSystem = button.targetGraphic.gameObject.GetOrAddComponent<CWJ.UI.LongPressEventSystem>();
            longPressEventSystem.ConstructorLoop(button, unityAction, availableTime: availableTime, loopInterval: loopInterval);

            if (isOnClickEventSame)
            {
                if (button is Button)
                    (button as Button).onClick.AddListener_New(unityAction);
                else if (button is Toggle)
                    (button as Toggle).onValueChanged.AddListener_New((_) => unityAction.Invoke());

            }
        }

        public static void RemoveLongPressLoopEvent<T>(this T button, UnityEngine.Events.UnityAction unityAction)
                    where T : Selectable
        {
            var longPressEventSystem = button.targetGraphic.GetComponent<CWJ.UI.LongPressEventSystem>();
            if (longPressEventSystem == null) return;
            longPressEventSystem.longPressLoopEvent.RemoveListener_New(unityAction);
        }

        public static void AddShortPressUpEvent<T>(this T button, UnityEngine.Events.UnityAction unityAction, float availableTime = 0.5f, bool isOnClickEventSame = false)
                    where T : Selectable
        {
            var longPressEventSystem = button.targetGraphic.gameObject.GetOrAddComponent<CWJ.UI.LongPressEventSystem>();
            longPressEventSystem.ConstructorUp(button, unityAction, availableTime, false);

            if (isOnClickEventSame)
            {
                if (button is Button)
                    (button as Button).onClick.AddListener_New(unityAction);
                else if (button is Toggle)
                    (button as Toggle).onValueChanged.AddListener_New((_) => unityAction.Invoke());
            }
        }

        /// <summary>
        /// 꾹 눌렀다가 떼면 Invoke
        /// </summary>
        /// <param name="button"></param>
        /// <param name="unityAction"></param>
        /// <param name="availableTime"></param>
        public static void AddLongPressUpEvent<T>(this T button, UnityEngine.Events.UnityAction unityAction, float availableTime = 0.5f, bool isOnClickEventSame = false)
                    where T : Selectable
        {
            var longPressEventSystem = button.targetGraphic.gameObject.GetOrAddComponent<CWJ.UI.LongPressEventSystem>();
            longPressEventSystem.ConstructorUp(button, unityAction, availableTime, true);

            if (isOnClickEventSame)
            {
                if (button is Button)
                    (button as Button).onClick.AddListener_New(unityAction);
                else if (button is Toggle)
                    (button as Toggle).onValueChanged.AddListener_New((_) => unityAction.Invoke());
            }
        }

        public static void RemoveLongPressUpEvent<T>(this T button, UnityEngine.Events.UnityAction unityAction, bool isLongPressedAfter)
                    where T : Selectable
        {
            var longPressEventSystem = button.targetGraphic.GetComponent<CWJ.UI.LongPressEventSystem>();
            if (longPressEventSystem == null) return;
            if (isLongPressedAfter)
                longPressEventSystem.longPressedUpEvent.RemoveListener_New(unityAction);
            else
                longPressEventSystem.shortPressedUpEvent.RemoveListener_New(unityAction);
        }

        /// <summary>
        /// availableTime : doubleClickEvent 유효 시간
        /// </summary>
        /// <param name="button"></param>
        /// <param name="unityAction"></param>
        /// <param name="availableTime"></param>
        public static void AddDoubleClickEvent(this Button button, UnityEngine.Events.UnityAction unityAction, float availableTime = 0)
        {
            var doubleClickEventSystem = button.targetGraphic.gameObject.GetOrAddComponent<CWJ.UI.DoubleClickEventSystem>();
            doubleClickEventSystem.Constructor(button, unityAction, availableTime);
        }

        /// <summary>
        /// availableTime : doubleClickEvent 유효 시간
        /// </summary>
        /// <param name="button"></param>
        /// <param name="unityAction"></param>
        /// <param name="availableTime"></param>
        public static void RemoveDoubleClickEvent(this Button button, UnityEngine.Events.UnityAction unityAction)
        {
            var doubleClickEventSystem = button.targetGraphic.GetComponent<CWJ.UI.DoubleClickEventSystem>();
            if (doubleClickEventSystem == null) return;
            doubleClickEventSystem.doubleClickEvent.RemoveListener_New(unityAction);
        }

        public enum ElementAlign
        {
            Top,
            Middle,
            Bottom
        }
        /// <summary>
        /// 타겟이 보이도록 스크롤을 타겟 위치까지 이동시키는 기능
        /// </summary>
        /// <param name="canvasRectTrf"></param>
        /// <param name="scrollRect"></param>
        /// <param name="targetRectTrf"></param>
        /// <param name="elementAlign"></param>
        public static void ScrollMoveToElement(RectTransform canvasRectTrf, ScrollRect scrollRect, RectTransform targetRectTrf, ElementAlign elementAlign)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.velocity = Vector2.zero;

            Vector2 contentRectPos = canvasRectTrf.InverseTransformPoint(scrollRect.content.position);
            Vector2 targetRectPos = canvasRectTrf.InverseTransformPoint(targetRectTrf.position);

            float contentHeight = scrollRect.content.rect.height;
            float targetHeight = targetRectTrf.rect.height * 0.6f; //0.5 + 여백 0.1
            float viewHeight = scrollRect.viewport.rect.height;

            float offset = contentRectPos.y - (targetRectPos.y + targetHeight);

            float integral = contentHeight;

            if (elementAlign.Equals(ElementAlign.Top))
            {
                integral -= viewHeight;
            }
            else if (elementAlign.Equals(ElementAlign.Bottom))
            {
                integral -= viewHeight;
                offset -= viewHeight - (targetHeight * 2);
            }

            scrollRect.verticalNormalizedPosition = 1 - Mathf.Clamp01(offset / integral);

            Canvas.ForceUpdateCanvases();
        }
    }

    public enum RectAnchor
    {
        TOP_LEFT = 0,
        TOP_CENTER = 1,
        TOP_RIGHT = 2,
        MIDDLE_LEFT = 3,
        MIDDLE_CENTER = 4,
        MIDDLE_RIGHT = 5,
        BOTTOM_LEFT = 6,
        BOTTOM_CENTER = 7,
        BOTTOM_RIGHT = 8,
        STRETCH_TOP = 9,
        STRETCH_MIDDLE = 10,
        STRETCH_BOTTOM = 11,
        STRETCH_LEFT = 12,
        STRETCH_CENTER = 13,
        STRETCH_RIGHT = 14,
        STRETCH_FULL = 15
    }

    /// <summary>
    /// Provides extension methods for rect transform components.
    /// </summary>
    public static class RectTransformUtil
    {
        /// <summary>
        /// Represents values for a rect anchor setting.
        /// </summary>
        private struct RectSetting
        {
            /// <summary>
            /// The anchor's max values.
            /// </summary>
            public Vector2 anchorMax;

            /// <summary>
            /// The anchor's min values.
            /// </summary>
            public Vector2 anchorMin;

            /// <summary>
            /// The pivot values.
            /// </summary>
            public Vector2 pivot;

            /// <summary>
            /// Initializes the rectangle setting.
            /// </summary>
            /// <param name="xMin">The minimum x value.</param>
            /// <param name="xMax">The maximum x value.</param>
            /// <param name="yMin">The minimum y value.</param>
            /// <param name="yMax">The maximum y value.</param>
            /// <param name="xPivot">The pivot value on the x axis.</param>
            /// <param name="yPivot">The pivot value on the y axis.</param>
            public RectSetting(float xMin, float xMax, float yMin, float yMax, float xPivot, float yPivot)
            {
                anchorMax = new Vector2(xMax, yMax);
                anchorMin = new Vector2(xMin, yMin);
                pivot = new Vector2(xPivot, yPivot);
            }
        }

        /// <summary>
        /// Holds the preset values used for each anchor setting.
        /// </summary>
        private static readonly Dictionary<RectAnchor, RectSetting> _anchorPresets = new Dictionary<RectAnchor, RectSetting>
        {
            { RectAnchor.TOP_LEFT, new RectSetting( 0f, 0f, 1f,1f, 0f,1f )},
            { RectAnchor.TOP_CENTER, new RectSetting( 0.5f, 0.5f, 1f,1f,0.5f,1f )},
            { RectAnchor.TOP_RIGHT, new RectSetting( 1f,1f,1f,1f, 1f,1f )},
            { RectAnchor.MIDDLE_LEFT, new RectSetting( 0f,0f, 0.5f, 0.5f,0f,0.5f )},
            { RectAnchor.MIDDLE_CENTER, new RectSetting( 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f )},
            { RectAnchor.MIDDLE_RIGHT, new RectSetting( 1f,1f,0.5f,0.5f, 1f, 0.5f )},
            { RectAnchor.BOTTOM_LEFT, new RectSetting( 0f,0f,0f,0f, 0f, 0f )},
            { RectAnchor.BOTTOM_CENTER, new RectSetting( 0.5f, 0.5f, 0f, 0f, 0.5f, 0f )},
            { RectAnchor.BOTTOM_RIGHT, new RectSetting( 1f,1f,0f,0f, 1f, 0f )},
            { RectAnchor.STRETCH_TOP, new RectSetting( 0f, 1f, 1f,1f,0.5f,1f )},
            { RectAnchor.STRETCH_MIDDLE, new RectSetting( 0f,1f,0.5f,0.5f, 0.5f, 0.5f )},
            { RectAnchor.STRETCH_BOTTOM, new RectSetting( 0f,1f,0f,0f, 0.5f, 0f )},
            { RectAnchor.STRETCH_LEFT, new RectSetting( 0f,0f,0f,1f, 0f,0.5f )},
            { RectAnchor.STRETCH_CENTER, new RectSetting( 0.5f, 0.5f, 0f, 1f, 0.5f, 0.5f )},
            { RectAnchor.STRETCH_RIGHT, new RectSetting( 1f,1f, 0f, 1f, 1f, 0.5f )},
            { RectAnchor.STRETCH_FULL, new RectSetting( 0f, 1f, 0f, 1f, 0.5f, 0.5f )},
        };

        /// <summary>
        /// Sets the anchor values.
        /// </summary>
        /// <param name="rectTrf">The rectangle transform.</param>
        /// <param name="xMin">The minimum x value.</param>
        /// <param name="xMax">The maximum x value.</param>
        /// <param name="yMin">The minimum y value.</param>
        /// <param name="yMax">The maximum y value.</param>
        public static void SetMinMaxAnchor(RectTransform rectTrf, Vector2 min, Vector2 max)
        {
            if (rectTrf == null)
                throw new ArgumentNullException(nameof(rectTrf));

            rectTrf.anchorMin = min;
            rectTrf.anchorMax = max;
        }
        //public static void SetPivotWithoutMoving(RectTransform rectTransform, Vector2 newPivot)
        //{
        //    if (rectTransform == null)
        //        return;

        //    Vector2 size = rectTransform.rect.size;
        //    Vector2 deltaPivot = newPivot - rectTransform.pivot;
        //    Vector2 deltaPosition = new Vector2(deltaPivot.x * size.x, deltaPivot.y * size.y);

        //    rectTransform.pivot = newPivot;
        //    rectTransform.anchoredPosition += deltaPosition;
        //}
        public static void SetPivotWithoutMoving(this RectTransform rectTransform, Vector2 newPivot)
        {
            if (rectTransform == null)
                return;

            Vector2 size = rectTransform.rect.size;
            Vector2 deltaPivot = newPivot - rectTransform.pivot;
            Vector2 deltaPosition = new Vector2(deltaPivot.x * size.x, deltaPivot.y * size.y);

            rectTransform.pivot = newPivot;
            rectTransform.anchoredPosition += deltaPosition;
        }
        public static void SetMinMaxAnchorWithoutMoving(RectTransform rectTransform, Vector2 newAnchorMin, Vector2 newAnchorMax)
        {
            if (rectTransform == null)
                return;

            Vector3 worldPosition = rectTransform.position;

            rectTransform.anchorMin = newAnchorMin;
            rectTransform.anchorMax = newAnchorMax;

            rectTransform.position = worldPosition;
        }
        //public static void SetMinMaxAnchorWithoutMoving(RectTransform rectTransform, Vector2 newAnchorMin, Vector2 newAnchorMax)
        //{
        //    if (rectTransform == null)
        //        return;

        //    RectTransform parentRect = rectTransform.parent as RectTransform;
        //    if (parentRect == null)
        //        return;

        //    Vector2 parentPivot = parentRect.pivot;
        //    Vector2 parentSize = parentRect.rect.size;
        //    Vector2 deltaAnchorMin = newAnchorMin - rectTransform.anchorMin;
        //    Vector2 deltaAnchorMax = newAnchorMax - rectTransform.anchorMax;

        //    Vector2 offset = new Vector2(
        //        (deltaAnchorMin.x * parentSize.x * (1 - rectTransform.pivot.x)) + (deltaAnchorMax.x * parentSize.x * rectTransform.pivot.x),
        //        (deltaAnchorMin.y * parentSize.y * (1 - rectTransform.pivot.y)) + (deltaAnchorMax.y * parentSize.y * rectTransform.pivot.y)
        //    );

        //    rectTransform.anchorMin = newAnchorMin;
        //    rectTransform.anchorMax = newAnchorMax;
        //    rectTransform.anchoredPosition += offset;
        //}

        public static void SetAnchorWithoutMoving(this RectTransform rectTrf, RectAnchor anchor, bool setPivot = true, bool isStrectchAutoSize = false)
        {
            Debug.Assert(rectTrf != null);

            RectSetting setting = _anchorPresets[anchor];
            
            if (setPivot)
                SetPivotWithoutMoving(rectTrf, setting.pivot);

            SetMinMaxAnchorWithoutMoving(rectTrf, setting.anchorMin, setting.anchorMax);

            if (EnumUtil.ToInt(anchor) >= 9 && isStrectchAutoSize)
                rectTrf.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// Sets the anchor to a given setting.
        /// </summary>
        /// <param name="rectTrf">The rectangle transform.</param>
        /// <param name="anchor">The anchor setting to use.</param>
        /// <param name="setPivot">Whether the pivot should also be set based on the new setting.</param>
        /// <param name="setPosition">Whether to set the position after the setting has been applied.</param>
        public static void SetAnchor(
            this RectTransform rectTrf,
            RectAnchor anchor,
            bool setPivot = true,
            bool setPosition = true, bool isStrectchAutoSize = true)
        {
            Debug.Assert(rectTrf != null);

            RectSetting setting = _anchorPresets[anchor];
            SetMinMaxAnchor(rectTrf, setting.anchorMin, setting.anchorMax);

            if (setPivot)
                rectTrf.pivot = setting.pivot;

            if (setPosition)
                rectTrf.anchoredPosition = Vector2.zero;

            if (EnumUtil.ToInt(anchor) >= 9 && isStrectchAutoSize)
                rectTrf.sizeDelta = Vector2.zero;
        }

        /// <summary>
        /// Returns the world rectangle of a rectangle transform.
        /// </summary>
        /// <param name="rectTrf">The rectangle transform.</param>
        /// <returns>The world rectangle.</returns>
        public static Rect GetWorldRect(this RectTransform rectTrf)
        {
            if (rectTrf == null)
                throw new ArgumentNullException(nameof(rectTrf));

            Vector3[] corners = new Vector3[4];
            rectTrf.GetWorldCorners(corners);

            Vector3 bottomLeft = corners[0];

            Vector2 size = new Vector2(
                rectTrf.lossyScale.x * rectTrf.rect.size.x,
                rectTrf.lossyScale.y * rectTrf.rect.size.y);

            return new Rect(bottomLeft, size);
        }

        /// <summary>
        /// Returns the rectangle transform. Will return null if a normal transform is used.
        /// </summary>
        /// <param name="component">The component of which to get the rectangle transform.</param>
        /// <returns>The rectangle transform instance.</returns>
        public static RectTransform GetRectTransform<T>(this T component) where T : Component
        {
            Debug.Assert(component != null);

            return component.transform as RectTransform;
        }

        /// <summary>
        /// Returns the rectangle transform. Will return null if a normal transform is used.
        /// </summary>
        /// <param name="gameObject">The game object of which to get the rectangle transform.</param>
        /// <returns>The rectangle transform instance.</returns>
        public static RectTransform GetRectTransform(this GameObject gameObject)
        {
            Debug.Assert(gameObject != null);

            return gameObject.transform as RectTransform;
        }
    }
}
