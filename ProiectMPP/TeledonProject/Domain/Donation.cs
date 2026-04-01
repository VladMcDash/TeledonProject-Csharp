using ProiectMPP.TeledonProject.Domain;

namespace ProiectMPP.TeledonProject.Domain;
using System;

[Serializable]
public class Donation : Entity<long>
{
    public Donor Donor { get; set; }
    public CharityCase CharityCase { get; set; }
    public double Amount { get; set; }

    public Donation() { }

    public Donation(Donor donor, CharityCase charityCase, double amount)
    {
        Donor = donor;
        CharityCase = charityCase;
        Amount = amount;
    }

    public override string ToString()
    {
        return $"Donation{{id={Id}, donor={Donor.Name}, case={CharityCase.Name}, amount={Amount}}}";
    }
}