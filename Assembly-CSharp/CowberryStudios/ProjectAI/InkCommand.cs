using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CowberryStudios.ProjectAI
{
	// Token: 0x02000264 RID: 612
	public class InkCommand : IReadOnlyCollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x0005F7EC File Offset: 0x0005D9EC
		public CommandID Id { get; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x0005F7F4 File Offset: 0x0005D9F4
		public int Count
		{
			get
			{
				return this._arguments.Length;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x0005F7FE File Offset: 0x0005D9FE
		public bool IsEmpty
		{
			get
			{
				return this.Id == CommandID.Empty;
			}
		}

		// Token: 0x17000119 RID: 281
		public string this[int index]
		{
			get
			{
				if (index < 0 || index >= this._arguments.Length)
				{
					throw new IndexOutOfRangeException();
				}
				return this._arguments[index];
			}
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0005F830 File Offset: 0x0005DA30
		public InkCommand(CommandID id, IEnumerable<string> arguments)
		{
			this.Id = id;
			this._arguments = new string[arguments.Count<string>()];
			int num = 0;
			foreach (string text in arguments)
			{
				this._arguments[num++] = text;
			}
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0005F8A0 File Offset: 0x0005DAA0
		public string[] AsArray()
		{
			return this._arguments;
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0005F8A8 File Offset: 0x0005DAA8
		public IEnumerator<string> GetEnumerator()
		{
			return ((IEnumerable<string>)this._arguments).GetEnumerator();
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0005F8B5 File Offset: 0x0005DAB5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._arguments.GetEnumerator();
		}

		// Token: 0x04000F60 RID: 3936
		private string[] _arguments;
	}
}
