using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200016E RID: 366
[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	// Token: 0x06000D16 RID: 3350 RVA: 0x0004B52C File Offset: 0x0004972C
	public void OnAfterDeserialize()
	{
		base.Clear();
		foreach (SerializedDictionary<TKey, TValue>.SerializedKeyValuePair serializedKeyValuePair in this.entries)
		{
			base[serializedKeyValuePair.Key] = serializedKeyValuePair.Value;
		}
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x0004B590 File Offset: 0x00049790
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x04000BD7 RID: 3031
	[SerializeField]
	[HideInInspector]
	private List<SerializedDictionary<TKey, TValue>.SerializedKeyValuePair> entries = new List<SerializedDictionary<TKey, TValue>.SerializedKeyValuePair>();

	// Token: 0x02000364 RID: 868
	[Serializable]
	public class SerializedKeyValuePair
	{
		// Token: 0x04001356 RID: 4950
		public TKey Key;

		// Token: 0x04001357 RID: 4951
		public TValue Value;
	}
}
