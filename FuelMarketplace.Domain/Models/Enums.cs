using System.Runtime.Serialization;

namespace FuelMarketplace.Domain.Models
{
    public enum Role
    {
        [EnumMember(Value = "user")]
        user,
        [EnumMember(Value = "moderator")]
        moderator,
        [EnumMember(Value = "administrator")]
        administrator
    }

    public enum FuelType
    {
        [EnumMember(Value = "coal")]
        coal,
        [EnumMember(Value = "charcoal")]
        charcoal,
        [EnumMember(Value = "peacoal")]
        peacoal,
        [EnumMember(Value = "wood")]
        wood
    }

    public enum Voivodeship
    {
        [EnumMember(Value = "Dolnośląskie")]
        dolnoslaskie,
        [EnumMember(Value = "Kujawsko-pomorskie")]
        kujawsko_pomorskie,
        [EnumMember(Value = "Lubelskie")]
        lubelskie,
        [EnumMember(Value = "Lubuskie")]
        lubuskie,
        [EnumMember(Value = "Łódzkie")]
        lodzkie,
        [EnumMember(Value = "Małopolskie")]
        malopolskie,
        [EnumMember(Value = "Mazowieckie")]
        mazowieckie,
        [EnumMember(Value = "Opolskie")]
        opolskie,
        [EnumMember(Value = "Podkarpackie")]
        podkarpackie,
        [EnumMember(Value = "Podlaskie")]
        podlaskie,
        [EnumMember(Value = "Pomorskie")]
        pomorskie,
        [EnumMember(Value = "Śląskie")]
        slaskie,
        [EnumMember(Value = "Świętokrzyskie")]
        swietokrzyskie,
        [EnumMember(Value = "Warmińsko-mazurskie")]
        warminsko_mazurskie,
        [EnumMember(Value = "Wielkopolskie")]
        wielkopolskie,
        [EnumMember(Value = "Zachodniopomorskie")]
        zachodniopomorskie,
        none
    }

    public enum ImageCategory
    {
        [EnumMember(Value = "user")]
        user,
        [EnumMember(Value = "post")]
        post,
        [EnumMember(Value = "offer")]
        offer,
        [EnumMember(Value = "comment")]
        comment,
        [EnumMember(Value = "salespoint")]
        salesPoint
    }
}
