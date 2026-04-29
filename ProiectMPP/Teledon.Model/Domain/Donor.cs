namespace ProiectMPP.TeledonProject.Domain;
using System;

[Serializable]
public class Donor : Entity<long>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }

    public Donor() { }

    public Donor(string name, string address, string phoneNumber)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
    }

    public override string ToString()
    {
        return $"Donor{{id={Id}, name='{Name}', address='{Address}', phone='{PhoneNumber}'}}";
    }
}