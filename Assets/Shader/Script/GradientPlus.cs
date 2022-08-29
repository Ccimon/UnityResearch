using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("UI/Effect/GradientPlus")]
//继承IMeshModifier接口以实现自定义修改UI控件网格
public class GradientPlus : BaseMeshEffect
{
    //初始渐变色
    public Color TopColor = new Color(1, 1, 1, 1);
    //终末渐变色
    public Color BottomColor = new Color(0, 0, 0, 1);

    //变更的初始字索引
    public int originIndex = 0;

    //变更的结束索引位置
    public int terminalIndex = 0;

    //自身text组件
    private Text _text;

    //渐变方式 决定了是横向渐变还是纵向渐变
    private enum Arrangement
    {
        Horizontal,
        Vertical,
    }

    //自定以渐变方向
    [SerializeField] private Arrangement arrangement = Arrangement.Vertical;

    //Start重写
    protected override void Start()
    {
        base.Start();
        //文本组建获取
        _text = GetComponent<Text>();
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        //未激活状态 且 没有任何可见字  不进行修改操作
        if (!IsActive() || vh.currentVertCount < 4) 
        {
            return;
        }

        //首末索引关系判断 如果末尾为0 或者末尾小于首尾 不响应mesh变化
        if (terminalIndex <= 0 || terminalIndex < originIndex)
        {
            return;
        }

        //首尾索引安全判断
        if (originIndex < 0)
        {
            originIndex = 0;
        }

        //末尾索引安全判断  最大为文字个数
        if (terminalIndex > vh.currentVertCount / 4 - 1) 
        {
            terminalIndex = vh.currentVertCount / 4 - 1;
        }

        //这个值 代表的是矩形顶点 一个矩形四个顶点 所以 14个字只会有56个顶点 但是会有84个三角形顶点
        //Debug.Log(vh.currentVertCount);

        List<UIVertex> vertexList = new List<UIVertex>();
        //读取顶点信息写入指定管理集合当中 这里表示的是三角形顶点信息，一个三角面三个顶点 注意顺时针可见
        vh.GetUIVertexStream(vertexList);

        if (arrangement ==Arrangement.Vertical)
        {
            VerticalTrans(vertexList, vh);
        }
        else
        {
            HorizontalTrans(vertexList,vh);
        }
    }

    /// <summary>
    /// 水平渐变 顶点信息颜色变化
    /// </summary>
    /// <param name="vertexList"></param>
    /// <param name="vh"></param>
    private void HorizontalTrans(List<UIVertex> vertexList ,VertexHelper vh)
    {
        if (string.IsNullOrEmpty(_text.text))
        {
            return;
        }

        //记录分隔位置的索引
        string[] splitStr = _text.text.Split('\n', '\r');

        List<int> splitIndex = new List<int>();

        int indexCount = 0;

        for (int i = 0; i < splitStr.Length; i++)
        {
            //获取换行时对应的末位字符
            List<int> temp = CalCharacterIndex(splitStr[i], _text);

            //添加总管理  因为分段管理 每次要累增上端的长度 以获取总体居于位置的索引
            foreach (var item in temp)
            {
                splitIndex.Add(item + indexCount);
            }

            //因为是根据分隔符号进行分隔的  分隔符本身占了一个位置  因此计算的时候 单个循环要加1 
            //但是分隔符\n \r本身不会参与渲染 所以要减1，在这里是为了参与顶点的计算，因此在这里不需要加1，不需要累计分隔符的索引

            //indexCount += splitStr[i].Length + 1;
            indexCount += splitStr[i].Length;
        }


        //========数据截取处理=======
        splitIndex.Add(originIndex);
        splitIndex.Add(terminalIndex);
        splitIndex.Sort();

        //2 6 6 10 13 13 20
        for (int i = splitIndex.Count - 1; i > splitIndex.IndexOf(terminalIndex); i--)
        {
            splitIndex.RemoveAt(i);
        }

        for (int i = splitIndex.LastIndexOf(originIndex) - 1; i >= 0; i--)
        {
            splitIndex.RemoveAt(i);
        }
        //=========数据截取处理=======  6 10 13

        int initIndex = splitIndex[0];
        int vertCountPerWord = 4;

        // 0 6 11 13
        //循环分段数  单行做数据处理
        for (int j = 1; j < splitIndex.Count; j++)
        {
            //初始x位置记录 初始化最高位最低位
            float bottomX = vertexList[initIndex * 6].position.x;
            float topX = vertexList[initIndex * 6].position.x;

            //遍历对应字体所在的顶点信息 计算最大最小位置 以及横向的插值总距离
            //这里注意 不是 <=splitIndex[j]*6  由于分段位置末位字符也是要算进来的，因此顶点数要基于分段末位索引进行计算，也就是加1，同时索引从0开始，末位不等，取不到
            //也就是 0--5 6--11 12--17
            for (int i = initIndex * 6; i < (splitIndex[j] + 1) * 6; i++) 
            {         
                //比较信息录入 初始点x记录  左白右黑 这里黑为最大值 越小越靠近白色
                float temp = vertexList[i].position.x;

                //最左位置记录 当前元素x < 记录x 更新记录x 越来越小，更新最左值
                if (temp < topX)
                {
                    topX = temp;
                }

                //当前x比记录x大的话 记录x的位置 也就是更新最右值
                else if (temp > bottomX)
                {
                    bottomX = temp;
                }
            }

            float fHeight = topX - bottomX;

            //循环遍历单行所有单位字体 循环遍历单行内单个矩形 
            for (int i = initIndex; i <= splitIndex[j]; i++)
            {
                //循环遍历矩形顶点索引
                for (int pIndex = 0; pIndex < vertCountPerWord; pIndex++)
                {
                    //实例化一个顶点信息对象
                    UIVertex vertex = new UIVertex();

                    //顶点索引记录 0 1 2 3 ----对应mesh顺时针方向依次索引
                    int nVerterIndex = i * vertCountPerWord + pIndex;

                    //既然是ref 那么函数结果一定影响了实例化对象的内容变更 也就是拿到矩形的顶点信息
                    vh.PopulateUIVertex(ref vertex, nVerterIndex);
 
                    //以最低点为计算初始位置点，计算当前顶点的到最低点的高度/总的渐变高度 作为插值比例 参与插值计算 
                    vertex.color = Color.Lerp(BottomColor, TopColor, (vertex.position.x - bottomX) / fHeight);

                    //在将顶点信息设置回去
                    vh.SetUIVertex(vertex, nVerterIndex);
                }

            }

            //初始计算索引变更 [0-6] [7-13]  因为记录的时候记录的是末位 因此这里要+1
            initIndex = splitIndex[j] + 1;
        }
    }


