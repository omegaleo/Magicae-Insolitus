using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : InstancedBehavior<PlayerManager>, IEntity
{
    [SerializeField] private float runSpeed = 10f;

    private float _health = 3f;
    private float _maxHealth = 3f;
    private float _mp = 3f;
    private float _maxMp = 3f;
    private bool _canDamage = true;
    public int coins = 0;
    
    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;
    
    private float _aimAngle;

    [SerializeField] private Transform _firePoint;
    [SerializeField] private List<ScriptableObject> _spells;
    public List<IEntity> CapturedMonsters = new List<IEntity>();

    private int _curSpell = 0;

    public Transform FirePoint => _firePoint;
    
    public string GetHeartString()
    {
        int fullHearts = (int)_health;
        float remainder = _health - fullHearts;

        string hearts = "";

        for (int i = 0; i < fullHearts; i++)
        {
            // add a heart
            hearts += "<sprite=0>";
        }

        if (remainder > 0f && remainder <= 0.25f)
        {
            // add a quarter heart
            hearts += "<sprite=3>";
        }
        else if (remainder > 0.25f && remainder <= 0.5f)
        {
            // add a half heart
            hearts += "<sprite=2>";
        }
        else if (remainder > 0.5f && remainder <= 0.75f)
        {
            // add a 3 quarters heart
            hearts += "<sprite=1>";
        }
        
        return hearts;
    }
    
    public string GetManaString()
    {
        int fullMP = (int)_mp;
        float remainder = _mp - fullMP;
        string mana = "";

        for (int i = 0; i < fullMP; i++)
        {
            // add a heart
            mana += "<sprite=4>";
        }
        
        if (remainder > 0f && remainder <= 0.25f)
        {
            // add a quarter heart
            mana += "<sprite=7>";
        }
        else if (remainder > 0.25f && remainder <= 0.5f)
        {
            // add a half heart
            mana += "<sprite=6>";
        }
        else if (remainder > 0.5f && remainder <= 0.75f)
        {
            // add a 3 quarters heart
            mana += "<sprite=5>";
        }
        
        return mana;
    }

    public string CoinString => $"<sprite=8> {coins}";

    private Rigidbody2D _rb2d;

    private bool _recoveringMP = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _health = 3f;
        _mp = 3f;
        _maxMp = 3f;
        KeyBinder.instance.OnMove += OnMove;
        KeyBinder.instance.OnAimMove += OnAimMove;
        KeyBinder.instance.OnFire += OnFire;
        KeyBinder.instance.OnNextSpell += NextSpell;
        KeyBinder.instance.OnPrevSpell += PrevSpell;
        _collider = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        HUD.instance.UpdateText();
        UpdateSpells();
    }

    
    private void OnDestroy()
    {
        KeyBinder.instance.OnMove -= OnMove;
        KeyBinder.instance.OnAimMove -= OnAimMove;
        KeyBinder.instance.OnFire -= OnFire;
        KeyBinder.instance.OnNextSpell -= NextSpell;
        KeyBinder.instance.OnPrevSpell -= PrevSpell;
    }

    /*private void OnEnable()
    {
        KeyBinder.instance.OnMove += OnMove;
        KeyBinder.instance.OnAimMove += OnAimMove;
        KeyBinder.instance.OnFire += OnFire;
        KeyBinder.instance.OnNextSpell += NextSpell;
        KeyBinder.instance.OnPrevSpell += PrevSpell;
    }
    
    private void OnDisable()
    {
        KeyBinder.instance.OnMove -= OnMove;
        KeyBinder.instance.OnAimMove -= OnAimMove;
        KeyBinder.instance.OnFire -= OnFire;
        KeyBinder.instance.OnNextSpell -= NextSpell;
        KeyBinder.instance.OnPrevSpell -= PrevSpell;
    }*/

    private void NextSpell()
    {
        _curSpell = GetSpell(1);
        UpdateSpells();
    }

    private void PrevSpell()
    {
        _curSpell = GetSpell(-1);
        UpdateSpells();
    }

    private int GetSpell(int increment)
    {
        var curSpell = _curSpell + increment;
        if (curSpell >= _spells.Count)
        {
            curSpell = 0;
        }
        if (curSpell < 0)
        {
            curSpell = _spells.Count - 1;
        }

        return curSpell;
    }

    private void OnFire()
    {
        try
        {
            var spell = (_spells[_curSpell] as ISpell);

            if (spell != null && spell.MpCost() <= _mp)
            {
                spell.Execute();
                _mp -= spell.MpCost();
                HUD.instance.UpdateText();
                SfxManager.instance.PlaySound(SfxManager.SfxType.Shoot);
            }

            if (_mp < _maxMp && !_recoveringMP)
            {
                // Start recovering
                StartCoroutine(RecoverMP());
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            //throw;
        }
    }

    private IEnumerator RecoverMP()
    {
        _recoveringMP = true;
        
        while (_mp < _maxMp)
        {
            yield return new WaitForSeconds(2.5f);
            _mp++;
            HUD.instance.UpdateText();
        }
        
        _recoveringMP = false;
    }
    
    private void OnAimMove(float x, float y)
    {
        if (x != 0f || y != 0f)
        {
            var dir = new Vector3(x, y, 0f) - Camera.main.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            //_aimAngle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90f;
            //_rb2d.rotation = _aimAngle;
        }
    }
    
    private void OnMove(float horizontal, float vertical)
    {
        if (!UIManager.instance.IsUIOpen)
        {
            if (horizontal != 0f || vertical != 0f)
            {
                _rb2d.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
                _rb2d.constraints = RigidbodyConstraints2D.None;
            }
            else
            {
                _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    public Guid GUID()
    {
        return new Guid();
    }

    public void DoDamage(float damage)
    {
        if (!_canDamage) return;
        
        _health -= damage;

        if (_health <= 0f)
        {
            Debug.Log("Player died");
        }

        SfxManager.instance.PlaySound(SfxManager.SfxType.Hit);
        HUD.instance.UpdateText();
        
        // Flash red
        StartCoroutine(Flashing());
    }

    private IEnumerator Flashing()
    {
        int loop = 0;

        //_collider.enabled = false;

        _canDamage = false;
        
        while (loop < 4)
        {
            if (_sprite.color == Color.white)
            {
                _sprite.color = Color.red;
            }
            else
            {
                _sprite.color = Color.white;
            }

            yield return new WaitForSeconds(0.25f);
            loop++;
        }
        
        _sprite.color = Color.white;
        _collider.enabled = true;
        _canDamage = true;
    }

    public void AddHearts(float hearts)
    {
        _health += hearts;

        if (_health > _maxHealth)
        {
            _maxHealth = Mathf.Ceil(_health);
        }
        
        HUD.instance.UpdateText();
    }
    
    public void AddMP(float mp)
    {
        _mp += mp;

        if (_mp > _maxMp)
        {
            _maxMp = Mathf.Ceil(_mp);
        }
        
        HUD.instance.UpdateText();
    }

    public void AddSpell(ScriptableObject spell)
    {
        _spells.Add(spell);
        _curSpell = 0;
        UpdateSpells();
    }

    void UpdateSpells()
    {
        SpellList.instance.UpdateIcons((_spells[_curSpell] as ISpell), (_spells[GetSpell(-1)] as ISpell), (_spells[GetSpell(1)] as ISpell));
    }
    
    public void AddCoins(int coins)
    {
        this.coins += coins;
        HUD.instance.UpdateText();
    }

    public void AddCapturedMonster(IEntity entity)
    {
        CapturedMonsters.Add(entity);
        
        if (MonsterManager.instance.CaughtThemAll())
        {
            AchievementUI.instance.UnlockAchievement("CAUGHT");
        }
    }

    public GameObject GetRandomCapturedMonster()
    {
        var monster = (CapturedMonsters != null && CapturedMonsters.Any()) ? CapturedMonsters.Random() : null;

        if (monster == null) return null;

        return MonsterManager.instance.GetMonster(monster);
    }
}
