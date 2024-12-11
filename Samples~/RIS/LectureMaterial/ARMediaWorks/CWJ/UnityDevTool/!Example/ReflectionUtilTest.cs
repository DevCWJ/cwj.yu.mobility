using UnityEngine;

namespace CWJ
{
    public class ReflectionUtilTest : MonoBehaviour
    {
        public TestStruct user;

        [InvokeButton]
        void PrintLogData()
        {
            user = new TestStruct();
            Debug.Log("로그 출력용 들여쓰기\n" + ReflectionUtil.GetAllDataToText(nameof(user), user, ReflectionUtil.EConvertType.Log));
            Debug.LogError("스크립트파일 작성용 들여쓰기\n" + ReflectionUtil.GetAllDataToText(nameof(user), user, ReflectionUtil.EConvertType.Script));
        }
    }

    public class TestStruct
    {
        public string name;
        public int sex;
        public string Sex { get => sex == 1 ? "Man" : "Woman"; }
        public int age;

        public Preset preset;

        public TestStruct()
        {
            name = "cwj";
            age = 69;
            sex = 1;
            preset = new Preset();
        }
    }
    public enum EReflectionEnum
    {
        Passive = 0,
        Active = 1,
        Resistive = 2
    }

    public class Preset
    {
        public EReflectionEnum exerciseMode;
        public int tableAngle;
        public int spasmLevel;

        public Preset()
        {
            DefaultSetting();
        }

        public void DefaultSetting()
        {
            exerciseMode = EReflectionEnum.Passive;

            tableAngle = 45;
            spasmLevel = 4;
        }
    } 
}