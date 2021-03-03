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

        [SerializeField]
        private List<ActionEffect> testEffects = new List<ActionEffect>();

        [ContextMenu("Set Action Effects")]
        void SetActionEffects() //A mettre en Init quelque part
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            System.Type[] effectsTypes = (from System.Type type in types where type.IsSubclassOf(typeof(ActionEffect)) select type).ToArray();

            testEffects = new List<ActionEffect>();

            for (int j = 0; j < actionTypes.Count; j++)
            {
                foreach (System.Type item in effectsTypes)
                {
                    if (item.Name == actionTypes[j].ToString())
                    {
                        object obj = System.Activator.CreateInstance(item);

                        ActionEffect eff = obj as ActionEffect;

                        eff.Process();

                        testEffects.Add(eff);

                    }

                }
            }
        }

        [ContextMenu("Do Something")]
        void DoSomething()
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            actionEffects = (from System.Type type in types where type.IsSubclassOf(typeof(ActionEffect)) select type).ToArray();
        }

        [ContextMenu("Display List")]
        void Display()
        {
            foreach (ActionEffect item in testEffects)
            {

                Debug.Log(item);
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

                        ActionEffect eff = obj as ActionEffect;

                        eff.Process();

                        //item.GetMethod("Process").Invoke(obj, null);

                    }

                }
            }
           

        }


    }

}