    /// <summary>
    /// 记录文本换行时候所对应的行末位字符
    /// </summary>
    /// <param name="content">参与计算的文本</param>
    /// <param name="textComponent">文本控件</param>
    /// <returns>所记录的换行字符以及其对应索引</returns>
    private List<int> CalCharacterIndex(string content, Text textComponent)
    {
        //实例化输出集合
        List<int> result = new List<int>();

        ////文本控件的宽度
        //float width = textComponent.GetComponent<RectTransform>().rect.width;

        //总长度计算
        int totalLength = 0;

        //字体样式获取
        Font myFont = textComponent.font;

        // Request characters to be added to the font texture (dynamic fonts only)
        myFont.RequestCharactersInTexture(content, textComponent.fontSize, textComponent.fontStyle);

        //字符串数组
        char[] arr = content.ToCharArray();

        float width = this.GetComponent<RectTransform>().rect.width;

        //规范 明确  如如何从字体纹理中渲染字符 CharacterInfo
        //Specification for how to render a character from the font texture. See Font.characterInfo.

        for (int i = 0; i < arr.Length; i++)
        {
            //获取文字信息
            myFont.GetCharacterInfo(arr[i], out CharacterInfo charaInfo, textComponent.fontSize);

            //The horizontal distance, rounded to the nearest integer, from the origin of this
            //character to the origin of the next character.
            totalLength += charaInfo.advance;

            //如果超过了文本控件的宽度 就认为换行了
            if (totalLength > width)
            {
                //记录换行时候的字符 添加的上一个 因为这个已经换行了
                result.Add(i - 1);
                //重置 再次运算
                totalLength = 0;

                //=========================需要特别注意=====================
                //当计算该元素的时候 实际上已经发现其换过行了 那么在录入下一次计算的时候该元素应当重新录入计算，因此应当自减 
                i--;
            }
            //刚好换行
            else if (totalLength == width)
            {
                //记录该字符元素
                result.Add(i);
                //重置 直到下次再超过控件宽度
                totalLength = 0;
            }
        }

        //单独对末尾的字符进行处理 如果索引计算并没有加入进去 说明不会是刚好分整数行的情况 单独进行加入
        if (!result.Contains(content.Length - 1))
        {
            //单独添加末位元素
            result.Add(content.Length - 1);
        }

        return result;
    }


