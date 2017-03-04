using System.Collections;
using FloodManager = Management.Flood.Manager;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public GameObject Tile;
	public GameObject Wall;
	public GameObject Room;
	public Sprite TileFlooded;
	public Sprite TileGround;
	public Sprite TilePlayer;

	public Camera Support;

	public Generator Generator;

	private Transform InternalPlayerTransfrom;
	public Transform PlayerTransform {
		get { return InternalPlayerTransfrom; }
		set {
			InternalPlayerTransfrom = value;
			Camera.main.GetComponent<SmoothCamera2D>().target = PlayerTransform;
		}
	}

	public static Game Instance { get; private set; }

	public FloodManager FloodManager;

	private int InternalHiScore;
	public int HiScore {
		get { return InternalHiScore; }
		set {
			InternalHiScore = value;
			UpdateHiScore();
		}
	}

	private int InternalScore;
	public int Score {
		get { return InternalScore; }
		set {
			InternalScore = value;
			UpdateScore();
		}
	}

	private void UpdateScore() {
		GameObject.Find("Score").GetComponent<Text>().text = Score.ToString();
	}

	private void UpdateHiScore() {
		PlayerPrefs.SetInt("hiScore", HiScore);
		GameObject.Find("HiScore").GetComponent<Text>().text = HiScore.ToString();
	}

	private int InternalCount = 0;
	public int Count {
		get { return InternalCount++; }
	}

	public void Awake() {
		if(Instance != null) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}
	}

	public void Start() {
		Generator = new Generator(72, transform, Room, new RoomSize(1, 1, 4, 4), Tile, Wall);
		Generator.SpawnLevel();
		FloodManager = new FloodManager();
		HiScore = PlayerPrefs.GetInt("hiScore", 0);
	}

	public void Do(IEnumerator something) {
		StartCoroutine(something);
	}

	public void GameOver() {
		throw new System.NotImplementedException();
	}
}