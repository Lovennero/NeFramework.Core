namespace Framework.Core.FrameVariable
{
    public sealed class FVariable<T> : IFVariable
    {
        public T Value { get; set; }

        public void Clear()
        {
            Value = default;
        }
    }
}
