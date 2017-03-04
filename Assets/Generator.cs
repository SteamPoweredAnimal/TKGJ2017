using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dynamic.Room;
using RoomModel = Data.Room.Model;
using TileModel = Data.Tile.Model;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public struct RoomSize {
	public int MinWidth, MaxWidth, MinHeight, MaxHeight;

	public RoomSize(int minWidth, int minHeight, int maxWidth, int maxHeight) {
		MinWidth = minWidth;
		MinHeight = minHeight;
		MaxWidth = maxWidth;
		MaxHeight = maxHeight;
	}
}

public class Generator {
	private int Quantity;
	private Transform Parent;
	private GameObject Prefab;
	private List<GameObject> Rooms;
	private RoomSize RoomSize;
	private GameObject RoomTile;
	private GameObject WallTile;

	private Rect LevelBounds;

	private int[,] Level;

	public Generator(int quantity,
		Transform parent,
		GameObject prefab,
		RoomSize roomSize,
		GameObject roomTile,
		GameObject wallTile) {
		Quantity = quantity;
		Parent = parent;
		Prefab = prefab;
		RoomSize = roomSize;
		RoomTile = roomTile;
		WallTile = wallTile;
		Rooms = new List<GameObject>();
	}

	public void SpawnLevel() {
		for(int i = 0; i < Quantity; i++) {
			Vector2 r = Random.insideUnitCircle;
			GameObject go = Object.Instantiate(Prefab, new Vector3(r.x, r.y, 0.0f), Quaternion.identity, Parent);
			go.transform.localPosition = new Vector3(r.x, r.y, 0.0f);
			go.name = "room " + i;
			Rooms.Add(go);
			int width;
			int height;
			SpawnRoom(go.transform, out width, out height);
			BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
			bc.size = new Vector2(width, height);
			bc.offset = new Vector2((width - 1) * 0.5f, (height - 1) * 0.5f); // Not clear why that equation, but it works
			go.AddComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			RoomModel rm = go.AddComponent<RoomModel>();
			go.AddComponent<RoomComponent>();
			rm.Height = height;
			rm.Width = width;
			Game.Instance.Do(SnapToGridWhenFinished());
		}
	}

	private void SpawnRoom(Transform goTransform, out int width, out int height) {
		width = Random.Range(RoomSize.MinWidth, RoomSize.MaxWidth + 1);
		height = Random.Range(RoomSize.MinHeight, RoomSize.MaxHeight + 1);
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				GameObject go = Object.Instantiate(RoomTile, new Vector3(i, j, 0), Quaternion.identity, goTransform);
				go.transform.localPosition = new Vector3(i, j, 0);
				go.name = "tile (" + i + ", " + j + ")";
				TileModel tm = go.AddComponent<TileModel>();
				go.AddComponent<Dynamic.Tile.TileComponent>();
				tm.X = i;
				tm.Y = j;
			}
		}
	}

	private IEnumerator SnapToGridWhenFinished() {
		while(true) {
			yield return new WaitForSeconds(0.5f);
			if(Rooms.All(go => {
				Vector2 v = go.GetComponent<Rigidbody2D>().velocity;
				return v.x <= 0.0001f && v.y <= 0.0001f;
			})) {
				int minX = 0, minY = 0, maxX = 0, maxY = 0;
				foreach(GameObject room in Rooms) {
					Object.Destroy(room.GetComponent<BoxCollider2D>());
					Object.Destroy(room.GetComponent<Rigidbody2D>());
					Vector2 p = room.transform.position;
					float x = Mathf.RoundToInt(p.x);
					float y = Mathf.RoundToInt(p.y);
					room.transform.position = new Vector3(x, y, 0.0f);
					RoomModel rm = room.GetComponent<RoomModel>();
					rm.X = (int) ((x - rm.Width / 2.0f) + 0.5f);
					rm.Y = (int) ((y - rm.Height / 2.0f) + 0.5f);
					if(minX > (x - rm.Width / 2.0f)) minX = (int) (x - rm.Width / 2.0f);
					if(minY > (y - rm.Height / 2.0f)) minY = (int) (y - rm.Height / 2.0f);
					if(maxX < (x + rm.Width / 2.0f)) maxX = (int) (x + rm.Width / 2.0f);
					if(maxY < (y + rm.Height / 2.0f)) maxY = (int) (y + rm.Height / 2.0f);
				}
				LevelBounds = new Rect(minX, minY, maxX - minX, maxY - minY);
				yield break;
			}
		}

	}
}
