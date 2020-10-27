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

    public enum Layout
    {
        Normal, //原始的
        Circle, //圆形
        Italic, //斜体
    }

    public enum Type
    {
        Normal,     //普通，fadein/out 和 posOffset
        Jump,       //跳跃
        Rotate,     //旋转
        Scale,      //缩放
        Color,      //扫光
        //Rainbow,    //彩虹
        Stretch,    //拉伸
        //CircleLayout,//布局 圆
        //Italic,     //斜体
    }

    public enum Stage
    {
        Layout = 0,     //布局
        Fadein = 1,     //淡入
        Display = 2,    //展示
        Fadeout = 3     //淡出
    }

    //消失类型
    public enum FadeoutType
    {
        Back,        //fadein的反向播放
        Immediately, //立即
        Other,       //其他
    }

    public bool playOnAwake = true;

    [Tooltip("Awake的延迟时间")]
    public float awakeDelay;


    #region 布局设置
    public Layout layout = Layout.Normal;
    public float radius = 200;
    public float angle = 10;
    public Vector2 italicFactor;

    Layout _lastLayout;
    //bool _layoutFinished = false;
    float _radius;
    float _angle;

    Vector3 _circleCenter;
    Vector3 _textCenter;
    List<Vector3> _verticleYDir;//顶点的Y坐标方向
    #endregion 

    public bool enableAnim;
    public bool unscaledTime;

    #region 第一阶段属性 淡入
    [Tooltip("是否需要淡入效果")]
    public bool fadein_1 = false;

    public Type type_1 = Type.Normal;

    [Tooltip("动画曲线,控制每个字的动画节奏")]
    public AnimationCurve animCurve_1;
    [Tooltip("透明度变化曲线")]
    public AnimationCurve alphaCurve_1 = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    [Tooltip("每个字动画的快慢")]
    public float perTextSpeed_1 = 2;
    [Tooltip("每个字的动画间隔时间")]
    public float perTextInterval_1 = 0.3f;

    [Tooltip("是否循环")]
    public bool loop_1 = false;
    [Tooltip("Loop的间隔时间")]
    public float loopInterval_1 = 5f;
    [Tooltip("Loop间隔时间加随机值")]
    public bool isRandomForLoopInterval_1 = false;
    [Tooltip("Loop间隔时间随机因子范围")]
    public float randomRangeForLoopInterval_1 = 0;

    [Tooltip("动画程度因子")]
    public float animFactor_1 = 30;
    [Tooltip("文字颜色(类似于扫光颜色)")]
    public Color textColor_1 = Color.red;

    [Tooltip("是否开启坐标位子偏移")]
    public bool openPosOffset_1 = false;
    [Tooltip("坐标位置偏移")]
    public Vector2 posOffset_1;

    [Tooltip("顺序随机化")]
    public bool randomOrder_1 = false;
    #endregion

    #region 第二阶段属性 持续显示阶段
    public bool stage_2;

    public Type type_2 = Type.Normal;

    [Tooltip("动画曲线,控制每个字的动画节奏")]
    public AnimationCurve animCurve_2;

    [Tooltip("每个字动画的快慢")]
    public float perTextSpeed_2 = 2;
    [Tooltip("每个字的动画间隔时间")]
    public float perTextInterval_2 = 0.3f;

    [Tooltip("是否循环")]
    public bool loop_2 = false;
    [Tooltip("Loop的间隔时间")]
    public float loopInterval_2 = 5f;
    [Tooltip("Loop间隔时间加随机值")]
    public bool isRandomForLoopInterval_2 = false;
    [Tooltip("Loop间隔时间随机因子范围")]
    public float randomRangeForLoopInterval_2 = 0;

    [Tooltip("动画程度因子")]
    public float animFactor_2 = 30;
    [Tooltip("文字颜色(类似于扫光颜色)")]
    public Color textColor_2 = Color.red;

    [Tooltip("是否开启坐标位子偏移")]
    public bool openPosOffset_2 = false;
    [Tooltip("坐标位置偏移")]
    public Vector2 posOffset_2;

    [Tooltip("顺序随机化")]
    public bool randomOrder_2 = false;

    #endregion

    #region 第三阶段属性 淡出
    public bool stage_3;

    #endregion

    #region 阶段切换需要修改的参数
    Type _curType;
    AnimationCurve _animCurve;
    float _perTextSpeed;
    float _perTextInterval;
    bool _loop;
    float _loopInterval;
    bool _isRandomForLoopInterval;
    float _randomRangeForLoopInterval;
    float _animFactor;
    Color _textColor;
    bool _openPosOffset;
    Vector2 _posOffset;
    bool _randomOrder;
    #endregion


    //阶段切换 参数重新赋值
    void SetParamsOnStageChanged()
    {
        Debug.LogError("SetParamsOnStageChanged " + _stage);
        _x = 0;
        if(_stage == Stage.Fadein)
        {
            _curType = type_1;
            _animCurve = animCurve_1;
            _perTextSpeed = perTextSpeed_1;
            _perTextInterval = perTextInterval_1;
            _loop = loop_1;
            _loopInterval = loopInterval_1;
            _isRandomForLoopInterval = isRandomForLoopInterval_1;
            _randomRangeForLoopInterval = randomRangeForLoopInterval_1;
            _animFactor = animFactor_1;
            _textColor = textColor_1;
            _openPosOffset = openPosOffset_1;
            _posOffset = posOffset_1;
            _randomOrder = randomOrder_1;
        }
        else if (_stage == Stage.Display)
        {
            _isFadeIn = false;//fadein只在Fadein阶段有用

            _curType = type_2;
            _animCurve = animCurve_2;
            _perTextSpeed = perTextSpeed_2;
            _perTextInterval = perTextInterval_2;
            _loop = loop_2;
            _loopInterval = loopInterval_2;
            _isRandomForLoopInterval = isRandomForLoopInterval_2;
            _randomRangeForLoopInterval = randomRangeForLoopInterval_2;
            _animFactor = animFactor_2;
            _textColor = textColor_2;
            _openPosOffset = openPosOffset_2;
            _posOffset = posOffset_2;
            _randomOrder = randomOrder_2;
        }
    }


    Stage _stage = Stage.Layout;

   

   


    

    [Tooltip("自动消失")]
    public bool autoDisappear;

    [Tooltip("显示时长(从AwakeDelay过后就开始计时)")]
    public float duration;


    public bool IsStart { get; private set; } = false;

    Type _lastType;
    float _randomForLoopInterval = 0;
    bool _isFadeIn;
    bool _isFadeout;
    //bool _openPosOffset;
    //Vector2 _posOffset;

   
    
    public void Play()
    {
        //if (type == Type.Rainbow)
        //{
        //    frequency = 8;
        //    periodicType = PeriodicFunction.Type.Linear01;
        //}
        _lastType = _curType;

        //_openPosOffset = openPosOffset_1;
        //_posOffset = posOffset_1;
        _isFadeIn = fadein_1;
        _isFadeout = false;
        IsStart = true;
        _onceEnd = false;
        _originColor = _text.color;
        _isFirst = true;

        SetParam();
        //_layoutFinished = false;
    }

    //从inspector面板获取参数并设置
    void SetParam()
    {
        _lastLayout = layout;
        _angle = angle;
        _radius = radius;
    }

    //淡出 和淡入反着来
    public void Fadeout()
    {
        //loop模式不支持淡出操作
        if (loop_1)
        {
            Debug.LogError("loop模式不支持淡出操作");
            return;
        }

        IsStart = true;
        _isFadeout = true;
        _onceEnd = false;

        //_openPosOffset = openPosOffset_1;
        //_posOffset = posOffset_1;
        _isFadeIn = fadein_1;
        //_posOffset = posOffset_1 * new Vector2(-1, 1);
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
        _stage = 0;
        _isFadeIn = false;
        IsStart = false;
        _openPosOffset = false;
        _x = 0;
        //_layoutFinished = false;
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
    float _tickForShowTime = 0;//显示时间计时(从AwakeDelay过后就开始计时)
    private void Update()
    {
       
#if UNITY_EDITOR
        //参数合法性检测
        if (!Application.isPlaying)
        {
            CheckCurve();
            CheckLoop();
        }

        //Relayout校验
        if (_angle != angle || _radius != radius || _lastLayout != layout)
        {
            SetParam();
            Relayout();
        }
#endif

        //未开启动画 后面的一切操作都不将执行
        if (!enableAnim)
        {
            return;
        }

        if (!IsStart)
            return;

        //if (_lastType != _curType)
        //{
        //    Debug.LogError("!!!!!!!!");
        //    Stop();
        //    return;
        //}
        if (_loop)
        {
            if (_onceEnd)
            {
                _tickForWait += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                if(_tickForWait >= loopInterval_1 + _randomForLoopInterval)
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
            _x -= unscaledTime ? Time.unscaledDeltaTime * _perTextSpeed : Time.deltaTime * _perTextSpeed;
        }
        else
        {
            _x += unscaledTime ? Time.unscaledDeltaTime * _perTextSpeed : Time.deltaTime * _perTextSpeed;
        }
        graphic.SetVerticesDirty();
    }

    Text _text;
    protected override void OnEnable()
    {
        base.OnEnable();
        _text = GetComponent<Text>();
        _lastType = type_1;

        _x = -awakeDelay * _perTextSpeed;
        if (playOnAwake && Application.isPlaying)
        {
            Play();
        }
    }

    protected override void OnDisable()
    {
        Stop();
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

        //如果顶点数有变化,说明文本内容有修改,需要清除记录
        if (_verticesOriPos != null && _verticesOriPos.Count != helper.currentVertCount)
        {
            ClearPosRecord();
        }

        List<UIVertex> vertices = new List<UIVertex>();
        helper.GetUIVertexStream(vertices);

        UIVertex v = new UIVertex();

        //记录顶点的初始位置
        if (_verticesOriPos == null)
        {
            _verticesOriPos = new List<Vector3>();
            for (int i = 0; i < _verticesCount; i++)
            {
                //Debug.LogError(v.position);
                helper.PopulateUIVertex(ref v, i);
                _verticesOriPos.Add(v.position);
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

        //随机位子淡入
        if (_randomOrder && _randomOrderList == null)
        {
            _randomOrderList = MagicTextUtils.GetRandomOrder(_verticesCount);
        }

        //处理布局阶段
        if(_stage == Stage.Layout)
        {
            RefreshLayout(helper, ref v);
            //进入下一阶段
            _stage = Stage.Fadein;
            SetParamsOnStageChanged();
        }
        
        //尝试处理淡入阶段
        if(_stage == Stage.Fadein && fadein_1)
        {
            //对每个顶点做相应的处理
            ProcessAnim(helper, ref v);
        }
        //尝试处理展示阶段
        else if(_stage == Stage.Display && stage_2)
        {
            //对每个顶点做相应的处理
            ProcessAnim(helper, ref v);
        }
        //尝试处理淡入阶段
        else if (_stage == Stage.Fadeout && stage_3)
        {

        }
        //
        else
        {
            LayoutDefault(helper, ref v);
        }
        

        //if (_isFadeIn)
        //{
        //    //对每个顶点做相应的处理
        //    ProcessAnim(helper, ref v);
        //}
        //else
        //{
        //    LayoutDefault(helper, ref v);
        //}
    }

    //刷新布局
    void RefreshLayout(VertexHelper helper, ref UIVertex v)
    {
        //圆形布局,重新计算顶点位置信息
        if (layout == Layout.Circle)
        {
            Vector3 tempCenter = new Vector3();
            for (int i = 0; i < _centerPos.Count; i++)
            {
                tempCenter += _centerPos[i];
            }

            tempCenter /= _centerPos.Count;
            _circleCenter = new Vector3(tempCenter.x, tempCenter.y - _radius);
            _textCenter = new Vector3(tempCenter.x, tempCenter.y);

            //重新计算每个顶点的布局
            for (int i = 0; i < helper.currentVertCount; i++)
            {
                helper.PopulateUIVertex(ref v, i);
                CircleLayout(helper, ref v, i);
                helper.SetUIVertex(v, i);
            }
            RecordAfterLayout(helper, ref v);
        }

        //_layoutFinished = true;
        
    }

    //处理动画
    void ProcessAnim(VertexHelper helper, ref UIVertex v)
    {
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
            if (_curType == Type.Normal)
            {
                Normal(helper, ref v, i);
            }
            else if (_curType == Type.Jump)
            {
                Jump(helper, ref v, i);
            }
            else if (_curType == Type.Scale)
            {
                Scale(helper, ref v, i);
            }
            else if (_curType == Type.Stretch)
            {
                Stretch(helper, ref v, i);
            }
            else if (_curType == Type.Rotate)
            {
                Rotate(helper, ref v, i);
            }
            //else if (type_1 == Type.CircleLayout)
            //{
            //    CircleLayout(helper, ref v, i);
            //}
            //else if (type_1 == Type.Italic)
            //{
            //    Italic(helper, ref v, i);
            //}
            else if (_curType == Type.Color)
            {
                ChangeColor(helper, ref v, i);
            }
            //else if (type_1 == Type.Rainbow)
            //{
            //    //Rainbow(helper, ref v, i);
            //}
            //else if (type == Type.Test)
            //{
            //    Test(helper, ref v, i);
            //}

            helper.SetUIVertex(v, i);
        }
    }

    //在布局后重新记录顶点信息
    void RecordAfterLayout(VertexHelper helper, ref UIVertex v)
    {
        _verticesOriPos.Clear();
        for (int i = 0; i < _verticesCount; i++)
        {
            helper.PopulateUIVertex(ref v, i);
            _verticesOriPos.Add(v.position);
        }

        _centerPos.Clear();
        for (int i = 0; i < _verticesCount; i += 4)
        {
            _centerPos.Add(_verticesOriPos[i] / 2 + _verticesOriPos[i + 2] / 2);
        }

        if(_verticleYDir == null)
        {
            _verticleYDir = new List<Vector3>();
        }
        _verticleYDir.Clear();
        // 0  1
        // 3  2
        for (int i = 0; i < _verticesCount; i++)
        {
            if (i % 4 == 0)
            {
                _verticleYDir.Add((_verticesOriPos[i] - _verticesOriPos[i + 3]).normalized);
            }
            else if (i % 4 == 1)
            {
                _verticleYDir.Add((_verticesOriPos[i] - _verticesOriPos[i + 1]).normalized);
            }
            else if (i % 4 == 2)
            {
                _verticleYDir.Add((_verticesOriPos[i - 1] - _verticesOriPos[i]).normalized);
            }
            else if (i % 4 == 3)
            {
                _verticleYDir.Add((_verticesOriPos[i - 3] - _verticesOriPos[i]).normalized);
            }
        }
    }

    //位置记录的清除操作
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
        //_layoutFinished = false;

        //回到布局阶段
        _stage = Stage.Layout;
    }

    //重新布局
    public void Relayout()
    {
        ClearPosRecord();
    }

    #region 采样函数

    float SampleAlpha(int index)
    {
        if (_randomOrder)
        {
            index = _randomOrderList[index];
        }

        float timeOff = (index / 4) * _perTextInterval * _perTextSpeed;
        float curveX = _x - (_isFadeout ? -timeOff : timeOff);
        //未循环 且初始不可见
        if (!_loop && curveX < 0 && _isFadeIn)
        {
            return INVALID;
        }
        return alphaCurve_1.Evaluate(curveX);
    }

    //坐标位子偏移采样
    Vector2 SampleOffset(int index)
    {
        if (_randomOrder)
        {
            index = _randomOrderList[index];
        }

        //一个字体网格四个顶点
        float timeOff = (index / 4) * _perTextInterval * _perTextSpeed;
        //float tempX = _x - (index / 4) * perTextInterval * perTextSpeed;
        float curveX = _x - (_isFadeout ? -timeOff : timeOff);
        if (curveX < 0 || curveX > 1)
        {
            return Vector2.zero;
        }
        else
        {
            return Vector2.Lerp(_posOffset, Vector2.zero, curveX);
        }
    }

    float SampleFromAnimCurve(int index)
    {
        if (!IsStart)
        {
            return 0;
        }


        if (_randomOrder)
        {
            index = _randomOrderList[index];
        }

        float timeOff = (index / 4) * _perTextInterval * _perTextSpeed;
        //一个字体网格四个顶点
        float curveX = _x - (_isFadeout ?  -timeOff : timeOff);

        //动画最后一个顶点的索引 拉伸每四个顶点只有前两个顶点在表现
        int endVerticIndex = _lastType == Type.Stretch ? _verticesCount - 3 : _verticesCount - 1;


        //淡出模式的动画结束标志判断
        if (_isFadeout)
        {
            if (!_loop && curveX > CURVE_MAX_TIME && _isFadeIn)
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
            if (!_loop && curveX < CURVE_MIN_TIME && _isFadeIn)
            {
                return INVALID;
            }
            //最后一个顶点动画结束标识
            if (!_onceEnd && index == endVerticIndex && curveX >= CURVE_MAX_TIME)
            {
                _onceEnd = true;
                if (!_loop)
                {
                    //切换阶段
                    _stage++;
                    SetParamsOnStageChanged();
                }

                //loop间隔随机因子计算
                if (_isRandomForLoopInterval)
                {
                    _randomForLoopInterval = Random.Range(-_randomRangeForLoopInterval, _randomRangeForLoopInterval);
                }
            }
        }

        return animCurve_1.Evaluate(curveX);
    }
    #endregion

    #region 效果执行函数

    Vector3 _tempDir = new Vector3();
    //跳跃效果
    void Jump(VertexHelper helper, ref UIVertex v, int i)
    {
        float y = SampleFromAnimCurve(i);
        if (!IsValide(y))
        {
            return;
        }
        
        //圆形布局需要计算方向
        if(layout == Layout.Circle)
        {
            v.position = _verticesOriPos[i] + SampleFromAnimCurve(i) * _animFactor * _verticleYDir[i];
        }
        else
        {
            v.position.y = _verticesOriPos[i].y + SampleFromAnimCurve(i) * _animFactor;
        }

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
        v.position = _verticesOriPos[i] + dir * y * _animFactor;

        ProcessOffset(ref v, i);
    }

    void LayoutDefault(VertexHelper helper, ref UIVertex v)
    {
        for (int i = 0; i < helper.currentVertCount; i++)
        {
            helper.PopulateUIVertex(ref v, i);
            v.position = _verticesOriPos[i];
            helper.SetUIVertex(v, i);
        }
    }

    //普通 只支持fadein/out  posOffset
    void Normal(VertexHelper helper, ref UIVertex v, int i)
    {
        v.position = _verticesOriPos[i];
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
        float curAngle = 0;
        if (_verticesCount / 4 % 2 == 0)
        {
            curAngle = (_verticesCount / 8 - i / 4) * _angle;
            if (i / 4 < _verticesCount / 8)
            {
                curAngle = (_verticesCount / 8 - i / 4 - 1) * _angle + _angle / 2;
            }
            else
            {
                curAngle =  -(i / 4 - _verticesCount / 8) * _angle - _angle / 2;
            }
        }
        else
        {
            curAngle = (_verticesCount / 8 - i / 4) * _angle;
        }

        float sin = Mathf.Sin(curAngle / 180 * Mathf.PI);
        float cos = Mathf.Cos(curAngle / 180 * Mathf.PI);
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

    void Italic(VertexHelper helper, ref UIVertex v, int i)
    {
        //float y = SampleFromAnimCurve(i);
        //if (!IsValide(y))
        //{
        //    return;
        //}

        if (i % 4 == 0)
        {
            v.position.x += italicFactor.x;
            //ProcessOffset(ref v, i);
        }
        else if(i % 4 == 1)
        {
            v.position.x += italicFactor.x;
            //ProcessOffset(ref v, i);
        }
    }

    Color _originColor;
    void ChangeColor(VertexHelper helper, ref UIVertex v, int i)
    {
        float y = SampleFromAnimCurve(i);
        if (!IsValide(y))
        {
            return;
        }
        byte curAlpah = v.color.a;
        v.color = Color32.Lerp(_originColor, _textColor, y);
        v.color.a = curAlpah;
    }

    //Color[] rainbowColors = new Color[] {
    //    Color.red,
    //    new Color(1, 165f/255f, 0),
    //    Color.yellow,
    //    Color.green,
    //    new Color(0, 127f/255f, 1),
    //    Color.blue,
    //    new Color(139f/255f, 0, 1)
    //};

    //void Rainbow(VertexHelper helper, ref UIVertex v, int i)
    //{
    //    //确定当前所处在哪一个周期
    //    float temp = _x - (i / 4) * perTextInterval;
    //    temp %= frequency + loopInterval * perTextSpeed;
    //    int rainbowIndex = 0;
    //    if (temp < frequency && temp >= 0)
    //    {
    //        rainbowIndex = Mathf.FloorToInt(temp);
    //        if (rainbowIndex == 0)
    //        {
    //            v.color = Color32.Lerp(_originColor, rainbowColors[0], Y(i));
    //        }
    //        else if (rainbowIndex == 7)
    //        {
    //            v.color = Color32.Lerp(rainbowColors[6], _originColor, Y(i));
    //        }
    //        else
    //        {
    //            v.color = Color32.Lerp(rainbowColors[rainbowIndex - 1], rainbowColors[rainbowIndex], Y(i));
    //        }
    //    }
    //}
    #endregion

    #endregion

    #region 参数校验

    //曲线检测
    public bool CheckCurve()
    {
        if(animCurve_1 != null && animCurve_1.length > 0)
        {
            //右边界校验 加0.01的范围值 避免误伤
            if(animCurve_1[animCurve_1.length - 1].time > CURVE_MAX_TIME + .01f)
            {
                Debug.LogError("曲线参数 curve 的横坐标最大值不能超过1,时间可以配合 speed 参数控制. 当前值:" + animCurve_1[animCurve_1.length - 1].time);
                return false;
            }

            //左边界校验
            if (animCurve_1[0].time < CURVE_MIN_TIME - .01f)
            {
                Debug.LogError("曲线参数 curve 的横坐标最小值不能超过0. 当前值:" + animCurve_1[0].time);
                return false;
            }
            //wrapModel校验
            if (animCurve_1.preWrapMode != WrapMode.Clamp)
            {
                animCurve_1.preWrapMode = WrapMode.Clamp;
            }
            if(animCurve_1.postWrapMode != WrapMode.Clamp)
            {
                animCurve_1.postWrapMode = WrapMode.Clamp;
            }

            //TODO 结尾值校验

        }

        return true;
    }

    //loop模式下参数检测
    void CheckLoop()
    {
        //if (Loop_1)
        //{
        //    //loop模式下不支持PosOffset
        //    if (openPosOffset_1)
        //    {
        //        openPosOffset_1 = false;
        //    }
        //}
    }

    #endregion

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




    ////获得随机序列
    //List<int> GetRandomOrder()
    //{
    //    List<int> order = new List<int>();

    //    for (int i = 0; i < _verticesOriPos.Count; i++)
    //    {
    //        order.Add(i);
    //    }

    //    int maxOrder = order.Count >> 2;
    //    for (int i = 0; i < order.Count; i+=4)
    //    {
    //        int randomOrder = Random.Range(0, maxOrder);
    //        int sweapIndex = randomOrder << 2;
    //        for (int j = 0; j < 4; j++)
    //        {
    //            order[i + j] = order[i + j] ^ order[sweapIndex + j];
    //            order[sweapIndex + j] = order[i + j] ^ order[sweapIndex + j];
    //            order[i + j] = order[i + j] ^ order[sweapIndex + j];
    //        }
    //    }

    //    return order;
    //}


}
