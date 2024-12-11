
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ.Serializable
{
	public interface IEditableDictionary : IDictionary
	{
		void PrepareForEdit();
		void ApplyEdits();
	}
    /// <summary>
    /// key가 string밖에안됨
    /// </summary>
    [Serializable, Obsolete]
	public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver, IEditableDictionary
	{
		[SerializeField] protected List<TKey> _keys = new List<TKey>();
		[SerializeField] protected List<TValue> _values = new List<TValue>();

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			ConvertToLists();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			ConvertFromLists();

			_keys.Clear();
			_values.Clear();
		}

		public void PrepareForEdit()
		{
			ConvertToLists();
		}

		public void ApplyEdits()
		{
			ConvertFromLists();
		}

		private void ConvertToLists()
		{
			_keys.Clear();
			_values.Clear();

			foreach (var entry in this)
			{
				_keys.Add(entry.Key);
				_values.Add(entry.Value);
			}
		}

		private void ConvertFromLists()
		{
			Clear();

			var count = Math.Min(_keys.Count, _values.Count);

			for (var i = 0; i < count; i++)
				Add(_keys[i], _values[i]);
		}
	}
}