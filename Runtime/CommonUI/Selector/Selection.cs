namespace WhiteArrow.ReactiveUI
{
    public class Selection<TData> : ISelection
    {
        public int Index { get; }
        public TData Item { get; }
        object ISelection.UntypedItem => Item;



        public Selection(int index, TData item)
        {
            Index = index;
            Item = item;
        }
    }
}