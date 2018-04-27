using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace MS
{
	[ExecuteInEditMode]
	public class SlideNavigator : MonoBehaviour
	{
		///Text Box where Value Display
		public TextMeshProUGUI displayText;
		///Navigation Button
		public Button NextButton, PrevButton;
		/// <summary>
		/// List Of options
		/// </summary>
		public List<string> options;
		/// <summary>
		/// On Option change
		/// </summary>
		public UnityEvent onValueChanged;

		public int value {
			get{ return options.IndexOf (_current); }
			set {
				if (value >= 0 && value < options.Count) {
					_current = options [value];
					OnChangeRun ();
				}
			}
		}

		private string _current;

		void Awake ()
		{
			if (options.Count == 0)
				options.Add ("");
			value = 0;
			NextButton.onClick.AddListener (() => {
				Next ();
			});
			PrevButton.onClick.AddListener (() => {
				Prev ();
			});
		}

		public void ClearOptions ()
		{
			options.Clear ();
		}

		public void OnChangeRun ()
		{
			if (gameObject.activeSelf) {
				RefreshView ();
				onValueChanged.Invoke ();
			}
		}

		public void Next ()
		{
			value = (value + 1) < options.Count ? value + 1 : 0;
		}

		public void Prev ()
		{
			value = (value - 1) >= 0 ? value - 1 : options.Count - 1;
		}

		public void RefreshView ()
		{
			displayText.text = _current;

		}

	}
}