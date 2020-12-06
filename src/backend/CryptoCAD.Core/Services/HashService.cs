using System;
using CryptoCAD.Domain.Repositories;
using CryptoCAD.Core.Hashers.SHA256;
using CryptoCAD.Core.Hashers.Abstractions;
using CryptoCAD.Core.Services.Abstractions;
using CryptoCAD.Core.Factories.Abstractions;

namespace CryptoCAD.Core.Services
{
    public class HashService : IHashService
    {
        private readonly IHasherFactory HasherFactory;
        private readonly IMethodsRepository MethodsRepository;

        public HashService(IMethodsRepository methodsRepository)
        {
            MethodsRepository = methodsRepository;
        }

        public string Hash(string name, string data, string configuration)
        {
            var hasher = GetHasher(name, configuration);
            return hasher.Hash(data);
        }

        private IHasher GetHasher(string name, string configuration)
        {
            switch (name.ToLowerInvariant())
            {
                case "sha256":
                    return new SHA256Hasher();
                default:
                    try
                    {
                        return HasherFactory.CreateHasher(configuration);
                    }
                    catch (Exception exception)
                    {
                        throw new NotImplementedException($"Hasher '{name}' is not supported!", exception);
                    }
            }
        }
    }
}