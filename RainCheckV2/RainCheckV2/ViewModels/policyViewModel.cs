using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RainCheckV2.Models;

namespace RainCheckV2.ViewModels
{
    public class policyViewModel
    {
        private RainCheckServerEntities context = new RainCheckServerEntities();
        private decimal user_id;

        public policyViewModel(decimal user_id)
        {
            this.user_id = user_id;
        }

        public IEnumerable<policy_tbl> current_policy
        {
            get
            {
                return context.policy_tbl
                    .Where(o => o.user_id == user_id)
                    .GroupBy(o => o.policy_number)
                    .Select(o => o.OrderByDescending(t => t.start_date))
                    .FirstOrDefault();
            }
        }
        public IEnumerable<policy_tbl> policy_history
        {
            get
            {
                return context.policy_tbl
                    .Where(o => o.user_id == user_id);
            }
        }
        public policy_tbl BodyCoverageLevel
        {
            get
            {
                var coverage = context.policy_tbl.Where(o => o.user_id == user_id).OrderByDescending(o => o.start_date).First();
                return coverage;

            }
        }
    }
}