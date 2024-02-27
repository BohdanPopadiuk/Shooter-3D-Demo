namespace Weapons
{
    public interface IWeaponTarget
    {
        public float Health { get; }
        public bool Dead { get; }
        public void TakeDamage(float damage);
    }
}