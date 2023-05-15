using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_tests.FactoryServices
{
    public static class FactoryUser
    {
        internal static User SimpleUser()
        {
            return new User()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a962",
                Name = "Gustavo Lima",
                DocumentId = "051.554.252-34",
                Status = 0
            };
        }
        internal static IEnumerable<User> ListSimpleUser()
        {
            List<User> listUser = new List<User>();
            listUser.Add(SimpleUser());

            return listUser;
        }
    }
}