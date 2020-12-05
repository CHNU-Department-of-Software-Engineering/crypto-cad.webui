using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using CryptoCAD.Domain.Entities;
using CryptoCAD.Domain.Entities.DES;
using CryptoCAD.Domain.Repositories;
using CryptoCAD.Common.CiphersConfiguration;

namespace CryptoCAD.Data.Repositories
{
    public class TempCipherEntityRepository : ICipherEntityRepository
    {
        private readonly string StoragePath;
        private readonly ICollection<CipherEntity> Ciphers;

        public TempCipherEntityRepository(string storagePath, bool dropCreate = false)
        {
            StoragePath = storagePath;
            if (!File.Exists(StoragePath))
            {
                Ciphers = InitStorage(StoragePath);
            }
            else if (dropCreate)
            {
                File.Delete(StoragePath);
                Ciphers = InitStorage(StoragePath);
            }
            else
            {
                Ciphers = ReadAll(StoragePath);
            }
        }

        public void Add(CipherEntity entity)
        {
            Ciphers.Add(entity);
        }

        public void AddRange(IList<CipherEntity> entities)
        {
            foreach (var entity in entities)
            {
                Ciphers.Add(entity);
            }
        }

        public CipherEntity Get(Guid id)
        {
            return Ciphers.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<CipherEntity> GetAll()
        {
            return Ciphers;
        }

        public void Remove(CipherEntity entity)
        {
            Ciphers.Remove(entity);
        }

        public void Update(CipherEntity entity)
        {
            Remove(entity);
            Add(entity);
        }

        public void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(Ciphers);
            File.WriteAllText(StoragePath, json);
        }

        private ICollection<CipherEntity> ReadAll(string path)
        {
            var json = File.ReadAllText(path);
            var ciphers = JsonConvert.DeserializeObject<ICollection<CipherEntity>>(json);
            return ciphers;
        }
        private ICollection<CipherEntity> InitStorage(string path)
        {
            var ciphers = GetCiphers();
            var json = JsonConvert.SerializeObject(ciphers);
            File.WriteAllText(path, json);
            return ciphers;
        }
        private ICollection<CipherEntity> GetCiphers()
        {
            var ciphers = new List<CipherEntity>
            {
                new CipherEntity
                {
                    Id = new Guid("30870aee-f7ea-4f6d-aa60-fdfc48cc9a60"),
                    Name = "des",
                    Type = CipherTypes.SymmetricCipher,
                    IsModifiable = false,
                    Configuration = string.Empty,
                    Key = new CipherKeyEntity
                    {
                        Type = typeof(string).Name,
                        Length = 8
                    }
                },
                new CipherEntity
                {
                    Id = new Guid("8cb02965-ceab-4afb-bd70-3e4382f3ddae"),
                    Name = "aes",
                    Type = CipherTypes.SymmetricCipher,
                    IsModifiable = false,
                    Configuration = string.Empty,
                    Key = new CipherKeyEntity
                    {
                        Type = typeof(string).Name,
                        Length = 16
                    }
                },
                new CipherEntity
                {
                    Id = new Guid("5c3ba99c-bb77-4bd6-b171-1df79f129941"),
                    Name = "gost",
                    Type = CipherTypes.SymmetricCipher,
                    IsModifiable = false,
                    Configuration = string.Empty,
                    Key = new CipherKeyEntity
                    {
                        Type = typeof(string).Name,
                        Length = 32
                    }
                }
            };

            ciphers.Add(GetCustomCipher());

            return ciphers;
        }
        private CipherEntity GetCustomCipher()
        {
            var desConfiguration = new DESConfiguration
            {
                InitialPermutationTable = DESConfigurations.INITIAL_PERMUTATION_TABLE.Select(x => (int)x).ToArray(),
                FinalPermutationTable = DESConfigurations.FINAL_PERMUTATION_TABLE.Select(x => (int)x).ToArray(),
                ExpansionPermutationTable = DESConfigurations.EXPANSION_PERMUTATION_TABLE.Select(x => (int)x).ToArray(),
                SubstitutionBoxes = DESConfigurations.SUBSTITUTION_BOXES.Select(x => x.Select(y => (int)y).ToArray()).ToArray(),
                PermutationTable = DESConfigurations.PERMUTATION_TABLE.Select(x => (int)x).ToArray(),
                Pc1PermutationTable = DESConfigurations.PC1_PERMUTATION_TABLE.Select(x => (int)x).ToArray(),
                Pc2PermutationTable = DESConfigurations.PC2_PERMUTATION_TABLE.Select(x => (int)x).ToArray(),
                RotationsTable = DESConfigurations.ROTATIONS.Select(x => (int)x).ToArray()
            };

            var serializedConfiguration = JsonConvert.SerializeObject(desConfiguration);

            var cipher = new CipherEntity
            {
                Id = new Guid("ddc14bd0-a864-4895-addd-8ac11cc68b63"),
                Name = "des_template",
                Type = CipherTypes.SymmetricCipher,
                IsModifiable = false,
                Configuration = serializedConfiguration,
                Key = new CipherKeyEntity
                {
                    Type = typeof(string).Name,
                    Length = 8
                }
            };

            return cipher;
        }
    }
}