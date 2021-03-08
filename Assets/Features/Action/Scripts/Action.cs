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

        public bool isTileEffect = false;
        [ShowIf("isTileEffect")]
        public TileEffect tileEffect;
        [HideIf("isTileEffect")]
        public GameObject prefab;
        [Range(0,10)]
        public int paCost;
        [Range(0, 200)]
        public int fatigueDmg;
        public int pushCase;
        [Range(0, 7)]
        public int range;

        public bool onSelf = false;


        [Header("Action")]
        public List<ActionType> actionTypes;
        public System.Type[] actionEffects;


        [ContextMenu("FindActionEffectSubClass")]
        void FindActionEffectSubClass()
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            actionEffects = (from System.Type type in types where type.IsSubclassOf(typeof(ActionEffect)) select type).ToArray();

            //Sort
            actionTypes.Sort((eff1, eff2) => { return eff1.CompareTo(eff2); });
        }


        [ContextMenu("Process")]
        public void Process(Tile casterTile, Tile targetTile, Action action)
        {
            Player player;

            player = targetTile.Player;
            Tile playerTile;
            FindActionEffectSubClass();
            for (int j = 0; j < actionTypes.Count; j++)
            {
                foreach (System.Type item in actionEffects)
                {
                    if (item.Name == actionTypes[j].ToString())
                    {
                        object obj = System.Activator.CreateInstance(item);

                        ActionEffect eff = obj as ActionEffect;

                        if (!eff.Process(casterTile, targetTile, action)) // on essaye l effet sur la case ciblé
                        {
                            if (GridManager.Grid.TryGetTile(player.position, out playerTile))
                            {
                                eff.Process(casterTile, playerTile, action); // le player a peut être été déplacé donc on essaye
                            }
                        }
                    }
                }
            }
           

        }


    }

}

