//Script from https://www.youtube.com/watch?v=XOjd_qU2Ido

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/player.agf";

    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("ERROR: Save file not found in " + path);
            return null;
        }
    }

    public static void DestroySaveFile()
    {
        string path = Application.persistentDataPath + "/player.agf";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Succesfully deleted save file");
        }
        else
        {
            Debug.LogError("ERROR: Unable to find file with specified path" + path);
        }
    }

    public static bool DoesFileExist()
    {
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
