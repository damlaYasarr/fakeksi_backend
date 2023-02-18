using System.Text;

namespace WEBAPI.Utilities.Security.Hashing
{
    public class Hashinghelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt  )
        {   //password encrypted-create
            using (var hmac= new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            }
        }
        public static bool VerifyPasswordHash(string password,  byte[] passwordhash, byte[] passwordsalt)
        { //verify
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
              
                var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedhash.Length; i++)
                {//compare
                    if (computedhash[i] != passwordhash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }


    }
}
