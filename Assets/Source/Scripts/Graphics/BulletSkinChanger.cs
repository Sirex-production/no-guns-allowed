using UnityEngine;

namespace Ingame.Graphics
{
    [RequireComponent(typeof(Bullet))]
    public class BulletSkinChanger : MonoBehaviour
    {
        [Tooltip("Material that will be applied to the bullet after player reflect it")]
        [SerializeField] private Material reflectSkin;
        [Tooltip("Color of the trail after player reflect bullet")]
        [SerializeField] private Color reflectTrailColor;
        [Space]
        [SerializeField] private MeshRenderer bulletMeshRenderer;
        [SerializeField] private TrailRenderer bulletTrailRenderer;

        private Bullet _bullet;

        private void Awake()
        {
            _bullet = GetComponent<Bullet>();
        }

        private void Start()
        {
            _bullet.OnReflect += ChangeAppearance;
        }

        private void OnDestroy()
        {
            _bullet.OnReflect -= ChangeAppearance;
        }

        private void ApplyReflectionSkin()
        {
            if(bulletMeshRenderer != null)
                bulletMeshRenderer.material = reflectSkin;
            if (bulletTrailRenderer != null)
            {
                bulletTrailRenderer.startColor = reflectTrailColor;
                bulletTrailRenderer.endColor = reflectTrailColor;
            }
        }

        private void ChangeAppearance(Bullet.ReflectionType reflectionType)
        {
            switch (reflectionType)
            {
                case Bullet.ReflectionType.Surface:
                    break;
                case Bullet.ReflectionType.Player:
                    ApplyReflectionSkin();
                    break;
            }
        }
    }
}