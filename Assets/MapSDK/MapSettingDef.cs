using System;
using UnityEngine;

namespace EvertechMapSDK
{
    /// <summary>
    /// Kinds of settings a custom map can expose in the in-game World Settings panel.
    /// Keep this list append-only: bundles built against an older SDK store the enum
    /// value by index, so reordering or removing entries breaks already-published maps.
    /// </summary>
    public enum MapSettingType
    {
        Bool = 0,   // rendered as a Toggle
        Slider = 1, // rendered as a Slider (min/max/step)
        Enum = 2,   // rendered as a Dropdown (options)
    }

    /// <summary>
    /// One author-defined setting. Plain serializable data — filled in the Inspector by
    /// map authors, read by the game to build the panel and resolve defaults/clamping.
    /// All fields are public and append-only for the same bundle-compatibility reason as
    /// <see cref="MapSettingType"/>.
    /// </summary>
    [Serializable]
    public class MapSettingDef
    {
        [Tooltip("Unique key within this map. Used to store the value and to bind it.")]
        public string key;

        [Tooltip("Label shown next to the control in the settings panel.")]
        public string label;

        public MapSettingType type = MapSettingType.Bool;

        [Header("Bool")]
        public bool defaultBool;

        [Header("Slider")]
        public float min = 0f;
        public float max = 1f;
        public float step = 0f;        // 0 = continuous
        public float defaultFloat;

        [Header("Enum")]
        public string[] options;
        public int defaultOption;

        public float ClampFloat(float v)
        {
            v = Mathf.Clamp(v, min, max);
            if (step > 0f) v = min + Mathf.Round((v - min) / step) * step;
            return v;
        }

        public int ClampOption(int v)
        {
            int count = options != null ? options.Length : 0;
            if (count == 0) return 0;
            return Mathf.Clamp(v, 0, count - 1);
        }
    }
}
