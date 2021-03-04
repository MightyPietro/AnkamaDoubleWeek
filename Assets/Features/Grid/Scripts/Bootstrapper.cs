﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeekAnkama
{
    public class Bootstrapper : MonoBehaviour
    {
        public int x = 10;
        public int y = 10;
        public Vector2 size;
        public GameObject test;
        public Text text;
        Tile casterTile;
        Tile currentTile;
        public Vector3 pos;
        public FireTileEffect effect;
        public FireTileEffect effect2;

        // Start is called before the first frame update
        void Awake()
        {
            MouseHandler.OnMouseMove += (Vector2 position) => {
                RaycastHit hitData;
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(position.x, position.y)); 
                if(Physics.Raycast(ray,out hitData, 100))
                {
                    pos = hitData.point;                    
                    Debug.Log("Move !!!" + pos);
                }
                else
                {
                    pos = new Vector3(-100,-100,-100);
                }
            };

            MouseHandler.OnMouseLeftClick += () =>
            {
                Tile oldTile = currentTile;
                if (GridManager.Grid.TryGetTile(pos, out currentTile))
                {
                    text.transform.position = Camera.main.WorldToScreenPoint(GridManager.Grid.GetTileWorldPosition(currentTile.Coords.x, currentTile.Coords.y));
                    test.transform.position = GridManager.Grid.GetTileWorldPosition(currentTile.Coords.x, currentTile.Coords.y);

                    MouseHandler.OnTileClick(currentTile);
                }
                else
                {
                    currentTile = oldTile;
                }
            };
        }

        private void Start()
        {
        }

        private void Update()
        {
            //MouseHandler.Instance.DisableGameplayInputs();
            GridManager.Grid.DebugGrid();

            /*_grid.TryGetTile(Vector3.zero, out Tile t);
            t.SetTileEffect(effect2);
            Debug.Log("-------------" + t.Effect.ToString()) ;

            _grid.TryGetTile(new Vector3(3, 0, 1), out Tile t2);
            t2.SetTileEffect(effect);
            Debug.Log("-------------" + t2.Effect.ToString());*/

            if (currentTile == null) return;
            //currentTile.value++;
            text.text = $"{currentTile.Coords.x} - {currentTile.Coords.y} ____ ";    
        }
    }
}

