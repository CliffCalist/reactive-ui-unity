using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WhiteArrow.ReactiveUI
{
    internal class UIListRebuilder<TData, TElement>
        where TElement : Component
    {
        private readonly IList<TData> _dataSource;
        private readonly IList<TElement> _elementsSource;
        private readonly UIListRebuilderConfig<TData, TElement> _config;

        private readonly Func<TElement> _createElement;
        private readonly Action<TElement> _destroyElement;



        public UIListRebuilder(IList<TData> dataSource, IList<TElement> elementsSource, UIListRebuilderConfig<TData, TElement> config)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _elementsSource = elementsSource ?? throw new ArgumentNullException(nameof(elementsSource));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            if (_config.Create != null)
                _createElement = _config.Create;
            else
            {
                _createElement = () =>
                {
                    if (config.Prefab == null)
                    {
                        throw new InvalidOperationException(
                            $"{config.GetType().Name} requires either {nameof(config.Create)} callback or {nameof(config.Prefab)}.");
                    }

                    if (config.Parent == null)
                    {
                        throw new InvalidOperationException(
                            $"{config.GetType().Name} requires either {nameof(config.Create)} callback or {nameof(config.Parent)}.");
                    }

                    return Object.Instantiate(config.Prefab, config.Parent);
                };
            }

            if (_config.Destroy != null)
                _destroyElement = _config.Destroy;
            else
                _destroyElement = elem => Object.Destroy(elem.gameObject);
        }



        public void Rebuild()
        {
            RemoveExtraElements();
            AddMissingElements();
            BindElements();
        }

        private void RemoveExtraElements()
        {
            while (_elementsSource.Count > _dataSource.Count)
            {
                var last = _elementsSource[^1];
                _elementsSource.Remove(last);
                _destroyElement(last);
            }
        }

        private void AddMissingElements()
        {
            while (_elementsSource.Count < _dataSource.Count)
                _elementsSource.Add(_createElement());
        }

        private void BindElements()
        {
            if (_config.Bind == null)
                return;

            for (int i = 0; i < _dataSource.Count; i++)
                _config.Bind(_elementsSource[i], i, _dataSource[i]);
        }
    }
}