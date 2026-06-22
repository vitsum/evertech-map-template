using System.Collections.Generic;
using UnityEngine;

namespace EvertechMapSDK
{
    /// <summary>
    /// Drop ONE of these into a custom-map scene and fill the list in the Inspector to
    /// publish settings into the game's World Settings panel. No scripting required.
    ///
    /// Registers itself with <see cref="MapSettings"/> on Awake so the panel can find it
    /// after the map's AssetBundle scene finishes loading.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Evertech/Map Settings Declaration")]
    public class MapSettingsDeclaration : MonoBehaviour
    {
        public List<MapSettingDef> settings = new List<MapSettingDef>();

        private void Awake()
        {
            MapSettings.SetCurrent(this);
        }

        private void OnDestroy()
        {
            MapSettings.Clear(this);
        }
    }
}
