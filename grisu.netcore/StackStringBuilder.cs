using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GrisuDotNet
{
    internal ref struct StackStringBuilder
    {
        private readonly Span<char> _backBuffer;
        public int Index;

        public StackStringBuilder(Span<char> buf)
        {
            _backBuffer = buf;
            Index = 0;
        }

        public void Append(char ch)
        {
            _backBuffer[Index++] = ch;
        }

        public void Append(char ch, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _backBuffer[Index++] = ch;
            }
        }

        public void Append(Span<char> buf, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _backBuffer[Index++] = buf[i];
            }
        }

        public void Append(Span<char> buf, int startIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _backBuffer[Index++] = buf[startIndex + i];
            }
        }

        // Devirtualization is required.
        public unsafe new string ToString()
        {
            return new string((char*)Unsafe.AsPointer(ref _backBuffer.GetPinnableReference()));
        }
    }
}
