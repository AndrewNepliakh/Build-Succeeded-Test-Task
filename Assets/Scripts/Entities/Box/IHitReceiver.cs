namespace Entities
{
    public interface IHitReceiver
    {
        void ReceiveHit();
        public void SetCanReceiveTap(bool value);
    }
}