using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
	[SerializeField] private Sprite sprite;
	[SerializeField] private Sprite hole;

	public Sprite Sprite {
		get => sprite;
	}

	public Sprite Hole {
		get => hole;
	}
}
