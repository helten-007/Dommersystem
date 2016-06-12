using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace NordicArenaDomainModels.Models
{
    /// <summary>
    /// Represents a judge of the tournament
    /// </summary>
    public class Judge
    {
        public long Id { get; set; } 
        [Required]
        public String Name { get; set; }
        public String LoginCode { get; private set;  }
		public bool IsHeadJudge { get; set; }

        // Navigation / ID's
        [Required]
        public long TournamentId { get; set; }
        [JsonIgnore]
        public virtual Tournament Tournament { get; set; }
        [JsonIgnore]
        public virtual ICollection<RunJudging> RunJudgings {get; set;}

        public Judge()
        {
        }

        public Judge(String name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Initializing lists. Not doing this in constructor because EF creates proxied lists. Only use when adding new elements or unit testing
        /// </summary>
        public void EnsureListsAreInitialized()
        {
            if (RunJudgings == null) RunJudgings = new List<RunJudging>();
        }

        /// <summary>
        /// Generates a 5-character login code used as authentication when logging in from device later. Code is
        /// placed in property LoginCode.
        /// </summary>
        public void GenerateLoginCode()
        {
            var sb = new StringBuilder();
            var rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                String s = Encoding.ASCII.GetString(new byte[]{(byte)rand.Next(97,97+26)}, 0, 1); // a-z character generation
                sb.Append(s);
            }
            LoginCode = sb.ToString();
        }
    }
}