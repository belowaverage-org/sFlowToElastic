using System;

namespace BelowAverage
{
    static class Converter
    {
        public static uint ToUInt(this byte[] buffer, int start, int length)
        {
            Span<byte> snip = buffer.AsSpan(start, length);
            if (BitConverter.IsLittleEndian) snip.Reverse();
            return BitConverter.ToUInt32(snip);
        }
        public static ulong ToULong(this byte[] buffer, int start, int length)
        {
            Span<byte> snip = buffer.AsSpan(start, length);
            if (BitConverter.IsLittleEndian) snip.Reverse();
            return BitConverter.ToUInt64(snip);
        }
        public static ushort ToUShort(this byte[] buffer, int start, int length)
        {
            Span<byte> snip = buffer.AsSpan(start, length);
            if (BitConverter.IsLittleEndian) snip.Reverse();
            return BitConverter.ToUInt16(snip);
        }
    }
}