using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Newtonsoft.Json.Linq;
using Amg_ingressos_aqui_cadastro_api.Dtos;

namespace Amg_ingressos_aqui_cadastro_tests.FactoryServices
{
    public static class FactoryUser
    {
        internal static User SimpleUser()
        {
            return new User()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a954",
                Name = "isabella",
                DocumentId = "05292425234",
                Status = 0,
                Type = TypeUserEnum.Admin,
                Address = new Address()
                {
                    AddressDescription = "Parque Sabiázinho",
                    Cep = "38400000",
                    Number = "768",
                    Neighborhood = "teste",
                    Complement = "Banco da Praça",
                    ReferencePoint = "arbusto ao lado",
                    City = "Uberlandia",
                    State = "MG",
                },
                Contact = new Contact()
                {
                    Email = "isabel1a@gmail.com",
                    PhoneNumber = "34994568769"
                },
                UserConfirmation = new UserConfirmation() {
                    EmailConfirmationCode = "wieufhu233f23fnf",
                    EmailConfirmationExpirationDate = new DateTime(2024, 02, 01, 16, 00, 00),
                    EmailVerified = true,
                    PhoneVerified = true
                },
                Password = "1Dois3$56&8",
            };
        }
        
        internal static User CustomerUser()
        {
            return new User()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a958",
                Name = "isabella",
                DocumentId = "05292425234",
                Status = 0,
                Type = TypeUserEnum.Customer,
                Address = new Address()
                {
                    AddressDescription = "Parque Sabiázinho",
                    Cep = "38400000",
                    Number = "768",
                    Neighborhood = "teste",
                    Complement = "Banco da Praça",
                    ReferencePoint = "arbusto ao lado",
                    City = "Uberlandia",
                    State = "MG",
                },
                Contact = new Contact()
                {
                    Email = "isabel1a@gmail.com",
                    PhoneNumber = "34994568769"
                },
                UserConfirmation = new UserConfirmation() {
                    EmailConfirmationCode = "wieufhu233f23fnf",
                    EmailConfirmationExpirationDate = new DateTime(2024, 02, 01, 16, 00, 00),
                    EmailVerified = true,
                    PhoneVerified = true
                },
                Password = "1Dois3$56&8",
            };
        }
        
        internal static User ProducerUser()
        {
            return new User()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a95c",
                Name = "isabella",
                DocumentId = "05292425234",
                Status = 0,
                Type = TypeUserEnum.Producer,
                Address = new Address()
                {
                    AddressDescription = "Parque Sabiázinho",
                    Cep = "38400000",
                    Number = "768",
                    Neighborhood = "teste",
                    Complement = "Banco da Praça",
                    ReferencePoint = "arbusto ao lado",
                    City = "Uberlandia",
                    State = "MG",
                },
                Contact = new Contact()
                {
                    Email = "isabel1a@gmail.com",
                    PhoneNumber = "34994568769"
                },
                UserConfirmation = new UserConfirmation() {
                    EmailConfirmationCode = "wieufhu233f23fnf",
                    EmailConfirmationExpirationDate = new DateTime(2024, 02, 01, 16, 00, 00),
                    EmailVerified = true,
                    PhoneVerified = true
                },
                Password = "1Dois3$56&8",
            };
        }
        
        internal static User ColabUser()
        {
            return new User()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a962",
                Name = "isabella",
                DocumentId = "05292425234",
                Status = 0,
                Type = TypeUserEnum.Colab,
                Contact = new Contact()
                {
                    Email = "isabel1a@gmail.com",
                    PhoneNumber = "",
                },
                Password = "1Dois3$56&8",
            };
        }
        
        internal static List<User> ListSimpleUser()
        {
            List<User> listUser = new List<User>();
            listUser.Add(new User(SimpleUser()));
            listUser.Add(new User(ProducerUser()));
            listUser.Add(new User(ColabUser()));

            return listUser;
        }
        
        internal static List<User> ListCloab()
        {
            List<User> listColab = new List<User>();
            listColab.Add(new User(ColabUser()));

            return listColab;
        }
        
        internal static List<User> ListProducer()
        {
            List<User> listProducer = new List<User>();
            listProducer.Add(new User(ProducerUser()));

            return listProducer;
        }
    }
}