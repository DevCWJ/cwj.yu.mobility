#	UnityDevTool Changelog

Copyright 2019. CWJ All rights reserved

Contact : <cwj@kakao.com> (조우정)

## BUG

1.  __Unity2019의 UGUI 컴포넌트들과 Unity2020의 UGUI컴포넌트들은 서로 호환이 안됨 (Unity버그지만 내 컴포넌트에 영향이 가므로 Bug처리)__
    - Unity2020에서 PolygonChartManager 생성에 문제가 있음
2.  PolygonChartManager 생성할때 아무 Canvas에나 만들어짐 (DontDestroyOnLoad의 Canvas는 피하도록 수정)

##	TODO (importance priority)

0.  VisualizeProperty, VisualizeField 를 통해 그려질때 isFindAllBaseClass가 true 일 경우 변수이름앞에 base. / this. 로 구분

1.	__CWJ_Inspector_Core.cs 완성__
    - CWJ_Inspector_Core_Cache, FieldCache 완성
      - 이는 외형을 직접 그려주는 Attribute가 아닌 ReadonlyAttribute_Editor, HideInConditionalAttribute_Editor, SyncValueAttribute_Editor, OnValueChangedAttribute_Editor,
	등과 같은 상태 attribute의 경우엔 PropertyDrawer를 없애고 FieldCache에서 실시간으로 상태를 판단해주게함
    - DrawPropAction에 미리 그릴 함수를 캐싱
      - 직접 그려주는 어트리뷰트의 경우엔 해당 PropertyDrawer를 가져와서 캐시해놓고 사용하도록 수정하기.
	그렇게 최종적으로는 EditorGUI_CWJ.PropertyField_New를 없에는게 목표임.
	더 나아가서는 CWJInspector_VisualizeField, CWJInspector_VisualizeProperty 또한 비슷한방식을 거쳐서 그려지도록
	(현재의 CWJInspector_BodyAndFoldout.PropCache 와 기능비슷함)
    - Array/List, Class/Struct의 경우 Element를 그릴때 FieldInfo로 그리는데 SerializedProperty를 갖고있는 경우 element부터는 SerializedProperty가 그리게하자...

2.	~~__CWJInspector_ElementAbstract.isForciblyGetMembers 기능 완성__~~
    - ~~설명: 사용자가 CWJInspector_VisualizeProperty foldout을 열었을때 이미 isForciblyGetMembers을 활성화 한 경우가 아니라면 가장 밑에 isForciblyGetMembers 버튼이 나오고
	그 버튼을 누를경우 해당 클래스의 모든 property를 보여줌~~
    - __*완료*__

3.	__CWJInspector_ElementAbstract 들 Dispose 구현__
    - isForciblyGetMembers 기능때문에 컴파일되지않더라도 CWJInspector_ElementAbstract들이 Dispose되어야 하는 경우가 생김

4.	__WarningThatNonSerialized 제거__
    - 2까지 완료되면 isForciblyGetMembers를 활성화 할 때에만 경고 팝업 띄우면 될듯

5.	__RuntimeLogSetting 완성__
	- CWJ Special Foldout Group에서는 DebugSetting foldout을 그냥 버튼식으로 보여주면 될듯
	define 으로 빌드에 영향을 주는 DebugSettingWindow와 컴파일시키지 않아도 Log활성화 여부를 선택적으로 설정할수있는 RuntimeLogSetting 두가지를 Foldout에 버튼으로 보여주면되고
	완성시켜야할것은 RuntimeLogSetting.
	RuntimeLogSetting안에서 log를 ignore시킬 Component 타입 혹은 특정 오브젝트를 추가/삭제/관리 할수있으며
	추가되어있는 Comp타입 혹은 오브젝트의 인스펙터에선 log가 ignore되어있음을 확인할수있음 (고도화요소로는 Hierachy에서 ignore시킬 오브젝트를 관리할수있게(visibable이랑 selectable 결정할수있는거처럼))

6.	__VrDevTool UnityEngine.XR로 전환__

7.	__Custom Transform GUI__
    - position, rotation, scale 각 Vector3 값을 복사 / 붙여넣기 할수있게

8.  ~~Multiple Objects Edit : Array/List~~
    - ~~20.1.0에서 복수선택시 inspector multiple edit이 가능하게끔 수정했으나
    복수선택후 drag and drop으로 값을 추가 할 시 선택된 오브젝트의 배열들이 모두 같은값이 되는 문제를 해결해야함~~
	__*아 ㅋㅋ 유니티 builtIn array drawer에도 똑같은 현상 발생됨 그대로 놔둬도 될듯*__

9.  Array, List Drawer for SerializedProperty
    - while(index > arrayProp.arraySize)
        arrayProp.InsertArrayElementAtIndex(arrayProp.arraySize);
    while(index < arrayProp.arraySize)
        arrayProp.DeleteArrayElementAtIndex(arrayProp.arraySize-1);
