using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;


//支持中文.
/*----------------------------------------------------------------
// Copyright (C) 公司名称 成都微美互动科技有限公司
// 版权所有。  
//
// 文件名：MagicText.cs
// 文件功能描述：文字的动态效果
 
// 创建标识：Created by Vitens On 2019/08/15 11:40:46

// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/

[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class MagicText : BaseMeshEffect
{
    const float INVALID = int.MaxValue - 10;//无效值
    const float CURVE_MAX_TIME = 1;//曲线横坐标右边界
    const float CURVE_MIN_TIME = 0;//曲线横坐标左边界
    public enum Type
    {
        None,       //无
        Normal,     //普通，fadein/out 和 posOffset
        Jump,       //跳跃
        Rotate,     //旋转
        Scale,      //缩放
        Color,      //扫光
        Rainbow,    //彩虹
        Stretch,    //拉伸
        CircleLayout,//布局 圆
    }
    public bool playOnAwake = true;
    public Type type = Type.Jump;
    
    [Tooltip("动画曲线,控制每个字的动画节奏")]
    public AnimationCurve animCurve;
    [Tooltip("透明度变化曲线")]
    public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public PeriodicFunction.Type periodicType = PeriodicFunction.Type.Sawtooth;

    [Tooltip("每个字动画的快慢")]
    public float perTextSpeed = 5;

    [Tooltip("动画频率")]
    public int frequency = 1;

    [Tooltip("衰减参数")]
    public float atten = 0;

    [Tooltip("每个字的动画间隔时间")]
    public float perTextInterval = 0.3f;

    [Tooltip("是否循环")]
    public bool Loop = false;

    [Tooltip("Loop的间隔时间")]
    public float loopInterval = 5f;

    [Tooltip("Loop间隔时间加随机值")]
    public bool isRandomForLoopInterval = false;

    [Tooltip("Loop间隔时间随机因子范围")]
    public float randomRangeForLoopInterval = 0;

    [Tooltip("动画程度因子")]
    public float animFactor = 30;

    [Tooltip("文字颜色(类似于扫光颜色)")]
    public Color textColor = Color.red;

    [Tooltip("是否开启坐标位子偏移")]
    public bool openPosOffset = false;
    [Tooltip("坐标位置偏移")]
    public Vector2 posOffset;

    [Tooltip("是否需要淡入效果")]
    public bool isFadein = false;

    [Tooltip("顺序随机化")]
    public bool randomOrder = false;

    [Tooltip("圆心布局半径")]
    public float radius;

    public float textInterval;
    public float textIntervalFactor;

    public bool IsStart { get; private set; } = false;

    Type _lastType;
    float _randomForLoopInterval = 0;
    bool _isFadeIn;
    bool _isFadeout;
    bool _openPosOffset;
    Vector2 _posOffset;
    Vector3 _circleCenter;
    Vector3 _textCenter;
    
    public void Play()
    {
        if (type == Type.Rainbow)
        {
            frequency = 8;
            periodicType = PeriodicFunction.Type.Linear01;
        }
        _lastType = type;

        _openPosOffset = openPosOffset;
        _posOffset = posOffset;
        _isFadeIn = isFadein;
        _isFadeout = false;
        IsStart = true;
        _onceEnd = false;
        _originColor = _text.color;
        _isFirst = true;
    }

    //淡出 和淡入反着来
    public void Fadeout()
    {
        //loop模式不支持淡出操作
        if (Loop)
        {
            Debug.LogError("loop模式不支持淡出操作");
            return;
        }

        IsStart = true;
        _isFadeout = true;
        _onceEnd = false;

        _openPosOffset = openPosOffset;
        _posOffset = posOffset;
        _isFadeIn = isFadein;
        _posOffset = posOffset * new Vector2(-1, 1);
        _x = 1;
    }

    //普通的淡出动画
    public void FadeoutNormal(float duration = 0.3f)
    {
        
    }

    //暂停动画
    public void Pause()
    {
        IsStart = false;
    }

    //停止动画
    public void Stop()
    {
        _isFadeIn = false;
        IsStart = false;
        _openPosOffset = false;
        _x = 0;

        ClearPosRecord();
        graphic.SetVerticesDirty();
    }

    //重新播放
    public void Replay()
    {
        Stop();
        Play();
    }

    #region internal

    float _x;
    
    bool _onceEnd;//所有文字完成一轮动画
    float _tickForWait = 0;//等待计时器
    private void Update()
    {
#if UNITY_EDITOR
        //参数合法性检测
        if (!Application.isPlaying)
        {
            CheckCurve();
            CheckLoop();
        }
#endif

        if (!IsStart)
            return;

        if (_lastType != type)
        {
            Stop();
            return;
        }

        if (Loop)
        {
            if (_onceEnd)
            {
                _tickForWait += Time.deltaTime;
                if(_tickForWait >= loopInterval + _randomForLoopInterval)
                {
                    _onceEnd = false;
                    _x = 0;
                    _tickForWait = 0;
                }
            }
        }
        else
        {
            if (_onceEnd)
            {
                IsStart = false;
                _isFadeIn = false;
                return;
            }
        }

        if (_isFadeout)
        {
            _x -= Time.deltaTime * perTextSpeed;
        }
        else
        {
            _x += Time.deltaTime * perTextSpeed;
        }
        graphic.SetVerticesDirty();
    }

    Text _text;
    protected override void OnEnable()
    {
        base.OnEnable();
        _text = GetComponent<Text>();
        _lastType = type;

        if (playOnAwake && Application.isPlaying)
        {
            Play();
        }
    }

    bool _isFirst = true;

    List<Vector3> _verticesOriPos;
    List<Vector3> _centerPos;
    List<int> _randomOrderList;
    int _verticesCount;
    public override void ModifyMesh(VertexHelper helper)
    {
        if (!IsActive() || helper.currentVertCount == 0)
            return;
        _verticesCount = helper.currentVertCount;

        List<UIVertex> vertices = new List<UIVertex>();
        helper.GetUIVertexStream(vertices);

        UIVertex v = new UIVertex();

        //记录顶点的初始位置
        if (_verticesOriPos == null)
        {
            _verticesOriPos = new List<Vector3>();


            bool even = _verticesCount / 4 % 2 == 0;

            for (int i = 0; i < _verticesCount; i++)
            {
                helper.PopulateUIVertex(ref v, i);
                _verticesOriPos.Add(v.position);
                //Debug.LogError("!!!!!! "+ textInterval);
                //if (textInterval == 0)
                //{
                //    continue;
                //}

                //    float tempInterval = textInterval * Mathf.Abs(_verticesCount / 8 - i / 4) * textIntervalFactor;
                //if (even)
                //{
                //    Debug.LogError("even");
                //    if(i/4 < _verticesCount / 8)
                //    {
                //        _verticesOriPos[i] -= new Vector3((_verticesCount / 8 - i / 4 - 1) * tempInterval + tempInterval / 2, 0, 0);
                //    }
                //    else
                //    {
                //        _verticesOriPos[i] += new Vector3((i / 4 - _verticesCount / 8) * tempInterval + tempInterval / 2, 0, 0);
                //    }
                //}
                //else
                //{
                //    Debug.LogError("!even");
                //    _verticesOriPos[i] -= new Vector3((_verticesCount / 8 - i / 4) * tempInterval, 0, 0);
                //}
                
            }
        }

        //记录每个文字的中心位置
        if (_centerPos == null)
        {
            _centerPos = new List<Vector3>();
            for (int i = 0; i < helper.currentVertCount; i += 4)
            {
                _centerPos.Add(_verticesOriPos[i] / 2 + _verticesOriPos[i + 2] / 2);
            }
        }

        //如果定点数有变化,说明文本内容有修改,需要清除记录
        if (_verticesOriPos.Count != helper.currentVertCount)
        {
            ClearPosRecord();
            return;
        }

        //随机位子淡入
        if (randomOrder && _randomOrderList == null)
        {
            //_randomOrderList = new List<int>();
            _randomOrderList =  GetRandomOrder();
        }

        //圆心布局 TODO 优化计算
        if(_lastType == Type.CircleLayout)
        {
            //_circleCenter = 
            Vector3 tempCenter = new Vector3();
            for (int i = 0; i < _centerPos.Count; i++)
            {
                tempCenter += _centerPos[i];
            }

            tempCenter /= _centerPos.Count;

            _circleCenter = new Vector3(tempCenter.x, tempCenter.y - radius);
            _textCenter = new Vector3(tempCenter.x, tempCenter.y);
        }

        //对每个定点做相应的处理
        for (int i = 0; i < helper.currentVertCount; i++)
        {
            helper.PopulateUIVertex(ref v, i);

            if (_isFadeIn)
            {
                float alpha = SampleAlpha(i);
                if (IsValide(alpha))
                {
                    v.color.a = (byte)(alpha * 255);
                }
                else
                {
                    v.color.a = 0;
                }
            }
            if (type == Type.Normal)
            {
                Normal(helper, ref v, i);
            }
            else if (type == Type.Jump)
            {
                Jump(helper, ref v, i);
            }
            else if (type == Type.Scale)
            {
                Scale(helper, ref v, i);
            }
            else if (type == Type.Stretch)
            {
                Stretch(helper, ref v, i);
            }
            else if (type == Type.Rotate)
            {
                Rotate(helper, ref v, i);
            }
            else if (type == Type.CircleLayout)
            {
                CircleLayout(helper, ref v, i);
            }
            else if (type == Type.Color)
            {
                ChangeColor(helper, ref v, i);
            }
            else if (type == Type.Rainbow)
            {
                Rainbow(helper, ref v, i);
            }

            helper.SetUIVertex(v, i);
        }
    }

    void ClearPosRecord()
    {
        if (_verticesOriPos != null)
        {
            _verticesOriPos.Clear();
            _verticesOriPos = null;
        }

        if (_centerPos != null)
        {
            _centerPos.Clear();
            _centerPos = null;
        }

        if(_randomOrderList != null)
        {
            _randomOrderList.Clear();
            _randomOrderList = null;
        }
    }
    #region 采样函数

    float SampleAlpha(int index)
    {
        if (randomOrder)
        {
            index = _randomOrderList[index];
        }
        float timeOff = (index / 4) * perTextInterval * perTextSpeed;
        float curveX = _x - (_isFadeout ? -timeOff : timeOff);
        //未循环 且初始不可见
        if (!Loop && curveX < 0 && _isFadeIn)
        {
            return INVALID;
        }
        return alphaCurve.Evaluate(curveX);
    }

    //坐标位子偏移采样
    Vector2 SampleOffset(int index)
    {
        if (randomOrder)
        {
            index = _randomOrderList[index];
        }
        //一个字体网格四个顶点
        float timeOff = (index / 4) * perTextInterval * perTextSpeed;
        //float tempX = _x - (index / 4) * perTextInterval * perTextSpeed;
        float curveX = _x - (_isFadeout ? -timeOff : timeOff);
        if (curveX < 0 || curveX > 1)
        {
            return Vector2.zero;
        }
        else
        {
            //if (_isFadeout)
            //{
            //    Vector2 temp = posOffset;
            //    temp.x *= -1;
                return Vector2.Lerp(_posOffset, Vector2.zero, curveX);
            //}
            //else
            //{
            //    return Vector2.Lerp(_posOffset, Vector2.zero, curveX);
            //}
            //return Vector2.Lerp(posOffset, Vector2.zero, curveX);
        }
    }

    float SampleFromAnimCurve(int index)
    {
        if (!IsStart)
        {
            return 0;
        }

        if (randomOrder)
        {
            index = _randomOrderList[index];
        }

        float timeOff = (index / 4) * perTextInterval * perTextSpeed;
        //一个字体网格四个顶点
        float curveX = _x - (_isFadeout ?  -timeOff : timeOff);

        //动画最后一个顶点的索引 拉伸每四个顶点只有前两个顶点在表现
        int endVerticIndex = _lastType == Type.Stretch ? _verticesCount - 3 : _verticesCount - 1;


        //淡出模式的动画结束标志判断
        if (_isFadeout)
        {
            if (!Loop && curveX > CURVE_MAX_TIME && _isFadeIn)
            {
                return INVALID;
            }

            if (!_onceEnd && index == endVerticIndex && curveX <= CURVE_MIN_TIME)
            {
                _onceEnd = true;
            }
        }
        else
        {
            if (!Loop && curveX < CURVE_MIN_TIME && _isFadeIn)
            {
                return INVALID;
            }
            //最后一个顶点动画结束标识
            if (!_onceEnd && index == endVerticIndex && curveX >= CURVE_MAX_TIME)
            {
                _onceEnd = true;
                //loop间隔随机因子计算
                if (isRandomForLoopInterval)
                {
                    _randomForLoopInterval = Random.Range(-randomRangeForLoopInterval, randomRangeForLoopInterval);
                }
            }
        }

        

        return animCurve.Evaluate(curveX);
    }

    
    float Y(int index)
    {
        if (!IsStart)
        {
            return 0;
        }

        //一个字体网格四个顶点
        float tempX = _x - (index / 4) * perTextInterval * perTextSpeed;
        //未循环 且初始不可见
        if(!Loop && tempX < 0 && _isFadeIn)
        {
            return INVALID;
        }
        switch (periodicType)
        {
            case PeriodicFunction.Type.Sawtooth:
                return PeriodicFunction.Sawtooth(tempX, loopInterval * perTextSpeed, frequency * 2, 0, atten);

            case PeriodicFunction.Type.SawtoothPlus:
                return PeriodicFunction.SawtoothWithoutNegative(tempX, loopInterval * perTextSpeed, frequency * 2, 0, atten);

            case PeriodicFunction.Type.Sine:
                return PeriodicFunction.Sine(tempX, loopInterval * perTextSpeed, frequency * 2, 0, atten);

            case PeriodicFunction.Type.SinePlus:
                return PeriodicFunction.SineWithoutNegative(tempX, loopInterval * perTextSpeed, frequency * 2, 0, atten);

            case PeriodicFunction.Type.Linear01:
                return PeriodicFunction.Linear01(tempX, loopInterval * perTextSpeed, frequency, 0, atten);

            case PeriodicFunction.Type.Linear10:
                return PeriodicFunction.Linear10(tempX, loopInterval * perTextSpeed, frequency, 0, atten);

            default:
                return 0;
        }
    }

    float Y2(int index)
    {
        if (!IsStart)
        {
            return 0;
        }

        //一个字体网格四个顶点
        float tempX = _x - (index / 4) * perTextInterval;
        switch (periodicType)
        {
            case PeriodicFunction.Type.Sawtooth:
                return PeriodicFunction.Sawtooth(tempX, loopInterval * perTextSpeed, frequency, 0, atten);

            case PeriodicFunction.Type.SawtoothPlus:
                return PeriodicFunction.SawtoothWithoutNegative(tempX, loopInterval * perTextSpeed, frequency, 0, atten);

            case PeriodicFunction.Type.Sine:
                return PeriodicFunction.Sine(tempX, loopInterval * perTextSpeed, frequency, 0, atten);

            case PeriodicFunction.Type.SinePlus:
                return PeriodicFunction.SineWithoutNegative(tempX, loopInterval * perTextSpeed, frequency, 0, atten);

            default:
                return 0;
        }
    }
    #endregion

    //跳跃效果
    void Jump(VertexHelper helper, ref UIVertex v, int i)
    {
        float y = SampleFromAnimCurve(i);
        if (!IsValide(y))
        {
            return;
        }
        v.position.y = _verticesOriPos[i].y + SampleFromAnimCurve(i) * animFactor;
        ProcessOffset(ref v, i);
    }

    //stretch
    void Stretch(VertexHelper helper, ref UIVertex v, int i)
    {
        if (i % 4 == 0 || i % 4 == 1)
        {
            Jump(helper, ref v, i);
        }
    }

    void Scale(VertexHelper helper, ref UIVertex v, int i)
    {
        if (!IsStart)
        {
            return;
        }

        float y = SampleFromAnimCurve(i);
        if(!IsValide(y))
        {
            return;
        }

        Vector3 dir = Vector3.Normalize(_verticesOriPos[i] - _centerPos[i / 4]);
        v.position = _verticesOriPos[i] + dir * y * animFactor;

        ProcessOffset(ref v, i);
    }

    //普通 只支持fadein/out  posOffset
    void Normal(VertexHelper helper, ref UIVertex v, int i)
    {
        if (!IsStart)
        {
            return;
        }

        float y = SampleFromAnimCurve(i);
        if (!IsValide(y))
        {
            return;
        }

        ProcessOffset(ref v, i);
    }

    //void TransparentText(VertexHelper helper, ref UIVertex v, int i)
    //{
    //    v.color.a = 0;
    //}

    void Rotate(VertexHelper helper, ref UIVertex v, int i)
    {
        Vector3 tempOriginPos = _verticesOriPos[i];
        tempOriginPos -= _centerPos[i / 4];
        //float temp = 1 - Y2(i);
        float y = SampleFromAnimCurve(i);
        if (!IsValide(y))
        {
            return;
        }
        float temp = 1 - y;
        float cos = Mathf.Cos(temp * Mathf.PI * 2);
        float sin = Mathf.Sin(temp * Mathf.PI * 2);
        v.position.x = tempOriginPos.x * cos - tempOriginPos.y * sin;
        v.position.y = tempOriginPos.x * sin + tempOriginPos.y * cos;
        v.position += _centerPos[i / 4];

        ProcessOffset(ref v, i);
    }

    void CircleLayout(VertexHelper helper, ref UIVertex v, int i)
    {
        //Vector3 tempOriginPos = _verticesOriPos[i];
        //Vector3 tempCenterPos = _centerPos[i / 4];
        //Vector3 len = tempCenterPos - _circleCenter;
        //Vector3 dir = Vector3.Normalize(len);
        //Vector3 center = new Vector3(0, 1, 0);
        //float cos = Vector3.Dot(dir, center);
        //float sin = Mathf.Sqrt(1 - cos * cos);
        //if(i >= _verticesCount / 2)
        //{
        //    sin = -sin;
        //}

        //tempOriginPos -= tempCenterPos;
        //v.position.x = tempOriginPos.x * cos - tempOriginPos.y * sin;
        //v.position.y = tempOriginPos.x * sin + tempOriginPos.y * cos;
        //v.position += tempCenterPos;

        //Vector3 R = dir * radius;
        //Vector3 off = len - R;
        //v.position -= off;

        //Vector3 textCenter = _circleCenter + radius;


        float angle = 0;
        if (_verticesCount / 4 % 2 == 0)
        {
            angle = (_verticesCount / 8 - i / 4) * textInterval;
            if (i / 4 < _verticesCount / 8)
            {
                angle = (_verticesCount / 8 - i / 4 - 1) * textInterval + textInterval / 2;
            }
            else
            {
                angle =  -(i / 4 - _verticesCount / 8) * textInterval - textInterval / 2;
            }
        }
        else
        {
            angle = (_verticesCount / 8 - i / 4) * textInterval;
        }

        float sin = Mathf.Sin(angle / 180 * Mathf.PI);
        float cos = Mathf.Cos(angle / 180 * Mathf.PI);
        Vector3 tempTextCenter = _textCenter;
        tempTextCenter -= _circleCenter;
        Vector3 tempCurCenter = new Vector3();
        tempCurCenter.x = tempTextCenter.x * cos - tempTextCenter.y * sin;
        tempCurCenter.y = tempTextCenter.x * sin + tempTextCenter.y * cos;
        tempCurCenter += _circleCenter;

        Vector3 offset = tempCurCenter - _centerPos[i / 4];
        Vector3 tempOriginPos = _verticesOriPos[i] + offset;
        tempOriginPos -= tempCurCenter;
        v.position.x = tempOriginPos.x * cos - tempOriginPos.y * sin;
        v.position.y = tempOriginPos.x * sin + tempOriginPos.y * cos;
        v.position += tempCurCenter;
    }

    Color _originColor;
    void ChangeColor(VertexHelper helper, ref UIVertex v, int i)
    {
        float y = SampleFromAnimCurve(i);
        if (!IsValide(y))
        {
            return;
        }
        v.color = Color32.Lerp(_originColor, textColor, y);
    }

    Color[] rainbowColors = new Color[] {
        Color.red,
        new Color(1, 165f/255f, 0),
        Color.yellow,
        Color.green,
        new Color(0, 127f/255f, 1),
        Color.blue,
        new Color(139f/255f, 0, 1)
    };

    void Rainbow(VertexHelper helper, ref UIVertex v, int i)
    {
        //确定当前所处在哪一个周期
        float temp = _x - (i / 4) * perTextInterval;
        temp %= frequency + loopInterval * perTextSpeed;
        int rainbowIndex = 0;
        if (temp < frequency && temp >= 0)
        {
            rainbowIndex = Mathf.FloorToInt(temp);
            if (rainbowIndex == 0)
            {
                v.color = Color32.Lerp(_originColor, rainbowColors[0], Y(i));
            }
            else if (rainbowIndex == 7)
            {
                v.color = Color32.Lerp(rainbowColors[6], _originColor, Y(i));
            }
            else
            {
                v.color = Color32.Lerp(rainbowColors[rainbowIndex - 1], rainbowColors[rainbowIndex], Y(i));
            }
        }
    }

    #endregion

    //曲线检测
    void CheckCurve()
    {
        if(animCurve != null && animCurve.length > 0)
        {
            //右边界校验 加0.01的范围值 避免误伤
            if(animCurve[animCurve.length - 1].time > CURVE_MAX_TIME + .01f)
            {
                Debug.LogError("曲线参数 curve 的横坐标最大值不能超过1,时间可以配合 speed 参数控制. 当前值:" + animCurve[animCurve.length - 1].time);
            }

            //左边界校验
            if (animCurve[0].time < CURVE_MIN_TIME - .01f)
            {
                Debug.LogError("曲线参数 curve 的横坐标最小值不能超过0. 当前值:" + animCurve[0].time);
            }
            //wrapModel校验
            if (animCurve.preWrapMode != WrapMode.Clamp)
            {
                animCurve.preWrapMode = WrapMode.Clamp;
            }
            if(animCurve.postWrapMode != WrapMode.Clamp)
            {
                animCurve.postWrapMode = WrapMode.Clamp;
            }

            //TODO 结尾值校验

        }
    }

    //loop模式下参数检测
    void CheckLoop()
    {
        if (Loop)
        {
            //loop模式下不支持fadein
            if (isFadein)
            {
                isFadein = false;
            }

            //loop模式下不支持PosOffset
            if (openPosOffset)
            {
                openPosOffset = false;
            }
        }
    }

    //输入值是否有效
    bool IsValide(float val)
    {
        return !(val <= INVALID + 1 && val >= INVALID - 1);
    }

    //处理偏移
    void ProcessOffset(ref UIVertex v, int i)
    {
        if (_openPosOffset)
        {
            Vector2 posOffset = SampleOffset(i);
            v.position.x += posOffset.x;
            v.position.y += posOffset.y;
        }
    }

    List<int> GetRandomOrder()
    {
        Debug.LogError("GetRandomOrder");
        List<int> order = new List<int>();

        for (int i = 0; i < _verticesOriPos.Count; i++)
        {
            order.Add(i);
        }

        int maxOrder = order.Count / 4;
        for (int i = 0; i < order.Count; i+=4)
        {
            int randomOrder = Random.Range(0, maxOrder);
            int sweapIndex = randomOrder * 4;
            for (int j = 0; j < 4; j++)
            {
                order[i + j] = order[i + j] ^ order[sweapIndex + j];
                order[sweapIndex + j] = order[i + j] ^ order[sweapIndex + j];
                order[i + j] = order[i + j] ^ order[sweapIndex + j];
            }
        }
        //Debug.LogError();
        //string s = "";
        //for (int i = 0; i < order.Count; i++)
        //{
        //    s += order[i] + " , ";
        //}
        //Debug.LogError(s);

        return order;
    }

    void SwipTwoOrder()
    {

    }
}

