using UnityEngine;

namespace EvertechMods
{
    // Custom-map marker: drop on a GameObject; the game reads its world Y as the water level
    // (GameInitializer.FindObjectOfType<WaterLevel>). In EvertechMods so obfuscation keeps the
    // type name stable for AssetBundle maps.
    public class WaterLevel : MonoBehaviour
    {
    }
}
