using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x020000CE RID: 206
public abstract class SerializableHashSetBase
{
	// Token: 0x020002F7 RID: 759
	public abstract class Storage
	{
	}

	// Token: 0x020002F8 RID: 760
	protected class HashSet<TValue> : global::System.Collections.Generic.HashSet<TValue>
	{
		// Token: 0x06001674 RID: 5748 RVA: 0x00068E26 File Offset: 0x00067026
		public HashSet()
		{
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00068E2E File Offset: 0x0006702E
		public HashSet(ISet<TValue> set)
			: base(set)
		{
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00068E37 File Offset: 0x00067037
		public HashSet(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
