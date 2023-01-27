using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;
using UnityEngine.UI;

public class SpellList : InstancedBehavior<SpellList>
{
    [SerializeField] private Image _prevSpell;
    [SerializeField] private Image _curSpell;
    [SerializeField] private Image _nextSpell;

    public void UpdateIcons(ISpell curSpell, ISpell prevSpell, ISpell nextSpell)
    {
        _prevSpell.sprite = prevSpell?.GetIcon();
        _curSpell.sprite = curSpell?.GetIcon();
        _nextSpell.sprite = nextSpell?.GetIcon();
    }
}
