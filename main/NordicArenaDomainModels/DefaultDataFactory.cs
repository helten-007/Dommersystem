using System;
using System.Collections.Generic;
using NordicArenaDomainModels.Lang;
using NordicArenaDomainModels.Models;

namespace NordicArenaDomainModels.Common
{
    /// <summary>
    /// Class which generates initial tournament data based on specifications
    /// </summary>
    public class DefaultDataFactory
    {
        public static HashSet<JudgingCriterion> GetJudgingCriteria()
        {
            String[] names = new String[] { "Trick", "Teknisk", "Stil", "Variasjon", "Fart" };
            var set = new HashSet<JudgingCriterion>();
            var template = new JudgingCriterion()
            {
                Min = 0M,
                Max = 10M,
                Step = 0.1M
            };
            for (int i = 0; i < names.Length; i++)
            {
                var criterion = template.GetShallowCloneByReflection<JudgingCriterion>();
                criterion.Title = names[i];
                set.Add(criterion);
            }
            return set;
        }
    }
}