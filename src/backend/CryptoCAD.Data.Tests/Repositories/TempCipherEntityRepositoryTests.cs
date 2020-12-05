using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using CryptoCAD.Domain.Entities;
using CryptoCAD.Data.Repositories;

namespace CryptoCAD.Data.Tests.Repositories
{
    [TestFixture]
    public class TempCipherEntityRepositoryTests
    {
        [TestCase("storage.json")]
        public void InitData(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, fileName);

            var repository = new TempCipherEntityRepository(path);

            var ciphers = repository.GetAll();

            Assert.AreEqual(ciphers.Count(), 4);

            var desEntity = repository.Get(new Guid("30870aee-f7ea-4f6d-aa60-fdfc48cc9a60"));
            Assert.AreEqual(desEntity.Name, "des");
            Assert.AreEqual(desEntity.Type, CipherTypes.SymmetricCipher);
            Assert.IsFalse(desEntity.IsModifiable);
            Assert.AreEqual(desEntity.Key.Length, 8);

            var aesEntity = repository.Get(new Guid("8cb02965-ceab-4afb-bd70-3e4382f3ddae"));
            Assert.AreEqual(aesEntity.Name, "aes");
            Assert.AreEqual(aesEntity.Type, CipherTypes.SymmetricCipher);
            Assert.IsFalse(aesEntity.IsModifiable);
            Assert.AreEqual(aesEntity.Key.Length, 16);

            var gostEntity = repository.Get(new Guid("5c3ba99c-bb77-4bd6-b171-1df79f129941"));
            Assert.AreEqual(gostEntity.Name, "gost");
            Assert.AreEqual(gostEntity.Type, CipherTypes.SymmetricCipher);
            Assert.IsFalse(gostEntity.IsModifiable);
            Assert.AreEqual(gostEntity.Key.Length, 32);

            var desTemplateEntity = repository.Get(new Guid("ddc14bd0-a864-4895-addd-8ac11cc68b63"));
            Assert.AreEqual(desTemplateEntity.Name, "des_template");
            Assert.AreEqual(desTemplateEntity.Type, CipherTypes.SymmetricCipher);
            Assert.IsFalse(desTemplateEntity.IsModifiable);
            Assert.AreEqual(desTemplateEntity.Key.Length, 8);

            File.Delete(path);
        }

        [TestCase("storage.json")]
        public void SaveChanges(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, fileName);

            var repository = new TempCipherEntityRepository(path);

            var ciphers = repository.GetAll();
            Assert.AreEqual(ciphers.Count(), 4);

            var desEntity = repository.Get(new Guid("30870aee-f7ea-4f6d-aa60-fdfc48cc9a60"));
            repository.Remove(desEntity);
            repository.SaveChanges();
            ciphers = repository.GetAll();
            Assert.AreEqual(ciphers.Count(), 3);

            repository = new TempCipherEntityRepository(path);
            ciphers = repository.GetAll();
            Assert.AreEqual(ciphers.Count(), 3);

            File.Delete(path);
        }

        [TestCase("storage.json")]
        public void DropCreate(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, fileName);

            var repository = new TempCipherEntityRepository(path);

            var ciphers = repository.GetAll();
            Assert.AreEqual(ciphers.Count(), 4);

            var desEntity = repository.Get(new Guid("30870aee-f7ea-4f6d-aa60-fdfc48cc9a60"));
            repository.Remove(desEntity);
            repository.SaveChanges();
            ciphers = repository.GetAll();
            Assert.AreEqual(ciphers.Count(), 3);

            repository = new TempCipherEntityRepository(path, true);
            ciphers = repository.GetAll();
            Assert.AreEqual(ciphers.Count(), 4);

            File.Delete(path);
        }
    }
}