using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace MongoDBUI
{
    internal class Program
    {
        private static MongoDBDataAccess db;
        private static readonly string tableName = "Contactss";
        static void Main(string[] args)
        {
            db = new MongoDBDataAccess("MongoContactsDb", GetConnectionString());

            //ContactModel user = new ContactModel
            //{
            //    FirstName = "Amjed",
            //    LastName = "Bukhari"
            //};

            //user.EmailAddresses.Add(new EmailAddressModel
            //{ EmailAddress = "mrmohandtv@gmail.com" });
            //user.EmailAddresses.Add(new EmailAddressModel
            //{ EmailAddress = "lol@gmail.com" });
            //user.PhoneNumbers.Add(new PhoneNumberModel
            //{
            //    PhoneNumber = "555-4444"
            //});

            //CreateContact(user);

            //GetAllContacts();
            //0e095342-a63b-4cb3-9d29-3f9c738573b1

            //GetContactById("0e095342-a63b-4cb3-9d29-3f9c738573b1");

            //32cfe08e-a610-4bd4-8195-0994d5364ccb

            //UpdateContactsFirstName("Rabea", "32cfe08e-a610-4bd4-8195-0994d5364ccb");
            //GetAllContacts();

            //RemovePhoneNumberFromUser("555-5555", "32cfe08e-a610-4bd4-8195-0994d5364ccb");

            RemoveUser("32cfe08e-a610-4bd4-8195-0994d5364ccb");

            Console.WriteLine("Done processing Mongo DB");
            Console.ReadLine();
        }

        public static void RemoveUser(string id)
        {
            Guid guid = new Guid(id);
            db.DeleteRecord<ContactModel>(tableName, guid);
        }

        public static void RemovePhoneNumberFromUser(string phoneNumber, string id)
        {
            Guid guid = new Guid(id);
            var contact = db.LoadRecordById<ContactModel>(tableName, guid);

            contact.PhoneNumbers = contact.PhoneNumbers.Where(
                x => x.PhoneNumber != phoneNumber).ToList();

            db.UpsertRecord(tableName, contact.Id, contact);

        }
        private static void UpdateContactsFirstName(string firstName, string id)
        {
            Guid guid = new Guid(id);
            var contact = db.LoadRecordById<ContactModel>(tableName, guid);

            contact.FirstName = firstName;

            db.UpsertRecord(tableName, contact.Id, contact);
        }
        private static void GetContactById(string id)
        {
            Guid guid = new Guid(id);
            var contact = db.LoadRecordById<ContactModel>(tableName, guid);

            Console.WriteLine($"{ contact.Id }: { contact.FirstName } { contact.LastName}");
        }
        private static void GetAllContacts()
        {
            var contacts = db.LoadRecords<ContactModel>(tableName);

            foreach (var contact in contacts)
            {
                
                Console.WriteLine($"{ contact.Id }: { contact.FirstName } { contact.LastName}");
            }
        }
        private static void CreateContact(ContactModel contact)
        {
            db.UpsertRecord(tableName, contact.Id, contact);
        }
        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettinga.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
