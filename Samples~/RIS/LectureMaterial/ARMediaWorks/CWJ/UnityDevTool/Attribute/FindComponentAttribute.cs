using System;
using UnityEngine;

namespace CWJ
{

    //TODO:부모클래스에서 사용한경우 안되었던거같음 테스트필요

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class _Root_FindCompAttribute : Attribute
    {
        public readonly bool isIncludeInactive;
        public readonly bool isFindIncludeMe;
        public readonly bool isFindOnlyWhenNull;
        public readonly string predicateName;
        public readonly string assignedCallbackName; //지금은 MustRequiredCompAttribute에서만 있음. 

        public _Root_FindCompAttribute(bool isIncludeInactive, bool isFindIncludeMe, bool isFindOnlyWhenNull, string predicateName, string assignedCallbackName = null)
        {
            this.isIncludeInactive = isIncludeInactive;
            this.isFindIncludeMe = isFindIncludeMe;
            this.isFindOnlyWhenNull = isFindOnlyWhenNull;
            this.predicateName = predicateName;
            this.assignedCallbackName = assignedCallbackName;
        }
    }

    /// <summary>
    /// <see langword="GetComponent, GetComponents"/>(배열일 경우)의 기능과 같음
    /// <para/><see langword="Field"/>의 <see cref="Type"/>과 같은 <see cref="Component"/>들을 캐싱함.
    /// <br/>Inspector에 보이지 않거나 Runtime중에는 작동하지않음(동적으로 생성되는 오브젝트에는 미리 할당해놓는 경우에만 쓸것)
    /// <para/> <see cref="isFindOnlyWhenNull"/> : <see langword="null"/> 일 때 만 찾을것인지?
    /// <para/> <see cref="isIncludeInactive"/> : 비활성화 오브젝트를 포함하여 찾을것인지?
    /// <para/> <see cref="predicateName"/> : 조건부 메소드(매개변수: <seealso cref="UnityEngine.Object"/>, 반환값: <seealso cref="bool"/>)의 이름을 넣으면 해당 조건의 Component만 찾음
    /// </summary>
    public class GetComponentAttribute : _Root_FindCompAttribute
    {
        public GetComponentAttribute(bool isIncludeInactive = true, bool isFindOnlyWhenNull = true, string predicateName = null, string assignedCallbackName = null) : base(isIncludeInactive: isIncludeInactive, isFindIncludeMe: true, isFindOnlyWhenNull: isFindOnlyWhenNull, predicateName, assignedCallbackName) { }
    }

    /// <summary>
    /// <see langword="GetComponentInChildren, GetComponentsInChildren"/>(배열일 경우)의 기능과 같음
    /// <para/><see langword="Field"/>의 <see cref="Type"/>과 같은 <see cref="Component"/>들을 캐싱함.
    /// <br/>Inspector에 보이지 않거나 Runtime중에는 작동하지않음(동적으로 생성되는 오브젝트에는 미리 할당해놓는 경우에만 쓸것)
    /// <para/> <see cref="isFindOnlyWhenNull"/> : <see langword="null"/> 일 때 만 찾을것인지?
    /// <para/> <see cref="isIncludeInactive"/> : 비활성화 오브젝트를 포함하여 찾을것인지?
    /// <para/> <see cref="predicateName"/> : 조건부 메소드(매개변수: <seealso cref="UnityEngine.Object"/>, 반환값: <seealso cref="bool"/>)의 이름을 넣으면 해당 조건의 Component만 찾음
    /// </summary>
    public class GetComponentInChildrenAttribute : _Root_FindCompAttribute
    {
        public GetComponentInChildrenAttribute(bool isIncludeInactive = false, bool isFindIncludeMe = false, bool isFindOnlyWhenNull = true, string predicateName = null, string assignedCallbackName = null) : base(isIncludeInactive: isIncludeInactive, isFindIncludeMe: isFindIncludeMe, isFindOnlyWhenNull: isFindOnlyWhenNull, predicateName: predicateName, assignedCallbackName: assignedCallbackName) { }
    }

