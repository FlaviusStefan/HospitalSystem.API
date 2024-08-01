namespace HospitalSystem.API.Models.Domain
{
    public class Hospital
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public ICollection<HospitalAffiliation> HospitalAffiliations { get; set; }
    }
}
