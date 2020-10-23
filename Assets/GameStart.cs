using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    RedpointSystem system;
    // Start is called before the first frame update
    void Start()
    {
        system = new RedpointSystem();
        system.InitRedpointTreeNode();

        RedpointSystem.AddListener(RedpointType.Chat, RedPointConst.s_chatNodeWorldSub,OnCallbackWorldCub);
        RedpointSystem.AddListener(RedpointType.Chat, RedPointConst.s_chatNodeWorld,OnCallbackWorld);
        RedpointSystem.AddListener(RedpointType.Chat, RedPointConst.s_chatNode, OnCallbackChat);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RedpointSystem.SetRpNum(RedpointType.Chat, RedPointConst.s_chatNodeWorldSub, 2);
            RedpointSystem.SetRpNum(RedpointType.Chat, RedPointConst.s_chatNodeSingle, 2);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            RedpointSystem.RemoveLisnener(RedpointType.Chat, RedPointConst.s_chatNode);
            RedpointSystem.SetRpNum(RedpointType.Chat, RedPointConst.s_chatNodeWorldSub,10);
        }

    }

    void OnCallbackWorldCub(int num)
    {
        Debug.Log("sub:" + num);
    }

    void OnCallbackWorld(int num)
    {
        Debug.Log("world:" + num);
    }

    void OnCallbackChat(int num)
    {
        Debug.Log("Chat:" + num);
    }
}
