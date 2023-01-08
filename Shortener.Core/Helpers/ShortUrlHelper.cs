using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.Helpers;

public static class ShortUrlHelper
{
    public static string Encode(int num) 
        => WebEncoders.Base64UrlEncode(BitConverter.GetBytes(num));

    public static int Decode(string str)
        => BitConverter.ToInt32(WebEncoders.Base64UrlDecode(str));
}
