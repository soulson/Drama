using Drama.Shard.Interfaces.Objects;
using System;
using System.Collections.Generic;

namespace Drama.Shard.Interfaces.Units
{
	public class UnitEntity : ObjectEntity
	{
		public UnitEntity() : this((short)UnitFields.END)
		{

		}

		protected UnitEntity(int fieldCount) : base(fieldCount)
		{
			TypeId = ObjectTypeID.Unit;
		}

		public Race Race { get; set; }
		public Class Class { get; set; }
		public Sex Sex { get; set; }
		public byte Level { get; set; }
	}
}
