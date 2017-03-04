using UnityEngine;

public class PersistentDataObject : MonoBehaviour {

	public static PersistentDataObject Instance { get; private set; }
	public void Awake() {
		if(Instance != null) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	public int Mode;
	public int Size;

}
