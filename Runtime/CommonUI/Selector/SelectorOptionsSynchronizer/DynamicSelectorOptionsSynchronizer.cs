using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    [Serializable]
    public class DynamicSelectorOptionsSynchronizer<T> : ISelectorOptionsSynchronizer<T>
        where T : SelectorOption
    {
        [SerializeField] private Transform _content;
        [SerializeField] private T _optionPrefab;
        [SerializeField, Min(0)] private int _targetCount;


        private Func<T, int, T> _factory;


        private readonly Subject<T> _optionPreDestroyed = new();
        public Observable<T> preDestroy => _optionPreDestroyed;



        public void SetTargetCount(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            _targetCount = count;
        }

        public void SetFactory(Func<T, int, T> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }



        public void SyncTo(List<T> options)
        {
            while (options.Count < _targetCount)
            {
                var newOption = CreateOption(options.Count);
                newOption.transform.SetParent(_content, false);
                options.Add(newOption);
            }

            while (options.Count > _targetCount)
            {
                var last = options.LastOrDefault();
                if (last == null)
                    break;

                options.Remove(last);
                _optionPreDestroyed.OnNext(last);
                UnityEngine.Object.Destroy(last.gameObject);
            }
        }

        private T CreateOption(int index)
        {
            return _factory != null
                ? _factory(_optionPrefab, index)
                : UnityEngine.Object.Instantiate(_optionPrefab, _content);
        }
    }
}