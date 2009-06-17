using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AspComet
{
    public class RngUniqueClientIDGenerator : IClientIDGenerator
    {
        private static readonly Regex NON_ALPHANUM = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);

        private readonly RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        private readonly IClientRepository clientRepository;

        public RngUniqueClientIDGenerator(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public string GenerateClientID()
        {
            string clientID;
            do
            {
                byte[] bytes = new byte[15];
                this.rngCryptoServiceProvider.GetBytes(bytes);
                clientID = NON_ALPHANUM.Replace(Convert.ToBase64String(bytes), "");
            }
            while (this.clientRepository.ContainsID(clientID));
            return clientID;
        }

    }
}