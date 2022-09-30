using BehaviourSystem;
using UnityEngine;

public abstract class GameObjectDecision : BehaviourDecision<GameObject> { }

public abstract class GameObjectDecision<TData> : BehaviourDecision<GameObject, TData> where TData : struct { }