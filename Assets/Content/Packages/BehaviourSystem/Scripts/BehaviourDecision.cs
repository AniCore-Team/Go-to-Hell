using System.Collections.Generic;
using UnityEngine;

namespace BehaviourSystem
{
	public abstract class BehaviourDecision<TEntity> : ScriptableObject
	{
		public abstract bool GetDecision(TEntity entity);

		public virtual void BeginDecisionProcessing(TEntity entity) {}
		public virtual void EndDecisionProcessing(TEntity entity) {}
	}

	public abstract class BehaviourDecision<TEntity, TData> : BehaviourDecision<TEntity> where TData : struct
	{
		private readonly IDictionary<TEntity, TData> database = new Dictionary<TEntity, TData>();
		
		public override void BeginDecisionProcessing(TEntity entity)
		{
			TData data = new TData();
			database.Add(entity, data);
			BeginDecisionProcessing(entity, ref data);
			database[entity] = data;
		}

		public override void EndDecisionProcessing(TEntity entity)
		{
			TData data = database[entity];
			EndDecisionProcessing(entity, data);
			database.Remove(entity);
		}

		public override bool GetDecision(TEntity entity)
		{
			TData data = database[entity];
			bool result = GetDecision(entity, ref data);
			database[entity] = data;
			return result;
		}
		
		protected void SetData(TEntity entity, TData value)
		{
			database[entity] = value;
		}

		protected bool TryGetData(TEntity entity, out TData value)
		{
			return database.TryGetValue(entity, out value);
		}

		protected abstract void BeginDecisionProcessing(TEntity entity, ref TData data);
		protected abstract void EndDecisionProcessing(TEntity entity, TData data);
		protected abstract bool GetDecision(TEntity entity, ref TData data);
	}
}
