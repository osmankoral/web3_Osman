using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Client.Skill;
using TMPro;

namespace Client.GamePlay
{
    public class HeroPuppet : MonoBehaviour
    {
        [SerializeField] private GameObject _arrowObject;
        [SerializeField] private bool _isRanger;
        public HeroSO _heroSO;
        public SkillData[] _skillData;
        public GameObject defaultAttackVFX;
        [SerializeField] private Slider _bar;
        [SerializeField] public GameObject Mark;
        [SerializeField] private int _skillCount;
        [SerializeField] private Sprite[] _skillImage;
        CombatManagerClient _combatManager;
        Animator _animator;

        private bool isText;
        private float timer;

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
        private int _health;

        private float _charSpeed;

        public float CharSpeed
        {
            get => _charSpeed;
            set => _charSpeed = value;
        }

        private void Awake()
        {
            _combatManager = FindObjectOfType<CombatManagerClient>();
           
        }

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _state= HeroState.Idle;
            
            _isAttack= false;
            _health = 5;
            _bar.value = 5;
            _bar.maxValue = _health;


            _charSpeed = Random.Range(1f, 1.5f);
        }
        private void FixedUpdate()
        {
            if (isText)
                TextScale();

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

        public void HeroSetSkill(out int skillCount,out Sprite[] img)
        {
            skillCount = _skillCount;
            img = _skillImage;
        }

        public void Damage()
        {
           _health -= 1;

            if (_health < 1)
            {
                gameObject.SetActive(false);
                _combatManager.HeroDead(this);
                return;
            }
            _bar.value = _health;
            transform.GetChild(4).gameObject.SetActive(true);
            timer = Time.time;
            isText= true;
        }

        public void HeroAction(EnemyPuppet enemy, int whichSkill)
        {
            if (_state != HeroState.Idle)
                return;
            _enemy = enemy;
            switch(whichSkill)
            {
                case 0:
                    Attack();
                    break;


                case 1:
                    Skill1();
                    break;


                case 2:
                    Skill2();
                    break;
            }
            
        }

        private void Attack()
        {
            if(_isRanger)
            {
                _position = transform.position;
                StartCoroutine(HeroAttack());
            }
            
            else
            {
                _targetPosition = _enemy.transform.position;
                _targetPosition.z -= 1.5f;
                _targetPosition.y = transform.position.y;
                _position = transform.position;
                _state = HeroState.Move;
            }
        }

        private void Skill1()
        {
            StartCoroutine(SkillDelay());

        }

        private void Skill2()
        {
            StartCoroutine(Skill2Delay());
        }

        IEnumerator SkillDelay()
        {
            Transform[] enemyTransforms = new Transform[1];
            enemyTransforms[0] = _enemy.transform;
            _skillData[0].Perform(transform, enemyTransforms);

            yield return new WaitForSeconds(1);
            _isAttack = false;
            _state = HeroState.Idle;
            Mark.SetActive(false);
            _combatManager.Who_Turn.Invoke();
        }

        IEnumerator Skill2Delay()
        {
            Transform[] enemyTransforms = new Transform[1];
            enemyTransforms[0] = _enemy.transform;
            _skillData[1].Perform(transform, enemyTransforms);

            yield return new WaitForSeconds(1);
            _isAttack = false;
            _state = HeroState.Idle;
            Mark.SetActive(false);
            _combatManager.Who_Turn.Invoke();
        }
        private void HeroMove()
        {
            float distance = Vector3.Distance(transform.position, _targetPosition);
            if(distance >= 0.1f)
            {
                _animator.SetBool("isMove", true);
                Vector3 temp = _targetPosition - transform.position;
                transform.position += Vector3.Normalize(temp) * 10f * Time.deltaTime;
            }

            else
            {
                if (!_isAttack)
                 {
                    _animator.SetBool("isMove", false);

                    
                     _state= HeroState.Attack;
                     _targetPosition = _position;
                    StartCoroutine(HeroAttack());
                 }
                 else
                 {
                    _animator.SetBool("isMove", false);
                    _isAttack= false;
                     _state= HeroState.Idle;
                    Mark.SetActive(false);
                    _combatManager.Who_Turn.Invoke();
                 }
            }
                    
        }

        IEnumerator HeroAttack()
        {
            if(_isRanger)
            {
                _animator.SetTrigger("isAttack");
                yield return new WaitForSeconds(0.7f);
                GameObject arrow = Instantiate(_arrowObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                arrow.GetComponent<RangeAttackManager>().Enemy = _enemy.gameObject;
                yield return new WaitForSeconds(2.7f);
                _isAttack = true;
                _targetPosition = _position;
                _state = HeroState.Move;
            }

            else
            {

                _animator.SetTrigger("isAttack");

                yield return new WaitForSeconds(0.8f);
                Instantiate(defaultAttackVFX, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
                _enemy.Damage();

                yield return new WaitForSeconds(0.8f);
                _isAttack = true;
                _state= HeroState.Move;
            }
        }

        

    }
}