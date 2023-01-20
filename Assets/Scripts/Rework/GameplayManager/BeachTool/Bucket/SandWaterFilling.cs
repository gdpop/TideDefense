namespace TideDefense
{
    public struct SandWaterFilling
    {
		public float quantity;
        public float sandConcentration;
		public float waterConcentration
		{
			get 
			{
				return 1f - sandConcentration;
			}
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
    }
}
