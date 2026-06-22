# Evertech Sandbox — Templat Peta Kustom

**Bahasa:** [English](README.md) · [Русский](README.ru.md) · Bahasa Indonesia

Proyek Unity ini adalah templat untuk membuat **peta kustom** kamu sendiri untuk Evertech Sandbox.
Buka, edit scene-nya, build, lalu taruh hasilnya ke dalam game.

---

## Persyaratan

- **Unity 2022** — pakai versi yang dipakai templat ini (2022.3.x). Patch 2022.x mana pun bisa;
  game hanya mengecek tahun mayor-nya. (Lihat *Versi Unity* di bawah.)
- Render pipeline-nya **URP** — sudah diatur. Jangan diganti.

---

## Isi templat

| Path | Keterangan |
|---|---|
| `Assets/Scene.unity` | Scene peta. **Inilah yang di-build menjadi peta.** Nama AssetBundle-nya `scene` — jangan diubah. |
| `Assets/MapScripts/` | Penanda peta (`PlayerSpawnPoint`, `WaterLevel`, …). |
| `Assets/MapSDK/` | SDK Map Settings — untuk menampilkan pengaturan di dalam game, **tanpa coding**. |
| `Assets/Editor/AssetBundleCreator.cs` | Alat build (menu **Custom Tools → Build Map …**). |
| `Assets/AssetBundles/` | Hasil build + `info.json` dan `preview.png` milikmu. Folder ini yang menjadi peta-mu. |

Saat membuka `Assets/Scene.unity`, kamu akan melihat contoh yang berfungsi. Pelajari dulu, lalu hapus
objek `--- Map Settings Example (delete me) ---` dan buat petamu sendiri.

---

## 1. Bangun scene-mu

Edit `Assets/Scene.unity` sebebasnya — medan, properti, pencahayaan, dll.

> ⚠️ **JANGAN tambahkan Camera.** Game sudah menyediakan pemain dan kamera.
> ⚠️ Simpan semuanya di **satu scene ini saja** — hanya `Assets/Scene.unity` yang di-build ke peta.

### Penanda (keduanya opsional)

- **Player Spawn Point** — GameObject kosong + komponen **`PlayerSpawnPoint`**.
  Menentukan di mana pemain muncul dan arah hadapnya (sumbu biru **Z / forward**).
  **Opsional** — tanpa ini pemain muncul di posisi default (titik asal/origin).
- **Water Level** — GameObject kosong + komponen **`WaterLevel`**.
  Nilai **Y (world)** objek menjadi ketinggian permukaan air.
  **Opsional** — tanpa ini game memakai ketinggian air default bawaannya.

(Ada penanda lain di `Assets/MapScripts/`: `PushDown`, `PushForward`, `SpeedLimit`, … — tambahkan dengan cara yang sama bila diperlukan.)

---

## 2. Map Settings — beri opsi ke pemain (opsional, tanpa kode)

SDK memungkinkan pemain mengubah petamu lewat panel **World Settings** di dalam game.

### Langkah A — deklarasikan pengaturan

Tambahkan **satu** GameObject kosong dengan komponen **`MapSettingsDeclaration`** dan isi daftar `settings`-nya.
Tiap entri adalah satu pengaturan:

| Tipe | Tampil sebagai | Field yang diisi |
|---|---|---|
| **Bool** | Toggle | `key`, `label`, `defaultBool` |
| **Slider** | Slider | `key`, `label`, `min`, `max`, `step` (0 = mulus), `defaultFloat` |
| **Enum** | Dropdown | `key`, `label`, `options[]`, `defaultOption` |

`key` adalah id unik (mis. `showDecorations`). `label` adalah teks yang dilihat pemain.

### Langkah B — buat pengaturan bekerja (receiver siap pakai, tanpa kode)

Tambahkan salah satu komponen ini dan arahkan `key`-nya ke pengaturan yang sudah kamu deklarasikan:

| Komponen | Untuk tipe | Fungsi |
|---|---|---|
| **`SettingToggleObject`** | Bool | Menampilkan/menyembunyikan GameObject di `targets`. |
| **`SettingEnumObjects`** | Enum | Mengaktifkan satu objek dari `optionObjects` per opsi (indeks = urutan opsi). |
| **`SettingSwapMaterial`** | Bool / Enum | Menukar `materials` pada `renderers` yang terdaftar (Bool: 0 = off, 1 = on; Enum: indeks = opsi). |
| **`MapSettingBinding`** | Apa saja | Serbaguna. Hubungkan **UnityEvent** `onBool` / `onFloat` / `onOption`-nya di Inspector ke apa pun yang bawaan. |

