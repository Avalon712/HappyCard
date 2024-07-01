
namespace HappyCard
{
    public sealed class User
    {
        public string name { get; set; }
        public string password { get; set; }
        public bool remember { get; set; }
        public string email { get; set; }

        public override string ToString()
        {
            return $"User{{name={name}, password={password}, email={email}, remember={remember}}}";
        }
    }
}
