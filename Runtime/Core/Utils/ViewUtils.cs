using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Core
{
    public static class ViewUtils
    {
        public static void RebuildList<TData, TElement>(
            IList<TData> dataSource,
            IList<TElement> elementsSource,
            ListRebuilderConfig<TData, TElement> config)
            where TElement : Component
        {
            var rebuilder = new ListRebuilder<TData, TElement>(dataSource, elementsSource, config);
            rebuilder.Rebuild();
        }

        public static void RebuildGrid<TData, TElement>(
            IList<TData> dataSource,
            Grid<TElement> grid,
            GridRebuilderConfig<TData, TElement> config)
            where TElement : Component
        {
            var rebuilder = new GridRebuilder<TData, TElement>(dataSource, grid, config);
            rebuilder.Rebuild();
        }



        public static ITransitionHandler Switch(UIView from, UIView to)
        {
            return TransitionCoordinator.Switch(from, to, TransitionConfig.Default);
        }

        public static ITransitionHandler Switch(UIView from, UIView to, TransitionConfig config)
        {
            return TransitionCoordinator.Switch(from, to, config);
        }
    }
}