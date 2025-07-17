using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x020000D0 RID: 208
public abstract class SerializableMapBase
{
	// Token: 0x020002F9 RID: 761
	public abstract class Storage
	{
	}

	// Token: 0x020002FA RID: 762
	protected class Dictionary<TKey, TValue> : global::System.Collections.Generic.Dictionary<TKey, TValue>
	{
		// Token: 0x06001678 RID: 5752 RVA: 0x00068E49 File Offset: 0x00067049
		public Dictionary()
		{
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00068E51 File Offset: 0x00067051
		public Dictionary(IDictionary<TKey, TValue> dict)
			: base(dict)
		{
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00068E5A File Offset: 0x0006705A
		public Dictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
