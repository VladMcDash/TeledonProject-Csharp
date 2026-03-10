namespace ProiectMPP.domain;

using System;

[Serializable]
public class Donation {
    public long Id { get; set; }
    public Donor Donor { get; set; }
    public CharityCase CharityCase { get; set; }
    public double Amount { get; set; }

    public Donation() { }

    public Donation(Donor donor, CharityCase charityCase, double amount) {
        Donor = donor;
        CharityCase = charityCase;
        Amount = amount;
    }
}