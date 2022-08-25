using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
	[SerializeField] private Image itemImage;
	[SerializeField] private DragAndDrop dragAndDrop;
	[SerializeField] private RectTransform downPoint;

	[SerializeField] private List<Level> levels;
	[SerializeField] private HorizontalLayoutGroup holesPanel;
	[SerializeField] private GameObject holePrefab;
	[SerializeField] private Sounder sounder;

	private List<Item> items = new List<Item>();
	private List<HoleImage> holeImages = new List<HoleImage>();

	private int rightIndex = 0;
	private static float maxDistance = 200;
	private int levelIndex = 0;

	private void Awake() {
		SetUp(levels[levelIndex]);
	}

	public void SetUp(Level level) {
		Clear();
		List<Item> items = level.Items;
		this.items = new List<Item>();
		for (int i = 0; i < items.Count; i++ ) {
			this.items.Add(items[i]);
		}
		rightIndex = Random.Range(0, items.Count);
		Item item = items[rightIndex];

		for (int i = 0; i < items.Count; i++ ) {
			HoleImage holeImage = Instantiate(holePrefab, holesPanel.transform).GetComponent<HoleImage>();
			holeImage.SetSprites(items[i].Hole, items[i].Sprite);
			holeImages.Add(holeImage);
		}

		itemImage.sprite = item.Sprite;

		SetSize(items.Count);

		StartCoroutine(LevelAppear());
	}

	private void Clear() {
		var children = new List<GameObject>();
		foreach ( Transform child in holesPanel.transform ) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
	}

	public void SetSize(int n, float ratio = 0.25f) {
		print(holesPanel.GetComponent<RectTransform>().sizeDelta.x);
		float x = holesPanel.GetComponent<RectTransform>().rect.width / (n + ratio * (n + 1));
		foreach (HoleImage holeImage in holeImages ) {
			holeImage.RectTransform.sizeDelta = Vector2.one * x;
		}
	}

	public void SetUp() {
		rightIndex = Random.Range(0, items.Count);
		Item item = items[rightIndex];

		itemImage.sprite = item.Sprite;
	}

	public void SetOutline(int i, bool active) {
		holeImages[i].SetOutline(active);
	}

	public void DisableOutlines() {
		for (int i = 0; i < holeImages.Count; i++ ) {
			SetOutline(i, false);
		}
	}

	public void TrySelect(Vector2 position) {
		int index = 0;
		float minDistance = float.PositiveInfinity;
		for (int i = 0; i < holeImages.Count; i++ ) {
			Vector2 holePosition = GetPosition(i);
			float distance = Vector2.Distance(holePosition, position);
			if ( distance < minDistance ) {
				minDistance = distance;
				index = i;
			}
		}

		print(minDistance);

		for (int i = 0; i < holeImages.Count; i++ ) {
			SetOutline(i, i == index && minDistance < maxDistance);
		}
	}

	public void CheckDrop(Vector2 position) {
		int index = 0;
		float minDistance = float.PositiveInfinity;
		for ( int i = 0; i < holeImages.Count; i++ ) {
			Vector2 holePosition = GetPosition(i);
			float distance = Vector2.Distance(holePosition, position);
			if ( distance < minDistance ) {
				minDistance = distance;
				index = i;
			}
		}

		if (index == rightIndex && minDistance < maxDistance ) {
			print(holeImages[rightIndex].GetComponentsInChildren<Image>(true)[1]);
			holeImages[rightIndex].GetComponentsInChildren<Image>(true)[1].enabled = true;
			items.RemoveAt(rightIndex);
			SetOutline(rightIndex, false);
			holeImages.RemoveAt(rightIndex);
			sounder.Play("item_success");
			if ( items.Count > 0 ) {
				SetUp();
				StartCoroutine(ItemAppear());
			}
			else {
				dragAndDrop.Position = dragAndDrop.StartPosition;
				levelIndex++;
				if (levelIndex >= levels.Count ) {
					levelIndex = 0;
				}
				SetUp(levels[levelIndex]);
			}
		}
		else {
			StartCoroutine(ReturnToStart());
		}
		DisableOutlines();
	}

	public IEnumerator ReturnToStart() {
		dragAndDrop.enabled = false;
		yield return Translator.instance.Translate(dragAndDrop.RectTransform, dragAndDrop.Position, dragAndDrop.StartPosition, 10000);
		dragAndDrop.enabled = true;
	}

	public IEnumerator LevelAppear() {
		yield return new WaitForEndOfFrame();
		dragAndDrop.enabled = false;
		holesPanel.enabled = false;

		for ( int i = 0; i < holeImages.Count; i++ ) {
			holeImages[i].gameObject.SetActive(false);
		}

		sounder.Play("item_appear");

		for ( int i = 0; i < holeImages.Count; i++ ) {
			Vector2 endPosition = holeImages[i].AnchoredPosition;
			Vector2 startPositon = endPosition + Vector2.up * 1000;
			holeImages[i].gameObject.SetActive(true);
			StartCoroutine(Translator.instance.Translate(holeImages[i].RectTransform, startPositon, endPosition, 3000));
			yield return new WaitForSeconds(0.2f);
			sounder.Play("item_appear");
		}

		holesPanel.enabled = true;
		dragAndDrop.enabled = true;
	}

	public IEnumerator ItemAppear() {
		Vector2 startPosition = downPoint.anchoredPosition;
		yield return Translator.instance.Translate(dragAndDrop.RectTransform, startPosition, dragAndDrop.StartPosition, 3000);
		sounder.Play("item_appear");
	}

	public Vector2 GetPosition(int i) {
		return holeImages[i].GetPosition();
	}
}
