using System;

namespace CowberryStudios.ProjectAI
{
	// Token: 0x02000260 RID: 608
	public struct CommandID : IEquatable<CommandID>
	{
		// Token: 0x060013C2 RID: 5058 RVA: 0x0005F531 File Offset: 0x0005D731
		public CommandID(string name)
		{
			this._name = name.ToLowerInvariant();
			this._hash = Util.FNVHash(this._name);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0005F550 File Offset: 0x0005D750
		public static bool operator ==(CommandID lhs, CommandID rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0005F55A File Offset: 0x0005D75A
		public static bool operator !=(CommandID lhs, CommandID rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0005F567 File Offset: 0x0005D767
		public static implicit operator CommandID(string value)
		{
			return new CommandID(value);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0005F56F File Offset: 0x0005D76F
		public bool Equals(CommandID other)
		{
			return other._hash == this._hash;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0005F580 File Offset: 0x0005D780
		public override bool Equals(object obj)
		{
			if (obj is CommandID)
			{
				CommandID commandID = (CommandID)obj;
				return this.Equals(commandID);
			}
			return false;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0005F5A5 File Offset: 0x0005D7A5
		public override int GetHashCode()
		{
			return (int)this._hash;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0005F5AD File Offset: 0x0005D7AD
		public override string ToString()
		{
			return this._name;
		}

		// Token: 0x04000F59 RID: 3929
		public static readonly CommandID Empty;

		// Token: 0x04000F5A RID: 3930
		private string _name;

		// Token: 0x04000F5B RID: 3931
		private uint _hash;
	}
}
