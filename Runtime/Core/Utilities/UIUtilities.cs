using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public static class UIUtilities
    {
        public static void RebuildList<TData, TElement>(
            IList<TData> dataSource,
            IList<TElement> elementsSource,
            UIListRebuilderConfig<TData, TElement> config)
            where TElement : Component
        {
            var rebuilder = new UIListRebuilder<TData, TElement>(dataSource, elementsSource, config);
            rebuilder.Rebuild();
        }



        public static void RebuildGrid<TData, TElement>(
            IList<TData> dataSource,
            UIGrid<TElement> grid,
            UIGridRebuilderConfig<TData, TElement> config)
            where TElement : Component
        {
            var rebuilder = new UIGridRebuilder<TData, TElement>(dataSource, grid, config);
            rebuilder.Rebuild();
        }
    }
}