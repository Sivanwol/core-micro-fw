using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.GQL
{
    public class ClientVendor : Vendor
    {
        [Description("Client")]
        public Client Client { get; set; }

    }
}