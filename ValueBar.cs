using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lupan
{
	[ExecuteInEditMode]
	public class ValueBar : MonoBehaviour
	{

		public Image backGroundImage, fillImage, iconImage;
		public Sprite[] icons;
		[Range (0f, 1f)][SerializeField]
		private float _value = 1;
		[Range (0f, 1f)][SerializeField]
		private float iconTotalValue = 1f;

		/// <summary>
		/// Gets or sets the value between 0-1.
		/// </summary>
		/// <value>Slider Value.</value>
		public float value {
			set {
				_value = value;
				UpdateUI ();
			}
			get{ return _value; }
		}

		/// <summary>
		/// Color Gredient of ValueBar
		/// </summary>
		public Gradient barColor, iconColor;

		public Image AlertImage;
		[Range (0f, 1f)]
		public float alertLimit;

		void Start ()
		{
			UpdateUI ();
		}
		#if UNITY_EDITOR
		void Update ()
		{
			UpdateUI ();
		}
		#endif
		public void UpdateUI ()
		{
			//	backGroundImage.color = new Color (barColor.Evaluate (_value).r,  barColor.Evaluate (_value).g,  barColor.Evaluate (_value).b, _backGroundAlpha);
			fillImage.color = barColor.Evaluate (_value);
			fillImage.fillAmount = _value;
			if (icons.Length > 0) {
				int i = Mathf.CeilToInt (_value / iconTotalValue * icons.Length - 1);
				iconImage.sprite = icons [Mathf.Clamp (i, 0, icons.Length - 1)];
				iconImage.color = iconColor.Evaluate (_value);
			}
			if (AlertImage != null)
				AlertImage.gameObject.SetActive (_value <= alertLimit);
		
		}
	}
}