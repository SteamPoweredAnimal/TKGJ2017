using UnityEngine;
using UnityEngine.UI;

namespace Assets {
	public class FadeInOut : MonoBehaviour {
		public Texture2D FadeTexture;
		public Image Fader;
		public float FadeSpeed = 0.5f;
		public int DrawDepth = -1000;

		private float Alpha = 1;
		public int FadeDir = 1;

		public void Update() {
			Alpha -= FadeDir * FadeSpeed * Time.deltaTime;
			Alpha = Mathf.Clamp01(Alpha);

			Color c = Fader.color;
			c.a = Alpha;
			Fader.color = c;

			//Fader.depth = DrawDepth;

			//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
		}
	}
}