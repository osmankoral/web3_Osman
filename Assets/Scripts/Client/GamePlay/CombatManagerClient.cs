using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace Client.GamePlay
{
    public class CombatManagerClient : MonoBehaviour
    {
        public static UnityEvent Player_Action = new();
        public static UnityEvent Enemy_Action = new();
        public static UnityEvent Who_Turn = new();
        public delegate void ButtonActive(bool isActive, Vector3 position);
        public delegate void BattleEnd(bool isActive);
        public ButtonActive buttonActive;
        public BattleEnd battleEnd;

        [SerializeField] private Camera cam;
        [SerializeField] private GameObject[] _heroPrefabs;
        [SerializeField] private GameObject[] _enemyPrefabs;

        [SerializeField] private Transform[] _heroTransforms; 
        [SerializeField] private Transform[] _enemyTransforms;

        [SerializeField] List<GameObject> _attackQueueGameObject = new List<GameObject>();
        [SerializeField] List<int> _attackQueueIndex = new List<int>();

        [SerializeField] LayerMask _layer;

        private List<HeroPuppet> _heroesScripts = new List<HeroPuppet>();
        private List<EnemyPuppet> _enemyScripts = new List<EnemyPuppet>();

        private int _heroCount;
        private int _enemyCount;
        private int _queueCount;
        private int _turnCount;
        private EnemyPuppet _enemy;
        private int _hero;

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
            Player_Action.AddListener(() => PlayerAction());
            Enemy_Action.AddListener(() => EnemyAction());
            Who_Turn.AddListener(() => WhoTurn());

        }

        private void OnDisable()
        {
            Player_Action.RemoveListener(() => PlayerAction());
            Enemy_Action.RemoveListener(() => EnemyAction());
            Who_Turn.RemoveListener(() => WhoTurn());
        }
        private void Awake()
        {
            
        }

        private void Start()
        {
            _queueCount = 0;
            CombatStateStarter();
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0) && _combatState == CombatState.PlayerAction)
            {
                if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, _layer))
                {
                    if (hit.transform.parent.TryGetComponent<EnemyPuppet>(out EnemyPuppet enemy))
                    {
                        _enemy = enemy;
                        HeroAction();
                    }
                }
            }
        }

        public void HeroDead(HeroPuppet hero)
        {
            _heroesScripts.Remove(hero);
            _heroCount--;
        }

        public void EnemyDead(EnemyPuppet enemy)
        {
            _enemyScripts.Remove(enemy);
            _enemyCount--;
        }

        private void PlayerAction()
        {
            if (_combatState != CombatState.PlayerTurn)
                return;
            _combatState = CombatState.PlayerAction;
            buttonActive(false, Vector3.zero);
        }

        private void CombatStateStarter()
        {
            _heroCount = GameManager.Instance.GetHeroCount();
            _enemyCount = GameManager.Instance.GetEnemyCount();
            HeroSpawn();
            EnemySpawn();
            SetAttackQueue();
            WhoTurn();
        }

        private void HeroSpawn()
        {
            for(int i = 0; i < GameManager.Instance._selectedHeroes.Length; i++)
            {
                if (GameManager.Instance._selectedHeroes[i] != 0)
                {

                    GameObject hero = Instantiate(
                    _heroPrefabs[ GameManager.Instance._selectedHeroes[i] - 1],
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
                    _enemyPrefabs[GameManager.Instance._selectedEnemy[i]],
                    _enemyTransforms[i].position,
                    _enemyTransforms[i].rotation
                    );

                _enemyScripts.Add(enemy.GetComponent<EnemyPuppet>());
            }
        }

        private void WhoTurn()
        {
            if(_heroCount <= 0)
            {
                battleEnd(false); return;
            }

            if (_enemyCount <= 0)
            {
                battleEnd(true); return;
            }

            if (_queueCount >= _attackQueueGameObject.Count)
            {
                _queueCount = 0;
                _turnCount++;
                SetAttackQueue();
            }

            GameObject currentObject = _attackQueueGameObject[_queueCount];
            if(currentObject.TryGetComponent<EnemyPuppet>(out EnemyPuppet enemy))
            {
                if(enemy.can > 0)
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
                if(hero.can > 0)
                {
                    _combatState = CombatState.PlayerTurn;
                    Vector3 tempPos = cam.WorldToScreenPoint(_attackQueueGameObject[_queueCount].transform.position);
                    tempPos.y += 1.5f;
                    buttonActive(true, tempPos);
                }
                else
                {
                    _queueCount++;
                    WhoTurn();
                }
            }
        }


        private void HeroAction()
        {
            if (_combatState != CombatState.PlayerAction)
                return;
;
            _attackQueueGameObject[_queueCount].TryGetComponent<HeroPuppet>(out HeroPuppet hero);
            hero.HeroAction(_enemy);

            //_heroesScripts[_attackQueueIndex[_queueCount]].HeroAction(_enemyScripts[_enemy]);
            _combatState = CombatState.Busy;
            _queueCount++;
        }


        private void EnemyAction()
        {
            if (_combatState != CombatState.EnemyTurn)
                return;
         
            _hero = UnityEngine.Random.Range(0, _heroCount);
            
            _attackQueueGameObject[_queueCount].TryGetComponent<EnemyPuppet>(out EnemyPuppet enemy);
            enemy.EnemyAction(_heroesScripts[_hero]);
            _combatState = CombatState.Busy;
            _queueCount++;

        }


        private void SetAttackQueue()
        {
            _attackQueueGameObject.Clear();
            _attackQueueIndex.Clear();

            int herocount = 0; int enemyCount = 0; 
            for(int i = 0; i < _heroCount + _enemyCount; i++)
            {
                if(i % 2 == 0)
                {
                    if(herocount < _heroCount) { 
                        _attackQueueGameObject.Add(_heroesScripts[herocount].gameObject);
                        _attackQueueIndex.Add(herocount);
                        herocount++;
                    }
                    else
                    {
                        _attackQueueGameObject.Add(_enemyScripts[enemyCount].gameObject);
                        _attackQueueIndex.Add(enemyCount);
                        enemyCount++;
                    }
                }

                else
                {
                    if(enemyCount < _enemyCount)
                    {
                        _attackQueueGameObject.Add(_enemyScripts[enemyCount].gameObject);
                        _attackQueueIndex.Add(enemyCount);
                        enemyCount++;
                    }
                    else
                    {
                        _attackQueueGameObject.Add(_heroesScripts[herocount].gameObject);
                        _attackQueueIndex.Add(herocount);
                        herocount++;
                    }
                }
            }
        }



    }
}
