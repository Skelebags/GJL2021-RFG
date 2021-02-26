using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SaveFileHandler : MonoBehaviour
{
    string save_directory;
    private string save_path;

    private string default_name = "NO SAVE DATA";

    private const int MAX_SAVES = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Get the save data directory path
        save_directory = System.IO.Path.Combine(Application.persistentDataPath, "save_data_");

        // If the directory does not exist, make it
        if (!System.IO.Directory.Exists(save_directory))
        {
            System.IO.Directory.CreateDirectory(save_directory);
        }

        // Find or create the save data files
        for(int save_slot = 0; save_slot < MAX_SAVES; save_slot++)
        {
            save_path = System.IO.Path.Combine(save_directory, "slot_" + save_slot.ToString() + ".json");

            //Debug.Log(save_path);

            if (System.IO.File.Exists(save_path))
            {
                JSONNode S = JSON.Parse(System.IO.File.ReadAllText(save_path));
                
            }
            else
            {

                System.IO.File.Create(save_path).Dispose();
                Debug.Log("Created save file at: " + save_path);

                JSONObject save_data = new JSONObject();

                // DEFAULT SAVE SLOT NAME
                save_data.Add("Save_Name", default_name);
                // STARTING MONEY OF 0
                save_data.Add("Money", 0);
                // START AT DAY 1
                save_data.Add("Day", 1);

                // INVENTORY STARTS EMPTY
                JSONArray inventory = new JSONArray();
                for(int i = 0; i < 9; i++)
                {
                    JSONObject item = new JSONObject();
                    item.Add("id", "none");
                    inventory.Add(item);
                }
                save_data.Add("Inventory", inventory);

                // QUANTITY ARRAY STARTS ALL 0s
                JSONArray quantities = new JSONArray();
                for(int i = 0; i < 9; i++)
                {
                    quantities.Add("slot " + i, 0);
                }
                save_data.Add("Quantities", quantities);
                
                System.IO.File.WriteAllText(save_path, save_data.ToString());
            }
        }

        
    }

    public void Save(int save_slot, JSONObject saveData)
    {
        save_path = System.IO.Path.Combine(save_directory, "slot_" + save_slot.ToString() + ".json");

        string file_string = System.IO.File.ReadAllText(save_path);

        JSONNode S = JSON.Parse(file_string);

        if (S["Save_Name"] != null && saveData["Save_Name"] != null)
        {
            S["Save_Name"] = saveData["Save_Name"].Value;
        }
        if (S["Money"] != null && saveData["Money"] != null)
        {
            S["Money"] = saveData["Money"].AsInt;
        }
        if (S["Day"] != null && saveData["Day"] != null)
        {
            S["Day"] = saveData["Day"].AsInt;
        }
        if (S["Inventory"] != null && saveData["Inventory"] != null)
        {
            S["Inventory"] = saveData["Inventory"].AsArray;
        }
        if(S["Quantities"] != null && saveData["Quantities"] != null)
        {
            S["Quantities"] = saveData["Quantities"].AsArray;
        }

        System.IO.File.WriteAllText(save_path, S.ToString());
    }

    public JSONNode Load(int save_slot)
    {
        string load_path = System.IO.Path.Combine(save_directory, "slot_" + save_slot.ToString() + ".json");

        string file_string = System.IO.File.ReadAllText(load_path);

        return JSON.Parse(file_string);
    }
}
