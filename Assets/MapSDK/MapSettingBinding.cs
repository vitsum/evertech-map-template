using System;
using UnityEngine;
using UnityEngine.Events;

namespace EvertechMapSDK
{
    [Serializable] public class BoolEvent : UnityEvent<bool> { }
    [Serializable] public class FloatEvent : UnityEvent<float> { }
    [Serializable] public class IntEvent : UnityEvent<int> { }

    /// <summary>
    /// Reacts to a declared setting without any author code. Add it to an object, point
    /// <see cref="key"/> at a setting from the map's <see cref="MapSettingsDeclaration"/>,
    /// and wire the matching UnityEvent in the Inspector to anything built-in
    /// (GameObject.SetActive, AudioSource.volume, a material property, a spawner speed...).
    ///
    /// The event fires once on load with the current value and again on every change, so
    /// authors don't manage initial state themselves.
    /// </summary>
    [AddComponentMenu("Evertech/Map Setting Binding")]
    public class MapSettingBinding : MonoBehaviour
    {
        [Tooltip("Key of a setting declared in this map's MapSettingsDeclaration.")]
        public string key;

        public BoolEvent onBool;
        public FloatEvent onFloat;
        public IntEvent onOption;

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
            // null = map (un)loaded / wholesale refresh; otherwise only react to our key.
            if (changedKey == null || changedKey == key) Apply();
        }

        private void Apply()
        {
            var def = MapSettings.GetDef(key);
            if (def == null) return;

            switch (def.type)
            {
                case MapSettingType.Bool:
                    onBool?.Invoke(MapSettings.GetBool(key));
                    break;
                case MapSettingType.Slider:
                    onFloat?.Invoke(MapSettings.GetFloat(key));
                    break;
                case MapSettingType.Enum:
                    onOption?.Invoke(MapSettings.GetOption(key));
                    break;
            }
        }
    }
}
