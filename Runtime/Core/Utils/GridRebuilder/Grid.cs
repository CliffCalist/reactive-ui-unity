using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Core
{
    public sealed class Grid<TElement> : IEnumerable<GridGroup<TElement>>
        where TElement : Component
    {
        public readonly List<GridGroup<TElement>> Groups = new();



        public int GroupsCount => Groups.Count;

        public GridGroup<TElement> this[Index index]
        {
            get => Groups[index];
            set => Groups[index] = value;
        }



        public void AddGroup(GridGroup<TElement> group)
        {
            Groups.Add(group);
        }

        public void RemoveGroup(GridGroup<TElement> group)
        {
            Groups.Remove(group);
        }



        public IEnumerator<GridGroup<TElement>> GetEnumerator()
        {
            return Groups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}