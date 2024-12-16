using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEngine.Events;
using CWJ;
using CWJ.SceneHelper;
using CWJ.Serializable;

public interface IVisibleInterface
{
    string boo { get; set; }
}

[System.Flags]
public enum FlagsEnumTest
{
    Action = 1 << 0,
    Binary = 1 << 1,
    Cache = 1 << 2,
    Debug = 1 << 3
}

public struct NonSerializableStructTest
{
    string[] strs;
    Transform[] transforms;
    Vector3[] vector3s;
}

[Serializable]
public class StringList : SerializedList<string> { }

[Serializable]
public class IntArray : SerializedArray<int> { public IntArray(int count) : base(count) { } }
[Flags]
public enum _TestFlagsEnum
{
    None = 0,
    Left = 1 << 0,
    Right = 1 << 1,
    Up = 1 << 2,
    Down = 1 << 3,
    All = ~0
}

public enum _TestEnum
{
    Left,
    Right,
    Up,
    Down
}

namespace CWJ
{
    [VisualizeField_All(true), VisualizeProperty_All(true)]
    [RequiredTag("TagTest_2"), RequiredLayer("LayerTest_2", true)]
    public class UnityDevToolExample : UnityDevToolExampleParent
    {
#pragma warning disable

        [InvokeButton]
        public UnityEvent unityEvent;
        [InvokeButton]
        public void AddEvent()
        {
            unityEvent.AddListener(AddEvent);
        }

        [SpriteLayer]
        public int spriteLayer;

        IVisibleInterface[] visibleInterface;

        [VisualizeField]
        public int ss = 1;

        public string boo { get; set; } = "boo!";

        [UnityBuiltInDrawer]
        public GameObject[] unityBuiltInArray;

        [FoldoutGroup("TestFold", true)]
        public Vector3[] v3Array;
        public List<Vector3> v3List;
        public List<GameObject> objList;
        public GameObject[] objs;


        [SerializeField] private string requiredStrs;
        [SerializeField] private string eStr;
        [FoldoutGroup("TestFold", false)]
        [SerializeField] private string fStr;

        public static int StaticField = 6;



        [DrawLine(AccessibleEditor.AttributeUtil.EParameterColor.Orange)]
        [EnumFlag]
        public FlagsEnumTest flagTest;
        [DrawHeaderAndLine("<color=#ffff00ff><b>CWJ Header</b></color> <color=#008000ff>And <i>Line</i></color>", lineColor: AccessibleEditor.AttributeUtil.EParameterColor.Blue)]
        [SearchableEnum]
        public KeyCode searchableEnum_keyCode;

        [DrawHeader("CWJ Header", AccessibleEditor.AttributeUtil.EParameterColor.Red)]
        [MinMaxRange(1, 12.5f)]
        public RangedFloat rangedFloat;

        private string propertyGetSet
        {
            get; set;
        }
        [VisualizeField] byte byteTest = 246;

        private Color privateColor = new Color(1f, 0f, 0f, 0f);

        protected Bounds protectedBounds = new Bounds(new Vector3(33f, 12f, 123f), new Vector3(8f, 90f, 912f));

        [VisualizeField] private List<UnityDevToolExampleParent> privateParentList;
        [VisualizeField, HideConditional(nameof(PredicateForConditionFieldTest)), Readonly] private Transform[] privateTrfArray;

        [VisualizeField] private List<NonSerializableStructTest> privateStructs;
        [VisualizeField, HideConditional(nameof(PredicateForConditionFieldTest))] private NonSerializableStructTest[] privateStructArray;



        public static string StaticPropertyGetSet
        {
            get; set;
        } = "abc";

        [ProgressBar("Progress", 100, isVisibleField: true)]
        public float progressBar = 50;

        [ErrorIfNull]
        public bool isInitialized;

        [ErrorIfNull]
        public Transform[] requiredFieldTrf;

        [Tooltip("ssss")]
        [OnValueChanged(nameof(OnValueChangedStr))]
        public string onValueChangedStr = "";
        void OnValueChangedStr()
        {
            Debug.LogError("OnValueChanged " + onValueChangedStr);
        }

        [Layer]
        public int layer;

        [Tag]
        public string tagName;

        [AssetPreview]
        public GameObject previewObj;
        [AssetPreview]
        public Sprite previewSprite;

        void OnValueChanged()
        {
            Debug.LogError("OnValueChanged " + valueChangeDetectArray[0]);
        }

        [OnValueChanged(nameof(OnValueChanged)), Range(0, 1)]
        public int[] valueChangeDetectArray;


        void OnEnemyLengthChanged()
        {
            //unityEvent.FindMethod(unityEvent.GetPersistentMethodName(0), gameObject, uniev)
            if (enemyArray.Length != enemyLength)
                Array.Resize(ref enemyArray, enemyLength);
        }

