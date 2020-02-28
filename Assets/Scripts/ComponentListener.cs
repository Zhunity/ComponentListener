using SMFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace SMFrame
{
	/// <summary>
	/// InspectorWindow.AddComponentButton打开选择Component下拉框
	/// UnityResourceReference AddComponentGUI是添加代码用的
	/// AddComponentWindow.Show 打开
	/// AddComponentWindow.OnItemSelected 目测时这个选中
	/// AddComponentWindow.
	/// </summary>
	[ExecuteInEditMode]
	public class ComponentListener : MonoBehaviour
	{
#if UNITY_EDITOR
		#region Common 通用逻辑
		Type window = GetType("AddComponentWindow");
		EventInfo selectionChanged;
		EventInfo windowClosed;
		Delegate closedDelegate;
		Delegate selectDelegate;
		const string COMPONENT_ITEM_TYPE_NAME = "ComponentDropdownItem";
		Type componentItem = GetType(COMPONENT_ITEM_TYPE_NAME);
		PropertyInfo displayName;

		private bool isClear = false;

		private void Awake()
		{
			if (DestroyOnPlay())
			{
				return;
			}
			Debug.Log("EditorAwake");

			selectionChanged = window.GetEvent("selectionChanged");
			windowClosed = window.GetEvent("windowClosed");
			closedDelegate = CreateDelegate(windowClosed.EventHandlerType, "WindowClosed");
			selectDelegate = CreateDelegate(selectionChanged.EventHandlerType, "ItemSelect");
			displayName = componentItem.GetProperty("name");


			// 感觉用Mono的Update频率比较慢
			EditorApplication.update += EditorUpdate;
		}


		private void EditorUpdate()
		{
			if (DestroyOnPlay())
			{
				return;
			}
			var windows = Resources.FindObjectsOfTypeAll(window);
			if (windows != null && windows.Length > 0)
			{
				if (isClear)
				{
					return;
				}
				isClear = true;
				foreach (var item in windows)
				{
					windowClosed.AddEventHandler(item, closedDelegate);
					selectionChanged.AddEventHandler(item, selectDelegate);
				}
			}
			else
			{
				isClear = false;
			}
		}

		public void WindowClosed(Object para)
		{
			isClear = false;
		}

		private void OnDestroy()
		{
			Debug.Log("destroy");

			EditorApplication.update -= EditorUpdate;
			var windows = Resources.FindObjectsOfTypeAll(window);
			if (windows != null && windows.Length > 0)
			{
				foreach (var item in windows)
				{
					windowClosed.RemoveEventHandler(item, closedDelegate);
					selectionChanged.RemoveEventHandler(item, selectDelegate);
				}
			}
		}

		private bool DestroyOnPlay()
		{
			if (Application.isPlaying)
			{
				DestroyImmediate(gameObject);
				return true;
			}
			return false;
		}

		private Delegate CreateDelegate(Type type, string name)
		{
			Type t = this.GetType();
			var method = t.GetMethod(name);
			return method.CreateDelegate(type, this);
		}

		/// <summary>
		/// 获取类型
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns></returns>
		public static Type GetType(string typeName)
		{
			Type type = null;
			Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
			int assemblyArrayLength = assemblyArray.Length;
			for (int i = 0; i < assemblyArrayLength; ++i)
			{
				type = assemblyArray[i].GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}

			for (int i = 0; (i < assemblyArrayLength); ++i)
			{
				Type[] typeArray = assemblyArray[i].GetTypes();
				int typeArrayLength = typeArray.Length;
				for (int j = 0; j < typeArrayLength; ++j)
				{
					if (typeArray[j].Name.Equals(typeName))
					{
						return typeArray[j];
					}
				}
			}
			return type;
		}
		#endregion

		/// <summary>
		/// 选中要添加的组件
		/// </summary>
		/// <param name="para"></param>
		public void ItemSelect(Object para)
		{
			if (para.GetType().Name == COMPONENT_ITEM_TYPE_NAME)
			{
				string componentName = (string)displayName.GetValue(para);
				var gos = Selection.gameObjects;
				foreach (var go in gos)
				{
					Component com = go.GetComponent(componentName);
					if (com == null)
					{
						Debug.LogError("can not find com ", go);
						continue;
					}
					switch (componentName)
					{
						case ("Image"):
							{
								ComponentOptimizing.OptimizingImage(com as Image);
								break;
							}
						case ("Text"):
							{
								ComponentOptimizing.OptimizingText(com as Text);
								break;
							}
						case ("Mask"):
							{
								ComponentOptimizing.OptimizingMask(com as Mask);
								break;
							}
						default:
							{
								Debug.Log("add " + componentName);
								break;
							}
					}
				}

				isClear = false;
			}
		}
#endif
	}
}