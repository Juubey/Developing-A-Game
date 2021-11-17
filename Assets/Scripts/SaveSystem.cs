using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (Player player){
        BinaryFormatter formatter = new BinaryFormatter();

        //string path = System.IO.Path.Combine(Application.persistentDataPath, player.Save);
        string path = Application.persistentDataPath + "/player.Save"; //Get Rid of this eventually
        FileStream stream = new FileStream(path,  FileMode.Create); //Get Rid of this eventually
        

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    

    public static PlayerData LoadPlayer()
    {

        //string path = System.IO.Path.Combine(Application.persistentDataPath, player.Save);
        string path = Application.persistentDataPath + "/player.Save"; //Get Rid of this eventually
        
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path,FileMode.Open))
        { 
                    try{
                        PlayerData data = formatter.Deserialize(stream) as PlayerData;
                        stream.Close();

                        return data;

                    }catch{
                        // Clean up the mess if something goes wrong
                        Debug.LogError("Save file not found in " + path);
                        return null;
                        } 
        }
    }



}