와
	SerializedProperty elemProp = arrayProp.GetArrayElementAtIndex(i);
    EditorGUI.PropertyField(position, elemProp, GUIContent.none);
를 이용해서 Array CWJ_Inspector_BodyAndFoldout 241에 SerializedProperty용 array/Listdarwer 만들기

10. DrawClassOrStructType도 Undo지원하도록 수정


11. HideConditional 이렇게
    ```
    public enum EConditionOperator
	{
		And,
		Or
	}
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class HideIfAttribute : ShowIfAttributeBase
	{
		public HideIfAttribute(string condition) : base(condition)
		{
			Inverted = true;
		}

		public HideIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
			: base(conditionOperator, conditions)
		{
			Inverted = true;
		}

		public HideIfAttribute(string enumName, object enumValue)
			: base(enumName, enumValue as Enum)
		{
			Inverted = true;
		}
	}
	```

##	Version
## 21.1.4 (23.05.16)
### Changed
 - Update InvokeButton's param (isNeedUndoNSave)

##  21.1.3 (23.02.23)
### Chagned
 - Add VisualizeField, VisualizeProperty, InvokeButton use null Class/Struct/Tuple

##  21.1.2 (23.02.17)
### Added
 - Add DisposableMonoBehaviour

##  21.1.1 (23.02.08)
### Added
 - Add MqttModule (use UniTask, MqttNet)
 - Add DisplayScriptableObjectDrawer
### Changed
 - Supported Unity2019 (2019~2021)

##  20.4.3 (23.01.18)
### Added
 - Add DebugLogWriter.AddSystemLog
### Changed
 - Supported 2021.3.x or lower

##  20.4.2 (23.01.12)
### Changed
 - Supported 2021.3.x or lower

##  20.4.1 (22.12.13)

##  20.4.0 (22.10.26)
### Added
 - Add UnityEvent Function [Copy/Paste/Get List/Set List]
### Changed
 - Fixed Exception Log Print System in InvokeButtonAttribute

##  20.3.1 (22.02.09)

### Changed
 - Fixed Bug in HideConditionalAttribute

##  20.3.0 (22.01.27)

### Added
- Add DictionaryVisualized

### Changed
- Fixed Bug and optimization
- MissingCompRestore is useful!

##  20.2.2 (21.11.18)

### Added
- Add StringPopAttribute

### Changed
- Fixed Bug and optimization
- MissingCompRestore is useful!

##  20.2.1 (21.10.20)

### Changed
- Refactoring EditorGUI_CWJ, PropertyDrawer_CWJ
- Optimization Editor (Attribute, Inspector .. etc)

##  20.2.0 (21.10.17)

### Added
- Add __EzDissolve__
- Add __MissingComponentRestore__
- Add CoroutineTracked (Execution status is visualized in the inspector, and it prevents duplicate execution,
It also can be used in editor-not_runtime state)

### Changed
- Fixed bug in Inspector, Attributes
- By adding 'PropertyDrawer_CWJ' and changing the order value of Visualization-related PropertyAttributes, __So Drawer-Attribute and State-Attribute can be used together.__


##  20.1.7 (21.07.21)

### Changed
- __Scriptable object caching system__

##  20.1.6 (21.03.09)

### Added
- Add ICanCreateInEditorMode Interface in SingletonCore
### Changed
- __ScrollCaptureManager supported all of resolutions__
***


##  20.1.5 (21.02.10)

