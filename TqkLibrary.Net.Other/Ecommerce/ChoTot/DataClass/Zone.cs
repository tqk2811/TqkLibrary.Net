#if NET462_OR_GREATER
namespace TqkLibrary.Net.Ecommerce.ChoTot
{
    public class Zone
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}
#endif