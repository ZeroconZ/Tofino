using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RE : MonoBehaviour
{

    int id;
    int src;
    int dst;
    int proto;
    bool block;

    Dictionary<int,(int srcR, int dstR, int protoR)> Whitelist = new Dictionary<int,(int, int, int)>();

    public void SrcDev(int val)
    {

        src = val;

        /*switch(val)
        {

            case 0: //Src HMI
                Debug.Log("HMI");
                break;

            case 1: //Src PC
                Debug.Log("PC");
                break;

            case 2: //Src PLC
                Debug.Log("PLC");
                break;

            case 3: //Src DRIVER
                Debug.Log("DRIVER");
                break;

            default:
                break;

        }*/

    }

    public void DstDev(int val)
    {

        dst = val;

        /*switch(val)
        {

            case 0: //Dst HMI
                Debug.Log("HMI");
                break;

            case 1: //Dst PC
                Debug.Log("PC");
                break;

            case 2: //Dst PLC
                Debug.Log("PLC");
                break;

            case 3: //Dst DRIVER
                Debug.Log("DRIVER");
                break;

            default:
                break;

        }*/

    }

    public void ProtoRule(int val)
    {

        proto = val;

        /*switch(val)
        {

            case 0:
                break;
            
            case 1:
                break;

            default:
                break;

        }*/

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
            Debug.Log("Nada que ver");
        else
        {

            id++;

            Whitelist.Add(id, (src, dst, proto));
            Debug.Log("Regla: " + id + " " + Whitelist[id]);

        }

    }

    public void Test()
    {

        if(Whitelist.ContainsValue((0,1,1)))
        {

            Debug.Log("BLOQUEADO PAPU");
        }
        else
        {

            Debug.Log("No existe esa mamada");

        }

    }

}
