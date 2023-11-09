using System.Collections.Generic;
using Projectiles;
using UnityEngine;

namespace ObjectPool.Good
{
    public class BulletsObjectPool : MonoBehaviour
    {
        private readonly List<Bullet> _freeBullets = new();
        private readonly List<Bullet> _bulletsInUse = new();

        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _bulletsToCreate;

        private void Start()
        {
            for (int i = 0; i < _bulletsToCreate; i++)
                _freeBullets.Add(Instantiate(_bulletPrefab, transform));
        }

        public Bullet GetBullet()
        {
            Bullet bullet;
            if (_freeBullets.Count > 1)
            {
                bullet = _freeBullets[0];
                _freeBullets.RemoveAt(0);
            }
            else
            {
                bullet = Instantiate(_bulletPrefab, transform);
            }

            bullet.ReturnRequested += OnBulletReturnRequested;
            _bulletsInUse.Add(bullet);
            return bullet;
        }

        public void ReturnAllBullets()
        {
            foreach(var bullet in _bulletsInUse)
                ReturnBullet(bullet);
            _bulletsInUse.Clear();
        }

        private void OnBulletReturnRequested(Bullet bullet)
        {
            _bulletsInUse.Remove(bullet);
            ReturnBullet(bullet);
        }

        private void ReturnBullet(Bullet bullet)
        {
            bullet.ReturnRequested -= OnBulletReturnRequested;
            bullet.gameObject.SetActive(false);
            bullet.ResetBullet();
            _freeBullets.Add(bullet);
        }
    }
}