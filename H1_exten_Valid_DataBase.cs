//Custom Validation for phone
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace Services
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneValidation : ValidationAttribute
    {
        public sealed override bool IsValid(object? value)
        {
            if (value is string phoneNumber)
            {
                string pattern = @"^09[0-9]{9}$";
                return !string.IsNullOrEmpty(phoneNumber) && phoneNumber.StartsWith("09") && Regex.IsMatch(phoneNumber, pattern);
            }

            return false;
        }
    }
}

/////////////////////////////////


//extension method for DateTime to PersianCalender
using System.Globalization;

namespace Services
{
    public static class PersianCalender
    {
        public static string ToPersianDate(this DateTime greDate)
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", pc.GetYear(greDate), pc.GetMonth(greDate), pc.GetDayOfMonth(greDate));
        }
    }
}


//////////////////////////
///

//DataBase methods and for save file and generate Id
public static class DataBase
{


    #region Fields
    private static readonly string _membersFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\FileName";

    public static List<Member> Members { get; set; } = new List<Member>();
    #endregion

    #region Methods
    public static List<Member> GetMembers() => Members;

    public static bool SaveMember(Member model)
    {
        if (NationalCodeIsOk(model.NationalCode))
        {
            model.Id = GenerateMemberId();
            var d = DateTime.Now;
            model.RegisterDate = d.ToPersianDate();
            Members.Add(model);
            SaveMembers<Member>(Members);
            return true;
        }
        return false;
    }

    public static void SaveMembers<T>(List<T> MemberList)
    {
        var jsonFile = JsonConvert.SerializeObject(MemberList);
        File.WriteAllText(_membersFilePath, jsonFile);
    }

    public static List<T> LoadMembers<T>()
    {
        try
        {
            var data = File.ReadAllText(_membersFilePath);
            return JsonConvert.DeserializeObject<List<T>>(data);
        }
        catch (FileNotFoundException)
        {
            File.Create(_membersFilePath).Close();
            File.WriteAllText(_membersFilePath, "[]");
            var data = File.ReadAllText(_membersFilePath);
            return JsonConvert.DeserializeObject<List<T>>(data);
        }
    }
    #endregion

    #region PrivateMethods
    private static int GenerateMemberId()
    {
        Members = LoadMembers<Member>();

        if (Members != null)
        {
            var lastMemberId = Members.Max(x => x.Id) + 1;
            return lastMemberId;
        }
        else
        {
            return 1;
        }
    }

    private static bool NationalCodeIsOk(string nationalCode)
    {
        Members = LoadMembers<Member>();

        if (Members != null)
        {
            var isAvailable = Members.Select(x => x.NationalCode).Any(y => y.Equals(nationalCode));

            if (!isAvailable)
            {
                return true;
            }
            return false;
        }
        return false;
    }
    #endregion
}