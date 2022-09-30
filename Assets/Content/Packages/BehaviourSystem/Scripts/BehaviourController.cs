using System;
using UnityEngine;

namespace BehaviourSystem
{
	public abstract class BehaviourController<TEntity> : IBehaviourController<TEntity>
	{
		private TEntity entity = default;
		
		private BehaviourState<TEntity> currentState = null;
		private BehaviourGraph<TEntity> graph = null;
		private bool firstProcessing = true;

		public bool HasBehaviour { get; private set; } = false;

		public bool TryInstall(TEntity entity, BehaviourGraph<TEntity> value)
		{
			if (HasBehaviour)
			{
				return false;
			}
            this.entity = entity;
			graph = value;
			HasBehaviour = true;
			ResetProcessing();
			return true;
		}

		public void FixBehaviourUpdate()
		{
			if (HasBehaviour && !firstProcessing)
			{
				FixProcessActions();
			}
		}

		public void BehaviourUpdate()
		{
			if (HasBehaviour == false)
			{
				return;
			}

			if (firstProcessing)
			{
				if (!ReferenceEquals(currentState, null))
				{
					BeginAction(currentState);
				}

				firstProcessing = false;
			}

			ProcessActions();
		}

		public void LateBehaviourUpdate()
		{
			if (HasBehaviour && !firstProcessing)
			{
				LateProcessActions();
				ProcessTransitions();
			}
		}

		public bool TryUninstall()
		{
			if (!HasBehaviour)
			{
				return false;
			}

			graph = null;
			HasBehaviour = false;
			return true;
		}

		private void ResetProcessing()
		{
			firstProcessing = true;
			currentState = graph.BeginState;
		}

		private void FixProcessActions()
		{
			int length = currentState.Actions.Length;
			for (int i = 0; i < length; i++)
			{
				currentState.Actions[i].DoFixAction(entity);
			}
		}
		
		private void ProcessActions()
        {
            int length = currentState.Actions.Length;
            for (int i = 0; i < length; i++)
            {
	            currentState.Actions[i].DoAction(entity);
            }
        }

		private void LateProcessActions()
		{
			int length = currentState.Actions.Length;
			for (int i = 0; i < length; i++)
			{
				currentState.Actions[i].DoLateAction(entity);
			}
		}

        private void BeginAction(BehaviourState<TEntity> state)
        {
            int actionCount = state.Actions.Length;
            for (int i = 0; i < actionCount; i++)
            {
                if (entity == null)
                {
                    return;
                }

				BehaviourAction<TEntity> npcAction = currentState.Actions[i];
                npcAction.BeginAction(entity);
            }

            int transitionCount = state.Transitions.Length;
            for (int i = 0; i < transitionCount; i++)
            {
                BehaviourState<TEntity>.Transition transition = state.Transitions[i];
                int decisionCount = transition.Decisions.Length;
                for (int j = 0; j < decisionCount; j++)
                {
                    transition.Decisions[j].BeginDecisionProcessing(entity);
                }
            }
        }

        private void EndAction(BehaviourState<TEntity> state)
        {
            int length = state.Actions.Length;
            for (int i = 0; i < length; i++)
            {
                if (entity == null)
                {
                    return;
                }

				BehaviourAction<TEntity> npcAction = currentState.Actions[i];
                npcAction.EndAction(entity);
            }

            int transitionCount = state.Transitions.Length;
            for (int i = 0; i < transitionCount; i++)
            {
                BehaviourState<TEntity>.Transition transition = state.Transitions[i];
                int decisionCount = transition.Decisions.Length;
                for (int j = 0; j < decisionCount; j++)
                {
                    transition.Decisions[j].EndDecisionProcessing(entity);
                }
            }
        }

        private void ProcessTransitions()
        {
            if (ReferenceEquals(currentState, null))
            {
                throw new ArgumentException("Current state is null: behaviourType ", $"behaviourGraph: {graph.name}");
            }

            if (currentState.Transitions == null)
            {
                throw new ArgumentException("Transitions is null: behaviourType ", $"behaviourGraph: {graph.name}");
            }

            if (ReferenceEquals(entity, null))
            {
                return;
            }

            int length = currentState.Transitions.Length;
            for (int i = 0; i < length; i++)
            {

                BehaviourState<TEntity>.Transition transition = currentState.Transitions[i];
                if (transition == null)
                {
                    throw new ArgumentException("Transition is null: behaviourType ", $"behaviourGraph: {graph.name} index: {i}");
                }

                if (transition.Decisions == null)
                {
                    throw new ArgumentException("Transition decisions is null: behaviourType ", $"behaviourGraph: {graph.name} index: {i}");
                }

                bool decisionResult = true;

                for (int j = 0; j < transition.Decisions.Length; j++)
                {
					BehaviourDecision<TEntity> decision = transition.Decisions[j];
                    if (decision == null)
                    {
                        throw new ArgumentException("Decision is null: behaviourType ", $"behaviourGraph: {graph.name} desicion index: {j}");
                    }

                    if (!decision.GetDecision(entity))
                    {
                        decisionResult = false;
                        break;
                    }
                }

                if (decisionResult)
                {
                    if (TransitionToState(transition.TrueState))
                    {
                        return;
                    }
                }
                else
                {
                    if (TransitionToState(transition.FalseState))
                    {
                        return;
                    }
                }
            }
        }

        private bool TransitionToState(BehaviourState<TEntity> nextState)
        {
            if (ReferenceEquals(nextState, null))
            {
                return false;
            }

            if (!ReferenceEquals(currentState, null))
            {
                EndAction(currentState);
            }

            currentState = nextState;
            if (!ReferenceEquals(currentState, null))
            {
                BeginAction(currentState);
            }

            return true;
        }

		private void OnEnable()
		{
			if (!HasBehaviour)
			{
				return;
			}

			ResetProcessing();
		}

		private void OnDisable()
		{
			if (!ReferenceEquals(currentState, null))
			{
				EndAction(currentState);
			}
		}

        public override string ToString()
        {
            return /*base.ToString() + */$"CurrentState: {currentState.name}";
        }
    }
}
