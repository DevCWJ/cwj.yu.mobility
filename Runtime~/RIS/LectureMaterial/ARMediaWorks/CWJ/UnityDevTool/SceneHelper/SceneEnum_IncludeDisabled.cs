namespace CWJ.SceneHelper
{
	/// <summary>
	/// 활성화된 씬만 존재.
	/// <para><see langword="1. How To Use"/> : <see langword="string"/>으로 변환 할 때 <see cref="SceneEnumUtil.ToSceneName"/> 을 사용할것.</para>
	/// <para>(<see cref="SceneEnum_IncludeDisabled"/>에 저장될때 공백' '은 'ˇ'로 대체, hyphen'-'은 'ㅡ'로 대체, 첫 글자가 숫자일땐 'ⁿ'가 붙는 등 이름이 자동으로 수정되어 적용됨)</para>
	/// <para><see langword="2. Scene Naming Rule"/> : <see cref="SceneEnumUtil.ToSceneName"/> 을 사용하면 되지만 그래도 특수문자나 공백은 Scene이름으로 사용을 지양.</para>
	/// <para><see langword="3. Update Condition"/> : Build Settings에서 씬 목록 수정된 후 <see langword="and"/> (5초 후 <see langword="or"/> ctrl+s 누른 직후 <see langword="or"/> compile 직후 <see langword="or"/> 빌드 직전) 자동 업데이트.</para>
	/// <para><see langword="4. How To Disable"/> : Enum이 Scene과 동기화되는 기능을 끄려면 'CWJ/Custom Define symbol setting' 에서 'isSceneEnumSync' 비활성화.</para>
	///CWJ - UnityDevTool (Version 10.7 [20.01.31])
	/// </summary>
	public enum SceneEnum
	{ 
		YU_Demo
	}

	/// <summary>
	/// 비활성화된 씬도 포함
	/// </summary>
	public class SceneEnum_IncludeDisabled
	{
		/// <summary>
		/// 활성화 and 비활성화 씬 존재.
		/// <para>Build Settings의 순서와 동일하게 정렬.</para>
		/// <para><see langword="string"/>으로 변환 할 때 <see cref="SceneEnumUtil.ToSceneName"/> 을 사용할것.</para>
		/// </summary>
		public enum SceneEnum_BuildSettings
		{ 
			YU_Demo,
			FbxAutoSetupTest,
			GrabSpinThrow,
			Demoscene
		}

		/// <summary>
		/// 활성화 and 비활성화 씬 존재.
		/// <para>sceneBuildIndex를 기준으로 정렬.(활성화된 씬부터 기입되어있음)</para>
		/// <para><see langword="string"/>으로 변환 할 때 <see cref="SceneEnumUtil.ToSceneName"/> 을 사용할것.</para>
		/// </summary>
		public enum SceneEnum_BuildIndex
		{ 
			YU_Demo,
			FbxAutoSetupTest,
			GrabSpinThrow,
			Demoscene
		}

		/// <summary>
		/// 비활성화된 씬만 존재.
		/// <para><see langword="string"/>으로 변환 할 때 <see cref="SceneEnumUtil.ToSceneName"/> 을 사용할것.</para>
		/// </summary>
		public enum SceneEnum_Disabled
		{ 
			FbxAutoSetupTest,
			GrabSpinThrow,
			Demoscene
		}

	}

}

