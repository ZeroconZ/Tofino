using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RE : MonoBehaviour
{

    public static RE instance;

    int id;
    int src;
    int dst;
    int proto;
    bool block;

    Dictionary<int,(int srcR, int dstR, int protoR)> Whitelist = new Dictionary<int,(int, int, int)>();

    void Awake()
    {

        if(RE.instance == null)
            RE.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    public void SrcDev(int val)
    {

        src = val;

    }

    public void DstDev(int val)
    {

        dst = val;

    }

    public void ProtoRule(int val)
    {

        proto = val;

    }

    public void Block(int val)
    {

        if(val == 0)
            block = false;
        else
            block = true;

    }

    public void Apply()
    {

        if(src == dst)
        {

            Debug.Log("Source and Destiny cannot be the same");

        }
        else if(block == false)
            return;

        else if(Whitelist.ContainsValue((src, dst, proto)))
            return;

        else
        {

            id++;

            Whitelist.Add(id, (src, dst, proto));
            Debug.Log("Regla: " + id + " " + Whitelist[id]);

        }

    }

    public bool CheckRule((int srcMsg, int dstMsg, int protoMsg)msg)
    {

        if(Whitelist.ContainsValue(msg))
        {

            return true; //Does not blocks the msg

        }
        else
        {

            return false; //Blocks the msg

        }

    }

}
