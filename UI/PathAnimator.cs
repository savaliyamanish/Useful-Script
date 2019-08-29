using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimator : MonoBehaviour
{
    [Range(0f,1f)]
    public float value=0f;
     public Animator anim;
     public string pathAnimationName;
     void Start()
     {
         anim.speed = 0;          
     }
     void FixedUpdate()
     {
         anim.Play(pathAnimationName, -1, 1-value);
     }
}
