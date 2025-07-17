using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x020000D1 RID: 209
[Serializable]
public abstract class SerializableMapBase<TKey, TValue, TValueStorage> : SerializableMapBase, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
{
	// Token: 0x060006D4 RID: 1748 RVA: 0x00027847 File Offset: 0x00025A47
	public SerializableMapBase()
	{
		this.m_dict = new SerializableMapBase.Dictionary<TKey, TValue>();
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0002785A File Offset: 0x00025A5A
	public SerializableMapBase(IDictionary<TKey, TValue> dict)
	{
		this.m_dict = new SerializableMapBase.Dictionary<TKey, TValue>(dict);
	}

	// Token: 0x060006D6 RID: 1750
	protected abstract void SetValue(TValueStorage[] storage, int i, TValue value);

	// Token: 0x060006D7 RID: 1751
	protected abstract TValue GetValue(TValueStorage[] storage, int i);

	// Token: 0x060006D8 RID: 1752 RVA: 0x00027870 File Offset: 0x00025A70
	public void CopyFrom(IDictionary<TKey, TValue> dict)
	{
		this.m_dict.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
		{
			this.m_dict[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x000278D8 File Offset: 0x00025AD8
	public void OnAfterDeserialize()
	{
		if (this.m_keys != null && this.m_values != null && this.m_keys.Length == this.m_values.Length)
		{
			this.m_dict.Clear();
			int num = this.m_keys.Length;
			for (int i = 0; i < num; i++)
			{
				this.m_dict[this.m_keys[i]] = this.GetValue(this.m_values, i);
			}
			this.m_keys = null;
			this.m_values = null;
		}
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0002795C File Offset: 0x00025B5C
	public void OnBeforeSerialize()
	{
		int count = this.m_dict.Count;
		this.m_keys = new TKey[count];
		this.m_values = new TValueStorage[count];
		int num = 0;
		foreach (KeyValuePair<TKey, TValue> keyValuePair in this.m_dict)
		{
			this.m_keys[num] = keyValuePair.Key;
			this.SetValue(this.m_values, num, keyValuePair.Value);
			num++;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060006DB RID: 1755 RVA: 0x000279FC File Offset: 0x00025BFC
	public ICollection<TKey> Keys
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict).Keys;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060006DC RID: 1756 RVA: 0x00027A09 File Offset: 0x00025C09
	public ICollection<TValue> Values
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict).Values;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060006DD RID: 1757 RVA: 0x00027A16 File Offset: 0x00025C16
	public int Count
	{
		get
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Count;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060006DE RID: 1758 RVA: 0x00027A23 File Offset: 0x00025C23
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).IsReadOnly;
		}
	}

	// Token: 0x17000026 RID: 38
	public TValue this[TKey key]
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict)[key];
		}
		set
		{
			((IDictionary<TKey, TValue>)this.m_dict)[key] = value;
		}
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00027A4D File Offset: 0x00025C4D
	public void Add(TKey key, TValue value)
	{
		((IDictionary<TKey, TValue>)this.m_dict).Add(key, value);
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00027A5C File Offset: 0x00025C5C
	public bool ContainsKey(TKey key)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).ContainsKey(key);
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00027A6A File Offset: 0x00025C6A
	public bool Remove(TKey key)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).Remove(key);
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00027A78 File Offset: 0x00025C78
	public bool TryGetValue(TKey key, out TValue value)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).TryGetValue(key, out value);
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00027A87 File Offset: 0x00025C87
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Add(item);
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00027A95 File Offset: 0x00025C95
	public void Clear()
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Clear();
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00027AA2 File Offset: 0x00025CA2
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Contains(item);
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00027AB0 File Offset: 0x00025CB0
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).CopyTo(array, arrayIndex);
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00027ABF File Offset: 0x00025CBF
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Remove(item);
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00027ACD File Offset: 0x00025CCD
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<TKey, TValue>>)this.m_dict).GetEnumerator();
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00027ADA File Offset: 0x00025CDA
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<TKey, TValue>>)this.m_dict).GetEnumerator();
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060006EC RID: 1772 RVA: 0x00027AE7 File Offset: 0x00025CE7
	public bool IsFixedSize
	{
		get
		{
			return ((IDictionary)this.m_dict).IsFixedSize;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060006ED RID: 1773 RVA: 0x00027AF4 File Offset: 0x00025CF4
	ICollection IDictionary.Keys
	{
		get
		{
			return ((IDictionary)this.m_dict).Keys;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060006EE RID: 1774 RVA: 0x00027B01 File Offset: 0x00025D01
	ICollection IDictionary.Values
	{
		get
		{
			return ((IDictionary)this.m_dict).Values;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060006EF RID: 1775 RVA: 0x00027B0E File Offset: 0x00025D0E
	public bool IsSynchronized
	{
		get
		{
			return ((ICollection)this.m_dict).IsSynchronized;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00027B1B File Offset: 0x00025D1B
	public object SyncRoot
	{
		get
		{
			return ((ICollection)this.m_dict).SyncRoot;
		}
	}

	// Token: 0x1700002C RID: 44
	public object this[object key]
	{
		get
		{
			return ((IDictionary)this.m_dict)[key];
		}
		set
		{
			((IDictionary)this.m_dict)[key] = value;
		}
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00027B45 File Offset: 0x00025D45
	public void Add(object key, object value)
	{
		((IDictionary)this.m_dict).Add(key, value);
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00027B54 File Offset: 0x00025D54
	public bool Contains(object key)
	{
		return ((IDictionary)this.m_dict).Contains(key);
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00027B62 File Offset: 0x00025D62
	IDictionaryEnumerator IDictionary.GetEnumerator()
	{
		return ((IDictionary)this.m_dict).GetEnumerator();
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00027B6F File Offset: 0x00025D6F
	public void Remove(object key)
	{
		((IDictionary)this.m_dict).Remove(key);
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00027B7D File Offset: 0x00025D7D
	public void CopyTo(Array array, int index)
	{
		((ICollection)this.m_dict).CopyTo(array, index);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00027B8C File Offset: 0x00025D8C
	public void OnDeserialization(object sender)
	{
		((IDeserializationCallback)this.m_dict).OnDeserialization(sender);
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00027B9A File Offset: 0x00025D9A
	protected SerializableMapBase(SerializationInfo info, StreamingContext context)
	{
		this.m_dict = new SerializableMapBase.Dictionary<TKey, TValue>(info, context);
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00027BAF File Offset: 0x00025DAF
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		((ISerializable)this.m_dict).GetObjectData(info, context);
	}

	// Token: 0x04000601 RID: 1537
	private SerializableMapBase.Dictionary<TKey, TValue> m_dict;

	// Token: 0x04000602 RID: 1538
	[SerializeField]
	private TKey[] m_keys;

	// Token: 0x04000603 RID: 1539
	[SerializeField]
	private TValueStorage[] m_values;
}
