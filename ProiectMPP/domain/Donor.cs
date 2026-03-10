namespace ProiectMPP.domain;
using System;

[Serializable]
public class Donor {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }

    public Donor() { }

    public Donor(string name, string address, string phoneNumber) {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
    }
}