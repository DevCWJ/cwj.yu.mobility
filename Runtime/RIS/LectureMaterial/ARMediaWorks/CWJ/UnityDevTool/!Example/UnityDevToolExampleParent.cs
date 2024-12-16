using System;
using System.Linq;
using UnityEngine;
using CWJ.SceneHelper;
using System.Collections.Generic;

namespace CWJ
{
    public class UnityDevToolExampleParent : MonoBehaviour
    {
        private string parentTest;
        [FoldoutGroup("TestFold", true)]
        [SerializeField] private string[] strArray;
        [SerializeField] private List<string> strList;
        [SerializeField] protected string requiredParentStrs;
        [FoldoutGroup("TestFold", false)]
        [SerializeField] private string cStr;
        private static string test;
        [Readonly] public int parentInt;

        protected string parentTestStr = "test12345678";
        [NonSerialized] protected Vector2Int parentTestVector = new Vector2Int(7, 4);

        protected string parentPropertyGet
        {
            get
            {
                return "bcde";
            }
        }

        protected string parentPropertyGetSet
        {
            get; set;
        }

        [CWJ.InvokeButton]
        protected void PrintJoystick()
        {
            string[] names = Input.GetJoystickNames();

            for (int i = 0; i < names.Length; i++)
            {
                Debug.Log("Connected Joysticks :: " + "Joystick" + (i + 1) + " = " + names[i]);
            }
        }

        [ResizableTextArea, SerializeField]
        private string codes = "public class TestScriptParent : MonoBehaviour" +
            "\n{" +
            "\nprotected string parentPropertyGet" +
            "\n{" +
            "\nget" +
            "\n{" +
            "\nreturn bcde;" +
            "\n}" +
            "\n}" +
            "\n}" +
            "\nprotected string parentPropertyGetSet" +
            "\n{" +
            "\nget; set;" +
            "\n}";

        [CWJ.InvokeButton]
        public void CWJ_DebugPrintCodes()
        {
            CWJ_Debug.LogError(codes.SetAutoInsertIndent(0));
        }

        protected int parentProtected = 1234566;
#pragma warning disable 0414
        private int parentPrivate = 444;
#pragma warning restore 0414

        [InvokeButton]
        void TestPrintLog()
        {
            CWJ.DebugLogUtil.PrintLogWarning("!", context: gameObject);
            CWJ.DebugLogUtil.PrintLogException<NotSupportedException>("?!", isPreventStackTrace: true);
            CWJ.DebugLogUtil.PrintLogException<NotSupportedException>("?!");

        }

        private void Start()
        {
            TestPrintLog();
        }
    }
}
