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
        [Header("Common Values")]
        [TextArea(2, 5)]
        public string description;
        [Range(0, 10)]
        public int paCost;
        public int bonusPA;
        [Range(0, 200)]
        public int fatigueDmg;
        [Range(0,10)]
        public int pushCase;
        [Range(0, 7)]
        public int range;

        [Space, Header("Action type")]
        public bool isTileEffect = false;
        [ShowIf("isTileEffect"), PropertySpace(0, 20.0f)]
        public TileEffect tileEffect;
        public bool hasSightView, isLinedRange;

        public bool isTargettingTile = false;

        public bool canBePlayedOnself = false;
        
        [Space]
        public bool isAreaAction;
        [ShowIfGroup("isAreaAction")]
        public int areaRange;
        [ShowIfGroup("isAreaAction")]public bool hasLineArea = true;
        [HideIfGroup("hasLineArea")] public List<Vector2> customArea;
        [Space]           
       

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
        public bool Process(Tile casterTile, Tile targetTile, Action action)
        {
            Player playerTargeted;
            Player playerCaster;
            bool res = true;
            playerTargeted = targetTile.Player;
            playerCaster = casterTile.Player;
            Tile targetedPlayerTile, casterPlayerTile;

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
                            if (playerTargeted == null)
                            {
                                return false;                                
                            }
                            if (GridManager.Grid.TryGetTile(playerTargeted.position, out targetedPlayerTile))
                            {
                                res = eff.Process(casterTile, targetedPlayerTile, action); // le player target a peut être été déplacé donc on essaye
                            }
                            if(GridManager.Grid.TryGetTile(playerCaster.position, out casterPlayerTile))
                            {
                                res = eff.Process(casterPlayerTile, targetTile, action); // maybe it's the caster which moved, we try (charge, TP)
                            }
                        }
                    }
                }
            }
            return res;
        }
    }

}

