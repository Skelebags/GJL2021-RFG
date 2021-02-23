using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/// <summary>
/// Reads a JSON file and compiles a list of the components and their data
/// </summary>
public class ComponentList : MonoBehaviour
{
    public class Component
    {
        public string name;
        public string slot;
        public Dictionary<string, float[]> effectsDict = new Dictionary<string, float[]>() { {"str", new float[2] { 0f, 1f} }, { "int", new float[2] { 0f, 1f } }, { "dex", new float[2] { 0f, 1f } } };
        //public List<string> effects = new List<string>();
        public int cost;
        public enum Tags { any, meat, mineral, plant};
        public List<Tags> tags = new List<Tags>();
        public int tier;
    }

    public TextAsset component_list_file;

    public List<Component> Data  = new List<Component>();

    /// <summary>
    /// Reads all the component data from the JSON file
    /// </summary>
    public void PopulateComponentsFromJSON()
    {
        string jsonString = component_list_file.text;

        //Debug.Log(jsonString);

        JSONNode N = JSON.Parse(jsonString);



        JSONArray array = N["Components"].AsArray;
        
        // Grab data from JSONArray
        for(int i = 0; i < array.Count; i++)
        {
            // Create a new component and get its name and slot type from the JSONArray
            Component component = new Component();
            component.name = array[i]["name"].Value;
            component.slot = array[i]["slot"].Value;

            // Read the effects as a new Array
            JSONArray effectArray = array[i]["effects"].AsArray;
            for(int j = 0; j < effectArray.Count; j++)
            {
                //component.effects.Add(effectArray[j].Value);
                // Check if the current effect is a strength effect, if it is add it to the dictionary
                if(effectArray[j]["str"] != null)
                {
                    JSONArray strArray = effectArray[j]["str"].AsArray;
                    component.effectsDict["str"] = new float[2] { component.effectsDict["str"][0] + strArray[0]["mod"].AsFloat, component.effectsDict["str"][1] * strArray[1]["mult"].AsFloat };
                    //Debug.Log(component.effectsDict["str"][0] + " : " + component.effectsDict["str"][1]);
                }
                else if (effectArray[j]["int"] != null) // Do the same for int
                {
                    JSONArray strArray = effectArray[j]["int"].AsArray;
                    component.effectsDict["int"] = new float[2] { component.effectsDict["int"][0] + strArray[0]["mod"].AsFloat, component.effectsDict["int"][1] * strArray[1]["mult"].AsFloat };
                    //Debug.Log(component.effectsDict["int"][0] + " : " + component.effectsDict["int"][1]);
                }
                else if(effectArray[j]["dex"] != null) // And dex
                {
                    JSONArray strArray = effectArray[j]["dex"].AsArray;
                    component.effectsDict["dex"] = new float[2] { component.effectsDict["dex"][0] + strArray[0]["mod"].AsFloat, component.effectsDict["dex"][1] * strArray[1]["mult"].AsFloat };
                    //Debug.Log(component.effectsDict["dex"][0] + " : " + component.effectsDict["dex"][1]);
                }

            }

            // Get the component's cost
            component.cost = array[i]["cost"].AsInt;

            // Get the component's tags
            JSONArray tagsArray = array[i]["tags"].AsArray;

            for(int j = 0; j < tagsArray.Count; j++)
            {
                component.tags.Add((Component.Tags)System.Enum.Parse(typeof(Component.Tags), tagsArray[j].Value));
            }

            foreach(Component.Tags tag in component.tags)
            {
                Debug.Log(tag.ToString());
            }

            // Get the component's tier
            component.tier = array[i]["tier"].AsInt;

            Data.Add(component);
        }

    }
}
