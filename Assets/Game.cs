using System.Collections;
using Dynamic.Tile;
using FloodManager = Management.Flood.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public GameObject Tile;
	public GameObject Wall;
	public GameObject Room;
	public Sprite TileFlooded;
	public Sprite TileWave;
	public Sprite TileGround;
	public Sprite[] TileFlowey;
	public Sprite TileBranch;
	public Sprite TileWell;
	public Sprite TileCrack;
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
		PlayerPrefs.SetInt("hiScore" + PersistentDataObject.Instance.Size + ":" +
			PersistentDataObject.Instance.Mode.ToString(), HiScore);
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
//		DontDestroyOnLoad(gameObject);
	}

	public void Start() {
		if(PersistentDataObject.Instance == null) {
			Init();
		} else {
			int size = PersistentDataObject.Instance.Size;
			int mode = PersistentDataObject.Instance.Mode;
			int min = size + 1;
			int max = size + 3;
			Generator = new Generator(
				32 * (PersistentDataObject.Instance.Size + 1),
				transform,
				Room,
				new RoomSize(min, min, max, max),
				Tile, Wall);
			Generator.SpawnLevel();
			FloodManager = new FloodManager(mode);
			HiScore = PlayerPrefs.GetInt("hiScore" + size.ToString() + ":" + mode.ToString(), 0);
		}
	}

	public void Init() {
		Generator = new Generator(72, transform, Room, new RoomSize(1, 1, 4, 4), Tile, Wall);
		Generator.SpawnLevel();
		FloodManager = new FloodManager();
		HiScore = PlayerPrefs.GetInt("hiScore", 0);
	}

	public void Do(IEnumerator something) {
		StartCoroutine(something);
	}

	public void GameOver() {
		if(HiScore < Score) HiScore = Score;
		SceneManager.LoadScene("Main");
	}

	public void MoveW() {
		Move(Vector2.left);
	}

	public void MoveN() {
		Move(Vector2.up);
	}

	public void MoveS() {
		Move(Vector2.down);
	}

	public void MoveE() {
		Move(Vector2.right);
	}

	private void Move(Vector3 where) {
		//Gizmos.DrawSphere(transform.position + (where * 1.5f), 0.1f);
		//Debug.Break();
		//GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//go.transform.position = transform.position + (where * 1.5f);
		//Debug.DrawLine(transform.position, transform.position + (where * 1.5f));
		//Debug.Break();
		Collider2D c = Physics2D.OverlapCircle(PlayerTransform.position + where, 0.1f);
		if(c != null && c.transform != PlayerTransform) {
			TileComponent tc = PlayerTransform.GetComponent<TileComponent>();
			c.transform.GetComponent<TileComponent>().IsPlayer = true;
			tc.IsPlayer = false;
			PlayerTransform = c.transform;
			FloodManager.Turn();
		}
	}

	public void Sleep() {
		FloodManager.Turn();
	}
}