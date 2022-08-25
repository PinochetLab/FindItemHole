using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	private Vector2 offset = Vector2.zero;
	private RectTransform rectTransform;
	private bool drag = false;
	private Vector2 startPosition;

	[SerializeField] private UnityEvent<bool> dragEvent;
	[SerializeField] private UnityEvent<Vector2> OnMoveVector2Event;
	[SerializeField] private UnityEvent<Vector2> OnEndDragVector2Event;


	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
		startPosition = rectTransform.anchoredPosition;
		dragEvent.Invoke(drag);
	}

	public void OnBeginDrag(PointerEventData eventData) {
		drag = true;
		dragEvent.Invoke(drag);
		offset = (Vector2)Input.mousePosition - rectTransform.anchoredPosition;
	}

	public void OnDrag(PointerEventData eventData) {
		rectTransform.anchoredPosition = (Vector2)Input.mousePosition - offset;
		OnMoveVector2Event.Invoke(GetPosition());
	}

	public void OnEndDrag(PointerEventData eventData) {
		drag = false;
		OnEndDragVector2Event.Invoke(GetPosition());
		dragEvent.Invoke(drag);
	}

	private Vector2 GetPosition() {
		Vector3[] corners = new Vector3[4];
		rectTransform.GetWorldCorners(corners);
		return (corners[0] + corners[2]) / 2;
	}

	public Vector2 Position {
		get => rectTransform.anchoredPosition;
		set => rectTransform.anchoredPosition = value;
	}

	public RectTransform RectTransform {
		get => rectTransform;
	}

	public Vector2 StartPosition {
		get => startPosition;
	}
}
