using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Util;
using RoomModel = Data.Room.Model;
using TileModel = Data.Tile.Model;

namespace Dynamic.Tile {
	public class TileComponent : MonoBehaviour {
		private RoomModel RoomModel;
		public TileModel TileModel;

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
			GetComponent<SpriteRenderer>().sprite = Game.Instance.TileFlooded;
			if(IsPlayer) {
				Game.Instance.GameOver();
			}
		}

		private void StateChangeToPlayer() {
			if(IsPlayer) {
				if(IsFlooded) {
					Debug.Log(IsPlayer);
					Debug.Log(IsFlooded);
					Game.Instance.GameOver();
				} else {
					GetComponent<SpriteRenderer>().sprite = Game.Instance.TilePlayer;
					Game.Instance.PlayerTransform = transform;
				}
			} else {
				GetComponent<SpriteRenderer>().sprite = Game.Instance.TileGround;
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

			if(TileModel.Idx == 3) {
				Game.Instance.Do(MakePlayerAfterDelay());
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
				RaycastHit2D[] col = Physics2D.CircleCastAll(transform.position, 0.8f, Vector2.up);
				RaycastHit2D hit = col.First(rh => rh.transform.GetComponent<TileComponent>().IsPlayer);
				if(hit != null) {
					if(hit.transform == transform) {
						// We clicked same obj
					} else {
						IsPlayer = true;
						hit.collider.GetComponent<TileComponent>().IsPlayer = false;
						Game.Instance.FloodManager.Turn();
					}
				}
			}
		}
	}
}