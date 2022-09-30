namespace BehaviourSystem
{
    public interface IBehaviourController<TEntity>
    {
		bool HasBehaviour { get; }
		
		bool TryInstall(TEntity entity, BehaviourGraph<TEntity> value);
		void FixBehaviourUpdate();
		void BehaviourUpdate();
		void LateBehaviourUpdate();
		bool TryUninstall();
	}
}
