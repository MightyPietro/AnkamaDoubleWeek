using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace WeekAnkama
{
    [CreateAssetMenu(menuName = "Assets/Action")]
    public class Action : ScriptableObject
    {
        
        [Range(0,10)]
        public int paCost;
        [Range(0, 200)]
        public int fatigueDmg;
        public GameObject prefab;

        [Header("Action")]
        public List<ActionType> actionTypes;
        public System.Type[] actionEffects;


        [ContextMenu("FindActionEffectSubClass")]
        void FindActionEffectSubClass()
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            actionEffects = (from System.Type type in types where type.IsSubclassOf(typeof(ActionEffect)) select type).ToArray();
        }


        [ContextMenu("Process")]
        public void Process(Tile targetTile, Action action)
        {
            FindActionEffectSubClass();
            for (int j = 0; j < actionTypes.Count; j++)
            {
                foreach (System.Type item in actionEffects)
                {
                    if (item.Name == actionTypes[j].ToString())
                    {
                        object obj = System.Activator.CreateInstance(item);

                        ActionEffect eff = obj as ActionEffect;

                        eff.Process(targetTile, action);


                    }

                }
            }
           

        }


    }

}

