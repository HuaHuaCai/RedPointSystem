using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RedpointNode 
{
    public string nodeName;
    public RedpointNode parentNode;
    public int pointNum = 0;
    public Dictionary<string, RedpointNode> dicChilds = new Dictionary<string, RedpointNode>();

    public RedpointSystem.OnRedpointBroadcast numChangeFunc; // 发生变化的回调函数

    public void SetRedpointNum(int num)
    {
        if (dicChilds.Count > 0)
        {
            throw new Exception("红点数量只能设置最后一个子节点");
        }
        pointNum = num;

        //广播
        numChangeFunc?.Invoke(num);
        if(parentNode != null)
        {
            parentNode.ChangePredPointNum();
        }
    }

    public void ChangePredPointNum()
    {
        int num = 0;
        foreach (var node in dicChilds.Values)
        {
            num += node.pointNum;
        }

        if(num != pointNum)
        {
            pointNum = num;
            //广播
            numChangeFunc?.Invoke(num);
        }
        if (parentNode != null) parentNode.ChangePredPointNum();
    }
}
