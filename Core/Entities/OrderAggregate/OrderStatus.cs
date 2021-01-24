using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate
{
    //This is going to track the status our order. These are just flags to say what state the order is at.
    public enum OrderStatus
    {
        /*
        So when we use an enum this is given a value of 0 for Pending, 1 for PaymentReceived and 2 for PaymentFailed.
        And that's what it's going to return to us by default.

        But we want to receive the text of this.
        So what we'll do we'll add EnumMember attribute here and give it a value.
         */
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "PaymentReceived")]
        PaymentReceived,

        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed
    }
}
