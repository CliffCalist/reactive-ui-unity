using System.Collections.Generic;

namespace WhiteArrow.ReactiveUI
{
    public interface ISelectorOptionsSynchronizer<T>
        where T : SelectorOption
    {
        void SyncTo(List<T> options);
    }
}