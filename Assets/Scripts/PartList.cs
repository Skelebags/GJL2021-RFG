using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reads a JSON file and compiles a list of the components and their data
/// </summary>
public class PartList : MonoBehaviour
{
    public class Part
    {
        public int id;
        public string name;
        public string slot;
        public Dictionary<string, float[]> effectsDict = new Dictionary<string, float[]>() { {"str", new float[2] { 0f, 1f} }, { "int", new float[2] { 0f, 1f } }, { "dex", new float[2] { 0f, 1f } } };
        //public List<string> effects = new List<string>();
        public int cost;
        public enum Tags { any, meat, mineral, plant, blessed, toxic};
        public List<Tags> tags = new List<Tags>();
        public int tier;
        public string graphic_path;

        public Sprite sprite;

        public string desc_string;
    }

    public TextAsset part_list_file;

    public Dictionary<int, Part> Data  = new Dictionary<int, Part>();

    /// <summary>
    /// Reads all the part data from the JSON file
    /// </summary>
    public void PopulatePartsFromJSON()
    {
        string jsonString = part_list_file.text;

        //Debug.Log(jsonString);

        JSONNode N = JSON.Parse(jsonString);

        JSONArray array = N["Parts"].AsArray;
        
        // Grab data from JSONArray
        for(int i = 0; i < array.Count; i++)
        {
            // Create a new part and get its name and slot type from the JSONArray
            Part part = new Part();
            part.id = array[i]["id"].AsInt;
            part.name = array[i]["name"].Value;
            part.slot = array[i]["slot"].Value;

            // Read the effects as a new Array if the part has any effects
            if (array[i]["effects"] != null)
            {
                JSONArray effectArray = array[i]["effects"].AsArray;
                for (int j = 0; j < effectArray.Count; j++)
                {
                    // Check if the current effect is a strength effect, if it is add it to the dictionary
                    if (effectArray[j]["str"] != null)
                    {
                        JSONArray strArray = effectArray[j]["str"].AsArray;
                        part.effectsDict["str"] = new float[2] { part.effectsDict["str"][0] + strArray[0]["mod"].AsFloat, part.effectsDict["str"][1] * strArray[1]["mult"].AsFloat };
                        //Debug.Log(component.effectsDict["str"][0] + " : " + component.effectsDict["str"][1]);
                    }
                    else if (effectArray[j]["int"] != null) // Do the same for int
                    {
                        JSONArray strArray = effectArray[j]["int"].AsArray;
                        part.effectsDict["int"] = new float[2] { part.effectsDict["int"][0] + strArray[0]["mod"].AsFloat, part.effectsDict["int"][1] * strArray[1]["mult"].AsFloat };
                        //Debug.Log(component.effectsDict["int"][0] + " : " + component.effectsDict["int"][1]);
                    }
                    else if (effectArray[j]["dex"] != null) // And dex
                    {
                        JSONArray strArray = effectArray[j]["dex"].AsArray;
                        part.effectsDict["dex"] = new float[2] { part.effectsDict["dex"][0] + strArray[0]["mod"].AsFloat, part.effectsDict["dex"][1] * strArray[1]["mult"].AsFloat };
                        //Debug.Log(component.effectsDict["dex"][0] + " : " + component.effectsDict["dex"][1]);
                    }

                }
            }

            // Get the part's cost
            part.cost = array[i]["cost"].AsInt;

            // Get the part's tags
            JSONArray tagsArray = array[i]["tags"].AsArray;

            for(int j = 0; j < tagsArray.Count; j++)
            {
                part.tags.Add((Part.Tags)System.Enum.Parse(typeof(Part.Tags), tagsArray[j].Value));
            }

            // Get the part's tier
            part.tier = array[i]["tier"].AsInt;

            // Get the part's graphic file path
            part.graphic_path = array[i]["graphic"].Value;
            
            part.sprite = Resources.Load<Sprite>(part.graphic_path);

            // Get the part's description
            part.desc_string = array[i]["desc"].Value;

            Data.Add(part.id, part);
        }

    }
}
