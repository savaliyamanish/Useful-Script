using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Lupan
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(Image))]
	public class UISpriteAnimation : MonoBehaviour
	{
		public int frameIndex = 0;

		[SerializeField] protected int FPS = 30;
		[SerializeField] protected bool Loop = true;

		protected Image ImageUI;
		protected float mDelta = 0f;
		protected bool mActive = true;
		[SerializeField] protected List<Sprite> Sprites = new List<Sprite> ();

		public int frames { get { return Sprites.Count; } }

		public int framesPerSecond { get { return FPS; } set { FPS = value; } }

		public bool loop { get { return Loop; } set { Loop = value; } }

		public bool isPlaying { get { return mActive; } }

		void Start ()
		{
			ImageUI = GetComponent<Image> ();
		}

		void Update ()
		{
			if (mActive && Sprites.Count > 1 && Application.isPlaying && FPS > 0) {
				mDelta += Mathf.Min (1f, Time.deltaTime);
				float rate = 1f / FPS;

				while (rate < mDelta) {
					mDelta = (rate > 0f) ? mDelta - rate : 0f;

					if (++frameIndex >= Sprites.Count) {
						frameIndex = 0;
						mActive = Loop;
					}

					if (mActive) {
						ImageUI.sprite = Sprites [frameIndex];
					}
				}
			}
		}


		public void Play ()
		{
			mActive = true;
		}


		public void Pause ()
		{
			mActive = false;
		}

		public void ResetToBeginning ()
		{
			mActive = true;
			frameIndex = 0;

			if (ImageUI != null && Sprites.Count > 0) {
				ImageUI.sprite = Sprites [frameIndex];
			}
		}
	}
}