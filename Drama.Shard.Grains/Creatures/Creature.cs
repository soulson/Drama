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

using Drama.Auth.Interfaces.Utilities;
using Drama.Core.Interfaces;
using Drama.Shard.Grains.Units;
using Drama.Shard.Interfaces.Creatures;
using Drama.Shard.Interfaces.Objects;
using Orleans;
using Orleans.Providers;
using System.Linq;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Creatures
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public sealed class Creature : AbstractCreature<CreatureEntity>, ICreature
	{

	}

	public abstract class AbstractCreature<TEntity> : AbstractUnit<TEntity>, ICreature<TEntity>, IObjectObserver
		where TEntity : CreatureEntity, new()
	{
		public async Task Create(CreatureDefinitionEntity creatureDefinition)
		{
			var randomService = GrainFactory.GetGrain<IRandomService>(0);

			State.Enabled = true;

			State.Id = new ObjectID(this.GetPrimaryKeyLong());
			State.Entry = creatureDefinition.Id;

			State.Scale = creatureDefinition.Scale;
			State.CombatReach = 1.5f; // TODO: placeholders
			State.BoundingRadius = 0.5f;

			State.Level = await randomService.GetRandomRange(creatureDefinition.Level);
			State.Health = State.HealthBase = State.HealthMax = await randomService.GetRandomRange(creatureDefinition.Health);
			State.Mana = State.ManaBase = State.ManaMax = await randomService.GetRandomRange(creatureDefinition.Mana);

			State.DisplayID = State.NativeDisplayID = creatureDefinition.ModelIds.ElementAt(await randomService.GetRandomInt(creatureDefinition.ModelIds.Count));

			State.Armor = creatureDefinition.Armor;

			State.AttackDamageMainhandMin = creatureDefinition.AttackDamage.Low;
			State.AttackDamageMainhandMax = creatureDefinition.AttackDamage.High;
			State.AttackDamageRangedMin = creatureDefinition.AttackDamageRanged.Low;
			State.AttackDamageRangedMax = creatureDefinition.AttackDamageRanged.High;

			State.AttackPower = creatureDefinition.AttackPower;
			State.AttackPowerRanged = creatureDefinition.AttackPowerRanged;

			State.AttackTimeMainhandMilliseconds = creatureDefinition.AttackTimeMilliseconds;
			State.AttackTimeRangedMilliseconds = creatureDefinition.AttackTimeRangedMilliseconds;

			State.Class = creatureDefinition.Class;
			State.UnitFlags = creatureDefinition.UnitFlags;
			State.NpcFlags = creatureDefinition.NpcFlags;

			State.MoveSpeed.Walking = State.MoveSpeed.Swimming = creatureDefinition.SpeedWalking;
			State.MoveSpeed.Running = State.MoveSpeed.Running;

			await WriteStateAsync();
		}

		public Task<CreatureEntity> GetCreatureEntity()
		{
			VerifyExists();

			return Task.FromResult<CreatureEntity>(State);
		}
	}
}
