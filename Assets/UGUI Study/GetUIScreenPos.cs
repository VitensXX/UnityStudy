using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Vitens on 2020/11/22 16:21:08
/// 
/// Description : 
///     获取UGUI中Canvas下的UI组件的屏幕坐标
/// </summary>
[ExecuteInEditMode]
public class GetUIScreenPos : MonoBehaviour
{
    public bool printScreenPos;
    //Graphic graphic;

    //screen space - overlay
    void PrintScreenPos()
    {
        Graphic graphic = GetComponent<Graphic>();
        float x = graphic.transform.position.x / Screen.width;
        float y = graphic.transform.position.y / Screen.height;
        Debug.LogError(x+" "+y);
    }

    void GetScreenPos(RenderMode renderMode)
    {
        if(renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Graphic graphic = GetComponent<Graphic>();
            float x = graphic.transform.position.x / Screen.width;
            float y = graphic.transform.position.y / Screen.height;
            Debug.Log(x + " " + y);
        }
        else
        {
            Camera uiCam = GetComponentInParent<Canvas>().worldCamera;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCam, GetComponent<Graphic>().transform.position);
            float x = screenPos.x / Screen.width;
            float y = screenPos.y / Screen.height;
            Debug.Log(x + " " + y);
        }
    }

    void Update()
    {
        if (printScreenPos)
        {
            Debug.LogError(GetUIScreenPosition(GetComponent<Graphic>()).ToString("#.#####"));
            printScreenPos = false;
        }
    }

    //获取UI的屏幕坐标【0,1】
    Vector2 GetUIScreenPosition(Graphic ui)
    {
        //获取到UI所处的canvas
        Canvas canvas = ui.GetComponentInParent<Canvas>();

        //Overlay模式 或者 ScreenSpaceCamera模式没有关联UI相机的情况
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || 
            canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
        {
            float x = ui.transform.position.x / Screen.width;
            float y = ui.transform.position.y / Screen.height;
            return new Vector2(x, y);
        }
        //ScreenSpaceCamera 和 WorldSpace模式  注意WorldSpace没有关联UI相机获取到的就会有问题
        else
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, ui.transform.position);
            float x = screenPos.x / Screen.width;
            float y = screenPos.y / Screen.height;
            return new Vector2(x, y);
        }
    }
}
