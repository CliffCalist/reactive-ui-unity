using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace WhiteArrow.MVVM.UI
{
    public class TabMenu : UIView
    {
        [Serializable]
        private class ButtonTabPair
        {
            public UIView Tab;
            public TabButton Button;
        }



        [SerializeField, Min(0)] private int _currentTabIndex = 0;
        [SerializeField] private List<ButtonTabPair> _buttonTabMap;



        protected override void InitCore()
        {
            for (int i = 0; i < _buttonTabMap.Count; i++)
            {
                var index = i;
                var pair = _buttonTabMap[index];

                pair.Button.Clicked
                    .Subscribe(_ => pair.Tab.Show())
                    .AddTo(pair.Tab);

                pair.Tab.IsShowed
                    .Where(v => v)
                    .Subscribe(_ =>
                    {
                        _currentTabIndex = index;
                        UpdateMapStates();
                    })
                    .AddTo(this);
            }

            UpdateMapStates();
        }

        private void UpdateMapStates()
        {
            for (int i = 0; i < _buttonTabMap.Count; i++)
            {
                var pair = _buttonTabMap[i];
                pair.Button.SetActive(i == _currentTabIndex);

                if (i == _currentTabIndex)
                {
                    if (pair.Tab != null && !pair.Tab.IsShowed.CurrentValue)
                        pair.Tab.Show();
                }
                else
                {
                    if (pair.Tab != null && pair.Tab.IsShowed.CurrentValue)
                        pair.Tab.Hide();
                }
            }
        }



#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_buttonTabMap != null && _currentTabIndex > 0 && _currentTabIndex >= _buttonTabMap.Count)
            {
                _currentTabIndex = _buttonTabMap.Count - 1;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}