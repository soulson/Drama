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

using Drama.Shard.Interfaces.Units;

namespace Drama.Shard.Interfaces.Characters
{
	public enum CharacterFields : short
	{
		DuelArbiter = 0x00 + UnitFields.END, // Size:2
		Flags = 0x02 + UnitFields.END, // Size:1
		GuildID = 0x03 + UnitFields.END, // Size:1
		GuildRank = 0x04 + UnitFields.END, // Size:1
		Bytes1 = 0x05 + UnitFields.END, // Size:1
		Bytes2 = 0x06 + UnitFields.END, // Size:1
		Bytes3 = 0x07 + UnitFields.END, // Size:1
		DuelTeam = 0x08 + UnitFields.END, // Size:1
		GuildTimestamp = 0x09 + UnitFields.END, // Size:1
		QuestLogFirst1 = 0x0A + UnitFields.END, // count = 20
		QuestLogFirst2 = 0x0B + UnitFields.END,
		QuestLogFirst3 = 0x0C + UnitFields.END,
		QuestLogLast1 = 0x43 + UnitFields.END,
		QuestLogLast2 = 0x44 + UnitFields.END,
		QuestLogLast3 = 0x45 + UnitFields.END,
		VisibleItemFirstCreator = 0x46 + UnitFields.END, // Size:2, count = 19
		VisibleItemFirst0 = 0x48 + UnitFields.END, // Size:8
		VisibleItemFirstProperties = 0x50 + UnitFields.END, // Size:1
		VisibleItemFirstPadding = 0x51 + UnitFields.END, // Size:1
		VisibleItemLastCreator = 0x11e + UnitFields.END,
		VisibleItemLast0 = 0x120 + UnitFields.END,
		VisibleItemLastProperties = 0x128 + UnitFields.END,
		VisibleItemLastPadding = 0x129 + UnitFields.END,
		InventorySlotHead = 0x12a + UnitFields.END, // Size:46
		PackSlotFirst = 0x158 + UnitFields.END, // Size:32
		PackSlotLast = 0x176 + UnitFields.END,
		BankSlotFirst = 0x178 + UnitFields.END, // Size:48
		BankSlotLast = 0x1a6 + UnitFields.END,
		BankBagSlotFirst = 0x1a8 + UnitFields.END, // Size:12
		BankBagSlotLast = 0x1b2 + UnitFields.END,
		VendorBuybackSlotFirst = 0x1b4 + UnitFields.END, // Size:24
		VendorBuybackSlotLast = 0x1ca + UnitFields.END,
		KeyringSlotFirst = 0x1cc + UnitFields.END, // Size:64
		KeyringSlotLast = 0x20a + UnitFields.END,
		FarSight = 0x20c + UnitFields.END, // Size:2
		ComboTarget = 0x20e + UnitFields.END, // Size:2
		XP = 0x210 + UnitFields.END, // Size:1
		XPNextLevel = 0x211 + UnitFields.END, // Size:1
		SkillInfoFirst1 = 0x212 + UnitFields.END, // Size:384
		CharacterPoints1 = 0x392 + UnitFields.END, // Size:1
		CharacterPoints2 = 0x393 + UnitFields.END, // Size:1
		TrackCreatures = 0x394 + UnitFields.END, // Size:1
		TrackResources = 0x395 + UnitFields.END, // Size:1
		BlockPercent = 0x396 + UnitFields.END, // Size:1
		DodgePercent = 0x397 + UnitFields.END, // Size:1
		ParryPercent = 0x398 + UnitFields.END, // Size:1
		CritPercent = 0x399 + UnitFields.END, // Size:1
		CritPercentRanged = 0x39a + UnitFields.END, // Size:1
		ExploredZones1 = 0x39b + UnitFields.END, // Size:64
		XPRestState = 0x3db + UnitFields.END, // Size:1
		Coinage = 0x3dc + UnitFields.END, // Size:1
		Stat0Pos = 0x3DD + UnitFields.END, // Size:1
		Stat1Pos = 0x3DE + UnitFields.END, // Size:1
		Stat2Pos = 0x3DF + UnitFields.END, // Size:1
		Stat3Pos = 0x3E0 + UnitFields.END, // Size:1
		Stat4Pos = 0x3E1 + UnitFields.END, // Size:1
		Stat0Neg = 0x3E2 + UnitFields.END, // Size:1
		Stat1Neg = 0x3E3 + UnitFields.END, // Size:1
		Stat2Neg = 0x3E4 + UnitFields.END, // Size:1
		Stat3Neg = 0x3E5 + UnitFields.END, // Size:1,
		Stat4Neg = 0x3E6 + UnitFields.END, // Size:1
		ResistanceBuffModsPositive0 = 0x3E7 + UnitFields.END, // Size:7
		ResistanceBuffModsNegative0 = 0x3EE + UnitFields.END, // Size:7
		DamageDoneModPos = 0x3F5 + UnitFields.END, // Size:7
		DamageDoneModNeg = 0x3FC + UnitFields.END, // Size:7
		DamageDonePhysicalModPercent = 0x403 + UnitFields.END, // Size:1
		DamageDoneHolyModPercent = 0x404 + UnitFields.END, // Size:1
		DamageDoneFireModPercent = 0x405 + UnitFields.END, // Size:1
		DamageDoneNatureModPercent = 0x406 + UnitFields.END, // Size:1
		DamageDoneFrostModPercent = 0x407 + UnitFields.END, // Size:1
		DamageDoneShadowModPercent = 0x408 + UnitFields.END, // Size:1
		DamageDoneArcaneModPercent = 0x409 + UnitFields.END, // Size:1
		Bytes4 = 0x40a + UnitFields.END, // Size:1
		AmmoID = 0x40b + UnitFields.END, // Size:1
		SelfResSpell = 0x40c + UnitFields.END, // Size:1
		PvpMedals = 0x40d + UnitFields.END, // Size:1
		BuybackPriceFirst = 0x40e + UnitFields.END, // count=12
		BuybackPriceLast = 0x419 + UnitFields.END,
		BuybackTimestampFirst = 0x41a + UnitFields.END, // count=12
		BuybackTimestampLast = 0x425 + UnitFields.END,
		HKsSession = 0x426 + UnitFields.END, // Size:1
		HKsYesterday = 0x427 + UnitFields.END, // Size:1
		HKsLastWeek = 0x428 + UnitFields.END, // Size:1
		HKsThisWeek = 0x429 + UnitFields.END, // Size:1
		ContributionThisWeek = 0x42a + UnitFields.END, // Size:1
		HKsLifetime = 0x42b + UnitFields.END, // Size:1
		DKsLifetime = 0x42c + UnitFields.END, // Size:1
		ContributionYesterday = 0x42d + UnitFields.END, // Size:1
		ContributionLastWeek = 0x42e + UnitFields.END, // Size:1
		RankLastWeek = 0x42f + UnitFields.END, // Size:1
		Bytes5 = 0x430 + UnitFields.END, // Size:1
		WatchedFactionIndex = 0x431 + UnitFields.END, // Size:1
		CombatRating1 = 0x432 + UnitFields.END, // Size:20

		END = 0x446 + UnitFields.END
	}
}
