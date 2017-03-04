using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;
using RoomModel = Data.Room.Model;
using TileModel = Data.Tile.Model;

namespace Dynamic.Tile {
	public class TileComponent : MonoBehaviour {
		private RoomModel RoomModel;
		public TileModel TileModel;
		private GameObject PotentialPlayer;

		private bool InternalIsPlayer;
		public bool IsPlayer {
			get { return InternalIsPlayer; }
			set {
				InternalIsPlayer = value;
				StateChangeToPlayer();
			}
		}

		private bool InternalIsFlooded = false;
		public bool IsFlooded {
			get { return InternalIsFlooded; }
			set {
				InternalIsFlooded = value;
				StateChangeToFlooded();
			}
		}

		private void StateChangeToFlooded() {
			GetComponent<SpriteRenderer>().sprite = Game.Instance.TileWave;
			if(IsPlayer) {
				Game.Instance.GameOver();
			}
		}

		private void StateChangeToPlayer() {
			if(IsPlayer) {
				if(IsFlooded) {
					Game.Instance.GameOver();
				} else {
					PotentialPlayer = new GameObject("player");
					PotentialPlayer.transform.parent = transform;
					PotentialPlayer.transform.localPosition = Vector3.zero;
					PotentialPlayer.AddComponent<SpriteRenderer>().sprite = Game.Instance.TilePlayer;

					//GetComponent<SpriteRenderer>().sprite = Game.Instance.TilePlayer;
					Game.Instance.PlayerTransform = transform;
				}
			} else {
				//GetComponent<SpriteRenderer>().sprite = Game.Instance.TileGround;
				Destroy(PotentialPlayer);
			}
		}

		private IEnumerator MakePlayerAfterDelay() {
			GetComponent<SpriteRenderer>().sprite = Game.Instance.TileGround;
			yield return new WaitForSeconds(4);
			IsPlayer = true;
		}


		public IEnumerator Start() {
			gameObject.AddComponent<BoxCollider2D>();
			RoomModel = transform.parent.GetComponent<RoomModel>();
			TileModel = gameObject.GetComponent<TileModel>();

			int r = Random.Range(0, 300);
			if(TileModel.Idx == 3) {
				Game.Instance.Do(MakePlayerAfterDelay());
			} else if(TileModel.Idx == 127 || TileModel.Idx == 212) {
				GetComponent<SpriteRenderer>().sprite = Game.Instance.TileWell;
			} else if(r > 280) {
				GetComponent<SpriteRenderer>().sprite = Game.Instance.TileBranch;
			} else if(r > 250) {
				GetComponent<SpriteRenderer>().sprite = Game.Instance.TileCrack;
			} else if(r > 150) {
				GetComponent<SpriteRenderer>().sprite = Game.Instance.TileFlowey[Random.Range(0, 2)];
			} else {
				GetComponent<SpriteRenderer>().sprite = Game.Instance.TileGround;
			}

			yield return new WaitForSeconds(1);

			Game.Instance.FloodManager.Add(this, RoomModel.Y + TileModel.Y);
		}

		public void OnMouseDown() {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 p = transform.position;
			if(mousePos.x.Within(p.x - 0.5f, p.x + 0.5f) &&
			   mousePos.y.Within(p.y - 0.5f, p.y + 0.5f)) {
				//RaycastHit2D[] col = Physics2D.CircleCastAll(transform.position, 0.8f, Vector2.up);
				//RaycastHit2D hit = col.First(rh => rh.transform.GetComponent<TileComponent>().IsPlayer);
				Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 0.8f);
				Collider2D hit = col.First(c => c.transform.GetComponent<TileComponent>().IsPlayer);
				if(hit != null) {
					if(hit.transform == transform) {
						// We clicked same obj
					} else {
						IsPlayer = true;
						hit.GetComponent<TileComponent>().IsPlayer = false;
						Game.Instance.FloodManager.Turn();
					}
				}
			}
		}

		public void AfterFlood() {
			GetComponent<SpriteRenderer>().sprite = Game.Instance.TileFlooded;
		}
	}
}