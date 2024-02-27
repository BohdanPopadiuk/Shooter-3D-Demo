namespace Boosters
{
    public interface IBoosterClient
    {
        public BoosterType BoosterType { get; }
        public float Booster { get; set; }
    }
}