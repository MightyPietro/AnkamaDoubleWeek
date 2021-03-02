using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace WeekAnkama
{
    [CreateAssetMenu(menuName = "Assets/Action")]
    public class Action : ScriptableObject
    {
        public enum ActionType { Push, Attract, Damage };
        public List<ActionType> actionTypes;
        public System.Type[] actionEffects;


        [ContextMenu("Do Something")]
        void DoSomething()
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            actionEffects = (from System.Type type in types where type.IsSubclassOf(typeof(ActionEffect)) select type).ToArray();
        }

        [ContextMenu("Display List")]
        void Display()
        {
            foreach (System.Type item in actionEffects)
            {

                //Debug.Log(item);
                object obj = System.Activator.CreateInstance(item);
                Debug.Log(obj);
            }

        }

        [ContextMenu("Process")]
        public void Process()
        {
            DoSomething();
            for (int j = 0; j < actionTypes.Count; j++)
            {
                foreach (System.Type item in actionEffects)
                {
                    if (item.Name == actionTypes[j].ToString())
                    {
                        object obj = System.Activator.CreateInstance(item);
                        item.GetMethod("Process").Invoke(obj, null);

                    }

                }
            }
           

        }


    }

}

