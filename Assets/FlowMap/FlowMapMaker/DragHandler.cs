using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject pressTest;
    public GameObject curTest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnBeginDrag(PointerEventData pointerEventData){
        Debug.LogError("OnBeginDrag");
        // Debug.LogError(pointerEventData.pressPosition +"   "+ pointerEventData.delta+"  "+pointerEventData.position);
        pressTest.transform.position = pointerEventData.pressPosition;
        curTest.transform.position = pointerEventData.position;
    }

    public void OnDrag(PointerEventData pointerEventData){
        pressTest.transform.position = pointerEventData.pressPosition;
        curTest.transform.position = pointerEventData.position;
    }


    public void OnEndDrag(PointerEventData pointerEventData){

    }

    // Color GetColor(Vector2 delta){
    //     delta = delta.normalized;
        
    //     //
    //     if(delta.x > 0 && delta.x > Mathf.Abs(delta.y)){

    //     }
    // }

}
