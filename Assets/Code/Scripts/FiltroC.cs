using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;

public class Filtro
{

    public Filtro()
    {
    }

    public string[] chopeador(string linea, string separador)
    {

        string[] partesL = linea.Split(separador);

        return partesL;

    }

    public string filtrado(string[] linea, string[] filtro)
    {

        string filtrado = "";

        foreach(string parte in linea)
        {

            bool est = true;

            foreach(string Pfiltro in filtro)
            {

                if(!parte.Contains(Pfiltro))
                {

                    est = false;
                    break;

                }

            }

            if(est)
            {

                filtrado = filtrado + parte  + "|";

            }

        }

        return filtrado;

    }


}
