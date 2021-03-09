using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WeekAnkama
{
    public class Player : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private int _PA;
        [SerializeField] private int _basePA, _stockPA;
        [SerializeField] private int _PM;
        [SerializeField] private int _basePM;
        [SerializeField] private int _fatigue;
        [SerializeField] private Vector2Int _position;
        [SerializeField] private List<Action> _deck;
        [SerializeField] private List<Action> _hand;
        [SerializeField] private Action _currentAction;
        [SerializeField] private TextMeshProUGUI _fatigueText;
        [SerializeField] private TextMeshProUGUI _PAText;
        [SerializeField] private TextMeshProUGUI _PMText;
        [SerializeField] private Feedback _playerFatigueDmg;
        [SerializeField] private Animator _anim;

        private bool _processMovement = false;
        private bool _isOut = false;
        private Vector2Int _direction;

        [HideInInspector]
        public List<Action> _deckReminder;
        #endregion


        #region Getter/Setter
        public int PA { get { return _PA; } set { _PA = value; } }// PAText.text = PA.ToString() + "/"; } }
        public int stockPA { get { return _stockPA; } set { _stockPA = value;} }
        public int PM { get { return _PM; } set { _PM = value; } }// _PMText.text = PM.ToString(); } }
        public int fatigue { get { return _fatigue; } 
        set { 
                _fatigue = value; 
                fatigueText.text = fatigue.ToString(); 
                FeedbackManager.instance.Feedback(_playerFatigueDmg,transform.position + new Vector3(0, transform.localScale.y+1,0) ,1f);
                Hurt();
            } }
        public Vector2Int position { get { return _position; } set { _position = value; } }
        public Vector2Int Direction { get { return _direction; } set { _direction = value; } }
        public List<Action> deck { get { return _deck; } set { _deck = value; } }
        public List<Action> hand { get { return _hand; } set { _hand = value; } }
        public Action currentAction { get { return _currentAction; } set { _currentAction = value; } }
        public TextMeshProUGUI fatigueText { get { return _fatigueText; } set { _fatigueText = value; } }
        public TextMeshProUGUI PAText { get { return _PAText; } set { _PAText = value; } }
        public TextMeshProUGUI PMText { get { return _PMText; } set { _PMText = value; } }
        public bool processMovement { get { return _processMovement; } set { _processMovement = value; } }
        public bool isOut { get { return _isOut; } set { _isOut = value; } }
        public Animator anim { get { return _anim; } set { _anim = value; } }
        #endregion

        private void Awake()
        {
            ResetDatas();
            _fatigue = 0;
            fatigueText.text = fatigue.ToString();
            DeplacementManager.OnPlayerMovementFinished += StopRun;
        }

        private void OnDisable()
        {
            DeplacementManager.OnPlayerMovementFinished -= StopRun;
        }
        private void StopRun(Player player)
        {
            if(this == player)
            {
                player.anim.SetBool("isRun", false);
                player.anim.SetBool("isIDLE", true);
            }

        }

        public void Punch()
        {
            int rand = Random.Range(0, 3);
            anim.SetTrigger("Attack" + rand.ToString());
        }

        private void Hurt()
        {
            int rand = Random.Range(0,2);
            anim.SetTrigger("Hurt" + rand.ToString());
        }
        public void TakeDamage(int amount)
        {

            Debug.Log("Allo ?");
            fatigue += amount;

            Debug.Log(fatigue);
        }

        public void ResetDatas()
        {
            
            PA = _basePA;
            PM = _basePM;
        }

        public void ResetFatigue()
        {
            fatigue = 0;
        }

    }
}

