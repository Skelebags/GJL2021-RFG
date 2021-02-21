using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ComponentList : MonoBehaviour
{
    public class Component
    {
        public string name;
        public string slot;
        public List<string> effects = new List<string>();
    }


    public enum ComponentsEnum { Colour, Descriptor, Type };

    public TextAsset component_list_file;

    
    
    public List<Component> Data  = new List<Component>();


    public void PopulateComponentsFromJSON()
    {
        string jsonString = component_list_file.text;

        //Debug.Log(jsonString);

        JSONNode N = JSON.Parse(jsonString);

        JSONArray array = N["Components"].AsArray;
        
        
        for(int i = 0; i < array.Count; i++)
        {
            Component component = new Component();
            component.name = array[i]["name"].Value;
            component.slot = array[i]["slot"].Value;
            JSONArray effectArray = array[i]["effects"].AsArray;
            for(int j = 0; j < effectArray.Count; j++)
            {
                component.effects.Add(effectArray[j].Value);
            }
            Data.Add(component);
        }
    }
}
