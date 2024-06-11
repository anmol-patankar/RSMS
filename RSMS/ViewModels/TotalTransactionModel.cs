using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RSMS.ViewModels
{
    public class TotalTransactionModel
    {
        public Domain.Models.Transaction Transaction { get; set; }
        public List<Domain.Models.TransactionDetail> TransactionDetailList { get; set; }

    }

}
