using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargaBd.API.Models
{
    public class Payload
    {
        [Key]
        public int Id { get; set; }
        public int? Order { get; set; }
        [MaxLength(150)]
        public string TrackingId { get; set; }
        [MaxLength(75)]
        public string Status { get; set; }
        [MaxLength(350)]
        public string Title { get; set; }
        [MaxLength(350)]
        public string Address { get; set; }
        [MaxLength(100)]
        public string Latitude { get; set; }
        [MaxLength(100)]
        public string Longitude { get; set; }
        public decimal Load { get; set; }
        public decimal Load2 { get; set; }
        public decimal Load3 { get; set; }
        [MaxLength(50)]
        public string WindowStart { get; set; }
        [MaxLength(50)]
        public string WindowEnd { get; set; }
        [MaxLength(50)]
        public string WindowStart2 { get; set; }
        [MaxLength(50)]
        public string WindowEnd2 { get; set; }
        [MaxLength(50)]
        public string Duration { get; set; }
        [MaxLength(350)]
        public string ContactName { get; set; }
        [MaxLength(25)]
        public string ContactPhone { get; set; }
        [MaxLength(100)]
        public string ContactEmail { get; set; }
        [MaxLength(500)]
        public string Reference { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        public List<SkillRequired> SkillsRequired { get; set; }
        public List<SkillOptional> SkillsOptional { get; set; }
        public List<Tag> Tags { get; set; }
        public string PlannedDate { get; set; }
        public string ProgrammedDate { get; set; }
        [MaxLength(90)]
        public string Route { get; set; }
        public string RouteEstimatedTimeStart { get; set; }
        public string EstimatedTimeArrival { get; set; }
        public string EstimatedTimeDeparture { get; set; }
        public string CheckinTime { get; set; }
        public string CheckoutTime { get; set; }
        public string CheckoutLatitude { get; set; }
        public string CheckoutLongitude { get; set; }
        public string CheckoutComment { get; set; }
        public string CheckoutObservation { get; set; }
        public string Signature { get; set; }
        public List<Picture> Pictures { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string EtaPredicted { get; set; }
        public string EtaCurrent { get; set; }
        public int? Driver { get; set; }
        public int Vehicle { get; set; }
        public bool Priority { get; set; }
        public bool Has_alert { get; set; }
        public int? PriorityLevel { get; set; }
        public string ExtraFieldValues { get; set; }
        public string GeocodeAlert { get; set; }
        public string VisitType { get; set; }
        public string CurrentEta { get; set; }
        public string Fleet { get; set; }
    }

    public class SkillRequired
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSkill { get; set; }
        [MaxLength(250)]
        public string NombreSkillRequired { get; set; }
    }

    public class SkillOptional
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSkill { get; set; }
        [MaxLength(300)]
        public string NombreSkillOptional { get; set; }
    }

    public class Tag
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTag { get; set; }
        [MaxLength(250)]
        public string NombreTag { get; set; }
    }

    public class Picture
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPicture { get; set; }
        [MaxLength(500)]
        public string UrlPicture { get; set; }
    }
}
