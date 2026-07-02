using UnityEngine;

/// <summary>
/// Custom-map marker (same idea as <see cref="WaterLevel"/> / <see cref="PlayerSpawnPoint"/>):
/// an INVISIBLE trigger volume placed in FRONT of a VERTICAL ladder. It is NOT the ladder and
/// NOT its collider — it is only the "grab" zone: when the player steps into this box the game
/// attaches them to the ladder so they climb (look up/down to move). On map load the game marks
/// this box as a trigger, so it never blocks or is visible.
///
/// Placement tips for map authors:
///  - The ladder itself STILL needs its own SOLID collider — one flat wall-like collider over
///    the whole ladder (NOT one per rung), so the climbing player presses against it instead of
///    falling through. This LadderVolume does not provide that; it is trigger-only.
///  - Put this box just in front of that solid ladder surface, sized to the reachable climb area.
///    Overlapping the ladder slightly is fine — the box is what the player walks into to grab on.
///  - Keep the object upright — the player climbs along the object's local UP (green Y) axis.
///
/// GLOBAL namespace (no namespace) — must match the game's type identity so the map
/// AssetBundle binds this component in obfuscated release builds. Do not rename or move it.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class LadderVolume : MonoBehaviour
{
#if UNITY_EDITOR
    // Make the grab box a trigger by default when the component is added in the editor,
    // so it never physically blocks the player (the game also enforces this at load).
    private void Reset()
    {
        var box = GetComponent<BoxCollider>();
        if (box != null) box.isTrigger = true;
    }

    // Visual aid for map authors in the editor (does nothing at runtime / in build).
    private void OnDrawGizmos()
    {
        var box = GetComponent<BoxCollider>();
        if (box == null) return;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0.95f, 0.75f, 0.2f, 0.9f);           // ladder-yellow
        Gizmos.DrawWireCube(box.center, box.size);

        // Climb direction (local up) — the axis the player moves along while climbing.
        Gizmos.color = Color.green;
        var top = box.center + Vector3.up * (box.size.y * 0.5f);
        Gizmos.DrawLine(box.center, top);
        Gizmos.DrawLine(top, top + new Vector3(0.08f, -0.12f, 0f));
        Gizmos.DrawLine(top, top + new Vector3(-0.08f, -0.12f, 0f));
    }
#endif
}
