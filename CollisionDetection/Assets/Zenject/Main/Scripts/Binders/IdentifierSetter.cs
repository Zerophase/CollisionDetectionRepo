using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenject
{
    public class IdentifierSetter
    {
        readonly ProviderBase _provider;

        public IdentifierSetter(ProviderBase provider)
        {
            _provider = provider;
        }

        public void As(object identifier)
        {
            _provider.Identifier = identifier;
        }
    }
}

