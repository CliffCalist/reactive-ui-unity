using System;
using System.Collections.Generic;

namespace WhiteArrow.ReactiveUI
{
    public static class UIUtilities
    {
        public static void RebuildList<TData, TUIElement>(
            IList<TData> source,
            IList<TUIElement> uiSource,
            UIListRebuildConfig<TData, TUIElement> config)
            where TUIElement : UnityEngine.Component
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            // CREATE
            var create = config.Create;
            if (create == null)
            {
                if (config.Prefab == null)
                    throw new InvalidOperationException(
                        "UIListRebuildConfig requires either Create callback or Prefab.");

                create = () => UnityEngine.Object.Instantiate(config.Prefab);
            }

            // DESTROY
            Action<TUIElement> destroy =
                config.Destroy ?? (elem => UnityEngine.Object.Destroy(elem.gameObject));

            // ===== REMOVE EXTRA =====
            while (uiSource.Count > source.Count)
            {
                var last = uiSource[^1];
                uiSource.RemoveAt(uiSource.Count - 1);
                destroy(last);
            }

            // ===== ADD MISSING =====
            while (uiSource.Count < source.Count)
                uiSource.Add(create());

            // ===== BIND (optional) =====
            if (config.Bind != null)
            {
                for (int i = 0; i < source.Count; i++)
                    config.Bind(uiSource[i], i, source[i]);
            }
        }
    }
}