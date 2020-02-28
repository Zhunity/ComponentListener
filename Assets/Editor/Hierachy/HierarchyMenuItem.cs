using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine.SceneManagement;

namespace SMFrame.Editor
{
	/// <summary>
	/// Hierarchy的右键菜单
	/// TODO 可参考UnityResourceReference  SceneHierarchy.AddCreateGameObjectItemsToMenu
	/// </summary>
	public class HierarchyMenuItem
	{
		[MenuItem("GameObject/UI/Image")]
		static void CreatImage()
		{
			AddUI.InstanceImage();
		}

		[MenuItem("GameObject/UI/Text")]
		static void CreatText()
		{
			AddUI.InstanceText();
		}
	}
}
