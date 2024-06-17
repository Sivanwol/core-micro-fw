using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.GQL;


[Description("Contact Object (for providers and vendors only)")]
public class Contact
{
    [Description("Contact id")]
    public int Id { get; set; }
    [Description("Contact first name")]
    public string FirstName { get; set; }
    [Description("Contact last name")]
    public string LastName { get; set; }
    [Description("Contact email")]
    public string Email { get; set; }
    [Description("Contact phone (main)")]
    public string? Phone1 { get; set; }
    [Description("Contact phone (backup)")]
    public string? Phone2 { get; set; }
    [Description("Contact fax")]
    public string? Fax { get; set; }
    [Description("Contact whats up phone for contact")]
    public string? Whatsapp { get; set; }
    [Description("Contact country")]
    public Country? Country { get; set; }
    [Description("Contact postal code")]
    public string? PostalCode { get; set; }
    [Description("Contact address")]
    public string? Address { get; set; }
    [Description("Contact city")]
    public string? City { get; set; }
    [Description("Contact state (if in us)")]
    public string? State { get; set; }
    [Description("Contact job title")]
    public string JobTitle { get; set; }
    [Description("Contact department")]
    public string? Department { get; set; }
    [Description("Contact company")]
    public string Company { get; set; }
    [Description("Contact any notes")]
    public string Notes { get; set; }
}
