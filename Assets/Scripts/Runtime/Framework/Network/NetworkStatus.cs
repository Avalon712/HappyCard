using UniVue.ViewModel;

namespace HayypCard
{
    public enum NetworkStatus
    {
        [EnumAlias("单机")]
        Local,

        [EnumAlias("局域网")]
        LAN,

        [EnumAlias("广域网")]
        WAN,
    }
}
