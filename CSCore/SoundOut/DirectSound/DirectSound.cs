﻿using System;
using System.Runtime.InteropServices;
using System.Security;

namespace CSCore.SoundOut.DirectSound
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("279AFA83-4981-11CE-A521-0020AF0BE560")]
    public unsafe class DirectSoundBase : ComObject
    {
        public DirectSoundBase(IntPtr directSound)
        {
            if (directSound == IntPtr.Zero) throw new ArgumentException("Invalid pointer to a IDirectSound Interface");
            _basePtr = directSound.ToPointer();
        }

        public bool SupportsFormat(WaveFormat format)
        {
            DirectSoundCapabilities caps;
            DirectSoundException.Try(GetCaps(out caps), "IDirectSound", "GetCaps");
            bool result = true;
            if (format.Channels == 2)
                result &= caps.Flags.HasFlag(DSCapabilitiesFlags.SecondaryBufferStereo);
            else if (format.Channels == 1)
                result &= caps.Flags.HasFlag(DSCapabilitiesFlags.SecondaryBufferMono);
            else result &= false;

            if (format.BitsPerSample == 8)
                result &= caps.Flags.HasFlag(DSCapabilitiesFlags.SecondaryBuffer8Bit);
            else if (format.BitsPerSample == 16)
                result &= caps.Flags.HasFlag(DSCapabilitiesFlags.SecondaryBuffer16Bit);
            else result &= false;

            result &= format.WaveFormatTag == AudioEncoding.Pcm;
            return result;
        }

        public DSResult SetCooperativeLevel(IntPtr hWnd, DSCooperativeLevelType level)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, hWnd.ToPointer(), unchecked((int)level), ((void**)(*(void**)_basePtr))[6]);
        }

        public DSResult CreateSoundBuffer(DSBufferDescription bufferDesc, out IntPtr soundBuffer, IntPtr pUnkOuter)
        {
            fixed (void* ptrsoundbuffer = &soundBuffer)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, &bufferDesc, ptrsoundbuffer, (void*)pUnkOuter, ((void**)(*(void**)_basePtr))[3]);
            }
        }

        public DSResult GetCaps(out DirectSoundCapabilities caps)
        {
            DirectSoundCapabilities tmp = new DirectSoundCapabilities();
            tmp.Size = Marshal.SizeOf(tmp);
            var result = InteropCalls.CalliMethodPtr(_basePtr, &tmp, ((void**)(*(void**)_basePtr))[4]);
            caps = tmp;
            return result;
        }

        public DSResult DuplicateSoundBuffer<T>(T bufferOriginal, out T duplicatedBuffer) where T : DirectSoundBufferBase
        {
            IntPtr resultPtr;
            var result = InteropCalls.CalliMethodPtr(_basePtr, bufferOriginal.BasePtr.ToPointer(), &resultPtr, ((void**)(*(void**)_basePtr))[5]);
            duplicatedBuffer = (T)Activator.CreateInstance(typeof(T), resultPtr);
            return result;
        }

        public DSResult Compact()
        {
            return InteropCalls.CalliMethodPtr(_basePtr, ((void**)(*(void**)_basePtr))[7]);
        }

        public DSResult GetSpeakerConfig(out int speakerConfig)
        {
            fixed (void* pspeakerConfig = &speakerConfig)
            {
                return InteropCalls.CalliMethodPtr(_basePtr, pspeakerConfig, ((void**)(*(void**)_basePtr))[8]);
            }
        }

        public DSResult SetSpeakerConfig(int speakerConfig)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, speakerConfig, ((void**)(*(void**)_basePtr))[9]);
        }

        public DSResult Initialize(Guid device)
        {
            return InteropCalls.CalliMethodPtr(_basePtr, &device, ((void**)(*(void**)_basePtr))[10]);
        }
    }
}