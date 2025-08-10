using System.Security.Cryptography;
using System.Text;

namespace Aggregation.Backend.Infrastructure.Cache
{
    public class LoginStore
    {
        private readonly Dictionary<string, string> _userNamesWithHashes = new();

        public LoginStore()
        {
            for (int i = 0; i < 10; i++)
            {
                _userNamesWithHashes[$"user{i}"] = ComputeSha256Hash($"password{i}");
            }
        }

        public bool ValidateUsersPassword(string user, string password)
        {
            return _userNamesWithHashes.TryGetValue(user, out var hashed) && hashed == ComputeSha256Hash(password);
        }

        public string ComputeSha256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}