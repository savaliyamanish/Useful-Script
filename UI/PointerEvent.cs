using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace MS
{
public class PointerEvent : MonoBehaviour, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler {
	[Header("Pointer Enter")]
	public UnityEvent onEnter;

	[Header("Pointer Hover")]
	public UnityEvent onHover;

	[Header("Pointer Exit")]
	public UnityEvent onExit;


	[Header("Pointer Up Event")]
	public UnityEvent onLeftUp;
	public UnityEvent onRightUp;
	[Header("Pointer Donw Event")]
	public UnityEvent onLeftDown;
	public UnityEvent onRightDown;

	[Header("Pointer Click Event")]
	public UnityEvent onLeftClick;
	public UnityEvent onRightClick;

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) 
		{
			onLeftUp.Invoke ();
		}
		if (eventData.button == PointerEventData.InputButton.Right) 
		{
			onRightUp.Invoke ();
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) 
		{
			onLeftDown.Invoke ();
		}
		if (eventData.button == PointerEventData.InputButton.Right) 
		{
			onRightDown.Invoke ();
		}
	}
	public void OnPointerClick (PointerEventData eventData) 
	{
		if (eventData.button == PointerEventData.InputButton.Left) 
		{
			onLeftClick.Invoke ();
		}
		if (eventData.button == PointerEventData.InputButton.Right) 
		{
			onRightClick.Invoke ();
		}
	}
	bool isHover=false;
	public void OnPointerEnter (PointerEventData eventData) 
	{
		onEnter.Invoke ();
		isHover = true;
	}
	public void OnPointerExit (PointerEventData eventData) 
	{
		onExit.Invoke ();
		isHover = false;

	}
	void FixedUpdate()
	{
		if (isHover) {
			onHover.Invoke ();
		}
	}
}
}