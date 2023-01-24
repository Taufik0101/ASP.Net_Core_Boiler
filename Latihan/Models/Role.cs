using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Latihan.Models;

public enum Role
{
    [EnumMember(Value = "super_admin")]
    SUPER_ADMIN,
    [EnumMember(Value = "admin")]
    ADMIN,
    [EnumMember(Value = "user")]
    USER
}