using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Core.Exceptions;

public class UrlNotFoundException : Exception
{
    public UrlNotFoundException() : base() { }

    public UrlNotFoundException(string message) : base(message) { }

    public UrlNotFoundException(string message, Exception? innerException) : base(message, innerException) { }
}
