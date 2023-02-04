using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Deckbuilder
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private Color damageColor;
        [SerializeField] private Color blockColor;
        [SerializeField] private Color healColor;
        [SerializeField] private Vector2 minMaxUpwardOffset;
        [SerializeField] private Vector2 minMaxSideOffset;
        [SerializeField] private float timeBeforeFading;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void Spawn(FloatingTextType type, string str)
        {
            _text.text = str;
            transform.DOMove(transform.position + new Vector3(Random.Range(minMaxSideOffset.x, minMaxSideOffset.y), Random.Range(minMaxUpwardOffset.x, minMaxUpwardOffset.y), 0), timeBeforeFading);
            _text.DOFade(0, 0.25f).SetDelay(timeBeforeFading);

            switch (type)
            {
                case FloatingTextType.Damage:
                    _text.color = damageColor;
                    break;
                case FloatingTextType.Block:
                    _text.color = blockColor;
                    break;
                case FloatingTextType.Heal:
                    _text.color = healColor;
                    break;
                default:
                    break;
            }
        }
    }
}