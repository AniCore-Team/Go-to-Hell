using XNode;

namespace BehaviourSystem
{
	public class BehaviourGraph<TEntity> : NodeGraph
	{
		public BehaviourState<TEntity> BeginState = null;

		#if UNITY_EDITOR
		private void Awake()
		{
			string path = UnityEditor.AssetDatabase.GetAssetPath(this);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
			name = fileName;
		}
		#endif
	}
}
