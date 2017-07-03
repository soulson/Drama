/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Formats.Dbc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Drama.Tools.Load.Formats.Dbc
{
	/// <summary>
	/// A simple RDB file format containing rows and columns but no schema
	/// information.
	/// </summary>
	public class Dbc<TRow> : IEnumerable<TRow>, IDisposable
		where TRow : new()
	{
		private const int Magic = 0x43424457;
		private const int HeaderLength = 20;

		public int RowCount { get; }
		protected int TableLength { get; }
		protected int RowSize { get; }
		protected int StringTableLength { get; }
		protected long BasePosition { get; }
		protected BinaryReader Reader { get; }
		private IEnumerable<FieldDefinition> RowDefinition { get; }

		private long TablePosition => BasePosition + HeaderLength;
		private long StringTablePosition => BasePosition + HeaderLength + RowCount * RowSize;

		private sealed class FieldDefinition
		{
			public int Offset { get; }
			public PropertyInfo Property { get; }

			public FieldDefinition(int offset, PropertyInfo property)
			{
				Offset = offset;
				Property = property ?? throw new ArgumentNullException(nameof(property));
			}
		}

		/// <summary>
		/// Creates a new Dbc object by reading the given stream. Takes ownership
		/// of the stream; the stream will be disposed with the Dbc object.
		/// </summary>
		/// <param name="stream">Cannot be null</param>
		public Dbc(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			BasePosition = stream.Position;
			Reader = new BinaryReader(stream, Encoding.UTF8, false);

			int magic = Reader.ReadInt32();
			if (magic != Magic)
				throw new InvalidDataException("this stream is not a valid DBC");

			RowCount = Reader.ReadInt32();
			TableLength = Reader.ReadInt32();
			RowSize = Reader.ReadInt32();
			StringTableLength = Reader.ReadInt32();

			RowDefinition =
				from property in typeof(TRow).GetTypeInfo().GetProperties()
				where property.GetCustomAttribute<DbcFieldOffsetAttribute>() != null
					 && property.SetMethod != null
				select new FieldDefinition(property.GetCustomAttribute<DbcFieldOffsetAttribute>().Offset, property);
		}

		/// <summary>
		/// Gets an enumerator that iterates over every row in this DBC in order.
		/// </summary>
		public IEnumerator<TRow> GetEnumerator()
		{
			for (int i = 0; i < RowCount; ++i)
			{
				TRow row = new TRow();

				foreach (var field in RowDefinition)
				{
					var type = field.Property.PropertyType;
					Reader.BaseStream.Position = TablePosition + i * RowSize + field.Offset;

					if (type == typeof(int) || (type.GetTypeInfo().IsEnum && type.GetTypeInfo().GetEnumUnderlyingType() == typeof(int)))
						field.Property.SetValue(row, Reader.ReadInt32());
					else if (type == typeof(float))
						field.Property.SetValue(row, Reader.ReadSingle());
					else if (type == typeof(string))
						field.Property.SetValue(row, ReadString(Reader.ReadInt32()));
					else
						throw new DramaException($"illegal dbc field type: {type.Name}");
				}

				yield return row;
			}
		}

		private string ReadString(int offset)
		{
			var originalPosition = Reader.BaseStream.Position;
			Reader.BaseStream.Position = StringTablePosition + offset;

			var result = Reader.ReadNullTerminatedString(Encoding.UTF8);

			Reader.BaseStream.Position = originalPosition;
			return result;
		}

		/// <summary>
		/// Gets an enumerator that iterates over every row in this DBC in order.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
			=> GetEnumerator();

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					Reader.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Dbc() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
