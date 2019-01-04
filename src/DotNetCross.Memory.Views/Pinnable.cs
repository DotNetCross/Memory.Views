using System.Runtime.InteropServices;

namespace DotNetCross.Memory.Views
{
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class Pinnable<T>
    {
        public T Data;
    }
}
