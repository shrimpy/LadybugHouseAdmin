using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DonationPage.Models
{
    public enum AcknowledgementType
    {        
        [Display(Name = "General Donation")]
        GeneralDonation,

        [Display(Name = "In memory of")]
        InMemoryOf,

        [Display(Name = "In honor of")]
        InHonorOf
    }

    public class DonationEntry : TableEntity
    {
        [DisplayName("Donation ID")]
        public string DonationID
        {
            get
            {
                return RowKey;
            }
            set
            {
                RowKey = value;
            }
        }

        [DisplayName("Donation Amount")]
        public string Amount { get; set; }

        [DisplayName("Acknowledgement Type")]
        public AcknowledgementType AcknowledgementType { get; set; }

        [DisplayName("Honoree")]
        public string Honoree { get; set; }

        [DisplayName("Donation comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [DisplayName("Approved")]
        public bool Approved { get; set; }

        private const string AmountFieldName = "amount";
        private const string AcknowledgementTypeFieldName = "acknoweldgement_type";
        private const string HonoreeFieldName = "honoree";
        private const string CommentsFieldName = "comments";
        private const string ApprovedFieldName = "approved";

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            this.DonationID = RowKey;

            this.Amount = ValueOrDefault(properties, AmountFieldName)?.StringValue;
            //this.AcknowledgementType = properties[AcknowledgementTypeFieldName].PropertyAsObject; // TODO: parse into an enum
            this.Honoree = ValueOrDefault(properties, HonoreeFieldName)?.StringValue;
            this.Comments = ValueOrDefault(properties, CommentsFieldName)?.StringValue;

            var approved = ValueOrDefault(properties, ApprovedFieldName)?.BooleanValue.GetValueOrDefault();
            this.Approved = approved.GetValueOrDefault();
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var properties = new Dictionary<string, EntityProperty>();

            properties.Add(AmountFieldName, new EntityProperty(this.Amount));
            //properties.Add(AcknowledgementTypeFieldName, new EntityProperty(this.AcknowledgementType));
            properties.Add(HonoreeFieldName, new EntityProperty(this.Honoree));
            properties.Add(CommentsFieldName, new EntityProperty(this.Comments));
            properties.Add(ApprovedFieldName, new EntityProperty(this.Approved));

            return properties;
        }

        public static TValue ValueOrDefault<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue result;

            if (!dictionary.TryGetValue(key, out result)) {
                result = default(TValue);
            }

            return result;
        }
    }
}