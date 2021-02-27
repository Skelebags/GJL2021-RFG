using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reads a JSON file and compiles a list of the components and their data
/// </summary>
public class PartList : MonoBehaviour
{
    /// <summary>
    /// The data class for the Parts
    /// </summary>
    public class Part
    {
        // The ID of the part
        public int id;
        public string part_name;

        // What slot in the ingredient does this part occupy
        public string slot;

        // Dictionary of the part's effects
        public Dictionary<string, float[]> effectsDict = new Dictionary<string, float[]>() { {"str", new float[2] { 0f, 1f} }, { "int", new float[2] { 0f, 1f } }, { "dex", new float[2] { 0f, 1f } } };
        public int cost;

        // The part's tags
        public enum Tags { any, meat, mineral, plant, blessed, toxic};
        public List<Tags> tags = new List<Tags>();

        // The part's tier (will only appear in shops of the same tier or higher)
        public int tier;

        // The path in the resources file to this part's sprite
        public string graphic_path;

        // The actual sprite
        public Sprite sprite;

        // The part's flavour text
        public string desc_string;
    }

    // The file the part data is stored in
    public TextAsset part_list_file;

    // The parts arranged by ID
    public Dictionary<int, Part> Data  = new Dictionary<int, Part>();

    /// <summary>
    /// Reads all the part data from the JSON file
    /// </summary>
    public void PopulatePartsFromJSON()
    {
        // Grab the JSON from the file as a string
        string jsonString = part_list_file.text;
        
        // Part is with SimpleJSON
        JSONNode N = JSON.Parse(jsonString);

        // The whole file is one BIG array
        JSONArray array = N["Parts"].AsArray;
        
        // Grab data from JSONArray
        for(int i = 0; i < array.Count; i++)
        {
            // Create a new part and get its name and slot type from the JSONArray
            Part part = new Part();
            part.id = array[i]["id"].AsInt;
            part.part_name = array[i]["name"].Value;
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
                    }
                    else if (effectArray[j]["int"] != null) // Do the same for int
                    {
                        JSONArray strArray = effectArray[j]["int"].AsArray;
                        part.effectsDict["int"] = new float[2] { part.effectsDict["int"][0] + strArray[0]["mod"].AsFloat, part.effectsDict["int"][1] * strArray[1]["mult"].AsFloat };
                    }
                    else if (effectArray[j]["dex"] != null) // And dex
                    {
                        JSONArray strArray = effectArray[j]["dex"].AsArray;
                        part.effectsDict["dex"] = new float[2] { part.effectsDict["dex"][0] + strArray[0]["mod"].AsFloat, part.effectsDict["dex"][1] * strArray[1]["mult"].AsFloat };
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

            // Add the part data to the dictionary, use its ID as the index
            Data.Add(part.id, part);
        }

    }
}
