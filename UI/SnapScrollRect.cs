using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Lupan
{
	public class SnapScrollRect : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
	{

		public int screens;
		float[] points;
		public int speed = 80;
		public float stepSize;

		ScrollRect scroll;
		bool lerp, isSlideShow;
		float target;
		public int index = 0;
		public Action<int> onPageChanged;

		void Awake ()
		{

			Reset ();
		
		}

		public void Reset ()
		{
			bool lerp = false;
			float target = 0;
			index = 0;
			onPageChanged = null;

			scroll = gameObject.GetComponent<ScrollRect> ();
			screens = scroll.content.childCount;
			if (screens != 0)
				InitPoints (screens);



		}

		public void InitPoints (int _screens)
		{
			screens = _screens;
			points = new float[screens];
   
			if (screens > 1) {
				stepSize = 1 / (float)(screens - 1);

				for (int i = 0; i < screens; i++) {
					points [i] = i * stepSize;
				}
			} else {
				points [0] = 0;
			}
		}

		void Update ()
		{

			if (lerp == false)
				return;
			if (scroll.horizontal) {
				scroll.horizontalNormalizedPosition = Mathf.Lerp (scroll.horizontalNormalizedPosition, target, speed * scroll.elasticity * Time.deltaTime);
				if (Mathf.Approximately (scroll.horizontalNormalizedPosition, target))
					lerp = false;
			} else {
				scroll.verticalNormalizedPosition = Mathf.Lerp (scroll.verticalNormalizedPosition, target, speed * scroll.elasticity * Time.deltaTime);
				if (Mathf.Approximately (scroll.verticalNormalizedPosition, target))
					lerp = false;
			}
		}

		private float beginDragTimer;

		public void OnBeginDrag (PointerEventData eventData)
		{
			beginDragTimer = Time.time;
		}

		private int direction;

		public void OnEndDrag (PointerEventData data)
		{
			if (screens > 0) {
				float dragTime = Time.time - beginDragTimer;
				if (dragTime > 0.15f) {
					MoveParent ();
				} else {
					Invoke ("MoveParent", 0.1f);
				}
			}
		}

		public void MoveParent ()
		{
			index = scroll.horizontal ? FindNearest_SlowSwipe (scroll.horizontalNormalizedPosition, points) :
			scroll.vertical ? FindNearest_SlowSwipe (scroll.verticalNormalizedPosition, points) : index;
			MoveToPage (index);
		}

		public void NextPage ()
		{
			MoveToPage (index + 1);
		}

		public void PreviousPage ()
		{
			MoveToPage (index - 1);
		}

		public void MoveToPage (int pageIndex)
		{
			index = Mathf.Clamp (pageIndex, 0, screens - 1);
			target = points [index];
			lerp = true;

			if (onPageChanged != null)
				onPageChanged (index);
		}

		public void SetPage (int pageIndex)
		{
			index = Mathf.Clamp (pageIndex, 0, screens - 1);
			target = points [index];
			if (scroll.horizontal)
				scroll.horizontalNormalizedPosition = target;
			else
				scroll.verticalNormalizedPosition = target;

      
			if (onPageChanged != null)
				onPageChanged (index);
		}

		public void SideShow (float t)
		{
			StartCoroutine (ShowingSlide (t));
		}

		IEnumerator ShowingSlide (float t)
		{
			if (points != null && points.Length > 0) {
				isSlideShow = true;
				scroll.enabled = false;
				SetPage (screens - 1);
				for (int i = screens - 1; i >= 0; i--) {
					MoveToPage (i);
					yield return new WaitForSeconds (t);
				}
				isSlideShow = false;
				scroll.enabled = true;
			}
		}


		public void OnDrag (PointerEventData data)
		{
			lerp = false;
		}

		int FindNearest_SlowSwipe (float f, float[] array)
		{
			float distance = Mathf.Infinity;
			int output = 0;
			for (int index = 0; index < array.Length; index++) {
				if (Mathf.Abs (array [index] - f) < distance) {
					distance = Mathf.Abs (array [index] - f);
					output = index;
				}
			}
			return output;
		}

		int FindNearest_FastSwipe (float f, float[] array)
		{
			float sigment = array [1] - array [0];
			int output = direction == 1 ? array.Length - 1 : 0;

			for (int i = index; i >= 0 && i < array.Length; i += direction) {
				if (Mathf.Abs (i - index) <= 1 && Mathf.Abs (array [i] - f) < 0.2f * sigment) {
					output = i;
					break;

				} else if (direction == 1 && array [i] - f > 0 || direction == -1 && array [i] - f < 0) {
					output = i;
					break;
				}
			}

			return output;
		}


	}
}