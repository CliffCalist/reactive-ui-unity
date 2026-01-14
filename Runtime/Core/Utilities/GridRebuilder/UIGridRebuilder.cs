using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace WhiteArrow.ReactiveUI
{
    internal class UIGridRebuilder<TData, TElement>
        where TElement : Component
    {
        private readonly IList<TData> _dataSource;
        private readonly UIGrid<TElement> _grid;
        private readonly UIGridRebuilderConfig<TData, TElement> _config;
        private readonly int _requiredGroupCount;


        private Func<Transform> _createGroup;
        private Action<Transform> _destroyGroup;

        private Func<TElement> _createElement;
        private Action<TElement> _destroyElement;

        private Func<Transform> _createPlaceholder;
        private Action<Transform> _destroyPlaceholder;





        private int _totalElementsCount => _dataSource.Count;



        #region Constructor & Init
        public UIGridRebuilder(IList<TData> dataSource, UIGrid<TElement> grid, UIGridRebuilderConfig<TData, TElement> config)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            if (config.ElementsPerGroup <= 0)
                throw new InvalidOperationException($"{nameof(config.ElementsPerGroup)} must be greater than zero.");

            ResolveFactories();
            _requiredGroupCount =
                (_totalElementsCount + _config.ElementsPerGroup - 1) / _config.ElementsPerGroup;
        }

        private void ResolveFactories()
        {
            ResolveGroupFactories();
            ResolveElementFactories();
            ResolvePlaceholdersFactories();
        }

        private void ResolveGroupFactories()
        {
            if (_config.CreateGroup != null)
                _createGroup = _config.CreateGroup;
            else
            {
                _createGroup = () =>
                {
                    if (_config.GroupPrefab == null)
                    {
                        throw new InvalidOperationException(
                            $"{_config.GetType().Name} requires either {nameof(_config.CreateGroup)} callback or {nameof(_config.GroupPrefab)}."
                        );
                    }

                    if (_config.GroupsRoot == null)
                    {
                        throw new InvalidOperationException(
                            $"{_config.GetType().Name} requires either {nameof(_config.CreateGroup)} callback or {nameof(_config.GroupsRoot)}."
                        );
                    }

                    return Object.Instantiate(_config.GroupPrefab, _config.GroupsRoot);
                };
            }

            _destroyGroup = _config.DestroyGroup ?? (g => Object.Destroy(g.gameObject));
        }

        private void ResolveElementFactories()
        {
            if (_config.CreateElement != null)
                _createElement = _config.CreateElement;
            else
            {
                _createElement = () =>
                {
                    if (_config.ElementPrefab == null)
                        throw new InvalidOperationException($"{_config.GetType().Name} requires either {nameof(_config.CreateElement)} callback or {nameof(_config.ElementPrefab)}.");

                    return Object.Instantiate(_config.ElementPrefab);
                };
            }

            _destroyElement = _config.DestroyElement ?? (e => Object.Destroy(e.gameObject));
        }

        private void ResolvePlaceholdersFactories()
        {
            if (_config.CreatePlaceholder != null)
                _createPlaceholder = _config.CreatePlaceholder;
            else
            {
                _createPlaceholder = () =>
                {
                    if (_config.PlaceholderPrefab == null)
                    {
                        var go = new GameObject("Placeholder", typeof(RectTransform));
                        return go.GetComponent<RectTransform>();
                    }
                    else return Object.Instantiate(_config.PlaceholderPrefab);
                };
            }

            _destroyPlaceholder = _config.DestroyPlaceholder ?? (p => Object.Destroy(p.gameObject));
        }
        #endregion



        #region Build
        public void Rebuild()
        {
            SyncGroups();
            SyncElements();

            Observable.NextFrame()
                .ObserveOnMainThread()
                .Subscribe(_ => LayoutRebuilder.MarkLayoutForRebuild(_config.GroupsRoot as RectTransform));
        }

        private void SyncGroups()
        {
            while (_grid.GroupsCount > _requiredGroupCount)
            {
                var last = _grid[^1];
                _grid.RemoveGroup(last);
                _destroyGroup(last.Root);
            }

            while (_grid.GroupsCount < _requiredGroupCount)
            {
                var root = _createGroup();
                _grid.AddGroup(new UIGridGroup<TElement>(root));
            }
        }

        private void SyncElements()
        {
            var dataIndex = 0;

            for (int g = 0; g < _grid.GroupsCount; g++)
            {
                var group = _grid[g];

                var elementsInGroup =
                    Math.Min(_config.ElementsPerGroup, _totalElementsCount - dataIndex);

                while (group.Elements.Count > elementsInGroup)
                {
                    var last = group.Elements[^1];
                    group.Elements.RemoveAt(group.Elements.Count - 1);
                    _destroyElement(last);
                }

                while (group.Elements.Count < elementsInGroup)
                {
                    var element = _createElement();
                    element.transform.SetParent(group.Root, false);
                    group.Elements.Add(element);
                }

                for (int i = 0; i < group.Elements.Count; i++)
                {
                    var element = group.Elements[i];

                    if (_config.BindElement != null)
                    {
                        _config.BindElement(
                            element,
                            dataIndex,
                            _dataSource[dataIndex]
                        );
                    }

                    dataIndex++;
                }

                SyncPlaceholders(group);
                SetElementsPlaceholdersOrder(group);
            }
        }

        private void SyncPlaceholders(UIGridGroup<TElement> group)
        {
            var requiredPlaceholders =
                _config.ElementsPerGroup - group.Elements.Count;

            while (group.Placeholders.Count > requiredPlaceholders)
            {
                var last = group.Placeholders[^1];
                group.Placeholders.RemoveAt(group.Placeholders.Count - 1);
                _destroyPlaceholder(last);
            }

            while (group.Placeholders.Count < requiredPlaceholders)
            {
                var placeholder = _createPlaceholder();
                placeholder.SetParent(group.Root, false);
                group.Placeholders.Add(placeholder);
            }
        }

        private void SetElementsPlaceholdersOrder(UIGridGroup<TElement> group)
        {
            var sibling = 0;

            for (int i = 0; i < group.Elements.Count; i++)
            {
                group.Elements[i].transform.SetSiblingIndex(sibling++);
            }

            for (int i = 0; i < group.Placeholders.Count; i++)
            {
                group.Placeholders[i].SetSiblingIndex(sibling++);
            }
        }
        #endregion
    }
}