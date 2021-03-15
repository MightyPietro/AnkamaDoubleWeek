using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using DG.Tweening;

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
        [SerializeField] private List<Action> _deck = new List<Action>();
        [SerializeField] private List<Action> _hand = new List<Action>();
        [SerializeField] private Action _currentAction;
        [SerializeField] private TextMeshProUGUI _fatigueText;
        [SerializeField] private TextMeshProUGUI _PAText;
        [SerializeField] private TextMeshProUGUI _PMText;
        [SerializeField] private Image[] _PAImages;
        [SerializeField] private Image[] _PMImages;
        [SerializeField] private Image _icone;
        [SerializeField] private TextMeshProUGUI _damages;
        [SerializeField] private TextMeshProUGUI _playerFatigueDmgTxt;
        [SerializeField] private Feedback _playerFatigueDmg;
        [SerializeField] private Feedback _playerFatigueDmgUI;
        [SerializeField] private Animator _anim;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private PlayerClassScriptable[] _classes;
        [SerializeField] private Material _outlineMat;
        [SerializeField] private Color _outlineColor;



        private bool _processMovement = false;
        private bool _isOut = false;
        private Vector2Int _direction;
        private float vulnerability = 1;

        [SerializeField]
        private Sprite icone;
        private TextMeshProUGUI mainFatigueTxt, pmTxt, paTxt, stockPaTxt;

        [SerializeField]
        private GameObject turnFeedback;
        [SerializeField]
        private ArrowTurnFeedBack turnArrowFeedback;

        [HideInInspector]
        public List<Action> discardPile = new List<Action>();

        [SerializeField]
        private List<PlayerEffect> effects = new List<PlayerEffect>();

        [SerializeField]
        public PlayerClassScriptable classe;
        [SerializeField]
        private IntVariable playerValue;

        private PlayerPassive passive;

        private System.Action<Player, Player> beginTurn, takeDamage, doDamage, passEnemyExhaust, passSelfExhaust;
        private System.Action<Player> passEnemyExhaustSolo;
        #endregion


        #region Getter/Setter
        public int PA
        {
            get
            {
                return _PA;
            }
            set
            {
                _PA = value;
                if (paTxt != null)
                {
                    paTxt.text = PA.ToString();
                    for (int i = 0; i < _PAImages.Length; i++)
                    {
                        _PAImages[i].enabled = false;
                        if (PA > i)
                        {
                            _PAImages[i].enabled = true;
                        }
                    }
                }
            }
        }
        public int stockPA { get { return _stockPA; } set { _stockPA = value; if (stockPaTxt != null) { stockPaTxt.text = stockPA.ToString(); } } }
        public int PM
        {
            get
            {
                return _PM;
            }
            set
            {
                _PM = value;
                if (pmTxt != null)
                {
                    pmTxt.text = PM.ToString();
                }
                for (int i = 0; i < _PMImages.Length; i++)
                {
                    _PMImages[i].enabled = false;
                    if (PM > i)
                    {
                        _PMImages[i].enabled = true;
                    }
                }
            }
        }
        public int fatigue
        {
            get { return _fatigue; }
            set
            {
                _fatigue = value;
                fatigueText.text = fatigue.ToString();

                if (mainFatigueTxt != null)
                {
                    mainFatigueTxt.text = fatigue.ToString();
                }
            }
        }
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

        public int uniquePlayerValue;
        #endregion


        private IEnumerator Start()
        {
            ResetDatas();
            ResetFatigue();


            switch (classe.passive)
            {
                case PlayerClasses.Earth:
                    passive = new EarthClassPassive();
                    break;
                case PlayerClasses.Fire:
                    passive = new FireClassPassive();
                    break;
                case PlayerClasses.Water:
                    passive = new WaterClassPassive();
                    break;
                case PlayerClasses.Wind:
                    passive = new WindClassPassive();
                    break;
            }

            if (passive != null)
            {
                switch (passive.Trigger)
                {
                    case PassiveTrigger.BeginTurn:
                        beginTurn += passive.ApplyPassive;
                        break;
                    case PassiveTrigger.DoDamage:
                        doDamage += passive.ApplyPassive;
                        break;
                    case PassiveTrigger.TakeDamage:
                        takeDamage += passive.ApplyPassive;
                        break;
                    case PassiveTrigger.PassEnemyExhaust:
                        passEnemyExhaust += passive.ApplyPassive;
                        break;
                    case PassiveTrigger.PassSelfExhaust:
                        passSelfExhaust += passive.ApplyPassive;
                        break;
                }
            }

            _deck = new List<Action>(classe.deck);

            DeplacementManager.OnPlayerMovementFinished += StopRun;
            passEnemyExhaustSolo += PlayerManager.instance.DrawCard;

            yield return new WaitForSeconds(.5f);
            if (PhotonNetwork.IsConnected)
            {
                for (int i = 0; i < TurnManager.instance.players.Count; i++)
                {
                    if (this == TurnManager.instance.players[i])
                    {
                        for (int j = 0; j < _classes.Length; j++)
                        {
                            if (this.classe == _classes[j]) ;
                            {
                                _photonView.RPC("SetPlayerClassViaRPC", RpcTarget.All, i, j);

                                break;
                            }
                        }

                    }
                }
            }

        }


        private void OnDisable()
        {
            DeplacementManager.OnPlayerMovementFinished -= StopRun;
        }

        [PunRPC]
        public void SetPlayerClassViaRPC(int playerValue, int playerClasse)
        {

            TurnManager.instance.players[playerValue].classe = _classes[playerClasse];
        }
        private void StopRun(Player player)
        {
            if (this == player)
            {
                player.anim.SetBool("isRun", false);
            }

        }

        public void Punch()
        {
            int rand = Random.Range(0, 3);
            anim.SetTrigger("Attack" + rand.ToString());
            StartCoroutine(WaitResetPunch(rand));
        }

        private void Hurt(out int rand)
        {
            rand = Random.Range(0, 2);
            anim.SetTrigger("Hurt" + rand.ToString());

        }

        private IEnumerator WaitResetAnim(int rand)
        {
            yield return new WaitForSeconds(.1f);
            anim.ResetTrigger("Hurt" + rand.ToString());
            yield return null;
        }

        private IEnumerator WaitResetPunch(int rand)
        {
            yield return new WaitForSeconds(.1f);
            anim.ResetTrigger("Attack" + rand.ToString());
            yield return null;
        }


        public void BeginTurn()
        {
            ResetDatas();
            beginTurn?.Invoke(this, this);

            turnFeedback.SetActive(true);
            turnArrowFeedback.SetActive(true);

            foreach (PlayerEffect eff in effects)
            {
                switch(eff.effectType)
                {
                    case PlayerEffectTypes.PA:
                        PA += (int)eff.value;
                        break;
                    case PlayerEffectTypes.PM:
                        PM += (int)eff.value;
                        break;
                    case PlayerEffectTypes.Vulnerability:
                        vulnerability += eff.value;
                        break;
                }
            }
            effects.Clear();
        }

        public void EndTurn()
        {
            turnFeedback.SetActive(false);
            turnArrowFeedback.SetActive(false);
        }

        public void DoDamage(Player attackTarget, int amount)
        {
            if(amount>0)
            {
                doDamage?.Invoke(this, attackTarget);
                Punch();
            }
        }

        public void TakeHeal(int amount)
        {
            fatigue -= amount;
            if (fatigue<0)
            {
                fatigue = 0;
            }
        }

        public int TakeDamage(Player attacker, int amount)
        {
            int lastFatigue = fatigue;

            fatigue += Mathf.RoundToInt((float)amount * vulnerability);
            StopRun(this);
            FeedbackManager.instance.Feedback(_playerFatigueDmg, transform.position, 1f);
            Hurt(out int rand);
            StartCoroutine(WaitResetAnim(rand));
            if (amount>0)
            {

                _playerFatigueDmgTxt.GetComponent<Animator>().SetTrigger("Damage");
                _playerFatigueDmgTxt.text = amount.ToString();
                FeedbackManager.instance.Feedback(_playerFatigueDmgUI, fatigueText.transform.position, 2f, fatigueText.transform);
                Sequence _iconeSequence = DOTween.Sequence();
                Sequence _damagesSequence = DOTween.Sequence();
                _iconeSequence.Append(_icone.rectTransform.DOScale(_icone.rectTransform.localScale * 1.2f, .5f)).OnComplete(()=>_icone.rectTransform.DOScale(_icone.rectTransform.localScale / 1.2f, .5f));
                _damagesSequence.Append(_damages.rectTransform.DOScale(_damages.rectTransform.localScale * 1.5f, .5f)).OnComplete(()=> _damages.rectTransform.DOScale(_damages.rectTransform.localScale / 1.5f, .5f));
                
                takeDamage?.Invoke(attacker, this);                
            }

            if (Mathf.FloorToInt(_fatigue / 100) > Mathf.FloorToInt(lastFatigue / 100))
            {
                passSelfExhaust?.Invoke(this, this);

                if(attacker != null)
                {
                    attacker.PassEnnemyExhaust(this);
                }
            }

            return fatigue;
        }

        public void PassEnnemyExhaust(Player targetPlayer)
        {
            passEnemyExhaust?.Invoke(this, targetPlayer);
            passEnemyExhaustSolo?.Invoke(this);
        }

        public void ResetDatas()
        {
            PA = _basePA;
            PM = _basePM;
            vulnerability = 1;
            fatigue = fatigue;
            stockPA = stockPA;
        }

        public void ResetFatigue()
        {
            fatigue = 0;
        }

        public void UnsetPlayerUI()
        {
            mainFatigueTxt = null;
            pmTxt = null;
            paTxt = null;
            stockPaTxt = null;
        }

        public void SetPlayerUI(Image _icone, Image _spellIcon, TextMeshProUGUI _mainFatigueTxt, TextMeshProUGUI _pmTxt, TextMeshProUGUI _paTxt, TextMeshProUGUI _stockPaTxt)
        {
            _icone.sprite = icone;
            _spellIcon.sprite = classe.icon;
            mainFatigueTxt = _mainFatigueTxt;
            pmTxt = _pmTxt;
            paTxt = _paTxt;
            stockPaTxt = _stockPaTxt;
        }

        public void AddEffect(PlayerEffect effect)
        {
            effects.Add(effect);
        }

        public void ChangeOutline(bool isPlayerTurn)
        {
            if (isPlayerTurn)
            {
                _outlineMat.SetColor("_Color", _outlineColor);
                _outlineMat.SetFloat("_Size", 1.2f);
            }
            else
            {
                _outlineMat.SetColor("_Color", Color.black);
                _outlineMat.SetFloat("_Size", .5f);

            }
            
        }
    }
}

