using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Common;
using TMPro;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class AnimateTMProTextTags : MonoBehaviour
{
	// Token: 0x06000063 RID: 99 RVA: 0x00003511 File Offset: 0x00001711
	private void Awake()
	{
		if (this.m_TextComponent == null)
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00003534 File Offset: 0x00001734
	private void OnEnable()
	{
		if (this.animateOnEnable)
		{
			if (this.m_TextComponent != null && string.IsNullOrEmpty(this.textToAnimate))
			{
				this.textToAnimate = this.m_TextComponent.text;
			}
			this.Init(this.textToAnimate);
			this.Play();
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00003587 File Offset: 0x00001787
	public void Play()
	{
		base.StartCoroutine(this.AnimateTextCoroutine());
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003596 File Offset: 0x00001796
	public void Init(string dialogTextIn)
	{
		if (!this.m_TextComponent)
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}
		this.animating = false;
		this.textToAnimate = dialogTextIn;
		this.ProcessTextToAnimate(dialogTextIn);
		this.initialized = true;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x000035D2 File Offset: 0x000017D2
	private IEnumerator AnimateTextCoroutine()
	{
		yield return base.StartCoroutine(this.AnimateVertices());
		yield break;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x000035E1 File Offset: 0x000017E1
	public void Stop()
	{
		this.timerDelta = -1f;
		this.stopCalled = true;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x000035F5 File Offset: 0x000017F5
	public void Pause()
	{
		this.paused = true;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000035FE File Offset: 0x000017FE
	public bool IsAnimating()
	{
		return this.animating;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003606 File Offset: 0x00001806
	public void Reset()
	{
		this.Play();
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00003610 File Offset: 0x00001810
	private void ProcessTextToAnimate(string dialogTextIn)
	{
		int num = 0;
		foreach (AnimateTMProTextTags.TextAnimationSettings textAnimationSettings in this.textAnimationSettings)
		{
			if (textAnimationSettings.tag == "")
			{
				T17Debug.LogError("Tag string must be specificed");
			}
			string text = dialogTextIn;
			string text2 = "<" + textAnimationSettings.tag + ">";
			string text3 = "</" + textAnimationSettings.tag + ">";
			this.textAnimationSettings[num].letterList = new List<List<int>>();
			while (text.IndexOf(text2) >= 0)
			{
				int num2 = text.IndexOf(text2, 0);
				int num3 = text.IndexOf(text3, num2);
				List<int> list = new List<int>();
				for (int j = num2; j < num3 - text2.Length; j++)
				{
					list.Add(j);
				}
				this.textAnimationSettings[num].letterList.Add(list);
				string text4 = text.Substring(0, num2);
				string text5 = text.Substring(num2 + text2.Length, num3 - (num2 + text2.Length));
				string text6 = text.Substring(num3 + text3.Length);
				text = text4 + text5 + text6;
			}
			this.m_TextComponent.text = text;
			num++;
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x0000376D File Offset: 0x0000196D
	private IEnumerator AnimateVertices()
	{
		this.timerDelta = -1f;
		this.m_TextComponent.ForceMeshUpdate(false, false);
		yield return new WaitForEndOfFrame();
		TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
		TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
		float animStartTime = Time.time;
		int _iterations = 0;
		this.animating = true;
		while (this.animating && this.initialized)
		{
			if (!this.paused)
			{
				this.timerDelta += Time.deltaTime;
			}
			for (int i = 0; i < this.textAnimationSettings.Length; i++)
			{
				foreach (List<int> list in this.textAnimationSettings[i].letterList)
				{
					if (!this.textAnimationSettings[i].performOnce || !this.textAnimationSettings[i].performed)
					{
						int count = list.Count;
						int num = 0;
						foreach (int num2 in list)
						{
							if (!this.paused)
							{
								TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[num2];
								if (tmp_CharacterInfo.isVisible)
								{
									int materialReferenceIndex = textInfo.characterInfo[num2].materialReferenceIndex;
									int vertexIndex = textInfo.characterInfo[num2].vertexIndex;
									Vector3[] vertices = cachedMeshInfo[materialReferenceIndex].vertices;
									Vector2 vector = Vector2.zero;
									switch (this.pivot)
									{
									case AnimateTMProTextTags.LetterPivot.baselineCenter:
										vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, tmp_CharacterInfo.baseLine);
										break;
									case AnimateTMProTextTags.LetterPivot.baselineLeft:
										vector = new Vector2(vertices[vertexIndex].x, tmp_CharacterInfo.baseLine);
										break;
									case AnimateTMProTextTags.LetterPivot.baselineRight:
										vector = new Vector2(vertices[vertexIndex].x + vertices[vertexIndex + 2].x, tmp_CharacterInfo.baseLine);
										break;
									default:
										vector = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
										break;
									}
									Vector3 vector2 = vector;
									Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
									if (this.stopCalled)
									{
										vertices2[vertexIndex] = vertices[vertexIndex];
										vertices2[vertexIndex + 1] = vertices[vertexIndex + 1];
										vertices2[vertexIndex + 2] = vertices[vertexIndex + 2];
										vertices2[vertexIndex + 3] = vertices[vertexIndex + 3];
									}
									else
									{
										vertices2[vertexIndex] = vertices[vertexIndex] - vector2;
										vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - vector2;
										vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - vector2;
										vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - vector2;
										if (!this.stopCalled)
										{
											Mathf.Clamp01((Time.time - (animStartTime + (float)num2 * this.textAnimationSettings[i].letterDelay)) / this.textAnimationSettings[i].speed);
										}
										float num3 = 1f - (float)num * this.textAnimationSettings[i].wave / (float)count * this.textAnimationSettings[i].letterDelay;
										float num4 = this.textAnimationSettings[i].speed * (this.timerDelta + num3);
										if (num4 < 0f)
										{
											num4 = 0f;
										}
										float num5 = (this.stopCalled ? 1f : Mathf.Sin(num4));
										Vector3[] array = vertices2;
										int num6 = vertexIndex;
										array[num6].y = array[num6].y + num5 * this.textAnimationSettings[i].direction.y;
										Vector3[] array2 = vertices2;
										int num7 = vertexIndex + 1;
										array2[num7].y = array2[num7].y + num5 * this.textAnimationSettings[i].direction.y;
										Vector3[] array3 = vertices2;
										int num8 = vertexIndex + 2;
										array3[num8].y = array3[num8].y + num5 * this.textAnimationSettings[i].direction.y;
										Vector3[] array4 = vertices2;
										int num9 = vertexIndex + 3;
										array4[num9].y = array4[num9].y + num5 * this.textAnimationSettings[i].direction.y;
										Vector3[] array5 = vertices2;
										int num10 = vertexIndex;
										array5[num10].x = array5[num10].x + num5 * this.textAnimationSettings[i].direction.x;
										Vector3[] array6 = vertices2;
										int num11 = vertexIndex + 1;
										array6[num11].x = array6[num11].x + num5 * this.textAnimationSettings[i].direction.x;
										Vector3[] array7 = vertices2;
										int num12 = vertexIndex + 2;
										array7[num12].x = array7[num12].x + num5 * this.textAnimationSettings[i].direction.x;
										Vector3[] array8 = vertices2;
										int num13 = vertexIndex + 3;
										array8[num13].x = array8[num13].x + num5 * this.textAnimationSettings[i].direction.x;
										vertices2[vertexIndex] += vector2;
										vertices2[vertexIndex + 1] += vector2;
										vertices2[vertexIndex + 2] += vector2;
										vertices2[vertexIndex + 3] += vector2;
										num++;
									}
								}
							}
						}
					}
				}
			}
			for (int j = 0; j < textInfo.meshInfo.Length; j++)
			{
				if (textInfo.meshInfo[j].mesh != null && textInfo.meshInfo[j].vertexCount != 0)
				{
					textInfo.meshInfo[j].mesh.vertices = textInfo.meshInfo[j].vertices;
					this.m_TextComponent.UpdateGeometry(textInfo.meshInfo[j].mesh, j);
				}
			}
			int num14 = _iterations;
			_iterations = num14 + 1;
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000377C File Offset: 0x0000197C
	public virtual Vector3 LetterTranslateFunction(float p, int index, int numCharacters)
	{
		return new Vector3(0f, 0f, 0f);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00003792 File Offset: 0x00001992
	public virtual Quaternion LetterRotateFunction(float p, int index, int numCharacters)
	{
		return Quaternion.Euler(0f, 0f, 0f);
	}

	// Token: 0x06000070 RID: 112 RVA: 0x000037A8 File Offset: 0x000019A8
	public virtual Vector3 LetterScaleFunction(float p, int index, int numCharacters)
	{
		return Vector3.one;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x000037AF File Offset: 0x000019AF
	public virtual Color32 LetterColorFunction(Color32 originalColor, float p, int index, int numCharacters)
	{
		return Random.ColorHSV();
	}

	// Token: 0x06000072 RID: 114 RVA: 0x000037BB File Offset: 0x000019BB
	public virtual void OnAnimationComplete()
	{
	}

	// Token: 0x0400007B RID: 123
	[SerializeField]
	private AnimateTMProTextTags.LetterPivot pivot;

	// Token: 0x0400007C RID: 124
	[SerializeField]
	private TMP_Text m_TextComponent;

	// Token: 0x0400007D RID: 125
	[SerializeField]
	private bool animateOnEnable;

	// Token: 0x0400007E RID: 126
	[SerializeField]
	private AnimateTMProTextTags.TextAnimationSettings[] textAnimationSettings;

	// Token: 0x0400007F RID: 127
	[SerializeField]
	private bool paused;

	// Token: 0x04000080 RID: 128
	[SerializeField]
	private bool stopCalled;

	// Token: 0x04000081 RID: 129
	private bool hasTextChanged;

	// Token: 0x04000082 RID: 130
	private bool animating;

	// Token: 0x04000083 RID: 131
	private string textToAnimate = "";

	// Token: 0x04000084 RID: 132
	private bool initialized;

	// Token: 0x04000085 RID: 133
	private float timerDelta;

	// Token: 0x02000277 RID: 631
	[Serializable]
	public struct TextAnimationSettings
	{
		// Token: 0x04000F9D RID: 3997
		public string tag;

		// Token: 0x04000F9E RID: 3998
		public Vector2 direction;

		// Token: 0x04000F9F RID: 3999
		public bool performOnce;

		// Token: 0x04000FA0 RID: 4000
		[NonSerialized]
		public bool performed;

		// Token: 0x04000FA1 RID: 4001
		[NonSerialized]
		public int interationsPerformed;

		// Token: 0x04000FA2 RID: 4002
		public float speed;

		// Token: 0x04000FA3 RID: 4003
		public float wave;

		// Token: 0x04000FA4 RID: 4004
		public float letterDelay;

		// Token: 0x04000FA5 RID: 4005
		public List<List<int>> letterList;
	}

	// Token: 0x02000278 RID: 632
	[SerializeField]
	private enum LetterPivot
	{
		// Token: 0x04000FA7 RID: 4007
		center,
		// Token: 0x04000FA8 RID: 4008
		baselineCenter,
		// Token: 0x04000FA9 RID: 4009
		baselineLeft,
		// Token: 0x04000FAA RID: 4010
		baselineRight
	}
}
