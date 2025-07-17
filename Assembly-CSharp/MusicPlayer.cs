using System;
using System.Collections;
using System.Collections.Generic;
using AirFishLab.ScrollingList;
using Rewired;
using T17.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200010A RID: 266
public class MusicPlayer : MonoBehaviour
{
	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060008FA RID: 2298 RVA: 0x00034AE9 File Offset: 0x00032CE9
	public int CurrentMusicIndex
	{
		get
		{
			return this.currentEntry - 1;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060008FB RID: 2299 RVA: 0x00034AF3 File Offset: 0x00032CF3
	public int TotalEntries
	{
		get
		{
			return this.SongEntries.Count;
		}
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00034B00 File Offset: 0x00032D00
	public void Awake()
	{
		this.CreatePlayList();
		this.musicAnimator = base.GetComponent<Animator>();
		this.musicListBank = base.GetComponent<MusicListBank>();
		this.musicAnimator.enabled = true;
		this._uiUtilities = Object.FindObjectOfType<UIUtilities>();
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x00034B37 File Offset: 0x00032D37
	public void Start()
	{
		if (MusicPlayer.Instance != null && MusicPlayer.Instance != this)
		{
			Object.Destroy(this);
			return;
		}
		MusicPlayer.Instance = this;
		this.PopulateItemList();
		this.UpdateBackAction();
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x00034B6C File Offset: 0x00032D6C
	private void OnEnable()
	{
		base.StartCoroutine(this.SelectFirstMusicButton());
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00034B7B File Offset: 0x00032D7B
	private IEnumerator SelectFirstMusicButton()
	{
		yield return new WaitUntil(() => this.scrollingList.isActiveAndEnabled);
		ControllerMenuUI.SetCurrentlySelected(this.scrollingList.GetFocusingBox().gameObject, ControllerMenuUI.Direction.Down, false, false);
		yield break;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x00034B8C File Offset: 0x00032D8C
	private void UpdateBackAction()
	{
		if (this.backButton.GetComponentInChildren<ActionOnDisable>(true) != null)
		{
			ActionOnDisable componentInChildren = this.backButton.GetComponentInChildren<ActionOnDisable>(true);
			componentInChildren.onDisable = (Action)Delegate.Combine(componentInChildren.onDisable, new Action(this.RefocusFromDisabledBackButton));
		}
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x00034BDA File Offset: 0x00032DDA
	private void RefocusFromDisabledBackButton()
	{
		if (this.backButton.gameObject == ControllerMenuUI.GetCurrentSelectedControl())
		{
			if (this.player == null)
			{
				this.player = ReInput.players.GetPlayer(0);
			}
			base.StartCoroutine(this.SelectFirstMusicButton());
		}
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x00034C1C File Offset: 0x00032E1C
	private void PopulateItemList()
	{
		if (this.currentEntry == -1)
		{
			this.currentEntry = 0;
		}
		this.musicListBank.Init(this.soundtrack);
		this.scrollingList.Initialize();
		ListBox[] listBoxes = this.scrollingList.ListBoxes;
		int i = 0;
		int num = listBoxes.Length;
		while (i < num)
		{
			if (this.soundtrack[i] != null)
			{
				MusicEntryButton musicEntryButton = (MusicEntryButton)listBoxes[i];
				musicEntryButton.GetComponent<Button>().onClick.AddListener(delegate
				{
					this.ButtonClicked(musicEntryButton);
				});
				this.SongEntries.Add(musicEntryButton);
			}
			i++;
		}
		this.scrollingList.Refresh(0);
		this.UpdateTrackText(this.currentEntry);
		this.SetupEntryNavigation();
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00034CEC File Offset: 0x00032EEC
	public void OpenMusicApp()
	{
		if (this._uiUtilities == null)
		{
			this._uiUtilities = Object.FindObjectOfType<UIUtilities>();
		}
		if (this._uiUtilities != null)
		{
			this._uiUtilities.SetMusicFrequency(1f);
			this._uiUtilities.SetMusicResonance(1f);
		}
		this.previousSong = null;
		foreach (AudioManager.MusicChild musicChild in Singleton<AudioManager>.Instance.CurrentTracks)
		{
			if (musicChild.Type == AUDIO_TYPE.MUSIC && musicChild.isplaying)
			{
				this.previousSong = musicChild.GetSong();
			}
		}
		this.StopSong();
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00034DAC File Offset: 0x00032FAC
	public void CloseMusicApp()
	{
		if (this._uiUtilities != null)
		{
			this._uiUtilities.SetMusicFrequency(800f);
			this._uiUtilities.SetMusicResonance(1f);
		}
		if (this.volumePopupOpen)
		{
			Singleton<Popup>.Instance.ClosePopup();
		}
		this.StopSong();
		if (this.previousSong != null)
		{
			Singleton<AudioManager>.Instance.PlayTrack(this.previousSong, AUDIO_TYPE.MUSIC, true, false, 0f, false, 1f, null, false, SFX_SUBGROUP.NONE, false);
		}
		Singleton<PhoneManager>.Instance.ReturnToMainPhoneScreenRotate();
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00034E39 File Offset: 0x00033039
	public void TogglePlay()
	{
		if (!this.IsSongPlaying())
		{
			this.PlaySong();
			return;
		}
		if (!this.IsSongPaused())
		{
			this.PausedTrack();
			this.SetPlayingSprite(false);
			return;
		}
		this.SetPlayingSprite(true);
		this.ResumePausedTrack();
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00034E70 File Offset: 0x00033070
	private bool IsMusicMuted()
	{
		float @float = Services.GameSettings.GetFloat("masterVolume", 1f);
		float float2 = Services.GameSettings.GetFloat("musicVolume", 1f);
		return @float <= 0f || float2 <= 0f;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00034EB8 File Offset: 0x000330B8
	private void ResumePausedTrack()
	{
		if (this.IsSongPaused())
		{
			Singleton<AudioManager>.Instance.ResumeTrack(this.trackPaused, false, false, 0f);
			this.trackPaused = "";
		}
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00034EE4 File Offset: 0x000330E4
	private void PausedTrack()
	{
		if (!this.IsSongPaused())
		{
			this.trackPaused = this.GetMusicEntry(this.currentEntry).song.name;
			Singleton<AudioManager>.Instance.PauseTrack(this.trackPaused, 0f);
		}
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00034F1F File Offset: 0x0003311F
	private void SetPlayingSprite(bool playing)
	{
		if (playing)
		{
			this.PlayBtn.sprite = this.Pause;
			return;
		}
		this.PlayBtn.sprite = this.Play;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00034F48 File Offset: 0x00033148
	private void ButtonClicked(MusicEntryButton button)
	{
		if (this.IsMusicMuted())
		{
			this.CreateVolumePopup();
			return;
		}
		int num = this.FindTrackInPlayList(button.index);
		if (this.IsSongPlaying() && num == this.currentEntry)
		{
			this.TogglePlay();
			return;
		}
		this.SongClickedOn(button.musicEntry);
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00034F95 File Offset: 0x00033195
	private void SongClickedOn(MusicEntry entry)
	{
		this.currentEntry = this.FindTrackInPlayList(entry.number - 1);
		this.UpdateTrackText(this.currentEntry);
		if (this.IsSongPlaying())
		{
			this.StopSong();
		}
		this.PlaySong();
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x00034FCC File Offset: 0x000331CC
	private void CreateVolumePopup()
	{
		this.volumePopupOpen = true;
		UnityEvent unityEvent = new UnityEvent();
		unityEvent.AddListener(new UnityAction(this.FlagPopupClosed));
		string text = "As fun as watching a spinning record can be, we recommend turning up the music volume in your phone settings for a complete <nobr>audio-visual</nobr> experience.";
		Singleton<Popup>.Instance.CreatePopup("VOLUME WARNING", text, unityEvent, true);
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x00035010 File Offset: 0x00033210
	private void FlagPopupClosed()
	{
		this.volumePopupOpen = false;
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x00035019 File Offset: 0x00033219
	private MusicEntry GetMusicEntry(int indexIntoPlaylist)
	{
		if (indexIntoPlaylist >= this.playList.Count)
		{
			indexIntoPlaylist = 0;
		}
		return this.soundtrack[this.playList[indexIntoPlaylist]];
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00035044 File Offset: 0x00033244
	private void PlaySong()
	{
		this.ResumePausedTrack();
		MusicEntry musicEntry = this.GetMusicEntry(this.currentEntry);
		Singleton<AudioManager>.Instance.PlayTrack(musicEntry.song, AUDIO_TYPE.MUSIC, true, false, 0f, false, 1f, null, false, SFX_SUBGROUP.NONE, false);
		this.timeBeforeEnd = musicEntry.song.length;
		this.UpdateTrackText(this.currentEntry);
		this.SetPlayingSprite(true);
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x000350AA File Offset: 0x000332AA
	public void TryTogglePlay()
	{
		if (this.IsMusicMuted())
		{
			this.CreateVolumePopup();
			return;
		}
		this.TogglePlay();
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x000350C4 File Offset: 0x000332C4
	private void UpdateTrackText(int playListIndex)
	{
		MusicEntry musicEntry = this.GetMusicEntry(playListIndex);
		this.SongTitle.text = string.Format("{0} - {1}", musicEntry.number, musicEntry.title);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x000350FF File Offset: 0x000332FF
	private void StopSong()
	{
		this.ResumePausedTrack();
		this.timeBeforeEnd = 0f;
		Singleton<AudioManager>.Instance.StopTrackGroup(AUDIO_TYPE.MUSIC, 0.5f);
		this.SetPlayingSprite(false);
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0003512C File Offset: 0x0003332C
	private void NextSong()
	{
		if (this.repeatType == RepeatType.RepeatOne)
		{
			this.PlaySong();
			return;
		}
		if (this.repeatType == RepeatType.NoRepeat && this.currentEntry == this.playList.Count - 1)
		{
			this.StopSong();
			return;
		}
		int num = this.currentEntry + 1;
		this.currentEntry = num;
		if (num == this.playList.Count)
		{
			this.currentEntry = 0;
		}
		this.PlaySong();
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00035198 File Offset: 0x00033398
	public void ToggleRepeat()
	{
		if (this.repeatType == RepeatType.NoRepeat)
		{
			this.repeatType = RepeatType.RepeatAll;
			this.RepeatBtn.sprite = this.RepeatAll;
			return;
		}
		if (this.repeatType == RepeatType.RepeatAll)
		{
			this.repeatType = RepeatType.RepeatOne;
			this.RepeatBtn.sprite = this.RepeatOne;
			return;
		}
		this.repeatType = RepeatType.NoRepeat;
		this.RepeatBtn.sprite = this.NoRepeat;
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00035200 File Offset: 0x00033400
	public void ToggleShuffle()
	{
		int num = this.playList[this.currentEntry];
		this.isShuffle = !this.isShuffle;
		this.CreatePlayList();
		this.currentEntry = this.FindTrackInPlayList(num);
		if (this.isShuffle)
		{
			this.ShuffleBtn.sprite = this.ShuffleOn;
			if (this.currentEntry != 0)
			{
				this.EnsureEntryIsFirstInPlaylist(this.currentEntry);
				this.currentEntry = 0;
				return;
			}
		}
		else
		{
			this.ShuffleBtn.sprite = this.ShuffleOff;
		}
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00035288 File Offset: 0x00033488
	private void EnsureEntryIsFirstInPlaylist(int entry)
	{
		if (entry != 0)
		{
			int num = this.playList[0];
			this.playList[0] = this.playList[entry];
			this.playList[entry] = num;
		}
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x000352CC File Offset: 0x000334CC
	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if (this.IsSongPlaying() && !this.IsSongPaused() && (this.timeBeforeEnd -= deltaTime) <= 0f)
		{
			this.timeBeforeEnd = 0f;
			this.NextSong();
		}
		this.UpdateAnimator();
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x00035320 File Offset: 0x00033520
	private void CreatePlayList()
	{
		if (this.playList == null)
		{
			this.playList = new List<int>(this.soundtrack.Count);
		}
		this.playList.Clear();
		int i = 0;
		int count = this.soundtrack.Count;
		while (i < count)
		{
			if (this.soundtrack[i] != null)
			{
				this.playList.Add(i);
			}
			i++;
		}
		if (this.isShuffle)
		{
			Random random = new Random();
			int j = this.playList.Count;
			while (j > 1)
			{
				j--;
				int num = random.Next(j + 1);
				int num2 = this.playList[num];
				this.playList[num] = this.playList[j];
				this.playList[j] = num2;
			}
		}
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x000353F0 File Offset: 0x000335F0
	private int FindTrackInPlayList(int songTrackIndex)
	{
		int i = 0;
		int count = this.playList.Count;
		while (i < count)
		{
			if (this.playList[i] == songTrackIndex)
			{
				return i;
			}
			i++;
		}
		return 0;
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x00035427 File Offset: 0x00033627
	private bool IsSongPlaying()
	{
		return this.timeBeforeEnd != 0f;
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00035439 File Offset: 0x00033639
	private bool IsSongPaused()
	{
		return !string.IsNullOrEmpty(this.trackPaused);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0003544C File Offset: 0x0003364C
	private void UpdateAnimator()
	{
		if (this.musicAnimator != null)
		{
			float num = 0f;
			if (this.IsSongPlaying() && !this.IsSongPaused())
			{
				num = 1f;
			}
			if (this.musicAnimator.speed != num)
			{
				this.musicAnimator.speed = num;
			}
		}
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0003549D File Offset: 0x0003369D
	private void OnApplicationPause(bool paused)
	{
		if (paused && this.IsSongPlaying() && !this.IsSongPaused())
		{
			this.TogglePlay();
		}
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x000354B8 File Offset: 0x000336B8
	public void SetupEntryNavigation()
	{
		for (int i = 0; i < this.SongEntries.Count; i++)
		{
			Navigation navigation = this.SongEntries[i].transform.GetComponent<Button>().navigation;
			navigation.selectOnLeft = this.backButton.GetComponent<Button>();
			if (i == 0)
			{
				navigation.selectOnUp = this.SongEntries[this.SongEntries.Count - 1].transform.GetComponent<Button>();
			}
			else
			{
				navigation.selectOnUp = this.SongEntries[Mathf.Clamp(i - 1, 0, this.SongEntries.Count - 1)].transform.GetComponent<Button>();
			}
			if (i == this.SongEntries.Count - 1)
			{
				navigation.selectOnDown = this.SongEntries[0].transform.GetComponent<Button>();
			}
			else
			{
				navigation.selectOnDown = this.SongEntries[Mathf.Clamp(i + 1, 0, this.SongEntries.Count - 1)].transform.GetComponent<Button>();
			}
			navigation.mode = Navigation.Mode.Explicit;
			this.SongEntries[i].transform.GetComponent<Button>().navigation = navigation;
		}
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x000355F4 File Offset: 0x000337F4
	public void PageEntries(bool up)
	{
		int num = Math.Clamp(this.scrollingList.GetFocusingContentID() + (up ? (-this.pageAmount) : this.pageAmount), 0, this.soundtrack.Count - 1);
		for (int i = 0; i < this.SongEntries.Count; i++)
		{
			if (this.SongEntries[i].ContentID == num)
			{
				ControllerMenuUI.SetCurrentlySelected(this.SongEntries[i].gameObject, up ? ControllerMenuUI.Direction.Up : ControllerMenuUI.Direction.Down, true, false);
				return;
			}
		}
	}

	// Token: 0x04000842 RID: 2114
	public RepeatType repeatType;

	// Token: 0x04000843 RID: 2115
	public bool isPlaying;

	// Token: 0x04000844 RID: 2116
	public bool isShuffle;

	// Token: 0x04000845 RID: 2117
	public List<MusicEntry> soundtrack;

	// Token: 0x04000846 RID: 2118
	public static MusicPlayer Instance;

	// Token: 0x04000847 RID: 2119
	public Image PlayBtn;

	// Token: 0x04000848 RID: 2120
	public Image RepeatBtn;

	// Token: 0x04000849 RID: 2121
	public Image ShuffleBtn;

	// Token: 0x0400084A RID: 2122
	public TextMeshProUGUI SongTitle;

	// Token: 0x0400084B RID: 2123
	public Sprite Play;

	// Token: 0x0400084C RID: 2124
	public Sprite Pause;

	// Token: 0x0400084D RID: 2125
	public Sprite NoRepeat;

	// Token: 0x0400084E RID: 2126
	public Sprite RepeatAll;

	// Token: 0x0400084F RID: 2127
	public Sprite RepeatOne;

	// Token: 0x04000850 RID: 2128
	public Sprite ShuffleOn;

	// Token: 0x04000851 RID: 2129
	public Sprite ShuffleOff;

	// Token: 0x04000852 RID: 2130
	private AudioClip previousSong;

	// Token: 0x04000853 RID: 2131
	private int currentEntry;

	// Token: 0x04000854 RID: 2132
	public GameObject entryButtonPrefab;

	// Token: 0x04000855 RID: 2133
	public GameObject buttonHolder;

	// Token: 0x04000856 RID: 2134
	[SerializeField]
	private List<MusicEntryButton> SongEntries = new List<MusicEntryButton>();

	// Token: 0x04000857 RID: 2135
	[SerializeField]
	private IsSelectableRegistered backButton;

	// Token: 0x04000858 RID: 2136
	private float timeBeforeEnd;

	// Token: 0x04000859 RID: 2137
	private List<int> playList = new List<int>();

	// Token: 0x0400085A RID: 2138
	private Animator musicAnimator;

	// Token: 0x0400085B RID: 2139
	private string trackPaused = "";

	// Token: 0x0400085C RID: 2140
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x0400085D RID: 2141
	private Player player;

	// Token: 0x0400085E RID: 2142
	private MusicListBank musicListBank;

	// Token: 0x0400085F RID: 2143
	[SerializeField]
	public CircularScrollingList scrollingList;

	// Token: 0x04000860 RID: 2144
	private const string MASTER_VOL_KEY = "masterVolume";

	// Token: 0x04000861 RID: 2145
	private const string MUSIC_VOL_KEY = "musicVolume";

	// Token: 0x04000862 RID: 2146
	private bool volumePopupOpen;

	// Token: 0x04000863 RID: 2147
	private int pageAmount = 5;

	// Token: 0x04000864 RID: 2148
	private UIUtilities _uiUtilities;
}
