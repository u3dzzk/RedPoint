using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPointSystem
{
    public delegate void OnPointNumChange(RedPointNode node);//红点变化通知
    private RedPointNode mRootNode;//红点树Root节点

    private static List<string> lstRedPointTreeList = new List<string> //初始化红点树
    {
        RedPointConst.main,
        RedPointConst.mail,
        RedPointConst.mailSystem,
        RedPointConst.mailTeam,
        RedPointConst.mailAlliance,

        RedPointConst.task,
        RedPointConst.alliance,
    };

    public void InitRedPointTreeNode()
    {
        mRootNode = new RedPointNode();
        mRootNode.nodeName = RedPointConst.main;

        foreach (var item in lstRedPointTreeList)
        {
            var node = mRootNode;
            var treeNodeAy = item.Split('.');
            if (treeNodeAy[0] != mRootNode.nodeName)
            {
                Debug.Log("RedPointTree Root Node Error" + treeNodeAy[0]);
                continue;
                ;
            }

            if (treeNodeAy.Length > 1)
            {
                for (int i = 1; i < treeNodeAy.Length; i++)
                {
                    if (!node.dicChilds.ContainsKey(treeNodeAy[i]))
                    {
                        node.dicChilds.Add(treeNodeAy[i], new RedPointNode());
                    }

                    node.dicChilds[treeNodeAy[i]].nodeName = treeNodeAy[i];
                    node.dicChilds[treeNodeAy[i]].parent = node;

                    node = node.dicChilds[treeNodeAy[i]];
                }
            }
        }
    }

    public void SetRedPointNodeCallBack(string strNode, RedPointSystem.OnPointNumChange callBack)
    {
        var nodeList = strNode.Split('.');//分析树节点
        if (nodeList.Length == 1)
        {
            if (nodeList[0] != RedPointConst.main)
            {
                Debug.Log("Get Wrong Root Mode! Current is " + nodeList[0]);
                return;
            }
        }

        var node = mRootNode;
        for (int i = 1; i < nodeList.Length; i++)
        {
            if (!node.dicChilds.ContainsKey(nodeList[i]))
            {
                Debug.Log("Does Not Contains Child Node" + nodeList[i]);
                return;
            }

            node = node.dicChilds[nodeList[i]];

            if (i == nodeList.Length - 1)
            {
                node.numChangeFunc = callBack;
                return;
            }
        }
    }

    public void SetInvoke(string strNode, int rpNum)
    {
        var nodeList = strNode.Split('.');//分析树节点
        if (nodeList.Length == 1)
        {
            if (nodeList[0] != RedPointConst.main)
            {
                Debug.Log("Get Wrong Root Node! Current is " + nodeList[0]);
                return;
            }
        }

        var node = mRootNode;
        for (int i = 1; i < nodeList.Length; i++)
        {
            if (!node.dicChilds.ContainsKey(nodeList[i]))
            {
                Debug.Log("Does Not Contains Child Node: " + nodeList[i]);
                return;
            }

            node = node.dicChilds[nodeList[i]];

            if (i == nodeList.Length - 1)//最后一个节点了
            {
                node.SetRedPointNum(rpNum); //设置节点的红点数量
            }
        }
    }
    

}
