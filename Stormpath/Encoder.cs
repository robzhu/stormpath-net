using System;
using System.Text;

namespace Stormpath
{
    internal static class Encoder
    {
        public static string FormatAndBase64Encode( string username, string password )
        {
            string formatted = string.Format( "{0}:{1}", username, password );
            return Convert.ToBase64String( Encoding.UTF8.GetBytes( formatted ) );
        }
    }
}
