using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using T17.Services;
using Team17.Common;
using Team17.Scripts.Services.AssetProvider;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000028 RID: 40
public class AudioManager : Singleton<AudioManager>
{
	// Token: 0x060000BF RID: 191 RVA: 0x00004E13 File Offset: 0x00003013
	public void ReggieMusic()
	{
		base.Invoke("ReggieMusicInvoked", 2.314f);
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00004E28 File Offset: 0x00003028
	private void ReggieMusicInvoked()
	{
		Singleton<AudioManager>.Instance.PlayTrack("Character Themes/reggie_music", AUDIO_TYPE.MUSIC, true, false, 0.5f, false, 1f, null, true, null, SFX_SUBGROUP.NONE, null, false);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00004E5C File Offset: 0x0000305C
	public void PlayTrack(AudioClip track, AUDIO_TYPE type, bool pauseOthersOfType, bool pauseOthersNotOfType, float fadeTime = 0f, bool playOverOtherSounds = false, float lowerVolumeOfOthers = 1f, GameObject objectFor3dSound = null, bool loopSfx = false, SFX_SUBGROUP subgroup = SFX_SUBGROUP.NONE, bool isMagicalFoley = false)
	{
		if (track == null)
		{
			return;
		}
		this.PlayTrack(track.name, type, pauseOthersOfType, pauseOthersNotOfType, fadeTime, playOverOtherSounds, lowerVolumeOfOthers, objectFor3dSound, loopSfx, track, subgroup, null, isMagicalFoley);
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00004E94 File Offset: 0x00003094
	public void PlayTrackWithIntro(string track, string trackIntro)
	{
		this.LoadTrack(track, AUDIO_TYPE.MUSIC, null, false, null, SFX_SUBGROUP.NONE);
		Singleton<AudioManager>.Instance.ResumeTrack(track, true, false, 0.5f);
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00004EB4 File Offset: 0x000030B4
	public IEnumerator FinishPlaying(float introLength, string track, string trackIntro)
	{
		yield return new WaitForSeconds(introLength);
		Singleton<AudioManager>.Instance.ResumeTrack(track, true, false, 0f);
		yield return new WaitForEndOfFrame();
		Singleton<AudioManager>.Instance.StopTrack(trackIntro, 0f);
		yield break;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00004ED4 File Offset: 0x000030D4
	private IEnumerator PlayTrackNextFrame(string track, AUDIO_TYPE type, bool pauseOthersOfType, bool pauseOthersNotOfType, float fadeTime, bool playOverOtherSounds, float lowerVolumeOfOthers, GameObject objectFor3dSound, bool loopSfx, AudioClip providedTrack, SFX_SUBGROUP subgroup, string trackLoop)
	{
		yield return null;
		this.PlayTrack(track, type, pauseOthersOfType, pauseOthersNotOfType, fadeTime, playOverOtherSounds, lowerVolumeOfOthers, objectFor3dSound, loopSfx, providedTrack, subgroup, trackLoop, false);
		yield break;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00004F4C File Offset: 0x0000314C
	public void PlayTrackAfterWait(string track, AUDIO_TYPE type, bool pauseOthersOfType, bool pauseOthersNotOfType, float fadeTime, bool playOverOtherSounds, float lowerVolumeOfOthers, float waitTimeInSeconds)
	{
		base.StartCoroutine(this.PlayTrackAfterWaitInternal(track, type, pauseOthersOfType, pauseOthersNotOfType, fadeTime, playOverOtherSounds, lowerVolumeOfOthers, waitTimeInSeconds));
		if (!this.DictFutureTracks.Keys.Contains(type))
		{
			this.DictFutureTracks.Add(type, new List<string>());
		}
		this.DictFutureTracks[type].Add(track);
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00004FAC File Offset: 0x000031AC
	private IEnumerator PlayTrackAfterWaitInternal(string track, AUDIO_TYPE type, bool pauseOthersOfType, bool pauseOthersNotOfType, float fadeTime, bool playOverOtherSounds, float lowerVolumeOfOthers, float waitTimeInSeconds)
	{
		yield return new WaitForSeconds(waitTimeInSeconds);
		if (this.DictFutureTracks[type].Contains(track))
		{
			this.PlayTrack(track, type, pauseOthersOfType, pauseOthersNotOfType, fadeTime, playOverOtherSounds, lowerVolumeOfOthers, null, false, null, SFX_SUBGROUP.NONE, null, false);
			this.DictFutureTracks[type].Remove(track);
		}
		yield break;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005004 File Offset: 0x00003204
	public float PlayTrack(string track, AUDIO_TYPE type, bool pauseOthersOfType, bool pauseOthersNotOfType, float fadeTime = 0f, bool playOverOtherSounds = false, float lowerVolumeOfOthers = 1f, GameObject objectFor3dSound = null, bool loopSfx = false, AudioClip providedTrack = null, SFX_SUBGROUP subgroup = SFX_SUBGROUP.NONE, string trackLoop = null, bool isMagicalFoley = false)
	{
		bool flag = this.debugAudioStarts;
		if (track == "")
		{
			return 0f;
		}
		if (!AssetProviderExtensions.CanDoAsyncAssetLoading)
		{
			Coroutine coroutine = base.StartCoroutine(this.PlayTrackNextFrame(track, type, pauseOthersOfType, pauseOthersNotOfType, fadeTime, playOverOtherSounds, lowerVolumeOfOthers, objectFor3dSound, loopSfx, providedTrack, subgroup, trackLoop));
			this.QueueCoroutineForTrack(track, coroutine);
			return 0f;
		}
		if (this.TrackDictionary.ContainsKey(track))
		{
			if (playOverOtherSounds || !this.TrackDictionary[track].isplaying || track.EndsWith("_Intro"))
			{
				this.TrackDictionary[track].Play(fadeTime);
				return 0f;
			}
			return 0f;
		}
		else
		{
			AudioManager.MusicChild musicChild = this.NewTrack(track, type, objectFor3dSound, loopSfx, providedTrack, subgroup, fadeTime, isMagicalFoley);
			if (musicChild != null)
			{
				this.TrackDictionary.Add(track, musicChild);
				this.CurrentTracks.Add(musicChild);
				this.testCurrentTrack = musicChild;
				if (this.CurrentTracks.Count > 1)
				{
					for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
					{
						AudioManager.MusicChild musicChild2 = this.CurrentTracks[i];
						if (musicChild2 != null)
						{
							if (musicChild2.Type == type)
							{
								if (musicChild2.Name != track && pauseOthersOfType && (trackLoop == null || (trackLoop != null && musicChild2.name != trackLoop)))
								{
									musicChild2.Stop(fadeTime);
								}
							}
							else if (pauseOthersNotOfType)
							{
								musicChild2.Stop(fadeTime);
							}
						}
					}
					base.StartCoroutine(this.DelayStart(track, fadeTime));
				}
				else
				{
					this.TrackDictionary[track].Play(fadeTime);
				}
				if (lowerVolumeOfOthers != 1f)
				{
					base.StartCoroutine(this.LowerVolumeOfOthersWhilePlaying(type, this.TrackDictionary[track].GetSong().length, lowerVolumeOfOthers));
				}
				return this.TrackDictionary[track].GetSong().length;
			}
			MonoBehaviour.print("<color=#E867FF>[AUDIO MANAGER]</color> Failed to find track '" + track + "'");
			return 0f;
		}
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005200 File Offset: 0x00003400
	private void QueueCoroutineForTrack(string track, Coroutine coroutine)
	{
		List<Coroutine> list;
		if (!this._queuedPlayTracks.TryGetValue(track, out list))
		{
			list = new List<Coroutine>();
			this._queuedPlayTracks.Add(track, list);
		}
		list.Add(coroutine);
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005238 File Offset: 0x00003438
	public void LoadTrack(string track, AUDIO_TYPE type, GameObject objectFor3dSound = null, bool loopSfx = false, AudioClip audioClip = null, SFX_SUBGROUP subgroup = SFX_SUBGROUP.NONE)
	{
		if (this.TrackDictionary.ContainsKey(track))
		{
			return;
		}
		AudioManager.MusicChild musicChild = this.NewTrack(track, type, objectFor3dSound, loopSfx, audioClip, subgroup, 0f, false);
		this.TrackDictionary.Add(track, musicChild);
		this.CurrentTracks.Add(musicChild);
		this.testCurrentTrack = musicChild;
		this.TrackDictionary[track].Play(0.5f);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x000052A0 File Offset: 0x000034A0
	public float GetTrackDuration(string track, GameObject objectFor3dSound = null)
	{
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null && musicChild.Name == track)
			{
				return musicChild.GetSong().length;
			}
		}
		AudioManager.MusicChild musicChild2 = this.NewTrack(track, AUDIO_TYPE.MUSIC, objectFor3dSound, false, null, SFX_SUBGROUP.NONE, 0f, false);
		this.TrackDictionary.Add(track, musicChild2);
		this.CurrentTracks.Add(musicChild2);
		this.testCurrentTrack = musicChild2;
		return musicChild2.GetSong().length;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005354 File Offset: 0x00003554
	public IEnumerator LowerVolumeOfOthersWhilePlaying(AUDIO_TYPE type, float duration, float volume)
	{
		if (type != AUDIO_TYPE.DIALOGUE)
		{
			this.SetTrackGroupVolume(AUDIO_TYPE.DIALOGUE, volume);
		}
		if (type != AUDIO_TYPE.MUSIC)
		{
			this.SetTrackGroupVolume(AUDIO_TYPE.MUSIC, volume);
		}
		if (type != AUDIO_TYPE.SFX)
		{
			this.SetTrackGroupVolume(AUDIO_TYPE.SFX, volume);
		}
		yield return new WaitForSeconds(duration);
		if (type != AUDIO_TYPE.DIALOGUE)
		{
			this.SetTrackGroupVolume(AUDIO_TYPE.DIALOGUE, 1f);
		}
		if (type != AUDIO_TYPE.MUSIC)
		{
			this.SetTrackGroupVolume(AUDIO_TYPE.MUSIC, 1f);
		}
		if (type != AUDIO_TYPE.SFX)
		{
			this.SetTrackGroupVolume(AUDIO_TYPE.SFX, 1f);
		}
		if (type != AUDIO_TYPE.SPECIAL)
		{
			this.SetTrackGroupVolume(AUDIO_TYPE.SPECIAL, 1f);
		}
		yield break;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005378 File Offset: 0x00003578
	public void SetTrackGroupVolume(AUDIO_TYPE type, float volume)
	{
		for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
		{
			AudioManager.MusicChild musicChild = this.CurrentTracks[i];
			if (musicChild != null && musicChild.Type == type)
			{
				musicChild.GetAudio().volume = volume;
			}
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x000053C8 File Offset: 0x000035C8
	public void SetMusicFrequency(float frequency, bool crossfade, float fadeTime)
	{
		if (!crossfade)
		{
			this.MasterMixer.SetFloat("music_frequency", frequency);
			return;
		}
		float num;
		this.MasterMixer.GetFloat("music_frequency", out num);
		base.StartCoroutine(this.CrossFade(num, frequency, fadeTime));
	}

	// Token: 0x060000CE RID: 206 RVA: 0x0000540E File Offset: 0x0000360E
	private IEnumerator CrossFade(float initialFrequency, float targetFrequency, float duration)
	{
		for (float f = 0f; f < duration; f += Time.deltaTime)
		{
			this.MasterMixer.SetFloat("music_frequency", Mathf.Lerp(initialFrequency, targetFrequency, f / duration));
			yield return new WaitForEndOfFrame();
		}
		this.MasterMixer.SetFloat("music_frequency", targetFrequency);
		yield break;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005432 File Offset: 0x00003632
	public void SetMusicResonance(float resonance)
	{
		this.MasterMixer.SetFloat("music_resonance", resonance);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005448 File Offset: 0x00003648
	public void SetTrackGroupPitch(AUDIO_TYPE type, float pitch)
	{
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null && musicChild.Type == type)
			{
				musicChild.GetAudio().pitch = pitch;
			}
		}
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000054B4 File Offset: 0x000036B4
	public void SetTrackGroupResonance(AUDIO_TYPE type, float resonance)
	{
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null && musicChild.Type == type)
			{
				musicChild.GetAudio().spatialBlend = resonance;
			}
		}
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005520 File Offset: 0x00003720
	public void PauseTrack(string track, float fadeTime = 0.5f)
	{
		if (!this.TrackDictionary.ContainsKey(track))
		{
			return;
		}
		bool flag = this.debugAudioPauses;
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null && musicChild.Name == track)
			{
				musicChild.Pause(fadeTime);
			}
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x000055A0 File Offset: 0x000037A0
	public void ResumeTrack(string track, bool pauseOthersOfType, bool pauseOthersNotOfType, float fadeTime = 0.5f)
	{
		if (!this.TrackDictionary.ContainsKey(track))
		{
			return;
		}
		bool flag = this.debugAudioResumes;
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null)
			{
				if (musicChild.Name != track)
				{
					if (musicChild.Type == this.TrackDictionary[track].Type)
					{
						if (musicChild.Name != track && pauseOthersOfType)
						{
							musicChild.Pause(fadeTime);
						}
					}
					else if (pauseOthersNotOfType)
					{
						musicChild.Pause(fadeTime);
					}
				}
				else
				{
					musicChild.Play(fadeTime);
				}
			}
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005664 File Offset: 0x00003864
	private void RemoveTrackIfQueuedToPlay(string track)
	{
		List<Coroutine> list;
		if (!this._queuedPlayTracks.TryGetValue(track, out list))
		{
			return;
		}
		foreach (Coroutine coroutine in list)
		{
			base.StopCoroutine(coroutine);
		}
		list.Clear();
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000056CC File Offset: 0x000038CC
	public void StopTrack(string track, float fadeTime = 0f)
	{
		bool flag = this.debugAudioStops;
		this.RemoveTrackIfQueuedToPlay(track);
		if (this.TrackDictionary == null)
		{
			return;
		}
		if (!this.TrackDictionary.ContainsKey(track))
		{
			return;
		}
		this.TrackDictionary[track].isplaying = false;
		this.TrackDictionary.Remove(track);
		for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
		{
			AudioManager.MusicChild musicChild = this.CurrentTracks[i];
			if (musicChild != null && musicChild.Name == track)
			{
				musicChild.isplaying = false;
				musicChild.Stop(fadeTime);
			}
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00005768 File Offset: 0x00003968
	public bool IsPlayingTrack(string track)
	{
		return this.TrackDictionary != null && this.TrackDictionary.ContainsKey(track);
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00005780 File Offset: 0x00003980
	public void FadeTrackOut(string track, float fadeTime = 0.5f)
	{
		this.RemoveTrackIfQueuedToPlay(track);
		bool flag = this.debugAudioFades;
		if (!this.TrackDictionary.ContainsKey(track))
		{
			return;
		}
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null && musicChild.Name == track)
			{
				musicChild.Mute(fadeTime);
			}
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00005808 File Offset: 0x00003A08
	public void FadeTrackIn(string track, float fadeTime = 0.5f)
	{
		if (!this.TrackDictionary.ContainsKey(track))
		{
			return;
		}
		bool flag = this.debugAudioFades;
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null && musicChild.Name == track)
			{
				musicChild.UnMute(fadeTime);
			}
		}
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00005888 File Offset: 0x00003A88
	public void PauseTrackGroup(AUDIO_TYPE type, float fadeTime = 0.5f)
	{
		bool flag = this.debugAudioPauses;
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null && musicChild.Type == type)
			{
				musicChild.Pause(fadeTime);
			}
		}
	}

	// Token: 0x060000DA RID: 218 RVA: 0x000058F4 File Offset: 0x00003AF4
	public void ResumeTrackGroup(AUDIO_TYPE type, bool pauseOthers, float fadeTime = 0.5f)
	{
		bool flag = this.debugAudioResumes;
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null)
			{
				if (musicChild.Type != type)
				{
					if (pauseOthers)
					{
						musicChild.Pause(fadeTime);
					}
					else
					{
						musicChild.Play(fadeTime);
					}
				}
				else
				{
					musicChild.Play(fadeTime);
				}
			}
		}
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00005978 File Offset: 0x00003B78
	public void StopTrackGroup(AUDIO_TYPE type, float fadeTime = 0.5f)
	{
		bool flag = this.debugAudioStops;
		for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
		{
			AudioManager.MusicChild musicChild = this.CurrentTracks[i];
			if (musicChild != null && musicChild.Type == type)
			{
				musicChild.Stop(fadeTime);
			}
		}
		if (this.DictFutureTracks.Keys.Contains(type))
		{
			this.DictFutureTracks[type].Clear();
		}
	}

	// Token: 0x060000DC RID: 220 RVA: 0x000059F0 File Offset: 0x00003BF0
	public void StopSFXTrackGroup(SFX_SUBGROUP subgroup, float fadeTime = 0.5f)
	{
		bool flag = this.debugAudioStops;
		for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
		{
			AudioManager.MusicChild musicChild = this.CurrentTracks[i];
			if (musicChild != null && musicChild.Type == AUDIO_TYPE.SFX && musicChild.Subgroup == subgroup)
			{
				musicChild.Stop(fadeTime);
			}
		}
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00005A4C File Offset: 0x00003C4C
	public void PauseAll(float fadeTime = 0.5f)
	{
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null)
			{
				musicChild.Pause(fadeTime);
			}
		}
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00005AA8 File Offset: 0x00003CA8
	public void StopAll(float fadeTime = 0.5f)
	{
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null)
			{
				musicChild.Stop(fadeTime);
			}
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00005B04 File Offset: 0x00003D04
	private IEnumerator DelayStart(string track, float time)
	{
		yield return new WaitForSeconds(time);
		if (this.TrackDictionary.ContainsKey(track))
		{
			this.TrackDictionary[track].Play(time);
		}
		yield break;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00005B24 File Offset: 0x00003D24
	public void RemoveTrack(string track)
	{
		if (!this.TrackDictionary.ContainsKey(track))
		{
			return;
		}
		AudioManager.MusicChild musicChild = this.TrackDictionary[track];
		this.TrackDictionary.Remove(track);
		for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
		{
			if (this.CurrentTracks[i] != null && this.CurrentTracks[i].Name == track)
			{
				this.CurrentTracks[i] = null;
			}
		}
		Object.Destroy(musicChild.gameObject);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00005BB8 File Offset: 0x00003DB8
	private AudioManager.MusicChild NewTrack(string track, AUDIO_TYPE type, GameObject objectFor3dSound, bool loopSfx = false, AudioClip _clip = null, SFX_SUBGROUP subgroup = SFX_SUBGROUP.NONE, float fadeTime = 0f, bool isMagicalFoley = false)
	{
		bool flag = true;
		AudioClip audioClip;
		if (_clip != null)
		{
			audioClip = _clip;
			flag = false;
		}
		else
		{
			if (type == AUDIO_TYPE.DIALOGUE && !track.Contains("_"))
			{
				type = AUDIO_TYPE.SFX;
			}
			switch (type)
			{
			case AUDIO_TYPE.MUSIC:
			{
				string text = Path.Combine("Audio", "Music", "Character Themes", track);
				audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, true);
				if (audioClip == null)
				{
					text = Path.Combine("Audio", "Music", track);
					audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
					if (audioClip == null)
					{
						T17Debug.LogError("No Track Music/" + track + " Found!");
						audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(Path.Combine("Audio", "Music", "date_everything_random_song"), false);
					}
				}
				break;
			}
			case AUDIO_TYPE.SFX:
			{
				string text = Path.Combine("Audio", "Sfx", track);
				audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
				if (audioClip == null)
				{
					T17Debug.LogError("No Track SFX/" + track + " Found!");
					audioClip = SFXBank.Instance.ui_select;
					flag = false;
				}
				break;
			}
			case AUDIO_TYPE.DIALOGUE:
			{
				string text = Path.Combine("Audio", "Dialogue", track);
				audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, true);
				this.recentVOFound = true;
				if (audioClip == null)
				{
					string text2 = track.Replace("\\", "/");
					string text3 = text2.Substring(0, text2.IndexOf("/"));
					string text4 = text2.Substring(text2.IndexOf("/") + 1);
					string text5 = text4.Substring(text4.IndexOf("/") + 1);
					text4 = text4.Substring(0, text4.IndexOf("/"));
					string text6 = text5.Substring(0, text5.IndexOf("_"));
					if (text4.Trim() != text6.Trim())
					{
						text = Path.Combine(new string[] { "Audio", "Dialogue", text3, text6, text5 });
						audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
						this.recentVOFound = true;
					}
					if (audioClip == null)
					{
						audioClip = SFXBank.Instance.ui_dialogue_confirm;
						flag = false;
						this.recentVOFound = false;
					}
				}
				break;
			}
			case AUDIO_TYPE.SPECIAL:
			{
				string text = Path.Combine("Audio", "Music", track);
				audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
				if (audioClip == null)
				{
					text = Path.Combine("Audio", "Music", "Character Themes", track);
					audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
					if (audioClip == null)
					{
						T17Debug.LogError("No Track Music/Character Themes/" + track + " Found!");
						audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(Path.Combine("Audio", "Music", "date_everything_random_song"), false);
					}
				}
				break;
			}
			default:
			{
				string text = Path.Combine("Audio", track);
				audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, true);
				if (audioClip == null)
				{
					text = Path.Combine("Audio", "Music", track);
					audioClip = Services.AssetProviderService.LoadResourceAsset<AudioClip>(text, false);
					if (audioClip == null)
					{
						T17Debug.LogError("No Track " + track + " Found!");
					}
				}
				break;
			}
			}
		}
		if (track.Contains("VoiceOver") && !isMagicalFoley)
		{
			type = AUDIO_TYPE.DIALOGUE;
		}
		if (audioClip == null)
		{
			T17Debug.LogError("No Track " + track + " Found!");
			return null;
		}
		GameObject gameObject = new GameObject();
		gameObject.transform.name = track;
		gameObject.transform.SetParent(base.transform);
		if (objectFor3dSound != null)
		{
			gameObject.transform.position = objectFor3dSound.transform.position;
		}
		else
		{
			gameObject.transform.position = base.transform.position;
		}
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		if (objectFor3dSound != null)
		{
			audioSource.maxDistance = 30f;
			audioSource.spatialize = true;
			audioSource.spatialBlend = 1f;
			audioSource.rolloffMode = AudioRolloffMode.Linear;
		}
		AudioMixerGroup audioMixerGroup;
		switch (type)
		{
		case AUDIO_TYPE.MUSIC:
			audioMixerGroup = this.MusicGroup;
			switch (subgroup)
			{
			case SFX_SUBGROUP.UI:
				if (this.SfxUiGroup)
				{
					audioMixerGroup = this.SfxUiGroup;
				}
				break;
			case SFX_SUBGROUP.FOLEY:
				if (this.SfxFoleyGroup)
				{
					audioMixerGroup = this.SfxFoleyGroup;
				}
				if (isMagicalFoley)
				{
					audioMixerGroup = this.Sfx3DVoiceGroup;
				}
				break;
			case SFX_SUBGROUP.AMBIENT:
				if (this.SfxAmbientGroup)
				{
					audioMixerGroup = this.SfxAmbientGroup;
				}
				break;
			case SFX_SUBGROUP.STINGER:
				if (this.SfxStingerGroup)
				{
					audioMixerGroup = this.SfxStingerGroup;
				}
				break;
			case SFX_SUBGROUP.EMOTE:
				if (this.SfxEmoteGroup)
				{
					audioMixerGroup = this.SfxEmoteGroup;
				}
				break;
			default:
				loopSfx = true;
				break;
			}
			break;
		case AUDIO_TYPE.SFX:
			audioMixerGroup = this.SfxGroup;
			switch (subgroup)
			{
			case SFX_SUBGROUP.UI:
				if (this.SfxUiGroup)
				{
					audioMixerGroup = this.SfxUiGroup;
				}
				break;
			case SFX_SUBGROUP.FOLEY:
				if (this.SfxFoleyGroup)
				{
					audioMixerGroup = this.SfxFoleyGroup;
				}
				if (isMagicalFoley)
				{
					audioMixerGroup = this.Sfx3DVoiceGroup;
				}
				break;
			case SFX_SUBGROUP.AMBIENT:
				if (this.SfxAmbientGroup)
				{
					audioMixerGroup = this.SfxAmbientGroup;
				}
				break;
			case SFX_SUBGROUP.STINGER:
				if (this.SfxStingerGroup)
				{
					audioMixerGroup = this.SfxStingerGroup;
				}
				break;
			case SFX_SUBGROUP.EMOTE:
				if (this.SfxEmoteGroup)
				{
					audioMixerGroup = this.SfxEmoteGroup;
				}
				break;
			}
			break;
		case AUDIO_TYPE.DIALOGUE:
			audioMixerGroup = this.DialogueGroup;
			break;
		case AUDIO_TYPE.SPECIAL:
			audioMixerGroup = this.SpecialGroup;
			break;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
		return gameObject.AddComponent<AudioManager.MusicChild>().Init(track, type, audioSource, audioClip, audioMixerGroup, loopSfx, flag, fadeTime, subgroup, objectFor3dSound);
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x000061AC File Offset: 0x000043AC
	public float GetLengthOfCurrentSongOfType(AUDIO_TYPE audioType)
	{
		float num = 0f;
		if (this.CurrentTracks != null && this.CurrentTracks.Count > 0)
		{
			for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
			{
				AudioManager.MusicChild musicChild = this.CurrentTracks[i];
				if (musicChild != null && musicChild.Type == audioType)
				{
					num = this.CurrentTracks[i].GetSong().length;
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00006228 File Offset: 0x00004428
	public void StopAllTracksForGameObject(GameObject gameObjectOwner)
	{
		List<string> list = new List<string>();
		foreach (AudioManager.MusicChild musicChild in this.CurrentTracks)
		{
			if (musicChild != null && musicChild.HasOwner && musicChild.GameObjectOwner == gameObjectOwner)
			{
				list.Add(musicChild.Name);
			}
		}
		foreach (string text in list)
		{
			this.StopTrack(text, 0f);
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000062EC File Offset: 0x000044EC
	private void Update()
	{
		this.CleanUpCurrentTrack();
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000062F4 File Offset: 0x000044F4
	private void CleanUpCurrentTrack()
	{
		if (this.CurrentTracks != null)
		{
			for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
			{
				if (this.CurrentTracks[i] == null)
				{
					this.CurrentTracks.RemoveAll((AudioManager.MusicChild child) => child == null);
					return;
				}
			}
		}
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00006364 File Offset: 0x00004564
	public AudioManager.MusicChild GetTrack(string name)
	{
		for (int i = this.CurrentTracks.Count - 1; i >= 0; i--)
		{
			AudioManager.MusicChild musicChild = this.CurrentTracks[i];
			if (musicChild != null && string.CompareOrdinal(musicChild.Name, name) == 0)
			{
				return musicChild;
			}
		}
		return null;
	}

	// Token: 0x040000CB RID: 203
	[Header("Audio Groups")]
	public AudioMixerGroup MusicGroup;

	// Token: 0x040000CC RID: 204
	public AudioMixerGroup SfxGroup;

	// Token: 0x040000CD RID: 205
	public AudioMixerGroup DialogueGroup;

	// Token: 0x040000CE RID: 206
	public AudioMixerGroup SpecialGroup;

	// Token: 0x040000CF RID: 207
	public AudioMixerGroup SfxUiGroup;

	// Token: 0x040000D0 RID: 208
	public AudioMixerGroup SfxFoleyGroup;

	// Token: 0x040000D1 RID: 209
	public AudioMixerGroup SfxAmbientGroup;

	// Token: 0x040000D2 RID: 210
	public AudioMixerGroup SfxStingerGroup;

	// Token: 0x040000D3 RID: 211
	public AudioMixerGroup SfxEmoteGroup;

	// Token: 0x040000D4 RID: 212
	public AudioMixerGroup Sfx3DVoiceGroup;

	// Token: 0x040000D5 RID: 213
	public AudioMixer MasterMixer;

	// Token: 0x040000D6 RID: 214
	public List<AudioManager.MusicChild> CurrentTracks = new List<AudioManager.MusicChild>();

	// Token: 0x040000D7 RID: 215
	private Dictionary<AUDIO_TYPE, List<string>> DictFutureTracks = new Dictionary<AUDIO_TYPE, List<string>>();

	// Token: 0x040000D8 RID: 216
	private Dictionary<string, AudioManager.MusicChild> TrackDictionary = new Dictionary<string, AudioManager.MusicChild>();

	// Token: 0x040000D9 RID: 217
	public AudioManager.MusicChild testCurrentTrack;

	// Token: 0x040000DA RID: 218
	public bool recentVOFound = true;

	// Token: 0x040000DB RID: 219
	private Dictionary<string, List<Coroutine>> _queuedPlayTracks = new Dictionary<string, List<Coroutine>>();

	// Token: 0x040000DC RID: 220
	[Header("Debug")]
	[SerializeField]
	private bool debugAudioStops;

	// Token: 0x040000DD RID: 221
	[SerializeField]
	private bool debugAudioStarts;

	// Token: 0x040000DE RID: 222
	[SerializeField]
	private bool debugAudioResumes;

	// Token: 0x040000DF RID: 223
	[SerializeField]
	private bool debugAudioPauses;

	// Token: 0x040000E0 RID: 224
	[SerializeField]
	private bool debugAudioFades;

	// Token: 0x02000297 RID: 663
	public class MusicChild : MonoBehaviour
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x000634C3 File Offset: 0x000616C3
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x000634CB File Offset: 0x000616CB
		public GameObject GameObjectOwner { get; private set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x000634D4 File Offset: 0x000616D4
		public bool HasOwner
		{
			get
			{
				return this.GameObjectOwner != null;
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x000634E4 File Offset: 0x000616E4
		public AudioManager.MusicChild Init(string _name, AUDIO_TYPE _type, AudioSource _audioSource, AudioClip _song, AudioMixerGroup _mixGroup, bool _loop, bool _releaseAudioClipOnDestroy, float _fadeTime, SFX_SUBGROUP subgroup = SFX_SUBGROUP.NONE, GameObject gameObjectOwner = null)
		{
			this.Name = _name;
			this.Song = _song;
			this.Type = _type;
			this.Subgroup = subgroup;
			this.GameObjectOwner = gameObjectOwner;
			if (this.GameObjectOwner != null)
			{
				StopAllGameObjectTracksOnDestroy.EnsureHasComponent(gameObjectOwner);
			}
			this.Audio = _audioSource;
			this.Audio.clip = this.Song;
			this.Audio.playOnAwake = false;
			this.Audio.loop = _loop;
			this.Audio.outputAudioMixerGroup = _mixGroup;
			this.Audio.dopplerLevel = 0f;
			this.Audio.spread = 90f;
			this.volumeMaximum = 1f;
			_name = _name.ToLowerInvariant();
			if (_name.Contains("dateviatorsbox") || _name.Contains("mysterybox") || _name.Contains("mysteriousbox"))
			{
				this.Audio.spread = 80f;
				this.Audio.maxDistance = 25f;
				this.Audio.loop = true;
			}
			if (_name.Contains("peninput") || _name.Contains("backspace"))
			{
				this.Audio.pitch = Random.Range(0.9f, 1.1f);
			}
			if (_name.Contains("drone") || _name.Contains("deliver"))
			{
				this.Audio.spread = 50f;
				this.Audio.dopplerLevel = 1f;
				this.Audio.rolloffMode = AudioRolloffMode.Linear;
				if (_name.Contains("loop"))
				{
					this.Audio.maxDistance = 25f;
					this.Audio.volume = 0.5f;
				}
				else if (_name.Contains("leaving"))
				{
					this.Audio.maxDistance = 60f;
				}
				else
				{
					this.Audio.maxDistance = 160f;
				}
			}
			if (_name.Contains("sm_car"))
			{
				this.Audio.dopplerLevel = 1f;
			}
			if (_name.Contains("vent"))
			{
				this.Audio.maxDistance = 15f;
			}
			if (_name.Contains("hvac"))
			{
				this.Audio.maxDistance = 14f;
			}
			this.releaseAudioClipOnDestroy = _releaseAudioClipOnDestroy;
			return this;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0006372B File Offset: 0x0006192B
		private void OnDestroy()
		{
			if (this.releaseAudioClipOnDestroy)
			{
				Services.AssetProviderService.UnloadResourceAsset(this.Song);
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00063745 File Offset: 0x00061945
		private void OnDisable()
		{
			if (!this.shuttingDown)
			{
				this.Stop();
			}
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00063755 File Offset: 0x00061955
		public IEnumerator Fade(float duration, bool In, bool stop)
		{
			float min = (In ? 0f : this.volumeMaximum);
			float max = (In ? this.volumeMaximum : 0f);
			this.Audio.volume = min;
			if (In)
			{
				this.Audio.Play();
			}
			for (float f = 0f; f < duration; f += Time.deltaTime)
			{
				this.Audio.volume = Mathf.Lerp(min, max, f / duration);
				yield return new WaitForEndOfFrame();
			}
			this.Audio.volume = max;
			if (!In && !stop)
			{
				this.Pause();
			}
			if (!In && stop)
			{
				this.Stop();
			}
			yield break;
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0006377C File Offset: 0x0006197C
		public void Play(float fadeIn = 0.5f)
		{
			this.isplaying = true;
			if (this.Audio.name.Contains("file_select") || this.Audio.name.Contains("bodhi_song") || this.Audio.name.Contains("credits") || this.Audio.name.Contains("mysteriousbox_music") || this.Audio.name.Contains("miranda_ballad_afraid") || this.Audio.name.Contains("miranda_ballad_burn") || this.Audio.name.Contains("miranda_ballad_cant_find") || this.Audio.name.Contains("miranda_ballad_home") || this.Audio.name.Contains("miranda_ballad_horizon") || this.Audio.name.Contains("miranda_ballad_vase"))
			{
				this.Audio.volume = 0.8f;
				this.volumeMaximum = 0.8f;
			}
			if (this.Audio.name.Contains("opening_cinematic") || this.Audio.name.Contains("credits") || this.Audio.name.Contains("bodhi_song") || this.Audio.name.Contains("miranda_ballad") || this.Audio.name.Contains("keyes_concerto"))
			{
				this.Audio.loop = false;
			}
			else if (this.Audio.name.Contains("main_menu"))
			{
				this.Audio.volume = 0.85f;
				this.volumeMaximum = 0.85f;
				this.Audio.loop = false;
			}
			else if (this.Audio.name.Contains("opening_music"))
			{
				this.Audio.volume = 0.8f;
				this.volumeMaximum = 0.8f;
			}
			if (this.Audio.name == "lucinda_music" || this.Audio.name == "reggie_music")
			{
				this.Audio.volume = 0.8f;
				this.volumeMaximum = 0.8f;
			}
			else
			{
				this.Audio.volume = 1f;
				this.volumeMaximum = 1f;
			}
			base.StartCoroutine(this.Fade(fadeIn, true, false));
			if (!this.Audio.loop)
			{
				base.StartCoroutine(this.DelayStop(this.Song.length + fadeIn));
			}
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x00063A2D File Offset: 0x00061C2D
		public void Pause()
		{
			this.Audio.Pause();
			this.isplaying = false;
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x00063A41 File Offset: 0x00061C41
		public void Pause(float fadeOut)
		{
			base.StartCoroutine(this.Fade(fadeOut, false, false));
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00063A54 File Offset: 0x00061C54
		public void Stop()
		{
			this.shuttingDown = true;
			base.StopAllCoroutines();
			if (this.Name != null && Singleton<AudioManager>.Instance != null)
			{
				Singleton<AudioManager>.Instance.RemoveTrack(this.Name);
			}
			Object.Destroy(this.Audio.gameObject);
			Object.Destroy(this);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00063AA9 File Offset: 0x00061CA9
		public void Stop(float fadeOut)
		{
			if (fadeOut == 0f)
			{
				this.Stop();
				return;
			}
			base.StopCoroutine("Fade");
			base.StopCoroutine("FadeStop");
			base.StartCoroutine(this.FadeStop(fadeOut));
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00063ADE File Offset: 0x00061CDE
		public void Mute(float duration)
		{
			this.isplaying = false;
			base.StartCoroutine(this.Fade(duration, false, false));
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00063AF7 File Offset: 0x00061CF7
		public void UnMute(float duration)
		{
			this.isplaying = true;
			base.StartCoroutine(this.Fade(duration, true, false));
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00063B10 File Offset: 0x00061D10
		public IEnumerator FadeStop(float duration)
		{
			for (float f = 0f; f < duration; f += Time.deltaTime)
			{
				this.Audio.volume = Mathf.Min(Mathf.Lerp(1f, 0f, f / duration), this.Audio.volume);
				yield return new WaitForEndOfFrame();
			}
			this.Stop();
			yield break;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00063B26 File Offset: 0x00061D26
		public IEnumerator DelayStop(float wait)
		{
			yield return new WaitForSecondsRealtime(wait);
			yield return new WaitUntil(() => !this.Audio.isPlaying && Application.isFocused);
			this.Stop();
			yield break;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00063B3C File Offset: 0x00061D3C
		public AudioClip GetSong()
		{
			return this.Song;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00063B44 File Offset: 0x00061D44
		public AudioSource GetAudio()
		{
			return this.Audio;
		}

		// Token: 0x04001040 RID: 4160
		public string Name;

		// Token: 0x04001041 RID: 4161
		public AUDIO_TYPE Type;

		// Token: 0x04001042 RID: 4162
		public SFX_SUBGROUP Subgroup;

		// Token: 0x04001043 RID: 4163
		public bool isplaying = true;

		// Token: 0x04001044 RID: 4164
		private AudioSource Audio;

		// Token: 0x04001045 RID: 4165
		private AudioClip Song;

		// Token: 0x04001046 RID: 4166
		private bool shuttingDown;

		// Token: 0x04001047 RID: 4167
		private bool releaseAudioClipOnDestroy;

		// Token: 0x04001049 RID: 4169
		public float volumeMaximum = 1f;
	}
}
