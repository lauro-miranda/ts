using System.ComponentModel;

namespace TSDelivery.Deliverymans.SiloHost.Grains.Enums
{
    public enum LocationStatus
    {
        [Description("Aguando novas entregas")]
        Disconnected = 1,
        [Description("Aguando novas entregas")]
        Awaiting = 2,
        [Description("Entrega em andamento")]
        OnDelivery = 3
    }
}