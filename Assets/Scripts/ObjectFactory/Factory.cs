using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Factory
{
	static Factory()
	{
		ObjectFactory.componentWasAdded += ComponentWasAdded;
	}

	static public void ComponentWasAdded(Component com)
	{
		Debug.Log(com);
	}
}
