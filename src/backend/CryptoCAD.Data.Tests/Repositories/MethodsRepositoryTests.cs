using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using CryptoCAD.Data.Storage;
using CryptoCAD.Data.Repositories;
using CryptoCAD.Domain.Entities.Methods;

namespace CryptoCAD.Data.Tests.Repositories
{
    [TestFixture]
    public class MethodsRepositoryTests
    {
        [TestCase("storage.crptcd")]
        public void InitData(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, fileName);

            try
            {
                var context = new StorageContext(path);

                var repository = new MethodsRepository(context);

                var methods = repository.GetAll();

                Assert.AreEqual(5, methods.Count());

                var desEntity = repository.Get(new Guid("30870aee-f7ea-4f6d-aa60-fdfc48cc9a60"));
                Assert.AreEqual("des_library", desEntity.Name);
                Assert.AreEqual(MethodTypes.Cipher, desEntity.Type);
                Assert.IsFalse(desEntity.IsEditable);
                Assert.AreEqual(8, desEntity.SecretLength);

                var aesEntity = repository.Get(new Guid("8cb02965-ceab-4afb-bd70-3e4382f3ddae"));
                Assert.AreEqual("aes", aesEntity.Name);
                Assert.AreEqual(MethodTypes.Cipher, aesEntity.Type);
                Assert.IsFalse(aesEntity.IsModifiable);
                Assert.IsFalse(aesEntity.IsEditable);
                Assert.AreEqual(16, aesEntity.SecretLength);

                var gostEntity = repository.Get(new Guid("5c3ba99c-bb77-4bd6-b171-1df79f129941"));
                Assert.AreEqual("gost", gostEntity.Name);
                Assert.AreEqual(MethodTypes.Cipher, gostEntity.Type);
                Assert.IsFalse(gostEntity.IsModifiable);
                Assert.IsFalse(gostEntity.IsEditable);
                Assert.AreEqual(32, gostEntity.SecretLength);

                var sha156Entity = repository.Get(new Guid("4ea2b184-f6f2-406d-8f66-0a1949c84872"));
                Assert.AreEqual("sha256", sha156Entity.Name);
                Assert.AreEqual(MethodTypes.Hash, sha156Entity.Type);
                Assert.IsFalse(sha156Entity.IsModifiable);
                Assert.IsFalse(sha156Entity.IsEditable);

                var desTemplateEntity = repository.Get(new Guid("ddc14bd0-a864-4895-addd-8ac11cc68b63"));
                Assert.AreEqual("des", desTemplateEntity.Name);
                Assert.AreEqual(MethodTypes.Cipher, desTemplateEntity.Type);
                Assert.IsFalse(desTemplateEntity.IsModifiable);
                Assert.IsFalse(desTemplateEntity.IsEditable);
                Assert.AreEqual(8, desTemplateEntity.SecretLength);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [TestCase("storage.crptcd")]
        public void SaveChanges(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, fileName);

            try
            {
                var context = new StorageContext(path);
                var repository = new MethodsRepository(context);

                var ciphers = repository.GetAll();
                Assert.AreEqual(5, ciphers.Count());

                var desEntity = repository.Get(new Guid("30870aee-f7ea-4f6d-aa60-fdfc48cc9a60"));
                repository.Remove(desEntity);
                repository.SaveChanges();
                ciphers = repository.GetAll();
                Assert.AreEqual(4, ciphers.Count());

                context = new StorageContext(path);
                repository = new MethodsRepository(context);
                ciphers = repository.GetAll();
                Assert.AreEqual(4, ciphers.Count());
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [TestCase("storage.crptcd")]
        public void DropCreate(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            var path = Path.Combine(directory, fileName);

            try
            {
                var context = new StorageContext(path);
                var repository = new MethodsRepository(context);

                var ciphers = repository.GetAll();
                Assert.AreEqual(5, ciphers.Count());

                var desEntity = repository.Get(new Guid("30870aee-f7ea-4f6d-aa60-fdfc48cc9a60"));
                repository.Remove(desEntity);
                repository.SaveChanges();
                ciphers = repository.GetAll();
                Assert.AreEqual(4, ciphers.Count());

                context = new StorageContext(path, true);
                repository = new MethodsRepository(context);
                ciphers = repository.GetAll();
                Assert.AreEqual(5, ciphers.Count());
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}