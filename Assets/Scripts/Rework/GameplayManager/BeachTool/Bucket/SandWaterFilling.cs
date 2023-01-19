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
    }
}
