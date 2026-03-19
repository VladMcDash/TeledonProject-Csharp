namespace ProiectMPP.TeledonProject.domain;
using System;

[Serializable]
public class Volunteer : Entity<long>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }

    public Volunteer() { }

    public Volunteer(string username, string password, string name)
    {
        Username = username;
        Password = password;
        Name = name;
    }

    public override string ToString()
    {
        return $"Volunteer{{id={Id}, username='{Username}', name='{Name}'}}";
    }
}