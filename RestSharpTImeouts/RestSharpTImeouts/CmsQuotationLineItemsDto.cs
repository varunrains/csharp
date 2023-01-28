using System;
using System.Collections.Generic;
using System.Text;

namespace RestSharpTImeouts
{
    public interface IMember
    {
        string IdentifiedServiceCode { get; set; }
        IdentifiedServiceQuotationLine IdentifiedServiceQuotationLine { get; set; }
        QuotedTurnAroundTime QuotedTurnAroundTime { get; set; }
    }

    public interface IQuotedItemPriceDetails
    {
        int Quantity { get; set; }
        QuotedPrice QuotedPrice { get; set; }
    }

    public class QuotationLineItemsDto
    {
        public List<Item> Items { get; set; }
        public string QuotationCode { get; set; }
        public int QuotationVersionNumber { get; set; }
    }
    public class Item
    {
        public QuotedItem QuotedItem { get; set; }
        public CommercialItemSubGroup CommercialItemSubGroup { get; set; }
    }

    public class IdentifiedServiceQuotationLine
    {
        public int IdentifiedServiceType { get; set; }
        public string InterpretationValue { get; set; }
        public string PerformerCode { get; set; }
        public int PerformerType { get; set; }
        public string QuotationLineComment { get; set; }
        public string QuotationLineLabel { get; set; }
    }

    public class QuotedPrice
    {
        public double Amount { get; set; }
        public string CurrencyIsoCode { get; set; }
    }

    public class CustomerPackageMember : IMember
    {
        public string IdentifiedServiceCode { get; set; }
        public IdentifiedServiceQuotationLine IdentifiedServiceQuotationLine { get; set; }
        public int RepetitionNumber { get; set; }
        public QuotedTurnAroundTime QuotedTurnAroundTime { get; set; }
    }

    public class CustomerPackage
    {
        public List<object> AvailableCustomizedSampleFactions { get; set; }
        public List<CustomerPackageMember> CustomerPackageMembers { get; set; }
        public string Name { get; set; }
    }

    public class QuotedItem : IMember, IQuotedItemPriceDetails
    {
        public string Code { get; set; }
        public IdentifiedServiceQuotationLine IdentifiedServiceQuotationLine { get; set; }
        public int PredefinedSpecialCondition { get; set; }
        public int Quantity { get; set; }
        public QuotedPrice QuotedPrice { get; set; }
        public CustomerPackage CustomerPackage { get; set; }
        public string IdentifiedServiceCode { get { return this.Code; } set { this.Code = value; } }
        public QuotedTurnAroundTime QuotedTurnAroundTime { get; set; }
    }

    public class QuotedTurnAroundTime
    {
        public string Duration { get; set; }
        public int PreferedDisplayUnit { get; set; }
    }

    public class CommercialItemSubGroupMember : IMember, IQuotedItemPriceDetails
    {
        public string IdentifiedServiceCode { get; set; }
        public IdentifiedServiceQuotationLine IdentifiedServiceQuotationLine { get; set; }
        public int PredefinedSpecialCondition { get; set; }
        public int Quantity { get; set; }
        public QuotedPrice QuotedPrice { get; set; }
        public QuotedTurnAroundTime QuotedTurnAroundTime { get; set; }
    }

    public class CommercialItemSubGroup
    {
        public string Code { get; set; }
        public List<CommercialItemSubGroupMember> CommercialItemSubGroupMembers { get; set; }
        public string Name { get; set; }
    }

}
