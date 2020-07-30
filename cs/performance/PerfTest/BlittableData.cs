﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using FASTER.core;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace FASTER.PerfTest
{
    public struct BlittableData
    {
        // Modified by RMW
        internal uint Modified;

        // This is key.Value which we know must fit in an int as we allocate an array of sequential Keys
        internal int Value;

        public static int SizeOf = sizeof(uint) + sizeof(int);

        internal void Read(BinaryReader reader)
        {
            this.Modified = reader.ReadUInt32();
            this.Value = reader.ReadInt32();
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(this.Modified);
            writer.Write(this.Value);
        }

        private readonly long LongValue => (long)this.Modified << 32 | (uint)this.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(BlittableData other) => this.LongValue == other.LongValue;

        public readonly override bool Equals(object other) => other is BlittableData bd && this.Equals(bd);

        public readonly override int GetHashCode() => HashCode.Combine(Modified, Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long GetHashCode64() => Utility.GetHashCode(this.LongValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BlittableData lhs, BlittableData rhs) => lhs.Equals(rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BlittableData lhs, BlittableData rhs) => !lhs.Equals(rhs);

        public override string ToString() => $"mod {Modified}, val {Value}";
    }
}
