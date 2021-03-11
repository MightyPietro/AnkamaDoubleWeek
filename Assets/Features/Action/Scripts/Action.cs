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
        private TileEffect _actionTileEffect;

        [Header("Common Values")]
        public Sprite icon;
        [TextArea(2, 5)]
        public string description;
        [Range(0, 10)]
        public int paCost;
        public int bonusPA;
        [Range(0, 200)]
        public int fatigueDmg;
        [Range(0,10)]
        public int pushCase;
        [Range(0, 15)]
        public int range;
        [Range(0, 15)]
        public int minimalRange;

        [Space, Header("Action type")]
        public bool canTerraform = false;
        [ShowIf("canTerraform"), PropertySpace(0, 20.0f)]
        [SerializeField]private List<TileEffect> tileEffects;
        public bool hasSightView, isLinedRange;

        public bool isTargettingTile = false;

        public bool canBePlayedOnself = false;
        
        [Space]
        public bool isAreaAction;
        [ShowIfGroup("isAreaAction")]
        public int areaRange;
        [ShowIfGroup("isAreaAction")] public bool hasLineArea = true;
        [ShowIfGroup("isAreaAction")] public List<Vector2> customArea;
        [Space]           
       

        [Header("Action")]
        public List<ActionType> actionTypes;
        public System.Type[] actionEffects;



        public TileEffect ActionTileEffect => _actionTileEffect;


        [ContextMenu("FindActionEffectSubClass")]
        void FindActionEffectSubClass()
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            actionEffects = (from System.Type type in types where type.IsSubclassOf(typeof(ActionEffect)) select type).ToArray();

            //Sort
            actionTypes.Sort((eff1, eff2) => { return eff1.CompareTo(eff2); });
        }

        public void AddActionElementalEffect(ActionType elementalEffect)
        {
            if (!actionTypes.Contains(elementalEffect))
            {
                actionTypes.Add(elementalEffect);
                switch (elementalEffect)
                {
                    case ActionType.Water:
                        _actionTileEffect = GetElementEffect(typeof(WaterTileEffect));
                        break;
                    case ActionType.Fire:
                        _actionTileEffect = GetElementEffect(typeof(FireTileEffect));
                        break;
                    case ActionType.Earth:
                        _actionTileEffect = GetElementEffect(typeof(EarthTileEffect));
                        break;
                    case ActionType.Air:
                        _actionTileEffect = GetElementEffect(typeof(AirTileEffect));
                        break;                    
                }
            }
        }

        private TileEffect GetElementEffect(System.Type tileEffect)
        {
            for (int i = 0; i < tileEffects.Count; i++)
            {
                if( tileEffects[i].GetType() == tileEffect)
                {
                    return tileEffects[i];
                }
            }
            return null;
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

                        res = TryProcessingEffect(casterTile, targetTile, action, playerTargeted, playerCaster, eff);
                    }
                }
            }

            CleanElements();
            return res;            
        }

        private void CleanElements()
        {
            for (int i = 0; i < actionTypes.Count; i++)
            {
                if(actionTypes[i] == ActionType.Water ||
                    actionTypes[i] == ActionType.Fire ||
                    actionTypes[i] == ActionType.Earth ||
                    actionTypes[i] == ActionType.Air)
                {
                    actionTypes.RemoveAt(i);
                    i--;
                }
            }
        }

        private bool TryProcessingEffect(Tile casterTile, Tile targetTile, Action action, Player playerCaster, Player playerTargeted,  ActionEffect eff)
        {
            bool res = true;

            if (!eff.Process(casterTile, targetTile, action)) // on essaye l effet sur la case ciblé
            {
                if (playerTargeted == null)
                {
                    res = false;
                }
                else if (GridManager.Grid.TryGetTile(playerTargeted.position, out Tile targetedPlayerTile))
                {
                    res = eff.Process(casterTile, targetedPlayerTile, action); // le player target a peut être été déplacé donc on essaye
                }
                else {  
                    if (playerCaster == null)
                    {
                        res = false;
                    }
                    else if (GridManager.Grid.TryGetTile(playerCaster.position, out Tile casterPlayerTile))
                    {
                        res = eff.Process(casterPlayerTile, targetTile, action); // maybe it's the caster which moved, we try (charge, TP)
                    }
                }               
            }

            return res;
        }
    }

}

