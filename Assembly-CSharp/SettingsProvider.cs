using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Team17.Platform.SaveGame;

// Token: 0x0200018D RID: 397
public abstract class SettingsProvider
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0004D19C File Offset: 0x0004B39C
	public bool IsDirty
	{
		get
		{
			return this._dirty;
		}
	}

	// Token: 0x06000DB7 RID: 3511
	protected abstract Task<SaveGameError> SerialiseData();

	// Token: 0x06000DB8 RID: 3512
	protected abstract Task<SaveGameErrorType> DeserialiseData();

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0004D1A4 File Offset: 0x0004B3A4
	public virtual async void Save(Action<SaveGameError> onCompletioncallback = null, bool force = false)
	{
		SaveGameError result = SaveGameError.None;
		while (this._serialisationState != SettingsProvider.SerialisationState.idle)
		{
			await Task.Yield();
		}
		if (this._dirty || force)
		{
			this._serialisationState = SettingsProvider.SerialisationState.Saving;
			result = await this.SerialiseData();
			if (result.ErrorType == SaveGameErrorType.None)
			{
				this._dirty = false;
			}
		}
		this._serialisationState = SettingsProvider.SerialisationState.idle;
		if (onCompletioncallback != null)
		{
			onCompletioncallback(result);
		}
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0004D1EC File Offset: 0x0004B3EC
	public virtual async void Load(Action<SaveGameErrorType> onCompltionCallback = null)
	{
		while (this._serialisationState != SettingsProvider.SerialisationState.idle)
		{
			await Task.Yield();
		}
		this._serialisationState = SettingsProvider.SerialisationState.Loading;
		SaveGameErrorType saveGameErrorType = await this.DeserialiseData();
		this._dirty = false;
		this._serialisationState = SettingsProvider.SerialisationState.idle;
		if (onCompltionCallback != null)
		{
			onCompltionCallback(saveGameErrorType);
		}
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0004D22B File Offset: 0x0004B42B
	protected virtual string ToJson()
	{
		return JsonConvert.SerializeObject(this._settingsData);
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0004D238 File Offset: 0x0004B438
	protected virtual bool FromJson(string json)
	{
		bool flag = false;
		this._settingsData.Clear();
		try
		{
			if (!string.IsNullOrEmpty(json))
			{
				this._settingsData = JsonConvert.DeserializeObject<SettingsProvider.SettingsData>(json);
				flag = true;
			}
		}
		catch (Exception)
		{
		}
		return flag;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x0004D280 File Offset: 0x0004B480
	public void DeleteAll()
	{
		this._settingsData.Clear();
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x0004D290 File Offset: 0x0004B490
	public void DeleteKey(string key)
	{
		if (this._settingsData.FloatEntries.ContainsKey(key))
		{
			this._settingsData.FloatEntries.Remove(key);
			return;
		}
		if (this._settingsData.IntEntries.ContainsKey(key))
		{
			this._settingsData.IntEntries.Remove(key);
			return;
		}
		if (this._settingsData.StringEntries.ContainsKey(key))
		{
			this._settingsData.StringEntries.Remove(key);
		}
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0004D310 File Offset: 0x0004B510
	public float GetFloat(string key, float defaultValue)
	{
		try
		{
			float num;
			if (this._settingsData.FloatEntries.TryGetValue(key, out num))
			{
				return num;
			}
		}
		catch (Exception)
		{
		}
		return defaultValue;
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0004D350 File Offset: 0x0004B550
	public float GetFloat(string key)
	{
		return this.GetFloat(key, 0f);
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x0004D360 File Offset: 0x0004B560
	public int GetInt(string key, int defaultValue)
	{
		try
		{
			int num;
			if (this._settingsData.IntEntries.TryGetValue(key, out num))
			{
				return num;
			}
		}
		catch (Exception)
		{
		}
		return defaultValue;
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x0004D3A0 File Offset: 0x0004B5A0
	public int GetInt(string key)
	{
		return this.GetInt(key, 0);
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x0004D3AC File Offset: 0x0004B5AC
	public string GetString(string key, string defaultValue)
	{
		try
		{
			string text;
			if (this._settingsData.StringEntries.TryGetValue(key, out text))
			{
				return text;
			}
		}
		catch (Exception)
		{
		}
		return defaultValue;
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x0004D3EC File Offset: 0x0004B5EC
	public string GetString(string key)
	{
		return this.GetString(key, string.Empty);
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x0004D3FC File Offset: 0x0004B5FC
	public bool HasKey(string key)
	{
		return this._settingsData.FloatEntries.ContainsKey(key) || this._settingsData.IntEntries.ContainsKey(key) || this._settingsData.StringEntries.ContainsKey(key);
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x0004D44C File Offset: 0x0004B64C
	public void SetFloat(string key, float value)
	{
		float num;
		if (this._settingsData.FloatEntries.TryGetValue(key, out num) && num == value)
		{
			return;
		}
		this._settingsData.FloatEntries[key] = value;
		this._dirty = true;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x0004D48C File Offset: 0x0004B68C
	public void SetInt(string key, int value)
	{
		int num;
		if (this._settingsData.IntEntries.TryGetValue(key, out num) && num == value)
		{
			return;
		}
		this._settingsData.IntEntries[key] = value;
		this._dirty = true;
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x0004D4CC File Offset: 0x0004B6CC
	public void SetString(string key, string value)
	{
		string text;
		if (this._settingsData.StringEntries.TryGetValue(key, out text) && text == value)
		{
			return;
		}
		this._settingsData.StringEntries[key] = value;
		this._dirty = true;
	}

	// Token: 0x04000C4A RID: 3146
	protected SettingsProvider.SettingsData _settingsData = new SettingsProvider.SettingsData();

	// Token: 0x04000C4B RID: 3147
	protected SettingsProvider.SerialisationState _serialisationState;

	// Token: 0x04000C4C RID: 3148
	protected bool _dirty;

	// Token: 0x02000376 RID: 886
	[Serializable]
	public class SettingsData
	{
		// Token: 0x060017FD RID: 6141 RVA: 0x0006CF9A File Offset: 0x0006B19A
		public void Clear()
		{
			this.FloatEntries.Clear();
			this.IntEntries.Clear();
			this.StringEntries.Clear();
		}

		// Token: 0x04001398 RID: 5016
		public Dictionary<string, float> FloatEntries = new Dictionary<string, float>();

		// Token: 0x04001399 RID: 5017
		public Dictionary<string, int> IntEntries = new Dictionary<string, int>();

		// Token: 0x0400139A RID: 5018
		public Dictionary<string, string> StringEntries = new Dictionary<string, string>();
	}

	// Token: 0x02000377 RID: 887
	public enum SerialisationState
	{
		// Token: 0x0400139C RID: 5020
		idle,
		// Token: 0x0400139D RID: 5021
		Saving,
		// Token: 0x0400139E RID: 5022
		Loading
	}
}
