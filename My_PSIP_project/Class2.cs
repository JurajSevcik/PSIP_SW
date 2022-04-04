#region Assembly SharpPcap, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\jurse\Desktop\4_semester\PSIP\My_PSIP_project\packages\SharpPcap.6.1.0\lib\netstandard2.0\SharpPcap.dll
// Decompiled with ICSharpCode.Decompiler 6.1.0.5902
#endregion

using System;

namespace SharpPcap 
{
    public readonly ref struct PacketCapture_a
    {
        public ICaptureDevice Device
        {
            get;
        }

        public ICaptureHeader Header
        {
            get;
        }

        public ReadOnlySpan<byte> Data
        {
            get;
        }

        public RawCapture GetPacket()
        {
            return new RawCapture(Device, Header, Data);
        }

        /*public void PacketCapture(ICaptureDevice device, ICaptureHeader header, ReadOnlySpan<byte> data)
        {
            Header = header;
            Device = device;
            Data = data;
        }*/
    }

    public readonly ref struct PacketCapture_b
    {
        public ICaptureDevice Device
        {
            get;
        }

        public ICaptureHeader Header
        {
            get;
        }

        public ReadOnlySpan<byte> Data
        {
            get;
        }

        public RawCapture GetPacket()
        {
            return new RawCapture(Device, Header, Data);
        }

        public PacketCapture_b(ICaptureDevice device, ICaptureHeader header, ReadOnlySpan<byte> data)
        {
            Header = header;
            Device = device;
            Data = data;
        }
    }
}
#if false // Decompilation log
'29' items in cache
------------------
Resolve: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Facades\netstandard.dll'
------------------
Resolve: 'PacketDotNet, Version=1.3.0.0, Culture=neutral, PublicKeyToken=451414c7667b2a58'
Found single assembly: 'PacketDotNet, Version=1.3.0.0, Culture=neutral, PublicKeyToken=451414c7667b2a58'
Load from: 'C:\Users\jurse\Desktop\4_semester\PSIP\My_PSIP_project\packages\PacketDotNet.1.3.0\lib\net45\PacketDotNet.dll'
------------------
Resolve: 'System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Users\jurse\Desktop\4_semester\PSIP\My_PSIP_project\packages\System.Memory.4.5.4\lib\net461\System.Memory.dll'
------------------
Resolve: 'System.Text.Encoding.CodePages, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Text.Encoding.CodePages, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Users\jurse\Desktop\4_semester\PSIP\My_PSIP_project\packages\System.Text.Encoding.CodePages.5.0.0\lib\net461\System.Text.Encoding.CodePages.dll'
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll'
------------------
Resolve: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.dll'
------------------
Resolve: 'System.Diagnostics.Tracing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Diagnostics.Tracing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Drawing.dll'
------------------
Resolve: 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.IO.Compression.FileSystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.IO.Compression.FileSystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.ComponentModel.Composition, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.ComponentModel.Composition, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.Net.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Http, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '4.0.0.0', Got: '4.2.0.0'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Net.Http.dll'
------------------
Resolve: 'System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Numerics.dll'
------------------
Resolve: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Transactions.dll'
------------------
Resolve: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.dll'
------------------
Resolve: 'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.Linq.dll'
#endif
