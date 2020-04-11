using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyGPUInstancingTest : MonoBehaviour
{
    public GameObject ArmyGpuInstancing;
    public GameObject Army;

    public Transform GPUInstancingAnchor;
    public Transform NormalAnchor;
    public int gridX, gridY;

    bool isGPUInstancing = false;

    public Text stateText;
    public Text inputX, inputY;
    public Text actionText, colorText;

    public GameObject BtnAction, BtnColor;


    public Camera cam;

    public float moveSpeed = 1;
    
    Transform _tran;

    Dictionary<int, MeshRenderer> _armyId2Renderer;

    List<GameObject> _armys = new List<GameObject>();
    List<GameObject> _armysInstancing = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _tran = transform;
        _armyId2Renderer = new Dictionary<int, MeshRenderer>();
        GPUInstancingAnchor.gameObject.SetActive(false);
        //NormalAnchor.gameObject.SetActive(false);
        CreateArmyInstancing();
        CreateNormal();

        stateText.text = (isGPUInstancing ? "GPU Instancing" : "Normal") + "   Count:" + gridX * gridY;
    }

    void CreateArmyInstancing()
    {
        int index = 0;
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                GameObject armyInst = Instantiate(ArmyGpuInstancing);
                armyInst.transform.parent = GPUInstancingAnchor;
                armyInst.name = index + " (" + x + "," + y + ")";

                armyInst.transform.localPosition = new Vector3(PosX(x), 0, y * 3);
                _armysInstancing.Add(armyInst);

                if (_offset || _colorDiff)
                {
                    MeshRenderer renderer = armyInst.GetComponentInChildren<MeshRenderer>();
                    MaterialPropertyBlock props = new MaterialPropertyBlock();
                    if (_offset)
                    {
                        props.SetFloat("_AnimLen", Random.Range(0.5f, 2));
                        props.SetFloat("_AnimState", Random.Range(0, 2));
                    }
                    if (_colorDiff)
                    {
                        float r = Random.Range(0.0f, 1.0f);
                        float g = Random.Range(0.0f, 1.0f);
                        float b = Random.Range(0.0f, 1.0f);
                        props.SetColor("_Tint", new Color(r, g, b));
                    }
                    renderer.SetPropertyBlock(props);
                    //_armyId2Renderer.Add(index++, renderer);
                }
            }
        }

        
       
        //for (int i = 0; i < 5; i++)
        //{
        //    MaterialPropertyBlock props = new MaterialPropertyBlock();
        //    props.SetFloat("_AnimState", i % 2);
        //    //props.SetFloat("_AnimLen", Random.Range(0.5f, 2));
        //    _armyId2Renderer[i].SetPropertyBlock(props);
        //}
    }

    int PosX(int index)
    {
        if (index == 0)
        {
            return 0;
        }
        else
        {
            int angleWithoutSign = (((index - 1) >> 1) + 1) * 1;
            return (index & 1) == 1 ? -angleWithoutSign : angleWithoutSign;
        }
    }

    void CreateNormal()
    {
        int index = 0;
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                GameObject armyInst = Instantiate(Army);
                armyInst.name = index + " (" + x + "," + y + ")";
                armyInst.transform.parent = NormalAnchor;
                armyInst.transform.localPosition = new Vector3(PosX(x), 0, y * 3);
                _armys.Add(armyInst);
            }
        }
    }

    
    bool _offset = false;
    bool _colorDiff = false;


    public void OnClickChange()
    {
        if (!GPUInstancingAnchor.gameObject.activeSelf)
        {
            isGPUInstancing = true;
            GPUInstancingAnchor.gameObject.SetActive(true);
            NormalAnchor.gameObject.SetActive(false);
        }
        else
        {
            isGPUInstancing = false;
            GPUInstancingAnchor.gameObject.SetActive(false);
            NormalAnchor.gameObject.SetActive(true);
        }

        stateText.text = (isGPUInstancing ? "GPU Instancing" : "Normal") + "   Count:" + gridX * gridY;

        BtnAction.SetActive(isGPUInstancing);
        BtnColor.SetActive(isGPUInstancing);
    }

    public void OnClickAction()
    {
        _offset = !_offset;

        for (int i = 0; i < _armys.Count; i++)
        {
            Destroy(_armysInstancing[i]);
        }
        _armysInstancing.Clear();
        CreateArmyInstancing();

        if (_offset)
        {
            actionText.text = "关闭动作差异化";
        }
        else
        {
            actionText.text = "开启动作差异化";
        }
    }

    public void OnClickColor()
    {
        _colorDiff = !_colorDiff;

        for (int i = 0; i < _armys.Count; i++)
        {
            Destroy(_armysInstancing[i]);
        }
        _armysInstancing.Clear();
        CreateArmyInstancing();

        if (_colorDiff)
        {
            colorText.text = "关闭颜色差异化";
        }
        else
        {
            colorText.text = "开启颜色差异化";
        }
    }

    public void OnClickRestar()
    {
        for (int i = 0; i < _armys.Count; i++)
        {
            Destroy(_armys[i]);
            Destroy(_armysInstancing[i]);
        }

        _armys.Clear();
        _armysInstancing.Clear();
        gridX = int.Parse(inputX.text);
        gridY = int.Parse(inputY.text);
        CreateArmyInstancing();
        CreateNormal();

        stateText.text = (isGPUInstancing ? "GPU Instancing" : "Normal") + "   Count:" + gridX * gridY;
    }

    Vector3 _lastMousePos = Vector3.zero;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (_lastMousePos == Vector3.zero)
            {
                _lastMousePos = Input.mousePosition;
                return;
            }

            Vector3 curMousePos = Input.mousePosition;
            float xMove = curMousePos.x - _lastMousePos.x;
            float yMove = curMousePos.y - _lastMousePos.y;
            //Debug.LogError(xMove+" "+yMove);

            Vector3 camLastPos = cam.transform.localPosition;
            cam.transform.localPosition = new Vector3(camLastPos.x - xMove * moveSpeed * Time.deltaTime, camLastPos.y, camLastPos.z - yMove * moveSpeed * Time.deltaTime);

            _lastMousePos = curMousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _lastMousePos = Vector3.zero;
        }
    }
}
