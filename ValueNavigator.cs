using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Lupan
{
	public class ValueNavigator : MonoBehaviour
	{
	
		[SerializeField] TextMeshProUGUI valueText;
		[SerializeField] Button plusBtn, minusBtn;

		public Action onValueChanged;

		int _value = 1, _min = 1, _max = 1;

		public int Value {
			get{ return Mathf.Clamp (_value, _min, _max); }
			set {
				_value = value;
				_value = Mathf.Clamp (_value, _min, _max);
				UpdateUI ();
				if (onValueChanged != null)
					onValueChanged ();
			}
		}

		public int Min {
			get{ return _min; }
			set {
				_min = value;
				Value = _value;
				UpdateUI ();
			}
		}

		public int Max {
			get{ return _max; }
			set {
				_max = value;
				Value = _value;
				UpdateUI ();
			}
		}

		void Awake ()
		{
			plusBtn.onClick.RemoveAllListeners ();
			plusBtn.onClick.AddListener (() => {
				Value++;
			});


			minusBtn.onClick.RemoveAllListeners ();
			minusBtn.onClick.AddListener (() => {
				Value--;
			});
			UpdateUI ();
		}

		void UpdateUI ()
		{
			valueText.text = Value + "";
		}

		public void setupValueNavigator (int minmum, int maximum, Action onChangeAction)
		{
			onValueChanged = null;
			Min = minmum;
			Max = maximum;
			onValueChanged = onChangeAction;
			Value = minmum;
		}
	}
}