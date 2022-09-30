using System.Collections.Generic;
using UnityEngine;

namespace BehaviourSystem
{
	public abstract class BehaviourAction<TEntity> : ScriptableObject
	{
		public abstract void BeginAction(TEntity entity);
		public abstract void DoFixAction(TEntity entity);
		public abstract void DoAction(TEntity entity);
		public abstract void DoLateAction(TEntity entity);
		public abstract void EndAction(TEntity entity);
	}

	public abstract class BehaviourAction<TEntity, TData> : BehaviourAction<TEntity> where TData : struct
	{
		private readonly IDictionary<TEntity, TData> database = new Dictionary<TEntity, TData>();

		public override sealed void BeginAction(TEntity entity)
		{
			TData data = new TData();
			if (!database.ContainsKey(entity))
			{
				database.Add(entity, data);
			}

			data = database[entity];
			BeginAction(entity, ref data);
			database[entity] = data;
		}

		public override sealed void DoFixAction(TEntity entity)
		{
			TData data = database[entity];
			DoFixAction(entity, ref data);
			database[entity] = data;
		}

		public override sealed void DoAction(TEntity entity)
		{
			TData data = database[entity];
			DoAction(entity, ref data);
			database[entity] = data;
		}

		public override sealed void DoLateAction(TEntity entity)
		{
			TData data = database[entity];
			DoLateAction(entity, ref data);
			database[entity] = data;
		}

		public override sealed void EndAction(TEntity entity)
		{
			TData data = database[entity];
			EndAction(entity, ref data);
			database[entity] = data;
		}

		protected void SetData(TEntity entity, TData value)
		{
			database[entity] = value;
		}

		protected bool TryGetData(TEntity entity, out TData value)
		{
			return database.TryGetValue(entity, out value);
		}

		protected abstract void BeginAction(TEntity entity, ref TData data);
		protected abstract void DoFixAction(TEntity entity, ref TData data);
		protected abstract void DoAction(TEntity entity, ref TData data);
		protected abstract void DoLateAction(TEntity entity, ref TData data);
		protected abstract void EndAction(TEntity entity, ref TData data);
	}
}
