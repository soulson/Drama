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

using Drama.Shard.Interfaces.Objects;

namespace Drama.Shard.Interfaces.Units
{
	public enum UnitFields : short
	{
		Charm = 0x00 + ObjectFields.END, // Size:2
		Summon = 0x02 + ObjectFields.END, // Size:2
		CharmedBy = 0x04 + ObjectFields.END, // Size:2
		SummonedBy = 0x06 + ObjectFields.END, // Size:2
		CreatedBy = 0x08 + ObjectFields.END, // Size:2
		Target = 0x0A + ObjectFields.END, // Size:2
		Persuaded = 0x0C + ObjectFields.END, // Size:2
		ChannelObject = 0x0E + ObjectFields.END, // Size:2
		Health = 0x10 + ObjectFields.END, // Size:1
		Mana = 0x11 + ObjectFields.END, // Size:1
		Rage = 0x12 + ObjectFields.END, // Size:1
		Focus = 0x13 + ObjectFields.END, // Size:1
		Energy = 0x14 + ObjectFields.END, // Size:1
		Happiness = 0x15 + ObjectFields.END, // Size:1
		MaxHealth = 0x16 + ObjectFields.END, // Size:1
		MaxMana = 0x17 + ObjectFields.END, // Size:1
		MaxRage = 0x18 + ObjectFields.END, // Size:1
		MaxFocus = 0x19 + ObjectFields.END, // Size:1
		MaxEnergy = 0x1A + ObjectFields.END, // Size:1
		MaxHappiness = 0x1B + ObjectFields.END, // Size:1
		Level = 0x1C + ObjectFields.END, // Size:1
		FactionTemplate = 0x1D + ObjectFields.END, // Size:1
		Bytes0 = 0x1E + ObjectFields.END, // Size:1
		VirtualItemSlotDisplay = 0x1F + ObjectFields.END, // Size:3
		VirtualItemSlotDisplay1 = 0x20 + ObjectFields.END,
		VirtualItemSlotDisplay2 = 0x21 + ObjectFields.END,
		VirtualItemInfo = 0x22 + ObjectFields.END, // Size:6
		VirtualItemInfo1 = 0x23 + ObjectFields.END,
		VirtualItemInfo2 = 0x24 + ObjectFields.END,
		VirtualItemInfo3 = 0x25 + ObjectFields.END,
		VirtualItemInfo4 = 0x26 + ObjectFields.END,
		VirtualItemInfo5 = 0x27 + ObjectFields.END,
		Flags = 0x28 + ObjectFields.END, // Size:1
		AuraFirst = 0x29 + ObjectFields.END, // Size:48
		AuraLast = 0x58 + ObjectFields.END,
		AuraFlags = 0x59 + ObjectFields.END, // Size:6
		AuraFlags1 = 0x5a + ObjectFields.END,
		AuraFlags2 = 0x5b + ObjectFields.END,
		AuraFlags3 = 0x5c + ObjectFields.END,
		AuraFlags4 = 0x5d + ObjectFields.END,
		AuraFlags5 = 0x5e + ObjectFields.END,
		AuraLevelsFirst = 0x5f + ObjectFields.END, // Size:12
		AuraLevelsLast = 0x6a + ObjectFields.END,
		AuraApplicationsFirst = 0x6b + ObjectFields.END, // Size:12
		AuraApplicationsLast = 0x76 + ObjectFields.END,
		AuraState = 0x77 + ObjectFields.END, // Size:1
		AttackTimeBase = 0x78 + ObjectFields.END, // Size:2
		AttackTimeOffhand = 0x79 + ObjectFields.END, // Size:2
		AttackTimeRanged = 0x7a + ObjectFields.END, // Size:1
		BoundingRadius = 0x7b + ObjectFields.END, // Size:1
		CombatReach = 0x7c + ObjectFields.END, // Size:1
		DisplayID = 0x7d + ObjectFields.END, // Size:1
		NativeDisplayID = 0x7e + ObjectFields.END, // Size:1
		MountDisplayID = 0x7f + ObjectFields.END, // Size:1
		DamageMin = 0x80 + ObjectFields.END, // Size:1
		DamageMax = 0x81 + ObjectFields.END, // Size:1
		DamageOffhandMin = 0x82 + ObjectFields.END, // Size:1
		DamageOffhandMax = 0x83 + ObjectFields.END, // Size:1
		Bytes1 = 0x84 + ObjectFields.END, // Size:1
		PetNumber = 0x85 + ObjectFields.END, // Size:1
		PetNameTimestamp = 0x86 + ObjectFields.END, // Size:1
		PetExperience = 0x87 + ObjectFields.END, // Size:1
		PetNextLevelExperience = 0x88 + ObjectFields.END, // Size:1
		DynamicFlags = 0x89 + ObjectFields.END, // Size:1
		ChannelSpell = 0x8a + ObjectFields.END, // Size:1
		ModCastSpeed = 0x8b + ObjectFields.END, // Size:1
		CreatedBySpell = 0x8c + ObjectFields.END, // Size:1
		NpcFlags = 0x8d + ObjectFields.END, // Size:1
		NpcEmoteState = 0x8e + ObjectFields.END, // Size:1
		TrainingPoints = 0x8f + ObjectFields.END, // Size:1
		Strength = 0x90 + ObjectFields.END, // Size:1
		Agility = 0x91 + ObjectFields.END, // Size:1
		Stamina = 0x92 + ObjectFields.END, // Size:1
		Intellect = 0x93 + ObjectFields.END, // Size:1
		Spirit = 0x94 + ObjectFields.END, // Size:1
		Armor = 0x95 + ObjectFields.END, // Size:1
		ResistanceHoly = 0x96 + ObjectFields.END, // Size:1
		ResistanceFire = 0x97 + ObjectFields.END, // Size:1
		ResistanceNature = 0x98 + ObjectFields.END, // Size:1
		ResistanceFrost = 0x99 + ObjectFields.END, // Size:1
		ResistanceShadow = 0x9a + ObjectFields.END, // Size:1
		ResistanceArcane = 0x9b + ObjectFields.END, // Size:1
		BaseMana = 0x9c + ObjectFields.END, // Size:1
		BaseHealth = 0x9d + ObjectFields.END, // Size:1
		Bytes2 = 0x9e + ObjectFields.END, // Size:1
		AttackPower = 0x9f + ObjectFields.END, // Size:1
		AttackPowerMods = 0xa0 + ObjectFields.END, // Size:1
		AttackPowerMultiplier = 0xa1 + ObjectFields.END, // Size:1
		AttackPowerRanged = 0xa2 + ObjectFields.END, // Size:1
		AttackPowerRangedMods = 0xa3 + ObjectFields.END, // Size:1
		AttackPowerRangedMultiplier = 0xa4 + ObjectFields.END, // Size:1
		DamageRangedMin = 0xa5 + ObjectFields.END, // Size:1
		DamageRangedMax = 0xa6 + ObjectFields.END, // Size:1
		PowerCostModifier = 0xa7 + ObjectFields.END, // Size:7
		PowerCostModifier1 = 0xa8 + ObjectFields.END,
		PowerCostModifier2 = 0xa9 + ObjectFields.END,
		PowerCostModifier3 = 0xaa + ObjectFields.END,
		PowerCostModifier4 = 0xab + ObjectFields.END,
		PowerCostModifier5 = 0xac + ObjectFields.END,
		PowerCostModifier6 = 0xad + ObjectFields.END,
		PowerCostMultiplier = 0xae + ObjectFields.END, // Size:7
		PowerCostMultiplier1 = 0xaf + ObjectFields.END,
		PowerCostMultiplier2 = 0xb0 + ObjectFields.END,
		PowerCostMultiplier3 = 0xb1 + ObjectFields.END,
		PowerCostMultiplier4 = 0xb2 + ObjectFields.END,
		PowerCostMultiplier5 = 0xb3 + ObjectFields.END,
		PowerCostMultiplier6 = 0xb4 + ObjectFields.END,
		Padding = 0xb5 + ObjectFields.END,

		END = 0xb6 + ObjectFields.END,
	}
}