        [OnValueChanged(nameof(OnEnemyLengthChanged))]
        public int enemyLength = 5;

        void OnEnemyArrayChanged()
        {
            if (enemyArray.Length != enemyLength)
                enemyLength = enemyArray.Length;
        }

        [OnValueChanged(nameof(OnEnemyArrayChanged))]
        public int[] enemyArray;

        [ResizableTextArea]
        public string resizableTextAreas = "cwj";
        private bool PredicateForConditionFieldTest() => resizableTextAreas.Contains("cwj");

        [ReadonlyConditional(nameof(PredicateForConditionFieldTest)), Range(0, 1)]
        public int readonlyRange;

        [HideConditional(nameof(PredicateForConditionFieldTest))]
        public GameObject[] conditionFieldTest;

        [SerializeField, ReadonlyConditional(nameof(PredicateForConditionFieldTest))] private UnityEvent readonlyEvent;


        bool GetCompPredicate(UnityObject obj)
        {
            Collider collider = (Collider)obj;
            return collider.enabled && collider.gameObject.activeSelf;
        }

        [FoldoutGroup("GetComponent Attribute Test", true)]
        [GetComponent]
        public Component getComp = null;

        //예시를 위한 Component[] 일뿐. Component를 상속받는것이면 모두 GetComponent가능
        [GetComponent, Readonly]
        public Component[] getCompArray = null;

        [GetComponentInChildren]
        public List<Component> getCompInChildList = null;

        [GetComponentInChildren(predicateName: nameof(GetCompPredicate))]
        public Collider[] getCompInChildWithPredicate = null;

        [GetComponentInParent(true)]
        public Component[] getCompInParents;

        [FindObject] public Transform[] findAllTrfs;

        [FoldoutGroup("GetComponent Attribute Test", false)]
        [FindObject(isIncludeInactive: false)] public Transform[] findOnlyActiveTrfs;

        public bool isConditionalFieldDemo = false;

        [HideConditional(nameof(isConditionalFieldDemo))] public GameObject demoConditionalField;


        [InvokeButton]
        private Vector3 TestLossyToLocalScale(Vector3 lossyScale)
        {
            return transform.LossyToLocalScale(lossyScale);
        }

        [VisualizeField] List<NonSerializableStructTest> structsField = new List<NonSerializableStructTest>();
        [VisualizeProperty] private List<NonSerializableStructTest> structsGetProperty => structsField;

        [InvokeButton]
        private void Start()
        {
            structsField = new List<NonSerializableStructTest>();
            structsField.Add(new NonSerializableStructTest());
            CWJ_Debug.LogWarning(ReflectionUtil.GetPrevMethodName());
            CWJ_Debug.LogWarning(string.Join(", ", System.Array.ConvertAll(FindUtil.GetRootObjsOfDontDestroyOnLoad(), (o) => o.name)));
            CWJ_Debug.LogError(rangedFloat.LerpFromRange(0.5f));
        }

        //private void FixedUpdate()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        CWJ_Debug.LogError(i);
        //    }
        //}

        [InvokeButton]
        private bool FlagEnumTest()
        {
            return flagTest.HasFlag(FlagsEnumTest.Action);
        }
#if CWJ_SCENEENUM_ENABLED
        public SceneEnum sceneEnum;
        [InvokeButton]
        public void ChangeScene(SceneEnum sceneEnum)
        {
            string nextSceneName = sceneEnum.ToSceneName();
            if (Application.isPlaying)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            }
            else
            {
#if UNITY_EDITOR
                var curScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(curScene);

                var nextScene = UnityEditor.EditorBuildSettings.scenes.Find(scene => scene.path.EndsWith(nextSceneName + ".unity"));
                //UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(nextScene.path);

                UnityEditor.SceneManagement.EditorSceneManager.CloseScene(curScene, true);
                //에디터 씬전환 완료
#endif
            }
        }
#endif

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                parentProtected++;
            }
        }

        [InvokeButton]
        public void SetInterface()
        {
            visibleInterface = FindUtil.FindInterfaces<IVisibleInterface>();
        }

        [InvokeButton]
        public void CallInterface()
        {
            if (visibleInterface == null || visibleInterface.Length == 0) return;
            CWJ_Debug.LogError((visibleInterface[visibleInterface.Length - 1] as Component).gameObject.name + " 1." + visibleInterface[visibleInterface.Length - 1].boo);
            visibleInterface[visibleInterface.Length - 1].boo = "Set test!";
            CWJ_Debug.LogError((visibleInterface[visibleInterface.Length - 1] as Component).gameObject.name + " 2." + visibleInterface[visibleInterface.Length - 1].boo);
        }
#pragma warning restore
    }
}
