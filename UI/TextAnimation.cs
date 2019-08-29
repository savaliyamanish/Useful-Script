using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimation : MonoBehaviour {
	[Range(0.01f,2f)]
	public float speed=0.2f;
	public List<string> textList;
	TextMeshProUGUI lbl;
	Coroutine updateCo;
	void Start () {
		lbl = GetComponent<TextMeshProUGUI> ();
		PlayAnimtion ();
	}
	public void PlayAnimtion()
	{
		updateCo=StartCoroutine (updateText());
	}
	public void StopAnimtion()
	{
		StopCoroutine (updateCo);
	}
	IEnumerator updateText()
	{
		if (textList.Count > 0) 
		{
			int i = 0;
			while (true) {
				lbl.text = textList [i];
				yield return new WaitForSeconds (speed);
				i = (i + 1) % textList.Count;
			}
		
		}
	}
}
