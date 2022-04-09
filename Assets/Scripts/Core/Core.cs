using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour {

	private readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();

	private void Awake() {

	}

	public void LogicUpdate() {
		foreach (CoreComponent component in CoreComponents) {
			component.LogicUpdate();
		}
	}

	public void AddComponent(CoreComponent component) {
		if (!CoreComponents.Contains(component)) {
			CoreComponents.Add(component);
		}
	}

	public T GetCoreComponent<T>() where T : CoreComponent {
		var comp = CoreComponents.OfType<T>().FirstOrDefault();

		if (comp == null) {
			Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
		}

		return comp;
	}

	public T GetCoreComponent<T>(ref T value) where T : CoreComponent {
		value = GetCoreComponent<T>();
		return value;
	}

}
