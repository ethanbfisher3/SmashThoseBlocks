namespace Blocks
{
    public class WeightBlock : Block
    {
        float otherBlockMass;
        float otherThrowPower;

        public override void OnBecomeFusedTo(Block other)
        {
            otherBlockMass = other.Rigidbody.mass;
            otherThrowPower = other.throwPower;
            other.Rigidbody.mass = Rigidbody.mass;
            other.throwPower = throwPower;
        }

        public override void OnBreakApartFrom(Block other)
        {
            other.Rigidbody.mass = otherBlockMass;
            other.throwPower = otherThrowPower;
        }

        public override bool CanFuseWith(Block other) => !other.ContainsBlockOfType<WeightBlock>();
    }
}