namespace TideDefense
{
    public struct SandWaterFilling
    {
        public float quantity;
        public float sandConcentration;
        public float waterConcentration
        {
            get { return 1f - sandConcentration; }
        }

        public SandWaterFilling(float quantity, float sandConcentration)
        {
            this.quantity = quantity;
            this.sandConcentration = sandConcentration;
        }

        public override string ToString()
        {
            return $"Quantity : {quantity} \r Sand Concentration : {sandConcentration}";
        }

        public static SandWaterFilling operator +(SandWaterFilling a, SandWaterFilling b)
        {
            float rSandConcentration =
                ((a.sandConcentration * a.quantity) + (b.sandConcentration * b.quantity))
                / (a.quantity + b.quantity);
            return new SandWaterFilling(a.quantity + b.quantity, rSandConcentration);
        }

        public static float GetSandConcentrationFromWetness(float wetness)
        {
            return 1f - (wetness / 2f);
        }
    }
}
