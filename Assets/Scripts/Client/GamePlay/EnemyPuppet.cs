using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Client.GamePlay
{
    public class EnemyPuppet : MonoBehaviour
    {
        [SerializeField] CombatManagerClient _combatManagerClient;
        [SerializeField] private Slider _bar;
        [SerializeField] public GameObject Mark;

        Animator _animator;
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

        private float _charSpeed;

        public int can;
        private float timer;
        private bool isText;

        public float CharSpeed
        {
            get => _charSpeed;
            set => _charSpeed = value;
        }

        private void Awake()
        {
            _combatManagerClient= FindObjectOfType<CombatManagerClient>();
        }

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _state = EnemyState.Idle;
            _isAttack = false;
            can = 5;
            _bar.value = can;
            _bar.maxValue = can;
            _charSpeed = Random.Range(1f, 1.5f);
        }
        private void FixedUpdate()
        {
            if(isText)
            {
                TextScale();
            }

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

        private void TextScale()
        {
            if (Time.time - timer > 2f)
            {
                transform.GetChild(4).gameObject.SetActive(false);
                isText = false;
                return;
            }
            transform.GetChild(4).position += transform.up * 0.1f * Time.deltaTime;
            transform.GetChild(4).localScale -= new Vector3(0.0001f, 0.0001f, 0.0001f);
        }

        public void MarkChgange(bool isActive, int layer)
        {
            Mark.gameObject.SetActive(isActive);
            gameObject.transform.GetChild(0).gameObject.layer = layer;
            
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
            transform.GetChild(4).gameObject.SetActive(true);
            timer = Time.time;
            isText = true;
        }

        public void EnemyAction(HeroPuppet hero)
        {
            if (_state != EnemyState.Idle)
                return;
            
            _hero = hero;
            _targetPosition = hero.transform.position;
            _targetPosition.z += 1.5f;
            _targetPosition.y = transform.position.y;
            _position = transform.position;
            _state = EnemyState.Move;
        }

        private void EnemyMove()
        {
            float distance = Vector3.Distance(transform.position, _targetPosition);
            if (distance >= 0.1f)
            {
                _animator.SetBool("isMove", true);
                Vector3 temp = _targetPosition - transform.position;
                transform.position += Vector3.Normalize(temp) * 3f * Time.deltaTime;
            }

            else
            {
                if (!_isAttack)
                {
                    _animator.SetBool("isMove", false);

                    _state = EnemyState.Attack;
                    _targetPosition = _position;
                    StartCoroutine(EnemyAttack());
                }
                else
                {
                    _animator.SetBool("isMove", false);
                    _isAttack = false;
                    _state = EnemyState.Idle;
                    _combatManagerClient.Who_Turn.Invoke();
                }
            }

        }

        IEnumerator EnemyAttack()
        {
            _animator.SetTrigger("isAttack");
            yield return new WaitForSeconds(2.2f);
            _hero.Damage();
            _isAttack = true;
            _state = EnemyState.Move;
        }
    }
}