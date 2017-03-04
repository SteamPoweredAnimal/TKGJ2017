using System.Collections.Generic;
using System.Linq;
using Dynamic.Tile;
using UnityEngine;

namespace Management.Flood {
	public class Manager {
		private int? CurrentRow = null;
		private Dictionary<int, List<TileComponent>> Rows;
		private int Max = 0;
		private int Difficulty;
		private int Module;

		public Manager(int difficulty = 1) {
			Rows = new Dictionary<int, List<TileComponent>>();
			Difficulty = difficulty;
		}

		public void Add(TileComponent tile, int y) {
			if(!Rows.ContainsKey(y)) {
				Rows.Add(y, new List<TileComponent>(new TileComponent[] {tile}));
			} else {
				Rows[y].Add(tile);
			}
		}

		public void Turn() {

			if(CurrentRow == null) {
				Max = Rows.Keys.Max();
				CurrentRow = Max;
			}
			if(Difficulty == 0) {
				if(Module++ % 2 == 1) {
					return;
				}
			}
			if(Rows.ContainsKey(CurrentRow.Value)) {
				foreach(TileComponent tileComponent in Rows[CurrentRow.Value]) {
					tileComponent.IsFlooded = true;
				}
			}
			if(Rows.ContainsKey(CurrentRow.Value - 1)) {
				foreach(TileComponent tileComponent in Rows[CurrentRow.Value - 1]) {
					tileComponent.AfterFlood();
				}
			}
			CurrentRow--;
			Game.Instance.Score = Max - CurrentRow.Value;
		}
	}
}