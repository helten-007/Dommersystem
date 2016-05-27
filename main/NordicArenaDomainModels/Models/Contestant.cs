using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NordicArenaDomainModels.Models
{
    /// <summary>
    /// Represents a contestant in the tournament
    /// </summary>
    public class Contestant
    {
        public long Id { get; set; }
        [Required]
        public String Name { get; set; }
        public String Sponsors { get; set; }
        public String Location { get; set; }
        [DataType(DataType.Date)]
        // http://stackoverflow.com/questions/18546971/mvc-4-how-to-validate-a-non-us-date-with-client-validation
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Set to true if player is removed, DQ'ed, DNS or similar
        /// </summary>
        public bool IsRemoved { get; set; }
        [Required]
        [JsonIgnore]
        public virtual Tournament Tournament { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoundContestant> RoundParticipations { get; set; }

        /// <summary>
        /// Viewmodel property. Todo: Refactor in own viewmodel class
        /// </summary>
        public String AgeYears
        {
            get
            {
                if (DateOfBirth == null) return String.Empty;
                int years = DateTime.Now.Year - DateOfBirth.Value.Year;
                var birthDay = new DateTime(DateTime.Now.Year, DateOfBirth.Value.Month, DateOfBirth.Value.Day);
                if (birthDay > DateTime.Now) years--;
                return "" + years;
            }
        }

        public Contestant()
        {
        }

        public Contestant(String name) : this()
        {
            Name = name;
        }

        internal void EnsureListsAreInitialized()
        {
            if (RoundParticipations == null) RoundParticipations = new HashSet<RoundContestant>();
        }

        public override string ToString()
        {
            return String.Format("{0}/{1}", Id, Name);
        }
    }
}