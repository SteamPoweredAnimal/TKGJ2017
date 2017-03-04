using UnityEngine;

namespace Data.Tile {
	public class Model : MonoBehaviour {
		public int X, Y;
		public int Idx;

		public void Start() {
			Idx = Game.Instance.Count;
		}

	}
}