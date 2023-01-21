namespace PierreMizzi.MouseInteractable
{
    public enum HoldClickStatus
    {
        None,

        // Still considered to be a simple click
        inClick,

        // Button is being held, maybe to make an HoldClick ?
        inTreshold,

        // Button is still being held, it's going for a HoldClick
        inLong,

        // HoldClick as been completed
        completed,
    }
}
