namespace Entities
{
    public interface IHitReceiver
    {
        void ReceiveHit(IAttackSource attackSource);
        public void SetCanReceiveTap(bool value);
    }
}