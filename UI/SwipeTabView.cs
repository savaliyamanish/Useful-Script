using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HedgehogTeam.EasyTouch;

namespace MS
{
	[RequireComponent (typeof(QuickSwipe))]
	public class SwipeTabView : MonoBehaviour
	{
		public Canvas parentCanvas;
		public ToggleGroup toggleGroup;
		public List<TabDetail> tabList;

		[Header ("Tab Swipe Setting")]
		public int currentPage = 0;
		public float pageGape = 1300f;
		public float tweenTime = .4f;
		public AnimationCurve tweenCurve;

		bool isMoving {
			get { 
				return LeanTween.isTweening (gameObject);
			}
		}

		void Start ()
		{
			ResetChildPosition ();

			GetComponent<QuickSwipe> ().enablePickOverUI = true;
			GetComponent<QuickSwipe> ().allowSwipeStartOverMe = true;
			GetComponent<QuickSwipe> ().actionTriggering = QuickSwipe.ActionTriggering.End;
			GetComponent<QuickSwipe> ().swipeDirection = QuickSwipe.SwipeDirection.Horizontal;
			GetComponent<QuickSwipe> ().onSwipeAction.AddListener (OnSwipe);

			setPage (0);
		}

		void ResetChildPosition ()
		{
			float x = 0f;
			for (int i = 0; i < tabList.Count; i++) {
			
				tabList [i].container.localPosition = new Vector2 (x, tabList [i].container.localPosition.y);
				x += pageGape;

				if (tabList [i].toggle != null) {
					tabList [i].toggle.isOn = false;
					tabList [i].toggle.group = toggleGroup;
					tabList [i].toggle.onValueChanged.RemoveAllListeners ();
					int index = i;
					tabList [i].toggle.onValueChanged.AddListener ((arg0) => {
						if (arg0) {
							setPage (index);
						}
					});

				}
			}
		}

		public void OnSwipe (Gesture g)
		{
			if (!parentCanvas.enabled)
				return;
			if (g.swipe == EasyTouch.SwipeDirection.Right && !isMoving)
				PrePage ();
			else if (g.swipe == EasyTouch.SwipeDirection.Left && !isMoving)
				NextPage ();
		}

		public void NextPage ()
		{
			setPage ((currentPage + 1) == tabList.Count ? currentPage : currentPage + 1);
		}

		public void PrePage ()
		{
			setPage (currentPage == 0 ? 0 : (currentPage - 1));
		}

		public void setPage (int i)
		{
			currentPage = Mathf.Clamp (i, 0, tabList.Count);
			if (tabList [currentPage].toggle != null) {
				tabList [currentPage].toggle.isOn = true;
			}
			float target = currentPage * -pageGape;
			float diff = Mathf.Abs (target - transform.localPosition.x);
			float t = diff * tweenTime / pageGape;
			LeanTween.moveLocalX (gameObject, target, t).setEase (tweenCurve);
		}
	}

	[System.Serializable]
	public class TabDetail
	{
		public Toggle toggle;
		public RectTransform container;
	}
}