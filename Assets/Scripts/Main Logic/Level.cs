using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Place { Place1 = 0, Place2 = 1, Place3 = 2};

[CreateAssetMenu(menuName = "Level")]
public class Level : ScriptableObject
{
	[SerializeField] private List<Item> items;

	public List<Item> Items {
		get => items;
	}
}
