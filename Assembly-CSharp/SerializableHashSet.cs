using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x020000CF RID: 207
[Serializable]
public abstract class SerializableHashSet<T> : SerializableHashSetBase, ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
{
	// Token: 0x060006B7 RID: 1719 RVA: 0x000275A8 File Offset: 0x000257A8
	public SerializableHashSet()
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>();
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x000275BB File Offset: 0x000257BB
	public SerializableHashSet(ISet<T> set)
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>(set);
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x000275D0 File Offset: 0x000257D0
	public void CopyFrom(ISet<T> set)
	{
		this.m_hashSet.Clear();
		foreach (T t in set)
		{
			this.m_hashSet.Add(t);
		}
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x0002762C File Offset: 0x0002582C
	public void OnAfterDeserialize()
	{
		if (this.m_keys != null)
		{
			this.m_hashSet.Clear();
			int num = this.m_keys.Length;
			for (int i = 0; i < num; i++)
			{
				this.m_hashSet.Add(this.m_keys[i]);
			}
			this.m_keys = null;
		}
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00027680 File Offset: 0x00025880
	public void OnBeforeSerialize()
	{
		int count = this.m_hashSet.Count;
		this.m_keys = new T[count];
		int num = 0;
		foreach (T t in this.m_hashSet)
		{
			this.m_keys[num] = t;
			num++;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060006BC RID: 1724 RVA: 0x000276F8 File Offset: 0x000258F8
	public int Count
	{
		get
		{
			return ((ICollection<T>)this.m_hashSet).Count;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060006BD RID: 1725 RVA: 0x00027705 File Offset: 0x00025905
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.m_hashSet).IsReadOnly;
		}
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00027712 File Offset: 0x00025912
	public bool Add(T item)
	{
		return ((ISet<T>)this.m_hashSet).Add(item);
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x00027720 File Offset: 0x00025920
	public void ExceptWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).ExceptWith(other);
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0002772E File Offset: 0x0002592E
	public void IntersectWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).IntersectWith(other);
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0002773C File Offset: 0x0002593C
	public bool IsProperSubsetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsProperSubsetOf(other);
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002774A File Offset: 0x0002594A
	public bool IsProperSupersetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsProperSupersetOf(other);
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x00027758 File Offset: 0x00025958
	public bool IsSubsetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsSubsetOf(other);
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00027766 File Offset: 0x00025966
	public bool IsSupersetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsSupersetOf(other);
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x00027774 File Offset: 0x00025974
	public bool Overlaps(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).Overlaps(other);
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00027782 File Offset: 0x00025982
	public bool SetEquals(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).SetEquals(other);
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00027790 File Offset: 0x00025990
	public void SymmetricExceptWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).SymmetricExceptWith(other);
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x0002779E File Offset: 0x0002599E
	public void UnionWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).UnionWith(other);
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x000277AC File Offset: 0x000259AC
	void ICollection<T>.Add(T item)
	{
		((ISet<T>)this.m_hashSet).Add(item);
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x000277BB File Offset: 0x000259BB
	public void Clear()
	{
		((ICollection<T>)this.m_hashSet).Clear();
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x000277C8 File Offset: 0x000259C8
	public bool Contains(T item)
	{
		return ((ICollection<T>)this.m_hashSet).Contains(item);
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x000277D6 File Offset: 0x000259D6
	public void CopyTo(T[] array, int arrayIndex)
	{
		((ICollection<T>)this.m_hashSet).CopyTo(array, arrayIndex);
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x000277E5 File Offset: 0x000259E5
	public bool Remove(T item)
	{
		return ((ICollection<T>)this.m_hashSet).Remove(item);
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x000277F3 File Offset: 0x000259F3
	public IEnumerator<T> GetEnumerator()
	{
		return ((IEnumerable<T>)this.m_hashSet).GetEnumerator();
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00027800 File Offset: 0x00025A00
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<T>)this.m_hashSet).GetEnumerator();
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0002780D File Offset: 0x00025A0D
	public void OnDeserialization(object sender)
	{
		((IDeserializationCallback)this.m_hashSet).OnDeserialization(sender);
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x0002781B File Offset: 0x00025A1B
	protected SerializableHashSet(SerializationInfo info, StreamingContext context)
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>(info, context);
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00027830 File Offset: 0x00025A30
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		((ISerializable)this.m_hashSet).GetObjectData(info, context);
	}

	// Token: 0x040005FF RID: 1535
	private SerializableHashSetBase.HashSet<T> m_hashSet;

	// Token: 0x04000600 RID: 1536
	[SerializeField]
	private T[] m_keys;
}
