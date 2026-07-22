namespace Entities
{
    public interface IHitReceiver
    {
        void ReceiveHit();
        public void SetCanReceiveHit(bool value);
    }
}