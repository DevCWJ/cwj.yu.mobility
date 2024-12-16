using System;

using UnityEngine;
using CWJ.AccessibleEditor;

namespace CWJ
{
	/// <summary>
	/// <para>[HideInInspector] worked according to condition</para>
	/// <paramref name="predicateName"/> 라는 이름의 변수혹은 함수의 반환 bool값이 <paramref name="forPredicateComparison"/>(default:false) 와 같을 시 숨겨짐
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public abstract class _VisualizeConditionalAttribute : _Root_ConditionalAttribute
	{
		public _VisualizeConditionalAttribute(EPlayMode hidingSituation, string predicateName, bool forPredicateComparison) : base(hidingSituation, predicateName, forPredicateComparison) { }

		public _VisualizeConditionalAttribute(EPlayMode hidingSituation, bool forPredicateComparison) : base(hidingSituation, null, forPredicateComparison) { }
	}

	public class ShowConditionalAttribute : _VisualizeConditionalAttribute
	{
		/// <summary>
		/// <see langword="predicateName"/>이 true일때 보여지고, false일 경우 숨겨짐
		/// </summary>
		/// <param name="predicateName"></param>
		/// <param name="showPlayMode"></param>
		public ShowConditionalAttribute(string predicateName, EPlayMode showPlayMode = EPlayMode.Always) : base(showPlayMode, predicateName, true) { }
		public ShowConditionalAttribute(EPlayMode showPlayMode) : base(showPlayMode, true) { }
	}

    public class HideConditionalAttribute : _VisualizeConditionalAttribute
    {
		/// <summary>
		/// <see langword="predicateName"/>이 true일때 숨겨지고, false일 경우 보여짐
		/// </summary>
		/// <param name="predicateName"></param>
		/// <param name="hidePlayMode"></param>
		public HideConditionalAttribute(string predicateName, EPlayMode hidePlayMode = EPlayMode.Always) : base(hidePlayMode, predicateName, false) { }
		public HideConditionalAttribute(EPlayMode hidePlayMode) : base(hidePlayMode, false) { }
	}

}
