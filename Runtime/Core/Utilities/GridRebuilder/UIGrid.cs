using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public sealed class UIGrid<TElement> : IEnumerable<UIGridGroup<TElement>>
        where TElement : Component
    {
        public readonly List<UIGridGroup<TElement>> Groups = new();



        public int GroupsCount => Groups.Count;

        public UIGridGroup<TElement> this[Index index]
        {
            get => Groups[index];
            set => Groups[index] = value;
        }



        public void AddGroup(UIGridGroup<TElement> group)
        {
            Groups.Add(group);
        }

        public void RemoveGroup(UIGridGroup<TElement> group)
        {
            Groups.Remove(group);
        }



        public IEnumerator<UIGridGroup<TElement>> GetEnumerator()
        {
            return Groups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}