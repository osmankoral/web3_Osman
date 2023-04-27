using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace Client.GamePlay
{
    public class CombatManagerClient : MonoBehaviour
    {
        public UnityEvent Enemy_Action = new();
        public UnityEvent Who_Turn = new();
        public delegate void Player_Action(int whichSkill);
        public delegate void ButtonActive(bool isActive, int whichButton);
        public delegate void BattleEnd(bool isActive);
        public delegate void SkillSet(int count, Sprite[] img);
        public  Player_Action player_Action;
        public  ButtonActive buttonActive;
        public  BattleEnd battleEnd;
        public SkillSet skillSet;

        GameManager _gameManager;

        [SerializeField] private Camera cam;
        [SerializeField] private GameObject[] _heroPrefabs;
        [SerializeField] private GameObject[] _enemyPrefabs;

        [SerializeField] private Transform[] _heroTransforms;
        [SerializeField] private Transform[] _enemyTransforms;

        [SerializeField] List<GameObject> _attackQueue = new List<GameObject>();
        [SerializeField] List<GameObject> _frontLine = new List<GameObject>();
        [SerializeField] List<GameObject> _backLine = new List<GameObject>();

        [SerializeField] LayerMask _layer;

        private List<HeroPuppet> _heroesScripts = new List<HeroPuppet>();
        private List<EnemyPuppet> _enemyScripts = new List<EnemyPuppet>();

        private int _heroCount;
        private int _enemyCount;
        private int _queueCount;
        private int _turnCount;
        private EnemyPuppet _enemy;
        private int _hero;

        private int _selectedSkill;

        CombatState _combatState;

        enum CombatState
        {
            PlayerTurn = 0,
            PlayerAction,
            EnemyTurn,
            Busy
        };

        private void OnEnable()
        {
            //Player_Action.AddListener(() => PlayerAction());
            Enemy_Action.AddListener(() => EnemyAction());
            Who_Turn.AddListener(() => WhoTurn());
            player_Action += PlayerAction;
        }

        private void OnDisable()
        {
           // Player_Action.RemoveListener(() => PlayerAction());
            Enemy_Action.RemoveListener(() => EnemyAction());
            Who_Turn.RemoveListener(() => WhoTurn());
        }
        private void Awake()
        {

        }

        private void Start()
        {
            _queueCount = 0;
        }

        public void ReadyGameStart()
        {
            CombatStateStarter();
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _combatState == CombatState.PlayerAction)
            {
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, _layer))
                {
                    if (hit.transform.parent.TryGetComponent<EnemyPuppet>(out EnemyPuppet enemy))
                    {
                        _enemy = enemy;
                        HeroAction(_selectedSkill, enemy);
                    }
                }
            }
        }

        public void HeroDead(HeroPuppet hero)
        {
            if (_heroesScripts.IndexOf(hero) < _queueCount)
                _queueCount--;
            _heroesScripts.Remove(hero);
            _heroCount--;
        }

        public void EnemyDead(EnemyPuppet enemy)
        {
            foreach(var en in _frontLine)
            {
                en.TryGetComponent<EnemyPuppet>(out EnemyPuppet script);
                if(script == enemy)
                { 
                    _frontLine.Remove(en);
                    break; 
                }
            }

            foreach (var en in _backLine)
            {
                if (en == enemy)
                {
                    _backLine.Remove(en);
                    break;
                }
            }
            _enemyScripts.Remove(enemy);
            _enemyCount--;
        }

        private void PlayerAction(int whichSkill)
        {
            if (_combatState != CombatState.PlayerTurn)
                return;
            _combatState = CombatState.PlayerAction;
            buttonActive(false, whichSkill);
            PlayerSelectSkill(whichSkill);
        }

        private void PlayerSelectSkill(int whichSkill)
        {
            _selectedSkill = whichSkill;
            EnemySelectionMark(true, 6);
        }

        private void CombatStateStarter()
        {
            if(_gameManager == null)
            {
               _gameManager = FindObjectOfType<GameManager>();
                CombatStateStarter();
            }

            else
            {
                _heroCount = _gameManager.GetHeroCount();
                _enemyCount = _gameManager.GetEnemyCount();
                HeroSpawn();
                EnemySpawn();
                SetAttackQueue();
                WhoTurn();
            }
            
        }

        private void HeroSpawn()
        {
            for (int i = 0; i < _gameManager._selectedHeroes.Length; i++)
            {
                if (_gameManager._selectedHeroes[i] != 0)
                {

                    GameObject hero = Instantiate(
                    _heroPrefabs[_gameManager._selectedHeroes[i] - 1],
                    _heroTransforms[i].position,
                    _heroTransforms[i].rotation
                    );


                    
                    
                    _heroesScripts.Add(hero.GetComponent<HeroPuppet>());
                }
            }
        }

        private void EnemySpawn()
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                GameObject enemy = Instantiate(
                    _enemyPrefabs[_gameManager._selectedEnemy[i]],
                    _enemyTransforms[i].position,
                    _enemyTransforms[i].rotation
                    );

                    if(i<2)
                        _frontLine.Add(enemy);
                    else
                        _backLine.Add(enemy);

                _enemyScripts.Add(enemy.GetComponent<EnemyPuppet>());
            }
        }

        private void WhoTurn()
        {
            if (_heroCount <= 0)
            {
                battleEnd(false); return;
            }

            if (_enemyCount <= 0)
            {
                battleEnd(true); return;
            }

            if (_queueCount >= _attackQueue.Count)
            {
                _queueCount = 0;
                _turnCount++;
                SetAttackQueue();
            }

            GameObject currentObject = _attackQueue[_queueCount];
            if (currentObject.TryGetComponent<EnemyPuppet>(out EnemyPuppet enemy))
            {
                if (enemy.can > 0)
                {
                    _combatState = CombatState.EnemyTurn;
                    Enemy_Action.Invoke();
                }
                else
                {
                    _queueCount++;
                    WhoTurn();
                }
            }

            else if (currentObject.TryGetComponent<HeroPuppet>(out HeroPuppet hero))
            {
                if (hero._heroSO.Health > 0)
                {
                    hero.HeroSetSkill(out int count, out Sprite[] img);
                    skillSet(count, img);
                    hero.Mark.SetActive(true);
                    _combatState = CombatState.PlayerTurn;
                }
                else
                {
                    _queueCount++;
                    WhoTurn();
                }
            }
        }

        private void EnemySelectionMark(bool isActive, int layer)
        {
            if(_frontLine.Count > 0)
            {
                foreach(var enemy in _frontLine)
                {
                    enemy.TryGetComponent<EnemyPuppet>(out EnemyPuppet scripts);
                    scripts.MarkChgange(isActive, layer);
                }
            }
            else
            {
                foreach (var enemy in _backLine)
                {
                    enemy.TryGetComponent<EnemyPuppet>(out EnemyPuppet scripts);
                    scripts.MarkChgange(isActive, layer);
                }
            }
        }

        private void HeroAction(int whichSkill, EnemyPuppet enemy)
        {

            if (_combatState != CombatState.PlayerAction)
                return;

            EnemySelectionMark(false, 0);
            _attackQueue[_queueCount].TryGetComponent<HeroPuppet>(out HeroPuppet hero);

            _queueCount++;
            _combatState = CombatState.Busy;

            hero.HeroAction(enemy, whichSkill);
        }


        private void EnemyAction()
        {
            if (_combatState != CombatState.EnemyTurn)
                return;

            _hero = UnityEngine.Random.Range(0, _heroCount);

            _attackQueue[_queueCount].TryGetComponent<EnemyPuppet>(out EnemyPuppet enemy);
            enemy.EnemyAction(_heroesScripts[_hero]);
            _combatState = CombatState.Busy;
            _queueCount++;

        }


        private void SetAttackQueue()
        {
            _attackQueue.Clear();

            int index = 0;
            int index2 = 0;
            int listCount = _heroCount + _enemyCount;

            for(int i= 0; i < listCount; i++)
            {
                index = (i < _heroCount) ? i : i - _heroCount; 
                index2 = _attackQueue.Count;

                for(int j = 0; j < index2 + 1; j++)
                {
                     if (j == index2)
                     {
                        if(i< _heroCount)
                        {
                             _attackQueue.Add(_heroesScripts[index].gameObject);
                            break;
                        }

                        else
                        {
                            _attackQueue.Add(_enemyScripts[index].gameObject);
                            break;
                        }
                     }

                    if (_attackQueue[j].TryGetComponent<HeroPuppet>(out HeroPuppet hero))
                    {
                        if (i < _heroCount)
                        {
                            if (hero.CharSpeed < _heroesScripts[index].CharSpeed)
                            {
                                _attackQueue.Insert(j, _heroesScripts[index].gameObject);
                                break;
                            }
                        }

                        else
                        {
                            if (hero.CharSpeed < _enemyScripts[index].CharSpeed)
                            {
                                _attackQueue.Insert(j, _enemyScripts[index].gameObject);
                                break;
                            }
                        }
                    }

                    else if(_attackQueue[j].TryGetComponent<EnemyPuppet>(out EnemyPuppet enemy))
                    {
                        if (i < _heroCount)
                        {

                            if (enemy.CharSpeed < _heroesScripts[index].CharSpeed)
                            {
                                _attackQueue.Insert(j, _heroesScripts[index].gameObject);
                                break;
                            }
                        }

                        else
                        {
                            if (enemy.CharSpeed < _enemyScripts[index].CharSpeed)
                            {
                                _attackQueue.Insert(j, _enemyScripts[index].gameObject);
                                break;
                            }
                        }

                    }
                }
            }
        }


    }
}
