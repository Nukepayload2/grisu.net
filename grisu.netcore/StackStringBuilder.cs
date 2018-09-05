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

        public void Append(string buf)
        {
            for (int i = 0; i < buf.Length; i++)
            {
                _backBuffer[Index++] = buf[i];
            }
        }

        public void Append(Span<char> buf, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _backBuffer[Index++] = buf[i];
            }
        }

        public unsafe void AppendNumber0To999(int value)
        {
            Debug.Assert(value >= 0 && value <= 999);
            char* begin = stackalloc char[3];
            char* end = begin + 2;
            char* cur = end;
            while (value >= 10)
            {
                int a = value;
                value /= 10;
                int b = value * 10;
                int digit = a - b;
                *cur-- = (char)(48 + digit);
            }
            *cur = (char)(48 + value);
            while (cur <= end)
            {
                _backBuffer[Index++] = *cur++;
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
