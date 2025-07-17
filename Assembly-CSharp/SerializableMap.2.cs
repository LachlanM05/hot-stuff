using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x020000D3 RID: 211
[Serializable]
public class SerializableMap<TKey, TValue> : SerializableMapBase<TKey, TValue, TValue>
{
	// Token: 0x060006FB RID: 1787 RVA: 0x00027BBE File Offset: 0x00025DBE
	public SerializableMap()
	{
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00027BC6 File Offset: 0x00025DC6
	public SerializableMap(IDictionary<TKey, TValue> dict)
		: base(dict)
	{
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00027BCF File Offset: 0x00025DCF
	protected SerializableMap(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00027BD9 File Offset: 0x00025DD9
	protected override TValue GetValue(TValue[] storage, int i)
	{
		return storage[i];
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00027BE2 File Offset: 0x00025DE2
	protected override void SetValue(TValue[] storage, int i, TValue value)
	{
		storage[i] = value;
	}
}
