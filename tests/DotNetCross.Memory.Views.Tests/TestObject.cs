namespace DotNetCross.Memory.Views.Tests
{
    public class TestObject
    {
        int i;
        string s;

        public TestObject(int i, string s)
        {
            this.i = i;
            this.s = s;
        }

        public ref int Int => ref i;
        public ref readonly int ReadOnlyInt => ref i;
        public ref string S => ref S;
    }
}
