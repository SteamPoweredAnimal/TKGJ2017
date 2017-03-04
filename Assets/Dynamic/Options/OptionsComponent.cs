using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dynamic.Options {
	public class OptionsComponent : MonoBehaviour {

		public GameObject Mode, Size;

		public void Start() {
			Size.SetActive(false);
		}

		public void Easy() {
			PersistentDataObject.Instance.Mode = 0;
			Next();
		}

		public void Hard() {
			PersistentDataObject.Instance.Mode = 1;
			Next();
		}

		private void Next() {
			Size.SetActive(true);
			Mode.SetActive(false);
		}

		public void Small() {
			PersistentDataObject.Instance.Size = 0;
			End();
		}

		private void End() {
			SceneManager.LoadScene("Main");
		}

		public void Medium() {
			PersistentDataObject.Instance.Size = 1;
			End();
		}

		public void Large() {
			PersistentDataObject.Instance.Size = 2;
			End();
		}

	}
}