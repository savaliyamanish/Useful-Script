using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MS
{
	public class Popup : MonoBehaviour {
		public static Popup current;

		[Header("Anim Setting")]
		public Animator animator;
		public AnimationClip openClip,closeClip;
		[Header("Popup Setting")]
		public bool isOpen;
		public bool closeOnEsc;
		bool isPlaying;
		public UnityEvent onClose,onOpen;
		void Start () 
		{
			AnimationEvent onOpenStop=new AnimationEvent();
			onOpenStop.functionName="onStopAnim";
			onOpenStop.time=openClip.length;
			openClip.AddEvent (onOpenStop);

			AnimationEvent onCloseStop=new AnimationEvent();
			onCloseStop.functionName="onStopAnim";
			onCloseStop.time = openClip.length;
			closeClip.AddEvent (onCloseStop);

			if (isOpen) {
				Open ();	
			} else {
				Close ();	
			}
		}
		[ContextMenu("Open")]
		public void Open(bool onEnd=false)
		{
			if (isPlaying || isOpen)
				return;
			
			current = this;

			isOpen = true;
			isPlaying = true;
			animator.Play (openClip.name);
			if (onEnd) 
			{
				onOpen.Invoke ();
			}
		}
		[ContextMenu("Close")]
		public void Close(bool onEnd=false)
		{
			if (isPlaying || !isOpen)
				return;
			
			isPlaying = true;
			animator.Play (closeClip.name);
			isOpen = false;
			if (onEnd) 
			{
				onClose.Invoke ();
			}
		}
		[ContextMenu("Open")]
		public void Open()
		{
			Open (false);
		}
		[ContextMenu("Close")]
		public void Close()
		{
			Close (false);
		}
		public void onStopAnim()
		{
			isPlaying = false;
		}

	}
}