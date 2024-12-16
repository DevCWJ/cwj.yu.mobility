using System;

namespace CWJ
{
    /// <summary>
    /// Inspector에 함수를 실행하는 버튼을 만들어줌
    /// <para>void 함수, Coroutine, 리턴타입(ValueType, Tuple, class, Struct) 함수, 선택적 매개변수를 가진 함수 등 테스트완료.</para>
    /// <br/>주의사항 : 매개변수에 <see langword="abstract"/> class가 올 순 없습니다.
    /// <br/>매개변수에 class를 넣을땐 해당 class에는 빈 생성자를 선언해둘것
    /// <br/>(추가적으로 안되는 경우가 있을시 연락바람)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
    public class InvokeButtonAttribute : Attribute
    {
        public readonly bool isNeedUndoNSave;
        public readonly bool isEmphasizeBtn;
        public readonly string onMarkedBoolName;
        public readonly bool isOnlyButton;
        public readonly string displayName;
        public readonly string tooltip;
        public readonly string aboveFieldName;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="onMarkedBoolName">이 이름을 가진 bool변수(field/property)를 true로 만듬 (코드를 통해 실행되었을때와 InvokeButtonAttribute를 통해 실행되었을때에 대한 차이점을 만들기위해)</param>
        /// <param name="isNeedUndoNSave">버튼누르면 저장됨 (Undo에도 등록됨)</param>
        /// <param name="isOnlyButton"><see langword="true"/>: foldout으로 숨겨져있지않고 버튼만 표시함.(매개변수가 없어야함)</param>
        /// <param name="isEmphasizeBtn">버튼강조 (Invoke Button 그룹에서 나와잇음)</param>
        /// <param name="displayName"></param>
        /// <param name="tooltip"></param>
        /// <param name="aboveFieldName"></param>
        public InvokeButtonAttribute(bool isNeedUndoNSave = false, string onMarkedBoolName = null, bool isOnlyButton = true, bool isEmphasizeBtn = false, string displayName = null, string tooltip = null, string aboveFieldName = null)
        {
            this.isNeedUndoNSave = isNeedUndoNSave;
            this.onMarkedBoolName = onMarkedBoolName;
            this.isOnlyButton = isOnlyButton;
            this.displayName = displayName;
            this.tooltip = tooltip;
            this.aboveFieldName = aboveFieldName;
            this.isEmphasizeBtn = isEmphasizeBtn;
        }
    }
}
