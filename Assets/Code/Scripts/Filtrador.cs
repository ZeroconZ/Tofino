using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;


public class Filtrador : MonoBehaviour
{

    StreamReader reader;
    Filtro tolva;
    
    void Start()
    {
        
        tolva = new Filtro();

        string ruta = Application.dataPath + "/Docs" + "/Logs" + "/logstofino.txt";
        reader = new StreamReader(ruta); 

    }

    // Update is called once per frame
    void Update()
    {
        
        if(!reader.EndOfStream && reader != null)
        {

            string linea = reader.ReadLine();
            string[] lineaChop = tolva.chopeador(linea, "|");
            string[] Pfiltro = {"Tofino", ":"};

            string lineaF = tolva.filtrado(lineaChop, Pfiltro);

            Debug.Log(lineaF);

        }

    }

    void OnDestroy()
    {

        reader.Close();

    }
    
}
