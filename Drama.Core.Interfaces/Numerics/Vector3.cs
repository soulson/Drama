﻿/* 
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

using System;

namespace Drama.Core.Interfaces.Numerics
{
	/// <summary>
	/// This class exists as a workaround for the following bug. Should be using System.Numerics.Vector3 instead:
	/// 
	/// 2>Orleans-CodeGen - Generating file C:\Users\foxic\documents\Projects\Drama\Drama.Shard.Interfaces\obj\Debug\netcoreapp1.1\Drama.Shard.Interfaces.orleans.g.cs
	/// 2>-- Code-gen FAILED --
	/// 2>
	/// 2>Exc level 0: System.BadImageFormatException: Could not load file or assembly 'file:///C:\Users\foxic\.nuget\packages\system.numerics.vectors\4.3.0\ref\netstandard1.0\System.Numerics.Vectors.dll' or one of its dependencies.Reference assemblies should not be loaded for execution.They can only be loaded in the Reflection-only loader context. (Exception from HRESULT: 0x80131058)
	/// 2>   at System.Signature.GetSignature(Void* pCorSig, Int32 cCorSig, RuntimeFieldHandleInternal fieldHandle, IRuntimeMethodInfo methodHandle, RuntimeType declaringType)
	/// 2>   at System.Reflection.RuntimeMethodInfo.get_ReturnType()
	/// 2>   at Orleans.CodeGeneration.GrainInterfaceUtils.ValidateInterfaceMethods(Type type, List`1 violations)
	/// 2>   at Orleans.CodeGeneration.GrainInterfaceUtils.TryValidateInterfaceRules(Type type, List`1& violations)
	/// 2>   at Orleans.CodeGeneration.GrainInterfaceUtils.ValidateInterfaceRules(Type type)
	/// 2>   at Orleans.CodeGenerator.RoslynCodeGenerator.GenerateForAssemblies(List`1 assemblies, Boolean runtime)
	/// </summary>
	public struct Vector3 : IEquatable<Vector3>
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public System.Numerics.Vector3 AsNativeVector()
			=> new System.Numerics.Vector3(X, Y, Z);

		public bool Equals(Vector3 other)
			=> other.X == X && other.Y == Y && other.Z == Z;

		public override int GetHashCode()
			=> BitConverter.ToInt32(BitConverter.GetBytes(X * 3.0f + Y * 7.0f + Z * 11.0f), 0);

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			else if (obj.GetType().Equals(GetType()))
				return Equals((Vector3)obj);
			else
				return false;
		}

		public static bool operator ==(Vector3 left, Vector3 right)
			=> left.Equals(right);

		public static bool operator !=(Vector3 left, Vector3 right)
			=> !left.Equals(right);
	}
}
