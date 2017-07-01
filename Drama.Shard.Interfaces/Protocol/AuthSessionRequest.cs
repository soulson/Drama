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
using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Utilities;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	[ClientPacket(ShardClientOpcode.AuthSession)]
	public sealed class AuthSessionRequest : IInPacket
	{
		public int ClientBuild { get; set; }
		public string Identity { get; set; }
		public int ClientSeed { get; set; }
		public BigInteger ClientDigest { get; set; }
		public IList<AddonBlock> AddonBlocks { get; } = new List<AddonBlock>();

		public bool Read(Stream stream)
		{
			try
			{
				using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
				{
					ClientBuild = reader.ReadInt32();

					stream.Seek(4, SeekOrigin.Current);
					Identity = reader.ReadNullTerminatedString(Encoding.UTF8);
					ClientSeed = reader.ReadInt32();
					ClientDigest = reader.ReadBigInteger(20);

					var addonBlockSize = reader.ReadInt32();
					if (addonBlockSize < 0 || addonBlockSize > 0xfffff)
						throw new DramaException($"addon data block size {addonBlockSize} must be 0 <= blockSize <= 0xfffff");

					// skip the zlib header, which is 2 bytes
					stream.Seek(2, SeekOrigin.Current);

					using (var deflateStream = new DeflateStream(stream, CompressionMode.Decompress, true))
					{
						var addonData = new byte[addonBlockSize];
						var bytesDecompressed = deflateStream.Read(addonData, 0, addonData.Length);

						if (bytesDecompressed != addonBlockSize)
							throw new DramaException($"decompressing addon data block, expected {addonBlockSize} bytes but read {bytesDecompressed}");

						AddonBlocks.Clear();

						using (var addonReader = new BinaryReader(new MemoryStream(addonData, false), Encoding.UTF8, false))
						{
							while(addonReader.BaseStream.Position < addonReader.BaseStream.Length)
							{
								var addonName = addonReader.ReadNullTerminatedString(Encoding.UTF8);
								var addonCrc = addonReader.ReadInt32();

								addonReader.BaseStream.Seek(5, SeekOrigin.Current);

								AddonBlocks.Add(new AddonBlock()
								{
									Name = addonName,
									Crc = addonCrc,
								});
							}
						}
					}
				}

				return true;
			}
			catch (EndOfStreamException)
			{
				return false;
			}
		}

		public sealed class AddonBlock
		{
			public string Name { get; set; }
			public int Crc { get; set; }
		}
	}
}
