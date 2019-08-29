using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class IndexChangeEvent : UnityEvent<int>
{
}
[ExecuteInEditMode][RequireComponent(typeof(ScrollRect))]
public class CenterInScroll : MonoBehaviour,IBeginDragHandler, IEndDragHandler
{
    public ScrollRect  scrollRect;
     #if UNITY_EDITOR
    [Range(0f,1f)]
    public float normalizeValue=0;
    #endif
        
    [Range(0.0f,500f)]
    public float velocityDiff=100f;
    [Range(0.01f,1f)]
    public float movingSpeed=0.1f;

    [Range(0f,1f)]
    public List<float> itemPoints;
    public IndexChangeEvent OnGetFocus;
    public IndexChangeEvent OnLostFocus;
    float scrollNoramalPos
    {
        get{
            if(scrollRect.horizontal)
                return scrollRect.horizontalNormalizedPosition;
            else
                return scrollRect.verticalNormalizedPosition;
         }
        set{
            if(scrollRect.horizontal)
                scrollRect.horizontalNormalizedPosition=value;
            else
                scrollRect.verticalNormalizedPosition=value;
                
        }
    }
    int lastPage=-1;
    bool isDrag;
    public int currentIndex
    {
        get
        {
            
             int index=0;
             float diff=1;
             for(int i=0;i<itemPoints.Count;i++)
            {

                if(Mathf.Abs(scrollNoramalPos-itemPoints[i]) <diff)
                {
                    diff=Mathf.Abs(scrollNoramalPos-itemPoints[i]);
                    index=i;
                }
            }
            return index;
          
        }
    }
    void Start() {
       setOnCenter();


    }
    void Update()
    {
        #if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlaying)
            {

                if(scrollRect==null)
                {
                    scrollRect=GetComponent<ScrollRect>();
                    
                }
                if(itemPoints.Count==0)
                {
                    itemPoints.Add(0f);
                    itemPoints.Add(1f);
                }
                itemPoints.Sort();     
               
                scrollNoramalPos=normalizeValue;
            }
            else if(!isDrag)
            {
                if(( scrollRect.horizontal && Mathf.Abs(scrollRect.velocity.x)<=velocityDiff&&scrollNoramalPos!=itemPoints[currentIndex]) || 
                (scrollRect.vertical && Mathf.Abs(scrollRect.velocity.y)<=velocityDiff &&scrollNoramalPos!=itemPoints[currentIndex]))
                {
                    setOnCenter();
                }
            }
        #else
        if(!isDrag)
        {
               if(( scrollRect.horizontal && Mathf.Abs(scrollRect.velocity.x)<=velocityDiff&&scrollNoramalPos!=itemPoints[currentIndex]) || 
                (scrollRect.vertical && Mathf.Abs(scrollRect.velocity.y)<=velocityDiff &&scrollNoramalPos!=itemPoints[currentIndex]))
                {
                    setOnCenter();
                }
                
            }
        #endif
    }
    void setOnCenter()
    {
        if(scrollNoramalPos<itemPoints[currentIndex])
        {

            if(scrollNoramalPos+(Time.deltaTime*movingSpeed) > itemPoints[currentIndex])
            {
                scrollNoramalPos=itemPoints[currentIndex];
                 if(lastPage!=currentIndex)
                    {
                        if(lastPage!=-1)
                        {
                            OnLostFocus.Invoke(lastPage);
                        }
                        OnGetFocus.Invoke(currentIndex);
                    }
                    lastPage=currentIndex;
            }
            else
            {
                scrollNoramalPos+=(Time.deltaTime*movingSpeed);
            }        
        }
        else
        {
            if(scrollNoramalPos-(Time.deltaTime*movingSpeed) < itemPoints[currentIndex])
            {
               if(lastPage!=currentIndex)
                    {
                        if(lastPage!=-1)
                        {
                            OnLostFocus.Invoke(lastPage);
                        }
                        OnGetFocus.Invoke(currentIndex);
                    }
                     lastPage=currentIndex;
            }
            else
            {
                scrollNoramalPos-=(Time.deltaTime*movingSpeed);
            }  
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      isDrag=true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag=false;
    }
}
