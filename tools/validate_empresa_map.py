"""Static Unity scene checks. Run with Python and PyYAML installed."""
from pathlib import Path
import re
import unittest

import yaml

ROOT = Path(__file__).resolve().parents[1]
ASSETS = ROOT / "videojuego/Assets"
SCENE = ASSETS / "Art/Characters/Scenes/Nivel01_Empresa/nivel1.unity"


def documents(path):
    text = path.read_text(encoding="utf-8-sig")
    result = {}
    for match in re.finditer(r"^--- !u!\d+ &(-?\d+)[^\n]*\n(.*?)(?=^--- !u!|\Z)", text, re.M | re.S):
        key = int(match[1])
        assert key not in result, f"Duplicate fileID {key} in {path}"
        result[key] = yaml.safe_load(match[2])
    return result


class EmpresaMapTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.scene = documents(SCENE)
        cls.metas = {}
        for path in ASSETS.rglob("*.meta"):
            match = re.search(r"^guid: (\w+)", path.read_text(encoding="utf-8-sig"), re.M)
            if match:
                cls.metas[match[1]] = path
        cls.maps = {}
        for doc in cls.scene.values():
            if "Tilemap" in doc:
                tilemap = doc["Tilemap"]
                name = cls.scene[tilemap["m_GameObject"]["fileID"]]["GameObject"]["m_Name"]
                cls.maps[name] = tilemap

    def test_office_floor_and_layout(self):
        floor = self.maps["Tilemap_Floor"]["m_Tiles"]
        cells = {(t["first"]["x"], t["first"]["y"]) for t in floor}
        self.assertEqual(cells, {(x, y) for x in range(-18, 18) for y in range(-12, 12)})
        walls = self.maps["Tilemap_Walls"]["m_Tiles"]
        wall_cells = {(t["first"]["x"], t["first"]["y"]) for t in walls}
        self.assertGreaterEqual(len(wall_cells), 300)
        self.assertTrue({(-18, -12), (17, 11)} <= wall_cells)
        doors = {(t["first"]["x"], t["first"]["y"]) for t in self.maps["Tilemap_Doors"]["m_Tiles"]}
        windows = {(t["first"]["x"], t["first"]["y"]) for t in self.maps["Tilemap_Windows"]["m_Tiles"]}
        self.assertEqual(len(doors), 45)
        self.assertEqual(len(windows), 18)
        self.assertFalse(doors & wall_cells)
        self.assertFalse(windows & wall_cells)
        self.assertFalse(self.maps["Tilemap_Details"]["m_Tiles"])

    def test_tiles_and_sprites_resolve(self):
        for path in (SCENE, ASSETS / "Mapa-Principal.prefab"):
            for doc in documents(path).values():
                if "Tilemap" not in doc:
                    continue
                tilemap = doc["Tilemap"]
                for tile in tilemap["m_Tiles"]:
                    indices = tile["second"]
                    for index_key, array_key in (
                        ("m_TileIndex", "m_TileAssetArray"),
                        ("m_TileSpriteIndex", "m_TileSpriteArray"),
                        ("m_TileMatrixIndex", "m_TileMatrixArray"),
                        ("m_TileColorIndex", "m_TileColorArray"),
                    ):
                        self.assertLess(indices[index_key], len(tilemap[array_key]))
                for entry in tilemap["m_TileAssetArray"]:
                    self.assertIn(entry["m_Data"]["guid"], self.metas)
                for entry in tilemap["m_TileSpriteArray"]:
                    ref = entry["m_Data"]
                    self.assertIn(ref["guid"], self.metas)
                    text = self.metas[ref["guid"]].read_text()
                    self.assertIn(f'internalID: {ref["fileID"]}\n', text)

    def test_local_references(self):
        for ref in re.findall(r"\{fileID: (-?\d+)\}", SCENE.read_text()):
            if int(ref):
                self.assertIn(int(ref), self.scene)

    def test_collision_boundary_and_clear_spawn(self):
        colliders = [d["TilemapCollider2D"] for d in self.scene.values() if "TilemapCollider2D" in d]
        self.assertEqual(len(colliders), 12)  # walls + 11 persistent decoration layers
        self.assertTrue(all(not c["m_IsTrigger"] for c in colliders))
        self.assertIn(1119227067, [c["m_GameObject"]["fileID"] for c in colliders])
        self.assertFalse(any("BoxCollider2D" in d for d in self.scene.values()))

    def test_player_light_camera_and_start_scene(self):
        instances = [d["PrefabInstance"] for d in self.scene.values() if "PrefabInstance" in d]
        self.assertEqual(len(instances), 1)
        self.assertEqual(instances[0]["m_SourcePrefab"]["guid"], "ec1a1ab512b351a49860ea9faf4adfd2")
        behaviours = [d["MonoBehaviour"] for d in self.scene.values() if "MonoBehaviour" in d]
        self.assertTrue(any(b.get("m_LightType") == 4 for b in behaviours))
        self.assertTrue(any(b.get("mapSize") == {"x": 36, "y": 24} for b in behaviours))
        build = documents(ROOT / "videojuego/ProjectSettings/EditorBuildSettings.asset")
        scenes = next(iter(build.values()))["EditorBuildSettings"]["m_Scenes"]
        self.assertEqual(scenes[0]["path"], SCENE.relative_to(ASSETS.parent).as_posix())
        self.assertEqual(scenes[0]["enabled"], 1)


if __name__ == "__main__":
    unittest.main(verbosity=2)