> Pengaturan **Slider** tidak punya receiver khusus — gunakan **`MapSettingBinding`**:
> set `key`-nya ke slider-mu, lalu di Inspector seret target ke event **`onFloat`** dan pilih
> setter float (mis. `Light.intensity`, `AudioSource.volume`). Event memicu sekali saat load dan pada tiap perubahan.

**Jaga `key` tetap unik** dan sama persis antara deklarasi dan receiver/binding.

Ada contoh langsung di scene di bawah objek `--- Map Settings Example (delete me) ---`
(toggle Bool menyembunyikan kubus, Enum mengganti bentuk). Hapus seluruh objek itu sebelum rilis.

---

## 3. Isi info peta

Edit dua file ini di `Assets/AssetBundles/`:

- **`info.json`** (wajib — peta tanpa `info.json` tidak akan muncul di game)
  ```json
  {
      "name": "Nama Peta Saya",
      "author": "NamaKamu",
      "version": "1.0",
      "description": "Deskripsi singkat yang tampil di daftar peta"
  }
  ```
- **`preview.png`** — thumbnail di kartu peta. Buat **kecil** — sekitar **300×200** sudah cukup.
  Jangan pakai screenshot resolusi penuh yang besar — itu cuma membengkakkan peta.

---

## 4. Build peta

Menu: **Custom Tools → Build Map for all platforms**
(atau **Build Map for Android only** untuk build uji cepat).

Ini menulis AssetBundle ke `Assets/AssetBundles/<Platform>/` untuk tiap platform yang dipakai game:
`Android`, `Windows64`, `iOS`.

---

## 5. Susun folder peta final

Game mengharapkan folder peta berbentuk seperti ini — **`info.json` dan `preview.png` di root**, satu folder per platform, masing-masing hanya berisi file `scene`:

```
<PetaSaya>/
  info.json
  preview.png
  Android/scene
  Windows64/scene
  iOS/scene
```

Kabar baik: setelah build, `Assets/AssetBundles/` sudah berisi `info.json`, `preview.png`,
dan folder-folder platform — jadi folder itu **memang** folder peta-mu. Cukup salin keluar dan ganti namanya.
**Nama folder = id peta**, jadi beri nama yang unik.

> Tiap folder platform hanya butuh file bernama **`scene`**. File `*.manifest` tambahan dan file bundle
> yang senama dengan folder adalah sisa build — boleh dihapus agar peta bersih (game mengabaikannya).

---

## 6. Pasang & uji di game

Taruh folder `<PetaSaya>` ke folder **maps** milik game, lalu jalankan game — peta muncul di daftar Maps,
dan bisa kamu Open dari sana.

Folder maps ada di dalam data simpanan game:

- **Windows:** `%USERPROFILE%\AppData\LocalLow\IronTube Games\Evertech Sandbox\maps\`
- **Mobile:** folder persistent data aplikasi, `…/Evertech Sandbox/maps/`

Jadi path akhirnya misalnya:
`…\Evertech Sandbox\maps\<PetaSaya>\info.json`, `…\maps\<PetaSaya>\Windows64\scene`, dst.

---

## Versi Unity

Game mengecek **tahun mayor** Unity yang dipakai mem-build peta (harus **2022**).
- Build dengan patch **2022.x** mana pun aman — perbedaan kecil (mis. `2022.3.56` vs `2022.3.62`)
  paling banter cuma menyebabkan sedikit perbedaan tampilan.
- Build dengan **versi mayor berbeda** (2021, 2023, 6000, …) membuat game menandai peta sebagai
  *tidak didukung* di daftar peta. Tetap pakai 2022.

---

## Kesalahan umum

- ❌ Menambah Camera ke scene → game sudah punya. Hapus.
- ❌ Mengganti nama `Assets/Scene.unity` atau mengubah nama AssetBundle-nya → harus tetap `scene`.
- ❌ `info.json` / `preview.png` ditaruh di dalam folder platform → seharusnya di **root** folder peta.
- ❌ Tidak ada `info.json` → peta tidak akan muncul di daftar game sama sekali.
- ❌ Dua pengaturan memakai `key` yang sama, atau `key` receiver tidak cocok dengan deklarasi.
