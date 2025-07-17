using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class Squirrel : MonoBehaviour
{
	// Token: 0x06000016 RID: 22 RVA: 0x000023A5 File Offset: 0x000005A5
	private void Start()
	{
		this.squirrel = base.GetComponent<Animator>();
		this.characterController = base.GetComponent<CharacterController>();
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000023C0 File Offset: 0x000005C0
	private void Update()
	{
		this.characterController.Move(this.moveDirection * Time.deltaTime);
		this.moveDirection.y = this.gravity * Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.Speed1 = !this.Speed1;
			this.Speed2 = false;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.Speed2 = !this.Speed2;
			this.Speed1 = false;
		}
		if (this.squirrel.GetCurrentAnimatorStateInfo(0).IsName("idle"))
		{
			this.squirrel.SetBool("jump", false);
			this.squirrel.SetBool("up", false);
			this.squirrel.SetBool("down", false);
		}
		if (this.squirrel.GetCurrentAnimatorStateInfo(0).IsName("stand"))
		{
			this.squirrel.SetBool("eat", false);
		}
		if (Input.GetKeyDown(KeyCode.W) && this.Speed1)
		{
			this.squirrel.SetBool("idle", false);
			this.squirrel.SetBool("walk", true);
		}
		if (Input.GetKeyDown(KeyCode.W) && this.Speed2)
		{
			this.squirrel.SetBool("idle", false);
			this.squirrel.SetBool("run", true);
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			this.squirrel.SetBool("walk", false);
			this.squirrel.SetBool("run", false);
			this.squirrel.SetBool("idle", true);
		}
		if (Input.GetKeyDown(KeyCode.A) && this.Speed1)
		{
			this.squirrel.SetBool("left", true);
			this.squirrel.SetBool("idle", false);
			this.squirrel.SetBool("walk", false);
		}
		if (Input.GetKeyDown(KeyCode.A) && this.Speed2)
		{
			this.squirrel.SetBool("runleft", true);
			this.squirrel.SetBool("run", false);
			this.squirrel.SetBool("idle", false);
		}
		if (Input.GetKeyUp(KeyCode.A))
		{
			this.squirrel.SetBool("runleft", false);
			this.squirrel.SetBool("left", false);
			this.squirrel.SetBool("idle", true);
		}
		if (Input.GetKeyDown(KeyCode.D) && this.Speed1)
		{
			this.squirrel.SetBool("right", true);
			this.squirrel.SetBool("idle", false);
			this.squirrel.SetBool("walk", false);
		}
		if (Input.GetKeyDown(KeyCode.D) && this.Speed2)
		{
			this.squirrel.SetBool("runright", true);
			this.squirrel.SetBool("run", false);
			this.squirrel.SetBool("idle", false);
		}
		if (Input.GetKeyUp(KeyCode.D))
		{
			this.squirrel.SetBool("runright", false);
			this.squirrel.SetBool("right", false);
			this.squirrel.SetBool("idle", true);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.squirrel.SetBool("jump", true);
			this.squirrel.SetBool("idle", false);
		}
		if (Input.GetKeyDown("up"))
		{
			this.squirrel.SetBool("up", true);
			this.squirrel.SetBool("idle", false);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			this.squirrel.SetBool("stand", false);
			this.squirrel.SetBool("eat", true);
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			this.squirrel.SetBool("eat", false);
			this.squirrel.SetBool("stand", true);
		}
		if (Input.GetKeyDown("down"))
		{
			this.squirrel.SetBool("down", true);
			this.squirrel.SetBool("stand", false);
		}
	}

	// Token: 0x04000011 RID: 17
	private Animator squirrel;

	// Token: 0x04000012 RID: 18
	private bool Speed1 = true;

	// Token: 0x04000013 RID: 19
	private bool Speed2;

	// Token: 0x04000014 RID: 20
	public float gravity = 1f;

	// Token: 0x04000015 RID: 21
	private Vector3 moveDirection = Vector3.zero;

	// Token: 0x04000016 RID: 22
	private CharacterController characterController;
}
