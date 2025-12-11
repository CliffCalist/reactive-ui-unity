namespace WhiteArrow.ReactiveUI
{
    public class Selection<TData>
    {
        public int Index { get; }
        public TData Item { get; }



        public Selection(int index, TData item)
        {
            Index = index;
            Item = item;
        }
    }
}