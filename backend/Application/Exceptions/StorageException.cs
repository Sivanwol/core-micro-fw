using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Enums;

namespace Application.Exceptions;

public class StorageException : BaseException
{
    public StorageException(string message, StatusCodeErrors errorCode) : base("StorageController", errorCode, message)
    {
    }
}
