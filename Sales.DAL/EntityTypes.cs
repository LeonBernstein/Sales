
namespace Sales.DAL
{
    public class EntityTypes
    {
        public string Value { get; private set; }

        private EntityTypes(string value) => Value = value;

        internal static EntityTypes Users { get { return CreateNewType("Users"); } }
        internal static EntityTypes Customers { get { return CreateNewType("Customers"); } }

        private static EntityTypes CreateNewType(string type)
        {
            return new EntityTypes(type);
        }
    }
}
