using System;
using System.Collections;
using System.Collections.Generic;

using CWJ.AccessibleEditor;

using UnityEngine;

namespace CWJ
{
    // TODO: 구조체에 쓰면 foldout 기능이 적용안됨

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class Root_ReadonlyAttribute : _Root_ConditionalAttribute
    {
        public Root_ReadonlyAttribute(EPlayMode readonlySituation, string predicateName, bool forPredicateComparison) : base(readonlySituation, predicateName, forPredicateComparison) { }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ReadonlyAttribute : Root_ReadonlyAttribute
    {
        public ReadonlyAttribute() : base(readonlySituation: EPlayMode.Always, predicateName: null, forPredicateComparison: false) { }
    }

    /// <summary>
    /// <paramref name="predicateName"/> 라는 이름의 변수혹은 함수의 반환 bool값이 <paramref name="forPredicateComparison"/>(default:true) 와 같을 시 Readonly처리
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ReadonlyConditionalAttribute : Root_ReadonlyAttribute
    {
        public ReadonlyConditionalAttribute(string predicateName, bool forPredicateComparison = true, EPlayMode readonlySituation = EPlayMode.Always) : base(readonlySituation: readonlySituation, predicateName: predicateName, forPredicateComparison: forPredicateComparison) { }

        public ReadonlyConditionalAttribute(EPlayMode readonlySituation) : base(readonlySituation: readonlySituation, predicateName: null, forPredicateComparison: false) { }
    }

    /// <summary>
    /// 중복된 어트리뷰트 사용시 BeginReadonlyGroup를 해도 비활성화 적용이 되지않는경우가 있는데 그럴땐
    /// 적용이 안된 field/ property에 ReadonlyAttribute를 추가하도록.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class BeginReadonlyGroupAttribute : PropertyAttribute
    {
        public readonly EPlayMode callSituation;

        public BeginReadonlyGroupAttribute(EPlayMode readonlySituation = EPlayMode.Always)
        {
            this.callSituation = readonlySituation;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class EndReadonlyGroupAttribute : PropertyAttribute { }
}