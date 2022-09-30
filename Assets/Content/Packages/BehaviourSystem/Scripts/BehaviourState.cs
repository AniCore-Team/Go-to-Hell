using System;
using XNode;

namespace BehaviourSystem
{
	public class BehaviourState<TEntity> : Node
	{
		[Serializable]
		public class Connection {}

		[Serializable]
		public class Transition
		{
			public BehaviourDecision<TEntity>[] Decisions;
			public BehaviourState<TEntity> TrueState;
			public string TruePortName = string.Empty;
			public BehaviourState<TEntity> FalseState;
			public string FalsePortName = string.Empty;
		}
		
		[Input(connectionType = ConnectionType.Multiple)] public Connection Enter;
		public BehaviourAction<TEntity>[] Actions;
		public Transition[] Transitions;
		
		public override void OnCreateConnection(NodePort from, NodePort to)
		{
			base.OnCreateConnection(from, to);
			SetupStates();
		}

		public override void OnRemoveConnection(NodePort port)
		{
			base.OnRemoveConnection(port);
			SetupStates();
		}

		public override object GetValue(NodePort port)
		{
			if (port.IsConnected)
			{
				BehaviourState<TEntity> connection = port.GetConnection(0).node as BehaviourState<TEntity>;
				return connection;
			}
			return null;
		}

		private void SetupStates()
		{
			for (int i = 0; i < Transitions.Length; i++)
			{
				Transition item = Transitions[i];
				NodePort truePort = GetOutputPort(item.TruePortName);
				if (truePort == null)
				{
					continue;
				}
				if (truePort.IsConnected)
				{
					BehaviourState<TEntity> state = truePort.GetOutputValue() as BehaviourState<TEntity>;
					item.TrueState = state != null ? state : null;
				}
				else
				{
					item.TrueState = null;
				}

				NodePort falsePort = GetOutputPort(item.FalsePortName);
				if (falsePort == null)
				{
					continue;
				}
				if (falsePort.IsConnected)
				{
					BehaviourState<TEntity> state = falsePort.GetOutputValue() as BehaviourState<TEntity>;
					item.FalseState = state != null ? state : null;
				}
				else
				{
					item.FalseState = null;
				}
			}
		}
	}
}
