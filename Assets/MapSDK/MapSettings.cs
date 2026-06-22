using System;
using UnityEngine;

namespace EvertechMapSDK
{
    /// <summary>
    /// Runtime bridge between a custom map's declared settings and the game.
    ///
    /// The SDK lives in its own assembly so the exact same types compile into both the
    /// game and the map-author template (that's what lets a MonoBehaviour serialized in a
    /// map AssetBundle resolve against the game at load time). Because of that it must NOT
    /// reference any game code. Persistence is therefore injected by the game at startup
    /// through the *Getter/*Setter hooks; until then it falls back to PlayerPrefs so the
    /// template and standalone tests still work.
    ///
    /// Values are stored globally on the profile, namespaced by map id, so each map
    /// remembers its own settings.
    /// </summary>
    public static class MapSettings
    {
        // --- Persistence hooks (game overrides these to route into its profile store) ---
        public static Func<string, bool, bool> BoolGetter = (k, d) => PlayerPrefs.GetInt(k, d ? 1 : 0) == 1;
        public static Action<string, bool> BoolSetter = (k, v) => PlayerPrefs.SetInt(k, v ? 1 : 0);
        public static Func<string, float, float> FloatGetter = (k, d) => PlayerPrefs.GetFloat(k, d);
        public static Action<string, float> FloatSetter = (k, v) => PlayerPrefs.SetFloat(k, v);
        public static Func<string, int, int> IntGetter = (k, d) => PlayerPrefs.GetInt(k, d);
        public static Action<string, int> IntSetter = (k, v) => PlayerPrefs.SetInt(k, v);

        /// <summary>Set by the game before a map scene loads; namespaces stored keys.</summary>
        public static string CurrentMapId;

        /// <summary>The active map's declaration, or null when the current map has none.</summary>
        public static MapSettingsDeclaration Current { get; private set; }

        /// <summary>Fires with a setting key when its value changes; null key = panel should refresh wholesale (map (un)loaded).</summary>
        public static event Action<string> OnChanged;

        public static void SetCurrent(MapSettingsDeclaration declaration)
        {
            Current = declaration;
            OnChanged?.Invoke(null);
        }

        public static void Clear(MapSettingsDeclaration declaration)
        {
            if (Current == declaration)
            {
                Current = null;
                OnChanged?.Invoke(null);
            }
        }

        public static MapSettingDef GetDef(string key)
        {
            if (Current == null || Current.settings == null) return null;
            foreach (var def in Current.settings)
                if (def != null && def.key == key) return def;
            return null;
        }

        private static string StorageKey(string key) => "mapset." + (CurrentMapId ?? "global") + "." + key;

        // --- Reads (fall back to the declared default, clamped to the def) ---
        public static bool GetBool(string key)
        {
            var def = GetDef(key);
            return BoolGetter(StorageKey(key), def != null && def.defaultBool);
        }

        public static float GetFloat(string key)
        {
            var def = GetDef(key);
            float value = FloatGetter(StorageKey(key), def != null ? def.defaultFloat : 0f);
            return def != null ? def.ClampFloat(value) : value;
        }

        public static int GetOption(string key)
        {
            var def = GetDef(key);
            int value = IntGetter(StorageKey(key), def != null ? def.defaultOption : 0);
            return def != null ? def.ClampOption(value) : value;
        }

        // --- Writes (persist, then notify) ---
        public static void SetBool(string key, bool value)
        {
            BoolSetter(StorageKey(key), value);
            OnChanged?.Invoke(key);
        }

        public static void SetFloat(string key, float value)
        {
            var def = GetDef(key);
            if (def != null) value = def.ClampFloat(value);
            FloatSetter(StorageKey(key), value);
            OnChanged?.Invoke(key);
        }

        public static void SetOption(string key, int value)
        {
            var def = GetDef(key);
            if (def != null) value = def.ClampOption(value);
            IntSetter(StorageKey(key), value);
            OnChanged?.Invoke(key);
        }
    }
}
