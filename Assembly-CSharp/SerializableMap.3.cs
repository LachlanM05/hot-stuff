using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x020000D4 RID: 212
[Serializable]
public class SerializableMap<TKey, TValue, TValueStorage> : SerializableMapBase<TKey, TValue, TValueStorage> where TValueStorage : SerializableMap.Storage<TValue>, new()
{
	// Token: 0x06000700 RID: 1792 RVA: 0x00027BEC File Offset: 0x00025DEC
	public SerializableMap()
	{
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00027BF4 File Offset: 0x00025DF4
	public SerializableMap(IDictionary<TKey, TValue> dict)
		: base(dict)
	{
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00027BFD File Offset: 0x00025DFD
	protected SerializableMap(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00027C07 File Offset: 0x00025E07
	protected override TValue GetValue(TValueStorage[] storage, int i)
	{
		return storage[i].data;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00027C1A File Offset: 0x00025E1A
	protected override void SetValue(TValueStorage[] storage, int i, TValue value)
	{
		storage[i] = new TValueStorage();
		storage[i].data = value;
	}
}
