using System;
using NordicArenaDomainModels.Lang;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace NordicArenaDomainModels.Models
{
    /// <summary>
    /// Represents a criterion from which the contestant is scored 
    /// </summary>
    public class JudgingCriterion
    {
        public long Id { get; set; }
        [Required]
        public String Title { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Step { get; set; }
        [NotMapped]
        public int NumDecimals { get { return Step.GetDecimalCount(); } }

        // Nav properties
        [Required]
        public virtual Tournament Tournament { get; set; }
        public virtual ICollection<RunJudging> RunJudgings { get; set; }
    }
}