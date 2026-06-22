using System.Collections.Generic;
using UnityEngine;

namespace EvertechMapSDK
{
    /// <summary>
    /// Ready-made receiver for a Bool setting: shows/hides objects. Add it, set the key to
    /// a Bool setting, drag the targets — no UnityEvent wiring needed.
    /// </summary>
    [AddComponentMenu("Evertech/Receivers/Setting Toggle Object")]
    public class SettingToggleObject : MonoBehaviour
    {
        public string key;
        public List<GameObject> targets = new List<GameObject>();
        [Tooltip("Invert: targets are active when the setting is OFF.")]
        public bool invert;

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

            bool on = MapSettings.GetBool(key);
            if (invert) on = !on;

            foreach (var target in targets)
                if (target != null) target.SetActive(on);
        }
    }
}