    /// <summary>
    /// <see langword="GetComponentInParent, GetComponentsInParent"/>(배열일 경우)의 기능과 같음
    /// <para/><see langword="Field"/>의 <see cref="Type"/>과 같은 <see cref="Component"/>들을 캐싱함.
    /// <br/>Inspector에 보이지 않거나 Runtime중에는 작동하지않음(동적으로 생성되는 오브젝트에는 미리 할당해놓는 경우에만 쓸것)
    /// <para/> <see cref="isFindOnlyWhenNull"/> : <see langword="null"/> 일 때 만 찾을것인지?
    /// <para/> <see cref="isIncludeInactive"/> : 비활성화 오브젝트를 포함하여 찾을것인지?
    /// <para/> <see cref="predicateName"/> : 조건부 메소드(매개변수: <seealso cref="UnityEngine.Object"/>, 반환값: <seealso cref="bool"/>)의 이름을 넣으면 해당 조건의 Component만 찾음
    /// </summary>
    public class GetComponentInParentAttribute : _Root_FindCompAttribute
    {
        public GetComponentInParentAttribute(bool isIncludeInactive = false, bool isFindIncludeMe = false, bool isFindOnlyWhenNull = true, string predicateName = null, string assignedCallbackName = null) : base(isIncludeInactive: isIncludeInactive, isFindIncludeMe: isFindIncludeMe, isFindOnlyWhenNull: isFindOnlyWhenNull, predicateName: predicateName, assignedCallbackName: assignedCallbackName) { }
    }

    /// <summary>
    /// <see langword="FindObjectOfType_New, FindObjectsOfType_New"/>(배열일 경우)의 기능과 같음
    /// <para/><see langword="Field"/>의 <see cref="Type"/>과 같은 <see cref="Component"/>들을 캐싱함.
    /// <br/>Inspector에 보이지 않거나 Runtime중에는 작동하지않음(동적으로 생성되는 오브젝트에는 미리 할당해놓는 경우에만 쓸것)
    /// <para/> <see cref="isFindOnlyWhenNull"/> : <see langword="null"/> 일 때 만 찾을것인지?
    /// <para/> <see cref="isIncludeInactive"/> : 비활성화 오브젝트를 포함하여 찾을것인지?
    /// <para/> <see cref="predicateName"/> : 조건부 메소드(매개변수: <seealso cref="UnityEngine.Object"/>, 반환값: <seealso cref="bool"/>)의 이름을 넣으면 해당 조건의 Component만 찾음
    /// </summary>
    public class FindObjectAttribute : _Root_FindCompAttribute
    {
        public FindObjectAttribute(bool isIncludeInactive = false, bool isFindOnlyWhenNull = true, string predicateName = null, string assignedCallbackName = null) : base(isIncludeInactive: isIncludeInactive, isFindIncludeMe: true, isFindOnlyWhenNull: isFindOnlyWhenNull, predicateName: predicateName, assignedCallbackName: assignedCallbackName) { }
    }

    /// <summary>
    /// 무조건 값을 넣어줌. (GetComponent == <see langword="null"/> 이면 추가해서라도 Component를 넣어줌)
    /// <para/>UnityEngine의 RequireComponent의 선택적 상위호환버전
    /// </summary>
    public class MustRequiredCompAttribute : _Root_FindCompAttribute
    {
        /// <summary>
        /// AddComponent 될때 실행하고싶은 함수이름 적으면 실행해줌
        /// </summary>
        /// <param name="assignEventName"></param>
        public MustRequiredCompAttribute(bool isIncludeInactive = true, bool isFindOnlyWhenNull = true, string predicateName = null, string assignedCallbackName = null) : base(isIncludeInactive: isIncludeInactive, isFindIncludeMe: true, isFindOnlyWhenNull: isFindOnlyWhenNull, predicateName: predicateName, assignedCallbackName: assignedCallbackName) { }
    }
}