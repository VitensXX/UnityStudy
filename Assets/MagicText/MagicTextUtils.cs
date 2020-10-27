using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTextUtils
{
    //获得随机序列
    public static List<int> GetRandomOrder(int len)
    {
        List<int> order = new List<int>();

        for (int i = 0; i < len; i++)
        {
            order.Add(i);
        }

        int maxOrder = order.Count >> 2;
        for (int i = 0; i < order.Count; i += 4)
        {
            int randomOrder = Random.Range(0, maxOrder);
            int sweapIndex = randomOrder << 2;
            for (int j = 0; j < 4; j++)
            {
                order[i + j] = order[i + j] ^ order[sweapIndex + j];
                order[sweapIndex + j] = order[i + j] ^ order[sweapIndex + j];
                order[i + j] = order[i + j] ^ order[sweapIndex + j];
            }
        }

        return order;
    }

}
