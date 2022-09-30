using System.IO;
using UnityEditor;

namespace BehaviourSystem.Editor
{
	public class BehaviourGraphModificationProcessor<TEntity> : AssetModificationProcessor
	{
		private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
		{
			if (!(AssetDatabase.LoadMainAssetAtPath(sourcePath) is BehaviourGraph<TEntity> npcBehaviourGraph))
			{
				return AssetMoveResult.DidNotMove;
			}

			string sourceDirectory = Path.GetDirectoryName(sourcePath);
			string destinationDirectory = Path.GetDirectoryName(destinationPath);

			if (sourceDirectory != destinationDirectory)
			{
				return AssetMoveResult.DidNotMove;
			}

			string fileName = Path.GetFileNameWithoutExtension(destinationPath);
			npcBehaviourGraph.name = fileName;
			return AssetMoveResult.DidNotMove;
		}
	}
}
