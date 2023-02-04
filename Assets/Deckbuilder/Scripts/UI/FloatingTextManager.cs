using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deckbuilder
{
    public class FloatingTextManager : Singleton<FloatingTextManager>
    {
        [SerializeField] private FloatingText floatingTextPrefab;

        private void Start()
        {
            SpawnText(new Vector3(-1000000000, 0, 0), 0, FloatingTextType.Damage);
        }

        public void SpawnText(Vector3 pos, int value, FloatingTextType type)
        {
            if (value != 0)
                Instantiate(floatingTextPrefab, Camera.main.WorldToScreenPoint(pos), Quaternion.identity, transform).Spawn(type, value.ToString());
        }
    }
    public enum FloatingTextType
    {
        Damage,
        Block,
        Heal
    }
}