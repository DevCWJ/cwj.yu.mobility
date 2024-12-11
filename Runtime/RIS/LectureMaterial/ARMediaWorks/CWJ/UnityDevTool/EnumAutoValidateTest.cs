using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public enum Language
{
    Kor,
    Eng,
    Chn,
    Jpn,
    KKK,
}

[Serializable]
public struct LanguageDataContainer : IEquatable<LanguageDataContainer>
{
    [HideInInspector] public string name;
    [SerializeField] private Language _language;

    public Language language => _language;

    public void SetLanguage(Language lang)
    {
        this._language = lang;
        name = lang.ToString();
    }

#if UNITY_EDITOR
    public void Editor_UpdateName()
    {
        if (name != _language.ToString())
        {
            SetLanguage(_language);
        }
    }
#endif

    public int tempInt; //예시

    public LanguageDataContainer(Language language, int tempInt)
    {
        this._language = language;
        this.tempInt = tempInt;
        this.name = language.ToString();
        SetLanguage(language);
    }

    public bool Equals(LanguageDataContainer other)
    {
        return this.language == other.language;
    }
}

public class EnumAutoValidateTest : MonoBehaviour
{
#if UNITY_EDITOR
    readonly static Type LangEnumType = typeof(Language);
    readonly static string[] LangEnumNames = Enum.GetNames(LangEnumType);
    readonly static int LangEnumLength = LangEnumNames.Length;
    private void Editor_UpdateLanguageData()
    {
        if (languageDatas.Length == 0)
        {
            languageDatas = Array.ConvertAll(LangEnumNames, e => new LanguageDataContainer((Language)Enum.Parse(LangEnumType, e, true), 0));
            return;
        }

        if (LangEnumLength != languageDatas.Length)
        {
            Array.Resize(ref languageDatas, LangEnumLength);

            for (int i = 0; i < LangEnumLength; i++)
            {
                if (LangEnumNames[i] != languageDatas[i].language.ToString())
                {
                    languageDatas[i].SetLanguage((Language)Enum.ToObject(LangEnumType, i));
                }
            }
        }
    }

    private void Editor_CheckModifyLanguageData()
    {
        for (int i = 0; i < languageDatas.Length; i++)
        {
            languageDatas[i].Editor_UpdateName();
        }
    }

    Language? overlappedLang = null;

    private void Editor_NotifyOverlapLanguage()
    {
        var overlapData = languageDatas.GroupBy(x => x)
                              .Where(g => g.Count() > 1)
                              .Select(y => y.Key)
                              .ToArray();
        if (overlapData.Length > 0)
        {
            if (overlappedLang == null || overlappedLang.Value != overlapData[0].language)
            {
                overlappedLang = overlapData[0].language;
                Debug.LogError($"[ERROR] {overlapData[0].language} 가 {overlapData.Length}개 중복됨!!", gameObject);

                UnityEditor.EditorGUIUtility.PingObject(gameObject);
            }
        }
        else
        {
            if (overlappedLang != null)
            {
                Debug.LogError("[LOG] 중복된 Language가 없어짐.", gameObject);
                overlappedLang = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Editor_CheckModifyLanguageData();
        Editor_NotifyOverlapLanguage();
    }

    private void OnValidate()
    {
        Editor_UpdateLanguageData();
        Editor_NotifyOverlapLanguage();
    }
#endif

    [SerializeField] private LanguageDataContainer[] languageDatas = new LanguageDataContainer[0];

    private static readonly LanguageDataContainer LanguageDataNull = new LanguageDataContainer();

    public bool TryGetLanguageData(Language language, out LanguageDataContainer languageData)
    {
        int index = Array.FindIndex(languageDatas, d => d.language == language);
        if (index >= 0)
        {
            languageData = languageDatas[0];
            return true;
        }
        languageData = LanguageDataNull;
        return false;
    }
}
