using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xTool.Models
{
    public class MainData
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TwoFactorAuthentication { get; set; }
        public string CardNumber { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string CardCode { get; set; }
        public string AdsAddress { get; set; }
        public string AdsName { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }

        public MainData(string inputData)
        {
            var tokens = inputData.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            this.UserName = tokens[0];
            this.Password = tokens[1];
            this.TwoFactorAuthentication = tokens[2];
            this.CardNumber = tokens[3];
            this.Month = tokens[4];
            this.Year = tokens[5];
            this.CardCode = tokens[6];
            this.AdsAddress = tokens[7];
            this.AdsName = tokens[8];
            this.Status = "Đang chờ";
            this.Color = "Black";
        }
    }
}
