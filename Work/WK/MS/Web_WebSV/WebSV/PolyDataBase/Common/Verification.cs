using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace SLPolyGame.Web.Common
{
	public class Verification
	{
		static Regex reg = new Regex(@"^[a-zA-Z0-9_]+$");//英数，以及下划线
		//static Regex reg = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W).{6,30}$"); //英数，以及下划线
		static Regex reg_double = new Regex(@"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$");// 正浮点数
		static Regex reg_Name = new Regex(@"^[a-zA-Z0-9_\u4e00-\u9fa5]+$");//汉字，英数，以及下划线
		static Regex reg_num = new Regex(@"^-?\d+$");
		static Regex reg_Pwd = new Regex("[-\\da-zA-Z`=\\\\\\[\\];',./~!@#$%^&*()_+|{}:\"<>?]*");//6-20位的密码
        static Regex reg_strongPwd = new Regex(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9 !""#$%&'*+,\-.:;<=>?@^_`~\\|(){}[\]\/]){6,16}$");

        public static bool ValidateStrongPwd(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            bool hasSpaceStartOrEnd = value.Trim().Length != value.Length;

            if (hasSpaceStartOrEnd)
                return false;

            return reg_strongPwd.Match(value).Success;
        }

        public static bool ValidateInput(string value)
		{
			if (value == string.Empty)
				return true;
			return reg.Match(value).Success;
		}
		public static bool PwdInput(string value)
		{
			return reg_Pwd.Match(value).Success;
		}
		public static bool ValidateUserID(string value)
		{
			if (value == string.Empty)
				return true;
			return reg_Name.Match(value).Success;
		}
		public static bool ValidateInputModify(string value)
		{
			return reg.Match(value).Success;
		}
		public static bool ValidateUserIDModify(string value)
		{
			return reg_Name.Match(value).Success;
		}
		public static bool ValidateMoney(string value)
		{
			return reg_double.Match(value).Success;
		}
		public static bool ValidateBetMoney(string value)
		{
			return reg_double.Match(value).Success;
		}
		public static bool ValidateName(string value)
		{
			if (value == string.Empty)
				return true;
			return reg_Name.Match(value).Success;
		}
		public static bool IsValidEmail(string value)
		{
			return Regex.IsMatch(
			   value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)" +
			   @"|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
		}
		public static bool ValidateNameModify(string value)
		{
			return reg_Name.Match(value).Success;
		}
		public static bool ValidateNums(string value)
		{
			if (value == string.Empty)
				return true;
			return reg_num.Match(value).Success;
		}
		public static bool ValidateNumsModify(string value)
		{
			return reg_num.Match(value).Success;
		}
		public static bool IsZNumber(string inputData)
		{
			if (string.IsNullOrEmpty(inputData))
			{
				return true;
			}
			else
			{
				Regex rex = new Regex(@"^[0-9]\d*$");
				return rex.IsMatch(inputData);
			}
		}
		public static bool IsZNumberModify(string inputData)
		{
			if (string.IsNullOrEmpty(inputData))
			{
				return false;
			}
			else
			{
				Regex rex = new Regex(@"^[0-9]\d*$");
				return rex.IsMatch(inputData);
			}
		}
		public static bool IsZFloatModify(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			else
			{
				if (value.IndexOf(".") > -1)
				{
					return reg_double.IsMatch(value);
				}
				else
				{
					Regex rex = new Regex(@"^[0-9]\d*$");
					return rex.IsMatch(value);
				}

			}
		}

		/// <summary>
		/// 检验银行卡是否合法
		/// </summary>
		/// <param name="bankno"></param>
		/// <returns></returns>
		public static Boolean isValidBankCard(string bankno)
		{
			if (string.IsNullOrEmpty(bankno))
			{
				return false;
			}

			if (!IsZNumber(bankno))
			{
				return false;
			}

			if (bankno.Length > 30)
			{
				return false;
			}

			var lastNum = Convert.ToInt32(bankno.Substring(bankno.Length - 1, 1));//取出最后一位（与luhm进行比较）

			var first15Num = bankno.Substring(0, bankno.Length - 1);//前15或18位
			var newArr = new List<int>();
			for (var i = first15Num.Length - 1; i > -1; i--)
			{    //前15或18位倒序存进数组
				newArr.Add(Convert.ToInt32(first15Num.Substring(i, 1)));
			}

			var arrJiShu = new List<int>();  //奇数位*2的积 <9
			var arrJiShu2 = new List<int>(); //奇数位*2的积 >9

			var arrOuShu = new List<int>();  //偶数位数组
			for (var j = 0; j < newArr.Count; j++)
			{
				if ((j + 1) % 2 == 1)
				{//奇数位
					if (Convert.ToInt32(newArr[j]) * 2 < 9)
						arrJiShu.Add(newArr[j] * 2);
					else
						arrJiShu2.Add(newArr[j] * 2);
				}
				else //偶数位
					arrOuShu.Add(newArr[j]);
			}

			var jishu_child1 = new List<int>();//奇数位*2 >9 的分割之后的数组个位数
			var jishu_child2 = new List<int>();//奇数位*2 >9 的分割之后的数组十位数
			for (var h = 0; h < arrJiShu2.Count; h++)
			{
				jishu_child1.Add(arrJiShu2[h] % 10);
				jishu_child2.Add(arrJiShu2[h] / 10);
			}

			var sumJiShu = 0; //奇数位*2 < 9 的数组之和
			var sumOuShu = 0; //偶数位数组之和
			var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
			var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
			var sumTotal = 0;
			for (var m = 0; m < arrJiShu.Count; m++)
			{
				sumJiShu = sumJiShu + arrJiShu[m];
			}

			for (var n = 0; n < arrOuShu.Count; n++)
			{
				sumOuShu = sumOuShu + arrOuShu[n];
			}

			for (var p = 0; p < jishu_child1.Count; p++)
			{
				sumJiShuChild1 = sumJiShuChild1 + jishu_child1[p];
				sumJiShuChild2 = sumJiShuChild2 + jishu_child2[p];
			}
			//计算总和
			sumTotal = sumJiShu + sumOuShu + sumJiShuChild1 + sumJiShuChild2;

			//计算Luhm值
			var k = sumTotal % 10 == 0 ? 10 : sumTotal % 10;
			var luhm = 10 - k;

			if (lastNum == luhm)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 手機號碼格式驗證
		/// </summary>
		/// <param name="MobileNo"></param>
		/// <returns></returns>
		public static bool IsValidMobileNo(string MobileNo)
		{
			const string regPattern = @"^13[0-9]{9}|14[0-9]{9}|15[0-9]{9}|17[0-9]{9}|18[0-9]{9}$";
			return Regex.IsMatch(MobileNo, regPattern);
		}
	}
}