    /// <summary>
    /// 垂直渐变 顶点颜色信息变化
    /// </summary>
    /// <param name="vertexList"></param>
    /// <param name="vh"></param>
    private void VerticalTrans(List<UIVertex> vertexList, VertexHelper vh)
    {
        //处理不掉换行符的情况  换行符也会累加进入该计算当中 经观测实际上是不会对无法显示的东西进行网格渲染的 比如换行 或者空字符
        //因此有多少个实体字完全可以由顶点除以6去解决

        //int wordCount =0
        //foreach (var item in _text.text.ToCharArray())
        //{
        //    if (item != ' ')
        //    {
        //        wordCount++;
        //    }
        //}
        //

        //有多少个可见字 即有多少个矩形
        int wordCount = vertexList.Count / 6;

        //没有意义了 从0开始的 但是给了我灵感应--可以达到只变更部分字体渐变的效果
        //int beginIndex = vertexList.Count - wordCount * 6;

        //遍历所有顶点 计算出总的渐变高度
        float fHeight = 0;

        //测试顶点位置   012 230 这样的效果 顶点依次从左上顺时针四个顶点索引
        //for (int i = 0; i < 6; i++)
        //{
        //    Debug.Log(vertexList[i].position);
        //}

        //遍历顶点集合  单个矩形内操作
        for (int i = 0; i < wordCount; i++)
        {
            //第0个顶点y  与第四个顶点的y  也就是单个字体内 左高
            float leftH = vertexList[i * 6].position.y - vertexList[i * 6 + 4].position.y;
            //第一个顶点y 与 第二个顶点y   也就是单个字体内右高
            float rightH = vertexList[i * 6 + 1].position.y - vertexList[i * 6 + 2].position.y;

            //找出最大的单位间距
            fHeight = Mathf.Max(leftH, rightH, fHeight);
        }

        //这么一个操作下来拿到了单位字体内 最大的一个垂直间距


        #region 计算每个文字底部posY
        //List<float> minPosY = new List<float>();
        ////循环遍历字体 0-5  6-11 12-17 初始索引位应当都是6的i倍数
        //for (int i = 0; i < wordCount; i++)
        //{
        //    //单个字体内 左下
        //    int leftLowp = i * 6 + 4;
        //    //单个字体内 右下
        //    int rightLowp = i * 6 + 2;
        //    //拿到其中最小的一个posY
        //    float tem = Mathf.Min(vertexList[leftLowp].position.y, vertexList[rightLowp].position.y);
        //    //管理集合添加记录
        //    minPosY.Add(tem);
        //}
        #endregion
   
        // 如果字体未发生任何形变的话 那么这两个y其实是相等的  true
        //Debug.Log(vertexList[4].position.y == vertexList[2].position.y);

        List<float> minPosY = new List<float>();
        for (int i = 0; i < wordCount; i++)
        {
            //单个字体内一定是左下或者右下 这里随便取一个
            minPosY.Add(vertexList[i * 6 + 4].position.y);
        }

        //int vertCountPerWord = 4;
        ////56  84 矩形顶点数 以及 三角形顶点数 一定是不等的
        //if (vh.currentVertCount == vh.currentIndexCount)
        //{
        //    vertCountPerWord = 6;
        //}


        //开始的时候 直接写成了6  那么久导致了索引越界
        //对于顶点颜色信息的变更 不需要三角形顶点去算 有些计算是多余的
        int vertCountPerWord = 4;

        //循环遍历所有单位字体 循环遍历单个矩形  初始 终末
        for (int index = originIndex; index <= terminalIndex; index++) 
        {
            //循环遍历矩形顶点索引
            for (int pIndex = 0; pIndex < vertCountPerWord; pIndex++)
            {
                //实例化一个顶点信息对象
                UIVertex vertex = new UIVertex();

                //顶点索引记录 0 1 2 3 ----对应mesh顺时针方向依次索引
                int nVerterIndex = index * vertCountPerWord + pIndex;

                //既然是ref 那么函数结果一定影响了实例化对象的内容变更 也就是拿到矩形的顶点信息
                vh.PopulateUIVertex(ref vertex, nVerterIndex);


                //-413  120 
                //-306 120 
                //-306 20
                //-13 20
                //if (index ==0)
                //{
                //    Debug.Log(vertex.position);
                //}

                //以最低点为计算初始位置点，计算当前顶点的到最低点的高度/总的渐变高度 作为插值比例 参与插值计算 
                //那就是底部偏黑 顶部偏白  实际计算上，minPos[index]其实就是左下或者右下posY点，也就是左下右下在计算时一定纯黑，而顶部位置不一定纯白
                vertex.color = Color.Lerp(BottomColor, TopColor, (vertex.position.y - minPosY[index]) / fHeight);

                //在将顶点信息设置回去
                vh.SetUIVertex(vertex, nVerterIndex);
            }
        }
    }
}