### Added
- __Add "Visualize All Property, Visualize All Field, Convert All Method to Button"__
- Remove Unnecessary stack trace in log (InvokeButton's log,, etc)
***

##  20.1.4 (21.01.21)

### Added
- Add MeshAssist Scripts (UnityDevTool/Function/MeshAssist -> MeshCombinerWindow, MeshColliderMerge)
###	Changed
- Fixed Array/List/Dictionary ToDetailedString (Affect: InvokeButton)
***


##  20.1.3 (21.01.14)

### Added
- InputClickManager
- Serialized Dicionary for unity2019, unity2020
***

##  20.1.1 (20.10.13)

###	Changed
- Fixed ScrollCaptureManager cut-off bug
- Optimization ScrollCaptureManager
***

##  20.1.0 (20.09.11)

### Added
- Hierachy Comment Function
  - Hierarchy에서 연필모양 버튼을 클릭하면 주석을 등록/수정/제거 할수있는 팝업창이 나옵니다.
- Transform Custom GUI
  - Transform의 world space 정보도 함께 Inspector에 표시합니다.
- Added RequiredComponent Attribute
  - 선언된 Field의 Component가 없으면 자동으로 AddComponent를 합니다
  - 클래스 위에 선언하는 Unity의 [RequireComponent]와는 다르게  Field에 직접 선언해야합니다 (spelling도 다름).

###	Changed
- Upgrade Custom Inspector Design
  - Foldout, Array/List 등의 Foldout 을 더 직관적이고 보기편하게 바꾸었습니다.
- Fixed Bugs with Array/List
  - 여러 오브젝트를 복수선택하여 array/list의 값을 수정할시 첫번째 선택오브젝트만 수정되었던 버그를 해결했습니다
  - 값이나 배열의 길이를 수정 했을때 Undo(Ctrl+Z)를 할수있게 수정했습니다
***

##	20.0.1 [20.09.01]
###	Added
- Added setting to ignore 'UnityEngine.Debug' of specified components (Use 'CWJ_Debug' instead of 'UnityEngine.Debug'/ See [Debug Setting] in inspector)
- Added setting to listen 'UnityEngine.Debug' only specified components (Use 'CWJ_Debug' instead of 'UnityEngine.Debug'/ See [Debug Setting] in inspector)

###	Changed
- Optimization CWJ_Inspector elements (CWJ_InspectorElement, etc)
- Changed Foldout, Box style (EditorGUICustomStyle)
- Attribute supported LayerMask, UnityEvent
  - InvokeButton supported 'UnityEvent'
  - EditorDrawUtil.DrawVariousType supported 'LayerMask'
***

##	20.0.0 [20.08.25]
### Added
- Added Attribute :
  - DrawHeader, DrawLine, DrawHeaderAndLine
  - Addon : SearchableEnum, MinMaxRange
- Added ConditionalField_Variable(supported field,property) (Classify ConditionalField into ConditionalField_Method and ConditionalField_Variable) (Attribute)
- Added VR HandInteraction

###	Changed
- Supported Unity19.4.8(for pro skin)
- Attribute Updated
  - __VisualizeField, VisualizeProperty is supported interface__
  - VisualizeField, VisualizeProperty is auto repaint when value changed
  - Readonly, ConditionalField is supported UnityEvent
- Optimization CWJ_Inspector
  - Optimization and fix EditorDrawUtil.PropertyField_CWJ
  - (see CWJ_InspectorCore, CWJ_InspectorProcess_Abstract) and Attribute
***

##	19.7.7 (20.07.31)
###	Added
- AssetReferenceFinder (프로젝트 창에서 참조 검색 원하는 오브젝트를 마우스 오른쪽 버튼-Find References)
- Added Singleton interface 'ICannotPreCreatedInScene' (Classes that override this interface cannot be pre-created in scene.)
- Added 19.3 version New ResolutionDialog
- MouseClickManager
- UIRenderQueueModifier (world space UI canvas 를 3d object보다 위에 rendering 하게끔 도와줌)

###	Changed
- OffscreenIndicator is supported VR
- RequiredField is supported Array, layermask, bool, int

###	Removed
- Removed EditorCoroutine script (PackageManager를 통해 직접 설치하도록 팝업창 으로 안내)
***

##	19.7 [20.07.24]
###	Added
- Added AutoDefineSwitcher (Possible #if ApiCompatibilityLevel)

###	Changed
- Modified Name [VisualizeAllField] -> [VisualizeField_All] / [VisualizeAllProperty] -> [VisualizeProperty_All]
- Renewed InvokeButtonAttribute's design
- Optimization InvokeButtonAttribute
- Optimization CWJScriptableObject
***

##	19.6 [20.07.22]
###	Added
- Added GetSizeOfBoundsThroughCamera
- Added FoldoutAttribute.isMergeParentFoldout
###	Changed
- Fixed base class's FoldoutAttribute
- Optimization FoldoutAttribute's process
- Fixed VolatileEditorPrefs's key name (use project's unique GUID)
###	Removed
***

##	19.5 [20.07.17]
###	Added
- Added OffscreenIndicator
- Added StringUtil.ToReadableString
###	Changed
- Fixed Attribute compatibility issues
- EditorDrawUtility.DrawVariousType is supported Tuple<>, ValueTuple<>
###	Removed
***

##	19.3.5 [20.07.06]
###	Added
- Added MeshVisualizing
- Added ReflectionUtil. ConvertNameToFunc + ConvertNameToAction
- __Added ConditionalField - Hide/show field through predicate name__

###	Changed
- __Attribute Updated__
  - GetComponent, FindComponent add predicate param
  - Readonly, ConditionalField, VisualizeNonPublic, VisualizeProperty, OnValueChanged, RequiredField, ... are compatible with each other (Attribute)
***

##	19.3 [20.07.01]
###	Changed
- EditorDrawUtility.DrawVariousType (Add DragAndDrop Function in array/list type)
- non-public 'array, list, struct, class' that declared VisualizeNonPublicAttribute are can be modify in editor mode
***

##	19.2 [20.06.29]
###	Added
- Added MeshUtil.GetColumnLines
- Added MathUtil.GetNearNumber
- Added VectorUtil.GetAngle (0~360, -180~180)

###	Changed
- Improved convenience of FindStaticEditorFlag
- Change MeshUtil.RandomCache Struct
- Readonly attribute Can be used more general-purpose (VisualizeNonPublicAttribute, VisualizePropertyAttribute ...) (+ AttributeUtility.GetIsReadonly)
- array, list, struct, class are also added to EditorDrawUtility.DrawVariousType's support type (Thanks to 김상민) (Now EditorDrawUtility.DrawVariousType supports everything)
- Visualize of non-public array, list, struct, class (Can only be modified during 'Application.isPlaying')
***

##	19 [20.06.17]
###	Changed
- Optimization DebugLogWriter (CWJ/DebugSetting/isSaveLogEnabled)
- __log.txt file is Synchronized with now-log data (CWJ/DebugSetting/isSaveLogEnabled)__
- Added Scroll UI in FindStaticEditorFlagObject Window
- Refactoring DebugSetting_Window, CustomDefine_Window
###	Removed
- __Replaced SingletonCoreGeneric.isAutoCreateWhenNull -> 'ICannotAutoCreateWhenNull' interface__
  - (Classes that override this interface cannot be created automatically.)

##	18.8 [20.06.14]
###	Added
- Added MeshUtil (GetEdgeFromBounds, GetRandomPosOnNonConvexMesh ...)
- Added InGameGizmos (Addon Popcron.Gizmos)
- Added DrawLineAttribute
###	Changed
- __Modified Name ContextButton -> InvokeButton__
- Fixed SceneEnum (When the first char of scene name is a number)
- Updated VrDevTool
***

##	18 [20.05.20]
- Added MathUtil.ConvertOtherRangeVlaue
- Added GetCompAttribute param (IsUpdateOnlyNull)
- Added AccessibleEditorUtil.PingAssetFile
- Added FindStaticEditorFlagObject
- Added FindObjectAttribute (kind of GetComponent Attribute)
- Fixed OnScreenKeyboardHelper, InputFieldBugHelper
- Added Open Button in BuildEventSystem-BuildCompleted-DialogPopup
- Modified Name SerializePropertyAttribute -> VisualizePropertyAttribute
- Added Exception (Call SingletonCore.Instance when Dispose(Application Quit))
- Added SingletonHelper (Upgrade DontDestroyHelper => SingletonHelper)
- Fixed and Upgrade VR_Manager
- Added SingletonCore.isAutoCreateWhenNull property
***

##	17.7 [20.04.10]
- Added GetAllDataToText, GetAllDataToLogFormat, SetAutoInsertIndent
- Added CameraScreenshotFunction
- Added FindObjectAttribute, FindObjectInactiveAttribute
- Added PingAssetFile
- Added FindStaticEditorFlagObject
- Added EzPaintManager - UndoPreviousDraw (Thanks to 김성수)
- Fixed OnScreenKeyboardHelper, InputFieldBugHelper
***

##	17.6 [20.04.05]
- Added Editor Function (SetHierarchySearchField/ SetProjectBrowerSearchField)
- Fixed SingletongDontDestroyOnload
- Fixed PsMessage
***

##	17.5 [20.04.05]
- Added Utils
- Fixed SingletonCore - Editor Process
***

##	17.3 [20.04.03]
- Added VectorUtil, WorldTransform
***

##	17.2 [20.04.02]
- Added GetAllVariablesInfoToText
- Added Type Extension (about isConditions)
***

##	17.1 [20.04.01]
- TouchManager supported + touch monitor for windows
***

##	17 [20.03.29]
- Fixed & Refactoring EzPaint (Works well regardless of the 'RenderMode' of the 'Canvas')
- Added Save Function of EzPaint
***

##	16 [20.03.27]
- Fixed & Refactoring TouchManager
- Fixed ScriptableObject
***

##	15.5 [20.03.26]
- Security enhancement
- Can identify when 'import or delete CWJ library' -> at this time, add or remove a scripting_define_symbols.
***

##	13.1 [20.03.23]
- CustomAttributeHandler optimization
- Fixed 'RuntimeFixedTool forcibly disable'
***

##	13 [20.03.20]
- Added function to disable RuntimeFixedTool
***

##	12.7.7 [20.03.17]
- Unity 2019.3.2 perfectly supported (with ui)
- Added LoadStreamingAssets()
- Added CopyStreamingAssetsFileToPersistent()
***

##	12.7 [20.03.12]
- SingletonCore optimization
- Editor class optimization
***

##	12.5 [20.03.10]
- Fixed log saving bug in AOS
- Fixed bug when importing UnityDevTool-package
- etc.. fixed bug
***

##	12.4 [20.03.06]
- You can choose whether to disable logging in build only.
- DebugSetting, CustomDefine, PsMessage layout fix
***

##	12.3 [20.03.05]
- Fixed bug with saving logs (before: stack trace in log not being correct in the Unity)
***

##	12.2 [20.03.03]
- Unity 2019.3.2 supported.
- Troubleshooting import issues between projects or between project versions.
- Added property to check if 'import unitypackage(UnityDevTool)' is in progress.
***

##	12.1 [20.03.02]
- BuildNamePackage bug fix.
- Avoid recursive calls
***

##	12.0 [20.02.27]
- Unity 2019.3 test.
- Refactoring related to ScriptableObject. (CWJScriptableObject,, etc)
- Optimized WindowBehaviour using ScriptableObject.
***

##	11.7.7 [20.02.26]
- SceneEnum.cs파일이 없더라도 자동 생성하여 오류가 안나도록 수정.
- DebugSetting - Log 관련 Define symbol 리팩토링.
- OpenAsset 관련 디버깅.
***

##	11.6 [20.02.21]
- Assets 직계하위 경로에 새로운 폴더 생길 시 로그출력, 폴더 강조
***

##	11.5 [20.02.20]
- Attribute, Inspector 관련 최적화
***

##	11.4 [20.02.19]
- EditorEventSystem, CustomAttributeHandler 이벤트 관련 최적화
***

##	11.3 [20.02.19]
- Static, private 변수 직렬화 시스템 개발 완료 (serializeField를 사용하는것이 아닌 initializer를 자동으로 작성해주는 방식)
***

##	11.2 [20.02.14]
- SceneEnum 동기화 시점을 5초후 대기 기능 추가 (Build Setting 수정완료 후 5초 후에 실행됨)
- EditorCallback 기능 추가
- DebugSetting, CustomDefineSetting 등 윈도우 창 관련 개선 (프로젝트가 열린 후 초기화메세지 띄운다거나.. CWJ.Editor 관련 로그는 끌수있다거나.. 기타 등등 사용편의성 개선)
***

##	11.1 [20.02.13]
- 에디터 이벤트 시스템 개발 완료 (자식/부모 생성, 변경 시 실행이벤트 최적화 완료)
- 번아웃..
***

##	11.0 [20.02.07]
- 에디터에서 컴포넌트/오브젝트 이동(드래그&드랍 포함), 삭제, 추가 시 이벤트 실행(GetComponentAttribute와 연동) -> 아직테스트중 많이 느릴 시 주석처리하기
- 컴포넌트 캐쉬 시스템 완성(GetComponentAttribute와 연동)
- CWJ_Setting 윈도우창 관련 설정(DebugSetting 등) 수정 시 컴파일 로딩 텍스트 표시
***

##	10.9 [20.02.05]
- BuildPackageName 바뀌었을 때 Setting 관련 Scriptable Object들 초기화 시기 조정.(프로젝트 열리고 로딩후)
- 에디터에서 FindObjectOfType을 할 때 DontDestroyOnLoad 오브젝트가 검색되는 문제 해결 (FindObjectOfType_New로 해결)
: includeDontDestroyOnLoadObjs 가 false 일 때는 DontDestroyOnLoad오브젝트는 검색안되도록함.
***

##	10.8 [20.02.03]
- BuildSetting에 씬을 등록하면 SceneEnum.cs의 SceneEnum 에 자동 등록되는 기능 디버깅 및 보완
- 확장 메소드 summary
***

##	10.7 [20.01.31]
- BuildSetting에 씬을 등록하면 SceneEnum.cs의 SceneEnum 에 자동 등록되는 기능 개발
- 1.을 포함한 SceneControlManager 등을 사용하려면 using CWJ.SceneHelper 선언해야함
- 1.의 개발로 인해 SceneNameAttribute가 필요없어짐. 주석처리함.
4. 모든 싱글톤은 Instance 호출하기전까지는 Instance생성안됨 (DontDestroyOnLoad는 Awake에서 DontDestroyOnLoad 적용됨)
***

##	10.6 [20.01.30]
- 싱글톤 Core 기능 보완 (더 오래된/ 더 먼저 생성된 싱글톤 판단, 활용 기능 추가)
- CWJ 관련 EDITOR 로그 StackTrace 삭제
***

##	10.5 [20.01.29]
- CWJ.Utility namespace 삭제 (~Util 클래스들 using CWJ 선언만 해도 사용가능)
- 싱글톤 최적화.
- SingletonBehaviour 종류를 바꾸면 실시간으로 정보가 업데이트되게 수정
***

##	10.4 [20.01.28]
- 디버깅, 리팩토링 완료
- 빌드 완료(성공) 후 빌드 시간, 빌드 저장 경로등의 정보가 적힌 알림창이 표시되는 기능 추가
***

##	10.3 [20.01.23]
- 프로젝트를 열고나서 로딩완료된 직후 1회 실행되는 콜백이벤트 개발.
- StackTrace 안보이는 Editor용 DebugLog 기능 추가
***

##	10.2 [20.01.21]
- Editor 상에서 AddComponent, RemoveComponent 실행될 시 알려주는 콜백이벤트 개발.
- 1.의 콜백이벤트 개발덕분에 RequiredTab/RequiredLayer 어트리뷰트를 가진 컴포넌트 생성했을때 생성하는 동시에 적용이됨.
***

##	10.1 [20.01.20]
- ContextButton Attribute 런타임중이 아닐때 코루틴 실행 기능 추가(시간대기의 경우 WaitForSecondsRealtime를 사용해야함)
- Foldout Attribute를 사용하면 base클래스 변수들이 FoldoutAttribute아래로 내려가는 문제해결
***

##	10.0 [20.01.20]
- GitHub 업로드 완료
- DebugSetting / DefineSetting / PsMessage 분리
- MadeByCWJ -> CWJ / Developer-friendlyPackage -> UnityDevTool 폴더명 변경
***

##	9.9.1 [20.01.17]
- GetComponentAttribute 실행 타이밍 변경 + 알고리즘 수정
이제 씬이 저장될때 수정한게 있으면 실행, 게임이 실행되기 직전에 실행, 스크립트가 수정되었을때 실행
찾은 값이 들어가있는 값과 다를경우에만(변수의 타입 혹은 갯수가 변경되는 등) 씬에 Dirt를 적용하여 프리팹 Overrides - Apply All 을 해도 적용이 안되던 문제 해결
***

##	9.9 [20.01.16]
- Attribute 대거 추가 (AssetPreview, ProgressBar, SerializeAllFields, SerializeProperty, RequiredLayer, RequiredTag)
- namespace 대규모 구조조정
<정리>
a. 많은 확장메소드, ~Util 클래스들 using CWJ.Utility를 선언해야 사용가능. -> 10.5 에서 취소. 그냥 using CWJ 선언만으로 사용가능하게 복구
b. Attribute는 using CWJ; 선언만으로 가능.
c.  Singleton은 using CWJ.Singleton; 선언해야함
d.  ExtensionPackage의 클래스들이 파일로 분리, Utility폴더로 이동, 클래스의 이름(FindExtension, ReflectionExtension 등)이 ~Utility로 변경.
e. 자주쓰일 SerializeAllFields, SerializeProperty는 isFindAllBaseClass 를 true로 하면 MonoBehaviour 전의 최상위 클래스의 nonPublic/ property를 표시해줄수있음
***

##	9.8 [20.01.15]
- Attribute 대거 추가 (ContextButton, RequiredField, SerializeProperty, SceneName, Layer, Tag)
- Editor, Attribute 스크립트 구조 전체 리팩토링
***

##	9.7 [20.01.14]
- GetComponentAttribute 리팩토링 + 개선 (성능최적화 + 인스펙터 확인하지않아도 실행)
- Foldout Attribute 추가
***

##	9.6 [20.01.13]
- SerializableInterface new version (편의성 향상)
***

##	9.5
- Attribute 대거 추가 (SceneNameAttribute, EmailAttribute, OnValueChangedAttribute)
***

##	9.4
- SerializableInterface 수정중
***

##	9.3
- SingletonRoot [DisallowMultipleComponent] 제거
- New_FindObjectsOfType 중복된 컴포넌트를 갖고있을경우 하나 만 찾는 문제 해결
***

##	9.2
- DebugSettingManager isDebugLogEnabled 켜고 바로끄면 곧바로는 Log가 제거되지 않는 버그 해결 (DefineSymbol이 즉시 추가/제거 안되던 문제)
***

##	9.1
- WindowBehaviour 간략화
***

##	9.0 [2020 1.1 HappyNewYear]
- Debug Setting Manager PS메세지 기능 완성
- ExtensionPackage 관련 디버깅
TODO : MissingObjectFindManager 완성(CopyComponent기능 완성)

8.9
 - DebugSetting 관련 수정
 - VRManager 사용 유무에 따라 SteamVR 사용유무를 판단하여 #if 처리. (VRManager 사용시 CWJ_VR DefineSymbol이 등록되고 SteamVR 플러그인을 필요로함)

8.8
 - Additive Define Symbols 삭제가능(Remove 버튼)
 - 유니티에디터 프로그램 종료전 이벤트로 PS메세지 기능 보완

8.7
 - VR관련 수정

8.6
 - CWJ.EditorScript 디버깅 (빌드 시 오류 hotfix)

8.5
 - 안정화

8.4
 - VR 폴더 생성 (VR 모듈단위 유틸) VR_PlayerMovable, VR_ShowController 등

8.3
 - VRManager 개발중

8.2
 - 씬전환 스크립트 생성
 - 폴더 구조 변경

8.1
 -  DontDestroyOnLoad 의 오브젝트도 찾을수있게 함.(7.8에서는 불가능했음)
	DontDestroyOnLoadHelper.FindComponentsOfDontDestroyOnLoad<>()

8.0
 - DebugSettingManager PS 추신 메세지 기능 추가

7.9
 - 안정화

7.8
 - DebugSettingManager 편의성 개선 : DebugSetting을 설정(Window/CWJ/CWJ_Debug) 했다면 빌드전에 또 DefineSymbols를 등록할 필요없어짐
 - SerializeInterface관련 함수 버그 디버깅
 - DontDestroyOnLoad 시킨 오브젝트는 FindExtension.GetRootGameObjects()로 못찾으므로 DontDestroyOnLoad된 오브젝트의 자식을 검색하는 방식으로 찾아야함 (-> 8.1 버전에서 해결, 가능)

7.5
 - 전체적 디버깅
 - DisplayDialogExtension 편의성개선
 -  UGUI 버튼을 사용중일때 더블클릭이나 길게 누르고있을때 실행되는 이벤트 등록 확장함수 추가 (사용예시: resistancePlusBtn.AddLongPressLoopEvent(Btn_PlusResistance, availableTime: 0.3f, loopInterval: 0.15f);)

7.2
 - MultiDisplayManager 추가 (컴포넌트를 최초Load씬에 추가 시 'CWJ_MULTI_DISPLAY' define symbol이 자동으로 추가됨. MultiDisplay설정을 끄려면 DebugSettingManager윈도우 켜서 체크박스 해제하기)
 - DebugSettingManager 윈도우창에 Additive Define Symbol리스트 추가 (유니티 Reset이벤트에 DebugSettingManager.AddAdditiveDefienSymbol 코드추가해주면 윈도우창에 보이게됨. 내 싱글톤은 On_Reset)

7.1
 - ScrollCaptureManager 오타/버그 수정

7.0
 - 싱글톤 추상클래스(SingletonRoot) 리팩토링 (구조 전체를 뜯어내서 OnlyUseNew까지 병합, 추상클래스에 모든 함수, 기능을 쳐박아넣고 실행은 상속받는 클래스들(SingletonBehaviour, 등)에서 시켜줌 -함수만 간결하게 적혀있음- )
 - 싱글톤 제네릭클래스들의 인스펙터에 'isDontDestroyOnLoad', 'isOnlyUseNew' bool변수를 가시화 (인스펙터만 봐도 이 싱글톤이 DontDestroyOnLoad 싱글톤인지, 새로추가되는 Instance를 Instance로 확정짓는 녀석인지 확인가능하게함)

6.9
 - 현재 씬에 존재하는 싱글톤을 찾아주는 기능 추가 (실행법: 상단탭 Window/CWJ/Singleton/CWJ_SingletonFind 클릭)

6.7
 - Network관련 추가(Telepathy dll추가, Telepathy예제 추가)
 - Serializable 관련 에디터 아닐 시 최적화

6.6
 - ApplicationQuitWait 추가 (종료전에 수행해야 할 콜백이 등록되어 있을경우 종료를 취소하고 콜백 수행완료 시(코루틴/메소드 상관없음) 종료를 시작함, 현재는 에디터에서만 가능)

6.5
 - namespace정리
 - New_FindObjectOfType 디버깅

6.4
 - New_FindObjectOfType predicate 조건설정기능 추가

6.3
 - SerializableInterface 추가

6.2
 - DisplayDialogExtension 추가 (WindowBehaviour,InspectorBehaviour에 각각 중복적으로 있던 함수 합치고 확장메소드로 만듬)

6.1
 - EzPaintingManager GameObject/CWJ/2D_EzPaintingManager 관련 오류(CWJ탭이 열리자마자 DialogDisplay가 팝업되어 탭이 취소됨)
		isValidateFunction기능을 주석처리함. 즉 비활성화되진않지만 클릭시 DialogDisplay가 뜨도록 수정
 - WindowBehavior, InspectorBehaviour DisplayDialog 시스템수정 (함수호출을 제네릭을 통해 하면 타이틀을 자동으로 생성하고, 해당 메세지 내용으로 DebugLog를 출력함)

6.0
 - DebugSetting 관련 디버깅
 - DebugLogExtension 로그 꾸미는 기능 완성 주석색상 함수 추가

5.8
 - unity BuildPipeline 적용 (빌드 전/후 조건문 콜백이벤트 추가가능) (BuildCallback 폴더 참고)
 - DebugSetting 의 일부기능에 1.의 콜백이벤트 이용, Define Symbol의 수정이 필요할 시 자동으로 빌드 중단 후 재 빌드 강요하도록 자동화 기능 개선
 - DefineExtension 기능 추가 (모든 플랫폼 타겟에 한번에 Define 적용하는 기능)

5.7
 - DebugSetting 관련 디버깅
 - 전체적인 디버깅

5.6
 - ComponentExtension.New_FindObjectOfType 확장메소드 추가
		비활성화되어있는 컴포넌트 찾는기능 (기존 FindObjectOfType 상위호환/ 비활성화되어있는거까지 찾을땐 비용은 큼, DontDestroyOnLoad는 여전히 찾을수없다..-> 8.1 버전에서 해결, 가능)
 - DebugLogExtension Log색상,크기 설정 확장메소드 추가

5.5
- SetScriptExecutionOrder 추가
		함수를 통해 스크립트의 실행순서 설정가능 (확인은 Edit/Project Settings/Script Execution Order)
 - ApplicationQuitEvent 추가
		프로그램 종료될때 콜백추가 가능 (예시: CWJ.ApplicationQuitEvent.Instance.AddCallback(액션);)
 - DebugSetting
		txt파일 저장방식 수정(시작하자마자 한번 open 하고 종료직전에 close시킴)
		Error와 Assert도 기본 커스텀된 메세지로 출력됨 (Exception만 Stack Trace가 길거임)
		빌드 직전에만 DebugSetting 설정값 확인시키고, 수정 가능(수정하려고 할 시 빌드취소) ->2018버전에서 테스트필요 BuildPipeLine 써야할지도
		이제 DebugSetting 윈도우창 뜨는 일은 빌드시작직전에 수정하려고할때 뿐임

5.3
- SingletonBehaviour/DontDestroyOnLoad
		빌드후 런타임에서부턴 DontDestroyOnLoad 를 특정해내지 못하여 (-> 8.1 버전에서 해결, 가능)
		DontDestroyOnLoad에 포함되어있는 싱글톤을 찾는것은 포기. ->
		대신 DontDestroyOnLoad가 되는 모든 싱글톤(자식포함)은 DontDestroyOnLoad가 되기전에 Instance 적용.
 - DebugLogManager
		DebugSetting 이름변경 그리고 안정성 향상, Debug의 모든 스크립트(Assert 등) 까지 끄고켜는기능에 적용됨
- LogTxt파일 저장 방식 수정

5.2
- SingletonBehaviourDontDestroy는 부모를 하나로 두면안되고 각각 다른 오브젝트로 두어야함 아직 문제수정못했음(수정할거임)

5.1
- FixedInputFieldBug 버그 수정
- SingletonBehaviour 관련 : DontDestroyOnLoad 씬정보를 가져올수가없어서(-> 8.1 버전에서 해결, 가능)  DontDestroyOnLoad에 싱글톤(DontDestroyOnLoad 아닌)이 있을 시 Instance 초기화할때 없다고 나옴.
수정 요망

5.0
- DebugLogManager 추가 (Debug.Log 출력관련 제어, 저장 기능)
  - Window 탭에서 CWJ - CWJ_DebugLogManager 를 클릭하면 설정할수있습니다
  - isDebugLogEnabled 를 끄면 Debug 로그 출력관련한 함수들이 컴파일에서 제외되어 실행되지않습니다
  - isSaveDebugLog 를 켜면 빌드설정에서 Development Build 가 활성화됩니다


4.3
Scroll Capture 추가 (해상도에 상관없이 원하는 크기로 스크롤 내 의 컨텐츠를 캡쳐가능)

4.2
FixedInputFieldBug (CWJ_InputField) 디버깅 완료 (이미 포커스되어있는것을 다시 클릭시 무시하는 버그)

4.1
UI관련 확장모듈들 자동생성기능 추가

4.0
- TouchManager 완성(자동 Switching 싱글톤 적용, PC - 모바일 용으로 자동 전환)
- PaintingManager 완성(런타임중 아닐때 PaintingManager 컴포넌트 추가시 자동오브젝트 설정)
- Singleton OnlyUseNew 추가
- 자동 Switching 싱글톤 예제 추가(Singleton OnlyUseNew 이용)
- NewInputField 한글로 입력시 버그나오던 문제 대처용 InputField (사용법은 런타임중일때:AvoidInputFieldBug InputField에 추가 / 런타임중 아닐때: CWJ_InputField 컴포넌트 추가)

3.6
- EzPainting 개발완료
- EditorCallbackSystem 개발완료

3.5
- SingletonBehaviour 관련 안정성 개선 (SingletonRoot 추가)

3.4
- PolygonChart 기능 추가 및 개선
- SerializableDictionary Remove관련 버그 디버깅

3.3
- MissingObjectFinder 안정성 개선 (재시작 메소드를 delayCall에 추가시켜 씬이 저장된 이후 실행되도록)

3.2
- EditorRestartScript 추가
- MissingObjectFinder 안정성 개선(삭제나 컴포넌트 붙여넣기 후 재시작 강요)

3.1
- FindMissingObject 완성

2.8
- InpectorBehaviour 추가
- SequenceEvent 추가

2.7
- PolygonChart 추가(radar chart.. 동적 다이아몬드형 그래프관련)
- EventDelayList 추가(이벤트 배열 각각이 딜레이가 존재하는)
- SerializableDictionary 추가
- GizmoManager 추가

2.5
- GetComponentsInParentWithPredicate 추가(getcomponent에 조건문)
- AddListener_New 관련 수정

2.4
- ReadOnlyAttribute 수정(매개변수를 통한 runtimeOnly 값 수정을 없애고 ReadOnlyWhileRuntimeAttribute 어트리뷰트를 추가함)
- ExtensionPackage에 UnityComponentExtension 추가(GetComponentsInParentWithoutMe, GetComponentsInChildrenWithoutMe)

2.3
- SingletonBehaviourDontDestroy 를 SingletonBehaviour 상속으로 수정
- ExtensionPackage 에 TextFileExtension 추가

1.0
- Developer-friendly Tool 라이브러리 시발점 2019.1.7
- CWJMade namespace 제작

//Comments
OS별 키 설정은 InputKeySetting.cs 참고
JsonObject사용할땐 ExtensionPackage.cs의 JsonObjectExtension클래스 주석해제하기
