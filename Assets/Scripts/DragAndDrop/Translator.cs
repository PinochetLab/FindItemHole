using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : MonoBehaviour
{
	public static Translator instance;

	private void Awake() {
		instance = this;
	}
	public IEnumerator Translate(RectTransform rectTransform, Vector2 startPosition, Vector2 endPosition, float speed) {
		rectTransform.anchoredPosition = startPosition;
		float time = Vector2.Distance(endPosition, startPosition) / speed;
		float dt = 0.02f;
		int n = (int)(time / dt) + 1;
		dt = time / (n + 1);
		for (int i = 0; i <= n; i++ ) {
			float t = (float)i / (float)n;
			rectTransform.anchoredPosition = startPosition + (endPosition - startPosition) / 2f * (1 - Mathf.Cos(Mathf.PI * t));
			yield return new WaitForSeconds(dt);
		}
	}
}
