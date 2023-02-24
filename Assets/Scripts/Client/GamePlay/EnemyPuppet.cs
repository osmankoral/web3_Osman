using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Client.Heroes;

namespace Client.GamePlay
{
    public class EnemyPuppet : MonoBehaviour
    {
        [SerializeField] CombatManagerClient _combatManagerClient;
        [SerializeField] private Slider _bar;
        enum EnemyState
        {
            Idle = 0,
            Attack,
            Move
        }

        private EnemyState _state;
        private Vector3 _targetPosition;
        private Vector3 _position;
        private bool _isAttack;
        private HeroPuppet _hero;

        public int can;

        private void Awake()
        {
            _combatManagerClient= FindObjectOfType<CombatManagerClient>();
        }

        private void Start()
        {
            _state = EnemyState.Idle;
            _isAttack = false;
            can = 5;
            _bar.value = can;
        }
        private void FixedUpdate()
        {
            switch (_state)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.Attack:
                    break;
                case EnemyState.Move:
                    EnemyMove();
                    break;
            }
        }

        public void Damage()
        {
            can -= 1;

            if(can < 1)
            {
                gameObject.SetActive(false);
                _combatManagerClient.EnemyDead(this);
                return;
            }
            _bar.value = can;
        }

        public void EnemyAction(HeroPuppet hero)
        {
            if (_state != EnemyState.Idle)
                return;
            _hero = hero;
            _targetPosition = hero.transform.position;
            _targetPosition.x += 0.5f;
            _position = transform.position;
            _state = EnemyState.Move;
        }

        private void EnemyMove()
        {
            float distance = Vector3.Distance(transform.position, _targetPosition);
            if (distance >= 0.1f)
            {
                Vector3 temp = _targetPosition - transform.position;
                transform.position += Vector3.Normalize(temp) * 3f * Time.deltaTime;
            }

            else
            {
                if (!_isAttack)
                {
                    _state = EnemyState.Attack;
                    _targetPosition = _position;
                    EnemyAttack();
                }
                else
                {
                    _isAttack = false;
                    _state = EnemyState.Idle;
                    CombatManagerClient.Who_Turn.Invoke();
                }
            }

        }

        private void EnemyAttack()
        {
            _hero.Damage();
            _isAttack = true;
            _state = EnemyState.Move;
        }
    }
}