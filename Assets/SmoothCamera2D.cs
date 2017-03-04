using UnityEngine;
using System.Collections;
using Assets;

public class SmoothCamera2D : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	public IEnumerator Start() {
		yield return new WaitForSeconds(0.5f);
		GetComponent<FadeInOut>().FadeDir = 1;

		float ratio = 9.0f / 16;
		Camera.main.rect = new Rect(0, 0, 1, ratio);
		Game.Instance.Support.rect = new Rect(0, ratio, 1, 1 - ratio);

		target = Game.Instance.transform;
		yield return new WaitForSeconds(2.5f);
		GetComponent<FadeInOut>().FadeDir = -1;
		yield return new WaitForSeconds(3f);
		Camera.main.orthographicSize = 2.5f;
		target = Game.Instance.PlayerTransform;
		yield return new WaitForSeconds(1f);
		GetComponent<FadeInOut>().FadeDir = 1;
	}

	// Update is called once per frame
	void Update ()
	{
		if (target)
		{
			Vector3 point = Camera.main.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

	}
}