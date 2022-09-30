using BehaviourSystem;
using UnityEngine;

public abstract class GameObjectAction : BehaviourAction<GameObject> {}

public abstract class GameObjectAction<TData> : BehaviourAction<GameObject, TData> where TData : struct {}