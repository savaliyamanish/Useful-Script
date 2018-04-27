using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MS
{
	[RequireComponent (typeof(VerticalLayoutGroup))]
	[RequireComponent (typeof(ContentSizeFitter))]
	public class VerticalAdvantageContent : MonoBehaviour
	{

		enum Direction
		{
			TOP,
			DOWN
		}

		[Header ("ScrollRect Setting")]
		public ScrollRect scrollRect;
		[Range (5, 15)]
		public int maximumObject = 10;
	
		public GameObject _prefab;
		public int addElement = 3;
		[Range (.5f, 1f)]
		public float TopCreateLimit = 0.8f;

		[Range (0f, .5f)]
		public float DownCreateLimit = 0.2f;
		public int poolSize = 20;
   
    
		[Header ("Paging Setting")]
    
		[Range (0, -.5f)]
		public float PullDownOffset = 0;
		public GameObject pagingLoader;
  
		public UnityEvent onPageLoadStart;


		public delegate void FillGameObjectType (int i, GameObject temp);

		public FillGameObjectType GameObjectFiller;
    
	
		int TotalElements;
		List<RectTransform> currentObjects;
		RectTransform parrentRect;
		int couterTop = 0;
		int couterDown = 0;

		bool isInit = false;
		bool isPaging;

		public bool isPageLoading {
			get {
				return pagingLoader.activeSelf;
			}
			set {
				if (!value) {
					SetPivot (Vector2.up); 
					pagingLoader.SetActive (value);
					Canvas.ForceUpdateCanvases ();
				} else {
					SetPivot (Vector2.zero); 
					pagingLoader.transform.parent = parrentRect;
					pagingLoader.transform.SetAsLastSibling ();
					pagingLoader.SetActive (value);                
				}
			}
		}

		List<GameObject> objects;

		void Awake ()
		{
			if (scrollRect == null) {
				Debug.LogError ("Set ScrollRect Refrence in Content", this);
			}
			currentObjects = new List<RectTransform> ();
			parrentRect = GetComponent<RectTransform> ();
       
			GetComponent<ContentSizeFitter> ().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			scrollRect.onValueChanged.AddListener (UpdatePos);
			objects = new List<GameObject> ();
		}

		public void Init (int totalElement, FillGameObjectType tempGameObject, bool ispaging = false)
		{
			TotalElements = totalElement;
			GameObjectFiller = tempGameObject;
			for (int i = 0; i < totalElement && i < maximumObject; i++) {
				CreateNewElement (Direction.DOWN);
			}
			isInit = true;
			isPaging = ispaging;
			if (isPaging && (pagingLoader == null)) {
				Debug.Log ("Set PagingLoader", gameObject);
			}
		}

		public void PageLoadCompleted (int totalElement, bool isnextPage = false)
		{
      
			TotalElements = totalElement;
			UpdatePos (Vector2.zero);
			UpdatePos (Vector2.zero);
			UpdatePos (Vector2.zero);
			UpdatePos (Vector2.zero);
			isPageLoading = false;
			isPaging = isnextPage;
		}

		public void RemoveAll ()
		{
			if (currentObjects == null)
				currentObjects = new List<RectTransform> ();
			for (int i = currentObjects.Count - 1; i >= 0; i--)
				DestroyGameObject (currentObjects [i].gameObject);
		
			isInit = false;
			currentObjects = new List<RectTransform> ();
			couterTop = 0;
			couterDown = 0;
		}

		void Update ()
		{
			scrollRect.velocity = new Vector2 (0, Mathf.Clamp (scrollRect.velocity.y, -3000, 3000));
			if (Mathf.Abs (scrollRect.velocity.y) <= 10 && !Input.GetMouseButton (0)) {
				scrollRect.velocity = Vector2.zero;    
				if (scrollRect.verticalNormalizedPosition > 0.6) {
					RemoveElement (Direction.DOWN);
				} else if (scrollRect.verticalNormalizedPosition < 0.4) {
					RemoveElement (Direction.TOP);                
				}
			}
		}

		void RemoveElement (Direction dir)
		{
			if (currentObjects.Count <= maximumObject)
				return;

			if (dir == Direction.DOWN) {
				SetPivot (Vector2.up); 
				while (currentObjects.Count > maximumObject) {                        
					DestroyGameObject (currentObjects [currentObjects.Count - 1].gameObject);
					currentObjects.RemoveAt (currentObjects.Count - 1);   
					couterDown--;    
				}
				Canvas.ForceUpdateCanvases ();
			} else {
				SetPivot (Vector2.zero);    
				while (currentObjects.Count > maximumObject) {
                    
					DestroyGameObject (currentObjects [0].gameObject);
					currentObjects.RemoveAt (0);
					couterTop++;         
				}
				Canvas.ForceUpdateCanvases ();
			}
        
		}

		public void SetPivot (Vector2 pivot)
		{
			Vector2 size = parrentRect.rect.size;
			Vector2 deltaPivot = parrentRect.pivot - pivot;
			Vector3 deltaPosition = new Vector3 (deltaPivot.x * size.x, deltaPivot.y * size.y);
			parrentRect.pivot = pivot;
			parrentRect.localPosition -= deltaPosition;
		}

		void UpdatePos (Vector2 v)
		{
			//if(!isInit || currentObjects.Count <maximumObject)
			if (!isInit)
				return;


			if (scrollRect.verticalNormalizedPosition > TopCreateLimit) {
				for (int i = 0; i < addElement; i++) {
					if (couterTop > 0)
						CreateNewElement (Direction.TOP);
				}
			} else if (scrollRect.verticalNormalizedPosition < DownCreateLimit) {
				for (int i = 0; i < addElement; i++) {
					if (couterDown < TotalElements)
						CreateNewElement (Direction.DOWN);
                
				}    
				if (scrollRect.verticalNormalizedPosition < PullDownOffset) {
					if (isPaging) {
						isPageLoading = true;
						isPaging = false;
						onPageLoadStart.Invoke ();
					}
				}
			}
          
        
		}

		void CreateNewElement (Direction dir)
		{
        
			Vector2 velocity = scrollRect.velocity;

			if (dir == Direction.DOWN) {
            
				GameObject temp = createGameObject ();
				GameObjectFiller (couterDown, temp);	       

				couterDown++;

            
				SetPivot (Vector2.up);
				RectTransform tempRect = temp.GetComponent<RectTransform> ();
				tempRect.SetAsLastSibling ();
				temp.SetActive (true);            
				currentObjects.Add (tempRect);
            
            
			} else {
			
				couterTop--;
				GameObject temp = createGameObject ();
				GameObjectFiller (couterTop, temp);			
				RectTransform tempRect = temp.GetComponent<RectTransform> ();
            
				SetPivot (Vector2.zero);           
				tempRect.SetAsFirstSibling ();    
				temp.SetActive (true);
				currentObjects.Insert (0, tempRect);
			}
		}



		GameObject createGameObject ()
		{
			GameObject temp;
			if (objects.Count > 0) {
				temp = objects [0];      
				objects.RemoveAt (0);
			} else {
				temp = Instantiate<GameObject> (_prefab, Vector3.zero, Quaternion.Euler (0, 0, 0), parrentRect);
			}
			return temp;
		}

		void DestroyGameObject (GameObject temp)
		{
			if (temp == null)
				return;
		
			if (objects.Count > poolSize) {
				Destroy (temp);
			} else {
				temp.SetActive (false);
				objects.Add (temp);
			}
		}


	}
}