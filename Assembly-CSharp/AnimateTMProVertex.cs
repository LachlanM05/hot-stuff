using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000015 RID: 21
[RequireComponent(typeof(TMP_Text))]
public class AnimateTMProVertex : MonoBehaviour
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600004D RID: 77 RVA: 0x00003389 File Offset: 0x00001589
	public bool playSound
	{
		get
		{
			return !Singleton<AudioManager>.Instance.recentVOFound;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600004E RID: 78 RVA: 0x00003398 File Offset: 0x00001598
	private TMP_Text m_TextComponent
	{
		get
		{
			if (this._m_TextComponent != null)
			{
				return this._m_TextComponent;
			}
			this._m_TextComponent = base.GetComponent<TMP_Text>();
			return this._m_TextComponent;
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000033C1 File Offset: 0x000015C1
	private void Start()
	{
		this.initialized = true;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000033CA File Offset: 0x000015CA
	public void AnimateText()
	{
		base.StartCoroutine(this.AnimateVertices());
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000033D9 File Offset: 0x000015D9
	public IEnumerator AnimateTextCoroutine()
	{
		yield return base.StartCoroutine(this.AnimateVertices());
		yield break;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000033E8 File Offset: 0x000015E8
	public void Skip()
	{
		if (this.animating && !this.skipCalled && !this.skipDisabled)
		{
			this.skipCalled = true;
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00003409 File Offset: 0x00001609
	public bool IsAnimating()
	{
		return this.animating;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00003411 File Offset: 0x00001611
	private IEnumerator AnimateVertices()
	{
		this.animating = true;
		if (!this.initialized)
		{
			yield return new WaitForEndOfFrame();
		}
		this.m_TextComponent.ForceMeshUpdate(false, false);
		TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
		TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
		float animStartTime = Time.time;
		int lastSeen = 0;
		for (int i = 0; i < textInfo.characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				lastSeen = i;
			}
		}
		while (this.animating)
		{
			int characterCount = textInfo.characterCount;
			for (int j = 0; j < characterCount; j++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[j];
				if (tmp_CharacterInfo.isVisible)
				{
					int materialReferenceIndex = textInfo.characterInfo[j].materialReferenceIndex;
					Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
					int vertexIndex = textInfo.characterInfo[j].vertexIndex;
					Vector3[] vertices = cachedMeshInfo[materialReferenceIndex].vertices;
					Vector2 vector = Vector2.zero;
					switch (this.pivot)
					{
					case AnimateTMProVertex.LetterPivot.baselineCenter:
						vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, tmp_CharacterInfo.baseLine);
						break;
					case AnimateTMProVertex.LetterPivot.baselineLeft:
						vector = new Vector2(vertices[vertexIndex].x, tmp_CharacterInfo.baseLine);
						break;
					case AnimateTMProVertex.LetterPivot.baselineRight:
						vector = new Vector2(vertices[vertexIndex].x + vertices[vertexIndex + 2].x, tmp_CharacterInfo.baseLine);
						break;
					default:
						vector = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
						break;
					}
					Vector3 vector2 = vector;
					Vector3[] vertices2 = textInfo.meshInfo[materialReferenceIndex].vertices;
					vertices2[vertexIndex] = vertices[vertexIndex] - vector2;
					vertices2[vertexIndex + 1] = vertices[vertexIndex + 1] - vector2;
					vertices2[vertexIndex + 2] = vertices[vertexIndex + 2] - vector2;
					vertices2[vertexIndex + 3] = vertices[vertexIndex + 3] - vector2;
					float num = (this.skipCalled ? 1f : Mathf.Clamp01((Time.time - (animStartTime + (float)j * this.letterDelay)) / this.duration));
					Vector3 vector3 = this.LetterTranslateFunction(num, j, characterCount);
					Quaternion quaternion = this.LetterRotateFunction(num, j, characterCount);
					Vector3 vector4 = this.LetterScaleFunction(num, j, characterCount);
					Matrix4x4 matrix4x = Matrix4x4.TRS(vector3, quaternion, vector4);
					vertices2[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex]);
					vertices2[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
					vertices2[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
					vertices2[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
					vertices2[vertexIndex] += vector2;
					vertices2[vertexIndex + 1] += vector2;
					vertices2[vertexIndex + 2] += vector2;
					vertices2[vertexIndex + 3] += vector2;
					Color32 color = textInfo.characterInfo[j].color;
					if (textInfo.characterInfo[j].isVisible)
					{
						color = this.LetterColorFunction(color, num, j, characterCount);
						colors[vertexIndex] = color;
						colors[vertexIndex + 1] = color;
						colors[vertexIndex + 2] = color;
						colors[vertexIndex + 3] = color;
						this.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					}
					if (j == lastSeen && num >= 1f)
					{
						this.animating = false;
						this.skipCalled = false;
						this.OnAnimationComplete();
					}
				}
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003420 File Offset: 0x00001620
	public virtual Vector3 LetterTranslateFunction(float p, int index, int numCharacters)
	{
		return Vector3.zero;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00003427 File Offset: 0x00001627
	public virtual Quaternion LetterRotateFunction(float p, int index, int numCharacters)
	{
		return Quaternion.Euler(0f, 0f, 0f);
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0000343D File Offset: 0x0000163D
	public virtual Vector3 LetterScaleFunction(float p, int index, int numCharacters)
	{
		return Vector3.one;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00003444 File Offset: 0x00001644
	public virtual Color32 LetterColorFunction(Color32 originalColor, float p, int index, int numCharacters)
	{
		return originalColor;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003447 File Offset: 0x00001647
	public virtual void OnAnimationComplete()
	{
	}

	// Token: 0x04000069 RID: 105
	public float duration = 1f;

	// Token: 0x0400006A RID: 106
	public float letterDelay = 0.1f;

	// Token: 0x0400006B RID: 107
	public AnimateTMProVertex.LetterPivot pivot;

	// Token: 0x0400006C RID: 108
	public bool skipDisabled;

	// Token: 0x0400006D RID: 109
	private TMP_Text _m_TextComponent;

	// Token: 0x0400006E RID: 110
	private bool hasTextChanged;

	// Token: 0x0400006F RID: 111
	private bool animating;

	// Token: 0x04000070 RID: 112
	private bool skipCalled;

	// Token: 0x04000071 RID: 113
	[SerializeField]
	private bool debug;

	// Token: 0x04000072 RID: 114
	private bool initialized;

	// Token: 0x02000274 RID: 628
	public enum LetterPivot
	{
		// Token: 0x04000F8F RID: 3983
		center,
		// Token: 0x04000F90 RID: 3984
		baselineCenter,
		// Token: 0x04000F91 RID: 3985
		baselineLeft,
		// Token: 0x04000F92 RID: 3986
		baselineRight
	}
}
