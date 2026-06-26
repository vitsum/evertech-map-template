using UnityEngine;

/// <summary>
/// Custom-map marker (same idea as <see cref="WaterLevel"/>): drop this component on an
/// empty GameObject in a custom scene and the player will spawn at its position/rotation
/// on a fresh start. Discovered via FindObjectOfType in GameInitializer.OnMapLoaded.
/// Ignored when loading a save that already has a stored player position
/// (UseDefaultSpawn == false), so existing builds keep their spot.
/// The object's forward (blue Z axis) is the direction the player faces.
/// GLOBAL namespace (no namespace) — must match the game's type identity so the map
/// AssetBundle binds this component in obfuscated release builds.
/// </summary>
public class PlayerSpawnPoint : MonoBehaviour
{
#if UNITY_EDITOR
    // Visual aid for map authors in the editor (does nothing at runtime / in build).
    private void OnDrawGizmos()
    {
        var p = transform.position;
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.9f);
        Gizmos.DrawWireSphere(p + Vector3.up * 0.9f, 0.4f);          // head
        Gizmos.DrawLine(p, p + Vector3.up * 1.8f);                   // body
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(p + Vector3.up * 0.9f, transform.forward * 1.5f); // facing
    }
#endif
}
