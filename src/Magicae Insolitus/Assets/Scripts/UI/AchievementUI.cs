using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Attributes;
using OmegaLeo.Toolbox.Runtime.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : InstancedBehavior<AchievementUI>
{
    [ColoredHeader("Configuration")]
    [SerializeField] private float hideY;
    [SerializeField] private float showY;
    [SerializeField] private float speed = 1f;
    [SerializeField] private List<Achievement> _achievements = new List<Achievement>();

    [ColoredHeader("UI")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;

    private bool _show = false;
    
    private void Update()
    {
        if (_show)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, showY, 0f), speed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, hideY, 0f), speed * Time.deltaTime);
        }
    }

    private void Start()
    {
        SetPositions();
    }
    
    private void SetPositions()
    {
        var rect = GetComponent<RectTransform>();
        var offset = rect.rect.height;

        showY = transform.localPosition.y;
        hideY = transform.localPosition.y + offset;
        transform.localPosition = new Vector3(transform.localPosition.x, hideY, 0f);
    }

    public void UnlockAchievement(string identifier)
    {
        var achievement =
            _achievements.FirstOrDefault(x => x.Identifier.Equals(identifier, StringComparison.OrdinalIgnoreCase));

        if (achievement != null && !achievement.Unlocked)
        {
            text.text = achievement.Text;
            image.sprite = achievement.Icon;
            _show = true;
            StartCoroutine(Hide());
            achievement.Unlocked = true;
        }
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(5f);
        _show = false;
    }
}
