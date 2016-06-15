using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DonationPage.Models
{
    public class DonationDbInitializer : DropCreateDatabaseIfModelChanges<DonationPageContext>
    {
        protected override void Seed(DonationPageContext context)
        {
            context.DonationEntries.Add(new DonationEntry { DonationID = Guid.NewGuid().ToString(), DonationTitle = "In memory" });

            base.Seed(context);
        }
    }
}