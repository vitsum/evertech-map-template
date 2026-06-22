using System.Collections.Generic;
using UnityEngine;

namespace EvertechMapSDK
{
    /// <summary>
    /// Ready-made receiver for an Enum setting: activates one object per option. The list
    /// order matches the setting's options — the selected one is enabled, the rest disabled.
    /// </summary>
    [AddComponentMenu("Evertech/Receivers/Setting Enum Objects")]
    public class SettingEnumObjects : MonoBehaviour
    {
        public string key;
        [Tooltip("Index aligns with the Enum option order.")]
        public List<GameObject> optionObjects = new List<GameObject>();

        private void OnEnable()
        {
            MapSettings.OnChanged += HandleChanged;
            Apply();
        }

        private void OnDisable()
        {
            MapSettings.OnChanged -= HandleChanged;
        }

        private void HandleChanged(string changedKey)
        {
            if (changedKey == null || changedKey == key) Apply();
        }

        private void Apply()
        {
            if (MapSettings.GetDef(key) == null) return;

            int selected = MapSettings.GetOption(key);
            for (int i = 0; i < optionObjects.Count; i++)
                if (optionObjects[i] != null) optionObjects[i].SetActive(i == selected);
        }
    }
}
