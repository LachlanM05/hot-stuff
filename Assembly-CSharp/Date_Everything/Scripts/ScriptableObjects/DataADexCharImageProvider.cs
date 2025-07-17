using System;
using UnityEngine;

namespace Date_Everything.Scripts.ScriptableObjects
{
	// Token: 0x0200026D RID: 621
	public class DataADexCharImageProvider
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x00060992 File Offset: 0x0005EB92
		public Sprite CharImage
		{
			get
			{
				return this.LazyLoadCharImage();
			}
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0006099A File Offset: 0x0005EB9A
		public DataADexCharImageProvider(string internalName, RelationshipStatus status)
		{
			this._status = status;
			this.SetPoseAndExpression();
			this._characterUtility = Singleton<CharacterHelper>.Instance._characters[internalName];
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x000609C8 File Offset: 0x0005EBC8
		private void SetPoseAndExpression()
		{
			switch (this._status)
			{
			case RelationshipStatus.Hate:
				this._pose = E_General_Poses.hate;
				this._expression = E_Facial_Expressions.angry;
				return;
			case RelationshipStatus.Love:
				this._pose = E_General_Poses.love;
				this._expression = E_Facial_Expressions.flirt;
				return;
			case RelationshipStatus.Friend:
				this._pose = E_General_Poses.friend;
				this._expression = E_Facial_Expressions.joy;
				return;
			case RelationshipStatus.Realized:
				this._pose = E_General_Poses.realized;
				this._expression = E_Facial_Expressions.joy;
				return;
			}
			this._pose = E_General_Poses.neutral;
			this._expression = E_Facial_Expressions.neutral;
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x00060A4A File Offset: 0x0005EC4A
		public void ReleaseImage()
		{
			if (this._charImage == null)
			{
				return;
			}
			CharacterUtility.ReturnCharacterSprite(this._charImage);
			this._charImage = null;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00060A70 File Offset: 0x0005EC70
		private Sprite LazyLoadCharImage()
		{
			if (this._characterUtility != null)
			{
				RelationshipStatus dateStatus = Singleton<Save>.Instance.GetDateStatus(this._characterUtility.internalName);
				if (dateStatus != this._status || this._characterUtility.internalName == "dirk")
				{
					this._status = dateStatus;
					this.SetPoseAndExpression();
					this._charImage = null;
				}
			}
			if (this._charImage != null)
			{
				return this._charImage;
			}
			this._charImage = this._characterUtility.GetSpriteFromPoseExpression(this._pose, this._expression, false, true);
			return this._charImage;
		}

		// Token: 0x04000F78 RID: 3960
		private E_General_Poses _pose;

		// Token: 0x04000F79 RID: 3961
		private E_Facial_Expressions _expression;

		// Token: 0x04000F7A RID: 3962
		private RelationshipStatus _status;

		// Token: 0x04000F7B RID: 3963
		private Sprite _charImage;

		// Token: 0x04000F7C RID: 3964
		private CharacterUtility _characterUtility;
	}
}
