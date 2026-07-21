namespace Entities
{
    public interface IDamageReceiver
    {
        void ReceiveDamage();
        public void SetCanReceiveDamage(bool value);
    }
}