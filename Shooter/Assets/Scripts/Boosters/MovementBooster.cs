namespace Boosters
{
    public class MovementBooster: Booster
    {
        protected override void ActivateBooster()
        {
            PlayerMovement.UpdateBooster?.Invoke(booster, boosterDuration);
        }
    }
}