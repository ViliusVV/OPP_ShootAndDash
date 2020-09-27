using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ModelInterfaces
{
    interface IGun
    {
        public GunId GunId { get; set; }
        public int AmmoLeft { get; set; }
    }
}
