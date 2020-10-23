using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedpointSystem
{
    static RedpointSystem s_instance;

    public delegate void OnRedpointBroadcast(int num);

    //红点树各个类型的子节点名称
    Dictionary<RedpointType, string[]> _redpointTreeDict;

    // 红点树Root节点
    Dictionary<RedpointType,RedpointNode>  _rootNodeDic; 

    string[] _chatTreeList;

    public RedpointSystem()
    {
        s_instance = this;
        _redpointTreeDict =  new Dictionary<RedpointType, string[]>();
        _rootNodeDic = new Dictionary<RedpointType, RedpointNode>();

        CreateTreeList();

    }

    void CreateTreeList()
    {
        _chatTreeList = new string[]
        {
            RedPointConst.s_chatNode,
            RedPointConst.s_chatNodeWorld,
            RedPointConst.s_chatNodeSingle,
            RedPointConst.s_chatNodeWorldSub,
        };
        _redpointTreeDict.Add(RedpointType.Chat, _chatTreeList);
    }

    public void InitRedpointTreeNode()
    {
        var chatRootNode = new RedpointNode();
        chatRootNode.nodeName = _chatTreeList[0];
        _rootNodeDic.Add(RedpointType.Chat, chatRootNode);
        CreateNodes(chatRootNode, _chatTreeList);

        var activityNode = new RedpointNode();
    }

    public static void SetRpNum(RedpointType type, string nodeName, int rpNum)
    {
        var nodeList = nodeName.Split('.');

        RedpointNode node;
        if(s_instance._rootNodeDic.TryGetValue(type, out node))
        {
            if (nodeList[0] != node.nodeName)
            {
                throw new Exception(string.Format("RedpointType和NodeName不对应,RedpointType:{0},NodeName:{1}",type, node.nodeName));
            }

            for (int i = 1; i < nodeList.Length; i++)
            {
                var subNodeName = nodeList[i];
                if (!node.dicChilds.ContainsKey(subNodeName))
                {
                    Debug.Log("Does Not Contains Child Node :" + nodeList[i]);
                    return;
                }
                node = node.dicChilds[subNodeName];
                if (i == nodeList.Length - 1)
                {
                    node.SetRedpointNum(rpNum);
                }
            }
        }
    }

    public static void AddListener(RedpointType type, string nodeName, OnRedpointBroadcast callback)
    {
        var nodeList = nodeName.Split('.');
        RedpointNode node;
        if (s_instance._rootNodeDic.TryGetValue(type, out node))
        {
            if (nodeList.Length == 1 && node.nodeName == nodeList[0])
            {
                node.numChangeFunc = callback;
                return;
            }
            else if (nodeList[0] != node.nodeName)
            {
                throw new Exception(string.Format("RedpointType和NodeName不对应,RedpointType:{0},NodeName:{1}", type, nodeName));
            }

            for (int i = 1; i < nodeList.Length; i++)
            {
                var subNodeName = nodeList[i];
                if (!node.dicChilds.ContainsKey(subNodeName))
                {
                    Debug.Log("Does Not Contains Child Node :" + nodeList[i]);
                    return;
                }
                node = node.dicChilds[subNodeName];
                if (i == nodeList.Length - 1)
                {
                    node.numChangeFunc = callback;
                }
            }
        }
    }

    public static void RemoveLisnener(RedpointType type, string nodeName)
    {
        var nodeList = nodeName.Split('.');
        RedpointNode node;
        if (s_instance._rootNodeDic.TryGetValue(type, out node))
        {
            if (nodeList.Length == 1 && node.nodeName == nodeList[0])
            {
                if(node.numChangeFunc !=  null) node.numChangeFunc = null;
                return;
            }
            else if (nodeList[0] != node.nodeName)
            {
                throw new Exception(string.Format("RedpointType和NodeName不对应,RedpointType:{0},NodeName:{1}", type, nodeName));
            }

            for (int i = 1; i < nodeList.Length; i++)
            {
                var subNodeName = nodeList[i];
                if (!node.dicChilds.ContainsKey(subNodeName))
                {
                    Debug.Log("Does Not Contains Child Node :" + nodeList[i]);
                    return;
                }
                node = node.dicChilds[subNodeName];
                if (i == nodeList.Length - 1)
                {
                    if(node.numChangeFunc != null) node.numChangeFunc = null;
                }
            }
        }
    }


    void CreateNodes(RedpointNode treeRootNode, string[] list)
    {
        foreach (var tree in list)
        {
            var node = treeRootNode;
            var treeNodeAy = tree.Split('.');
            if (treeNodeAy[0] != node.nodeName)
            {
                Debug.Log("RedPointTree Root Node Error:" + treeNodeAy[0]);
                continue;
            }
            if (treeNodeAy.Length > 1)
            {
                for (int i = 1; i < treeNodeAy.Length; i++)
                {
                    var treeName = treeNodeAy[i];
                    if (!node.dicChilds.ContainsKey(treeName))
                    {
                        node.dicChilds.Add(treeName, new RedpointNode());
                    }
                    var tmpNode = node.dicChilds[treeName];
                    tmpNode.nodeName = treeName;
                    tmpNode.parentNode = node;
                    node = tmpNode;
                }
            }
        }
    }
}
