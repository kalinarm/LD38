using UnityEngine;
using System.Collections;

public static class UnityExtensions
{

	public static void ResetLocal (this Transform self)
	{
		self.localPosition = Vector3.zero;
		self.localRotation = Quaternion.identity;
		self.localScale = Vector3.one;
	}

	public static void setFullSize (this RectTransform self)
	{
		self.ResetLocal ();
		self.anchorMin = Vector2.zero;
		self.anchorMax = Vector2.one;
		self.offsetMin = Vector2.zero;
		self.offsetMax = Vector2.zero;
	}
}