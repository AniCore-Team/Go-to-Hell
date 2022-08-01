using UnityEngine;
using Zenject;

public class ZenFactory<T> : IFactory<GameObject, T> where T: MonoBehaviour
{
    readonly DiContainer container;

    public ZenFactory(DiContainer container)
    {
        this.container = container;
    }

    public T Create(GameObject prefab)
    {
        return container.InstantiatePrefabForComponent<T>(prefab);
    }
}

public class Factory<T> : PlaceholderFactory<GameObject, T>
{

}