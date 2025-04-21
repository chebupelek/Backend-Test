using System.Runtime.Serialization;

namespace MCC.TestTask.Domain;

public enum Gender
{
    [EnumMember(Value = "Male")] Male,
    [EnumMember(Value = "Female")] Female
}