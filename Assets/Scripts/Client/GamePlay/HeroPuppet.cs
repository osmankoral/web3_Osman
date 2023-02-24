using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Client.Heroes;

namespace Client.GamePlay
{
    public class HeroPuppet : MonoBehaviour
    {
        [SerializeField] private Slider _bar;
        CombatManagerClient _comabtManager;
       enum HeroState
        {
            Idle = 0,
            Attack,
            Move
        }

        private HeroState _state;
        private Vector3 _targetPosition;
        private Vector3 _position;
        private bool _isAttack;
        private EnemyPuppet _enemy;

        public int can;

        private void Awake()
        {
            _comabtManager = FindObjectOfType<CombatManagerClient>();
        }

        private void Start()
        {
            _state= HeroState.Idle;
            _isAttack= false;
            can = 5;
            _bar.value = 5;
        }
        private void FixedUpdate()
        {
            switch(_state )
            {
                case HeroState.Idle:
                    break;
                case HeroState.Attack: 
                    break;
                case HeroState.Move:
                    HeroMove();
                    break;
            }
        }

        public void Damage()
        {
            can -= 1;

            if (can < 1)
            {
                gameObject.SetActive(false);
                _comabtManager.HeroDead(this);
                return;
            }
            _bar.value = can;
        }

        public void HeroAction(EnemyPuppet enemy)
        {
            if (_state != HeroState.Idle)
                return;

            _enemy = enemy;
            _targetPosition= _enemy.transform.position;
            _targetPosition.x -= 0.5f;
            _position = transform.position;
            _state = HeroState.Move;
        }

        private void HeroMove()
        {
            float distance = Vector3.Distance(transform.position, _targetPosition);
            if(distance >= 0.1f)
            {
                Vector3 temp = _targetPosition - transform.position;
                transform.position += Vector3.Normalize(temp) * 3f * Time.deltaTime;
            }

            else
            {
                if (!_isAttack)
                 {
                     _state= HeroState.Attack;
                     _targetPosition = _position;
                     HeroAttack();
                 }
                 else
                 {
                    _isAttack= false;
                     _state= HeroState.Idle;
                    CombatManagerClient.Who_Turn.Invoke();
                 }
            }
                    
        }

        private void HeroAttack()
        {
            _enemy.Damage();
            _isAttack = true;
            _state= HeroState.Move;
        }

        

    }
}