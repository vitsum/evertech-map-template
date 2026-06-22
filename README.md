# Evertech Sandbox — Custom Map Template

**Languages:** English · [Русский](README.ru.md) · [Bahasa Indonesia](README.id.md)

This Unity project is a template for building your own **custom maps** for Evertech Sandbox.
Open it, edit the scene, build, and drop the result into the game.

---

## Requirements

- **Unity 2022** — use the version this template ships with (2022.3.x). Any 2022.x works; the game
  only checks the major year. (See *Unity version* below.)
- The render pipeline is **URP** — already set up. Don't switch it.

---

## What's inside

| Path | What it is |
|---|---|
| `Assets/Scene.unity` | The map scene. **This is what gets built into the map.** Its AssetBundle name is `scene` — keep it. |
| `Assets/MapScripts/` | Map markers (`PlayerSpawnPoint`, `WaterLevel`, …). |
| `Assets/MapSDK/` | The Map Settings SDK — lets you expose in-game settings, **no coding**. |
| `Assets/Editor/AssetBundleCreator.cs` | The build tool (menu **Custom Tools → Build Map …**). |
| `Assets/AssetBundles/` | Build output + your `info.json` and `preview.png`. This folder becomes your map. |

When you open `Assets/Scene.unity` you'll see a working example. Study it, then delete
`--- Map Settings Example (delete me) ---` and build your own.

---

## 1. Build your scene

Edit `Assets/Scene.unity` freely — terrain, props, lighting, etc.

> ⚠️ **Do NOT add a Camera.** The game supplies the player and camera.
> ⚠️ Keep everything in **this one scene** — only `Assets/Scene.unity` is built into the map.

### Markers (both optional)

- **Player Spawn Point** — empty GameObject + component **`PlayerSpawnPoint`**.
  Sets where the player appears and which way they face (the blue **Z / forward** axis).
  **Optional** — without it the player spawns at the default position (world origin).
- **Water Level** — empty GameObject + component **`WaterLevel`**.
  The object's **world Y** becomes the water surface height.
  **Optional** — without it the game uses its built-in default water level.

(Other markers exist in `Assets/MapScripts/`: `PushDown`, `PushForward`, `SpeedLimit`, … — add them the same way if your map needs them.)

---

## 2. Map Settings — expose options to players (optional, no code)

The SDK lets players tweak your map from the in-game **World Settings** panel.

### Step A — declare the settings

Add **one** empty GameObject with the **`MapSettingsDeclaration`** component and fill its `settings` list.
Each entry is one setting:

| Type | Shows as | Fields you fill |
|---|---|---|
| **Bool** | Toggle | `key`, `label`, `defaultBool` |
| **Slider** | Slider | `key`, `label`, `min`, `max`, `step` (0 = smooth), `defaultFloat` |
| **Enum** | Dropdown | `key`, `label`, `options[]`, `defaultOption` |

`key` is a unique id (e.g. `showDecorations`). `label` is the text players see.

### Step B — make settings do something (ready-made receivers, no code)

Add any of these components and point their `key` at a setting you declared:

| Component | Works with | What it does |
|---|---|---|
| **`SettingToggleObject`** | Bool | Shows/hides the GameObjects in `targets`. |
| **`SettingEnumObjects`** | Enum | Activates one object from `optionObjects` per option (index matches option order). |
| **`SettingSwapMaterial`** | Bool / Enum | Swaps the `materials` on the listed `renderers` (Bool: 0 = off, 1 = on; Enum: index = option). |
| **`MapSettingBinding`** | Any | Generic. Wire its `onBool` / `onFloat` / `onOption` **UnityEvent** in the Inspector to anything built-in. |

> **Slider settings** have no dedicated receiver — use **`MapSettingBinding`**:
> set its `key` to your slider, then in the Inspector drag a target into the **`onFloat`** event and pick a
> float setter (e.g. `Light.intensity`, `AudioSource.volume`). The event fires once on load and on every change.

**Keep keys unique** and identical between the declaration and the receivers/bindings.

There's a live example in the scene under `--- Map Settings Example (delete me) ---`
(a Bool toggle showing/hiding cubes, and an Enum switching between shapes). Delete that whole object before shipping.

---

## 3. Fill in map info

Edit these two files in `Assets/AssetBundles/`:

- **`info.json`** (required — a map with no `info.json` won't show up in the game)
  ```json
  {
      "name": "My Map Name",
      "author": "YourName",
      "version": "1.0",
      "description": "Short description shown in the map browser"
  }
  ```
- **`preview.png`** — the thumbnail shown on the map card. Keep it **small** — around **300×200** is plenty.
  Don't ship a huge full-resolution screenshot; it just bloats the map.

---

## 4. Build the map

Menu: **Custom Tools → Build Map for all platforms**
(or **Build Map for Android only** for a quick test build).

This writes the AssetBundles into `Assets/AssetBundles/<Platform>/` for every platform the game uses:
`Android`, `Windows64`, `iOS`.

---

## 5. Assemble the final map folder

The game expects a map folder shaped like this — **`info.json` and `preview.png` at the root**, one folder per platform, each holding just the `scene` file:

```
<MyMap>/
  info.json
  preview.png
  Android/scene
  Windows64/scene
  iOS/scene
```

Good news: after building, `Assets/AssetBundles/` already contains `info.json`, `preview.png`
and the platform folders — so it **is** your map folder. Just copy it out and rename it to your map.
**The folder name is the map's id**, so give it something unique.

> Each platform folder only needs the file named **`scene`**. The extra `*.manifest` files and the
> folder-named bundle files are build leftovers — you can delete them to keep the map clean (the game ignores them).

---

## 6. Install & test in the game

Put your `<MyMap>` folder into the game's **maps** folder, then start the game — it shows up in the
Maps list, where you can Open it.

The maps folder is inside the game's save data:

- **Windows:** `%USERPROFILE%\AppData\LocalLow\IronTube Games\Evertech Sandbox\maps\`
- **Mobile:** the app's persistent data folder, `…/Evertech Sandbox/maps/`

So the final path is e.g.
`…\Evertech Sandbox\maps\<MyMap>\info.json`, `…\maps\<MyMap>\Windows64\scene`, etc.

---

## Unity version

The game checks the **major year** of the Unity used to build the map (it must be **2022**).
- Building on any **2022.x** patch is fine — small differences (e.g. `2022.3.56` vs `2022.3.62`) at most
  cause minor visual differences.
- Building on a **different major version** (2021, 2023, 6000, …) makes the game flag the map as
  *not supported* in the maps list. Stick to 2022.

---

## Common mistakes

- ❌ Added a Camera to the scene → the game already has one. Remove it.
- ❌ Renamed `Assets/Scene.unity` or changed its AssetBundle name → it must stay `scene`.
- ❌ `info.json` / `preview.png` placed inside a platform folder → they go in the **root** of the map folder.
- ❌ Missing `info.json` → the map won't appear in the game's list at all.
- ❌ Two settings sharing the same `key`, or a receiver `key` not matching the declaration.
