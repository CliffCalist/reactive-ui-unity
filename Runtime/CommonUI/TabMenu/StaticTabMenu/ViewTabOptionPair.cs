using System;

namespace WhiteArrow.ReactiveUI
{
    [Serializable]
    public class ViewTabOptionPair<T>
        where T : SelectorOption
    {
        public T Option;
        public UIView UI;
    }
}