using System.Collections.Generic;
using UnityEngine;

namespace EvertechMapSDK
{
    /// <summary>
    /// Ready-made receiver that swaps materials on renderers — the custom-map equivalent of
    /// BigWorld's water/skybox quality switch. Bool: index 0 = off, 1 = on. Enum: index = option.
    /// </summary>
    [AddComponentMenu("Evertech/Receivers/Setting Swap Material")]
    public class SettingSwapMaterial : MonoBehaviour
    {
        public string key;
        public List<Renderer> renderers = new List<Renderer>();
        [Tooltip("Bool: [0]=off, [1]=on. Enum: index = selected option.")]
        public List<Material> materials = new List<Material>();

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
            var def = MapSettings.GetDef(key);
            if (def == null || materials.Count == 0) return;

            int idx;
            if (def.type == MapSettingType.Bool) idx = MapSettings.GetBool(key) ? 1 : 0;
            else if (def.type == MapSettingType.Enum) idx = MapSettings.GetOption(key);
            else return;

            idx = Mathf.Clamp(idx, 0, materials.Count - 1);
            foreach (var r in renderers)
                if (r != null) r.sharedMaterial = materials[idx];
        }
    }
}
