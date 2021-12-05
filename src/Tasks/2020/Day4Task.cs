using System.Text.RegularExpressions;

namespace AdventCode.Tasks2020;

public class Day4Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 4;
    private readonly ILogger<Day4Task> _logger;
    #region TestData
    protected override string TestData => @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";
    #endregion

    public Day4Task(IAdventWebClient client, ILogger<Day4Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var passports = GeneratePassports(data);
        return passports.Count(x => x.IsValid()).ToString();
    }

    private static List<Passport> GeneratePassports(List<string> data)
    {
        var passports = new List<Passport>();
        var index = 0;
        do
        {
            var stringEntry = new List<string>();
            string entry;
            do
            {
                entry = data[index];
                if (string.IsNullOrEmpty(entry) == false)
                {
                    stringEntry.Add(entry);
                }
                index++;
            } while (string.IsNullOrEmpty(entry) == false && index < data.Count);
            passports.Add(new Passport(stringEntry));
        } while (index < data.Count);
        return passports;
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var passports = GeneratePassports(data);
        return passports.Count(x => x.IsValidValues()).ToString();
    }

    private class Passport
    {
        private string? BirthYear { get; set; }
        private string? IssueYear { get; set; }
        private string? ExpirationYear { get; set; }
        private string? Height { get; set; }
        private string? HairColor { get; set; }
        private string? EyeColor { get; set; }
        private string? PassportId { get; set; }
        private string? CountryId { get; set; }
        public Passport(List<string> data)
        {
            foreach (var lineEntry in data)
            {
                foreach (var code in lineEntry.Split(" "))
                {
                    ProcessField(code);
                }
            }
        }

        private void ProcessField(string code)
        {
            var keyValue = code.Split(":");
            switch (keyValue[0])
            {
                case "byr":
                    BirthYear = keyValue[1];
                    break;
                case "iyr":
                    IssueYear = keyValue[1];
                    break;
                case "eyr":
                    ExpirationYear = keyValue[1];
                    break;
                case "hgt":
                    Height = keyValue[1];
                    break;
                case "hcl":
                    HairColor = keyValue[1];
                    break;
                case "ecl":
                    EyeColor = keyValue[1];
                    break;
                case "pid":
                    PassportId = keyValue[1];
                    break;
                case "cid":
                    CountryId = keyValue[1];
                    break;
            }
        }

        public bool IsValid() => BirthYear != null && IssueYear != null && ExpirationYear != null && Height != null && HairColor != null && EyeColor != null && PassportId != null;

        public bool IsValidValues() => IsBirthYearValid() &&
            IsIssueYearValid() &&
            IsExpirationYearValid() &&
            IsHeightValid() &&
            IsHairColorValid() &&
            IsEyeColorValid() &&
            IsPassportIdValid();

        private bool IsPassportIdValid()
        {
            if (string.IsNullOrEmpty(PassportId)) return false;
            if (int.TryParse(PassportId, out _))
            {
                return PassportId.Length == 9;
            }
            return false;
        }

        private bool IsEyeColorValid()
        {
            if (string.IsNullOrEmpty(EyeColor)) return false;
            return EyeColor switch
            {
                "amb" or "blu" or "brn" or "gry" or "grn" or "hzl" or "oth" => true,
                _ => false,
            };
        }

        private bool IsHairColorValid()
        {
            if (string.IsNullOrEmpty(HairColor)) return false;
            return Regex.IsMatch(HairColor, @"#[a-f0-9][a-f0-9][a-f0-9][a-f0-9][a-f0-9][a-f0-9]");
        }

        private bool IsHeightValid()
        {
            if (string.IsNullOrEmpty(Height)) return false;
            if (Height.EndsWith("in"))
            {
                if (int.TryParse(Height.Replace("in", ""), out var heightIn))
                {
                    return heightIn >= 59 && heightIn <= 76;
                }
            }
            else if (Height.EndsWith("cm"))
            {
                if (int.TryParse(Height.Replace("cm", ""), out var heightCm))
                {
                    return heightCm >= 150 && heightCm <= 193;
                }
            }
            return false;
        }

        private bool IsExpirationYearValid()
        {
            if (string.IsNullOrEmpty(ExpirationYear) || ExpirationYear.Length != 4) return false;
            return int.TryParse(ExpirationYear, out var ey) && ey >= 2020 && ey <= 2030;
        }

        private bool IsIssueYearValid()
        {
            if (string.IsNullOrEmpty(IssueYear) || IssueYear.Length != 4) return false;
            return int.TryParse(IssueYear, out var iy) && iy >= 2010 && iy <= 2020;
        }

        private bool IsBirthYearValid()
        {
            if (string.IsNullOrEmpty(BirthYear) || BirthYear.Length != 4) return false;
            return int.TryParse(BirthYear, out var by) && by >= 1920 && by <= 2002;
        }
    }
}
