using System;
using UnityEngine;

public class ConvertTypeTest : MonoBehaviour
{
    public enum TestEnum
    {
        apple,
        banana
    }
    [CWJ.Readonly]public int a;
    [CWJ.Readonly]public TestEnum testEnum;

    [SerializeField] private int _a;
    [SerializeField] private TestEnum _testEnum;

    [CWJ.InvokeButton]
    void Test()
    {
        var publicFields = GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var privateFields = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        for (int i = 0; i < publicFields.Length; i++)
        {
            SetValueViaString(publicFields[i], privateFields[i].GetValue(this).ToString());
        }
    }

    void SetValueViaString(System.Reflection.FieldInfo fieldInfo, string value)
    {
        Type type = fieldInfo.FieldType;
        if (type.IsEnum)
        {
            fieldInfo.SetValue(this, Enum.Parse(type, value));
            return;
        }

        fieldInfo.SetValue(this, Convert.ChangeType(value, type));
    }
}
