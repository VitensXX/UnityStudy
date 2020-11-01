using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


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
    private const float INVALID = int.MaxValue - 10;//无效值
    private const float CURVE_MAX_TIME = 1;//曲线横坐标右边界
    private const float CURVE_MIN_TIME = 0;//曲线横坐标左边界

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
        Fadeout = 3,    //淡出
        End = 4         //结束 不做任何处理
    }

    public enum OrderType
    {
        Normal, //正常,顺序
        Random, //随机
        Back,   //反向
    }

    public bool enableAnim;
    public bool unscaledTime;
    public bool playOnAwake = true;
    [Tooltip("Awake的延迟时间")]
    public float awakeDelay;
    [Tooltip("是否循环")]
    public bool loop_0 = false;
    [Tooltip("Loop的间隔时间")]
    public float loopInterval_0 = 5f;
    [Tooltip("Loop间隔时间加随机值")]
    public bool isRandomForLoopInterval_0 = false;
    [Tooltip("Loop间隔时间随机因子范围")]
    public float randomRangeForLoopInterval_0 = 0;


    #region 布局设置
    public Layout layout = Layout.Normal;
    public float radius = 200;
    public float angle = 10;
    public Vector2 italicFactor;

    Layout _lastLayout;
    float _radius;
    float _angle;

    Vector3 _circleCenter;
    Vector3 _textCenter;
    List<Vector3> _verticleYDir;//顶点的Y坐标方向
    #endregion 

   

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

    public OrderType order_1;
    #endregion

    #region 第二阶段属性 持续显示阶段
    public bool stage_2;
    [Tooltip("阶段切换的等待时间")]
    public float waitForStageSwitch_2;
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

    public OrderType order_2;
    #endregion

    #region 第三阶段属性 淡出
    public bool stage_3;
    public Type type_3 = Type.Normal;

    [Tooltip("是否启用强行淡出的一个延迟时间（从awakeDelay过后开始计时）")]
    public bool forceFadeoutDelay_3;
    public float forceFadeoutTime_3;

    [Tooltip("动画曲线,控制每个字的动画节奏")]
    public AnimationCurve animCurve_3;

    [Tooltip("每个字动画的快慢")]
    public float perTextSpeed_3 = 3;
    [Tooltip("每个字的动画间隔时间")]
    public float perTextInterval_3 = 0.3f;

    [Tooltip("是否循环")]
    public bool loop_3 = false;
    [Tooltip("Loop的间隔时间")]
    public float loopInterval_3 = 5f;
    [Tooltip("Loop间隔时间加随机值")]
    public bool isRandomForLoopInterval_3 = false;
    [Tooltip("Loop间隔时间随机因子范围")]
    public float randomRangeForLoopInterval_3 = 0;

    [Tooltip("动画程度因子")]
    public float animFactor_3 = 30;
    [Tooltip("文字颜色(类似于扫光颜色)")]
    public Color textColor_3 = Color.red;

    [Tooltip("是否开启坐标位子偏移")]
    public bool openPosOffset_3 = false;
    [Tooltip("坐标位置偏移")]
    public Vector2 posOffset_3;

    public OrderType order_3;

    public float waitForStageSwitch_3;
    #endregion

   
    #region 阶段相关
    
    Stage _curStage;
    Stage _nextStage;
    float _waitForStageSwitch;
    float _tickForSwitch;
    bool _stageSwiching;//阶段切换中

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
    OrderType _orderType;
    #endregion

    //阶段切换 参数重新赋值
    void SetParamsOnStageChanged()
    {
        if (_curStage == Stage.Fadein)
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
            _orderType = order_1;
            //_x = 0;
        }
        else if (_curStage == Stage.Display)
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
            _orderType = order_2;
            //_x = 0;
        }
        else if(_curStage == Stage.Fadeout)
        {
            _curType = type_3;
            _animCurve = animCurve_3;
            _perTextSpeed = perTextSpeed_3;
            _perTextInterval = perTextInterval_3;
            _loop = loop_3;
            _loopInterval = loopInterval_3;
            _isRandomForLoopInterval = isRandomForLoopInterval_3;
            _randomRangeForLoopInterval = randomRangeForLoopInterval_3;
            _animFactor = animFactor_3;
            _textColor = textColor_3;
            _openPosOffset = openPosOffset_3;
            _posOffset = posOffset_3;
            _orderType = order_3;

            //_x = 1;
            _isFadeout = true;
        }
        else if(_curStage == Stage.End)
        {
            _isFadeIn = enableAnim && fadein_1;
            _isFadeout = false;
            //_x = 0;
        }

        _x = _orderType == OrderType.Back ? 1 : 0;
        //normal的情况下不需要曲线控制节奏,只需要给个默认曲线,便于检测终点即可
        if (_curType == Type.Normal)
        {
            _animCurve = new AnimationCurve(new Keyframe(1, 1), new Keyframe(1, 1));
        }

    }


    //阶段切换的准备操作
    void PrepareSwitchStage()
    {
#if UNITY_EDITOR
        //Editor模式下未运行是不响应阶段切换的
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        if (_curStage == Stage.Fadeout)
        {
            _isFadeIn = enableAnim && fadein_1;
            _isFadeout = false;
        }

        _tickForSwitch = 0;
        _nextStage = GetNextStage();
        if (_nextStage == Stage.Fadein)
        {
            _waitForStageSwitch = _curStage == Stage.Fadeout ? loopInterval_0 + _randomForLoopInterval : awakeDelay;
            _stageSwiching = true;
            //Debug.LogError("准备切换 当前:" + _curStage + "  下一个:" + _nextStage + "  等待时间:" + _waitForStageSwitch);
        }
        else if(_nextStage == Stage.Display)
        {
            _waitForStageSwitch = _curStage == Stage.Fadeout ? loopInterval_0 + _randomForLoopInterval : waitForStageSwitch_2;
            _stageSwiching = true;
            //Debug.LogError("准备切换 当前:" + _curStage + "  下一个:" + _nextStage + "  等待时间:" + _waitForStageSwitch);
        }
        else if(_nextStage == Stage.Fadeout)
        {
            _waitForStageSwitch = _curStage == Stage.Fadeout ? loopInterval_0 + _randomForLoopInterval : waitForStageSwitch_3;
            _stageSwiching = true;
            //Debug.LogError("准备切换 当前:" + _curStage + "  下一个:" + _nextStage + "  等待时间:" + _waitForStageSwitch);
        }
        //else if (_nextStage == Stage.End)
        //{
        //    _isFadeIn = enableAnim && fadein_1;
        //    _isFadeout = false;
        //    _waitForStageSwitch = 3;
        //    _stageSwiching = true;
        //    Debug.LogError("准备切换 当前:" + _curStage + "  下一个:" + _nextStage + "  等待时间:" + _waitForStageSwitch);
        //}
    }

    void SwitchStage()
    {
        _curStage = _nextStage;
        SetParamsOnStageChanged();

        //阶段切换完毕
        _stageSwiching = false;
        _onceEnd = false;
        _tickForLoopWait = 0;
        //Debug.LogError("切换完毕: cur:" + _curStage);
    }

    //获取到下一个阶段
    Stage GetNextStage()
    {
        //未开启动画时,直接跳到End阶段
        if (!enableAnim)
        {
            return Stage.End;
        }

        if (_curStage == Stage.Layout)
        {
            if (fadein_1)
            {
                return Stage.Fadein;
            }
            else if (stage_2)
            {
                return Stage.Display;
            }
            else if (stage_3)
            {
                return Stage.Fadeout;
            }
        }

        if(_curStage == Stage.Fadein)
        {
            if (stage_2)
            {
                return Stage.Display;
            }
            else if (stage_3)
            {
                return Stage.Fadeout;
            }
        }

        if(_curStage == Stage.Display)
        {
            if (stage_3)
            {
                return Stage.Fadeout;
            }
        }

        //开启大循环模式
        if(loop_0)
        {
            if (fadein_1)
            {
                return Stage.Fadein;
            }
            else if (stage_2)
            {
                return Stage.Display;
            }
            else if (stage_3)
            {
                return Stage.Fadeout;
            }
        }

        return Stage.End;
    }
    #endregion



    [Tooltip("自动消失")]
    public bool autoDisappear;

    [Tooltip("显示时长(从AwakeDelay过后就开始计时)")]
    public float duration;


    public bool IsStart { get; private set; } = false;

    float _randomForLoopInterval = 0;
    bool _isFadeIn;
    bool _isFadeout;

    
    public void Play()
    {
        _isFadeIn = fadein_1 && enableAnim;
        _isFadeout = false;
        IsStart = true;
        _onceEnd = false;
        _originColor = _text.color;
        _isFirst = true;
        _tickForShowTime = 0;
        SetParam();
    }

    //从inspector面板获取参数并设置
    void SetParam()
    {
        _lastLayout = layout;
        _angle = angle;
        _radius = radius;
        _forceFadeout = forceFadeoutDelay_3;
    }

    //淡出 和淡入反着来
    public void Fadeout()
    {
        _nextStage = Stage.Fadeout;
        SwitchStage();
    }

    //暂停动画
    public void Pause()
    {
        IsStart = false;
    }

    //停止动画
    public void Stop()
    {
        _curStage = Stage.End;
        _isFadeIn = false;
        IsStart = false;
        _openPosOffset = false;
        _x = 0;
        _stageSwiching = false;
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
    float _tickForLoopWait = 0;//等待计时器
    float _tickForShowTime = 0;//显示时间计时(从AwakeDelay过后就开始计时)
    bool _forceFadeout;
    private void Update()
    {
#if UNITY_EDITOR
        //参数合法性检测
        if (!Application.isPlaying)
        {
            CheckCurves();
        }

        //Relayout校验
        if (_angle != angle || _radius != radius || _lastLayout != layout)
        {
            SetParam();
            Relayout();
        }
#endif

        if (!IsStart)
            return;

        if (_forceFadeout && stage_3)
        {
            _tickForShowTime += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if(_tickForShowTime >= forceFadeoutTime_3 + awakeDelay)
            {
                Fadeout();
                _forceFadeout = false;
            }
        }


        //阶段切换中 需要停止后续的Update逻辑
        if (_stageSwiching)
        {
            _tickForSwitch += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            if (_tickForSwitch >= _waitForStageSwitch)
            {
                SwitchStage();
            }
            return;
        }

        //未开启动画 后面的一切操作都不将执行
        if (!enableAnim)
        {
            return;
        }

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
                _tickForLoopWait += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                if(_tickForLoopWait >= _loopInterval + _randomForLoopInterval)
                {
                    _onceEnd = false;
                    _x = _orderType == OrderType.Back ? CURVE_MAX_TIME : CURVE_MIN_TIME;
                    _tickForLoopWait = 0;
                }
            }
        }
        else
        {
            if (_onceEnd)
            {
                //Debug.LogError("update onceEnd");
                IsStart = false;
                _isFadeIn = false;
                return;
            }
        }

        //if (_fadeout)
        if (_orderType == OrderType.Back)
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
    Canvas _canvas;
    protected override void OnEnable()
    {
        base.OnEnable();

        //如果是开启动画且循环模式下,需要挂一个单独的画布,出于UI动静分离的考虑
        if (enableAnim && (loop_0 || fadein_1 && loop_1 || stage_2 && loop_2 || stage_3 && loop_3) && _canvas == null)
        {
            _canvas = gameObject.GetComponent<Canvas>();
            if (_canvas == null)
            {
                _canvas = gameObject.AddComponent<Canvas>();
            }
        }

        _text = GetComponent<Text>();
        _curStage = Stage.Layout;
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
    int _verticesCountWithTag;//带有完整标签的
    public override void ModifyMesh(VertexHelper helper)
    {
        //Debug.LogError("Modify");
        if (!IsActive() || helper.currentVertCount == 0)
            return;

        _verticesCountWithTag = helper.currentVertCount;

        if (!_richPrcessed)
        {
            ProcessRichTags(_text.text);
            //_verticesCount = _verticesCountWithTag - tag * 4;
        }

        //如果顶点数有变化,说明文本内容有修改,需要清除记录
        if (_verticesOriPos != null && _verticesOriPos.Count != helper.currentVertCount)
        {
            ClearPosRecord();
        }

        List<UIVertex> vertices = new List<UIVertex>();
        helper.GetUIVertexStream(vertices);

        UIVertex v = new UIVertex();

        //记录顶点的初始信息
        RecordVerticesInfo(helper, ref v);
        
        //处理布局阶段
        if (_curStage == Stage.Layout)
        {
            RefreshLayout(helper, ref v);
            //进入下一阶段
            PrepareSwitchStage();
        }
        
        //尝试处理淡入阶段
        if(_curStage == Stage.Fadein && fadein_1)
        {
            //对每个顶点做相应的处理
            ProcessAnim(helper, ref v);
        }
        //尝试处理展示阶段
        else if(_curStage == Stage.Display && stage_2)
        {
            //对每个顶点做相应的处理
            ProcessAnim(helper, ref v);
        }
        //尝试处理淡出阶段
        else if (_curStage == Stage.Fadeout && stage_3)
        {
            //对每个顶点做相应的处理
            ProcessAnim(helper, ref v);
        }
        //else if(_curStage == Stage.End && loop_0)
        //{
        //    //Replay();
        //    PrepareSwitchStage();
        //}
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

    //记录顶点信息
    void RecordVerticesInfo(VertexHelper helper, ref UIVertex v)
    {
        //记录顶点的初始位置
        if (_verticesOriPos == null)
        {
            _verticesOriPos = new List<Vector3>();
            for (int i = 0; i < _verticesCountWithTag; i++)
            {
                helper.PopulateUIVertex(ref v, i);
                _verticesOriPos.Add(v.position);
            }
        }

        //记录每个文字的中心位置
        if (_centerPos == null)
        {
            _centerPos = new List<Vector3>();
            for (int i = 0; i < _verticesCountWithTag; i += 4)
            {
                _centerPos.Add(_verticesOriPos[i] / 2 + _verticesOriPos[i + 2] / 2);
            }
        }

        //随机位子淡入
        if (_orderType == OrderType.Random && _randomOrderList == null)
        {
            //_randomOrderList = MagicTextUtils.GetRandomOrder(_verticesCountWithTag);
            _randomOrderList = GetRandomOrder(_verticesCountWithTag, _verticesCount);
        }
    }

    //刷新布局
    void RefreshLayout(VertexHelper helper, ref UIVertex v)
    {
        //Debug.LogError("refreshLayout");
        //圆形布局,重新计算顶点位置信息
        if (layout == Layout.Circle)
        {
            ClearPosRecord();
            RecordVerticesInfo(helper, ref v);

            Vector3 tempCenter = new Vector3();
            for (int i = 0; i < _centerPos.Count; i++)
            {
                tempCenter += _centerPos[i];
            }

            tempCenter /= _centerPos.Count;
            _circleCenter = new Vector3(tempCenter.x, tempCenter.y - _radius);
            _textCenter = new Vector3(tempCenter.x, tempCenter.y);

            //重新计算每个顶点的布局
            for (int i = 0; i < _verticesCountWithTag; i++)
            {
                if (!IsValidVertice(i))
                {
                    continue;
                }
                helper.PopulateUIVertex(ref v, i);
                CircleLayout(helper, ref v, i);
                helper.SetUIVertex(v, i);
            }
            RecordAfterLayout(helper, ref v);
        }
        else if (layout == Layout.Italic)
        {
            ClearPosRecord();
            RecordVerticesInfo(helper, ref v);

            //重新计算每个顶点的布局
            for (int i = 0; i < _verticesCountWithTag; i++)
            {
                if (!IsValidVertice(i))
                {
                    continue;
                }
                helper.PopulateUIVertex(ref v, i);
                Italic(helper, ref v, i);
                helper.SetUIVertex(v, i);
            }
            RecordAfterLayout(helper, ref v);
        }
    }

    //处理动画
    void ProcessAnim(VertexHelper helper, ref UIVertex v)
    {
        //对每个定点做相应的处理
        for (int i = 0; i < _verticesCountWithTag; i++)
        {
            if (!IsValidVertice(i))
            {
                continue;
            }
            helper.PopulateUIVertex(ref v, i);

            if (_isFadeIn || _isFadeout)
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
            else if (_curType == Type.Color)
            {
                ChangeColor(helper, ref v, i);
            }
           
            helper.SetUIVertex(v, i);
        }
    }

    //在布局后重新记录顶点信息
    void RecordAfterLayout(VertexHelper helper, ref UIVertex v)
    {
        _verticesOriPos.Clear();
        for (int i = 0; i < _verticesCountWithTag; i++)
        {
            helper.PopulateUIVertex(ref v, i);
            _verticesOriPos.Add(v.position);
        }

        _centerPos.Clear();
        for (int i = 0; i < _verticesCountWithTag; i += 4)
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
        for (int i = 0; i < _verticesCountWithTag; i++)
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
        _richPrcessed = false;

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
        ProcessRichTags(_text.text);
        //回到布局阶段
        _curStage = Stage.Layout;
    }

    //重新布局
    public void Relayout()
    {
        ClearPosRecord();
    }

    #region 采样函数

    float SampleAlpha(int index)
    {
        float curveX = CalcCurveX(ref index);

        if (_curStage == Stage.Fadeout)
        {
            if(_orderType == OrderType.Back)
            {
                if (curveX > CURVE_MAX_TIME && (_isFadeIn || _isFadeout))
                {
                    return 1;
                }

                return alphaCurve_1.Evaluate(curveX);
            }
            else
            {
                //未循环 且初始不可见
                if (curveX < CURVE_MIN_TIME && (_isFadeIn || _isFadeout))
                {
                    return 1;
                }

                return 1 - alphaCurve_1.Evaluate(curveX);
            }
        }
        else
        {
            if (_orderType == OrderType.Back)
            {
                if (curveX > CURVE_MAX_TIME && (_isFadeIn || _isFadeout))
                {
                    return INVALID;
                }

                return 1 - alphaCurve_1.Evaluate(curveX);
            }
            else
            {
                //未循环 且初始不可见
                if (curveX < CURVE_MIN_TIME && (_isFadeIn || _isFadeout))
                {
                    return INVALID;
                }

                return alphaCurve_1.Evaluate(curveX);
            }
        }
    }

    //坐标位子偏移采样
    Vector2 SampleOffset(int index)
    {
        float curveX = CalcCurveX(ref index);
        if (_isFadeout && _orderType != OrderType.Back || !_isFadeout && _orderType == OrderType.Back)
        {
            curveX = 1 - curveX;
        }

        if (curveX < CURVE_MIN_TIME || curveX > CURVE_MAX_TIME)
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

        float curveX = CalcCurveX(ref index);

        int endVerticIndex = CalcEndVertIndex();

        if (_orderType == OrderType.Back)
        {
            if (curveX > CURVE_MAX_TIME && (_isFadeIn || _isFadeout))
            {
                return INVALID;
            }

            //最后一个顶点动画结束标识
            if (!_onceEnd && index == endVerticIndex && curveX <= CURVE_MIN_TIME)
            {
                //Debug.LogError(endVerticIndex);
                _onceEnd = true;
                if (!_loop)
                {
                    //切换阶段
                    PrepareSwitchStage();
                }

                //loop间隔随机因子计算
                if (_isRandomForLoopInterval)
                {
                    _randomForLoopInterval = Random.Range(-_randomRangeForLoopInterval, _randomRangeForLoopInterval);
                }
            }
        }
        else
        {
            if (curveX < CURVE_MIN_TIME && (_isFadeIn || _isFadeout))
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
                    PrepareSwitchStage();
                }

                //loop间隔随机因子计算
                if (_isRandomForLoopInterval)
                {
                    _randomForLoopInterval = Random.Range(-_randomRangeForLoopInterval, _randomRangeForLoopInterval);
                }
            }
        }

        if (_orderType == OrderType.Back)
        {
            return _animCurve.Evaluate(1 - curveX);
        }
        else
        {
            return _animCurve.Evaluate(curveX);
        }
    }

    int CalcEndVertIndex()
    {
        int lastNoTagVertIndex = GetLastNoTagVertIndex();
        //动画最后一个顶点的索引 拉伸每四个顶点只有前两个顶点在表现
        int endVerticIndex = _curType == Type.Stretch ? lastNoTagVertIndex - 3 : lastNoTagVertIndex - 1;
        endVerticIndex = endVerticIndex - (_tagOffset[endVerticIndex >> 2] << 2);

        return endVerticIndex;
    }

    float CalcCurveX(ref int index)
    {
        if (_orderType == OrderType.Random)
        {
            index = _randomOrderList[index];
            index = IndexOffsetByTag(index);
        }
        else if (_orderType == OrderType.Back)
        {
            index = _verticesCountWithTag - index - 1;
            index = IndexOffsetByTag(index);
        }
        else
        {
            index = IndexOffsetByTag(index);
        }

        float timeOff = (index >> 2) * _perTextInterval * _perTextSpeed;
        //一个字体网格四个顶点
        //float curveX = _x - (_orderType == OrderType.Back ? -timeOff : timeOff);

        return _x - (_orderType == OrderType.Back ? -timeOff : timeOff);
    }

    #endregion

    #region 效果执行函数

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
            v.position = _verticesOriPos[i];
            return;
        }
        //缩放 当Y为0时，是不能保持原样的，其他的都行 所以需要单独处理
        if(y == 0)
        {
            y = 1;
        }
        Vector3 dir = _verticesOriPos[i] - _centerPos[i / 4];

        v.position = _centerPos[i / 4] + dir * y * _animFactor;

        ProcessOffset(ref v, i);
    }

    void LayoutDefault(VertexHelper helper, ref UIVertex v)
    {
        for (int i = 0; i < _verticesCountWithTag; i++)
        {
            if (!IsValidVertice(i))
            {
                continue;
            }
            helper.PopulateUIVertex(ref v, i);

            if (_isFadeIn || _isFadeout)
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
        //i = IndexOffsetByTag(i);

        int curTextIndex = IndexOffsetByTag(i) >> 2;
        if (_verticesCount / 4 % 2 == 0)
        {
            curAngle = (_verticesCount / 8 - curTextIndex) * _angle;
            if (curTextIndex < _verticesCount / 8)
            {
                curAngle = (_verticesCount / 8 - curTextIndex - 1) * _angle + _angle / 2;
            }
            else
            {
                curAngle =  -(curTextIndex - _verticesCount / 8) * _angle - _angle / 2;
            }
        }
        else
        {
            curAngle = (_verticesCount / 8 - curTextIndex) * _angle;
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
        if (i % 4 == 0)
        {
            v.position.x += italicFactor.x;
            //v.position.x += (_verticesCountWithTag - (i - 0.5f))/ _verticesCountWithTag * italicFactor.x;
            //v.position.x -= (0.5f - i)/ _verticesCountWithTag * italicFactor.y;
        }
        else if(i % 4 == 1)
        {
            v.position.x += italicFactor.x;
            //v.position.x += (_verticesCountWithTag - (i + 0.5f)) / _verticesCountWithTag * italicFactor.x;
            //v.position.x -= (- 0.5f - i) / _verticesCountWithTag * italicFactor.y;
        }
        else
        {
            v.position = _verticesOriPos[i];
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
        v.position = _verticesOriPos[i];
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
    #endregion

    #endregion

    #region 参数校验

    //检测每个阶段的动画曲线
    void CheckCurves()
    {
        CheckCurveInStage1();
        CheckCurveInStage2();
        CheckCurveInStage3();
    }

    //检测第一阶段曲线
    public bool CheckCurveInStage1()
    {
        return CheckOneCurve(animCurve_1, type_1);
    }

    //检测第二阶段曲线
    public bool CheckCurveInStage2()
    {
        return CheckOneCurve(animCurve_2, type_2);
    }

    //检测第三阶段曲线
    public bool CheckCurveInStage3()
    {
        return CheckOneCurve(animCurve_3, type_3);
    }

    public bool CheckOneCurve(AnimationCurve curve, Type type)
    {
        if (curve != null && curve.length > 0)
        {
            //右边界校验 加0.01的范围值 避免误伤
            if (curve[curve.length - 1].time > CURVE_MAX_TIME + .01f)
            {
                Debug.LogError("曲线参数 curve 的横坐标最大值不能超过1,时间可以配合 speed 参数控制. 当前值:" + curve[curve.length - 1].time);
                return false;
            }

            //左边界校验
            if (curve[0].time < CURVE_MIN_TIME - .01f)
            {
                Debug.LogError("曲线参数 curve 的横坐标最小值不能超过0. 当前值:" + curve[0].time);
                return false;
            }
            //wrapModel校验
            if (curve.preWrapMode != WrapMode.Clamp)
            {
                curve.preWrapMode = WrapMode.Clamp;
            }
            if (curve.postWrapMode != WrapMode.Clamp)
            {
                curve.postWrapMode = WrapMode.Clamp;
            }
        }

        return true;
    }

    #endregion

    //输入值是否有效
    bool IsValide(float val)
    {
        return !(val <= INVALID + 1 && val >= INVALID - 1);
    }

    //private MatchCollection GetRegexMatchedTags(string text, out int lengthWithoutTags)
    //{
    //    MatchCollection matchedTags = Regex.Matches(text, REGEX_TAGS);
    //    lengthWithoutTags = 0;
    //    int overallTagLength = 0;

    //    foreach (Match matchedTag in matchedTags)
    //    {
    //        overallTagLength += matchedTag.Length;
    //    }

    //    lengthWithoutTags = text.Length - overallTagLength;
    //    return matchedTags;
    //}

    #region 支持富文本处理
    private const string REGEX_TAGS = @"<b>|</b>|<i>|</i>|<size=.*?>|</size>|<color=.*?>|</color>|<material=.*?>|</material>";

    //富文本标签位记录,按文字索引记录
    List<bool> _textIsTag;
    List<int> _tagOffset;
    List<int> _tagOffsetBack;
    int _lastTextIndex;
    int _firstTextIndex;//第一个有效文本的索引，为了支持倒序
    bool _richPrcessed;//是否已经处理了富文本
    private void ProcessRichTags(string text)
    {
        _richPrcessed = true;
        //如果本身就不需要支持富文本,则不处理
        if (!_text.supportRichText)
        {
            //tags = 0;
            _verticesCount = _verticesCountWithTag;
            return;
        }

        if(_textIsTag == null)
        {
            _textIsTag = new List<bool>();
        }
        _textIsTag.Clear();

        for (int i = 0; i < _text.text.Length; i++)
        {
            _textIsTag.Add(false);
        }

        MatchCollection matchedTags = Regex.Matches(text, REGEX_TAGS);
        int tags = 0;
        int startIndex = 0;
        foreach (Match matchedTag in matchedTags)
        {
            startIndex = text.IndexOf(matchedTag.ToString(), startIndex);
            tags += matchedTag.Length;

            //是tag的做标记
            for (int i = 0; i < matchedTag.ToString().Length; i++)
            {
                _textIsTag[startIndex + i] = true;
            }

            //每次匹配成功后 需要向后移动一个,避免重复tag重复匹配
            startIndex++;
        }

        _verticesCount = _verticesCountWithTag - tags * 4;

        _firstTextIndex = -1;
        //寻找最后一个有效字符(不是tag,因为tag是不会创建网格的)的位置
        for (int i = 0; i < _textIsTag.Count; i++)
        {
            if (!_textIsTag[i])
            {
                _lastTextIndex = i;
            }

            if (_firstTextIndex == -1 && !_textIsTag[i])
            {
                _firstTextIndex = i;
            }
        }

        //tag的位置偏移处理
        if(_tagOffset == null)
        {
            _tagOffset = new List<int>();
        }
        _tagOffset.Clear();

        int totalTagCount = 0;
        //记录每个位置的前方有多少个tag
        for (int i = 0; i < _textIsTag.Count; i++)
        {
            _tagOffset.Add(totalTagCount);
            if (_textIsTag[i])
            {
                totalTagCount++;
            }
        }

        if(_tagOffsetBack == null)
        {
            _tagOffsetBack = new List<int>();
        }
        _tagOffsetBack.Clear();
        totalTagCount = 0;
        //记录每个位置的后方有多少个tag，为了支持 Order back
        for (int i = _textIsTag.Count - 1; i >= 0; i--)
        {
            _tagOffsetBack.Add(totalTagCount);
            if (_textIsTag[i])
            {
                totalTagCount++;
            }
        }
    }

    //是tag标记位的顶点是无效的
    bool IsValidVertice(int i)
    {
        //不支持富文本的,每个位置都是有效位置
        if (!_text.supportRichText)
        {
            return true;
        }

        return !_textIsTag[i >> 2];
    }

    //计算因为tag而带来的索引偏差
    int IndexOffsetByTag(int i)
    {
        //不支持富文本的,每个位置都是有效位置,没有偏差
        if (!_text.supportRichText)
        {
            return i;
        }

        if (_orderType == OrderType.Back)
        {
            return i - (_tagOffsetBack[i >> 2] << 2);
        }
        else
        {
            return i - (_tagOffset[i >> 2] << 2);
        }
    }

    //获取最后一个不是tag的顶点索引
    int GetLastNoTagVertIndex()
    {
        if (!_text.supportRichText)
        {
            return _verticesCountWithTag;
        }

        //if (_orderType == OrderType.Back)
        //{
        //    return (_verticesCount / 4 - _firstTextIndex) << 2;
        //}
        //else
        //{
            return (_lastTextIndex + 1) << 2;
        //}
    }

    #endregion

    //获得随机序列
    public List<int> GetRandomOrder(int len, int validLen)
    {
        List<int> order = new List<int>();

        for (int i = 0; i < len; i++)
        {
            order.Add(i);
        }

        int maxOrder = validLen >> 2;
        for (int i = 0; i < order.Count; i += 4)
        {
            //是tag的,不需要交换 保持原样
            if (!IsValidVertice(i))
            {
                continue;
            }

            int randomOrder = Random.Range(0, maxOrder);
            int temp = 0;
            int textOrder = 0;
            while(temp <= randomOrder)
            {
                if (!_textIsTag[textOrder])
                {
                    temp++;
                }
                textOrder++;
            }
            int sweapIndex = randomOrder* 4 +  _tagOffset[textOrder - 1] * 4;

            for (int j = 0; j < 4; j++)
            {
                int tempV = order[i + j];
                order[i + j] = order[sweapIndex + j];
                order[sweapIndex + j] = tempV;
            }
        }

        return order;
    }

    //void LogArr(List<int> arr)
    //{
    //     string s = "";
    //    for (int i = 0; i < arr.Count; i++)
    //    {
    //        s += arr[i] + " ,";
    //    }
    //    //Debug.LogError(s);
    //}
}
