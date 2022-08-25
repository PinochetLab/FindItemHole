using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleImage : MonoBehaviour {
	[SerializeField] private Image holeImage;
	[SerializeField] private Image itemImage;
	[SerializeField] private Outline outline;

	private void Awake() {
		SetItemEnabled(false);
		SetOutline(false);
	}

	public void SetOutline(bool active) {
		outline.enabled = active;
	}

	public void SetItemEnabled(bool active) {
		itemImage.enabled = active;
	}

	public void SetSprites(Sprite holeSprite, Sprite itemSprite) {
		holeImage.sprite = holeSprite;
		itemImage.sprite = itemSprite;
	}

	public RectTransform RectTransform {
		get => holeImage.rectTransform;
	}

	public Vector2 GetPosition() {
		Vector3[] corners = new Vector3[4];
		holeImage.rectTransform.GetWorldCorners(corners);
		return (corners[0] + corners[2]) / 2;
	}

	public Vector2 AnchoredPosition {
		get => RectTransform.anchoredPosition;
	}
}