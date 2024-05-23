using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LargeFileProcessor : MonoBehaviour
{
    public string sourceFilePath = "path/to/large/file.txt";
    public string destinationFolderPath = "path/to/destination/folder";
    public int blockSize = 4096; // Tamaño del bloque de lectura en bytes
    public int batchWriteSize = 1000; // Número de líneas para escribir en un lote

    private StreamReader reader;
    private StreamWriter[] writers;

    void Start()
    {
        // Abre el archivo de origen para lectura
        reader = new StreamReader(sourceFilePath);

        // Inicializa los escritores para los archivos de destino
        writers = new StreamWriter[3];
        writers[0] = new StreamWriter(Path.Combine(destinationFolderPath, "logsFirewall.txt"), true);
        writers[1] = new StreamWriter(Path.Combine(destinationFolderPath, "logsEnforcer.txt"), true);
        writers[2] = new StreamWriter(Path.Combine(destinationFolderPath, "logsSystem.txt"), true);

        // Inicia el proceso de lectura
        StartCoroutine(ProcessLargeFile());
    }

    IEnumerator ProcessLargeFile()
    {
        char[] buffer = new char[blockSize];

        while (!reader.EndOfStream)
        {
            // Lee un bloque del archivo de origen
            int bytesRead = reader.ReadBlock(buffer, 0, blockSize);

            // Convierte el bloque en líneas
            string blockContent = new string(buffer, 0, bytesRead);
            string[] lines = blockContent.Split('\n');

            foreach (string line in lines)
            {
                // Determina el tipo de mensaje y obtén el índice del escritor correspondiente
                string messageType = MessageType(line);
                int writerIndex = GetWriterIndex(messageType);

                // Escribe la línea en el archivo correspondiente
                writers[writerIndex].WriteLine(line);

                // Si se ha alcanzado el tamaño del lote, escribe los lotes en los archivos y libera memoria
                if (writers[writerIndex].BaseStream.Position >= batchWriteSize)
                {
                    writers[writerIndex].Flush();
                }
            }

            yield return null; // Espera hasta el siguiente frame para continuar la iteración
        }

        // Cierra los archivos y libera recursos al finalizar la lectura
        CloseFiles();
    }

    private string MessageType(string line)
    {
        // Aquí implementa tu lógica para determinar el tipo de mensaje (ejemplo simplificado)
        // Supongamos que cada mensaje contiene un prefijo que indica su tipo
        if (line.StartsWith("[Firewall]"))
        {
            return "F";
        }
        else if (line.StartsWith("[Enforcer]"))
        {
            return "E";
        }
        else
        {
            return "TS";
        }
    }

    private int GetWriterIndex(string messageType)
    {
        // Retorna el índice del escritor correspondiente según el tipo de mensaje
        switch (messageType)
        {
            case "F":
                return 0;
            case "E":
                return 1;
            default:
                return 2;
        }
    }

    private void CloseFiles()
    {

        // Cierra los archivos y libera recursos
        reader.Close();
        foreach (StreamWriter writer in writers)
        {
            writer.Close();
        }

    }
}

