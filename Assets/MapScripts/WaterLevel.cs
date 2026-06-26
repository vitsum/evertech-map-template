using UnityEngine;

namespace EvertechMods
{
    // Custom-map marker: drop on a GameObject; the game reads its world Y as the water level
    // (GameInitializer.FindObjectOfType<WaterLevel>). In EvertechMods so obfuscation keeps the
    // type name stable for AssetBundle maps.
    //
    // The appearance fields below are OPT-IN. Old maps (built before these existed) deserialize
    // them as their C# defaults (overrideAppearance == false), which means the game keeps its
    // built-in underwater look exactly as before. Set overrideAppearance = true to customize.
    public class WaterLevel : MonoBehaviour
    {
        [Tooltip("Customize the underwater screen effect for this map. OFF = the game's default look (unchanged).")]
        public bool overrideAppearance = false;

        [Tooltip("Underwater screen overlay color (RGB). Alpha controls how strong/opaque the tint is.")]
        public Color tintColor = new Color(0.31772876f, 0.5442119f, 0.6603774f, 0.59607846f);

        [Tooltip("Play the underwater ambience sound when submerged.")]
        public bool playSound = true;
    }
}
