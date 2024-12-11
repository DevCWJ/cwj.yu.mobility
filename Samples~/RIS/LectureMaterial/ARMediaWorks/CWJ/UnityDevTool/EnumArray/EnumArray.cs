using System;

using UnityEngine;

namespace CWJ
{
    /// <summary>
    /// Enum을 object와 묶어서 배열로만들어주는것
    /// 스크립트에 값 보이게 하고싶으면 자료형정해서 상속받아서 쓰기 (+Serializable 필수)
    /// 상속받았을때 주의사항은 생성자 꼭 적어줘야함
    /// ex) public 자식생성자(int length) : base(length)
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="TValue"> 지금 Component만 제한해놨는데 필요하면 풀기</typeparam>
    public abstract class EnumArray<TEnum, TValue> where TValue : Component where TEnum : struct, Enum
    {
        public TEnum[] enumList;
        [SerializeField] protected TValue[] valueList;

        public int Length
        {
            get
            {
                return valueList.Length;
            }
        }

        public EnumArray(int length)
        {
            enumList = new TEnum[length];
            for (int i = 0; i < length; ++i)
            {
                enumList[i] = i.ToEnum<TEnum>();
            }
            valueList = new TValue[length];
        }

        public TValue this[TEnum key]
        {
            get
            {
                return valueList[key.ToInt()];
            }
            set
            {
                valueList[key.ToInt()] = value;
            }
        }

        public TValue this[int key]
        {
            get
            {
                return valueList[key];
            }
            set
            {
                valueList[key] = value;
            }
        }

        public void AllInit()
        {
            int length = valueList.Length;
            for (int i = 0; i < length; ++i)
            {
                InitValue(valueList[i]);
            }
        }

        public void AllInit(TValue ignoreValue)
        {
            int length = valueList.Length;
            for (int i = 0; i < length; ++i)
            {
                if (ignoreValue == valueList[i])
                {
                    continue;
                }
                InitValue(valueList[i]);
            }
        }

        public abstract void InitValue(TValue value);
    }
}