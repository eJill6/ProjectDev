using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.PasswordRule
{
    public class PasswordRuleTypes : BaseIntValueModel<PasswordRuleTypes>
    {
        public string RegExp { get; private set; }

        public int Score { get; private set; }

        public static PasswordRuleTypes LengthOneUntilNine = new PasswordRuleTypes()
        {
            Value = 0,
            RegExp = "^.{1,9}$",
            Score = 0
        };
        public static PasswordRuleTypes LengthTenUntilTwelve = new PasswordRuleTypes()
        {
            Value = 1,
            RegExp = "^.{10,12}$",
            Score = 10
        };
        public static PasswordRuleTypes LengthThirteenUntilFourteen = new PasswordRuleTypes()
        {
            Value = 2,
            RegExp = "^.{13,14}$",
            Score = 15
        };
        public static PasswordRuleTypes LengthFifteenUntilSixteen = new PasswordRuleTypes()
        {
            Value = 3,
            RegExp = "^.{15,16}$",
            Score = 20
        };
        public static PasswordRuleTypes MustOneLow = new PasswordRuleTypes()
        {
            Value = 4,
            RegExp = "^(?=.*[a-z]).+",
            Score = 5
        };
        public static PasswordRuleTypes MustOneUp = new PasswordRuleTypes()
        {
            Value = 5,
            RegExp = "^(?=.*[A-Z]).+",
            Score = 5
        };
        public static PasswordRuleTypes MustOneNumber = new PasswordRuleTypes()
        {
            Value = 6,
            RegExp = "^(?=.*[0-9]).+",
            Score = 5
        };
        public static PasswordRuleTypes MustOneSpecial = new PasswordRuleTypes()
        {
            Value = 7,
            RegExp = @"^(?=.*[@#$%&*+\-_(),+':;?.,!\[\]\s\\/]).+",
            Score = 3
        };

        public static PasswordRuleTypes MixLengthLetterNumber = new PasswordRuleTypes()
        {
            Value = 8,
            RegExp = "^(?=.*[a-z]+)(?=.*[A-Z]+)(?=.*[0-9]+).{10,}$",
            Score = 10
        };

        public static List<StrengthRuleInfo> GetAllList()
        {
            return GetAll().Select(x => new StrengthRuleInfo() {
                 RegExp = x.RegExp,
                 Score = x.Score
            }).ToList();
        }
    }

    public class StrengthScoreIntervals : BaseIntValueModel<StrengthScoreIntervals>
    {
        public int MinScore { get; private set; }
        public int MaxScore { get; private set; }

        public static StrengthScoreIntervals VeryWeak = new StrengthScoreIntervals()
        {
            Value = 1,
            MinScore = 15,
            MaxScore = 22,
            ResourcePropertyName = nameof(CommonElement.PasswordVeryWeak),
            ResourceType = typeof(CommonElement)
        };

        public static StrengthScoreIntervals Weak = new StrengthScoreIntervals()
        {
            Value = 2,
            MinScore = 23,
            MaxScore = 34, 
            ResourcePropertyName = nameof(CommonElement.PasswordWeak),
            ResourceType = typeof(CommonElement)
        };

        public static StrengthScoreIntervals Normal = new StrengthScoreIntervals()
        {
            Value = 3,
            MinScore = 35,
            MaxScore = 38, 
            ResourcePropertyName = nameof(CommonElement.PasswordNormal),
            ResourceType = typeof(CommonElement)
        };

        public static StrengthScoreIntervals Strong = new StrengthScoreIntervals()
        {
            Value = 4,
            MinScore = 39,
            MaxScore = 44, 
            ResourcePropertyName = nameof(CommonElement.PasswordStrong),
            ResourceType = typeof(CommonElement)
        };

        public static StrengthScoreIntervals VeryStrong = new StrengthScoreIntervals()
        {
            Value = 5,
            MinScore = 45,
            MaxScore = 999, 
            ResourcePropertyName = nameof(CommonElement.PasswordVeryStrong),
            ResourceType = typeof(CommonElement)
        };

        public static List<StrengthScoreInterval> GetAllList()
        {
            return GetAll().Select(x => new StrengthScoreInterval()
            {
                IntervalName = x.Name,
                MaxScore = x.MaxScore,
                MinScore = x.MinScore
            }).ToList();
        }
    }

}
