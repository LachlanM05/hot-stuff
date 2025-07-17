using System;
using System.Collections.Generic;
using Team17.Common;
using Team17.Scripts.Input.Rumble;
using UnityEngine;

namespace Team17.Input.Unity
{
	// Token: 0x020001E5 RID: 485
	public class ControllerRumbleService : IControllerRumbleService, IService
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x00055817 File Offset: 0x00053A17
		private bool AnyActiveRumbles
		{
			get
			{
				return this._rumbles.First != null;
			}
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00055827 File Offset: 0x00053A27
		public ControllerRumbleService(IControllerRumbleFactory controllerRumbleFactory)
		{
			this._controllerDeviceRumbleHandler = new RewiredEngagedControllerRumbleHandler();
			this._controllerRumbleFactory = controllerRumbleFactory;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0005584C File Offset: 0x00053A4C
		public void OnStartUp()
		{
			this._controllerDeviceRumbleHandler.OnStartUp();
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00055859 File Offset: 0x00053A59
		public void OnCleanUp()
		{
			this._controllerDeviceRumbleHandler.OnCleanUp();
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00055866 File Offset: 0x00053A66
		public void OnUpdate(float deltaTime)
		{
			this._controllerDeviceRumbleHandler.OnUpdate(deltaTime);
			if (!this.AnyActiveRumbles)
			{
				return;
			}
			this.ApplyRumbles(deltaTime);
			this.RemoveCompletedRumbles();
			if (!this.AnyActiveRumbles)
			{
				this.OnRumblesEnded();
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00055898 File Offset: 0x00053A98
		public void RequestRumble(RumbleProfileData rumbleProfileData, bool looping)
		{
			ControllerRumble controllerRumble = this._controllerRumbleFactory.RequestControllerRumble();
			controllerRumble.Setup(rumbleProfileData, looping);
			this._rumbles.AddLast(controllerRumble);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x000558C6 File Offset: 0x00053AC6
		public void StopRumble(RumbleProfileData rumbleProfileData)
		{
			if (!this.AnyActiveRumbles)
			{
				return;
			}
			this.RemoveAllRumblesWithProfile(rumbleProfileData);
			if (!this.AnyActiveRumbles)
			{
				this.OnRumblesEnded();
			}
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x000558E8 File Offset: 0x00053AE8
		public void StopAll()
		{
			if (!this.AnyActiveRumbles)
			{
				return;
			}
			foreach (ControllerRumble controllerRumble in this._rumbles)
			{
				this._controllerRumbleFactory.ReturnControllerRumble(controllerRumble);
			}
			this._rumbles.Clear();
			this.OnRumblesEnded();
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0005595C File Offset: 0x00053B5C
		public void OnApplicationFocus(bool focus)
		{
			if (!focus)
			{
				this._controllerDeviceRumbleHandler.StopDeviceRumble();
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0005596C File Offset: 0x00053B6C
		private void OnRumblesEnded()
		{
			this._controllerDeviceRumbleHandler.StopDeviceRumble();
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0005597C File Offset: 0x00053B7C
		private void ApplyRumbles(float deltaTime)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (ControllerRumble controllerRumble in this._rumbles)
			{
				controllerRumble.Update(deltaTime);
				num = Mathf.Max(controllerRumble.LeftRumble, num);
				num2 = Mathf.Max(controllerRumble.RightRumble, num2);
			}
			this._controllerDeviceRumbleHandler.SetDeviceRumble(num, num2);
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00055A00 File Offset: 0x00053C00
		private void RemoveCompletedRumbles()
		{
			LinkedListNode<ControllerRumble> next;
			for (LinkedListNode<ControllerRumble> linkedListNode = this._rumbles.First; linkedListNode != null; linkedListNode = next)
			{
				next = linkedListNode.Next;
				if (linkedListNode.Value.IsCompleted)
				{
					this.RemoveRumble(linkedListNode);
				}
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00055A3C File Offset: 0x00053C3C
		private void RemoveAllRumblesWithProfile(RumbleProfileData rumbleProfileData)
		{
			LinkedListNode<ControllerRumble> next;
			for (LinkedListNode<ControllerRumble> linkedListNode = this._rumbles.First; linkedListNode != null; linkedListNode = next)
			{
				next = linkedListNode.Next;
				if (linkedListNode.Value.ProfileData == rumbleProfileData)
				{
					this.RemoveRumble(linkedListNode);
				}
			}
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00055A7C File Offset: 0x00053C7C
		private void RemoveRumble(LinkedListNode<ControllerRumble> node)
		{
			ControllerRumble value = node.Value;
			this._rumbles.Remove(node);
			this._controllerRumbleFactory.ReturnControllerRumble(value);
		}

		// Token: 0x04000DE8 RID: 3560
		private readonly IControllerDeviceRumbleHandler _controllerDeviceRumbleHandler;

		// Token: 0x04000DE9 RID: 3561
		private readonly IControllerRumbleFactory _controllerRumbleFactory;

		// Token: 0x04000DEA RID: 3562
		private readonly LinkedList<ControllerRumble> _rumbles = new LinkedList<ControllerRumble>();
	}
}
